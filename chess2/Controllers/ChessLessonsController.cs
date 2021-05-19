using System.Web.Mvc;
using chess2.Models;

namespace chess2.Controllers
{
    public class ChessLessonsController : Controller
    {
        public ActionResult Index()
        {
            CLessons model = new CLessons();
            return View(model);
        }

        [HttpGet]
        public ActionResult Topics(int ID /*Lesson ID*/)
        {
            CLesson l = new CLesson(ID);

            return View(l);
        }

        [HttpGet]
        public ActionResult Steps(int ID /*Topic ID*/)
        {
            CTopic t = new CTopic(ID);

            return View(t);
        }

    }
}