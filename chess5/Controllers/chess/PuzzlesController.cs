using System.Web.Mvc;
using chess5.Models;

namespace chess5.Controllers.chess
{
    public class PuzzlesController : Controller
    {
        // GET: Puzzles
        public ActionResult Index()
        {
            PTemas model = new PTemas();
            return View(model);
        }

        public ActionResult PTema(int ID)
        {
            Puzzles ps = new Puzzles(ID);
            if (ps.Count > 0)
            {
                ViewBag.Puzzle = ps.FirstNotSolvedBy(CurrentUser.ID);
            }
            return View("Puzzle");
        }

        public ActionResult Puzzle(int ID)
        {
            ViewBag.Puzzle = new Puzzle(ID);

            return View();
        }

        public JsonResult GetPStep(int Puzzle_ID, int stepNumber)
        {
            PStep step = new PStep(Puzzle_ID, stepNumber);

            return Json(step, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPMoveResult(string new_position_after_move, int Step_ID)
        {
            new_position_after_move = new_position_after_move.Split(' ')[0];
            PMoveResult res = new PMoveResult(Step_ID, new_position_after_move);

            return Json(res, JsonRequestBehavior.AllowGet);
        }

    }
}