using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(C4Connect.Startup))]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Log4Net.config", Watch = true)]
namespace C4Connect
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
