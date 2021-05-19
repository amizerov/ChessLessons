using System.Data;
using System.Data.SqlClient;

namespace am.DB
{
	/// <summary>
	/// Summary description for DBManager.
	/// </summary>
	public class DBManager
	{
		#region Declarations
		// Connection settings for the application
		string _server;
		string _database;
		string _user;
		string _password;
		protected string _connectionString = string.Empty;
		protected int _connectionTimeOut;
		protected int _commandTimeOut;
		#endregion

		#region Singleton support
		private static object _lockFlag = new object();
		private static DBManager _instance;

		public DBManager()
		{
		}

		public static DBManager Instance
		{
			get
			{
				lock (_lockFlag)
				{
					if (_instance == null)
						_instance = new DBManager();
				}
				return _instance;
			}
		}

		#endregion

		#region Properties

		public string Database
		{
			get	{ return this._database; }
		}

		public string Server
		{
			get	{ return this._server; }
		}

		public string Username
		{
			get	{ return this._user; }
		}

		public string Password
		{
			get	{ return this._password; }
		}

		public int CommandTimeout
		{
			get	{ return this._commandTimeOut; }
		}

		public string ConnectionString
		{
			get { return this._connectionString; }
		}

		#endregion

		public void Init(string server, string database, string user, string password)
		{
			this._server = server;
			this._database = database;
			this._user = user;
			this._password = password;
			this._connectionTimeOut = 10;
			this._commandTimeOut = 30;
			this._connectionString = "Data Source=" + server + ";Initial Catalog=" + database + ";User ID=" + user + ";Password=" + password + ";Connection Timeout=" + this._connectionTimeOut;
		}

        public void Init(string connectionString, int connectionTimeOut = 60, int commandTimeOut = 60)
        {
          this._server = string.Empty;
          this._database = string.Empty;
          this._user = string.Empty;
          this._password = string.Empty;
          this._connectionTimeOut = connectionTimeOut;
          this._commandTimeOut = commandTimeOut;
          this._connectionString = connectionString;
        }
		
		public SqlConnection CreateConnection()
		{
			SqlConnection connection = new SqlConnection();
			connection.ConnectionString = this._connectionString;
			connection.Open();
			return connection;
		}

		public SqlCommand CreateCommand(string commandText, CommandType commandType)
		{
			return new SqlCommand(commandText, CreateConnection())
			                       {
			                         CommandType = commandType,
			                         CommandTimeout = this._commandTimeOut
			                       };
		}

		public DataSet GetDataSetFromStoredProcedure(string commandText)
		{
			SqlCommand command = CreateCommand(commandText, CommandType.StoredProcedure);
			SqlDataAdapter adapter = new SqlDataAdapter();
			adapter.SelectCommand = command;
			DataSet dataset = new DataSet();
			adapter.Fill(dataset);
			command.Connection.Close();
			return dataset;
		}

		public DataSet GetDataSetFromPreparedCommand(SqlCommand command)
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			adapter.SelectCommand = command;
			DataSet dataset = new DataSet();
			adapter.Fill(dataset);
			return dataset;
		}
	}
}
