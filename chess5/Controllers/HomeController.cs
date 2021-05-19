using System;
using System.Web.Mvc;
using chess5.Models;

namespace chess5.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            News model = new News();
            CPerson p = CurrentUser.Person;

            string un = p.FirstName + " " + p.LastName;
            if (un.Length == 1)
                un = CurrentUser.Login + " <a href='/Manage' style='color:white'>Заполни профиль и получи еще коины!</a>";
            else
                un += " (<a href='/Manage' style='color:white'>" + CurrentUser.Login + "</a>)";

            ViewBag.FirstName = p.FirstName;
            ViewBag.LastName = p.LastName;
            ViewBag.UserName = un;
            ViewBag.Avatar = p.Avatar;

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult CheckForNewGame()
        {
            var res = new NewGame();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

    }
}