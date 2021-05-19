using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using am.BL;

namespace chess5
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            string cs = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            am.DB.DBManager.Instance.Init(cs);

        }

        protected void Session_Start(object sender, EventArgs e)
        {
            //обработка начала сессии
            if (Request.IsAuthenticated)//если надо понять залогинен ли
            {
                string name = Context.User.Identity.Name;
                G.db_exec("insert SessionLog(IsAuth, UserName, IsStartEnd, SessionID) values(1, '{1}', 1, '{2}')", name, Session.SessionID);
            }
            else
                G.db_exec("insert SessionLog(IsAuth, IsStartEnd, SessionID) values(0, 1, '{1}')", Session.SessionID);
        }

        protected void Session_End(object sender, EventArgs e)
        {
            //обработка конца сессии
            G.db_exec("insert SessionLog(IsAuth, IsStartEnd, SessionID) values(0, 0, '{1}')", Session.SessionID);
        }
    }
}
