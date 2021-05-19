using System.Data;
using am.BL;

namespace chess5.Models
{
    public class CStep
    {
        public int ID { get; set; } = 0;
        public string Name { get; set; } = "";
        public string Position { get; set; } = "";
        public string Description { get; set; } = "";
        public int Topic_ID { get; set; } = 0;
        public int OrderNumb { get; set; } = 0;
        public int NextTopicID { get; set; } = 0;
        public int IsLastStep { get; set; } = 0;

        public CStep(int topic_id, int stepNumber)
        {
            Topic_ID = topic_id;

            DataTable dt = G.db_select(@"
                select * from Step where Topic_ID = {1} and OrderNumb = {2} order by ID
            ", Topic_ID, stepNumber);

            int c = dt.Rows.Count;
            if (c > 0)
            {
                DataRow r = dt.Rows[0];
                ID = G._I(r["ID"]);
                Name = G._S(r["Name"]);
                Description = G._S(r["Description"]);
                Position = G._S(r["Position"]);
                OrderNumb = G._I(r["OrderNumb"]);

                dt = G.db_select("select max(OrderNumb) from Step where Topic_ID = {1}", Topic_ID);
                IsLastStep = G._I(dt) == stepNumber ? 1 : 0;
            }
            else
            {
                CTopic t = new CTopic(Topic_ID);
                dt = G.db_select(@"
                    select ID from Topic where Lesson_ID = {1} and OrderNumb = {2}
                ", t.Lesson_ID, t.OrderNumb + 1);
                NextTopicID = G._I(dt);
            }
        }

    }
}