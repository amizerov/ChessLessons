using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using am.BL;

namespace chess4.Models
{
    public class CNews
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string TextHtml { get; set; }
        public string ImgPath { get; set; }
        public int OrderNumb { get; set; }
        public CNews() { }
        public CNews(int id)
        {
            DataTable dt = G.db_select("select * from News where ID = {1}", id);
            foreach (DataRow r in dt.Rows)
            {
                ID = G._I(r["ID"]);
                Title = G._S(r["Title"]);
                ImgPath = G._S(r["ImgPath"]);
                TextHtml = G._S(r["TextHtml"]);
                OrderNumb = G._I(r["OrderNumb"]);
            }
            if (ID == 0) OrderNumb = G._I(G.db_select("select IsNull(max(OrderNumb), 0)+1 from News"));
        }
        public int Save()
        {
            string sql = string.Format(@"
                update News set Title = '{1}', TextHtml = '{2}', ImgPath = '{3}', OrderNumb = {4} where ID = {0}
                select {0}
            ", ID, Title, TextHtml.Replace("'", "''"), ImgPath, OrderNumb);

            if (ID == 0)
                sql = string.Format(@"
                insert News(Title, TextHtml, ImgPath, OrderNumb) values('{0}', '{1}', '{2}', {3})
                select @@IDENTITY
            ", Title, TextHtml.Replace("'", "''"), ImgPath, OrderNumb);

            return ID = G._I(G.db_select(sql));
        }
        public void Delete()
        {
            G.db_select("delete News where ID = {1}", ID);
        }
    }

    public class CNewsList : List<CNews>
    {
        public CNewsList()
        {
            Update();
        }
        void Update()
        {
            DataTable dt = G.db_select(@"
                select 0 ID, 'Новый слайд' Title, -1 OrderNumb
                union all
                select ID, Title, IsNull(OrderNumb, 0) OrderNumb from News order by OrderNumb
            ");

            foreach (DataRow r in dt.Rows)
            {
                CNews n = new CNews { ID = G._I(r["ID"]), Title = G._S(r["Title"]) };
                this.Add(n);
            }
        }
    }
}