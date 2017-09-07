// Models
function Groups(data) {
    var self = this;
    data = data || {};
    self.ID = data.ID;
    self.Name = data.Name;
    self.FullName = data.FullName;
    self.Icon = data.Icon;
}

function Events(data) {
    var self = this;
    data = data || {};
    self.ID = data.ID;
    self.Name = data.Name;
    self.FullName = data.FullName;
    self.Icon = data.Icon;
}

//View Model

function VM_LeftHome() {
    var self = this;

    self.groups = ko.observableArray(0);
    self.events = ko.observableArray(0);
    self.error = ko.observable();
    // Reference the proxy for the hub.  
    self.hub = $.connection.pageHub;


    self.init = function () {
        self.error(null);
        self.hub.server.getLeftHome().fail(function (err) {
            self.error(err);
        });
    }

    self.hub.client.loadLeftHome = function (data) {
        self.groups($.map(data.Groups, function (item) { return new Groups(item); }));
        self.events($.map(data.Events, function (item) { return new Events(item); }));
    }

    self.addGroup = function () {
        $.get('http://localhost:50632/Group/Create/').done(function (data) {

            $('.popup-modal .modal-dialog').html(data);
            ko.applyBindings(vmPost, $('.popup-modal .modal-dialog .modal-content').get()[0]);
            $('.popup-modal').modal('show');
        });
    }


    self.hub.client.addGroup = function (data) {
        self.groups.splice(0, 0, new Groups(data));
        self.hub.server.addPost({
            "PostDescription": "created a group > " + data.FullName,
            "TypeID": 3, "Privacy": data.Privacy,
            "Text": "new group", "Link": "http://localhost:50632/Group/" + data.Name,
            "LinkIcon": data.Wallpaper, "LinkHeader": data.Fullname,
            "LinkDescription": data.Description
        }).fail(function (err) {
            self.error(err);
        });
    };

    self.onDisconnect = function (event) {

        self.hub.server.onDisconnected().done(function () {

        }).fail(function (err) {
            self.error(err);
        });
    }
    return self;
};