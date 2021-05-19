using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(chess.Client.Startup))]
namespace chess.Client
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
