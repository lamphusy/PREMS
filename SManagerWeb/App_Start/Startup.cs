using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SManagerWeb.Startup))]
namespace SManagerWeb
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
