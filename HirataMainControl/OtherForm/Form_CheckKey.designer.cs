namespace HirataMainControl
{
    partial class Form_CheckKey
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
            this.btnCkeck = new System.Windows.Forms.Button();
            this.labPassKey = new System.Windows.Forms.Label();
            this.labSerial = new System.Windows.Forms.Label();
            this.txtSerial = new System.Windows.Forms.TextBox();
            this.rtfPassKey = new System.Windows.Forms.RichTextBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCkeck
            // 
            this.btnCkeck.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCkeck.Location = new System.Drawing.Point(499, 189);
            this.btnCkeck.Name = "btnCkeck";
            this.btnCkeck.Size = new System.Drawing.Size(90, 50);
            this.btnCkeck.TabIndex = 1;
            this.btnCkeck.Text = "Check";
            this.btnCkeck.UseVisualStyleBackColor = true;
            this.btnCkeck.Click += new System.EventHandler(this.btnCkeck_Click);
            // 
            // labPassKey
            // 
            this.labPassKey.AutoSize = true;
            this.labPassKey.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labPassKey.Location = new System.Drawing.Point(12, 37);
            this.labPassKey.Name = "labPassKey";
            this.labPassKey.Size = new System.Drawing.Size(90, 19);
            this.labPassKey.TabIndex = 2;
            this.labPassKey.Text = "Pass key:";
            this.labPassKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labSerial
            // 
            this.labSerial.AutoSize = true;
            this.labSerial.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labSerial.Location = new System.Drawing.Point(12, 9);
            this.labSerial.Name = "labSerial";
            this.labSerial.Size = new System.Drawing.Size(135, 19);
            this.labSerial.TabIndex = 4;
            this.labSerial.Text = "Serial Number:";
            // 
            // txtSerial
            // 
            this.txtSerial.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSerial.Location = new System.Drawing.Point(153, 4);
            this.txtSerial.Name = "txtSerial";
            this.txtSerial.Size = new System.Drawing.Size(436, 29);
            this.txtSerial.TabIndex = 3;
            // 
            // rtfPassKey
            // 
            this.rtfPassKey.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtfPassKey.Location = new System.Drawing.Point(16, 59);
            this.rtfPassKey.Name = "rtfPassKey";
            this.rtfPassKey.Size = new System.Drawing.Size(468, 236);
            this.rtfPassKey.TabIndex = 5;
            this.rtfPassKey.Text = "";
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Location = new System.Drawing.Point(499, 245);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(90, 50);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // Form_CheckKey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 311);
            this.ControlBox = false;
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.rtfPassKey);
            this.Controls.Add(this.labSerial);
            this.Controls.Add(this.txtSerial);
            this.Controls.Add(this.labPassKey);
            this.Controls.Add(this.btnCkeck);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Form_CheckKey";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Check Hirata Key";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form_CheckKey_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCkeck;
        private System.Windows.Forms.Label labPassKey;
        private System.Windows.Forms.Label labSerial;
        private System.Windows.Forms.TextBox txtSerial;
        private System.Windows.Forms.RichTextBox rtfPassKey;
        private System.Windows.Forms.Button btnExit;
    }
}