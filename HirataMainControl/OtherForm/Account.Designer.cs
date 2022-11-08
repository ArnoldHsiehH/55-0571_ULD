namespace HirataMainControl
{
    partial class Account
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Account));
            this.TabAccount = new System.Windows.Forms.TabControl();
            this.TabLogin = new System.Windows.Forms.TabPage();
            this.LbLoginMessage = new System.Windows.Forms.Label();
            this.BtnOK = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.GbAccount = new System.Windows.Forms.GroupBox();
            this.LbPassword = new System.Windows.Forms.Label();
            this.TbPassword = new System.Windows.Forms.TextBox();
            this.LbAccountName = new System.Windows.Forms.Label();
            this.TbUserName = new System.Windows.Forms.TextBox();
            this.TabAdmin = new System.Windows.Forms.TabPage();
            this.BtnSave = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.TabAccount.SuspendLayout();
            this.TabLogin.SuspendLayout();
            this.GbAccount.SuspendLayout();
            this.TabAdmin.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabAccount
            // 
            this.TabAccount.Controls.Add(this.TabLogin);
            this.TabAccount.Controls.Add(this.TabAdmin);
            this.TabAccount.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.TabAccount.Location = new System.Drawing.Point(0, 0);
            this.TabAccount.Name = "TabAccount";
            this.TabAccount.SelectedIndex = 0;
            this.TabAccount.Size = new System.Drawing.Size(456, 425);
            this.TabAccount.TabIndex = 0;
            // 
            // TabLogin
            // 
            this.TabLogin.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.TabLogin.Controls.Add(this.LbLoginMessage);
            this.TabLogin.Controls.Add(this.BtnOK);
            this.TabLogin.Controls.Add(this.BtnCancel);
            this.TabLogin.Controls.Add(this.GbAccount);
            this.TabLogin.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.TabLogin.Location = new System.Drawing.Point(4, 28);
            this.TabLogin.Name = "TabLogin";
            this.TabLogin.Padding = new System.Windows.Forms.Padding(3);
            this.TabLogin.Size = new System.Drawing.Size(448, 393);
            this.TabLogin.TabIndex = 0;
            this.TabLogin.Text = "Login";
            // 
            // LbLoginMessage
            // 
            this.LbLoginMessage.AutoSize = true;
            this.LbLoginMessage.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LbLoginMessage.ForeColor = System.Drawing.Color.Red;
            this.LbLoginMessage.Location = new System.Drawing.Point(51, 220);
            this.LbLoginMessage.Name = "LbLoginMessage";
            this.LbLoginMessage.Size = new System.Drawing.Size(0, 25);
            this.LbLoginMessage.TabIndex = 14;
            // 
            // BtnOK
            // 
            this.BtnOK.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.BtnOK.FlatAppearance.BorderSize = 0;
            this.BtnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnOK.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnOK.Image = ((System.Drawing.Image)(resources.GetObject("BtnOK.Image")));
            this.BtnOK.Location = new System.Drawing.Point(227, 285);
            this.BtnOK.Margin = new System.Windows.Forms.Padding(4);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(96, 96);
            this.BtnOK.TabIndex = 12;
            this.BtnOK.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtnOK.UseVisualStyleBackColor = false;
            // 
            // BtnCancel
            // 
            this.BtnCancel.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.BtnCancel.FlatAppearance.BorderSize = 0;
            this.BtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCancel.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnCancel.Image = ((System.Drawing.Image)(resources.GetObject("BtnCancel.Image")));
            this.BtnCancel.Location = new System.Drawing.Point(341, 284);
            this.BtnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(96, 96);
            this.BtnCancel.TabIndex = 13;
            this.BtnCancel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtnCancel.UseVisualStyleBackColor = false;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // GbAccount
            // 
            this.GbAccount.Controls.Add(this.LbPassword);
            this.GbAccount.Controls.Add(this.TbPassword);
            this.GbAccount.Controls.Add(this.LbAccountName);
            this.GbAccount.Controls.Add(this.TbUserName);
            this.GbAccount.Location = new System.Drawing.Point(51, 45);
            this.GbAccount.Name = "GbAccount";
            this.GbAccount.Size = new System.Drawing.Size(340, 142);
            this.GbAccount.TabIndex = 2;
            this.GbAccount.TabStop = false;
            // 
            // LbPassword
            // 
            this.LbPassword.AutoSize = true;
            this.LbPassword.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.LbPassword.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LbPassword.Location = new System.Drawing.Point(19, 88);
            this.LbPassword.Name = "LbPassword";
            this.LbPassword.Size = new System.Drawing.Size(107, 25);
            this.LbPassword.TabIndex = 2;
            this.LbPassword.Text = "Password:";
            // 
            // TbPassword
            // 
            this.TbPassword.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.TbPassword.Location = new System.Drawing.Point(156, 85);
            this.TbPassword.Name = "TbPassword";
            this.TbPassword.Size = new System.Drawing.Size(168, 34);
            this.TbPassword.TabIndex = 3;
            // 
            // LbAccountName
            // 
            this.LbAccountName.AutoSize = true;
            this.LbAccountName.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.LbAccountName.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LbAccountName.Location = new System.Drawing.Point(19, 34);
            this.LbAccountName.Name = "LbAccountName";
            this.LbAccountName.Size = new System.Drawing.Size(121, 25);
            this.LbAccountName.TabIndex = 0;
            this.LbAccountName.Text = "User Name:";
            // 
            // TbUserName
            // 
            this.TbUserName.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.TbUserName.Location = new System.Drawing.Point(156, 31);
            this.TbUserName.Name = "TbUserName";
            this.TbUserName.Size = new System.Drawing.Size(168, 34);
            this.TbUserName.TabIndex = 1;
            // 
            // TabAdmin
            // 
            this.TabAdmin.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.TabAdmin.Controls.Add(this.BtnSave);
            this.TabAdmin.Controls.Add(this.button4);
            this.TabAdmin.Location = new System.Drawing.Point(4, 28);
            this.TabAdmin.Name = "TabAdmin";
            this.TabAdmin.Padding = new System.Windows.Forms.Padding(3);
            this.TabAdmin.Size = new System.Drawing.Size(448, 393);
            this.TabAdmin.TabIndex = 1;
            this.TabAdmin.Text = "Admin";
            // 
            // BtnSave
            // 
            this.BtnSave.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.BtnSave.FlatAppearance.BorderSize = 0;
            this.BtnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSave.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnSave.Image = ((System.Drawing.Image)(resources.GetObject("BtnSave.Image")));
            this.BtnSave.Location = new System.Drawing.Point(227, 285);
            this.BtnSave.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(96, 96);
            this.BtnSave.TabIndex = 16;
            this.BtnSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtnSave.UseVisualStyleBackColor = false;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button4.Image = ((System.Drawing.Image)(resources.GetObject("button4.Image")));
            this.button4.Location = new System.Drawing.Point(341, 284);
            this.button4.Margin = new System.Windows.Forms.Padding(4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(96, 96);
            this.button4.TabIndex = 17;
            this.button4.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button4.UseVisualStyleBackColor = false;
            // 
            // Account
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(458, 424);
            this.Controls.Add(this.TabAccount);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Account";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Account";
            this.TabAccount.ResumeLayout(false);
            this.TabLogin.ResumeLayout(false);
            this.TabLogin.PerformLayout();
            this.GbAccount.ResumeLayout(false);
            this.GbAccount.PerformLayout();
            this.TabAdmin.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl TabAccount;
        private System.Windows.Forms.TabPage TabLogin;
        public System.Windows.Forms.TextBox TbUserName;
        private System.Windows.Forms.Label LbAccountName;
        private System.Windows.Forms.GroupBox GbAccount;
        private System.Windows.Forms.Label LbPassword;
        private System.Windows.Forms.TextBox TbPassword;
        public System.Windows.Forms.Button BtnOK;
        public System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Label LbLoginMessage;
        private System.Windows.Forms.TabPage TabAdmin;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.Button button4;
    }
}