using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ComicPresence.Web.Startup))]
namespace ComicPresence.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
