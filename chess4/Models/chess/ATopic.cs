using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using am.BL;

namespace chess4.Models
{
    public class ATopics : List<ATopic>
    {
        public int Lesson_ID { get; set; }
        public ALesson Lesson { get { return new ALesson(Lesson_ID); } }

        public ATopics(){ }

        public ATopics(int les_id, int selTopic_ID) : this(les_id)
        {
            this.Find(t => t.ID == selTopic_ID).Selected = true;
        }

        public ATopics(int les_id)
        {
            Lesson_ID = les_id;
            Update();
        }

        public void Update()
        {
            DataTable dt = G.db_select(@"
                select 0 ID, 'Новое задание' Name, '' Description, 0 OrderNumb
                union all
                select ID, Name, Description, OrderNumb from Topic where Lesson_ID = {1} order by OrderNumb
            ", Lesson_ID);

            foreach (DataRow r in dt.Rows)
            {
                ATopic top = new ATopic(Lesson_ID, G._I(r["OrderNumb"])) { ID = G._I(r["ID"]), Name = G._S(r["Name"]), Description = G._S(r["Description"]) };
                this.Add(top);
            }

        }
    }

    public class ATopic : SelectListItem
    {
        public int ID { get; set; } = 0;
        public string Name { get; set; } = "";
        public string Position { get; set; } = "";
        public string Orientation { get; set; } = "";
        public string Description { get; set; } = "";
        public int Lesson_ID { get; set; } = 0;
        public int OrderNumb { get; set; }
        public int InArticle { get; set; }

        public ATopic() { }

        public ATopic(int Topic_ID, int pLesson_ID = 0)
        {
            Lesson_ID = pLesson_ID;

            DataTable dt = G.db_select("select * from Topic where ID = {1}", Topic_ID);
            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                ID = G._I(r["ID"]);
                Name = G._S(r["Name"]);
                Description = G._S(r["Description"]);
                Position = G._S(r["Position"]);
                Orientation = G._S(r["Orientation"]);
                OrderNumb = G._I(r["OrderNumb"]);
                InArticle = (bool)r["InArticle"] ? 1 : 0;

                if (Lesson_ID == 0 && Topic_ID > 0) Lesson_ID = G._I(r["Lesson_ID"]);
            }

            if(Topic_ID == 0)
                OrderNumb = G._I(G.db_select("select IsNull(max(OrderNumb), 0)+1 from Topic where Lesson_ID = {1}", Lesson_ID));
        }

        public int Save()
        {
            string sql = string.Format(@"
                update Topic set Name = '{1}', Description = '{2}', Position = '{3}', Orientation = '{4}', OrderNumb = {5}, InArticle = {6} where ID = {0}
                select {0}
            ", ID, Name, Description, Position, Orientation, OrderNumb, InArticle);


            if (ID == 0)
                sql = string.Format(@"
                    insert Topic(Name, Description, Position, Orientation, Lesson_ID, OrderNumb, InArticle) values('{0}', '{1}', '{2}', '{3}', {4}, {5}, {6})
                    select @@IDENTITY
                ", Name, Description, Position, Orientation, Lesson_ID, OrderNumb, InArticle);

            return ID = G._I(G.db_select(sql));
        }

        public void Delete()
        {
            G.db_select("delete Topic where ID = {1}", ID);
        }
    }
}