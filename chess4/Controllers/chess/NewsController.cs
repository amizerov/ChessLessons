using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using chess4.Models;

namespace chess4.Controllers.chess
{
    public class NewsController : Controller
    {
        // GET: News
        public ActionResult Index()
        {
            return View(new CNewsList());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(CNews news)
        {
            if (news.Title == "Delete") news.Delete();
            else
                if (news.Title != null) news.Save();

            return View(new CNewsList());
        }

        public JsonResult GetNewsDetails(int News_ID)
        {
            CNews n = new CNews(News_ID);

            return Json(n, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadImage()
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname;

                        fname = file.FileName;
                        fname = Path.Combine(Server.MapPath("~/img/news/"), fname);
                        file.SaveAs(fname);
                        fname = fname.Replace("chess4", "chess3");
                        file.SaveAs(fname);
                        fname = fname.Replace("chess3", "chess5");
                        file.SaveAs(fname);
                    }
                    return Json("OK");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }
    }
}