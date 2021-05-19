using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(chess2.Startup))]
namespace chess2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
