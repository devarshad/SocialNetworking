function VM_Chat() {
    self = this;
    //  self.hub = $.connection.chatHub;

    //Server Method

    self.sendMessage = function (message, done) {
        message.RoomId = 1;
        message.ConversationId = 1;
        self.hub.server.sendMessage(message).done(function () {
            done();
        });
    };

    // sends a typing signal to a room, conversation or user
    self.sendTypingSignal = function (roomId, conversationId, ToID, done) {
        roomId = 1;
        conversationId = 1;
        self.hub.server.sendTypingSignal(roomId, conversationId, ToID).done(function () {
            done();
        });
    };

    // gets the message history from a room, conversation or user
    self.getMessageHistory = function (roomId, conversationId, otherUserId, done) {
        roomId = 1;
        conversationId = 1;
        self.hub.server.getMessageHistory(roomId, conversationId, otherUserId).done(function (messageHistory) {
            done(messageHistory);
        });
    };

    // gets the given user info
    self.getUserInfo = function (userId, done) {
        self.hub.server.getUserInfo(userId).done(function (userInfo) {
            done(userInfo);
        });
    };

    // gets the user list in a room or conversation
    self.getUserList = function (roomId, conversationId, done) {
        conversationId = 1;
        self.hub.server.getUserList(roomId, conversationId).done(function (userList) {
            done(userList);
        });
    };

    // gets the rooms list
    self.getRoomsList = function (done) {
        self.hub.server.getRoomsList().done(function (roomsList) {
            done(roomsList);
        });
    };

    // enters the given room
    self.enterRoom = function (roomId, done) {
        self.hub.server.enterRoom(roomId).done(function () {
            done();
        });
    };

    // leaves the given room
    self.leaveRoom = function (roomId, done) {
        self.hub.server.leaveRoom(roomId).done(function () {
            done();
        });
    };

    //client method

    //called when a new message arrives
    self.hub.client.sendMessage = function (message) {
        _this.triggerMessagesChanged(message);
    };

    //called when typing signal recieved
    self.hub.client.sendTypingSignal = function (typingSignal) {
        _this.triggerTypingSignalReceived(typingSignal);
    };

    //callled when any update in user list found
    self.hub.client.updateUserList = function (userInfo) {
        _this.triggerUpdateUserList(userInfo);
    };
}