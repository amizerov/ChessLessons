using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using am.BL;

namespace AdminSQL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            am.DB.DBManager.Instance.Init(".", "ChessL", "chess", "Mizer160378");
            bool b = am.BL.G.CheckDB();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt = G.db_select("select m.ID, m.Position cur, s.Position old from PMove m join PStep s on m.Step_ID=s.ID order by 1");
            foreach (DataRow r in dt.Rows)
            {
                int ID = G._I(r["ID"]);
                string cur = G._S(r["cur"]);
                string old = G._S(r["old"]);
                string res = MoveFromFENs(cur, old);

                G.db_exec("update PMove set MoveFrom = '{1}' where ID = {2}", res, ID);
            }
        }

        string MoveFromFENs(string cur, string old)
        {
            string Figura = "", FromL = "", FromN = "", Move = "";
            string fr = matrix(old), to = matrix(cur);
            string[] ars = fr.Split('/');
            string[] are = to.Split('/');
            for (int i = 0; i < 8; i++)
            {
                if (ars[i] != are[i])
                    for (int j = 0; j < 8; j++)
                    {
                        if (ars[i][j] != '0' && are[i][j] == '0')
                        {
                            Figura = ars[i][j] + "";
                            FromL = (Char)((int)'a' + j) + "";
                            FromN = (8 - i) + "";
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

    }
}
