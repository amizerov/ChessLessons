using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using am.BL;

namespace chess2.Models
{
    public class CTopics : List<CTopic>
    {
        public int Lesson_ID;

        public CTopics(int lesson_id)
        {
            Lesson_ID = lesson_id;
            Update();
        }

        public void Update()
        {
            DataTable dt = G.db_select(@"
                select ID, Name, IsNull(OrderNumb, 0) OrderNumb from Topic 
                where Lesson_ID = {1}
                order by OrderNumb 
            ", Lesson_ID);

            foreach (DataRow r in dt.Rows)
            {
                CTopic top = new CTopic { ID = G._I(r["ID"]), Name = G._S(r["Name"]) };
                this.Add(top);
            }
        }
    }

    public class CTopic
    {
        public int ID { get; set; } = 0;
        public string Name { get; set; } = "";
        public string Position { get; set; } = "";
        public string Orientation { get; set; } = "";
        public string Description { get; set; } = "";
        public int Lesson_ID { get; set; } = 0;
        public int OrderNumb { get; set; }

        public CTopic() { }

        public CTopic(int id)
        {
            DataTable dt = G.db_select("select * from Topic where ID = {1}", id);
            foreach(DataRow r in dt.Rows)
            {
                ID = G._I(r["ID"]);
                Name = G._S(r["Name"]);
                Description = G._S(r["Description"]);
                Orientation = G._S(r["Orientation"]);
                Position = G._S(r["Position"]);
                OrderNumb = G._I(r["OrderNumb"]);
                Lesson_ID = G._I(r["Lesson_ID"]);
            }
        }

        public CTopic(int Lesson_ID, int topicNumber)
        {
            DataTable dt = G.db_select(@"
                select * from Topic where Lesson_ID = {1} and OrderNumb = {2} order by ID
            ", Lesson_ID, topicNumber);

            int c = dt.Rows.Count;
            if (c > 0)
            {
                DataRow r = dt.Rows[0];
                ID = G._I(r["ID"]);
                Name = G._S(r["Name"]);
                Description = G._S(r["Description"]);
                Position = G._S(r["Position"]);
                Orientation = G._S(r["Orientation"]);
                OrderNumb = G._I(r["OrderNumb"]);
                Lesson_ID = G._I(r["Lesson_ID"]);
            }
        }
    }
}