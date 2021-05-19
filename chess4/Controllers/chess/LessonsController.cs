using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using chess4.Models;

namespace chess4.Controllers.chess
{
    public class LessonsController : Controller
    {
        #region Lesson
        [Authorize]
        public ActionResult Lesson(ALesson les)
        {
            if (!CheckRole("AdminLesson")) return null;

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
            if (!CheckRole("AdminLesson")) return null;

            ATopics model = null;

            var Lesson_ID = ID;
            if (ID > 999 && ID % 1000 == 0) {
                ATopic t = new ATopic(ID / 1000);
                Lesson_ID = t.Lesson_ID;
                model = new ATopics(Lesson_ID, t.ID);
            }
            else
                model = new ATopics(Lesson_ID);

            return View(model);
        }

        [HttpPost]
        public ActionResult Topic(ATopic top)
        {
            if (!CheckRole("AdminLesson")) return null;

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
            if (!CheckRole("AdminLesson")) return null;

            ASteps model = null;

            var Topic_ID = ID;
            if (ID > 999 && ID % 1000 == 0) {
                AStep s = new AStep(ID / 1000);
                Topic_ID = s.Topic_ID;
                model = new ASteps(Topic_ID, s.ID);
            }
            else
                model = new ASteps(Topic_ID);

            return View(model);
        }

        [HttpPost]
        public ActionResult Step(AStep step)
        {
            if (!CheckRole("AdminLesson")) return null;

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
            if (!CheckRole("AdminLesson")) return null;

            var Step_ID = ID;
            var model = new AMoves(Step_ID);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Move(AMove move)
        {
            if (!CheckRole("AdminLesson")) return null;

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

        bool CheckRole(string role)
        {
            bool r = true;

            if (User.IsInRole("Admin"))
                am.BL.G.db_exec($"insert clog(src, msg, ip) values('Admin Lesson', 'User = {User.Identity.Name} InRole Admin', '{Request.UserHostAddress}')");
            else if (User.IsInRole(role))
                am.BL.G.db_exec($"insert clog(src, msg, ip) values('Admin Lesson', 'User = {User.Identity.Name} InRole {role}', '{Request.UserHostAddress}')");
            else
            {
                am.BL.G.db_exec($"insert clog(src, msg, ip) values('Admin Lesson', 'User {User.Identity.Name} is NOT admin', '{Request.UserHostAddress}')");
                r = false;
            }

            return r;
        }
    }
}