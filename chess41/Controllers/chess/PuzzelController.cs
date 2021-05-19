using System.Web.Mvc;
using chess41.Models;

namespace chess41.Controllers.chess
{
    public class PuzzelController : Controller
    {
        #region PTema
        [Authorize]
        public ActionResult PTema(PTema tem)
        {
            if (tem.Name == "Delete") tem.Delete();
            else
                if (tem.Name != null) tem.Save();

            ViewBag.PTemas = new SelectList(new PTemas(), "ID", "Name");
            return View();
        }

        public JsonResult GetPTemaDetails(int PTema_ID)
        {
            PTema t = new PTema(PTema_ID);

            return Json(t, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Puzzle
        [HttpGet]
        public ActionResult Puzzle(int ID)
        {
            Puzzles model = null;

            var Tema_ID = ID;
            if (ID > 999 && ID % 1000 == 0) {
                Puzzle p = new Puzzle(ID / 1000);
                Tema_ID = p.Tema_ID;
                model = new Puzzles(Tema_ID, p.ID);
            }
            else
                model = new Puzzles(Tema_ID);

            return View(model);
        }

        [HttpPost]
        public ActionResult Puzzle(Puzzle puz)
        {
            if (puz.Name == "Delete") puz.Delete();
            else
                if (puz.Name != null) puz.Save();

            var model = new Puzzles(puz.Tema_ID);
            return View(model);
        }

        public JsonResult GetPuzzleDetails(int Puzzle_ID, int Tema_ID)
        {
            Puzzle p = new Puzzle(Puzzle_ID, Tema_ID);

            return Json(p, JsonRequestBehavior.AllowGet);
        }

        public JsonResult MakePuzzleOfDay(int Puzzle_ID, int Tema_ID)
        {
            Puzzle p = new Puzzle(Puzzle_ID, Tema_ID);
            int res = p.MakePuzzleOfDay();
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PStep
        [HttpGet]
        public ActionResult PStep(int ID)
        {
            PSteps model = null;

            var Puzzle_ID = ID;
            if (ID > 999 && ID % 1000 == 0) {
                PStep s = new PStep(ID / 1000);
                Puzzle_ID = s.Puzzle_ID;
                model = new PSteps(Puzzle_ID, s.ID);
            }
            else
                model = new PSteps(Puzzle_ID);

            return View(model);
        }

        [HttpPost]
        public ActionResult PStep(PStep step)
        {
            if (step.Name == "Delete") step.Delete();
            else
                if (step.Name != null) step.Save();

            var model = new PSteps(step.Puzzle_ID);
            return View(model);
        }

        public JsonResult GetStepDetails(int Step_ID, int Puzzle_ID)
        {
            PStep s = new PStep(Step_ID, Puzzle_ID);

            return Json(s, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PMove
        [HttpGet]
        public ActionResult PMove(int ID)
        {
            var Step_ID = ID;
            var model = new PMoves(Step_ID);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PMove(PMove move)
        {
            if (move.Name == "Delete") move.Delete();
            else
                if (move.Name != null) move.Save();

            var model = new PMoves(move.Step_ID);
            return View(model);
        }

        public JsonResult GetMoveDetails(int Move_ID, int Step_ID)
        {
            PMove m = new PMove(Move_ID, Step_ID);

            return Json(m, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}