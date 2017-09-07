using System.Web;
using System.Web.Optimization;

namespace C4Connect
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/base/js").Include(
                       "~/Scripts/jquery-{version}.js",
                       "~/Scripts/jquery-ui-{version}.min.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/jquery.validate.min.js",
                      "~/Scripts/jquery.validate.unobtrusive.min.js",
                //    "~/Scripts/bootstrap.validate.js",
                        "~/Scripts/knockout-3.3.0.js",
                       "~/Scripts/jquery.autosize-min.js",
                       "~/Scripts/Date.min.js",
                      "~/Scripts/bootstrap-multiselect.js",
                      "~/Scripts/jsonify.js",
                       "~/Scripts/jquery-custom.js",
                       "~/Scripts/plugins.js"
                       ));

            //bundles.Add(new ScriptBundle("~/bundles/common").Include(
            //           "~/Scripts/jquery-custom.js",
            //           "~/Scripts/plugins.js"));
            //,"~/Scripts/ko/messages.js"
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js",
            //          "~/Scripts/jquery.validate.unobtrusive.min.js",
            //           "~/Scripts/bootstrap.validate.js"));

            bundles.Add(new ScriptBundle("~/bundles/main/js").Include(
                "~/Scripts/select2.js",
                "~/Scripts/jquery.timeago.js",
                "~/Plugins/Powertip/jquery.powertip.min.js",
                "~/Plugins/MediaElementPlayer/build/mediaelement-and-player.js",
                 "~/Scripts/ko/user.js",
                "~/Scripts/ko/page.js",
                "~/Scripts/ko/header.js",
                "~/Scripts/ko/posts.js",
                "~/Scripts/ko/leftHome.js",
                "~/Scripts/ko/leftPage.js",
                "~/Scripts/ko/right.js",
                "~/Scripts/ko/messages.js",
               "~/Scripts/ko/general.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/chat/js").Include(
                "~/Plugins/Chatjs/js/jquery.autosize.js",
             "~/Plugins/Chatjs/js/jquery.chatjs.utils.js",
             "~/Plugins/Chatjs/js/jquery.chatjs.adapter.servertypes.js",
             "~/Plugins/Chatjs/js/jquery.chatjs.adapter.js",
             "~/Plugins/Chatjs/js/jquery.chatjs.adapter.signalr.js",
             "~/Plugins/Chatjs/js/jquery.chatjs.window.js",
             "~/Plugins/Chatjs/js/jquery.chatjs.messageboard.js",
             "~/Plugins/Chatjs/js/jquery.chatjs.userlist.js",
             "~/Plugins/Chatjs/js/jquery.chatjs.pmwindow.js",
             "~/Plugins/Chatjs/js/jquery.chatjs.friendswindow.js",
             "~/Plugins/Chatjs/js/jquery.chatjs.controller.js",
             "~/Plugins/ChatJs/js/jquery.twemoji.min.js"
             ));
            bundles.Add(new StyleBundle("~/bundles/font-awesome").Include(
                       "~/Content/font-awesome.min.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-multiselect.css",
                      "~/Content/base.css"));

            bundles.Add(new StyleBundle("~/bundles/mediaelementplayer/css").Include(
                  "~/Plugins/MediaElementPlayer/build/mediaelementplayer.min.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/Chat/css").Include(
                     "~/Plugins/Chatjs/css/jquery.chatjs.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/account").Include(
                   "~/Content/account.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/baseIn").Include(
                   "~/Content/baseIn.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/post").Include(
                    "~/Content/post.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/home").Include(
                    "~/Content/home.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/page").Include(
                    "~/Content/page.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/powertip").Include(
                    "~/Plugins/Powertip/css/jquery.powertip.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/select2").Include(
                    "~/Content/css/select2.css", new CssRewriteUrlTransform()));
        }
    }
}
