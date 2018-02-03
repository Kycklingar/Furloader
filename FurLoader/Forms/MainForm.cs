using System;
using System.Windows.Forms;
using System.Threading;

namespace Furloader
{

    // TODO: Separate modes to individual sites
    enum MODE
    {
        subscription,
        gallery,
        watchlist,
        refresh
    }

    // This is the main form. TODO: Rename
    public partial class MainForm : Form
    {
        public WorkSheduler worker;
        private bool isClosing = false;
        private int numFiles = 0;
        private int totalFiles = 0;
        private int newFiles = 0;
        private int remaining = 0;
        private int failures = 0;

        private string waitText = "2500";
        private string threadsText = "1";

        public MainForm(WorkSheduler work)
        {
            InitializeComponent();
            worker = work;
            worker.updateProg += new updateProgress(updateProgressbar);
            comboBox_site.SelectedIndex = 0;
            comboBox_mode.SelectedIndex = 0;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button_Go(object sender, EventArgs e)
        {
            ///*
            //string site = (string)comboBox_site.SelectedItem;
            //int mode = getModeFromString((string)comboBox_mode.SelectedItem);
            //string user = textBox1.Text;

            //int wait = Convert.ToInt32(textBox_politeWait.Text);
            //bool scraps = checkBox_scraps.Checked;
            //bool nice = checkBox_nice_guy.Checked;


            //if (user != "")
            //{
            //    button_stop.Enabled = true;
            //}

            //worker.downloadAll(user, mode, site, wait, scraps, nice);
            //*/

            if (!updateThreadCounts())
            {
                return;
            }

            switch ((string)comboBox_mode.SelectedItem)
            {
                case "Search":
                    string searchString = textBox1.Text;
                    if (searchString == "" || searchString.Replace(" ", "") == "")
                    {
                        toolTip_User.Show("Query cannot be empty", textBox1, 3000);
                        return;
                    }
                    worker.startSearch(searchString, (string)comboBox_site.SelectedItem, checkBox_scraps.Checked);
                    break;
                case "Subscription":
                    int pages = 10;
                    Int32.TryParse(textBox1.Text, out pages);
                    worker.startSubscriptions(pages, (string)comboBox_site.SelectedItem, checkBox_scraps.Checked);
                    break;
                case "Gallery":
                    if (textBox1.Text == "" || textBox1.Text.Replace(" ", "") == "")
                    {
                        toolTip_User.Show("User can't be empty", textBox1, 3000);
                        return;
                    }
                    worker.getSubmissions(textBox1.Text, (string)comboBox_site.SelectedItem, checkBox_scraps.Checked);
                    break;
                case "Watchlist":
                    worker.downloadWatchList(textBox1.Text, (string)comboBox_site.SelectedItem, checkBox_scraps.Checked);
                    break;
                default:
                    Console.WriteLine("Error. Couldn't decide mode.");
                    break;

            }

        }

        private int getModeFromString(string item)
        {
            switch (item)
            {
                case "Gallery":
                    return (int)MODE.gallery;
                case "Subscription":
                    return (int)MODE.subscription;
                case "Watchlist":
                    return (int)MODE.watchlist;
                case "Update wait/threads":
                    return (int)MODE.refresh;
                default:
                    return (int)MODE.gallery;
            }
        }

        public void updateProgressbar(object sender, EventArgs e)
        {
            if (!isClosing)
            {
                if (e is ProgressFail)
                {
                    if (InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(delegate
                        {
                            failures++;
                            failCountLabel.Text = failures.ToString();
                            if (failures > 0)
                            {
                                failButton.Enabled = true;
                            }
                        }));
                    }
                }
                else
                {
                    ProgressArg progress = (ProgressArg)e;

                    if (!progress.files)
                    {

                        if (InvokeRequired)
                        {
                            this.Invoke(new MethodInvoker(delegate
                            {
                                totalFiles += progress.amount;
                                remaining += progress.amount;


                                button_stop.Enabled = true;
                                progressBar1.Maximum = remaining;
                                progressBar1.Value = 0;
                                label_numFiles.Text = totalFiles.ToString();
                                label_leftToDownload.Text = remaining.ToString();
                            }));
                        }
                    }
                    else
                    {
                        if (InvokeRequired)
                        {
                            this.Invoke(new MethodInvoker(delegate
                            {
                                progressBar1.Increment(1);
                                numFiles++;
                                remaining--;
                                label_leftToDownload.Text = remaining.ToString();
                                if (remaining <= 0)
                                {
                                    progressBar1.Maximum = 0;
                                    progressBar1.Value = 0;
                                    button_stop.Enabled = false;
                                }

                                label_filesDownloaded.Text = numFiles.ToString();

                                if (progress.newFiles)
                                {
                                    newFiles++;
                                    label_newFilesDownloaded.Text = newFiles.ToString();
                                }
                            }));
                        }
                    }
                }


            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button_login_Click(object sender, EventArgs e)
        {
            new loginsForm(worker).ShowDialog();
        }

        private void textBox_politeWait_TextChanged(object sender, EventArgs e)
        {
            if (checkBox_nice_guy.Checked)
            {
                waitText = textBox_politeWait.Text;
            }
            else
            {
                threadsText = textBox_politeWait.Text;
            }
        }

        private void button_pause_Click(object sender, EventArgs e)
        {
            if (worker.togglePause())
                button_pause.Text = "Resume";
            else
                button_pause.Text = "Pause";
        }

        private void progressBar1_Validated(object sender, EventArgs e)
        {
            Console.WriteLine("All Done!");
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            button_go.Enabled = false;
            Thread thread = new Thread(new ThreadStart(stopDownloads));
            thread.Start();
        }

        private void stopDownloads()
        {

            worker.stopExecution();
            if (!isClosing)
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        button_pause.Text = "Pause";
                        button_stop.Enabled = false;
                        button_go.Enabled = true;
                        totalFiles = numFiles;
                        label_numFiles.Text = numFiles.ToString();
                        progressBar1.Value = 0;
                        progressBar1.Maximum = 0;
                        remaining = 0;
                        label_leftToDownload.Text = remaining.ToString();
                    }));
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            isClosing = true;
            Thread thread = new Thread(new ThreadStart(stopDownloads));
            thread.IsBackground = false;
            thread.Start();

        }

