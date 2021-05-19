using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using am.BL;

namespace chess4.Models
{
    public class CorrDescr
    {
        public int ID { get; set; }
        public string Description { get; set; }
    }

    public class AMoves : List<AMove>
    {
        public int Step_ID { get; set; }
        public AStep Step { get { return new AStep(Step_ID); } }
        public List<CorrDescr> Last5CorrDescr {
            get {
                DataTable dt = G.db_select(
                    @"select top 5 count(*) c, max(ID) ID, Description from Move 
                      where Correctness = 1 
                      group by Description
                      order by 1 desc"
                );
                List<CorrDescr> ld = new List<CorrDescr>();
                foreach (DataRow r in dt.Rows)
                {
                    CorrDescr cd = new CorrDescr { ID = G._I(r, "ID"), Description = G._S(r, "Description") };
                    ld.Add(cd);
                }
                return ld;
            }
        }

        public AMoves() { }

        public AMoves(int step_id)
        {
            Step_ID = step_id;

            Update();
        }

        public void Update()
        {
            DataTable dt = G.db_select(@"
                select 0 ID, 'Новый ход' Name, '' Description, 0 OrderNumb
                union all
                select ID, Name, Description, OrderNumb from Move where Step_ID = {1} order by OrderNumb
            ", Step_ID);

            foreach (DataRow r in dt.Rows)
            {
                AMove top = new AMove(Step_ID, G._I(r["OrderNumb"])) { ID = G._I(r["ID"]), Name = G._S(r["Name"]), Description = G._S(r["Description"]) };
                this.Add(top);
            }
        }
    }

    public class AMove : SelectListItem
    {
        public int ID { get; set; } = 0;
        public string Name { get; set; } = "";
        public string Position { get; set; } = "";
        public string Description { get; set; } = "";
        public int Step_ID { get; set; } = 0;
        public int OrderNumb { get; set; } = 0;
        public int Correctness { get; set; } = 0;

        public AMove() { }

        public AMove(int Move_ID, int pStep_ID = 0)
        {
            Step_ID = pStep_ID;

            DataTable dt = G.db_select("select * from Move where ID = {1}", Move_ID);
            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                ID = G._I(r["ID"]);
                Name = G._S(r["Name"]);
                Description = G._S(r["Description"]);
                Position = G._S(r["Position"]);
                OrderNumb = G._I(r["OrderNumb"]);
                Correctness = G._I(r["Correctness"]);
                if(Step_ID == 0) Step_ID = G._I(r["Step_ID"]);
            }
            if (Move_ID == 0)
                OrderNumb = G._I(G.db_select("select IsNull(max(OrderNumb), 0)+1 from Move where Step_ID = {1}", Step_ID));
        }

        public AMove(int Move_ID, int moveNumber, bool DlyClienta = true)
        {
            DataTable dt = G.db_select(@"
                select * from Move where Step_ID = {1} and OrderNumb = {2} order by ID
            ", Step_ID, moveNumber);

            int c = dt.Rows.Count;
            if (c > 0)
            {
                DataRow r = dt.Rows[0];
                ID = G._I(r["ID"]);
                Name = G._S(r["Name"]);
                Description = G._S(r["Description"]);
                Position = G._S(r["Position"]);
                OrderNumb = G._I(r["OrderNumb"]);
                Correctness = G._I(r["Correctness"]);
            }
        }

        public int Save()
        {
            string sql = string.Format(@"
                update Move set Name = '{1}', Description = '{2}', Position = '{3}', OrderNumb = {4}, Correctness = {5} where ID = {0}
                select {0}
            ", ID, Name, Description, Position, OrderNumb, Correctness);

            if (ID == 0)
                sql = string.Format(@"
                    insert Move(Name, Description, Position, Step_ID, OrderNumb, Correctness) values('{0}', '{1}', '{2}', {3}, {4}, {5})
                    select @@IDENTITY
                ", Name, Description, Position, Step_ID, OrderNumb, Correctness);

            return ID = G._I(G.db_select(sql));
        }

        public void Delete()
        {
            G.db_select("delete Move where ID = {1}", ID);
        }
    }

    public class MoveResult
    {
        public string Description = "Это неправильный ход. Попробуй еще раз!";
        public int Correctness = 0;

        public MoveResult(int Step_ID, string position_after_move)
        {
            DataTable dt = G.db_select(@"
                select Description, Correctness from Move 
                where Step_ID = {2} and Position = '{1}' COLLATE SQL_Latin1_General_Cp1_CS_AS
            ", position_after_move, Step_ID);

            if (dt.Rows.Count > 0)
            {
                Description = G._S(dt.Rows[0][0]);
                Correctness = G._I(dt.Rows[0][1]);
            }
        }
    }

}