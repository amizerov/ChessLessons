using System.Collections.Generic;
using System.Data;
using am.BL;

namespace chess5.Models
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
                select ID, Name, Description, IsNull(OrderNumb, 0) OrderNumb, dtc from Lesson order by OrderNumb
            ");

            foreach (DataRow r in dt.Rows)
            {
                CLesson les = new CLesson
                {
                    ID = G._I(r["ID"]),
                    Name = G._S(r["Name"]),
                    Description = G._S(r["Description"]),
                    Date = G._S(r["dtc"]),
                    OrderNumb = G._I(r["OrderNumb"])
                };
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
        public string Orientation { get; set; }
        public int OrderNumb { get; set; }
        public bool Selected { get; set; }
        public string Date { get; set; }
        public int CoinCost { get; set; }
        public CTopics Topics
        {
            get
            {
                return new CTopics(ID);
            }
        }

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
                Orientation = G._S(r["Orientation"]);
                OrderNumb = G._I(r["OrderNumb"]);
                CoinCost = G._I(r["CoinCost"]);
                Date = G._S(r["dtc"]);
            }
            if (Position == "8/8/8/8/8/8/8/8" && Topics.Count > 0)
            {
                Position = Topics[0].Position;
                Orientation = Topics[0].Orientation;
            }
            if (Lesson_ID == 0) OrderNumb = G._I(G.db_select("select IsNull(max(OrderNumb), 0)+1 from Lesson"));
        }

        public bool CountUser()
        {
            if (CurrentUser.Account.ChessCoin < this.CoinCost) return false;

            string UserId = CurrentUser.ID; if (UserId.Length == 0) return false;

            DataTable dt = G.db_select($"select ID from UserLesson where User_ID = '{UserId}' and Lesson_ID = {this.ID}");
            int ul_id = G._I(dt);
            if (ul_id > 0)
            {
                G.db_exec($"update UserLesson set Count = Count+1, dtu=getdate() where ID = '{ul_id}'");
            }
            else
            {
                G.db_exec($"insert UserLesson(User_ID, Lesson_ID) values('{UserId}', {this.ID})");
                
                CurrentUser.Account.ChessCoin -= this.CoinCost;
            }
            return true;
        }
    }
}