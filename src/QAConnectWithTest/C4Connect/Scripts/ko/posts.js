var fileApiUrl = '/api/File/', postApiUrl = '/api/Post/', headerApiUrl = '/api/Header/';
function getTimeAgo(varDate) {
    if (varDate && new Date(varDate) != 'Invalid Date') {
        var hour = new Date(varDate).getHours();// Math.ceil((new Date() - new Date(varDate)) / 36e5);
        if (hour < 10)
            return $.timeago(varDate.toString().slice(-1) == 'Z' ? varDate : varDate + 'Z');
        else
            return $D(varDate).strftime("%b %d, %Y at %I:%m%p");
    }
    else {
        return new Date();
    }
}


// Model
function Post(data, hub) {

    var self = this;
    data = data || {};
    self.PostId = data.PostId;
    self.Message = ko.observable(data.Message || "");
    self.PostedBy = data.PostedBy || "";
    self.UserName = data.UserName;
    self.PostedByName = data.PostedByName || "";
    self.PostedByAvatar = data.PostedByAvatar || "";
    self.PostedDate = getTimeAgo(data.PostedDate);
    self.Privacy = data.Privacy;
    self.Link = data.Link,
    self.LinkIcon = data.LinkIcon,
    self.LinkHeader = data.LinkHeader,
    self.LinkDescription = data.LinkDescription,
    self.PostDescription = data.PostDescription,
    self.PostType = data.PostType,
    self.Active = ko.observable(data.Active || true),
    self.error = ko.observable();
    self.PostComments = ko.observableArray();
    self.NewComments = ko.observableArray();
    self.newCommentMessage = ko.observable();
    self.broadCastType = 1;
    self.TotalLikes = ko.observable(data.TotalLikes);
    self.Liked = ko.observable(data.Liked);

    self.hub = hub;

    self.authorizeMe = function () {
        alert("You need to login-first");
    }

    self.reportPost = function () {
        var post = new Post(this);
        return $.myAjax({
            url: postApiUrl + '/ReportPost/',
            data: ko.toJSON(post),
            type: 'POST',
            success: function (status) {
                self.Active(false);
            }
        })
    }

    self.hidePost = function () {
        var post = new Post(this);
        return $.myAjax({
            url: postApiUrl + '/HidePostRemoved/',
            data: ko.toJSON(post),
            type: 'POST',
            success: function (status) {
                self.Active(false);
            }
        })
    }

    self.deletePost = function () {
        var post = new Post(this);
        post.PostComments([]);
        return $.myAjax({
            url: postApiUrl + '/DeletePost/',
            data: ko.toJSON(post),
            type: 'DELETE',
            success: function (status) {
                vmPost.posts.remove(function (item) {
                    return item.PostId == post.PostId;
                });
            }
        })
    }

    self.addComment = function () {
        var me = this;
        var comment = new Comment(me);
        comment.Message = self.newCommentMessage();
        return $.myAjax({
            url: postApiUrl + '/AddComment/',
            data: ko.toJSON(comment),
            type: 'POST',
            success: function (data) {
                self.PostComments.push(new Comment(data));
                self.newCommentMessage('');
            }
        });
    }

    self.deleteComment = function () {
        var me = this;
        var comment = new Comment(me);
        return $.ajax({
            url: postApiUrl + comment.CommentId,
            dataType: "json",
            contentType: "application/json",
            cache: false,
            type: 'DELETE'
        })
       .done(function (result) {
           self.PostComments.remove(me);
       })
       .fail(function () {
           self.error('unable to delete comment');
       });
    }

    self.toggleComment = function (item, event) {
        $(event.target).closest('.postContent').find('.publishComment').toggle();
    }

    self.like = function () {
        var me = this;
        var like = new Like(me);
        return $.myAjax({
            url: postApiUrl + '/Like/',
            data: ko.toJSON(like),
            type: 'POST',
            success: function (TotalLikes) {
                self.Liked(true);
                self.TotalLikes(TotalLikes);
            }
        });
    };

    self.unlike = function (item, event) {
        var me = this;
        var like = new Like(me);
        return $.myAjax({
            url: postApiUrl + '/UnLike/',
            data: ko.toJSON(like),
            type: 'POST',
            success: function (TotalLikes) {
                self.Liked(false);
                self.TotalLikes(TotalLikes);
            }
        });
    };

    self.loadPostModal = function (item, event) {
        var me = this;
        var post = new Post(me);
        return $.ajax({
            url: postApiUrl + post.PostId,
            dataType: "json",
            contentType: "application/json",
            cache: false,
            type: 'GET',
        })
       .done(function (result) {
           $('.loadPost').show();
       })
       .fail(function () {
           self.error('unable to load post');
       });
    }

    self.loadSlide = function (Link, PostType, Title, LinkIcon) {
        vmPost.loadSlide(Link, PostType, Title, LinkIcon);
    }

    if (data.PostComments) {
        var mappedPosts = $.map(data.PostComments, function (item) { return new Comment(item); });
        self.PostComments(mappedPosts);
    }

}

