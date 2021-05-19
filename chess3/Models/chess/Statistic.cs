using System.Collections.Generic;
using System.Data;
using am.BL;

namespace chess5.Models
{
    public class Statistic
    {
        public CAccount UserAccount { get; set; }
        public StatGame Game { get; set; }
        public StatLesson Lesson { get; set; }
        public StatPuzzle Puzzle { get; set; }

        public Statistic()
        {
            UserAccount = new CAccount();
            Game = new StatGame();
        }
    }

    public class StatGame
    {
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Drows { get; set; }
        public int Fails { get; set; }
        public int Rating { get; set; }
        public Dictionary<string, int> RatingHistory { get; }
        public string RatingHistoryStringD { get; set; }
        public string RatingHistoryStringR { get; set; }
        public StatGame(string User_ID = null)
        {
            if (User_ID == null) User_ID = CurrentUser.ID;

            Rating = G._I(G.db_select("select RatingGame from Account where User_Id = '{1}'", User_ID));

            Wins = G._I(G.db_select("Stat_GameWins '{1}'", User_ID));
            Losses = G._I(G.db_select("Stat_GameLosses '{1}'", User_ID));
            Drows = G._I(G.db_select("Stat_GameDrows '{1}'", User_ID));
            Fails = G._I(G.db_select("Stat_GameFails '{1}'", User_ID));

            RatingHistoryStringD = "";
            RatingHistoryStringR = "";
            RatingHistory = new Dictionary<string, int>();
            DataTable dt = G.db_select("Stat_GameRatingHistory '{1}'", User_ID);
            foreach (DataRow r in dt.Rows)
            {
                RatingHistory.Add("'" + G._S(r["GameDate"]) + "'", G._I(r["Rating"]));
                RatingHistoryStringD += "'" + G._S(r["GameDate"]) + "',";
                RatingHistoryStringR += G._I(r["Rating"]) + ",";
            }
            RatingHistoryStringD = RatingHistoryStringD.Trim(',');
            RatingHistoryStringR = RatingHistoryStringR.Trim(',');
        }
    }

    public class StatLesson
    {
        public int PassedCnt;
        public StatLesson(string User_ID = null)
        {
            if (User_ID == null) User_ID = CurrentUser.ID;
            PassedCnt = G._I(G.db_select("select count(*) from UserLesson where User_ID = '{1}'", User_ID));
        }
    }

    public class StatPuzzle
    {
        public int PassedCnt;
        public StatPuzzle(string User_ID = null)
        {
            if (User_ID == null) User_ID = CurrentUser.ID;
            PassedCnt = G._I(G.db_select("select count(*) from UserPuzzle where User_ID = '{1}'", User_ID));
        }
    }

}