using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(chess4.Startup))]
namespace chess4
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
