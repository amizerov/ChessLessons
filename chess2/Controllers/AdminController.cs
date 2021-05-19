using System.Web.Mvc;
using chess2.Models;

namespace chess2.Controllers
{
    public class AdminController : Controller
    {
        #region Lesson
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Lesson(ALesson les)
        {
            if (les.Name == "Delete") les.Delete();
            else
                if (les.Name != null) les.Save();

            ViewBag.Lessons = new SelectList(new ALessons(), "ID", "Name");
            return View();
        }

        public JsonResult GetLessonDetails(int Lesson_ID)
        {
            ALesson l = new ALesson(Lesson_ID);

            return Json(l, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Topic
        [HttpGet]
        public ActionResult Topic(int ID)
        {
            ATopics model = null;

            var Lesson_ID = ID;
            if(ID > 999) { ATopic t = new ATopic(ID / 1000); Lesson_ID = t.Lesson_ID; model = new ATopics(Lesson_ID, t.ID); }
            else
                model = new ATopics(Lesson_ID);

            return View(model);
        }

        [HttpPost]
        public ActionResult Topic(ATopic top)
        {
            if (top.Name == "Delete") top.Delete();
            else
                if (top.Name != null) top.Save();

            var model = new ATopics(top.Lesson_ID);
            return View(model);
        }

        public JsonResult GetTopicDetails(int Topic_ID, int Lesson_ID)
        {
            ATopic t = new ATopic(Topic_ID, Lesson_ID);

            return Json(t, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Step
        [HttpGet]
        public ActionResult Step(int ID)
        {
            ASteps model = null;

            var Topic_ID = ID;
            if (ID > 999) { AStep s = new AStep(ID / 1000); Topic_ID = s.Topic_ID; model = new ASteps(Topic_ID, s.ID); }
            else
                model = new ASteps(Topic_ID);

            return View(model);
        }

        [HttpPost]
        public ActionResult Step(AStep step)
        {
            if (step.Name == "Delete") step.Delete();
            else
                if (step.Name != null) step.Save();

            var model = new ASteps(step.Topic_ID);
            return View(model);
        }

        public JsonResult GetStepDetails(int Step_ID, int Topic_ID)
        {
            AStep s = new AStep(Step_ID, Topic_ID);

            return Json(s, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Move
        [HttpGet]
        public ActionResult Move(int ID)
        {
            var Step_ID = ID;
            var model = new AMoves(Step_ID);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Move(AMove move)
        {
            if (move.Name == "Delete") move.Delete();
            else
                if (move.Name != null) move.Save();

            var model = new AMoves(move.Step_ID);
            return View(model);
        }

        public JsonResult GetMoveDetails(int Move_ID, int Step_ID)
        {
            AMove m = new AMove(Move_ID, Step_ID);

            return Json(m, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}