var userApiUrl = '/api/Page/', chatApiUrl = '/api/Chat/';

function Tooltip(data, hub) {
    var self = this;
    data = data || {};
    self.ID = data.ID;
    self.Fullname = data.Name;
    self.Description = data.Description || "";
    self.Username = data.Username || "";
    self.Gender = data.Gender;
    self.Image = data.Image || "";
    self.Wallpaper = data.Wallpaper || "";
    self.error = ko.observable();
    self.hub = hub;



    self.addFriend = function () {
        var ID = this.ID;
        self.hub.server.addFriend(ID).done(function () {
        }).fail(function (err) {
            self.error(err);
        });
    }

    self.newMessage = ko.observable();

    self.addFriendRequest = function () {
        var ID = this.ID;
        self.hub.server.addFriendRequest(ID).done(function () {
            vmPost.requests.remove(function (item) {
                return item.ID == ID;
            });

        }).fail(function (err) {
            self.error(err);
        });
    }

    self.friendMessage = function () {
        var ID = this.ID;
        var text = self.newMessage();
        self.hub.server.addFriendMessage(ID, text).done(function () {
            self.newMessage('');
        }).fail(function (err) {
            self.error(err);
        });
    }

    self.friendReject = function () {
        var ID = this.ID;
        self.hub.server.rejectFriend(ID).done(function () {
            vmPost.requests.remove(function (item) {
                return item.ID == ID;
            });

        }).fail(function (err) {
            self.error(err);
        });
    }

    self.friendRequestCancel = function () {
        var ID = this.ID;
        self.hub.server.cancelFriendRequest(ID).done(function () {
            vmPost.requests.remove(function (item) {
                return item.ID == ID;
            });

        }).fail(function (err) {
            self.error(err);
        });
    }
}

