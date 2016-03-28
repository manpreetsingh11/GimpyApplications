using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GimpySoftwareNewTemplate.Startup))]
namespace GimpySoftwareNewTemplate
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
