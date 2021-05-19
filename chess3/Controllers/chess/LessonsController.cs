using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using chess5.Models;

namespace chess5.Controllers.chess
{
    public class LessonsController : Controller
    {
        // GET: Lessons
        public ActionResult Index()
        {
            CLessons model = new CLessons();
            return View(model);
        }

        public ActionResult Lesson(int ID)
        {
            ViewBag.ReturnUrl = "/Lessons/Lesson/" + ID;
            CLesson Lesson = new CLesson(ID);
            return View(Lesson);
        }

        public ActionResult Topic(int ID)
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