namespace Furloader
{
    partial class loginsForm
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
            this.furaffinity = new System.Windows.Forms.Button();
            this.inkbunny = new System.Windows.Forms.Button();
            this.pictureBox_IB = new System.Windows.Forms.PictureBox();
            this.pictureBox_FA = new System.Windows.Forms.PictureBox();
            this.button_LoginAll = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_IB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_FA)).BeginInit();
            this.SuspendLayout();
            // 
            // furaffinity
            // 
            this.furaffinity.Location = new System.Drawing.Point(34, 41);
            this.furaffinity.Name = "furaffinity";
            this.furaffinity.Size = new System.Drawing.Size(186, 23);
            this.furaffinity.TabIndex = 0;
            this.furaffinity.Text = "FurAffinity";
            this.furaffinity.UseVisualStyleBackColor = true;
            this.furaffinity.Click += new System.EventHandler(this.button1_Click);
            // 
            // inkbunny
            // 
            this.inkbunny.Location = new System.Drawing.Point(34, 70);
            this.inkbunny.Name = "inkbunny";
            this.inkbunny.Size = new System.Drawing.Size(186, 23);
            this.inkbunny.TabIndex = 1;
            this.inkbunny.Text = "Inkbunny";
            this.inkbunny.UseVisualStyleBackColor = true;
            this.inkbunny.Click += new System.EventHandler(this.inkbunny_Click);
            // 
            // pictureBox_IB
            // 
            this.pictureBox_IB.Image = global::Furloader.Properties.Resources.LoginStandby;
            this.pictureBox_IB.ImageLocation = "";
            this.pictureBox_IB.Location = new System.Drawing.Point(226, 70);
            this.pictureBox_IB.Name = "pictureBox_IB";
            this.pictureBox_IB.Size = new System.Drawing.Size(23, 23);
            this.pictureBox_IB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_IB.TabIndex = 4;
            this.pictureBox_IB.TabStop = false;
            this.pictureBox_IB.Click += new System.EventHandler(this.pictureBox_IB_Click);
            // 
            // pictureBox_FA
            // 
            this.pictureBox_FA.Image = global::Furloader.Properties.Resources.LoginStandby;
            this.pictureBox_FA.ImageLocation = "";
            this.pictureBox_FA.Location = new System.Drawing.Point(226, 41);
            this.pictureBox_FA.Name = "pictureBox_FA";
            this.pictureBox_FA.Size = new System.Drawing.Size(23, 23);
            this.pictureBox_FA.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_FA.TabIndex = 3;
            this.pictureBox_FA.TabStop = false;
            this.pictureBox_FA.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // button_LoginAll
            // 
            this.button_LoginAll.Location = new System.Drawing.Point(34, 299);
            this.button_LoginAll.Name = "button_LoginAll";
            this.button_LoginAll.Size = new System.Drawing.Size(186, 23);
            this.button_LoginAll.TabIndex = 5;
            this.button_LoginAll.Text = "Login all";
            this.button_LoginAll.UseVisualStyleBackColor = true;
            this.button_LoginAll.Click += new System.EventHandler(this.button_LoginAll_Click);
            // 
            // loginsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 334);
            this.Controls.Add(this.button_LoginAll);
            this.Controls.Add(this.pictureBox_IB);
            this.Controls.Add(this.pictureBox_FA);
            this.Controls.Add(this.inkbunny);
            this.Controls.Add(this.furaffinity);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "loginsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Login";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_IB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_FA)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button furaffinity;
        private System.Windows.Forms.Button inkbunny;
        private System.Windows.Forms.PictureBox pictureBox_FA;
        private System.Windows.Forms.PictureBox pictureBox_IB;
        private System.Windows.Forms.Button button_LoginAll;
    }
}