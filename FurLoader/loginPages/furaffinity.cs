using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Furloader
{
    // This is because FA uses a captcha.
    public partial class furaffinity : Form
    {

        
        public furaffinity()
        {
            InitializeComponent();
        
        }

        private void username_TxtBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void password_TxtBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void captcha_TxtBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void setCaptcha(Image image)
        {
            captcha_PicBox.Image = image;
        }

        private void captcha_PicBox_Click(object sender, EventArgs e)
        {

        }

        

        public List<string> getInputs()
        {
            string username = username_TxtBox.Text;
            string password = password_TxtBox.Text;
            string captcha = captcha_TxtBox.Text;
            List<string> list = new List<string>();
            list.Add(username);
            list.Add(password);
            list.Add(captcha);
            return list;

        }
    }
}
