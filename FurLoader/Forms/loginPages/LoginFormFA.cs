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

    public partial class LoginFormFA : Form
    {

        public bool Success = false;
        private WorkSheduler worker;
        public LoginFormFA(WorkSheduler worker_, string title)
        {
            worker = worker_;
            InitializeComponent();
            Text = title;
            Sites.LoginData data = worker.GetLoginData("furaffinity");
            captcha_PicBox.Image = data.Captcha;
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            if (worker.loginSite(Text, username_TxtBox.Text, password_TxtBox.Text, captcha_TxtBox.Text))
            {
                Success = true;
                DialogResult = DialogResult.OK;
                Close();
                return;
            }
            MessageBox.Show("Login Failed");
            Sites.LoginData data = worker.GetLoginData("furaffinity");
            captcha_PicBox.Image = data.Captcha;
        }
    }
}
