using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using am.BL;
using chess5.Models;
using System.Data;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System;
using chess5.Hubs;

namespace chess5.Controllers.chess
{
    public class GameController : Controller
    {
        // GET: Game
        public ActionResult Index()
        {
            ViewBag.ReturnUrl = "/Game";
            ViewBag.FirstName = CurrentUser.Person.FirstName;

            return View();
        }
        public ActionResult Comp()
        {
            return View();
        }
        [Authorize]
        public ActionResult Human(int ID/*Game_ID*/, int GamePassword = 0)
        {
            var User_ID = User.Identity.GetUserId();
            MyGame model = new MyGame(ID, User_ID, GamePassword);
            /**/
            GMove move = new GMove(model.Game_ID, model.MyColor);
            ViewBag.GamePassword = GamePassword;
            ViewBag.LatestMove = move.FEN;
            /**/
            return View(model);
        }
        public ActionResult IsOpen()
        {
            ViewBag.PLAY = NewGame.IsOpen() > 0 ? "PLAY" : "";
            return View();
        }

        public JsonResult StartGame(int BaseTime, int Increment)
        {
            DataTable dt = G.db_select("Game_CreateGame '{1}', {2}, {3}", CurrentUser.ID, BaseTime, Increment);
            int Game_ID = G._I(dt, "Game_ID");
            string Color = G._S(dt, "Color");
            int IsNew = G._I(dt, "IsNew");

            if (IsNew == 1)
            {
                /*==Отправка оповещения о новой игре через вэб сокет==*/
                var newGameData = new NewGame();
                var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                context.Clients.All.hasNewGame(newGameData);
                /*====================================================*/
            }

            return Json(Game_ID, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSopernik(int Game_ID, string MyColor)
        {
            DataTable dt = G.db_select("Game_GetSopernik {1}, '{2}'", Game_ID, MyColor);
            CPerson p = new CPerson(G._S(dt));
            return Json(p, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CloseGame(int Game_ID)
        {
            G.db_exec("update Game set IsActive = 0 where ID = " + Game_ID);

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SetMove(int Game_ID, int MoveNumb, string MoveColor, string Move, string FEN, string PGN, string History, string Status, int Time)
        {
            string sql = string.Format(
                "Game_SetMove {0}, {1}, '{2}', '{3}', '{4}', '{5}', '{6}', {7}, '{8}'",
                        Game_ID, MoveNumb, MoveColor, Move, FEN, PGN, Status, Time, History); 

            int ID = G._I(G.db_select(sql));
            return Json(ID, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMove(int Game_ID, string MyColor)
        { 
            GMove res = new GMove(Game_ID, MyColor);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SetGameResult(int Game_ID, int MyResul)
        {
            MyGame g = new MyGame(Game_ID, CurrentUser.ID);

            int rg = CurrentUser.Account.RatingGame;
            int cc = CurrentUser.Account.ChessCoin;
            g.SetResult(MyResul);
            /**/
            G.db_exec("update Game set IsActive = 0 where ID = " + g.Game_ID);
            /**/
            int nrg = CurrentUser.Account.RatingGame;
            int ncc = CurrentUser.Account.ChessCoin;

            GameResult res = new GameResult { NewGameRating = nrg, NewChessCoin = ncc, GameRatingDelta = nrg - rg, ChessCoinDelta = ncc - cc };

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        private void SendMail()
        {
            MailAddress from = new MailAddress("nic230512@gmail.com", "Tom");
            MailAddress to = new MailAddress("andrey@mizerov.com");
            MailAddress yu = new MailAddress("yuliavm@list.ru");
            MailMessage m = new MailMessage(from, to);
            m.To.Add(yu);
            m.Subject = "CHESSNOK New Game [ " + User.Identity.Name + " ]";
            m.Body = "ИГРА с [ " + User.Identity.Name + " ] http://chessnok.com/Game";

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.UseDefaultCredentials = false;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;

            smtp.Credentials = new NetworkCredential("nic230512@gmail.com", "Mizer061290");
            try
            {
                smtp.Send(m);
            }
            catch (Exception e)
            {
                G.WriteLog(e.Message);
            }
        }
        public JsonResult SetChatMessage(int Game_ID, string SenderColor, string Message)
        {
            var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();

            string Type = "chat_message";
            object res = new { Type = Type, Game_ID = Game_ID, SenderColor = SenderColor, Message = Message };

            G.db_exec("insert GameChat(Game_ID, SenderColor, Message) values({1}, '{2}', '{3}')", Game_ID, SenderColor, Message);

            context.Clients.All.displayMessage(res);

            return Json(Message, JsonRequestBehavior.AllowGet);
        }
    }
}