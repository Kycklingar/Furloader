namespace Furloader
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button_go = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button_settings = new System.Windows.Forms.Button();
            this.button_login = new System.Windows.Forms.Button();
            this.textBox_politeWait = new System.Windows.Forms.TextBox();
            this.label_politeWait = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button_pause = new System.Windows.Forms.Button();
            this.label_filesDownloaded = new System.Windows.Forms.Label();
            this.label_numFiles = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBox_scraps = new System.Windows.Forms.CheckBox();
            this.label_New = new System.Windows.Forms.Label();
            this.label_newFilesDownloaded = new System.Windows.Forms.Label();
            this.button_stop = new System.Windows.Forms.Button();
            this.checkBox_nice_guy = new System.Windows.Forms.CheckBox();
            this.label_Left = new System.Windows.Forms.Label();
            this.label_leftToDownload = new System.Windows.Forms.Label();
            this.comboBox_site = new System.Windows.Forms.ComboBox();
            this.comboBox_mode = new System.Windows.Forms.ComboBox();
            this.button_refresh = new System.Windows.Forms.Button();
            this.toolTip_threads = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip_User = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // button_go
            // 
            this.button_go.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_go.Location = new System.Drawing.Point(343, 95);
            this.button_go.Name = "button_go";
            this.button_go.Size = new System.Drawing.Size(76, 23);
            this.button_go.TabIndex = 8;
            this.button_go.Text = "Go";
            this.button_go.UseVisualStyleBackColor = true;
            this.button_go.Click += new System.EventHandler(this.button_Go);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "User";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(12, 95);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(325, 20);
            this.textBox1.TabIndex = 3;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // button_settings
            // 
            this.button_settings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_settings.Location = new System.Drawing.Point(12, 271);
            this.button_settings.Name = "button_settings";
            this.button_settings.Size = new System.Drawing.Size(60, 37);
            this.button_settings.TabIndex = 11;
            this.button_settings.Text = "Settings";
            this.button_settings.UseVisualStyleBackColor = true;
            this.button_settings.Click += new System.EventHandler(this.button2_Click);
            // 
            // button_login
            // 
            this.button_login.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_login.Location = new System.Drawing.Point(359, 271);
            this.button_login.Name = "button_login";
            this.button_login.Size = new System.Drawing.Size(60, 37);
            this.button_login.TabIndex = 12;
            this.button_login.Text = "Login";
            this.button_login.UseVisualStyleBackColor = true;
            this.button_login.Click += new System.EventHandler(this.button_login_Click);
            // 
            // textBox_politeWait
            // 
            this.textBox_politeWait.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_politeWait.Location = new System.Drawing.Point(343, 46);
            this.textBox_politeWait.Name = "textBox_politeWait";
            this.textBox_politeWait.Size = new System.Drawing.Size(45, 20);
            this.textBox_politeWait.TabIndex = 5;
            this.textBox_politeWait.Text = "2500";
            this.textBox_politeWait.TextChanged += new System.EventHandler(this.textBox_politeWait_TextChanged);
            // 
            // label_politeWait
            // 
            this.label_politeWait.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_politeWait.AutoSize = true;
            this.label_politeWait.Location = new System.Drawing.Point(340, 30);
            this.label_politeWait.Name = "label_politeWait";
            this.label_politeWait.Size = new System.Drawing.Size(51, 13);
            this.label_politeWait.TabIndex = 7;
            this.label_politeWait.Text = "Wait (ms)";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(12, 144);
            this.progressBar1.Maximum = 0;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(407, 23);
            this.progressBar1.TabIndex = 8;
            this.progressBar1.Validated += new System.EventHandler(this.progressBar1_Validated);
            // 
            // button_pause
            // 
            this.button_pause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_pause.Location = new System.Drawing.Point(343, 173);
            this.button_pause.Name = "button_pause";
            this.button_pause.Size = new System.Drawing.Size(75, 23);
            this.button_pause.TabIndex = 10;
            this.button_pause.Text = "Pause";
            this.button_pause.UseVisualStyleBackColor = true;
            this.button_pause.Click += new System.EventHandler(this.button_pause_Click);
            // 
            // label_filesDownloaded
            // 
            this.label_filesDownloaded.AutoSize = true;
            this.label_filesDownloaded.Location = new System.Drawing.Point(59, 187);
            this.label_filesDownloaded.Name = "label_filesDownloaded";
            this.label_filesDownloaded.Size = new System.Drawing.Size(13, 13);
            this.label_filesDownloaded.TabIndex = 11;
            this.label_filesDownloaded.Text = "0";
            // 
            // label_numFiles
            // 
            this.label_numFiles.AutoSize = true;
            this.label_numFiles.Location = new System.Drawing.Point(59, 170);
            this.label_numFiles.Name = "label_numFiles";
            this.label_numFiles.Size = new System.Drawing.Size(13, 13);
            this.label_numFiles.TabIndex = 12;
            this.label_numFiles.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 187);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Done";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 170);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Total";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // checkBox_scraps
            // 
            this.checkBox_scraps.AutoSize = true;
            this.checkBox_scraps.Location = new System.Drawing.Point(12, 121);
            this.checkBox_scraps.Name = "checkBox_scraps";
            this.checkBox_scraps.Size = new System.Drawing.Size(59, 17);
            this.checkBox_scraps.TabIndex = 4;
            this.checkBox_scraps.Text = "Scraps";
            this.checkBox_scraps.UseVisualStyleBackColor = true;
            // 
            // label_New
            // 
            this.label_New.AutoSize = true;
            this.label_New.Location = new System.Drawing.Point(12, 204);
            this.label_New.Name = "label_New";
            this.label_New.Size = new System.Drawing.Size(29, 13);
            this.label_New.TabIndex = 17;
            this.label_New.Text = "New";
            // 
            // label_newFilesDownloaded
            // 
            this.label_newFilesDownloaded.AutoSize = true;
            this.label_newFilesDownloaded.Location = new System.Drawing.Point(59, 204);
            this.label_newFilesDownloaded.Name = "label_newFilesDownloaded";
            this.label_newFilesDownloaded.Size = new System.Drawing.Size(13, 13);
            this.label_newFilesDownloaded.TabIndex = 18;
            this.label_newFilesDownloaded.Text = "0";
            // 
            // button_stop
            // 
            this.button_stop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_stop.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_stop.Enabled = false;
            this.button_stop.Location = new System.Drawing.Point(262, 173);
            this.button_stop.Name = "button_stop";
            this.button_stop.Size = new System.Drawing.Size(75, 23);
            this.button_stop.TabIndex = 9;
            this.button_stop.Text = "Stop";
            this.button_stop.UseVisualStyleBackColor = true;
            this.button_stop.Click += new System.EventHandler(this.button_stop_Click);
            // 
            // checkBox_nice_guy
            // 
            this.checkBox_nice_guy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_nice_guy.AutoSize = true;
            this.checkBox_nice_guy.Checked = true;
            this.checkBox_nice_guy.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_nice_guy.Location = new System.Drawing.Point(343, 72);
            this.checkBox_nice_guy.Name = "checkBox_nice_guy";
            this.checkBox_nice_guy.Size = new System.Drawing.Size(88, 17);
            this.checkBox_nice_guy.TabIndex = 6;
            this.checkBox_nice_guy.Text = "Mr. Nice Guy";
            this.checkBox_nice_guy.UseVisualStyleBackColor = true;
            this.checkBox_nice_guy.CheckedChanged += new System.EventHandler(this.checkBox_nice_guy_CheckedChanged);
            // 
            // label_Left
            // 
            this.label_Left.AutoSize = true;
            this.label_Left.Location = new System.Drawing.Point(12, 221);
            this.label_Left.Name = "label_Left";
            this.label_Left.Size = new System.Drawing.Size(25, 13);
            this.label_Left.TabIndex = 21;
            this.label_Left.Text = "Left";
            // 
            // label_leftToDownload
            // 
            this.label_leftToDownload.AutoSize = true;
            this.label_leftToDownload.Location = new System.Drawing.Point(59, 221);
            this.label_leftToDownload.Name = "label_leftToDownload";
            this.label_leftToDownload.Size = new System.Drawing.Size(13, 13);
            this.label_leftToDownload.TabIndex = 22;
            this.label_leftToDownload.Text = "0";
            // 
            // comboBox_site
            // 
            this.comboBox_site.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_site.FormattingEnabled = true;
            this.comboBox_site.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.comboBox_site.Items.AddRange(new object[] {
            "FurAffinity",
            "InkBunny"});
            this.comboBox_site.Location = new System.Drawing.Point(12, 46);
            this.comboBox_site.MinimumSize = new System.Drawing.Size(50, 0);
            this.comboBox_site.Name = "comboBox_site";
            this.comboBox_site.Size = new System.Drawing.Size(168, 21);
            this.comboBox_site.TabIndex = 1;
            // 
            // comboBox_mode
            // 
            this.comboBox_mode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_mode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_mode.FormattingEnabled = true;
            this.comboBox_mode.Items.AddRange(new object[] {
            "Subscription",
            "Search",
            "Gallery",
            "Watchlist"});
            this.comboBox_mode.Location = new System.Drawing.Point(186, 45);
            this.comboBox_mode.MinimumSize = new System.Drawing.Size(50, 0);
            this.comboBox_mode.Name = "comboBox_mode";
            this.comboBox_mode.Size = new System.Drawing.Size(142, 21);
            this.comboBox_mode.TabIndex = 2;
            this.comboBox_mode.SelectedIndexChanged += new System.EventHandler(this.comboBox_mode_SelectedIndexChanged);
            // 
            // button_refresh
            // 
            this.button_refresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_refresh.BackgroundImage = global::Furloader.Properties.Resources.refresh;
            this.button_refresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_refresh.Location = new System.Drawing.Point(394, 41);
            this.button_refresh.Name = "button_refresh";
            this.button_refresh.Size = new System.Drawing.Size(30, 30);
            this.button_refresh.TabIndex = 7;
            this.button_refresh.UseVisualStyleBackColor = true;
            this.button_refresh.Click += new System.EventHandler(this.button_refresh_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this.button_go;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_stop;
            this.ClientSize = new System.Drawing.Size(431, 320);
            this.Controls.Add(this.button_refresh);
            this.Controls.Add(this.comboBox_mode);
            this.Controls.Add(this.comboBox_site);
            this.Controls.Add(this.label_leftToDownload);
            this.Controls.Add(this.label_Left);
            this.Controls.Add(this.checkBox_nice_guy);
            this.Controls.Add(this.button_stop);
            this.Controls.Add(this.label_newFilesDownloaded);
            this.Controls.Add(this.label_New);
            this.Controls.Add(this.checkBox_scraps);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label_numFiles);
            this.Controls.Add(this.label_filesDownloaded);
            this.Controls.Add(this.button_pause);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label_politeWait);
            this.Controls.Add(this.textBox_politeWait);
            this.Controls.Add(this.button_login);
            this.Controls.Add(this.button_settings);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_go);
            this.MinimumSize = new System.Drawing.Size(447, 325);
            this.Name = "MainForm";
            this.Text = "FurLoader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_go;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button_settings;
        private System.Windows.Forms.Button button_login;
        private System.Windows.Forms.TextBox textBox_politeWait;
        private System.Windows.Forms.Label label_politeWait;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button button_pause;
        private System.Windows.Forms.Label label_filesDownloaded;
        private System.Windows.Forms.Label label_numFiles;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBox_scraps;
        private System.Windows.Forms.Label label_New;
        private System.Windows.Forms.Label label_newFilesDownloaded;
        private System.Windows.Forms.Button button_stop;
        private System.Windows.Forms.CheckBox checkBox_nice_guy;
        private System.Windows.Forms.Label label_Left;
        private System.Windows.Forms.Label label_leftToDownload;
        private System.Windows.Forms.ComboBox comboBox_site;
        private System.Windows.Forms.ComboBox comboBox_mode;
        private System.Windows.Forms.Button button_refresh;
        private System.Windows.Forms.ToolTip toolTip_threads;
        private System.Windows.Forms.ToolTip toolTip_User;
    }
}

