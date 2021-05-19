using am.BL;
using System.Data;

namespace chess5.Models
{
    public class PMove
    {
    }

    public class PMoveResult
    {
        public int Correctness = 0;
        public int NewUserPuzleRating = 0;
        public int ChangeUserPuzleRating = 0;

        public PMoveResult(int Step_ID, string position_after_move)
        {
            PStep s = new PStep(Step_ID);

            DataTable dt = G.db_select(@"
                select Correctness from PMove 
                where Step_ID = {2} and Position = '{1}' COLLATE SQL_Latin1_General_Cp1_CS_AS
            ", position_after_move, Step_ID);

            Correctness = G._I(dt);

            if (s.IsLastStep == 1 || Correctness == 0)
            {
                int cr = CurrentUser.Account.RatingPuzzle;
                CurrentUser.UpdateRatingPuzzle(s.Puzzle_ID, Correctness);
                NewUserPuzleRating = CurrentUser.Account.RatingPuzzle;
                ChangeUserPuzleRating = NewUserPuzleRating - cr;
            }
        }
    }

}