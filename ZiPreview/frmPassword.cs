using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ZiPreview
{
	public partial class frmPassword : Form
	{
		static private string _password;

		static public string GetPassword()
		{
			_password = "";
			frmPassword frm = new frmPassword();
			frm.ShowDialog();
			return _password;
		}

		public frmPassword()
		{
			InitializeComponent();
		}

		private void chkVisible_CheckedChanged(object sender, EventArgs e)
		{
			if (chkVisible.Checked)
			{
				txtPassword.PasswordChar = '\0';
			}
			else
			{
				txtPassword.PasswordChar = '*';
			}
			txtPassword.Focus();
		}

		private void cmdOK_Click(object sender, EventArgs e)
		{
			Done();
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			_password = "";
			Close();
		}

		private void frmPassword_Load(object sender, EventArgs e)
		{
			txtPassword.Text = "";
			txtPassword.PasswordChar = '\0';
			chkVisible.Checked = true;
		}

		private void txtPassword_TextChanged(object sender, EventArgs e)
		{

		}

		private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == 13)
			{
				Done();
			}
		}

		private void Done()
		{
			_password = txtPassword.Text;
			Close();
		}
	}
}
