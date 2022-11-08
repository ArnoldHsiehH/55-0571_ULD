namespace HirataMainControl
{
    partial class Form_Initial
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
            this.txtSuccess = new System.Windows.Forms.TextBox();
            this.prgbInitial = new System.Windows.Forms.ProgressBar();
            this.btnComplete = new System.Windows.Forms.Button();
            this.txtFail = new System.Windows.Forms.TextBox();
            this.labSuccess = new System.Windows.Forms.Label();
            this.labFail = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtSuccess
            // 
            this.txtSuccess.BackColor = System.Drawing.SystemColors.Window;
            this.txtSuccess.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtSuccess.ForeColor = System.Drawing.Color.Blue;
            this.txtSuccess.Location = new System.Drawing.Point(12, 31);
            this.txtSuccess.Multiline = true;
            this.txtSuccess.Name = "txtSuccess";
            this.txtSuccess.ReadOnly = true;
            this.txtSuccess.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSuccess.Size = new System.Drawing.Size(600, 100);
            this.txtSuccess.TabIndex = 0;
            // 
            // prgbInitial
            // 
            this.prgbInitial.ForeColor = System.Drawing.Color.LightGreen;
            this.prgbInitial.Location = new System.Drawing.Point(12, 272);
            this.prgbInitial.Name = "prgbInitial";
            this.prgbInitial.Size = new System.Drawing.Size(400, 24);
            this.prgbInitial.TabIndex = 1;
            // 
            // btnComplete
            // 
            this.btnComplete.BackColor = System.Drawing.SystemColors.Control;
            this.btnComplete.Enabled = false;
            this.btnComplete.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnComplete.Location = new System.Drawing.Point(433, 272);
            this.btnComplete.Name = "btnComplete";
            this.btnComplete.Size = new System.Drawing.Size(83, 57);
            this.btnComplete.TabIndex = 2;
            this.btnComplete.Text = "Enter";
            this.btnComplete.UseVisualStyleBackColor = false;
            this.btnComplete.Click += new System.EventHandler(this.InitialComplete_Click);
            // 
            // txtFail
            // 
            this.txtFail.BackColor = System.Drawing.SystemColors.Window;
            this.txtFail.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtFail.ForeColor = System.Drawing.Color.Red;
            this.txtFail.Location = new System.Drawing.Point(12, 156);
            this.txtFail.Multiline = true;
            this.txtFail.Name = "txtFail";
            this.txtFail.ReadOnly = true;
            this.txtFail.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtFail.Size = new System.Drawing.Size(600, 100);
            this.txtFail.TabIndex = 3;
            // 
            // labSuccess
            // 
            this.labSuccess.AutoSize = true;
            this.labSuccess.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labSuccess.Location = new System.Drawing.Point(8, 9);
            this.labSuccess.Name = "labSuccess";
            this.labSuccess.Size = new System.Drawing.Size(81, 19);
            this.labSuccess.TabIndex = 4;
            this.labSuccess.Text = "Success:";
            // 
            // labFail
            // 
            this.labFail.AutoSize = true;
            this.labFail.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labFail.Location = new System.Drawing.Point(8, 134);
            this.labFail.Name = "labFail";
            this.labFail.Size = new System.Drawing.Size(54, 19);
            this.labFail.TabIndex = 5;
            this.labFail.Text = "Fail:";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.SystemColors.Control;
            this.btnClose.Enabled = false;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(529, 272);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(83, 57);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // Form_Initial
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(624, 341);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.labFail);
            this.Controls.Add(this.labSuccess);
            this.Controls.Add(this.txtFail);
            this.Controls.Add(this.btnComplete);
            this.Controls.Add(this.prgbInitial);
            this.Controls.Add(this.txtSuccess);
            this.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form_Initial";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Device link check";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSuccess;
        private System.Windows.Forms.ProgressBar prgbInitial;
        private System.Windows.Forms.Button btnComplete;
        private System.Windows.Forms.TextBox txtFail;
        private System.Windows.Forms.Label labSuccess;
        private System.Windows.Forms.Label labFail;
        public System.Windows.Forms.Button btnClose;
    }
}