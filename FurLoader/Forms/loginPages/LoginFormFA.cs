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
        }

        private async void LoginFormFA_Load(object sender, EventArgs e) {
            try
            {
                Sites.LoginData data = await worker.GetLoginDataAsync("furaffinity");
                captcha_PicBox.Image = data.Captcha;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Could not load captcha: " + ex.Message, ex.GetType().Name);
            }
        }

        private async void loginButton_Click(object sender, EventArgs e)
        {
            loginButton.Enabled = false;
            try
            {
                if (await worker.loginSiteAsync(Text, username_TxtBox.Text, password_TxtBox.Text, captcha_TxtBox.Text))
                {
                    Success = true;
                    DialogResult = DialogResult.OK;
                    Close();
                    return;
                }
                MessageBox.Show("Login Failed");
                Sites.LoginData data = await worker.GetLoginDataAsync("furaffinity");
                captcha_PicBox.Image = data.Captcha;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "An error occured: " + ex.Message, ex.GetType().Name);
            }
            loginButton.Enabled = true;
        }
    }
}
