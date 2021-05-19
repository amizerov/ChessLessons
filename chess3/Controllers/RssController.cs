﻿using chess5.Models;
using System;
using System.Data;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using am.BL;

namespace chess5.Controllers
{
    public class RssController : Controller
    {
        // GET: Rss
        public ActionResult Index()
        {
            var items = new List<SyndicationItem>();
            string sql = @"
			    select g.ID, u.Email from Game g
			    join AspNetUsers u on u.Id in (White_ID, Black_ID)
			    where (White_ID is null or Black_ID is null) and IsActive = 1
			    order by g.dtc desc
            ";
            DataTable dt = G.db_select(sql);
            //if(dt.Rows.Count == 0) dt = G.db_select("select 0 ID, 'andrey@mizerov.com' Email");
            foreach (DataRow r in dt.Rows)
            {
                string user = G._S(r["Email"]);
                int Game_ID = G._I(r["ID"]);

                string feedTitle = "Новая игра";

                //var helper = new UrlHelper(this.Request.RequestContext);
                //var url = helper.Action("Index", "Game", new { }, Request.IsSecureConnection ? "https" : "http");
                var url = "https://chesslessons.ru/Game/Human/" + Game_ID;

                var feedPackageItem = new SyndicationItem(feedTitle, user + " предлагает сыграть", new Uri(url));
                feedPackageItem.PublishDate = DateTime.Now;
                items.Add(feedPackageItem);
            }
            return new RssResult("Новая игра на ChessLessons.ru", items);
        }

    }
}