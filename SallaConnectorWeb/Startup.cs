using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SallaConnectorWeb.Startup))]
namespace SallaConnectorWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
