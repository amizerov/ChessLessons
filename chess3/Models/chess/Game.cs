using System.Data;
using am.BL;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace chess5.Models
{
    public class NewGame
    {
        public int ID { get; set; }
        public string Sopernik { get; set; }
        public string Avatar { get; set; }
        public int BaseTime { get; set; }
        public int Increment { get; set; }
        public NewGame()
        {
            ID = IsOpen();
            DataTable dt = G.db_select(@"
                select g.BaseTime, g.Increment, wu.Email w, bu.Email b,
                       wu.Avatar wa, bu.Avatar ba
                from Game g 
                left outer join AspNetUsers bu on g.Black_ID = bu.Id
                left outer join AspNetUsers wu on g.White_ID = wu.Id
                where g.ID = {1}
            ", ID);
            foreach (DataRow r in dt.Rows)
            {
                BaseTime = G._I(r["BaseTime"]);
                Increment = G._I(r["Increment"]);
                if (G._S(r["w"]) == "")
                {
                    Sopernik = G._S(r["b"]);
                    Avatar = G._S(r["ba"]);
                }
                else
                {
                    Sopernik = G._S(r["w"]);
                    Avatar = G._S(r["wa"]);
                }
            }
        }
        public static int IsOpen()
        {
            DataTable dt = G.db_select(@"
	        select g.ID from Game g 
		        where (select count(*) from GMove where Game_ID = g.ID) = 0
		          and (White_ID is null or Black_ID is null)
                  and IsActive = 1
            ");

            return G._I(dt);
        }
    }

    public class MyGame
    {
        public int Game_ID { get; set; }
        public int GamePassword { get; set; }
        public bool IsActive { get; set; }
        public string PGN { get; set; }
        public string MyColor { get; set; }
        public string SoperNik { get; set; }
        public string MyOwnNik { get; set; }
        public int BaseTime { get; set; }
        public int Increment { get; set; }
        public CAccount MyAccount { get; set; }

        public MyGame(int Game_ID, string User_ID, int GamePassword = 0)
        {
            this.Game_ID = Game_ID;
            if(GamePassword > 1000 && GamePassword < 9999)
            {
                this.GamePassword = GamePassword;
            } else
            {
                this.GamePassword = 0;
            }


            MyAccount = new CAccount(User_ID);


            /*
            DataTable dt = G.db_select(@"
                    select 
                        uw.FirstName FirstNameW, uw.LastName LastNameW,
                        ub.FirstName FirstNameB, ub.LastName LastNameB,
                        uw.UserName White, ub.UserName Black, White_ID, Black_ID, 
					    aw.RatingGame RatingW, ab.RatingGame RatingB, BaseTime, Increment,
                        g.IsActive, g.PGN, g.Password
                    from Game g left join AspNetUsers uw on uw.id = g.White_ID
                                left join AspNetUsers ub on ub.id = g.Black_ID
								left join Account aw on aw.User_Id = uw.Id
								left join Account ab on ab.User_Id = ub.Id
                    where g.ID = {1} and g.Password in (is null, {2})", Game_ID, GamePassword);

            */


            DataTable dt = G.db_select(@"
                    select 
                        uw.FirstName FirstNameW, uw.LastName LastNameW,
                        ub.FirstName FirstNameB, ub.LastName LastNameB,
                        uw.UserName White, ub.UserName Black, White_ID, Black_ID, 
					    aw.RatingGame RatingW, ab.RatingGame RatingB, BaseTime, Increment,
                        g.IsActive, g.PGN, g.Password
                    from Game g left join AspNetUsers uw on uw.id = g.White_ID
                                left join AspNetUsers ub on ub.id = g.Black_ID
								left join Account aw on aw.User_Id = uw.Id
								left join Account ab on ab.User_Id = ub.Id
                    where g.ID = {1}", Game_ID);

            foreach (DataRow r in dt.Rows)
            {
                this.IsActive = G._B(r["IsActive"]);
                this.PGN = G._S(r["PGN"]);
                string White_ID = G._S(r["White_ID"]);
                string Black_ID = G._S(r["Black_ID"]);

                if (White_ID == "" && Black_ID != "" && Black_ID != User_ID && IsActive)
                {
                    G.db_exec("update Game set White_ID = '{1}' where ID = {2}", User_ID, Game_ID);
                    White_ID = User_ID;
                }
                if (White_ID != "" && Black_ID == "" && White_ID != User_ID && IsActive)
                {
                    G.db_exec("update Game set Black_ID = '{1}' where ID = {2}", User_ID, Game_ID);
                    Black_ID = User_ID;
                }

                BaseTime = G._I(r["BaseTime"]);
                Increment = G._I(r["Increment"]);
                if (User_ID == White_ID && Black_ID == "" && GamePassword > 0)
                {
                    G.db_exec("update Game set Password = '{1}' where ID = {2}", GamePassword, Game_ID);
                }
                else if(User_ID == Black_ID && White_ID == "" && GamePassword > 0)
                {
                    G.db_exec("update Game set Password = '{1}' where ID = {2}", GamePassword, Game_ID);
                }

                if (User_ID == White_ID)
                {
                    MyColor = "w";
                    if(G._S(r["FirstNameB"]) + G._S(r["LastNameB"]) == "")
                        SoperNik = G._S(r["Black"]) + " (" + G._I(r["RatingB"]) + ")";
                    else
                        SoperNik = G._S(r["FirstNameB"]) + " " + G._S(r["LastNameB"]) + " (" + G._I(r["RatingB"]) + ")";

                    if (G._S(r["FirstNameW"]) + G._S(r["LastNameW"]) == "")
                        MyOwnNik = G._S(r["White"]) + " (" + G._I(r["RatingW"]) + ")";
                    else
                        MyOwnNik = G._S(r["FirstNameW"]) + " " + G._S(r["LastNameW"]) + " (" + G._I(r["RatingW"]) + ")";
                }
                else if(User_ID == Black_ID)
                {
                    MyColor = "b";
                    if (G._S(r["FirstNameW"]) + G._S(r["LastNameW"]) == "")
                        SoperNik = G._S(r["White"]) + " (" + G._I(r["RatingW"]) + ")";
                    else
                        SoperNik = G._S(r["FirstNameW"]) + " " + G._S(r["LastNameW"]) + " (" + G._I(r["RatingW"]) + ")";

                    if (G._S(r["FirstNameB"]) + G._S(r["LastNameB"]) == "")
                        MyOwnNik = G._S(r["Black"]) + " (" + G._I(r["RatingB"]) + ")";
                    else
                        MyOwnNik = G._S(r["FirstNameB"]) + " " + G._S(r["LastNameB"]) + " (" + G._I(r["RatingB"]) + ")";
                }
            }
            if (SoperNik == "(0)") SoperNik = "Ожидание соперника ...";
        }

        public int SetResult(int MyRes)
        {
            int res = 0;
            CurrentUser.UpdateRatingGame(Game_ID, MyColor == "w" ? 1 : 0, MyRes);

            return res;
        }
    }

    public class GMove
    {
        public string Color { get; set; }
        public string Move { get; set; }
        public string FEN { get; set; }
        public string PGN { get; set; }
        public string History { get; set; }
        public string Status { get; set; }
        public int SecondsSopernikOffline { get; set; }

        public GMove(int Game_ID, string MyColor)
        {   //Получаем последний ход нашей игры и отметим время когда мы были онлайн
            DataTable dt = G.db_select("Game_GetMove {1}, '{2}'", Game_ID, MyColor);
            foreach (DataRow r in dt.Rows)
            {
                Color = G._S(r["Color"]);  //цвет последнего хода
                Status = G._S(r["Status"]);//Состояние игры после последнего хода 
                                           //Шах, мат, ничья, сдался, закончилось время
                {//Если последний ход в игре сделал соперник, то двигаем фигуру
                    Move = G._S(r["Move"]);
                    FEN = G._S(r["FEN"]);
                    PGN = G._S(r["PGN"]);
                    History = G._S(r["History"]);
                 //Кто сделал этот последний ход мы отследим на клиенте
                }//и решать двигать или нет фигуру тоже будем там

                if (MyColor == "w") SecondsSopernikOffline = G._I(r["SecondsBlackOffline"]);
                if (MyColor == "b") SecondsSopernikOffline = G._I(r["SecondsWhiteOffline"]);
            }
        }
    }

    public class GameResult
    {
        public int NewGameRating = 0;
        public int NewChessCoin = 0;
        public int GameRatingDelta = 0;
        public int ChessCoinDelta = 0;
    }
}