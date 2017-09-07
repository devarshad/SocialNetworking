using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using C4Connect.Helpers;
using DataTypes;
using C4Connect.Filters;

namespace C4Connect.Controllers
{
    [CompressFilter(Order = 1)]
    [CacheFilter(Duration = 60, Order = 2)]
    [C4Connect.Helpers.MyAuthorize]
    public class PageController : BaseController
    {
        // GET: Page or Page/Index/name but by route it is: name
        public ActionResult Index(String name)
        {
            if (name.Equals(CurrentUser.UserName, StringComparison.InvariantCultureIgnoreCase))
                ViewBag.PageID = CurrentUser.UserID;
            else
            {
                long ID = _page.GetPageIDByName(name, Enums.PageType.Profile);
                ViewBag.PageID = ID;
            }
            ViewBag.PageType = (byte)Enums.PageType.Profile;
            return View();
        }

        // GET: Group/name
        public ActionResult Group(String name)
        {
            long ID = _page.GetPageIDByName(name, Enums.PageType.Group);
            ViewBag.PageID = ID;
            ViewBag.PageType = (byte)Enums.PageType.Group;
            return View();
        }

        // GET: Event/name
        public ActionResult Event(String name)
        {
            long ID = _page.GetPageIDByName(name, Enums.PageType.Event);
            ViewBag.PageID = ID;
            ViewBag.PageType = (byte)Enums.PageType.Event;
            return View();
        }

        //GET: Page/Message
        public ActionResult Message()
        {
            return View();
        }

        //GET: Page/Tooltip
        public ActionResult Tooltip()
        {
            return View();
        }

        public ActionResult EditAbout()
        {
            return View();
        }

        public ActionResult Groups()
        {
            ViewBag.PageID = CurrentUser.UserID;
            ViewBag.PageType = (byte)Enums.PageType.Profile;
            return View();
        }
        public ActionResult Events()
        {
            ViewBag.PageID = CurrentUser.UserID;
            ViewBag.PageType = (byte)Enums.PageType.Profile;
            return View();
        }

        public ActionResult About(long PageID, byte PageType)
        {
            ViewBag.PageID = PageID;
            ViewBag.PageType = PageType;
            return View();
        }

        public ActionResult Friends(long PageID, byte PageType)
        {
            ViewBag.PageID = PageID;
            ViewBag.PageType = PageType;
            return View();
        }

        public ActionResult Photos(long PageID, byte PageType)
        {
            ViewBag.PageID = PageID;
            ViewBag.PageType = PageType;
            return View();
        }

        public ActionResult Audios(long PageID, byte PageType)
        {
            ViewBag.PageID = PageID;
            ViewBag.PageType = PageType;
            return View();
        }

        public ActionResult Videos(long PageID, byte PageType)
        {
            ViewBag.PageID = PageID;
            ViewBag.PageType = PageType;
            return View();
        }
    }
}