        private void checkBox_nice_guy_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_nice_guy.Checked)
            {
                label_politeWait.Text = "Wait (ms)";
                textBox_politeWait.Text = waitText;
            }
            else
            {
                label_politeWait.Text = "Threads";
                textBox_politeWait.Text = threadsText;
            }
        }

        private void comboBox_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            //textBox1.Text = "";
            switch ((string)comboBox_mode.SelectedItem)
            {
                case "Subscription":
                    label1.Text = "Pages";
                    break;
                case "Search":
                    label1.Text = "Query";
                    break;
                case "Gallery":
                    label1.Text = "User";
                    break;
                default:
                    label1.Text = "User";
                    break;
            }
        }

        private void button_refresh_Click(object sender, EventArgs e)
        {
            updateThreadCounts();
        }

        private bool updateThreadCounts()
        {
            int suc;
            if (!Int32.TryParse(textBox_politeWait.Text, out suc))
            {
                toolTip_threads.Show("Must be numerical value.", textBox_politeWait, 5000);
                return false;
            }
            worker.changeThreadCount(!checkBox_nice_guy.Checked, suc);
            return true;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void failButton_Click(object sender, EventArgs e)
        {
            remaining += failures;
            label_leftToDownload.Text = remaining.ToString();
            failures = 0;
            failCountLabel.Text = failures.ToString();
            failButton.Enabled = false;

            Thread thread = new Thread(() => worker.retryFailures());
            thread.IsBackground = true;
            thread.Start();
        }
    }
}
