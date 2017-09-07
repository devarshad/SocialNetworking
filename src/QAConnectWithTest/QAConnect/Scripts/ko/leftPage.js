//Model
function About(data, hub) {
    var self = this;
    data = data || {};
    self.NickName = ko.observable(data.NickName);

    self.Gender = ko.observable(data.Gender);
    self.Age = ko.observable(data.Age);
    self.Religion = ko.observable(data.Religion || "");
    self.Language = ko.observable(data.Language || "");
    self.AboutMe = ko.observable(data.AboutMe || "");
    self.Rating = ko.observable(data.Rating || "");
    self.Privacy = ko.observable(data.Privacy || "");
    self.PhoneNumber = ko.observable(data.PhoneNumber || "");
    self.Skype = ko.observable(data.Skype);
    self.Email = ko.observable(data.Email || "");

    self.Education = ko.observable(data.Education || "");
    self.Organization = ko.observable(data.Organization);
    self.OrganizationLocation = ko.observable(data.OrganizationLocation || "");
    self.PositionInOrganization = ko.observable(data.PositionInOrganization || "");
    self.OrganizationJoinDate = ko.observable(data.OrganizationJoinDate);

    self.Address = ko.observable(data.Address || "");
    self.City = ko.observable(data.City || "");
    self.Country = ko.observable(data.Country);
    self.PostalCode = ko.observable(data.PostalCode || "");
    self.TimeZone = ko.observable(data.TimeZone || "");
    self.Relationship = ko.observable(data.Relationship || "");
    self.Interest = ko.observable(data.Interest);
    self.Hobbies = ko.observable(data.Hobbies || "");

    self.FavAnimals = ko.observable(data.FavAnimals || "");
    self.FavBooks = ko.observable(data.FavBooks);
    self.FavMusics = ko.observable(data.FavMusics || "");
    self.favArtist = ko.observable(data.favArtist || "");
    self.Smoker = ko.observable(data.Smoker);
    self.Drinker = ko.observable(data.Drinker || "");

    self.expanded = ko.observable(false);

    self.toggle = function (item) {
        self.expanded(!self.expanded());
    };
    self.linkLabel = ko.computed(function () {
        return self.expanded() ? "less" : "more";
    });

    self.showAboutBox = function () {
        var _model = this;
        $('.popup-modal').modal('show');
        return $.myAjax({
            url: '/Page/EditAbout/',
            dataType: 'html',
            success: function (data) {
                $('.popup-modal .modal-dialog').html(data);

                ko.applyBindings(_model, $('.popup-modal .modal-dialog .modal-content').get()[0]);
            }
        });
    }

    self.saveAbout = function (e, data) {
        var $thisButton = $(data.target);
        $thisButton.button('loading');
        var _model = {};
        _model.NickName = this.NickName();
        _model.Religion = this.Religion();
        _model.Language = this.Language();
        _model.AboutMe = this.AboutMe();
        _model.Privacy = this.Privacy();
        _model.PhoneNumber = this.PhoneNumber();
        _model.Skype = this.Skype();
        _model.Education = this.Education();
        _model.Organization = this.Organization();
        _model.OrganizationJoinDate = this.OrganizationJoinDate();
        _model.OrganizationLocation = this.OrganizationLocation();
        _model.PositionInOrganization = this.PositionInOrganization();
        _model.Address = this.Address();
        _model.City = this.City();
        _model.Country = this.Country();
        _model.PostalCode = this.PostalCode();
        _model.TimeZone = this.TimeZone();
        _model.Relationship = this.Relationship();
        _model.Interest = this.Interest();
        _model.Hobbies = this.Hobbies();
        _model.FavAnimals = this.FavAnimals();
        _model.favArtist = this.favArtist();
        _model.FavBooks = this.FavBooks();
        _model.FavMusics = this.FavMusics();
        _model.Smoker = this.Smoker();
        _model.Drinker = this.Drinker();
        _model.PageType = vmPage.PageType();
        _model.Id = vmPage.ID();
        return $.myAjax({
            url: pageApiUrl + '/SavePageDetail/',
            data: ko.toJSON(_model),
            type: 'POST',
            success: function (data) {
                $thisButton.button('reset');
                $('.popup-modal').modal('hide');
                vmMessage.newMessage({ 'Title': 'Page detail has been saved successfully !!!', 'Type': MessageType.Success });
                //self.NickName(data.NickName);
                //self.Religion(data.Religion);
                //self.Language(data.Language);
                //self.AboutMe(data.AboutMe);
                //self.Privacy(data.Privacy);
                //self.PhoneNumber(data.PhoneNumber);
                //self.Skype(data.Skype);
                //self.Education(data.Education);
                //self.Organization(data.Organization);
                //self.OrganizationJoinDate(data.OrganizationJoinDate);
                //self.OrganizationLocation(data.OrganizationLocation);
                //self.PositionInOrganization(data.PositionInOrganization);
                //self.Address(data.Address);
                //self.City(data.City);
                //self.Country(data.Country);
                //self.PostalCode(data.PostalCode);
                //self.TimeZone(data.TimeZone);
                //self.Relationship(data.Relationship);
                //self.Interest(data.Interest);
                //self.Hobbies(data.Hobbies);
                //self.FavAnimals(data.FavAnimals);
                //self.favArtist(data.favArtist);
                //self.FavBooks(data.FavBooks);
                //self.FavMusics(data.FavMusics);
                //self.Smoker(data.Smoker);
                //self.Drinker(data.Drinker);
            }
        });
    }
    self.hub = hub;
    self.error = ko.observable("");
}

