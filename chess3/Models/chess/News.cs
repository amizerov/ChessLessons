using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using am.BL;

namespace chess5.Models
{
    public class News
    {
        public int LastChessNewsID { get; set; }
        public string Title { get; set; }
        public string ImgPath { get; set; }
        public string TextHtml { get; set; }
        public CLesson LastLesson
        {
            get
            {
                int id = G._I(G.db_select("select top 1 ID from Lesson where len(Description) > 0 order by dtc desc"));
                return new CLesson(id);
            }
        }

        public CNewPuzzle LastPuzzle
        {
            get
            {
                return new CNewPuzzle();
            }
        }

        public CPuzzleOfDay PuzzleOfDay
        {
            get
            {
                return new CPuzzleOfDay();
            }
        }

        public int RegisteredUsersCount
        {
            get
            {
                int ruc = G._I(G.db_select("select count(*) from AspNetUsers"));
                return ruc;
            }
        }

        public Statistic Stat
        {
            get
            {
                return new Statistic();
            }
        }

        public News()
        {
            DataTable dt = G.db_select("select top 1 * from News order by OrderNumb desc");
            foreach (DataRow r in dt.Rows)
            {
                LastChessNewsID = G._I(r["ID"]);
                Title = G._S(r["Title"]);
                ImgPath = G._S(r["ImgPath"]);
                TextHtml = G._S(r["TextHtml"]);
            }
        }
    }
    public class CNewsItemComment
    { 
        public int ID { get; set; }
        public CPerson Author { get; set; }
        public string Message { get; set; }
        public string Date { get; set; }
    }
    public class CNewsItemComments : List<CNewsItemComment>
    {
        public CNewsItemComments(int news_id)
        {
            string sql = $"select * from NewsComment where News_ID = {news_id} and IsDeleted = 0 order by dtc desc";
            DataTable dtCom = G.db_select(sql);
            foreach (DataRow rCom in dtCom.Rows)
            {
                CNewsItemComment com = new CNewsItemComment();
                com.ID = G._I(rCom["ID"]);
                com.Author = new CPerson(G._S(rCom["User_ID"]));
                com.Message = G._S(rCom["Message"]);
                com.Date = G._D(rCom["dtc"]).ToString("dd.MM.yyyy");

                this.Add(com);
            }
        }
    }
    public class CNewsOfChessItem
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string ImgPath { get; set; }
        public string Text { get; set; }
        public string Date { get; set; }
        public CNewsItemComments Comments { get; set; }

        public CNewsOfChessItem()
        { }
        public CNewsOfChessItem(int id)
        {
            string sql = "select * from News where ID = " + id;
            DataTable dt = G.db_select(sql);
            foreach (DataRow r in dt.Rows)
            {
                ID = G._I(r["ID"]);
                Title = G._S(r["Title"]);
                ImgPath = G._S(r["ImgPath"]);
                Text = G._S(r["TextHtml"]);
                Date = G._D(r["dtc"]).ToString("dd.MM.yyyy");

                Comments = new CNewsItemComments(ID);
            }
        }
        public void AddNewComment(string message)
        {
            G.db_exec("insert NewsComment(News_ID, User_Id, Message) values({1}, '{2}', '{3}')",
                                          this.ID, CurrentUser.ID, message);
            if (G.LastError.Length == 0)
            {
                CNewsItemComment c = new CNewsItemComment()
                {
                    Author = CurrentUser.Person,
                    Message = message,
                    Date = DateTime.Now.ToString("dd.MM.yyyy")
                };
                Comments.Add(c);
            }
        }

    }
    public class CNewsOfChess : List<CNewsOfChessItem>
    {
        int _items_on_page = 4;
        public int NumberOfPages { get; set; }
        public int CurrentPage { get; set; }
        public CNewsOfChess(int page_number /*0-based page number*/)
        {
            CurrentPage = page_number;
            string sql = @"
                select * from News 
                order by OrderNumb desc 
                offset " + (_items_on_page * CurrentPage) + @" rows
                FETCH NEXT " + _items_on_page  + @" ROWS ONLY
            ";
            DataTable dt = G.db_select(sql);
            foreach (DataRow r in dt.Rows)
            {
                CNewsOfChessItem itm = new CNewsOfChessItem()
                {
                    ID = G._I(r["ID"]),
                    Title = G._S(r["Title"]),
                    ImgPath = G._S(r["ImgPath"]),
                    Text = G._S(r["TextHtml"]),
                    Date = G._D(r["dtc"]).ToString("dd.MM.yyyy")
                };
                Add(itm);
            }
            int c = G._I(G.db_select("select count(*) from News"));
            NumberOfPages = c / _items_on_page + (c % _items_on_page > 0 ? 1 : 0);
        }
    }


}