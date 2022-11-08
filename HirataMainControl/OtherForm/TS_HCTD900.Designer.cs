namespace HirataMainControl
{
    partial class TS_HCTD900
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
            this.gbxD900 = new System.Windows.Forms.GroupBox();
            this.cboCnt = new System.Windows.Forms.ComboBox();
            this.labCnt = new System.Windows.Forms.Label();
            this.btnSend = new System.Windows.Forms.Button();
            this.labCommad = new System.Windows.Forms.Label();
            this.cboCommand = new System.Windows.Forms.ComboBox();
            this.gbxD900.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxD900
            // 
            this.gbxD900.Controls.Add(this.cboCnt);
            this.gbxD900.Controls.Add(this.labCnt);
            this.gbxD900.Controls.Add(this.btnSend);
            this.gbxD900.Controls.Add(this.labCommad);
            this.gbxD900.Controls.Add(this.cboCommand);
            this.gbxD900.Cursor = System.Windows.Forms.Cursors.Default;
            this.gbxD900.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxD900.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.gbxD900.Location = new System.Drawing.Point(0, 0);
            this.gbxD900.Name = "gbxD900";
            this.gbxD900.Size = new System.Drawing.Size(316, 137);
            this.gbxD900.TabIndex = 32;
            this.gbxD900.TabStop = false;
            this.gbxD900.Text = "D900";
            // 
            // cboCnt
            // 
            this.cboCnt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCnt.Enabled = false;
            this.cboCnt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cboCnt.FormattingEnabled = true;
            this.cboCnt.Location = new System.Drawing.Point(11, 101);
            this.cboCnt.Name = "cboCnt";
            this.cboCnt.Size = new System.Drawing.Size(140, 28);
            this.cboCnt.TabIndex = 33;
            // 
            // labCnt
            // 
            this.labCnt.AutoSize = true;
            this.labCnt.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labCnt.ForeColor = System.Drawing.Color.DarkBlue;
            this.labCnt.Location = new System.Drawing.Point(7, 79);
            this.labCnt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labCnt.Name = "labCnt";
            this.labCnt.Size = new System.Drawing.Size(45, 19);
            this.labCnt.TabIndex = 32;
            this.labCnt.Text = "OCR:";
            // 
            // btnSend
            // 
            this.btnSend.BackColor = System.Drawing.SystemColors.Control;
            this.btnSend.Enabled = false;
            this.btnSend.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.btnSend.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSend.Location = new System.Drawing.Point(167, 100);
            this.btnSend.Margin = new System.Windows.Forms.Padding(0);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(140, 29);
            this.btnSend.TabIndex = 19;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = false;
            // 
            // labCommad
            // 
            this.labCommad.AutoSize = true;
            this.labCommad.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.labCommad.ForeColor = System.Drawing.Color.DarkBlue;
            this.labCommad.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labCommad.Location = new System.Drawing.Point(7, 25);
            this.labCommad.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labCommad.Name = "labCommad";
            this.labCommad.Size = new System.Drawing.Size(81, 19);
            this.labCommad.TabIndex = 23;
            this.labCommad.Text = "Command:";
            // 
            // cboCommand
            // 
            this.cboCommand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCommand.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.cboCommand.FormattingEnabled = true;
            this.cboCommand.Location = new System.Drawing.Point(12, 47);
            this.cboCommand.Name = "cboCommand";
            this.cboCommand.Size = new System.Drawing.Size(295, 28);
            this.cboCommand.TabIndex = 27;
            this.cboCommand.SelectedIndexChanged += new System.EventHandler(this.cboCommand_SelectedIndexChanged);
            // 
            // TS_D900
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 137);
            this.Controls.Add(this.gbxD900);
            this.Name = "TS_D900";
            this.Text = "TS_D900";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TS_D900_FormClosing);
            this.gbxD900.ResumeLayout(false);
            this.gbxD900.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxD900;
        public System.Windows.Forms.ComboBox cboCnt;
        private System.Windows.Forms.Label labCnt;
        public System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label labCommad;
        public System.Windows.Forms.ComboBox cboCommand;
    }
}