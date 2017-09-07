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

    self.init = function () {
        self.error(null);
        return $.myAjax({
            url: headerApiUrl + '/GetLeftHome/',
            success: function (data) {
                self.loadLeftHome(data);
            }
        });
    }

    self.loadLeftHome = function (data) {
        self.groups($.map(data.Groups, function (item) { return new Groups(item); }));
        self.events($.map(data.Events, function (item) { return new Events(item); }));
    }

    self.addGroup = function () {
        $.get('/Group/Create/').done(function (data) {

            $('.popup-modal .modal-dialog').html(data);
            ko.applyBindings(vmPost, $('.popup-modal .modal-dialog .modal-content').get()[0]);
            $('.popup-modal').modal('show');
        });
    }

    self.onDisconnect = function (event) {

        self.hub.server.onDisconnected().done(function () {

        }).fail(function (err) {
            self.error(err);
        });
    }

    self.initHub = function () {
        // Reference the proxy for the hub.  
        self.hub = $.connection.pageHub;

        self.hub.client.error = function (err) {
            self.error(err);
        }

        self.hub.client.addGroup = function (data) {
            self.groups.splice(0, 0, new Groups(data));
            self.hub.server.addPost({
                "PostDescription": "created a group > " + data.FullName,
                "TypeID": 3, "Privacy": data.Privacy,
                "Text": "new group", "Link": "/Group/" + data.Name,
                "LinkIcon": data.Wallpaper, "LinkHeader": data.Fullname,
                "LinkDescription": data.Description
            }).fail(function (err) {
                self.error(err);
            });
        };
    };

    return self;
};

try {
    var vmMessage = new VM_Message();
    var vmHeader = new VM_Header();
    var vmLeftHome = new VM_LeftHome();
    var vmTooltip = new VM_Page();
    var vmPost = new VM_Post();
    ko.applyBindings({
        koMesssages: vmMessage,
        koHeader: vmHeader,
        koLeftHome: vmLeftHome,
        koPost: vmPost,
        koTooltip: vmTooltip
    });
} catch (e) {
    ko.applyBindings({
        koMesssages: vmMessage,
        koHeader: vmHeader,
        koLeftHome: vmLeftHome,
        koTooltip: vmTooltip
    });
}

$(document).ready(function () {
    vmLeftHome.init();
})

$.connection.hub.start().done(function () {
    vmLeftHome.initHub();
});