using System.Web.Mvc;
using chess2.Models;

namespace chess2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Tennis()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult ChessL(int ID /*Задание*/)
        {            
            ViewBag.Topic = new CTopic(ID);

            return View();
        }

        public JsonResult GetStep(int Topic_ID, int stepNumber)
        {
            CStep step = new CStep(Topic_ID, stepNumber);

            return Json(step, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTopic(int Lesson_ID, int topicNumber)
        {
            CTopic topic = new CTopic(Lesson_ID, topicNumber);

            return Json(topic, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMoveResult(string new_position_after_move, int Step_ID)
        {
            new_position_after_move = new_position_after_move.Split(' ')[0];
            MoveResult res = new MoveResult(Step_ID, new_position_after_move);

            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}