var pageApiUrl = '/api/Page/';
function VM_User() {
    var self = this;
    self.ID = ko.observable();
    self.Name = ko.observable();
    self.FullName = ko.observable();
    self.Wallpaper = ko.observable();
    self.Picture = ko.observable();
    self.Description = ko.observable();
    self.error = ko.observable();

    // Reference the proxy for the hub.  
    self.hub = $.connection.userHub;

    self.init = function () {
        self.error(null);
        return $.myAjax({
            url: pageApiUrl + '/GetUserInfo/',
            success: function (data) {
                self.loadUserInfo(data);
            }
        });
    }

    //functions called by the Hub
    self.loadUserInfo = function (data) {
        data = data || {};
        self.ID(data.ID);
        self.Name(data.Name);
        self.FullName(data.FullName);
        self.Wallpaper(data.Wallpaper);
        self.Picture(data.Picture);
        self.Description(data.Description);
    }
}

//all knockout data setting
var vmMessage = new VM_Message();
var vmUser = new VM_User();
//var vmTooltip = new VM_Page();
var vmHeader = new VM_Header();
var vmLeftHome = new VM_LeftHome();
ko.applyBindings({
    koMesssages: vmMessage,
    koUser: vmUser
});
$.connection.hub.start().done(function () {
    vmUser.init();
    //vmTooltip.init();
});