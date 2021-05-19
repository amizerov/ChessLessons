using System.Collections.Generic;
using System.Web.Mvc;
using System.Data;
using am.BL;

namespace chess41.Models
{
    public class PSteps : List<PStep>
    {
        public int Puzzle_ID { get; set; }
        public Puzzle Puzzle { get { return new Puzzle(Puzzle_ID); } }
        public string Last_Position { get; set; }

        public PSteps() { }

        public PSteps(int puz_id, int selStep_ID) : this(puz_id)
        {
            this.Find(s => s.ID == selStep_ID).Selected = true;
        }

        public PSteps(int puz_id)
        {
            Puzzle_ID = puz_id;

            int last_step_id = G._I(G.db_select("select top 1 ID from PStep where Puzzle_ID = {1} order by OrderNumb desc", Puzzle_ID));
            if (last_step_id > 0)
            {
                int last_move_id = G._I(G.db_select("select top 1 ID from PMove where Step_ID = {1} order by OrderNumb desc", last_step_id));
                if (last_move_id > 0)
                    Last_Position = G._S(G.db_select("select Position from PMove where ID = {1}", last_move_id));
                else
                    Last_Position = G._S(G.db_select("select Position from PStep where ID = {1}", last_step_id));
            }
            else
                Last_Position = G._S(G.db_select("select Position from Puzzle where ID = {1}", Puzzle_ID));


            Update();
        }

        public void Update()
        {
            DataTable dt = G.db_select(@"
                select 0 ID, 'Новый шаг' Name, 0 OrderNumb
                union all
                select ID, Name, OrderNumb from PStep where Puzzle_ID = {1} order by OrderNumb
            ", Puzzle_ID);

            foreach (DataRow r in dt.Rows)
            {
                PStep st = new PStep(Puzzle_ID, G._I(r["OrderNumb"])) { ID = G._I(r["ID"]), Name = G._S(r["Name"]) };
                this.Add(st);
            }
        }
    }

    public class PStep : SelectListItem
    {
        public int ID { get; set; } = 0;
        public string Name { get; set; } = "";
        public string Position { get; set; } = "";
        public string Orientation { get; set; } = "";
        public int Puzzle_ID { get; set; } = 0;
        public Puzzle Puzzle { get { return new Puzzle(Puzzle_ID); } }
        public int OrderNumb { get; set; } = 0;

        public PStep() { }

        public PStep(int Step_ID, int puzzle_ID = 0)
        {
            Puzzle_ID = puzzle_ID;

            DataTable dt = G.db_select("select * from PStep where ID = {1}", Step_ID);
            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                ID = G._I(r["ID"]);
                Name = G._S(r["Name"]);
                Position = G._S(r["Position"]);
                Orientation = G._S(r["Orientation"]);
                OrderNumb = G._I(r["OrderNumb"]);

                if (Puzzle_ID == 0 && Step_ID > 0) Puzzle_ID = G._I(r["Puzzle_ID"]);
            }
            if (Step_ID == 0)
                OrderNumb = G._I(G.db_select("select IsNull(max(OrderNumb), 0)+1 from PStep where Puzzle_ID = {1}", Puzzle_ID));
        }

        public int Save()
        {
            string sql = string.Format(@"
                update PStep set Name = '{1}', Position = '{2}', OrderNumb = {3} where ID = {0}
                select {0}
            ", ID, Name, Position, OrderNumb);

            if (ID == 0)
                sql = string.Format(@"
                    insert PStep(Name, Position, Orientation, Puzzle_ID, OrderNumb) values('{0}', '{1}', '{2}', {3}, {4})
                    select @@IDENTITY
                ", Name, Position, Orientation, Puzzle_ID, OrderNumb);

            return ID = G._I(G.db_select(sql));
        }

        public void Delete()
        {
            G.db_select("delete PStep where ID = {1}", ID);
        }
    }
}