function VM_Page() {
    var self = this;
    self.ID = ko.observable();
    self.Name = ko.observable();
    self.FullName = ko.observable();
    self.Wallpaper = ko.observable();
    self.Picture = ko.observable();
    self.Description = ko.observable();
    self.Requester = ko.observable();
    self.Reciever = ko.observable();
    self.PageRelationStatus = ko.observable();
    self.PageType = ko.observable();
    self.newMessage = ko.observable();
    self.error = ko.observable();

    // Reference the proxy for the hub.  
    self.hub = $.connection.pageHub;

    self.init = function () {
        self.error(null);
        if (top.PageType != null)
            return $.myAjax({
                url: userApiUrl + '/GetPageInfo/',
                data: { 'PageType': top.PageType, 'PageID': top.PageID },
                success: function (data) {
                    self.loadPageInfo(data);
                    document.title = data.FullName;
                }
            });
    }

    //functions called by the Hub
    self.loadPageInfo = function (data) {
        data = data || {};
        self.ID(data.ID);
        self.Name(data.Name);
        self.FullName(data.FullName);
        self.Wallpaper(data.Wallpaper);
        self.Picture(data.Picture);
        self.Description(data.Description);
        self.PageRelationStatus(data.PageRelationStatus);
        self.Requester(data.Requester);
        self.Reciever(data.Reciever);
        self.PageType(data.PageType);
    }

    self.uploadWallpaper = function (file) {
        var fileInfo = checkFile(file, 2);
        if (fileInfo != true) {
            vmMessage.newMessage({ 'Title': 'Fix following errors:-', 'Type': MessageType.Error, 'Message': fileInfo });
            return;
        }

        var formData = new FormData();
        formData.append('fileData', file);
        $.myAjax({
            url: fileApiUrl + 'PostWallpaper/',
            contentType: false,
            cache: false,
            type: 'POST',
            processData: false,
            data: formData,
            showProgress: true,
            success: function (data) {
                var arr = data.split('#');
                var link = arr[0];
                var linkIcon = arr[1];
                self.Wallpaper(link);

                var post = new Post();
                post.Message = ' new wallpaper';
                post.Privacy = 1;
                post.PostDescription = ' updated wallpaper';
                post.Link = link;
                post.LinkIcon = linkIcon;
                post.PostType = 2;
                post.broadCastType = 1;

                vmPost.addPost(post);
            }
        });
    }

    self.uploadPicture = function (file) {
        var fileInfo = checkFile(file, 2);
        if (fileInfo != true) {
            vmMessage.newMessage({ 'Title': 'Fix following errors:-', 'Type': MessageType.Error, 'Message': fileInfo });
            return;
        }

        var formData = new FormData();
        formData.append('fileData', file);
        $.myAjax({
            url: fileApiUrl + 'PostPicture/',
            contentType: false,
            cache: false,
            type: 'POST',
            processData: false,
            data: formData,
            showProgress: true,
            success: function (data) {
                var arr = data.split('#');
                var link = arr[0];
                var linkIcon = arr[1];
                self.Picture(link);

                var post = new Post();
                post.Message = ' new picture';
                post.Privacy = 1;
                post.PostDescription = ' updated profile picture';
                post.Link = link;
                post.LinkIcon = linkIcon;
                post.PostType = 2;
                post.broadCastType = 1;

                vmPost.addPost(post);
            }
        });
    }

    self.addFriendRequest = function () {
        var friendRequest = {};
        friendRequest.PageID = this.ID;
        friendRequest.PageType = this.PageType;
        friendRequest.PageRelationStatus = this.PageRelationStatus;
        friendRequest.broadCastType = 1;
        friendRequest.UserID = -1;
        friendRequest.CreatedOn = new Date();
        return $.myAjax({
            url: pageApiUrl + '/AddFriendRequest/',
            data: ko.toJSON(friendRequest),
            type: 'POST',
            success: function (data) {
                self.Requester(vmHeader.ID());
                self.Reciever(friendRequest.PageID());
                self.PageRelationStatus(data);
            }
        });
    }

    self.acceptFriendRequest = function () {
        var friendRequest = {};
        friendRequest.UserID = this.ID();
        friendRequest.PageType = this.PageType;
        friendRequest.PageRelationStatus = 0;
        friendRequest.broadCastType = 1;
        friendRequest.PageID = -1;
        friendRequest.CreatedOn = new Date();
        return $.myAjax({
            url: pageApiUrl + '/AcceptFriendRequest/',
            data: ko.toJSON(friendRequest),
            type: 'POST',
            success: function (data) {
                vmHeader.removeRequests(data);
                //    vmHeader.requests.remove(function (item) {
                //        return item.UserID == friendRequest.UserID;
                //    });
                //    vmPage.Requester(data.UserID);
                //    vmPage.Reciever(data.PageID);
                //    vmPage.PageRelationStatus(data.PageRelationStatus);
            }
        });
    }

    self.rejectFriendRequest = function () {
        var friendRequest = {};
        friendRequest.UserID = this.ID();
        friendRequest.PageType = this.PageType;
        friendRequest.PageRelationStatus = 0;
        friendRequest.broadCastType = 1;
        friendRequest.PageID = -1;
        friendRequest.CreatedOn = new Date();
        return $.myAjax({
            url: pageApiUrl + '/RejectUserFriendRequest/',
            data: ko.toJSON(friendRequest),
            type: 'POST',
            success: function (data) {
                vmHeader.removeRequests(data);
                //vmPage.Requester(data.UserID);
                //vmPage.Reciever(data.PageID);
                //vmPage.PageRelationStatus(data.PageRelationStatus);
            }
        });
    }

    self.cancelFriendRequest = function () {
        var friendRequest = {};
        friendRequest.UserID = -1;
        friendRequest.PageType = this.PageType;
        friendRequest.PageRelationStatus = 0;
        friendRequest.broadCastType = 1;
        friendRequest.PageID = this.ID();
        friendRequest.CreatedOn = new Date();
        return $.myAjax({
            url: pageApiUrl + '/CancelFriendRequest/',
            data: ko.toJSON(friendRequest),
            type: 'POST',
            success: function (data) {
                vmHeader.removeRequests(data);
                //vmPage.Requester(data.UserID);
                //vmPage.Reciever(data.PageID);
                //vmPage.PageRelationStatus(data.PageRelationStatus);
            }
        });
    }

    self.removeFriend = function () {
        var friendRequest = {};
        friendRequest.UserID = -1;
        friendRequest.PageType = this.PageType;
        friendRequest.PageRelationStatus = self.PageRelationStatus();
        friendRequest.broadCastType = 1;
        friendRequest.PageID = this.ID();
        friendRequest.CreatedOn = new Date();
        return $.myAjax({
            url: pageApiUrl + '/RemoveFriend/',
            data: ko.toJSON(friendRequest),
            type: 'POST',
            success: function (data) {
                vmHeader.removeRequests(data);
                //vmPage.Requester(data.UserID);
                //vmPage.Reciever(data.PageID);
                //vmPage.PageRelationStatus(data.PageRelationStatus);
            }
        });
    }

    self.showMessageBox = function () {
        var _model = this;
        $('.popup-modal').modal('show');
        return $.myAjax({
            url: '/Page/Message/',
            dataType: 'html',
            success: function (data) {
                $('.popup-modal .modal-dialog').html(data);
                ko.applyBindings(_model, $('.popup-modal .modal-dialog .modal-content').get()[0]);
            }
        });
    }

    self.addMessage = function (e, data) {
        var $thisButton = $(data.target);
        $thisButton.button('loading');
        self.showDialog(false);
        var _message = {};
        _message.ToID = this.ID();
        _message.Message = self.newMessage();
        _message.PageType = this.PageType();
        _message.CreatedOn = new Date();
        _message.FromID = -1;
        _message.broadCastType = 1;
        return $.myAjax({
            url: chatApiUrl + '/SendChatMessage/',
            data: ko.toJSON(_message),
            type: 'POST',
            success: function (data) {
                $thisButton.button('reset');
                self.newMessage('');
                $('.popup-modal').modal('hide');
                vmMessage.newMessage({ 'Title': 'Message has been sent successfully !!!', 'Type': MessageType.Success });
            }
        });
    }

    return self;
}

ko.bindingHandlers.modal = {
    init: function (element, valueAccessor) {
        var value = valueAccessor();
        if (typeof value === 'function') {
            $(element).on('show.bs.modal', function (e) {
                $('.popup-modal .modal-dialog').html('<div class="empty-content">' +
                    '<center><span><i class="fa fa-5x fa-spinner fa-spin"></i></span></center></div>');
            })
            $(element).on('shown.bs.modal', function (e) {
                value(true);
                if ($(window).height() >= 320) {
                    $(window).resize(adjustModalMaxHeightAndPosition).trigger("resize");
                }
            })
            $(element).on('hidden.bs.modal', function (e) {
                value(false);
                $('.popup-modal .modal-dialog').html('');
            })
        }
    }
}

var iSModelOpen = false;