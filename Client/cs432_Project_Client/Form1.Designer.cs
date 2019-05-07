namespace cs432_Project_Client
{
    partial class Form1
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
            this.ipAdress = new System.Windows.Forms.TextBox();
            this.portNum = new System.Windows.Forms.TextBox();
            this.username = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.usernameLogin = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.enrollButton = new System.Windows.Forms.Button();
            this.loginButton = new System.Windows.Forms.Button();
            this.logs = new System.Windows.Forms.RichTextBox();
            this.passwordLogin = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.connectButton = new System.Windows.Forms.Button();
            this.oldPass = new System.Windows.Forms.TextBox();
            this.newPass = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.changePassButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ipAdress
            // 
            this.ipAdress.Location = new System.Drawing.Point(97, 11);
            this.ipAdress.Name = "ipAdress";
            this.ipAdress.Size = new System.Drawing.Size(100, 20);
            this.ipAdress.TabIndex = 0;
            // 
            // portNum
            // 
            this.portNum.Location = new System.Drawing.Point(97, 37);
            this.portNum.Name = "portNum";
            this.portNum.Size = new System.Drawing.Size(100, 20);
            this.portNum.TabIndex = 1;
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(97, 129);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(100, 20);
            this.username.TabIndex = 2;
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(97, 155);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(100, 20);
            this.password.TabIndex = 3;
            // 
            // usernameLogin
            // 
            this.usernameLogin.Location = new System.Drawing.Point(97, 239);
            this.usernameLogin.Name = "usernameLogin";
            this.usernameLogin.Size = new System.Drawing.Size(100, 20);
            this.usernameLogin.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "IP";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Port";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Username";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 163);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Password";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 246);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Username";
            // 
            // enrollButton
            // 
            this.enrollButton.Enabled = false;
            this.enrollButton.Location = new System.Drawing.Point(97, 181);
            this.enrollButton.Name = "enrollButton";
            this.enrollButton.Size = new System.Drawing.Size(75, 23);
            this.enrollButton.TabIndex = 10;
            this.enrollButton.Text = "Enroll";
            this.enrollButton.UseVisualStyleBackColor = true;
            this.enrollButton.Click += new System.EventHandler(this.enrollButton_Click);
            // 
            // loginButton
            // 
            this.loginButton.Enabled = false;
            this.loginButton.Location = new System.Drawing.Point(97, 291);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(75, 23);
            this.loginButton.TabIndex = 11;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // logs
            // 
            this.logs.Location = new System.Drawing.Point(208, 11);
            this.logs.Name = "logs";
            this.logs.Size = new System.Drawing.Size(188, 397);
            this.logs.TabIndex = 12;
            this.logs.Text = "";
            // 
            // passwordLogin
            // 
            this.passwordLogin.Location = new System.Drawing.Point(97, 265);
            this.passwordLogin.Name = "passwordLogin";
            this.passwordLogin.Size = new System.Drawing.Size(100, 20);
            this.passwordLogin.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 272);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Password";
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(97, 63);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 23);
            this.connectButton.TabIndex = 15;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // oldPass
            // 
            this.oldPass.Location = new System.Drawing.Point(97, 362);
            this.oldPass.Name = "oldPass";
            this.oldPass.Size = new System.Drawing.Size(100, 20);
            this.oldPass.TabIndex = 16;
            // 
            // newPass
            // 
            this.newPass.Location = new System.Drawing.Point(97, 388);
            this.newPass.Name = "newPass";
            this.newPass.Size = new System.Drawing.Size(100, 20);
            this.newPass.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 365);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Old Password";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 391);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(78, 13);
            this.label8.TabIndex = 19;
            this.label8.Text = "New Password";
            // 
            // changePassButton
            // 
            this.changePassButton.Location = new System.Drawing.Point(97, 415);
            this.changePassButton.Name = "changePassButton";
            this.changePassButton.Size = new System.Drawing.Size(100, 35);
            this.changePassButton.TabIndex = 20;
            this.changePassButton.Text = "Change Password";
            this.changePassButton.UseVisualStyleBackColor = true;
            this.changePassButton.Click += new System.EventHandler(this.changePassButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 463);
            this.Controls.Add(this.changePassButton);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.newPass);
            this.Controls.Add(this.oldPass);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.passwordLogin);
            this.Controls.Add(this.logs);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.enrollButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.usernameLogin);
            this.Controls.Add(this.password);
            this.Controls.Add(this.username);
            this.Controls.Add(this.portNum);
            this.Controls.Add(this.ipAdress);
            this.Name = "Form1";
            this.Text = "Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ipAdress;
        private System.Windows.Forms.TextBox portNum;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.TextBox usernameLogin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button enrollButton;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.RichTextBox logs;
        private System.Windows.Forms.TextBox passwordLogin;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.TextBox oldPass;
        private System.Windows.Forms.TextBox newPass;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button changePassButton;
    }
}

