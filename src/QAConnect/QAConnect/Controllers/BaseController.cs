using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using BusinessLogic;
using C4Connect.Filters;

namespace C4Connect.Controllers
{

    [CompressFilter(Order = 1)]
    [CacheFilter(Duration = 60, Order = 2)]
    public class BaseController : Controller
    {
        public AppUserPrincipal CurrentUser
        {
            get
            {
                return new AppUserPrincipal(base.User as ClaimsPrincipal);
            }
        }

        protected PageAccess _page;

        public BaseController()
        {
            _page = new PageAccess();
        }

        [AllowAnonymous]
        public PartialViewResult AlertMessages()
        {
            return PartialView("AlertMessages", TempData["messages"]);
        }
        /// <summary>
        /// Clear Messages from Temp Data
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult ClearAlertMessages()
        {
            TempData.Remove("messages");
            TempData["messages"] = null;
            return new EmptyResult();
        }
        /// <summary>
        /// Display Error or Successfull Message on top of Page
        /// </summary>
        /// <param name="title"></param>
        /// <param name="type"></param>
        /// <param name="isDismissible"></param>
        [NonAction]
        protected void AddAlertMessage(string title, DataTypes.Enums.MessageType type, bool isDismissible = true)
        {
            IList<DataTypes.AlertMessages> _messages = null;
            if (TempData.ContainsKey("messages"))
            {
                _messages = TempData["messages"] as IList<DataTypes.AlertMessages>;
            }
            if (_messages == null)
            {
                _messages = new List<DataTypes.AlertMessages>();
            }
            if (!_messages.Any(msg => msg.Title.Equals(title, StringComparison.InvariantCultureIgnoreCase) && msg.Type == type))
            {
                _messages.Add(new DataTypes.AlertMessages { Title = title, Type = type, IsDismissible = isDismissible });
            }
            TempData["messages"] = _messages;
            TempData.Keep("messages");
        }
        /// <summary>
        /// Get Display Name of current Logged in User
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        /// <summary>
        /// Redirect to returnUrl during User Login Required or Session TimeOut
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [NonAction]
        protected ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        [NonAction]
        protected string BuildActionUrl(string relativePath)
        {
            return Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + relativePath;
        }

        [NonAction]
        protected string GetBaseUrl()
        {
            return Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/');
        }
    }
}