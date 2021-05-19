using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Data;
using am.BL;

namespace chess41.Models
{
    public class Puzzles : List<Puzzle>
    {
        public int Tema_ID { get; set; }
        public PTema Tema { get { return new PTema(Tema_ID); } }

        public Puzzles(){ }

        public Puzzles(int tem_id, int selPuzz_ID) : this(tem_id)
        {
            this.Find(t => t.ID == selPuzz_ID).Selected = true;
        }

        public Puzzles(int tem_id)
        {
            Tema_ID = tem_id;
            Update();
        }

        public void Update()
        {
            DataTable dt = G.db_select(@"
                select 0 ID, 'Новая задача' Name, 0 OrderNumb
                union all
                select ID, Name, OrderNumb from Puzzle where Tema_ID = {1} order by OrderNumb desc
            ", Tema_ID);

            foreach (DataRow r in dt.Rows)
            {
                Puzzle puz = new Puzzle(Tema_ID, G._I(r["OrderNumb"])) { ID = G._I(r["ID"]), Name = G._S(r["Name"]) };
                this.Add(puz);
            }

        }
    }

    public class Puzzle : SelectListItem
    {
        public int ID { get; set; } = 0;
        public string Name { get; set; } = "";
        public string Position { get; set; } = "";
        public string Orientation { get; set; } = "";
        public int Tema_ID { get; set; } = 0;
        public PTema Tema { get { return new PTema(Tema_ID); } }
        public int OrderNumb { get; set; }
        public int Rating { get; set; }
        public int PuzzleOfDay { get; set; } = 0;
        public Puzzle() { }

        public Puzzle(int Puzzle_ID, int tema_ID = 0)
        {
            Tema_ID = tema_ID;

            DataTable dt = G.db_select("select * from Puzzle where ID = {1}", Puzzle_ID);
            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                ID = G._I(r["ID"]);
                Name = G._S(r["Name"]);
                Position = G._S(r["Position"]);
                Rating = G._I(r["Rating"]);
                Orientation = G._S(r["Orientation"]);
                OrderNumb = G._I(r["OrderNumb"]);
                PuzzleOfDay = G._I(r["PuzzleOfDay"]);

                if (Tema_ID == 0 && Puzzle_ID > 0) Tema_ID = G._I(r["Tema_ID"]);
            }

            if(Puzzle_ID == 0)
                OrderNumb = G._I(G.db_select("select IsNull(max(OrderNumb), 0)+1 from Puzzle where Tema_ID = {1}", Tema_ID));
        }

        public int MakePuzzleOfDay()
        {
            G.db_exec(@"
                update Puzzle set PuzzleOfDay = 0 where PuzzleOfDay = 1
                update Puzzle set PuzzleOfDay = 1 where ID = {1}", ID);
            return G.LastError.Length > 0 ? 0 : 1;
        }

        public int Save()
        {
            string sql = string.Format(@"
                update Puzzle set Name = '{1}', Position = '{2}', Rating = {3}, Orientation = '{4}', OrderNumb = {5} where ID = {0}
                select {0}
            ", ID, Name, Position, Rating, Orientation, OrderNumb);


            if (ID == 0)
                sql = string.Format(@"
                    insert Puzzle(Name, Position, Orientation, Tema_ID, OrderNumb) values('{0}', '{1}', '{2}', {3}, {4})
                    select @@IDENTITY
                ", Name, Position, Orientation, Tema_ID, OrderNumb);

            return ID = G._I(G.db_select(sql));
        }

        public void Delete()
        {
            G.db_select("delete Puzzle where ID = {1}", ID);
        }
    }
}