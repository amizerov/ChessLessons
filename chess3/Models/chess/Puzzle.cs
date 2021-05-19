using System.Collections.Generic;
using System.Data;
using am.BL;

namespace chess5.Models
{
    public class Puzzles : List<Puzzle>
    {
        public int Tema_ID;

        public Puzzles(int tema_id)
        {
            Tema_ID = tema_id;
            Update();
        }

        public void Update()
        {
            DataTable dt = G.db_select(@"
                select ID, Name, Position, Orientation, IsNull(OrderNumb, 0) OrderNumb from Puzzle 
                where Tema_ID = {1}
                order by OrderNumb 
            ", Tema_ID);

            foreach (DataRow r in dt.Rows)
            {
                Puzzle puz = new Puzzle
                {
                    ID = G._I(r["ID"]),
                    Name = G._S(r["Name"]),
                    Position = G._S(r["Position"]),
                    OrderNumb = G._I(r["OrderNumb"]),
                    Orientation = G._S(r["Orientation"]),
                };
                this.Add(puz);
            }
        }

        public int SolvedBy(string UserName)
        {
            string sql = @"
                select count(*) from UserPuzzle up 
                join Puzzle p on up.Puzzle_ID = p.ID
                join AspNetUsers u on u.Id = up.User_ID
                where IsComplete = 1 and UserName = '{1}' and Tema_ID = {2}";

            return G._I(G.db_select(sql, UserName, Tema_ID));
        }

        public Puzzle FirstNotSolvedBy(string User_ID)
        {
            DataTable dt = G.db_select(@"
                select top 1 p.ID from Puzzle p
                where Tema_ID = {1} and p.ID not in (
                        select Puzzle_ID from UserPuzzle up 
                        join AspNetUsers u on u.Id = up.User_ID 
                        where IsComplete = 1 and u.ID = '{2}')
                order by OrderNumb ", Tema_ID, User_ID);

            return new Puzzle(G._I(dt));
        }
    }

    public class Puzzle
    {
        public int ID { get; set; } = 0;
        public string Name { get; set; } = "";
        public string Position { get; set; } = "";
        public string Orientation { get; set; } = "";
        public int Tema_ID { get; set; } = 0;
        public int OrderNumb { get; set; }
        public int OrderNumbInTema {
            get
            {
                string sql = @"select Count(*) from Puzzle where Tema_ID = {1} and OrderNumb < {2}";
                DataTable dt = G.db_select(sql, Tema_ID, OrderNumb);

                return G._I(dt) + 1;
            }
        }
        public int Rating { get; set; }
        public string Date { get; set; }
        public PTema Tema
        {
            get
            {
                return new PTema(Tema_ID);
            }
        }
        public PStep FirstStep
        {
            get
            {
                return new PStep(ID, 1);
            }
        }

        public Puzzle() { }

        public Puzzle(int id)
        {
            DataTable dt = G.db_select("select * from Puzzle where ID = {1}", id);
            foreach (DataRow r in dt.Rows)
            {
                ID = G._I(r["ID"]);
                Name = G._S(r["Name"]);
                Orientation = G._S(r["Orientation"]);
                Position = G._S(r["Position"]);
                OrderNumb = G._I(r["OrderNumb"]);
                Rating = G._I(r["Rating"]);
                Tema_ID = G._I(r["Tema_ID"]);
                Date = G._S(r["dtc"]);
            }
        }

        public void Solved(string User_ID, int Correctness)
        {
            string sql = @"
                update UserPuzzle 
                    set IsComplete = 1, SolvedCorrectly = SolvedCorrectly + 1, dtu = getdate() 
                where User_ID = '{1}' and Puzzle_ID = {2}";

            if (Correctness != 1)
                sql = @"
                    update UserPuzzle
                        set SolvedInCorrect = SolvedInCorrect + 1, dtu = getdate()
                    where User_ID = '{1}' and Puzzle_ID = {2}";


            G.db_exec(sql, User_ID, this.ID/*Puzzle.ID*/);
        }
        public int SolvedCorrectly
        {
            get
            {
                return G._I(G.db_select(
                    @"select SolvedCorrectly from UserPuzzle 
                        where User_ID = '{1}' and Puzzle_ID = {2}",
                            CurrentUser.ID, this.ID
                    ));
            }
        }
        public int SolvedInCorrect
        {
            get
            {
                return G._I(G.db_select(
                    @"select SolvedInCorrect from UserPuzzle 
                        where User_ID = '{1}' and Puzzle_ID = {2}",
                            CurrentUser.ID, this.ID
                    ));
            }
        }
        public void CountUser(string UserName)
        {
            DataTable dt = G.db_select("select Id from AspNetUsers where UserName = '{1}'", UserName);
            if (dt.Rows.Count > 0)
            {
                string UserId = G._S(dt);
                dt = G.db_select("select ID from UserPuzzle where User_ID = '{1}' and Puzzle_ID = {2}", UserId, ID);
                int id = G._I(dt);
                if (id > 0)
                {
                    G.db_exec("update UserPuzzle set Count = Count+1, dtu=getdate() where ID = '{1}'", id);
                }
                else
                {
                    G.db_exec("insert UserPuzzle(User_ID, Puzzle_ID) values('{1}', {2})", UserId, ID);
                }
            }
        }
    }

    public class CPuzzleOfDay : Puzzle 
    {
        public int CountAll { get; set; }
        public int CountCor { get; set; }
        public int CountNot { get; set; }
        public CPuzzleOfDay() 
        {
            DataTable dt = G.db_select("News_GetPuzzleOfDay");
            foreach (DataRow r in dt.Rows)
            {
                ID = G._I(r[0]);
                CountAll = G._I(r[1]);
                CountCor = G._I(r[2]);
                CountNot = G._I(r[3]);

                dt = G.db_select("select * from Puzzle where ID = {1}", ID);
                foreach (DataRow rr in dt.Rows)
                {
                    Name = G._S(rr["Name"]);
                    Orientation = G._S(rr["Orientation"]);
                    Position = G._S(rr["Position"]);
                    OrderNumb = G._I(rr["OrderNumb"]);
                    Rating = G._I(rr["Rating"]);
                    Tema_ID = G._I(rr["Tema_ID"]);
                    Date = G._S(rr["dtc"]);
                }
            }
        }
    }
    public class CNewPuzzle : Puzzle
    {
        public int CountAll { get; set; }
        public int CountCor { get; set; }
        public int CountNot { get; set; }
        public CNewPuzzle()
        {
            DataTable dt = G.db_select("select top 1 * from Puzzle order by dtc desc");
            foreach (DataRow r in dt.Rows)
            {
                ID = G._I(r[0]);

                CountAll = G._I(G.db_select("select Count(*) from UserPuzzle where Puzzle_ID = " + ID));
                CountCor = G._I(G.db_select("select Count(*) from UserPuzzle where SolvedCorrectly > 0 and Puzzle_ID = " + ID));
                CountNot = G._I(G.db_select("select Count(*) from UserPuzzle where SolvedCorrectly = 0 and Puzzle_ID = " + ID));

                {
                    Name = G._S(r["Name"]);
                    Orientation = G._S(r["Orientation"]);
                    Position = G._S(r["Position"]);
                    OrderNumb = G._I(r["OrderNumb"]);
                    Rating = G._I(r["Rating"]);
                    Tema_ID = G._I(r["Tema_ID"]);
                    Date = G._S(r["dtc"]);
                }
            }
        }
    }
}