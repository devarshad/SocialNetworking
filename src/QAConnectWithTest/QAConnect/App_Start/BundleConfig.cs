using System.Web;
using System.Web.Optimization;

namespace QAConnect
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //Start ==================> Base layout <==================
            //-----------------------Styles
            bundles.Add(new StyleBundle("~/Content/cBase").Include(
                        "~/Content/bootstrap.css",
                        "~/Content/font-awesome.css",
                        "~/Content/base.css"
                ));
            //-----------------------Scripts
            bundles.Add(new ScriptBundle("~/bundles/jBase").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-1.11.4.min.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/respond.js",
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/jquery.validate.unobtrusive.min.js",
                        "~/Scripts/jquery.validate.custom.js",
                        "~/Scripts/knockout-3.3.0.js",
                        "~/Scripts/ko/messages.js"));
            //End ==================> Base layout <==================

            //Start ==================> Account layout <==================
            //-----------------------Styles
            bundles.Add(new StyleBundle("~/Content/cAccount").Include(
                        "~/Content/account.css"));
            //-----------------------Scripts
            bundles.Add(new ScriptBundle("~/bundles/jAccount").Include(
                        "~/Scripts/ko/account.js"));
            //End ==================> Account layout <==================

            //Start ==================> In layout <==================
            //-----------------------Styles
            bundles.Add(new StyleBundle("~/Content/cBaseIn").Include(
                        "~/Content/baseIn.css", new CssRewriteUrlTransform())
                                                            .Include(
                        "~/Content/css/select2.css", new CssRewriteUrlTransform())
                                                            .Include(
                        "~/Plugins/Chatjs/css/jquery.chatjs.css", new CssRewriteUrlTransform()));
            //-----------------------Scripts
            bundles.Add(new ScriptBundle("~/bundles/jBaseIn").Include(
                        "~/Scripts/jquery.autosize-min.js",
                        "~/Scripts/Date.min.js",
                        "~/Scripts/bootstrap-multiselect.js",
                        "~/Scripts/jsonify.js",
                        "~/Scripts/jquery.timeago.js",
                        "~/Scripts/ko/header.js",
                        "~/Scripts/ko/right.js",
                        "~/Scripts/ko/page.js",
                        "~/Scripts/jquery-custom.js",
                        "~/Scripts/plugins.js"));

            bundles.Add(new ScriptBundle("~/bundles/chat").Include(
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
            //End ==================> In layout <==================

            //Start ==================> Home layout <==================
            //-----------------------Styles
            bundles.Add(new StyleBundle("~/Content/home").Include(
                    "~/Content/home.css", new CssRewriteUrlTransform()));
            //-----------------------Scripts
            bundles.Add(new ScriptBundle("~/bundles/home").Include(
                        "~/Scripts/ko/leftHome.js"));
            //End ==================> Home layout <==================

            //Start ==================> Page layout <==================
            //-----------------------Styles
            bundles.Add(new StyleBundle("~/Content/page").Include(
                        "~/Content/page.css", new CssRewriteUrlTransform()));
            //-----------------------Scripts
            bundles.Add(new ScriptBundle("~/bundles/page").Include(
                        "~/Scripts/ko/leftPage.js"));
            //End ==================> page layout <==================

            //Start ==================> Post layout <==================
            //-----------------------Styles
            bundles.Add(new StyleBundle("~/Content/post").Include(
                        "~/Content/post.css", new CssRewriteUrlTransform())
                                                        .Include(
                        "~/Plugins/Powertip/css/jquery.powertip.css", new CssRewriteUrlTransform())
                                                        .Include(
                        "~/Plugins/MediaElementPlayer/build/mediaelementplayer.min.css", new CssRewriteUrlTransform()));
            //-----------------------Scripts
            bundles.Add(new ScriptBundle("~/bundles/post").Include(
                        "~/Scripts/ko/posts.js",
                       "~/Plugins/Powertip/jquery.powertip.min.js",
                       "~/Plugins/MediaElementPlayer/build/mediaelement-and-player.js"));
            //End ==================> Post layout <==================
        }
    }
}
