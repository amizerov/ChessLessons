using System.Collections.Generic;
using System.Data;
using am.BL;

namespace chess5.Models
{
    public class PTemas : List<PTema>
    {
        public PTemas()
        {
            Update();
        }

        public void Update()
        {
            DataTable dt = G.db_select(@"
                select ID, Name, IsNull(OrderNumb, 0) OrderNumb from PTema order by OrderNumb
            ");

            foreach (DataRow r in dt.Rows)
            {
                PTema les = new PTema { ID = G._I(r["ID"]), Name = G._S(r["Name"]) };
                this.Add(les);
            }
        }
    }

    public class PTema
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int OrderNumb { get; set; }
        public Puzzles Puzzles { get { return new Puzzles(ID); } }

        public PTema() { }

        public PTema(int Tema_ID)
        {
            DataTable dt = G.db_select("select * from PTema where ID = {1}", Tema_ID);
            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                ID = G._I(r["ID"]);
                Name = G._S(r["Name"]);
                Description = G._S(r["Description"]);
                OrderNumb = G._I(r["OrderNumb"]);
            }
            if (Tema_ID == 0) OrderNumb = G._I(G.db_select("select IsNull(max(OrderNumb), 0)+1 from PTema"));
        }
    }
}