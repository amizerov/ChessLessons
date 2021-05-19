using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace am
{
  public partial class FormLogin : Form
  {
    public String Server;
    public String Database;
    public String Login;
    public String Password;

    public FormLogin()
    {
      InitializeComponent();
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      Server = txtServer.Text;
      Database = txtDatabase.Text;
      Login = txtLogin.Text;
      Password = txtPassword.Text;

      this.DialogResult = DialogResult.OK;
      this.Hide();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

		private void FormLogin_Load(object sender, EventArgs e)
		{

			txtServer.Text = Server;
			txtDatabase.Text = Database;
			txtLogin.Text = Login;
			txtPassword.Text = Password;

			txtPassword.Focus();

		}

		private void FormLogin_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				btnOk_Click(null, null);
			}

			if (e.KeyCode == Keys.Escape)
			{
				btnCancel_Click(null, null);
			}
		}
  }
}