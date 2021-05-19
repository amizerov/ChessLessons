using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using am.BL;

namespace chess4.Models
{
    public class ALessons : List<ALesson>
    {
        public ALessons()
        {
            Update();
        }

        public void Update()
        {
            DataTable dt = G.db_select(@"
                select 0 ID, 'Новый урок' Name, -1 OrderNumb
                union all
                select ID, Name, IsNull(OrderNumb, 0) OrderNumb from Lesson order by OrderNumb
            ");

            foreach (DataRow r in dt.Rows)
            {
                ALesson les = new ALesson { ID = G._I(r["ID"]), Name = G._S(r["Name"]) };
                this.Add(les);
            }
        }
    }

    public class ALesson
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Position { get; set; }
        public string Orientation { get; set; }
        public int OrderNumb { get; set; }

        public bool Selected { get; set; }

        public ALesson() { }

        public ALesson(int Lesson_ID)
        {
            DataTable dt = G.db_select("select * from Lesson where ID = {1}", Lesson_ID);
            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                ID = G._I(r["ID"]);
                Name = G._S(r["Name"]);
                Description = G._S(r["Description"]);
                Position = G._S(r["Position"]);
                Orientation = G._S(r["Orientation"]);
                OrderNumb = G._I(r["OrderNumb"]);
            }
            if(Lesson_ID == 0) OrderNumb = G._I(G.db_select("select IsNull(max(OrderNumb), 0)+1 from Lesson"));
        }

        public int Save()
        {
            string sql = string.Format(@"
                update Lesson set Name = '{1}', Description = '{2}', Position = '{3}', Orientation = '{4}', OrderNumb = {5} where ID = {0}
                select {0}
            ", ID, Name, Description, Position, Orientation, OrderNumb);

            if (ID == 0)
                sql = string.Format(@"
                insert Lesson(Name, Description, Position, Orientation, OrderNumb) values('{0}', '{1}', '{2}', '{3}', {4})
                select @@IDENTITY
            ", Name, Description, Position, Orientation, OrderNumb);

            return ID = G._I(G.db_select(sql));
        }

        public void Delete()
        {
            G.db_select("delete Lesson where ID = {1}", ID);
        }
    }
}