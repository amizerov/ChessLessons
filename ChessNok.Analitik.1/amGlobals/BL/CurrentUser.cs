using System;
using System.Data;

namespace am.BL
{
	/// <summary>
	/// Summary description for CurrentUser.
	/// </summary>
	public class U
	{
		string	_user_name = "";
		int		_user_id = 0;
		string	_roles = "";
		string	_full_name = "";

		#region Singleton support

		private static object _lockFlag = new object();
		private static U _instance;

		public U()
		{
			Load();
		}

    public static string[] def_Roles
    {
      get
      {

        string sql = "select * from def_Roles";
        DataTable dt = G.db_select(sql);

        string[] res = new string[dt.Rows.Count];
        for(int i = 0; i<dt.Rows.Count; i++)
        {
          res[i] = dt.Rows[i]["ROLE_NAME"].ToString();
        }

        return res;
      }
    }

		public static U Cur
		{
			get
			{
				lock (_lockFlag)
				{
					if (_instance == null)
						_instance = new U();
				}
				return _instance;
			}
		}

		public static void Init(string userName)
		{
			Cur.UserName = userName;
			Cur.Load();
		}

		#endregion

		void Load()
		{
			string un = UserName;
			string sql = @"
				if not exists(select * from USERS u where USER_NAME = '{1}')
					insert USERS([USER_NAME], [FULL_NAME]) values('{1}', '{1}')
				select * from USERS u where USER_NAME = '{1}'
			";

			DataTable dt = G.db_select(sql, un);

			_user_id = G._I(dt, "USER_ID"); 
			_full_name = G._S(dt, "FULL_NAME");
		}

		public string UserName
		{
			get
			{
				return _user_name;
			}
      set
      {
        _user_name = value;
        string sql = @"
          if not exists(select * from users where USER_NAME = '{1}')
            insert users (USER_NAME, FULL_NAME) values('{1}', '{1}')
        ";
        G.db_exec(sql, _user_name);
      }
		}

		public int ID
		{
			get
			{
				if(_user_id == 0)
				{
					Load();
				}

				return _user_id;
			}
		}

		public string Roles
		{
			set
			{
				_roles = "";

				if (value == "")
					BL.G.db_exec("delete x_USER_ROLE where USER_ID = {1}", this.ID);
				else
				{
					string sql = @"
						insert x_USER_ROLE (USER_ID, ROLE_ID) 
						select {1}, ROLE_ID from def_ROLES where ROLE_NAME = '{2}'
					";
					BL.G.db_exec(sql, this.ID, value);
				}
			}

			get
			{
				if(_roles.Length == 0)
				{
					string urs = "", un = UserName;

					string sql = @"
						select * from x_USER_ROLE x 
						inner join USERS u on x.USER_ID = u.USER_ID
						inner join def_ROLES r on x.ROLE_ID = r.ROLE_ID
						where USER_NAME = N'{1}'
					";
					DataTable dt = BL.G.db_select(sql, un);
					foreach (DataRow r in dt.Rows)
					{
						urs += "[" + BL.G._S(r["ROLE_NAME"]).ToUpper() + "];";
					}

					_roles = urs;
				}

				return _roles;
			}
		}

    public bool InRole(int r)
    {
      string sql = @"
						select * from x_USER_ROLE x 
						inner join USERS u on x.USER_ID = u.USER_ID
						inner join def_ROLES r on x.ROLE_ID = r.ROLE_ID
						where USER_NAME = N'{1}' and x.ROLE_ID = {2}
					";
      DataTable dt = BL.G.db_select(sql, UserName, r);

      return dt.Rows.Count > 0;
    }

    public bool InRole(string r)
    {
      string sql = @"
						select * from x_USER_ROLE x 
						inner join USERS u on x.USER_ID = u.USER_ID
						inner join def_ROLES r on x.ROLE_ID = r.ROLE_ID
						where USER_NAME = N'{1}' and r.ROLE_NAME = '{2}'
					";
      DataTable dt = BL.G.db_select(sql, UserName, r);

      return dt.Rows.Count > 0;
    }

		public string FullName
		{
			get
			{
				if(_full_name.Length == 0)
				{
					Load();
				}

				return _full_name;
			}
		}

	}
}
