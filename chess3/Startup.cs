﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(chess5.Startup))]
namespace chess5
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
