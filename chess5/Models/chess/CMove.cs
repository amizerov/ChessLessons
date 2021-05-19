using am.BL;
using System.Data;

namespace chess5.Models
{
    public class CMove
    {
    }

    public class MoveResult
    {
        public string Description = "неправильный ход. Попробуй еще раз!";
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

            //Анализируем пройден ли Топик и пройден ли Урок
            if (Correctness == 1)
            {
                //Если это последний Шаг в Топике ..
                int tid = G._I(G.db_select("select Topic_ID from Step where Step_ID = {1}", Step_ID));
                dt = G.db_select("select top 1 Step_ID from Step where Topic_ID = {1} order by OrderNumb desc", tid);
                if (Step_ID == G._I(dt))// .. тогда топик пройден
                {
                    G.db_exec("update UserTopic set IsComlete = 1 where Topic_ID = {1} and User_Id = '{2}'");
                }
            }
        }
    }

}