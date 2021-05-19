using System;
using System.IO;
using System.Web;
using System.Data;
using System.Configuration;
using System.Windows.Forms;

[Flags]
public enum State : 
int{    
	Date=1, Comm=2, Prod=4
}


namespace am.BL
{
    public class G
	{
        static string _err = "";
        public static event Action<string> OnError;

        public static string LastError {
            get { return _err; } set { _err = value; if (_err.Length > 0 && OnError != null) OnError.Invoke(_err); }
        }

		public static string GetSqlServer()
		{
			string srv = DB.DBManager.Instance.Server;//ConfigurationSettings.AppSettings["SqlServer"];//
			return srv;
		}
		public static string GetDatabase()
		{
			string dbe = DB.DBManager.Instance.Database;//ConfigurationSettings.AppSettings["DataBase"];//
			return dbe;
		}
		public static string GetSqlLogin()
		{
			string uid = DB.DBManager.Instance.Username;//ConfigurationSettings.AppSettings["SqlLogin"];//
			return uid;
		}
		public static string GetSqlPassword()
		{
			string pwd = DB.DBManager.Instance.Password;//ConfigurationSettings.AppSettings["SqlPassword"];//
			return pwd;
		}

		public static bool db_exec(string sql, params object[] par)
		{
			LastError = "";
			DB.Connection con = new DB.Connection();
			bool res = con.exec(sql, par);
			LastError = con.LastError;
			return res;
		}

		public static DataTable db_select(string sql, params object[] par)
		{
			LastError = "";
			DB.Connection con = new DB.Connection();
			DataTable res = con.Select(sql, par);
			LastError = con.LastError;
			return res;
		}

		public static DateTime _D(object p)
		{
			return _D(p, DateTime.Now);
		}

        public static bool _B(object p)
        {
            if (p.ToString().ToLower() == "1") return true;
            if (p.ToString().ToLower() == "true") return true;
            if (p.ToString().ToLower() == "истина") return true;

            return false;
        }

        public static DateTime _D(object p, DateTime def)
		{
			DateTime r = def;
			if(p != null)
			{
				string s = p.ToString();
				if(s.Length > 0)
				{
					try
					{
						r = DateTime.Parse(s);
					}
					catch{r = def;}
				}
			}
			return r;
		}

		public static int _I(object p)
		{
			return _I(p, 0);
		}

		public static int _I(object p, int def)
		{
			int r = def;
			try
			{
				if(p.GetType().Name == "DataTable") 
				{
					DataTable dt = (DataTable)p;
					if(dt.Rows.Count > 0) r = int.Parse(dt.Rows[0][0]+"");
				}
				else if(p.GetType().Name == "DataRow") 
				{
					DataRow dr = (DataRow)p;
					r = int.Parse(dr[0]+"");
				}
				else
				{
					if(p.ToString().Length > 0)
						r = int.Parse(p.ToString());
				}
			}
			catch{r = def;}

			return r;
		}

		public static int _I(DataTable d, string f)
		{
			int r = 0;
			try
			{
				if(d.Rows.Count > 0) 
				{
					r = int.Parse(d.Rows[0][f]+"");
				}
			} catch{}

			return r;
		}

		public static int _I(DataRow dr, string f)
		{
			int r = 0;
			try
			{
				r = int.Parse(dr[f]+"");
			} 
			catch{}

			return r;
		}

		public static string _S(object p)
		{
			return _S(p, "");
		}

		public static string _S(object p, string def)
		{
			string r = def;
			try
			{
				if(p.GetType().Name == "DataTable") 
				{
					DataTable dt = (DataTable)p;
					if (dt.Rows.Count > 0)
					{
						if (def.Length > 0)
							r = dt.Rows[0][def] + "";
						else
							r = dt.Rows[0][0] + "";
					}
					else
						r = "";
				}
				else if(p.GetType().Name == "DataRow") 
				{
					DataRow dr = (DataRow)p;
					if(def.Length > 0)
						r = dr[def]+"";
					else
						r = dr[0]+"";
				}
				else
				{
					r = p.ToString();
					if(r == "") r = def;
				}
			}
			catch{r = def;}

			return r;
		}

		public static bool CheckDB()
		{
			DB.Connection con = new DB.Connection();
			try
			{
				con.GetConnection();
			}
			catch(Exception exc)
			{ 
				LastError = con.LastError+" ["+exc.Message+"]";
				return false;
			}
			LastError = con.LastError;
			return LastError.Length == 0;
		}

		public static void WriteLog(string src, string txt)
		{
			txt = "<br>\r\n--["+src+"]-------------<br>\r\n"+txt;
			WriteLog(txt);
		}
			
		public static void WriteLog(string txt)
		{
			try
			{
				//string sWriteLog = ConfigurationSettings.AppSettings["WriteLog"];
				//if(sWriteLog.ToLower() != "true") return;

				string sPath = GetCurDir();
				sPath += "\\___log";

				if(!Directory.Exists(sPath))
				{
					Directory.CreateDirectory(sPath);
				}
				DateTime dd = DateTime.Now;
				sPath += "\\log"+dd.Year+"_"+dd.Month+"_"+dd.Day+".html";

				if(!File.Exists(sPath))
				{
					File.Create(sPath).Close();
				}
				StreamWriter sw = new StreamWriter(sPath, true);
				sw.Write(txt);

				sw.WriteLine("<br>\r\n--["+DateTime.Now+"]--------------<br>\r\n");
				sw.Close();
			}
			catch(Exception ex){
				string sErr = ex.Message;
			}
		}

		public static string GetCurDir()
		{
			string cur_dir = System.Reflection.Assembly.GetCallingAssembly().Location;
			int e = cur_dir.LastIndexOf("\\");
			cur_dir = cur_dir.Remove(e, cur_dir.Length-e);
			cur_dir = cur_dir.ToLower().Replace("\\bin\\debug", "");

			return cur_dir;
		}

		public static string GetCurDir2()
		{
			string cur_dir = System.Reflection.Assembly.GetCallingAssembly().Location;
			int e = cur_dir.LastIndexOf("\\");
			cur_dir = cur_dir.Remove(e, cur_dir.Length-e);
			//cur_dir = cur_dir.ToLower().Replace("\\bin\\debug", "");

			return cur_dir+"\\";
		}

        public static bool IsDbError()
        {
            if (G.LastError.Length > 0)
            {
                return true;
            }
            return false;
        }
    }
}
