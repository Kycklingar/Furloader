namespace Furloader
{
    partial class furaffinity
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
            this.loginButton = new System.Windows.Forms.Button();
            this.username_TxtBox = new System.Windows.Forms.TextBox();
            this.password_TxtBox = new System.Windows.Forms.TextBox();
            this.usernameText = new System.Windows.Forms.Label();
            this.passwordText = new System.Windows.Forms.Label();
            this.captcha_TxtBox = new System.Windows.Forms.TextBox();
            this.captcha_PicBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.captcha_PicBox)).BeginInit();
            this.SuspendLayout();
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(12, 223);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(260, 37);
            this.loginButton.TabIndex = 0;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // username_TxtBox
            // 
            this.username_TxtBox.Location = new System.Drawing.Point(12, 32);
            this.username_TxtBox.Name = "username_TxtBox";
            this.username_TxtBox.Size = new System.Drawing.Size(260, 20);
            this.username_TxtBox.TabIndex = 1;
            this.username_TxtBox.TextChanged += new System.EventHandler(this.username_TxtBox_TextChanged);
            // 
            // password_TxtBox
            // 
            this.password_TxtBox.Location = new System.Drawing.Point(12, 84);
            this.password_TxtBox.Name = "password_TxtBox";
            this.password_TxtBox.Size = new System.Drawing.Size(260, 20);
            this.password_TxtBox.TabIndex = 2;
            this.password_TxtBox.UseSystemPasswordChar = true;
            this.password_TxtBox.TextChanged += new System.EventHandler(this.password_TxtBox_TextChanged);
            // 
            // usernameText
            // 
            this.usernameText.AutoSize = true;
            this.usernameText.Location = new System.Drawing.Point(12, 16);
            this.usernameText.Name = "usernameText";
            this.usernameText.Size = new System.Drawing.Size(55, 13);
            this.usernameText.TabIndex = 3;
            this.usernameText.Text = "Username";
            // 
            // passwordText
            // 
            this.passwordText.AutoSize = true;
            this.passwordText.Location = new System.Drawing.Point(12, 68);
            this.passwordText.Name = "passwordText";
            this.passwordText.Size = new System.Drawing.Size(53, 13);
            this.passwordText.TabIndex = 4;
            this.passwordText.Text = "Password";
            // 
            // captcha_TxtBox
            // 
            this.captcha_TxtBox.Location = new System.Drawing.Point(80, 197);
            this.captcha_TxtBox.Name = "captcha_TxtBox";
            this.captcha_TxtBox.Size = new System.Drawing.Size(121, 20);
            this.captcha_TxtBox.TabIndex = 6;
            this.captcha_TxtBox.TextChanged += new System.EventHandler(this.captcha_TxtBox_TextChanged);
            // 
            // captcha_PicBox
            // 
            this.captcha_PicBox.Location = new System.Drawing.Point(80, 127);
            this.captcha_PicBox.Name = "captcha_PicBox";
            this.captcha_PicBox.Size = new System.Drawing.Size(120, 60);
            this.captcha_PicBox.TabIndex = 5;
            this.captcha_PicBox.TabStop = false;
            this.captcha_PicBox.Click += new System.EventHandler(this.captcha_PicBox_Click);
            // 
            // furaffinity
            // 
            this.AcceptButton = this.loginButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 275);
            this.Controls.Add(this.captcha_TxtBox);
            this.Controls.Add(this.captcha_PicBox);
            this.Controls.Add(this.passwordText);
            this.Controls.Add(this.usernameText);
            this.Controls.Add(this.password_TxtBox);
            this.Controls.Add(this.username_TxtBox);
            this.Controls.Add(this.loginButton);
            this.Name = "furaffinity";
            this.Text = "furaffinity";
            ((System.ComponentModel.ISupportInitialize)(this.captcha_PicBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.TextBox username_TxtBox;
        private System.Windows.Forms.TextBox password_TxtBox;
        private System.Windows.Forms.Label usernameText;
        private System.Windows.Forms.Label passwordText;
        private System.Windows.Forms.TextBox captcha_TxtBox;
        private System.Windows.Forms.PictureBox captcha_PicBox;
    }
}