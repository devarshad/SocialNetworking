using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace C4Connect.Helpers
{
    public class MyAuthorizeAttribute : AuthorizeAttribute
    {
        public string controller { get; set; }
        public string actionName { get; set; }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var rd = filterContext.RequestContext.RouteData;
            string currentAction = rd.GetRequiredString("action");
            string currentController = rd.GetRequiredString("controller");

            if (currentAction.Equals("Index", StringComparison.CurrentCultureIgnoreCase) && currentController.Equals("Home", StringComparison.CurrentCultureIgnoreCase))
            {
                var routeValues = new RouteValueDictionary(new
                {
                    controller = "Account",
                    action = "Index",
                });
                filterContext.Result = new RedirectToRouteResult(routeValues);
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
            //}
            //else
            //{
            //    base.HandleUnauthorizedRequest(filterContext);
            //}
        }

        //protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        //{
        //    //if (!httpContext.User.Identity.IsAuthenticated)
        //    //    return false;
        //    var rd = httpContext.Request.RequestContext.RouteData;
        //    string currentAction = rd.GetRequiredString("action");
        //    string currentController = rd.GetRequiredString("controller");
        //    httpContext.Response=new RedirectToRouteResult(new RouteValueDictionary(new
        //        {
        //            controller = "someController",
        //            action = "someAction",
        //        });
        //    return base.AuthorizeCore(httpContext);
        //}

    }
}