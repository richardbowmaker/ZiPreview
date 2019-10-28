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
        private static string _password;
        private static bool _ok;

		static public bool GetPassword(ref string password)
		{
			_password = "";
			frmPassword frm = new frmPassword();
			frm.ShowDialog();
            password = _password;
            return _ok;
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
            _ok = false;
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
            _ok = true;
			_password = txtPassword.Text;
			Close();
		}
	}
}
