using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(QAConnect.Startup))]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Log4Net.config", Watch = true)]
namespace QAConnect
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
