using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(chess41.Startup))]
namespace chess41
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
