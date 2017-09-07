/// <reference path="../../../Scripts/Typings/jquery/jquery.d.ts"/>
/// <reference path="../../../scripts/typings/knockout/knockout.d.ts" />
/// <reference path="../../../Scripts/Typings/autosize/autosize.d.ts"/>
/// <reference path="jquery.chatjs.utils.ts"/>
/// <reference path="jquery.chatjs.adapter.ts"/>

var MessageBoardOptions = (function () {
    function MessageBoardOptions() {
    }
    return MessageBoardOptions;
})();

var MessageBoard = (function () {
    function MessageBoard(jQuery, options) {
        var _this = this;
        this.$el = jQuery;
        var defaultOptions = new MessageBoardOptions();
        defaultOptions.typingText = " is typing...";
        defaultOptions.playSound = true;
        defaultOptions.height = 100;
        defaultOptions.chatJsContentPath = "/chatjs/";
        defaultOptions.newMessage = function (message) {
        };
        defaultOptions.userClicked = function () {
        };
        this.options = $.extend({}, defaultOptions, options);
        var messBoardOptions = this.options;
        this.$el.addClass("message-board");

        ChatJsUtils.setOuterHeight(this.$el, this.options.height);

        this.$messagesWrapper = $("<div/>").addClass("messages-wrapper").appendTo(this.$el);

        // sets up the text
        var $windowTextBoxWrapper = $("<div/>").addClass("chat-window-text-box-wrapper").appendTo(this.$el);
        var $html = $('#emoji .emoji-container').clone();

        var otherUserID = this.options.otherUserId;

        $html.find(".tab-pane").each(function () {
            var $tabPane = $(this);
            $tabPane.attr("id", $tabPane.attr("id") + otherUserID);
        });

        $html.find("a[data-toggle='tab']").each(function () {
            var $anchor = $(this);
            $anchor.attr("href", $anchor.attr("href") + otherUserID);
        });

        $html.appendTo($windowTextBoxWrapper);

        this.$textBox = $("<div />").attr("contenteditable", "true").attr("style", "background:none; border:medium none;").addClass("chat-window-text-box").appendTo($windowTextBoxWrapper);

        this.$emojiBtn = $("<button />").addClass("chat-window-emoji-btn").appendTo($windowTextBoxWrapper);

        $windowTextBoxWrapper.find('ul.emoji-list li').click(function (d, e) {
            var mm = d.target.outerHTML;
            var textVal = _this.$textBox.html();
            _this.$textBox.html(textVal + mm);
            _this.$textBox.focus();
            _this.$emojiBtn.siblings('.emoji-container').toggle(500);
        });

        this.$emojiBtn.click(function (e) {
            _this.$emojiBtn.siblings('.emoji-container').toggle(500);
        });

        this.$textBox.click(function (e) {
            _this.$emojiBtn.siblings('.emoji-container').hide(500);
        });

        this.$textBox.autosize({
            callback: function (ta) {
                var messagesHeight = _this.options.height - $(ta).outerHeight();
                ChatJsUtils.setOuterHeight(_this.$messagesWrapper, messagesHeight);
            }
        });

        //this.$textBox.html(this.$textBox.html());
        this.options.adapter.client.onTypingSignalReceived(function (typingSignal) {
            var shouldProcessTypingSignal = false;
            _this.options = messBoardOptions;
            if (_this.options.otherUserId) {
                // it's a PM message board.
                shouldProcessTypingSignal = typingSignal.ToID == _this.options.userId && typingSignal.UserFrom.Id == _this.options.otherUserId;
            } else if (_this.options.roomId) {
                // it's a room message board
                shouldProcessTypingSignal = typingSignal.RoomId == _this.options.roomId && typingSignal.UserFrom.Id != _this.options.userId;
            } else if (_this.options.conversationId) {
                // it's a conversation message board
                shouldProcessTypingSignal = typingSignal.ConversationId == _this.options.conversationId && typingSignal.UserFrom.Id != _this.options.userId;
            }

            if (shouldProcessTypingSignal)
                _this.showTypingSignal(typingSignal.UserFrom);
        });

        this.options.adapter.client.onMessagesChanged(function (message) {
            var shouldProcessMessage = false;
            _this.options = messBoardOptions;
            if (_this.options.otherUserId) {
                // it's a PM message board.
                shouldProcessMessage = (message.FromID == _this.options.userId && message.ToID == _this.options.otherUserId) || (message.FromID == _this.options.otherUserId && message.ToID == _this.options.userId);
            } else if (_this.options.roomId) {
                // it's a room message board
                shouldProcessMessage = message.RoomId == _this.options.roomId;
            } else if (_this.options.conversationId) {
                // it's a conversation message board
                shouldProcessMessage = message.ConversationId == _this.options.conversationId;
            }

            if (shouldProcessMessage) {
                _this.addMessage(message);
                if (message.FromID != _this.options.userId) {
                    if (_this.options.playSound)
                        _this.playSound();
                }

                _this.options.newMessage(message);
            }
        });

        // gets the message history
        this.options.adapter.server.getMessageHistory(this.options.roomId, this.options.conversationId, this.options.otherUserId, function (messages) {
            for (var i = 0; i < messages.length; i++) {
                _this.addMessage(messages[i], false);
            }

            _this.adjustScroll();

            _this.$textBox.keypress(function (e) {
                // if a send typing signal is in course, remove it and create another
                if (_this.sendTypingSignalTimeout == undefined) {
                    _this.sendTypingSignalTimeout = setTimeout(function () {
                        _this.sendTypingSignalTimeout = undefined;
                    }, 3000);
                    _this.sendTypingSignal();
                }

                if (e.which == 13) {
                    e.preventDefault();
                    var abc = _this.$textBox.html();
                    if (_this.$textBox.html()) {
                        _this.sendMessage(_this.$textBox.html());
                        _this.$textBox.html('').trigger("autosize.resize");
                    }
                }
            });
        });
    }
    MessageBoard.prototype.showTypingSignal = function (user) {
        var _this = this;
        /// <summary>Adds a typing signal to this window. It means the other user is typing</summary>
        /// <param FullName="user" type="Object">the other user info</param>
        if (this.$typingSignal)
            this.$typingSignal.remove();
        this.$typingSignal = $("<p/>").addClass("typing-signal").text(user.Name + this.options.typingText);
        this.$messagesWrapper.append(this.$typingSignal);
        if (this.typingSignalTimeout)
            clearTimeout(this.typingSignalTimeout);
        this.typingSignalTimeout = setTimeout(function () {
            _this.removeTypingSignal();
        }, 5000);

        this.adjustScroll();
    };

    MessageBoard.prototype.removeTypingSignal = function () {
        /// <summary>Remove the typing signal, if it exists</summary>
        if (this.$typingSignal)
            this.$typingSignal.remove();
        if (this.typingSignalTimeout)
            clearTimeout(this.typingSignalTimeout);
    };

    MessageBoard.prototype.adjustScroll = function () {
        this.$messagesWrapper[0].scrollTop = this.$messagesWrapper[0].scrollHeight;
    };

    MessageBoard.prototype.sendTypingSignal = function () {
        /// <summary>Sends a typing signal to the other user</summary>
        this.options.adapter.server.sendTypingSignal(this.options.roomId, this.options.conversationId, this.options.otherUserId, function () {
        });
    };

    MessageBoard.prototype.sendMessage = function (messageText) {
        /// <summary>Sends a message to the other user</summary>
        /// <param FullName="messageText" type="String">Message being sent</param>
        var generateGuidPart = function () {
            return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
        };

        var clientGuid = (generateGuidPart() + generateGuidPart() + '-' + generateGuidPart() + '-' + generateGuidPart() + '-' + generateGuidPart() + '-' + generateGuidPart() + generateGuidPart() + generateGuidPart());
        var message = new ChatMessageInfo();
        message.FromID = this.options.userId;
        message.PostedByAvatar = this.options.ProfilePictureUrl;
        message.FromName = this.options.Url;
        message.Message = messageText;
        message.ClientGuid = clientGuid;
        message.ToID = this.options.otherUserId;
        message.PageType = 1;
        message.CreatedOn = new Date();
        message.BroadCastType = 1;
        this.addMessage(message);

        this.options.adapter.server.sendMessage(message, function () {
        });
    };

    MessageBoard.prototype.playSound = function () {
        /// <summary>Plays a notification sound</summary>
        /// <param FullName="fileFullName" type="String">The file path without extension</param>
        var $soundContainer = $("#soundContainer");
        if (!$soundContainer.length)
            $soundContainer = $("<div>").attr("id", "soundContainer").appendTo($("body"));
        var baseFileName = this.options.chatJsContentPath + "sounds/chat";
        var oggFileName = baseFileName + ".ogg";
        var mp3FileName = baseFileName + ".mp3";
        var wavFileName = baseFileName + ".wav";

        var $audioTag = $("<audio/>").attr("autoplay", "autoplay");
        $("<source/>").attr("src", mp3FileName).attr("type", "audio/mpeg").appendTo($audioTag);
        $("<embed/>").attr("src", mp3FileName).attr("autostart", "true").attr("loop", "false").appendTo($audioTag);

        $audioTag.appendTo($soundContainer);
    };

    MessageBoard.prototype.focus = function () {
        this.$textBox.focus();
    };

    MessageBoard.prototype.addMessage = function (message, scroll) {
        /// <summary>
        ///     Adds a message to the board. This method is called both when the current user or the other user is sending a
        ///     message
        /// </summary>
        /// <param name="message" type="Object">Message</param>
        /// <param name="clientGuid" type="String">Message client guid</param>
        if (scroll == undefined)
            scroll = true;

        if (message.FromID != this.options.userId) {
            // the message did not came from myself. Better erase the typing signal
            this.removeTypingSignal();
        }

        // takes a jQuery element and replace it's content that seems like an URL with an
        // actual link or e-mail
        function linkify($element) {
            var inputText = $element.html();
            var replacedText, replacePattern1, replacePattern2, replacePattern3;

            //URLs starting with http://, https://, or ftp://
            replacePattern1 = /(\b(?!<img[^>]*?>)(https?|ftp)(?![^<]*?>):\/\/[-A-Z0-9+&@#\/%?=~_|!:,.;]*[-A-Z0-9+&@#\/%=~_|])/gim;
            replacedText = inputText.replace(replacePattern1, '<a href="$1" target="_blank">$1</a>');

            //URLs starting with "www." (without // before it, or it'd re-link the ones done above).
            replacePattern2 = /(^|[^\/])(www\.[\S]+(\b|$))/gim;
            replacedText = replacedText.replace(replacePattern2, '$1<a href="http://$2" target="_blank">$2</a>');

            //Change email addresses to mailto:: links.
            replacePattern3 = /(\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,6})/gim;
            replacedText = replacedText.replace(replacePattern3, '<a href="mailto:$1">$1</a>');

            return $element.html(replacedText);
        }

        function emotify($element) {
            var inputText = $element.html();
            var replacedText = inputText;

            var emoticons = [
                { pattern: ":-\)", cssClass: "happy" },
                { pattern: ":\)", cssClass: "happy" },
                { pattern: "=\)", cssClass: "happy" },
                { pattern: ":-D", cssClass: "very-happy" },
                { pattern: ":D", cssClass: "very-happy" },
                { pattern: "=D", cssClass: "very-happy" },
                { pattern: ":-\(", cssClass: "sad" },
                { pattern: ":\(", cssClass: "sad" },
                { pattern: "=\(", cssClass: "sad" },
                { pattern: ":-\|", cssClass: "wary" },
                { pattern: ":\|", cssClass: "wary" },
                { pattern: "=\|", cssClass: "wary" },
                { pattern: ":-O", cssClass: "astonished" },
                { pattern: ":O", cssClass: "astonished" },
                { pattern: "=O", cssClass: "astonished" },
                { pattern: ":-P", cssClass: "tongue" },
                { pattern: ":P", cssClass: "tongue" },
                { pattern: "=P", cssClass: "tongue" }
            ];

            for (var i = 0; i < emoticons.length; i++) {
                replacedText = replacedText.replace(emoticons[i].pattern, "<span class='" + emoticons[i].cssClass + "'></span>");
            }

            return $element.html(replacedText);
        }

        if (message.ClientGuid && $("p[data-val-client-guid='" + message.ClientGuid + "']").length) {
            // in this case, this message is comming from the server AND the current user POSTED the message.
            // so he/she already has this message in the list. We DO NOT need to add the message.
            $("p[data-val-client-guid='" + message.ClientGuid + "']").removeClass("temp-message").removeAttr("data-val-client-guid");
        } else {
            var $messageP = $("<p/>").html(message.Message);
            if (message.ClientGuid)
                $messageP.attr("data-val-client-guid", message.ClientGuid).addClass("temp-message");

            linkify($messageP);
            emotify($messageP);

            // gets the last message to see if it's possible to just append the text
            var $lastMessage = $("div.chat-message:last", this.$messagesWrapper);
            if ($lastMessage.length && $lastMessage.attr("data-val-user-from") == message.FromID.toString()) {
                // we can just append text then
                $messageP.appendTo($(".chat-text-wrapper", $lastMessage));
            } else {
                // in this case we need to create a whole new message
                var $chatMessage = $("<div/>").addClass("chat-message").attr("data-val-user-from", message.FromID);

                if (message.FromID == this.options.userId) {
                    $chatMessage = $chatMessage.addClass("mine");
                } else {
                    $chatMessage = $chatMessage.addClass("others");
                }

                $chatMessage.appendTo(this.$messagesWrapper);

                //var $gravatarWrapper = $("<div/>").addClass("chat-gravatar-wrapper").appendTo($chatMessage);
                // add image
                //var $urlLink = $("<a/>").appendTo($gravatarWrapper);
                //var $img = $("<img/>").addClass("profile-picture").appendTo($urlLink);
                //$urlLink.attr("href", decodeURI(message.FromName));
                //$img.attr("src", decodeURI(message.PostedByAvatar));
                var $textWrapper = $("<div/>").addClass("chat-text-wrapper").appendTo($chatMessage);

                // add text
                $messageP.appendTo($textWrapper);
            }
        }

        if (scroll)
            this.adjustScroll();
    };
    return MessageBoard;
})();

$.fn.messageBoard = function (options) {
    if (this.length) {
        this.each(function () {
            var data = new MessageBoard($(this), options);
            $(this).data('messageBoard', data);
        });
    }
    return this;
};
//# sourceMappingURL=jquery.chatjs.messageboard.js.map
