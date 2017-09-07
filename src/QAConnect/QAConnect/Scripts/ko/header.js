var postApiUrl = '/api/WallPost/', commentApiUrl = '/api/Comment/', likeApiUrl = '/api/Like/';

// Model for search in header
function Search(data) {
    var self = this;
    data = data || {};
    self.Url = data.Url || "";
    self.Fullname = data.Fullname || "";
    self.Username = data.Username || "";
    self.Gender = data.Gender;
    self.Image = data.Image || "";
    self.error = ko.observable();
}

// Model for friend request
function FriendRequest(data, hub) {
    var self = this;
    data = data || {};
    self.ID = data.ID;
    self.UserID = data.UserID,
    self.Name = data.PostedByName || "";
    self.Avatar = data.PostedByAvatar || "";
    self.UserInfo = data.UserInfo;
    self.IsViewed = data.IsRead;
    self.PageType = data.PageType;
    self.hub = hub;
    self.error = ko.observable("");

    self.acceptFriendRequest = function () {
        var friendRequest = {};
        friendRequest.UserID = this.UserID;
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
                debugger;
                vmHeader.requests.remove(function (item) {
                    return item.UserID == friendRequest.UserID;
                });
                vmPage.Requester(data.UserID);
                vmPage.Reciever(data.PageID);
                vmPage.PageRelationStatus(data.PageRelationStatus);
            }
        });
    }

    self.rejectFriendRequest = function () {
        var ID = this.ID;
        var friendRequest = {};
        friendRequest.UserID = this.UserID;
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
                vmHeader.requests.remove(function (item) {
                    return item.UserID == friendRequest.UserID;
                });
                vmPage.Requester(data.UserID);
                vmPage.Reciever(data.PageID);
                vmPage.PageRelationStatus(data.PageRelationStatus);
            }
        });
    }

}

function Messages(data) {
    var self = this;
    data = data || {};
    self.ID = data.ID;
    self.Avatar = data.PostedByAvatar;
    self.Name = data.PostedByName,
    self.Text = data.Message;
    self.CreatedOn = getTimeAgo(data.CreatedOn);
    self.IsViewed = data.IsRead;
    self.error = ko.observable();
}

function Notification(data) {
    var self = this;
    data = data || {};
    self.ID = data.ID;
    self.Text = data.Message;
    self.PostedByAvatar = data.PostedByAvatar;
    switch (data.TypeID) {
        case 1:
            self.Link = 'post_' + data.ItemID;
            break;
        case 2:
            self.Link = 'comment_' + data.ItemID;
            break;
        case 3:
            self.Link = 'post_' + data.ItemID;
            break;
        case 4:
            self.Link = 'comment_' + data.ItemID;
            break;
        default:
            self.Link = 'comment_';
    }
    self.IsViewed = data.IsRead;
    self.error = ko.observable();

}

function NewFriend(data, hub) {
    var self = this;
    data = data || {};
    self.ID = data.Id;
    self.Name = data.Name || "";
    self.FullName = data.FullName || "";
    self.Avtar = data.Avtar || "";
    self.Detail = data.Detail;
    self.hub = hub;
    self.error = ko.observable("");
}

