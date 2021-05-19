using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using am.BL;

namespace chess41.Models
{
    public class CorrDescr
    {
        public int ID { get; set; }
        public string Description { get; set; }
    }

    public class PMoves : List<PMove>
    {
        public int Step_ID { get; set; }
        public PStep Step { get { return new PStep(Step_ID); } }

        public PMoves() { }

        public PMoves(int step_id)
        {
            Step_ID = step_id;
            Update();
        }

        public void Update()
        {
            DataTable dt = G.db_select(@"
                select 0 ID, 'Новый ход' Name, 0 OrderNumb
                union all
                select ID, Name, OrderNumb from PMove where Step_ID = {1} order by OrderNumb
            ", Step_ID);

            foreach (DataRow r in dt.Rows)
            {
                PMove mov = new PMove(Step_ID, G._I(r["OrderNumb"])) { ID = G._I(r["ID"]), Name = G._S(r["Name"])};
                this.Add(mov);
            }
        }
    }

    public class PMove : SelectListItem
    {
        public int ID { get; set; } = 0;
        public string Name { get; set; } = "";
        public string MoveFrom { get; set; } = "";
        public string Position { get; set; } = "";
        public int Step_ID { get; set; } = 0;
        public int OrderNumb { get; set; } = 0;
        public int Correctness { get; set; } = 0;

        public PMove() { }

        public PMove(int Move_ID, int step_ID = 0)
        {
            Step_ID = step_ID;

            DataTable dt = G.db_select("select * from PMove where ID = {1}", Move_ID);
            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                ID = G._I(r["ID"]);
                Name = G._S(r["Name"]);
                MoveFrom = G._S(r["MoveFrom"]);
                Position = G._S(r["Position"]);
                OrderNumb = G._I(r["OrderNumb"]);
                Correctness = G._I(r["Correctness"]);
                if(Step_ID == 0) Step_ID = G._I(r["Step_ID"]);
            }
            if (Move_ID == 0)
                OrderNumb = G._I(G.db_select("select IsNull(max(OrderNumb), 0)+1 from PMove where Step_ID = {1}", Step_ID));
        }

        public int Save()
        {
            MoveFrom = MoveFromFENs();

            string sql = string.Format(@"
                update PMove set Name = '{1}', MoveFrom = '{2}', Position = '{3}', OrderNumb = {4}, Correctness = {5} where ID = {0}
                select {0}
            ", ID, Name, MoveFrom, Position, OrderNumb, Correctness);

            if (ID == 0)
                sql = string.Format(@"
                    insert PMove(Name, MoveFrom, Position, Step_ID, OrderNumb, Correctness) 
                    values('{0}', '{1}', '{2}', {3}, {4}, {5})
                    select @@IDENTITY
                ", Name, MoveFrom, Position, Step_ID, OrderNumb, Correctness);

            return ID = G._I(G.db_select(sql));
        }

        string MoveFromFENs()
        {
            string Figura = "", FromL = "", FromN = "", Move = "";
            string fr = matrix(MoveFrom), to = matrix(Position);
            string[] ars = fr.Split('/');
            string[] are = to.Split('/');
            for (int i = 0; i < 8; i++)
            {
                if(ars[i] != are[i])
                    for (int j = 0; j < 8; j++)
                    {
                        if (ars[i][j] != '0' && are[i][j] == '0')
                        {
                            Figura = ars[i][j] + "";
                            FromL = (Char)((int)'a' + j) + "";
                            FromN = (8-i) + "";
                        }
                    }
            }
            Move = Figura + FromL + FromN;
            return Move;
        }

        string matrix(string fen)
        {
            fen = fen.Replace("8", "00000000");
            fen = fen.Replace("7", "0000000");
            fen = fen.Replace("6", "000000");
            fen = fen.Replace("5", "00000");
            fen = fen.Replace("4", "0000");
            fen = fen.Replace("3", "000");
            fen = fen.Replace("2", "00");
            fen = fen.Replace("1", "0");

            return fen;
        }

        public void Delete()
        {
            G.db_select("delete PMove where ID = {1}", ID);
        }
    }

}