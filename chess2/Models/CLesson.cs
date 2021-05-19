using System.Collections.Generic;
using System.Linq;
using System.Data;
using am.BL;

namespace chess2.Models
{
    public class CLessons : List<CLesson>
    {
        public CLessons()
        {
            Update();
        }

        public void Update()
        {
            DataTable dt = G.db_select(@"
                select ID, Name, IsNull(OrderNumb, 0) OrderNumb from Lesson order by OrderNumb
            ");

            foreach (DataRow r in dt.Rows)
            {
                CLesson les = new CLesson { ID = G._I(r["ID"]), Name = G._S(r["Name"]) };
                this.Add(les);
            }
        }
    }

    public class CLesson
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Position { get; set; }
        public int OrderNumb { get; set; }
        public CTopics Topics {
            get
            {
                return new CTopics(ID);
            }
        }

        public bool Selected { get; set; }

        public CLesson() { }

        public CLesson(int Lesson_ID)
        {
            DataTable dt = G.db_select("select * from Lesson where ID = {1}", Lesson_ID);
            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                ID = G._I(r["ID"]);
                Name = G._S(r["Name"]);
                Description = G._S(r["Description"]);
                Position = G._S(r["Position"]);
                OrderNumb = G._I(r["OrderNumb"]);
            }
            if (Lesson_ID == 0) OrderNumb = G._I(G.db_select("select IsNull(max(OrderNumb), 0)+1 from Lesson"));
        }
    }
}