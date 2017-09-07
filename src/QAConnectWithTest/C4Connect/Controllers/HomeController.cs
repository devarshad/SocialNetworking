using C4Connect.Filters;
using DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace C4Connect.Controllers
{
    [CompressFilter(Order = 1)]
    [CacheFilter(Duration = 60, Order = 2)]
    [C4Connect.Helpers.MyAuthorize]
    public class HomeController : BaseController
    {
        [C4Connect.Helpers.MyAuthorize(actionName = "Index", controller = "HomeController")]
        public ActionResult Index()
        {
            ViewBag.Home = true;
            ViewBag.PageID = CurrentUser.UserID;
            ViewBag.PageType = 1;

            return View();
        }

        public ActionResult Messages()
        {
            ViewBag.PageID = CurrentUser.UserID;
            ViewBag.PageType = 1;
            return View();
        }

        public ActionResult FriendRequests()
        {
            ViewBag.PageID = CurrentUser.UserID;
            ViewBag.PageType = 1;
            return View();
        }

        public ActionResult Notifications()
        {
            ViewBag.PageID = CurrentUser.UserID;
            ViewBag.PageType = 1;
            return View();
        }

        public ActionResult Search()
        {
            ViewBag.PageID = CurrentUser.UserID;
            ViewBag.PageType = 1;
            return View();
        }

        public ActionResult SearchFriends()
        {
            ViewBag.PageID = CurrentUser.UserID;
            ViewBag.PageType = 1;
            return View();
        }

        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public async Task<ActionResult> EditProfile()
        {
            ViewBag.PageID = CurrentUser.UserID;
            ViewBag.PageType = (byte)DataTypes.Enums.PageType.Profile;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GetBaseUrl());
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + CurrentUser.APIToken);
                // New code:
                HttpResponseMessage response = await client.GetAsync("/api/Page/GetPageTimeline/?PageType= " + (byte)DataTypes.Enums.PageType.Profile + "&PageID=" + CurrentUser.UserID);
                if (response.IsSuccessStatusCode)
                {
                    var _model = await response.Content.ReadAsAsync<DataTypes.AboutPageModel>();
                    return View(_model);
                }
            }
            return View();
        }
    }
}
