using System.Data;
using am.BL;

namespace chess5.Models
{
    public class PStep
    {
        public int ID { get; set; } = 0;
        public string Name { get; set; } = "";
        public string CorrectMoveFrom { get; set; } = "";
        public string Position { get; set; } = "";
        public int Puzzle_ID { get; set; } = 0;
        public int OrderNumb { get; set; } = 0;
        public int NextPuzzleID { get; set; } = 0;
        public int IsLastStep { get; set; } = 0;

        public PStep(int puzzle_id, int stepNumber)
        {
            Puzzle_ID = puzzle_id;

            DataTable dt = G.db_select(@"
                select s.ID, s.Name, MoveFrom, s.Position, s.OrderNumb 
                from PStep s join PMove m on s.ID = m.Step_ID
                where Puzzle_ID = {1} and s.OrderNumb = {2} and m.Correctness = 1 order by ID
            ", Puzzle_ID, stepNumber);

            int c = dt.Rows.Count;
            if (c > 0)
            {
                DataRow r = dt.Rows[0];
                ID = G._I(r["ID"]);
                Name = G._S(r["Name"]);
                CorrectMoveFrom = G._S(r["MoveFrom"]);
                Position = G._S(r["Position"]);
                OrderNumb = G._I(r["OrderNumb"]);

                dt = G.db_select("select max(OrderNumb) from PStep where Puzzle_ID = {1}", Puzzle_ID);
                IsLastStep = G._I(dt) == stepNumber ? 1 : 0;
            }
            else
            {
                Puzzle p = new Puzzle(Puzzle_ID);
                dt = G.db_select(@"
                    select top 1 ID from Puzzle where Tema_ID = {1} and OrderNumb > {2} order by OrderNumb
                ", p.Tema_ID, p.OrderNumb);
                NextPuzzleID = G._I(dt);
            }
        }

        public PStep(int ID)
        {
            DataTable dt = G.db_select(@"
                select top 1 s.ID, s.Name, MoveFrom, s.Position, s.OrderNumb, s.Puzzle_ID 
                from PStep s join PMove m on s.ID = m.Step_ID
                where s.ID = {1} and m.Correctness = 1", ID);
            foreach (DataRow r in dt.Rows)
            {
                ID = G._I(r["ID"]);
                Name = G._S(r["Name"]);
                CorrectMoveFrom = G._S(r["MoveFrom"]);
                Position = G._S(r["Position"]);
                OrderNumb = G._I(r["OrderNumb"]);
                Puzzle_ID = G._I(r["Puzzle_ID"]);

                dt = G.db_select("select max(OrderNumb) from PStep where Puzzle_ID = {1}", Puzzle_ID);
                IsLastStep = G._I(dt) == OrderNumb ? 1 : 0;
            }
        }
    }
}