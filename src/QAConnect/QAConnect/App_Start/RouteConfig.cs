using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace C4Connect
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            routes.MapRoute("Home", "", new { controller = "Home", action = "Index" });

            routes.MapRoute("message", "Messages", new { controller = "Home", action = "Messages" });

            routes.MapRoute("notification", "Notifications", new { controller = "Home", action = "Notifications" });

            routes.MapRoute("friendrequest", "FriendRequests", new { controller = "Home", action = "FriendRequests" });

            routes.MapRoute("search", "Search", new { controller = "Home", action = "Search" });

            routes.MapRoute("editProfile", "EditProfile", new { controller = "Home", action = "EditProfile" });

            routes.MapRoute("searchFriends", "SearchFriends", new { controller = "Home", action = "SearchFriends" });

            routes.MapRoute("about", "About/{PageID}/{PageType}", new { controller = "Page", action = "About", PageID = UrlParameter.Optional, PageType = UrlParameter.Optional });

            routes.MapRoute("groups", "Groups", new { controller = "Page", action = "Groups" });

            routes.MapRoute("events", "Events", new { controller = "Page", action = "Events" });

            routes.MapRoute("friends", "Friends/{PageID}/{PageType}", new { controller = "Page", action = "Friends", PageID = UrlParameter.Optional, PageType = UrlParameter.Optional });

            routes.MapRoute("photos", "Photos/{PageID}/{PageType}", new { controller = "Page", action = "Photos", PageID = UrlParameter.Optional, PageType = UrlParameter.Optional });

            routes.MapRoute("audios", "Audios/{PageID}/{PageType}", new { controller = "Page", action = "Audios", PageID = UrlParameter.Optional, PageType = UrlParameter.Optional });

            routes.MapRoute("videos", "Videos/{PageID}/{PageType}", new { controller = "Page", action = "Videos", PageID = UrlParameter.Optional, PageType = UrlParameter.Optional });

            routes.MapRoute("Account", "Account", new { controller = "Account", action = "Index" });

            routes.MapRoute("Named", "{name}", new { controller = "Page", action = "Index" });

            routes.MapRoute("Group", "Group/{name}", new { controller = "Page", action = "Group", name = UrlParameter.Optional });

            routes.MapRoute("Event", "Event/{name}", new { controller = "Page", action = "Event", name = UrlParameter.Optional });

            routes.MapRoute("GroupCreate", "Group/Create", new { controller = "Group", action = "Create", name = UrlParameter.Optional });

            routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}
