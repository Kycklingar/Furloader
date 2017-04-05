using Furloader.Sites;
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
    public delegate void ChangedGalleryEvent(object sender, EventArgs e);
    public delegate void GetImage(object sender, EventArgs e);

    // This is a experimental gallery viewer
    // Currently unused
    public partial class Gallery : Form
    {
        WorkSheduler worker;


        List<Panel> panels = new List<Panel>();
        

        public Gallery(WorkSheduler work)
        {
            InitializeComponent();
            worker = work;
            worker.updateGallery += new ChangedGalleryEvent(updateGallery);
        }

        private void updateGallery(object sender, EventArgs e)
        {
            Website obj = (Website)sender;
            while (true)
            {
                try
                {
                    Submission sub = obj.getNextImage();
                
                    if(sub.title == null)
                    {
                        break;
                    }

                ThumbnailPanel panel = new ThumbnailPanel(sub, obj);
                panel.Size = new Size(200, 240);
                panel.BackColor = Color.FromArgb(50, 0, 0, 0);
                panel.BackgroundImage = sub.thumbnail;
                panel.BackgroundImageLayout = ImageLayout.Center;
                panel.Click += Panel_Click;
                panel.DoubleClick += Panel_DoubleClick;

                Label label = new Label();
                label.Size = new Size(200, 20);
                label.Anchor = AnchorStyles.Bottom;
                label.Text = sub.title;
                label.Location = new Point(0, 220);

                panel.Controls.Add(label);

                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        flowLayoutPanel1.Controls.Add(panel);
                    }));
                }
                }
                catch (Exception se)
                {
                    Console.WriteLine(se);
                }
            }
            
        }

        private void Panel_DoubleClick(object sender, EventArgs e)
        {
            ThumbnailPanel obj = (ThumbnailPanel)sender;
            obj.site.getImage(obj.sub.pageSource.ToString());
            obj.BackColor = Color.FromArgb(255, 100, 200, 200);
        }

        private void Panel_Click(object sender, EventArgs e)
        {
            ThumbnailPanel obj = (ThumbnailPanel)sender;
            obj.BackColor = Color.FromArgb(200, 255, 0, 0);

            if (Form.ModifierKeys == Keys.Control)
            {             
                panels.Add(obj);
            }
            else
            {
                foreach(ThumbnailPanel panel in panels)
                {
                    panel.BackColor = Color.FromArgb(50, 0, 0, 0);
                }
                panels.Clear();
                panels.Add(obj);
            }
            
        }

        public void openImage(object sender, EventArgs e)
        {

        }

        public void getUserGallery(string user, string siteString)
        {
            string site;
            switch(siteString)
            {
                case "FurAffinity":
                    site = "FA";
                    break;
                case "Inkbunny":
                    site = "IB";
                    break;
                default:
                    site = "null";
                    break;
            }

           // worker.getSubmissions(user, site);
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        void drawThumbnail(Bitmap image)
        {

        }
    }

    public class ThumbnailPanel : Panel
    {
        public Submission sub;
        public Website site;
        public ThumbnailPanel(Submission submission, Website siteorigin)
        {
            sub = submission;
            site = siteorigin;
        }


    }
}
