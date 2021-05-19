using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(chess6.Startup))]
namespace chess6
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
