using System;
using System.Data;
using System.Data.SqlClient;

namespace am.DB
{
	/// <summary>
	/// Summary description for dataopen.
	/// </summary>
	public class Connection
	{
		private SqlConnection m_oConnection = null;
		private string m_sConnectionString;
		private string m_sLastError = "";
		private int m_nConRefCnt = 0;

		public string LastError
		{
			get
			{ 
				return m_sLastError;
			}
		}

		public Connection(string cs)
		{
			m_sConnectionString = cs;
		}

		public Connection()
		{
			//m_sConnectionString = ConfigurationSettings.AppSettings["SqlConnectionString"];
			/*
			string srv = BL.G.GetSqlServer();
			string dbe = BL.G.GetDatabase();
			string uid = BL.G.GetSqlLogin();
			string pwd = BL.G.GetSqlPassword();

			m_sConnectionString = "Server="+srv+";Database="+dbe+";UID="+uid+";PWD="+pwd+";";
       */
		  m_sConnectionString = DBManager.Instance.ConnectionString;
		}

		public void Close()
		{
			CloseConnection();
		}

		public SqlConnection GetConnection()
		{
			m_sLastError = "";
			SqlConnection oConnection = null;
			try
			{
				oConnection = new SqlConnection(m_sConnectionString);
				oConnection.Open();	

				if(oConnection.State != ConnectionState.Open)
				{
					m_sLastError = "Error open connection.";
					WriteLog(11, m_sLastError, m_sConnectionString);
				}
			}
			catch(Exception ex)
			{
				m_sLastError = ex.Message;
				WriteLog(1, m_sLastError, m_sConnectionString);
			}
			return oConnection;
		}

		private bool OpenConnection()
		{
			m_sLastError = "";
			try
			{
				CloseConnection();

				m_oConnection = new SqlConnection(m_sConnectionString);
				m_oConnection.Open();	

				if(m_oConnection.State != ConnectionState.Open)
				{
					m_sLastError = "Error open connection.";
					WriteLog(111, m_sLastError, m_sConnectionString);
					
					return false;
				}
	
				m_nConRefCnt++;
			}
			catch(Exception ex)
			{
				m_sLastError = ex.Message;
				WriteLog(12, m_sLastError, m_sConnectionString);

				return false;
			}
			return true;
		}

		private void CloseConnection()
		{
			if(m_oConnection == null) return;
			if(m_oConnection.State == ConnectionState.Closed) return;
			
			try
			{
				m_oConnection.Close();
				m_oConnection.Dispose();
				m_oConnection = null;	

				m_nConRefCnt--;
			}
			catch(Exception ex)
			{
				m_sLastError = ex.Message;
				WriteLog(0, m_sLastError);
			}
		}

		private void CloseConnection(SqlConnection oConnection)
		{
			if(oConnection == null) return;
			if(oConnection.State == ConnectionState.Closed) return;
			
			try
			{
				oConnection.Close();
				oConnection.Dispose();
				oConnection = null;	
			}
			catch(Exception ex)
			{
				m_sLastError = ex.Message;
				WriteLog(101, m_sLastError);
			}
		}

		public DataTable Select(string sql, params object[] par)
		{
			if(par != null && par.Length > 0) 
			{
				int i = 0; 
				foreach(object s in par) 
				{
					sql = sql.Replace("{"+(++i)+"}", s+"");
				}
			}
			return _Select(sql);
		}

		private DataTable _Select(string q)
		{
			int ln = 0;
			m_sLastError = "";
			SqlDataAdapter adp = null;
			DataTable dt = new DataTable();

			if (!OpenConnection()) return dt;
			try
			{	
				adp = new SqlDataAdapter(q, m_oConnection);	ln = 1;
				dt = new DataTable(); adp.Fill(dt);				ln = 2;
			}
			catch(Exception ex)
			{
				m_sLastError = "("+ln+") "+ex.Message;
				WriteLog(2, m_sLastError, q);
			}
			finally
			{
				CloseConnection();			
			}
			return dt;
		}

		public DataTable Select(string q, ref int count)
		{
			m_sLastError = "";
			DataTable dt = new DataTable();
			if(!OpenConnection()) return dt;
			try
			{
				SqlDataAdapter adp = new SqlDataAdapter(q, m_oConnection);									  					
				count = adp.Fill(dt); 
			}				
			catch(Exception ex)
			{
				m_sLastError = ex.Message;
				WriteLog(3, m_sLastError, q);
			}
			finally
			{
				CloseConnection();			
			}
			return dt;
		}

		public bool exec(string sql, params object[] par)
		{
			int i=0;
			foreach(object s in par)
			{
				sql = sql.Replace("{"+(++i)+"}", s+"");
			}
			return exec(sql);
		}

		public bool exec(string sql)
		{
			m_sLastError = "";
			bool success = true;
			if(!OpenConnection()) return false;
			try
			{				
				SqlCommand command = new SqlCommand(sql, m_oConnection);            	
				command.CommandType = System.Data.CommandType.Text;            
				command.ExecuteNonQuery();				
			}
			catch(Exception ex)
			{
				m_sLastError = ex.Message;
				WriteLog(4, m_sLastError, sql);

				success = false;
			}
			finally
			{	
				CloseConnection();			
			}					
	
			return success;
		}

		private void WriteLog(int n, string txt)
		{
			WriteLog(n, txt, "");
		}

		private void WriteLog(int n, string txt, string sql)
		{
			txt = "<br>\r\n--[datopen("+n+")("+m_nConRefCnt+")]-------------<br>\r\n"+txt;
			if(sql.Length > 0) txt += "<br>\r\n"+sql;
			BL.G.WriteLog(txt);
		}


	}
}
