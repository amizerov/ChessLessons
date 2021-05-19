using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Mvc;
using chess5.Models;
using am.BL;

namespace chess5.Controllers.chess
{
    public class NewsController : Controller
    {
        // GET: News
        public ActionResult Index()
        {
            int p = G._I(Request["page"]);

            return View(new CNewsOfChess(p));
        }
        public ActionResult Item(int id)
        {
            CNewsOfChessItem itm = new CNewsOfChessItem(id);
            return View(itm);
        }
        [HttpPost]
        public ActionResult Item(CNewsOfChessItem n)
        {
            CNewsOfChessItem itm = new CNewsOfChessItem(n.ID);
            string msg = G._S(Request["message"]);
            if (msg.Length > 0)
            {
                itm.AddNewComment(msg);
            }
            return View(itm);
        }
    }
}