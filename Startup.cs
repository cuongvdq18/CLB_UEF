using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ClubPortalMS.Startup))]
namespace ClubPortalMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

    }
}
