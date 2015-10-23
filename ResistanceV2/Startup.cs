using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ResistanceV2.Startup))]
namespace ResistanceV2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
