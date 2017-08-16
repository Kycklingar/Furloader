using System;
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

        private async void loginButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (await worker.loginSiteAsync(Text, username_TxtBox.Text, password_TxtBox.Text))
                {
                    Success = true;
                    DialogResult = DialogResult.OK;
                    Close();
                    return;
                }
                MessageBox.Show("Login Failed");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "An error occured: " + ex.Message, ex.GetType().Name);
            }
        }
    }
}
