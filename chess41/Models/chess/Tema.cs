using System.Collections.Generic;
using System.Data;
using am.BL;

namespace chess41.Models
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
                select 0 ID, 'Новая тема' Name, -1 OrderNumb
                union all
                select ID, Name, IsNull(OrderNumb, 0) OrderNumb from PTema order by OrderNumb
            ");

            foreach (DataRow r in dt.Rows)
            {
                PTema tem = new PTema { ID = G._I(r["ID"]), Name = G._S(r["Name"]) };
                this.Add(tem);
            }
        }
    }

    public class PTema
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int OrderNumb { get; set; }

        public bool Selected { get; set; }

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
            if(Tema_ID == 0) OrderNumb = G._I(G.db_select("select IsNull(max(OrderNumb), 0)+1 from PTema"));
        }

        public int Save()
        {
            string sql = string.Format(@"
                update PTema set Name = '{1}', Description = '{2}', OrderNumb = {3} where ID = {0}
                select {0}
            ", ID, Name, Description, OrderNumb);

            if (ID == 0)
                sql = string.Format(@"
                insert PTema(Name, Description, OrderNumb) values('{0}', '{1}', {2})
                select @@IDENTITY
            ", Name, Description, OrderNumb);

            return ID = G._I(G.db_select(sql));
        }

        public void Delete()
        {
            G.db_select("delete PTema where ID = {1}", ID);
        }
    }
}