using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using am.BL;

namespace chess4.Models
{
    public class ASteps : List<AStep>
    {
        public int Topic_ID { get; set; }
        public ATopic Topic { get { return new ATopic(Topic_ID); } }
        public string Last_Position { get; set; }

        public ASteps() { }

        public ASteps(int top_id, int selStep_ID) : this(top_id)
        {
            this.Find(s => s.ID == selStep_ID).Selected = true;
        }

        public ASteps(int top_id)
        {
            Topic_ID = top_id;

            int last_step_id = G._I(G.db_select("select top 1 ID from Step where Topic_ID = {1} order by OrderNumb desc", Topic_ID));
            if (last_step_id > 0)
            {
                int last_move_id = G._I(G.db_select("select top 1 ID from Move where Step_ID = {1} order by OrderNumb desc", last_step_id));
                if (last_move_id > 0)
                    Last_Position = G._S(G.db_select("select Position from Move where ID = {1}", last_move_id));
                else
                    Last_Position = G._S(G.db_select("select Position from Step where ID = {1}", last_step_id));
            }
            else
                Last_Position = G._S(G.db_select("select Position from Topic where ID = {1}", Topic_ID));


            Update();
        }

        public void Update()
        {
            DataTable dt = G.db_select(@"
                select 0 ID, 'Новый шаг' Name, '' Description, 0 OrderNumb
                union all
                select ID, Name, Description, OrderNumb from Step where Topic_ID = {1} order by OrderNumb
            ", Topic_ID);

            foreach (DataRow r in dt.Rows)
            {
                AStep top = new AStep(Topic_ID, G._I(r["OrderNumb"])) { ID = G._I(r["ID"]), Name = G._S(r["Name"]), Description = G._S(r["Description"]) };
                this.Add(top);
            }
        }
    }

    public class AStep : SelectListItem
    {
        public int ID { get; set; } = 0;
        public string Name { get; set; } = "";
        public string Position { get; set; } = "";
        public string Orientation { get; set; } = "";
        public string Description { get; set; } = "";
        public int Topic_ID { get; set; } = 0;
        public int OrderNumb { get; set; } = 0;

        public AStep() { }

        public AStep(int Step_ID, int pTopic_ID = 0)
        {
            Topic_ID = pTopic_ID;

            DataTable dt = G.db_select("select * from Step where ID = {1}", Step_ID);
            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                ID = G._I(r["ID"]);
                Name = G._S(r["Name"]);
                Description = G._S(r["Description"]);
                Position = G._S(r["Position"]);
                Orientation = G._S(r["Orientation"]);
                OrderNumb = G._I(r["OrderNumb"]);

                if (Topic_ID == 0 && Step_ID > 0) Topic_ID = G._I(r["Topic_ID"]);
            }
            if (Step_ID == 0)
                OrderNumb = G._I(G.db_select("select IsNull(max(OrderNumb), 0)+1 from Step where Topic_ID = {1}", Topic_ID));
        }

        public int Save()
        {
            string sql = string.Format(@"
                update Step set Name = '{1}', Description = '{2}', Position = '{3}', OrderNumb = {4} where ID = {0}
                select {0}
            ", ID, Name, Description, Position, OrderNumb);

            if (ID == 0)
                sql = string.Format(@"
                    insert Step(Name, Description, Position, Orientation, Topic_ID, OrderNumb) values('{0}', '{1}', '{2}', '{3}', {4}, {5})
                    select @@IDENTITY
                ", Name, Description, Position, Orientation, Topic_ID, OrderNumb);

            return ID = G._I(G.db_select(sql));
        }

        public void Delete()
        {
            G.db_select("delete Step where ID = {1}", ID);
        }
    }
}