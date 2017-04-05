using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Furloader.loginPages
{
    public partial class LoginForm : Form
    {
        WorkSheduler worker;
        public bool Success = false;
        public LoginForm(WorkSheduler work, string Title)
        {
            worker = work;
            InitializeComponent();
            Text = Title;
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            if (worker.loginSite(Text, username_TxtBox.Text, password_TxtBox.Text))
            {
                Success = true;
                DialogResult = DialogResult.OK;
                Close();
                return;
            }
            MessageBox.Show("Login Failed");
        }
    }
}