function Comment(data) {
    var self = this;
    data = data || {};
    // Persisted properties
    self.CommentId = data.CommentId;
    self.PostId = data.PostId;
    self.Message = ko.observable(data.Message || "");
    self.CommentedBy = data.UserID || "";
    self.CommentedByAvatar = data.PostedByAvatar || "";
    self.CommentedByName = data.PostedByName || "";
    self.CommentedDate = getTimeAgo(data.CreatedOn);
    self.TotalLikes = ko.observable(data.TotalLikes);
    self.Liked = ko.observable(data.Liked);
    self.broadCastType = 1;
    self.error = ko.observable();
}

function Like(data) {
    var self = this;
    data = data || {};

    // Persisted properties
    if ($(data.PostId)) {
        self.ItemID = data.PostId;
        self.ItemLevel = 1;
    }
    else {
        self.ItemID = data.CommentId;
        self.ItemLevel = 2;
    }
    self.UserID = data.PostedBy || "";
    self.broadCastType = 1;
    self.error = ko.observable();
    //persist edits to real values on accept
}

function VM_Post() {
    var self = this;

    self.authorizeMe = function () {
        alert("You need to login-first");
    }

    self.loadSlide = function (Link, PostType, Title, LinkIcon) {
        $('.popup-modal').modal('show');
        var me = this;
        var data = '<div class="modal-content"><div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>' +
        '<h4 class="modal-title" >' + Title + '</h4></div><div class="modal-body">';
        switch (PostType) {
            case 2:
                {
                    data += '<img src="' + Link + '" alt="Image" />';
                }
                break;
            case 5:
                {
                    data += '<img src="' + LinkIcon + '" alt="Image" />';
                    data += '<audio autoplay src="' + Link + '" data-bind="player:me" type="audio/mp3"></audio>';
                }
                break;
            case 6:
                {
                    data += '<video autoplay src="' + Link + '" data-bind="player:me" type="video/mp4" controls="controls"></video>';
                }
                break;
        }
        data += '    </div></div>';
        $('.popup-modal .modal-dialog').html(data);

        ko.applyBindings(vmPost, $('.popup-modal .modal-dialog .modal-content').get()[0]);
    }

    self.posts = ko.observableArray();
    self.newMessage = ko.observable();
    self.newLink = ko.observable();
    self.newLinkIcon = ko.observable();
    self.newLinkHeader = ko.observable();
    self.newLinkDescription = ko.observable();
    self.uploadLink = function (url) {
        $.getJSON("//query.yahooapis.com/v1/public/yql?"
                  + "q=SELECT%20*%20FROM%20html%20WHERE%20url=%27"
                  + encodeURIComponent(url)
                  + "%27%20AND%20xpath=%27descendant-or-self::meta%27"
                  + "&format=json&callback=?"
          , function (data) {
              if (data.query.results != null) {
                  self.newLink(url);
                  var res = $.grep(data.query.results.meta, function (o, key) {
                      return (o.hasOwnProperty("property") || o.hasOwnProperty("name")) && (o.property === "og:image" || o.property === "og:title" || (o.property === "og:description" || o.name === "Description"))
                  });
                  // if object having property specified returned , do stuff
                  if (res.length > 0) {
                      self.newLinkIcon(res[0].content);
                      self.newLinkHeader(res[1].content);
                      self.newLinkDescription(res[2].content);
                  }
              }
              else
                  self.error('Url seems to be not accessible !')
          });
    }
    self.postType = ko.observable("1");
    self.PostDescription = ko.observable('updated a status'),
    self.updateType = function (val, desc) {
        self.postType(val);
        self.PostDescription(desc);
    };
    self.postPrivacy = ko.observable('');
    self.pendingRequest = ko.observable(true);
    self.page = ko.observable(1);
    self.error = ko.observable();
    self.last = ko.observable();
    //SignalR related
    self.newPosts = ko.observableArray();
    // Reference the proxy for the hub.  
    self.hub = $.connection.postHub;

    self.init = function () {
        self.error(null);
        $.myAjax({
            url: postApiUrl + 'GetPosts/',
            data: { 'PageNumber': 1, 'PageSize': 25, 'PageType': top.PageType, 'PageID': top.PageID, 'IsHome': top.Home ? top.Home : false },
            showProgress: true,
            success: function (data) {
                self.loadPosts(data);
            }
        });
    }

    self.onDisconnect = function (event) {

        self.hub.server.onDisconnected().done(function () {

        }).fail(function (err) {
            self.error(err);
        });
    }

    self.loadPosts = function (data) {
        if (!data.length) {
            self.last(1);
            return;
        }
        var mappedPosts = $.map(data, function (item) { return new Post(item, self.hub); });

        if (self.page() > 1) {
            self.posts(self.posts().concat(mappedPosts));
        }
        else {
            self.posts(mappedPosts);
        }
        self.page(self.page() + 1);
        self.pendingRequest(false);
    }

    // Call this function on upload button click after user has selected the file 
    self.uploadFile = function (file) {
        var fileInfo = checkFile(file, self.postType());
        if (fileInfo != true) {
            vmMessage.newMessage({ 'Title': 'Fix following errors:-', 'Type': MessageType.Error, 'Message': fileInfo });
            return;
        }

        var formData = new FormData();
        formData.append('fileData', file);
        $.myAjax({
            url: fileApiUrl + 'PostFile/',
            contentType: false,
            cache: false,
            type: 'POST',
            processData: false,
            data: formData,
            showProgress: true,
            success: function (data) {
                var arr = data.split('#');
                self.newLink(arr[0]);
                self.newLinkIcon(arr[1]);
            },
        });
    }

    //Add post to wall
    self.addPost = function (readyPost, data) {
        self.postPrivacy($('.privacy option:selected').val());
        var post = new Post();
        if (readyPost && readyPost.Message)
            post = readyPost;
        else {
            post.Message = self.newMessage();
            post.Privacy = self.postPrivacy();
            post.PostDescription = self.PostDescription();
            post.Link = self.newLink();
            post.LinkIcon = self.newLinkIcon();
            post.LinkHeader = self.newLinkHeader();
            post.LinkDescription = self.newLinkDescription();
            post.PostType = self.postType();
            post.broadCastType = 1;
        }
        var postInfo = checkPost(post);
        if (postInfo != true) {
            vmMessage.newMessage({ 'Title': 'Fix following error(\'s):-', 'Type': MessageType.Error, 'Message': postInfo });
            return;
        }
        var $thisButton = $(data.target);
        $thisButton.button('loading');
        $.myAjax({
            url: postApiUrl + 'AddPost/',
            type: 'POST',
            data: ko.toJSON(post),
            success: function (data) {
                self.posts.splice(0, 0, new Post(data, self.hub));
                self.newMessage('');
                $('#uploadLink').val('');
                $('.fileUpload').val('');
                $thisButton.button('reset');
            }
        });
    }

    self.saveGroup = function () {
        if (self.error() == null) {
            self.hub.server.addGroup($('#CreatGroup').jsonify()).fail(function (err) {
                self.error(err);
            });
        } else
            self.error('Please fix the error')
    }

    self.hub.client.addPost = function (post, notification) {
        self.posts.splice(0, 0, new Post(post, self.hub));
        vmHeader.newNotifications(notification);
    }

    self.hub.client.updatePost = function (post, notification) {
        self.posts.splice(0, 0, new Post(post, self.hub));
    }

    self.hub.client.deletePost = function (post) {
        self.posts.remove(function (item) {
            return item.PostId == post.PostId;
        });
    }

    self.hub.client.error = function (err) {
        self.error(err);
    }

    self.hub.client.addComment = function (comment, notification) {
        //check in existing posts
        var posts = $.grep(self.posts(), function (item) {
            return item.PostId === comment.PostId;
        });
        if (posts.length > 0) {
            posts[0].PostComments.push(new Comment(comment));
        }
        vmHeader.newNotifications(notification);
    }

    self.hub.client.updateComment = function (comment, notification) {
        //check in existing posts
        var posts = $.grep(self.posts(), function (item) {
            return item.PostId === comment.PostId;
        });
        if (posts.length > 0) {
            posts[0].PostComments.push(new Comment(comment));
        }
    }

    self.hub.client.deleteComment = function (comment, notification) {
        //check in existing posts
        var posts = $.grep(self.posts(), function (item) {
            return item.PostId === comment.PostId;
        });
        if (posts.length > 0) {
            posts[0].PostComments.push(new Comment(comment));
        }
    }

    self.hub.client.like = function (like, totalLikes, notification) {
        //check in existing posts
        var posts = $.grep(self.posts(), function (item) {
            return item.PostId === like.ItemID;
        });
        if (posts.length > 0) {
            posts[0].TotalLikes(totalLikes);
            vmHeader.newNotifications(notification);
        }
    }

    self.hub.client.unLike = function (like, totalLikes) {
        //check in existing posts
        var posts = $.grep(self.posts(), function (item) {
            return item.PostId === like.ItemID;
        });
        if (posts.length > 0) {
            posts[0].TotalLikes(totalLikes);
        }
    }

    $(window).scroll(function () {
        if (!self.pendingRequest() && !self.last()) {
            if ($(window).scrollTop() == $(document).height() - $(window).height()) {
                self.getNextPage(self.page());
            }
        };
    });

    self.getNextPage = function (page) {
        self.pendingRequest(true);
        $.myAjax({
            url: postApiUrl + 'GetPosts/',
            data: { 'PageNumber': page, 'PageSize': 25, 'PageType': top.PageType, 'PageID': top.PageID, 'IsHome': top.Home ? top.Home : false },
            showProgress: true,
            success: function (data) {
                self.loadPosts(data);
                self.pendingRequest(false);
                if (self.error() == null) {
                    self.page(self.page() + 1)
                }
            }
        });
    }

    return self;
};

//custom bindings
//textarea autosize
ko.bindingHandlers.jqAutoresize = {
    init: function (element, valueAccessor, aBA, vm) {
        if (!$(element).hasClass('msgTextArea')) {
            $(element).css('height', '1em');
        }
        $(element).autosize();
    }
};

ko.bindingHandlers.executeOnEnter = {
    init: function (element, valueAccessor, allBindings, viewModel) {
        var callback = valueAccessor();
        $(element).keypress(function (event) {
            var keyCode = (event.which ? event.which : event.keyCode);
            if (keyCode === 13) {
                callback.call(viewModel);
                return false;
            }
            return true;
        });
    }
};

ko.bindingHandlers.player = {
    init: function () {
        $('audio,video').mediaelementplayer({
            //mode: 'shim',
            success: function (player, node) {
                $('#' + node.id + '-mode').html('mode: ' + player.pluginType);
            },
            error: function (x) {
            }
        });
        $('.mejs-audio .mejs-inner .mejs-layers .mejs-poster img').show();
        $('audio').height('30');
        $('.mejs-audio .mejs-inner .mejs-layers .mejs-poster').height('270');
    }
}
