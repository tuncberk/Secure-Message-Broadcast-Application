namespace cs432_Project_Server
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
            this.clientPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listenButton = new System.Windows.Forms.Button();
            this.logs = new System.Windows.Forms.RichTextBox();
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.oldPass = new System.Windows.Forms.TextBox();
            this.newPass = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.changePassButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // clientPort
            // 
            this.clientPort.Location = new System.Drawing.Point(100, 15);
            this.clientPort.Name = "clientPort";
            this.clientPort.Size = new System.Drawing.Size(100, 20);
            this.clientPort.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Port";
            // 
            // listenButton
            // 
            this.listenButton.Enabled = false;
            this.listenButton.Location = new System.Drawing.Point(100, 41);
            this.listenButton.Name = "listenButton";
            this.listenButton.Size = new System.Drawing.Size(75, 23);
            this.listenButton.TabIndex = 6;
            this.listenButton.Text = "Listen";
            this.listenButton.UseVisualStyleBackColor = true;
            this.listenButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // logs
            // 
            this.logs.Location = new System.Drawing.Point(206, 12);
            this.logs.Name = "logs";
            this.logs.Size = new System.Drawing.Size(246, 246);
            this.logs.TabIndex = 8;
            this.logs.Text = "";
            // 
            // passwordBox
            // 
            this.passwordBox.Location = new System.Drawing.Point(100, 91);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.Size = new System.Drawing.Size(100, 20);
            this.passwordBox.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Password";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(100, 117);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "Submit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // oldPass
            // 
            this.oldPass.Location = new System.Drawing.Point(100, 165);
            this.oldPass.Name = "oldPass";
            this.oldPass.Size = new System.Drawing.Size(100, 20);
            this.oldPass.TabIndex = 12;
            // 
            // newPass
            // 
            this.newPass.Location = new System.Drawing.Point(100, 191);
            this.newPass.Name = "newPass";
            this.newPass.Size = new System.Drawing.Size(100, 20);
            this.newPass.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 168);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Old Password";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 195);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "New Password";
            // 
            // changePassButton
            // 
            this.changePassButton.Location = new System.Drawing.Point(100, 218);
            this.changePassButton.Name = "changePassButton";
            this.changePassButton.Size = new System.Drawing.Size(75, 40);
            this.changePassButton.TabIndex = 16;
            this.changePassButton.Text = "Change Password";
            this.changePassButton.UseVisualStyleBackColor = true;
            this.changePassButton.Click += new System.EventHandler(this.changePassButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 264);
            this.Controls.Add(this.changePassButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.newPass);
            this.Controls.Add(this.oldPass);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.passwordBox);
            this.Controls.Add(this.logs);
            this.Controls.Add(this.listenButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.clientPort);
            this.Name = "Form1";
            this.Text = "Server";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox clientPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button listenButton;
        private System.Windows.Forms.RichTextBox logs;
        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox oldPass;
        private System.Windows.Forms.TextBox newPass;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button changePassButton;
    }
}

