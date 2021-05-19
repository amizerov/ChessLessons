using System.Linq;
using System.Security.Claims;
using System.Data;
using System.Web;
using am.BL;
using System;
using Microsoft.AspNet.Identity;

namespace chess5.Models
{
    public class CPerson
    {
        string _last_name, _first_name, _phone, _email, _avatar;
        public string User_ID { get; set; }
        public int Rating
        {
            get { return G._I(G.db_select($"select RatingGame from Account where User_ID = '{User_ID}'")); }
        }
        public string NikName
        {
            get
            {
                string f = _first_name, l = _last_name, e = _email;
                f = (f == "") ? e.Substring(0, e.IndexOf('@')) : f;
                l = (l == "") ? e.Substring(e.IndexOf('@') + 1, e.IndexOf('.', e.IndexOf('@')) - e.IndexOf('@') - 1) : l;
                return f + " " + l;
            }
        }
        public string FirstName
        {
            get { return _first_name; }
            set
            {
                if (_first_name.Length == 0 && value.Length > 0)
                {
                    CurrentUser.Account.ChessCoin += 100;
                }
                _first_name = value;
                G.db_exec("update AspNetUsers set FirstName = '{1}' where Id = '{2}'", _first_name, CurrentUser.ID);
            }
        }
        public string LastName
        {
            get { return _last_name; }
            set
            {
                if (_last_name.Length == 0 && value.Length > 0)
                {
                    CurrentUser.Account.ChessCoin += 100;
                }
                _last_name = value;
                G.db_exec("update AspNetUsers set LastName = '{1}' where Id = '{2}'", _last_name, CurrentUser.ID);
            }
        }
        public string Phone
        {
            get { return _phone; }
            set
            {
                if (_phone.Length == 0 && value.Length > 0)
                {
                    CurrentUser.Account.ChessCoin += 100;
                }
                _phone = value;
                G.db_exec("update AspNetUsers set PhoneNumber = '{1}' where Id = '{2}'", _phone, CurrentUser.ID);
            }
        }
        public string Email
        {
            get { return _email; }
            set
            {
                if (_email.Length == 0 && value.Length > 0)
                {
                    CurrentUser.Account.ChessCoin += 100;
                }
                _email = value;
                G.db_exec("update AspNetUsers set Email = '{1}' where Id = '{2}'", _email, CurrentUser.ID);
            }
        }
        public string Avatar
        {
            get { return _avatar; }
            set
            {
                if (_avatar.Length == 0 && value.Length > 0)
                {
                    CurrentUser.Account.ChessCoin += 200;
                }
                _avatar = value;
                G.db_exec("update AspNetUsers set Avatar = '{1}' where Id = '{2}'", _avatar, CurrentUser.ID);
            }
        }
        public CPerson(string user_id = null)
        {
            User_ID = user_id;
            if (User_ID == null) User_ID = CurrentUser.ID;
            DataTable dt = G.db_select("select * from AspNetUsers where Id = '{1}'", User_ID);
            foreach (DataRow r in dt.Rows)
            {
                _first_name = r["FirstName"].ToString();
                _last_name = r["LastName"].ToString();
                _phone = r["PhoneNumber"].ToString();
                _email = r["Email"].ToString();
                _avatar = r["Avatar"].ToString();
            }
        }
    }
    public class CurrentUser
    {
        #region Singleton support
        private static object _lockFlag = new object();
        private static CurrentUser _instance;
        public CurrentUser()
        {
        }
        public static CurrentUser Instance
        {
            get
            {
                lock (_lockFlag)
                {
                    if (_instance == null)
                        _instance = new CurrentUser();
                }
                return _instance;
            }
        }
        #endregion

        public static string ID { get { return (((ClaimsPrincipal)HttpContext.Current.User).Claims).ToList()[0].Value; } }
        public static string Login
        {
            get { return G._S(G.db_select("select UserName from AspNetUsers where Id = '{1}'", ID)); }
        }
        public static CPerson Person { get { return new CPerson(); } }
        public static CAccount Account { get { return new CAccount(); } }

        public static void UpdateRatingGame(int Game_ID, int MeWhite, int MyRes)
        {
            G.db_exec("Game_UpdateRating '{1}', {2}, {3}, {4}", ID, Game_ID, MeWhite, MyRes);
        }
        public static void UpdateRatingPuzzle(int Puzzle_ID, int Correctness)
        {
            Puzzle p = new Puzzle(Puzzle_ID);
            p.Solved(ID, Correctness);

            if (p.SolvedCorrectly + p.SolvedInCorrect > 1) return;

            int rp = p.Rating;
            int ru = Account.RatingPuzzle;

            if (Correctness == 1)
                ru = ru + (int)(1.5 * ((double)rp / (double)ru) * (double)8);
            else
                ru = ru - (int)(((double)ru / (double)rp) * (double)8);

            Account.RatingPuzzle = ru;
            Account.ChessCoin += 10 + 40 * Correctness;
        }
        public static void UpdateRatingLesson(int Lesson_ID)
        {

        }
    }
    public class CAccount
    {
        int _cc = 0, _rg = 0, _rp = 0, _rl = 0;
        public int ChessCoin
        {
            get { return _cc; }
            set { _cc = value; G.db_exec($"update Account set ChessCoin = {_cc} where User_Id = '{CurrentUser.ID}'"); }
        }
        public int RatingGame
        {
            get { return _rg; }
            set { _rg = value; G.db_exec("update Account set RatingGame = {1} where User_Id = '{2}'", _rg, CurrentUser.ID); }
        }
        public int RatingPuzzle
        {
            get { return _rp; }
            set { _rp = value; G.db_exec("update Account set RatingPuzzle = {1} where User_Id = '{2}'", _rp, CurrentUser.ID); }
        }
        public int RatingLesson
        {
            get { return _rl; }
            set { _rl = value; G.db_exec("update Account set RatingLesson = {1} where User_Id = '{2}'", _rl, CurrentUser.ID); }
        }

        public CAccount()
        {
            Update(CurrentUser.ID);
        }
        public CAccount(string uid)
        {
            Update(uid);
        }
        public void Create(string uid)
        {
            G.db_exec("insert Account(User_Id) values('{1}')", uid);
            Update(uid);
        }
        public void Update(string uid)
        {
            DataTable dt = G.db_select("select * from Account where User_Id = '{1}'", uid);
            foreach (DataRow r in dt.Rows)
            {
                _cc = G._I(r, "ChessCoin");
                _rg = G._I(r, "RatingGame");
                _rp = G._I(r, "RatingPuzzle");
                _rl = G._I(r, "RatingLesson");
            }
        }
    }
}