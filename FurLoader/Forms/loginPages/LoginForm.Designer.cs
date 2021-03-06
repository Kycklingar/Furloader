﻿namespace Furloader.loginPages
{
    partial class LoginForm
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
            this.passwordText = new System.Windows.Forms.Label();
            this.usernameText = new System.Windows.Forms.Label();
            this.password_TxtBox = new System.Windows.Forms.TextBox();
            this.username_TxtBox = new System.Windows.Forms.TextBox();
            this.loginButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // passwordText
            // 
            this.passwordText.AutoSize = true;
            this.passwordText.Location = new System.Drawing.Point(12, 60);
            this.passwordText.Name = "passwordText";
            this.passwordText.Size = new System.Drawing.Size(53, 13);
            this.passwordText.TabIndex = 11;
            this.passwordText.Text = "Password";
            // 
            // usernameText
            // 
            this.usernameText.AutoSize = true;
            this.usernameText.Location = new System.Drawing.Point(12, 8);
            this.usernameText.Name = "usernameText";
            this.usernameText.Size = new System.Drawing.Size(55, 13);
            this.usernameText.TabIndex = 10;
            this.usernameText.Text = "Username";
            // 
            // password_TxtBox
            // 
            this.password_TxtBox.Location = new System.Drawing.Point(12, 76);
            this.password_TxtBox.Name = "password_TxtBox";
            this.password_TxtBox.Size = new System.Drawing.Size(260, 20);
            this.password_TxtBox.TabIndex = 9;
            this.password_TxtBox.UseSystemPasswordChar = true;
            // 
            // username_TxtBox
            // 
            this.username_TxtBox.Location = new System.Drawing.Point(12, 24);
            this.username_TxtBox.Name = "username_TxtBox";
            this.username_TxtBox.Size = new System.Drawing.Size(260, 20);
            this.username_TxtBox.TabIndex = 8;
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(12, 125);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(260, 37);
            this.loginButton.TabIndex = 7;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // LoginForm
            // 
            this.AcceptButton = this.loginButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 186);
            this.Controls.Add(this.passwordText);
            this.Controls.Add(this.usernameText);
            this.Controls.Add(this.password_TxtBox);
            this.Controls.Add(this.username_TxtBox);
            this.Controls.Add(this.loginButton);
            this.Name = "LoginForm";
            this.Text = "LoginForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label passwordText;
        private System.Windows.Forms.Label usernameText;
        private System.Windows.Forms.TextBox password_TxtBox;
        private System.Windows.Forms.TextBox username_TxtBox;
        private System.Windows.Forms.Button loginButton;
    }
}