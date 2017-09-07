//all knockout data setting
var vmMessage = new VM_Message();
var vmUser = new VM_User();
var vmPage = new VM_Page();
var vmTooltip = new VM_Page();
var vmHeader = new VM_Header();
var vmLeftHome = new VM_LeftHome();
var vmLeftPage = new VM_LeftPage();
var vmPost = new VM_Post();
ko.applyBindings({
    koMesssages: vmMessage,
    koUser: vmUser,
    koPage: vmPage,
    koTooltip: vmTooltip,
    koHeader: vmHeader,
    koLeftHome: vmLeftHome,
    koLeftPage: vmLeftPage,
    koPost: vmPost
});
$.connection.hub.start().done(function () {
    vmUser.init();
    vmPage.init();
    vmTooltip.init();
    vmHeader.init();
    vmLeftHome.init();
    vmLeftPage.init();
    vmPost.init();
});