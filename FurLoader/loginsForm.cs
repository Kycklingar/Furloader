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
    public partial class loginsForm : Form
    {
        public WorkSheduler worker;
        public loginsForm(WorkSheduler work)
        {
            InitializeComponent();
            worker = work;
        }

        private void loginAll()
        {
            checkLogin("FA", pictureBox_FA);
            checkLogin("inkbunny", pictureBox_IB);
        }

        void checkLogin(string site, PictureBox PB)
        {
            PB.Image = Properties.Resources.LoginStandby;
            if (worker.checkLogin(site))
            {
                PB.Image = Properties.Resources.LoginOK;
            }
            else
            {
                PB.Image = Properties.Resources.LoginFailed;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (worker.getLogin("FA"))
            {
                pictureBox_FA.Image = Properties.Resources.LoginOK;
            }
            else
            {
                pictureBox_FA.Image = Properties.Resources.LoginFailed;
            }
        }

        private void inkbunny_Click(object sender, EventArgs e)
        {
            using (var form = new loginPages.LoginForm(worker, "Inkbunny"))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    Console.WriteLine("Success");
                    if (form.Success)
                    {
                        pictureBox_IB.Image = Properties.Resources.LoginOK;
                    }
                }
                else
                {
                    pictureBox_IB.Image = Properties.Resources.LoginFailed;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void update()
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            checkLogin("furaffinity", pictureBox_FA);
        }

        private void button_LoginAll_Click(object sender, EventArgs e)
        {
            loginAll();
        }

        private void pictureBox_IB_Click(object sender, EventArgs e)
        {
            checkLogin("inkbunny", pictureBox_IB);
        }
    }
}