function VM_Header() {
    var self = this;
    self.ID = ko.observable();
    self.Name = ko.observable();
    self.FullName = ko.observable();
    self.Wallpaper = ko.observable();
    self.Picture = ko.observable();
    self.Description = ko.observable();
    self.newMessage = ko.observable();
    self.error = ko.observable();
    self.searchResult = ko.observableArray();
    self.searchText = ko.observable('');
    self.pendingRequest = ko.observable(true);
    self.notificationPage = ko.observable(1);
    self.messagePage = ko.observable(1);
    self.requestPage = ko.observable(1);
    self.searchPage = ko.observable(1);
    self.notificationLast = ko.observable();
    self.messageLast = ko.observable();
    self.requestLast = ko.observable();
    self.searchLast = ko.observable();

    self.newFriends = ko.observableArray();

    self.requests = ko.observableArray();
    self.messages = ko.observableArray();
    self.notifications = ko.observableArray();

    // Reference the proxy for the hub.  
    self.hub = $.connection.headerHub;


    self.init = function () {
        self.error(null);
        return $.myAjax({
            url: headerApiUrl,
            success: function (data) {
                self.loadHeader(data);
            }
        });
    }

    self.onDisconnect = function (event) {
        self.hub.server.onDisconnected().done(function () {
            document.getElementById('logoutForm').submit();
        }).fail(function (err) {
            self.error(err);
        });
    }

    //window.onbeforeunload = function () {
    //    self.onDisconnect(event);
    //};
    self.requestCount = ko.observableArray(0);
    self.messageCount = ko.observable(0);
    self.notificationCount = ko.observable(0);

    self.loadHeader = function (data) {
        //load user information
        var Userdata = data.UserInfo || {};
        self.ID(Userdata.ID);
        self.Name(Userdata.Name);
        self.FullName(Userdata.FullName);
        self.Wallpaper(Userdata.Wallpaper);
        self.Picture(Userdata.Picture);
        self.Description(Userdata.Description);

        self.requests($.map(data.FriendRequests, function (item) { return new FriendRequest(item, self.hub); }));
        self.messages($.map(data.Messages, function (item) { return new Messages(item); }));
        self.notifications($.map(data.Notifications, function (item) { return new Notification(item); }));
        var count = 0;
        ko.utils.arrayForEach(data.FriendRequests, function (item) {
            if (!item.IsRead) {
                count++;
            }
        });
        self.requestCount(count);
        count = 0;
        ko.utils.arrayForEach(data.Messages, function (item) {
            if (!item.IsRead) {
                count++;
            }
        });
        self.messageCount(count);
        count = 0;
        ko.utils.arrayForEach(data.Notifications, function (item) {
            if (!item.IsRead) {
                count++;
            }
        });
        self.notificationCount(count);
        self.pendingRequest(false);
    }

    self.searchFriends = function () {
        return $.myAjax({
            url: headerApiUrl + '/GetSearchNewPages/',
            data: { 'Key': '', 'PageNumber': 1, 'PageSize': 20 },
            success: function (data) {
                self.newFriends($.map(data, function (item) {
                    return new NewFriend(item);
                }));
            }
        });
    }

    self.loadNotifications = function (data) {

        ko.utils.arrayForEach(data, function (item) {
            if (!item.IsRead) {
                self.notificationCount(self.notificationCount() + 1);
            }
        });

        if (!data.length) {
            self.notificationLast(1);
            return;
        }

        var mappedNotifications = $.map(data, function (item) { return new Notification(item); });

        self.notificationPage(self.notificationPage() + 1);

        if (self.notificationPage() > 1) {
            self.notifications(self.notifications().concat(mappedNotifications));
        }
        else {
            self.notifications(mappedNotifications);
        }
    }

    self.loadMessages = function (data) {

        ko.utils.arrayForEach(data, function (item) {
            if (!item.IsRead) {
                self.messageCount(self.messageCount() + 1);
            }
        });

        if (!data.length) {
            self.messageLast(1);
            return;
        }

        var mappedMessages = $.map(data, function (item) { return new Messages(item); });

        self.messagePage(self.messagePage() + 1);

        if (self.messagePage() > 1) {
            self.messages(self.messages().concat(mappedMessages));
        }
        else {
            self.messages(mappedMessages);
        }
    }

    self.loadRequests = function (data) {

        ko.utils.arrayForEach(data, function (item) {
            if (!item.IsRead) {
                self.requestCount(self.requestCount() + 1);
            }
        });

        if (!data.length) {
            self.requestLast(1);
            return;
        }

        var mappedRequests = $.map(data, function (item) { return new FriendRequests(item); });

        self.requestPage(self.requestPage() + 1);

        if (self.requestPage() > 1) {
            self.requests(self.requests().concat(mappedNotifications));
        }
        else {
            self.requests(mappedNotifications);
        }
    }

    self.loadSearchResult = function (data) {
        if (!data.length) {
            self.searchLast(1);
            if (self.searchPage() == 1) {
                self.searchResult([]);
                return;
            }
        }

        var mappedSearch = $.map(data, function (item) { return new Search(item); });

        self.searchPage(self.searchPage() + 1);

        if (self.searchPage() > 1) {
            self.searchResult(self.searchResult().concat(mappedSearch));
        }
        else {
            self.searchResult(mappedSearch);
        }
    }

    self.hub.client.error = function (err) {
        self.error(err);
    }

    self.hub.client.newNotifications = function (data) {
        self.newNotifications(data);
    }

    self.hub.client.newMessages = function (data) {
        self.newMessages(data);
    }

    self.hub.client.newRequests = function (data) {
        self.newRequests(data);
    }

    self.hub.client.removeRequests = function (data) {
        self.removeRequests(data);
    }

    self.newNotifications = function (data) {
        self.notificationCount(self.notificationCount() + 1);
        if (self.notifications().length == 10) {
            self.notifications.remove(self.notifications.pop());
        }
        self.notifications.splice(0, 0, new Notification(data));
    };

    self.newMessages = function (data) {
        self.messageCount(self.messageCount() + 1);
        if (self.messageCount().length == 10) {
            self.messages.remove(self.messages.pop());
        }
        self.messages.splice(0, 0, new Messages(data));
    };

    self.newRequests = function (data) {
        self.requestCount(self.requestCount() + 1);
        if (self.requests().length == 10) {
            self.requests.remove(self.requests.pop());
        }
        self.requests.splice(0, 0, new FriendRequest(data, self.hub));

        vmPage.Requester(data.UserID);
        vmPage.Reciever(data.PageID);
        vmPage.PageRelationStatus(data.PageRelationStatus);
    }

    self.removeRequests = function (data) {
        self.requests.remove(function (item) {
            return item.UserID == data.UserID;
        });
        self.requestCount(self.requestCount() - 1);
        vmPage.Requester(data.UserID);
        vmPage.Reciever(data.PageID);
        vmPage.PageRelationStatus(data.PageRelationStatus);
    }

    self.headerRead = function (type) {
        var me = this;
        var Ids = [];
        switch (type) {
            case 1:
                $.each(self.notifications(), function (i, o) {
                    if (!o.IsViewed)
                        Ids.push(o.ID);
                })
                break;
            case 2:
                $.each(self.messages(), function (i, o) {
                    if (!o.IsViewed)
                        Ids.push(o.ID);
                })

                break;
            case 3:
                $.each(self.requests(), function (i, o) {
                    if (!o.IsViewed)
                        Ids.push(o.ID);
                })

                break;
        }
        if (Ids.length > 0)
            return $.myAjax({
                url: headerApiUrl,
                type: 'POST',
                data: JSON.stringify({
                    IDs: Ids,
                    Type: type
                }),
                success: function () {
                    switch (type) {
                        case 1:
                            self.notificationCount(0);
                            break;
                        case 2:
                            self.messageCount(0);
                            break;
                        case 3:
                            self.requestCount(0);

                            break;
                    }
                }
            })
    }

    self.searchStart = function (key, event) {
        if (event == null || event == undefined)
            return true;
        key = self.searchText() + String.fromCharCode(event.keyCode);
        if (key == '')
        { }
        else {
            return $.myAjax({
                url: headerApiUrl + '/GetSearchPages/',
                data: { 'Key': key, 'PageNumber': self.searchPage(), 'PageSize': 10 },
                success: function (result) {
                    self.searchPage(1);
                    self.loadSearchResult(result);
                }
            });
        }
    };

    self.scrolled = function (type, event) {
        if (event == null || event == undefined)
            return true;
        var elem = event.target;
        if (elem.scrollTop == (elem.scrollHeight - elem.offsetHeight)) {
            var _url = headerApiUrl;
            var _inpudata = {};
            var last = false;
            switch (type) {
                case 1:
                    _url += 'GetNotifications/';
                    _inpudata = { 'PageNumber': self.notificationPage(), 'PageSize': 10 };
                    last = self.notificationLast();
                    break;
                case 2:
                    _url += '/GetMessages/';
                    _inpudata = { 'PageNumber': self.messagePage(), 'PageSize': 10 };
                    last = self.messageLast();
                    break;
                case 3:
                    _url += '/GetFriendRequests/';
                    _inpudata = { 'PageNumber': self.requestPage(), 'PageSize': 10 };
                    last = self.requestLast();
                    break;
                case 4:
                    _url += '/GetSearchPages/';
                    _inpudata = { 'PageNumber': self.searchPage(), 'PageSize': 10, 'Key': self.searchText() };
                    last = self.searchLast();
                    break;
            }

            if (!self.pendingRequest() && !last) {
                var data = {};

                self.pendingRequest(true);

                $.myAjax({
                    url: _url,
                    data: _inpudata,
                    async: false,
                    success: function (result) {
                        self.pendingRequest(false);
                        data = result;
                    }
                });

                switch (type) {
                    case 1:
                        self.loadNotifications(data);
                        break;
                    case 2:
                        self.loadMessages(data);
                        break;
                    case 3:
                        self.loadRequests(data);
                        break;
                    case 4:
                        self.loadSearchResult(data);
                        break;
                }
            };
        }
    };

    return self;
};