function Friend(data, hub) {
    var self = this;
    data = data || {};
    self.ID = data.ID;
    self.Name = data.Name || "";
    self.FullName = data.FullName || "";
    self.Avtar = data.Avtar || "";
    self.Detail = data.Detail;
    self.hub = hub;
    self.error = ko.observable("");
}

function Group(data, hub) {
    var self = this;
    data = data || {};
    self.ID = data.ID;
    self.Name = data.Name || "";
    self.FullName = data.FullName || "";
    self.Avtar = data.Avtar || "";
    self.Detail = data.Detail;
    self.hub = hub;
    self.error = ko.observable();
}

function Event(data, hub) {
    var self = this;
    data = data || {};
    self.ID = data.ID;
    self.Name = data.Name || "";
    self.FullName = data.FullName || "";
    self.Avtar = data.Avtar || "";
    self.Detail = data.Detail;
    self.hub = hub;
    self.error = ko.observable();
}

function Photo(data, hub) {
    var self = this;
    data = data || {};
    self.ID = data.ID;
    self.Name = data.Name || "";
    self.FullName = data.FullName || "";
    self.Avtar = data.Avtar || "";
    self.AvtarIcon = data.AvtarIcon || "";
    self.Detail = data.Detail;
    self.hub = hub;
    self.error = ko.observable();
    self.loadSlide = function (Link, PostType, Title, LinkIcon) {
        vmPost.loadSlide(Link, PostType, Title, LinkIcon);
    }
}

function Music(data, hub) {
    var self = this;
    data = data || {};
    self.ID = data.ID;
    self.Name = data.Name || "";
    self.FullName = data.FullName || "";
    self.Avtar = data.Avtar || "";
    self.AvtarIcon = data.AvtarIcon || "";
    self.Detail = data.Detail;
    self.hub = hub;
    self.error = ko.observable();
    self.loadSlide = function (Link, PostType, Title, LinkIcon) {
        vmPost.loadSlide(Link, PostType, Title, LinkIcon);
    }
}

function Video(data, hub) {
    var self = this;
    data = data || {};
    self.ID = data.ID;
    self.Name = data.Name || "";
    self.FullName = data.FullName || "";
    self.Avtar = data.Avtar || "";
    self.AvtarIcon = data.AvtarIcon || "";
    self.Detail = data.Detail;
    self.hub = hub;
    self.error = ko.observable();
    self.loadSlide = function (Link, PostType, Title, LinkIcon) {
        vmPost.loadSlide(Link, PostType, Title, LinkIcon);
    }
}

//View Model
function VM_LeftPage() {
    var self = this;

    self.error = ko.observable();

    self.about = ko.observable();
    self.friends = ko.observableArray();
    self.groups = ko.observableArray();
    self.events = ko.observableArray();
    self.photos = ko.observableArray();
    self.musics = ko.observableArray();
    self.videos = ko.observableArray();

    self.init = function () {
        self.error(null);
        if (top.PageType != null)
            return $.myAjax({
                url: userApiUrl + '/GetLeftPageInfo/',
                data: { 'PageType': top.PageType, 'PageID': top.PageID },
                success: function (data) {
                    self.loadLeftPage(data);
                }
            });
        //self.hub.server.getLeftPage(top.PageType, top.PageID).fail(function (err) {
        //    self.error(err);
        //});
    }

    self.loadLeftPage = function (data) {
        self.about(new About(data.About, self.hub));
        self.friends($.map(data.Friend, function (item) { return new Friend(item, self.hub); }));
        self.groups($.map(data.Group, function (item) { return new Group(item, self.hub); }));
        self.events($.map(data.Event, function (item) { return new Event(item, self.hub); }));
        self.photos($.map(data.Photo, function (item) { return new Photo(item, self.hub); }));
        self.musics($.map(data.Music, function (item) { return new Music(item, self.hub); }));
        self.videos($.map(data.Video, function (item) { return new Video(item, self.hub); }));
    }

    self.initHub = function () {
        // Reference the proxy for the hub.  
        self.hub = $.connection.pageHub;
    }

    self.onDisconnect = function (event) {

        self.hub.server.onDisconnected().done(function () {

        }).fail(function (err) {
            self.error(err);
        });
    }
    return self;
};

////all knockout data setting
//var vmMessage = new VM_Message();
//var vmPage = new VM_Page();
//var vmTooltip = new VM_Page();
//var vmLeftPage = new VM_LeftPage();
//ko.applyBindings({
//    koMesssages: vmMessage,
//    koPage: vmPage,
//    koTooltip: vmTooltip,
//    koLeftPage: vmLeftPage
//});
//$.connection.hub.start().done(function () {
//    vmPage.init();
//    vmTooltip.init();
//    vmLeftPage.init();
//});

try {
    var vmMessage = new VM_Message();
    var vmHeader = new VM_Header();
    var vmLeftPage = new VM_LeftPage();
    var vmPage = new VM_Page();
    var vmTooltip = new VM_Page();
    var vmPost = new VM_Post();
    ko.applyBindings({
        koMesssages: vmMessage,
        koHeader: vmHeader,
        koLeftPage: vmLeftPage,
        koPage: vmPage,
        koPost: vmPost,
        koTooltip: vmTooltip
    });
} catch (e) {
    ko.applyBindings({
        koMesssages: vmMessage,
        koHeader: vmHeader,
        koLeftPage: vmLeftPage,
        koPage: vmPage,
        koTooltip: vmTooltip
    });
}

$(document).ready(function () {
    vmPage.init();
    vmTooltip.init();
    vmLeftPage.init();
})

$.connection.hub.start().done(function () {
    vmLeftHome.initHub();
});