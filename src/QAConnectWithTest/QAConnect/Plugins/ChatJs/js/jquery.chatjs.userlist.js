/// <reference path="../../../Scripts/Typings/jquery/jquery.d.ts"/>
/// <reference path="../../../scripts/typings/knockout/knockout.d.ts" />
/// <reference path="jquery.chatjs.interfaces.ts"/>
/// <reference path="jquery.chatjs.utils.ts"/>
/// <reference path="jquery.chatjs.adapter.ts"/>
/// <reference path="jquery.chatjs.window.ts"/>
/// <reference path="jquery.chatjs.messageboard.ts"/>

var UserListOptions = (function () {
    function UserListOptions() {
    }
    return UserListOptions;
})();

var UserList = (function () {
    function UserList(jQuery, options) {
        var _this = this;
        this.$el = jQuery;

        var defaultOptions = new UserListOptions();
        defaultOptions.emptyRoomText = "No users available for chatting.";
        defaultOptions.height = 100;
        defaultOptions.excludeCurrentUser = false;
        defaultOptions.userClicked = function () {
        };

        this.options = $.extend({}, defaultOptions, options);

        this.$el.addClass("user-list");

        // ChatJsUtils.setOuterHeight(this.$el, this.options.height);
        //var _mainWindow = this.$el.parent().parent();
        // when the user list changed, this list must be updated
        this.options.adapter.client.onUserListChanged(function (userListData) {
            if ((_this.options.roomId && userListData.RoomId == _this.options.roomId) || (_this.options.conversationId && _this.options.conversationId == userListData.ConversationId)) {
                var userList = userListData.UserList;
                _this.populateList(userList);
            }
        });

        // loads the list now
        this.options.adapter.server.getUserList(this.options.roomId, this.options.conversationId, function (userList) {
            userList = userList.slice(0);

            if (_this.options.excludeCurrentUser) {
                var j = 0;
                while (j < userList.length) {
                    if (userList[j].Id == _this.options.userId)
                        userList.splice(j, 1);
                    else
                        j++;
                }
            }

            _this.options.usersList = userList;
            _this.populateList(userList);
        });

        // loads the list now
        this.options.adapter.client.onUpdateUserList(function (userInfo) {
            _this.updateList(userInfo);
        });
    }
    UserList.prototype.populateList = function (rawUserList) {
        var _this = this;
        var userList = this.options.usersList;
        this.$el.html('');
        if (userList.length == 0) {
            $("<div/>").addClass("user-list-empty").text(this.options.emptyRoomText).appendTo(this.$el);
        } else {
            for (var i = 0; i < userList.length; i++) {
                var $userListItem = $("<div/>").addClass("user-list-item").attr("data-val-id", userList[i].Id).attr("data-bind", "attr: { src: PostedByAvatar ,'data-PageID':PostedBy},tooltip:me").appendTo(this.$el);

                $("<img/>").addClass("profile-picture").attr("src", userList[i].ProfilePictureUrl).appendTo($userListItem);

                $("<div/>").addClass("profile-status").addClass(this.getStatus(userList[i].Status)).appendTo($userListItem);

                $("<div/>").addClass("content").addClass('truncate').text(userList[i].Name).appendTo($userListItem);

                // makes a click in the user to either create a new chat window or open an existing
                // I must clusure the 'i'
                (function (userId) {
                    // handles clicking in a user. Starts up a new chat session
                    $userListItem.click(function () {
                        _this.options.userClicked(userId);
                    });
                })(userList[i].Id);
            }
        }
    };

    UserList.prototype.updateList = function (userInfo) {
        var _this = this;
        var list = $('.chat-window-inner-content.user-list .user-list-item');
        var exist = false;
        for (var i = 0; i < list.length; i++) {
            if ($(list[i]).attr("data-val-id") == userInfo.Id) {
                var statusDiv = $(list[i]).find('.profile-status');
                statusDiv.removeClass(statusDiv.attr('class').split(' ')[1]);
                statusDiv.addClass(this.getStatus(userInfo.Status));
                exist = true;
            }
        }

        if (!exist) {
            var $userListItem = $("<div/>").addClass("user-list-item").attr("data-val-id", userInfo.Id).appendTo(this.$el);

            $("<img/>").addClass("profile-picture").attr("src", userInfo.ProfilePictureUrl).appendTo($userListItem);

            $("<div/>").addClass("profile-status").addClass(this.getStatus(userInfo.Status)).appendTo($userListItem);

            $("<div/>").addClass("content").addClass('truncate').text(userInfo.Name).appendTo($userListItem);

            // makes a click in the user to either create a new chat window or open an existing
            // I must clusure the 'i'
            (function (userId) {
                // handles clicking in a user. Starts up a new chat session
                $userListItem.click(function () {
                    _this.options.userClicked(userId);
                });
            })(userInfo.Id);
        }
    };

    UserList.prototype.getStatus = function (statusID) {
        switch (statusID) {
            case 1:
                return "online";
            case 2:
                return "offline";
            default:
                return "offline";
        }
    };
    return UserList;
})();

$.fn.userList = function (options) {
    if (this.length) {
        this.each(function () {
            var data = new UserList($(this), options);
            $(this).data('userList', data);
        });
    }
    return this;
};
//# sourceMappingURL=jquery.chatjs.userlist.js.map
