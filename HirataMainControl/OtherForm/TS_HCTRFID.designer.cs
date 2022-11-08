namespace HirataMainControl
{
    partial class TS_HCTRFID
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
            this.gbxRFID = new System.Windows.Forms.GroupBox();
            this.clstPage = new System.Windows.Forms.CheckedListBox();
            this.labPage = new System.Windows.Forms.Label();
            this.cboCnt = new System.Windows.Forms.ComboBox();
            this.labCnt = new System.Windows.Forms.Label();
            this.labTime = new System.Windows.Forms.Label();
            this.btnSend = new System.Windows.Forms.Button();
            this.labCommad = new System.Windows.Forms.Label();
            this.cboCommand = new System.Windows.Forms.ComboBox();
            this.gbxRFID.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxRFID
            // 
            this.gbxRFID.Controls.Add(this.clstPage);
            this.gbxRFID.Controls.Add(this.labPage);
            this.gbxRFID.Controls.Add(this.cboCnt);
            this.gbxRFID.Controls.Add(this.labCnt);
            this.gbxRFID.Controls.Add(this.labTime);
            this.gbxRFID.Controls.Add(this.btnSend);
            this.gbxRFID.Controls.Add(this.labCommad);
            this.gbxRFID.Controls.Add(this.cboCommand);
            this.gbxRFID.Cursor = System.Windows.Forms.Cursors.Default;
            this.gbxRFID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxRFID.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Bold);
            this.gbxRFID.Location = new System.Drawing.Point(0, 0);
            this.gbxRFID.Name = "gbxRFID";
            this.gbxRFID.Size = new System.Drawing.Size(319, 186);
            this.gbxRFID.TabIndex = 31;
            this.gbxRFID.TabStop = false;
            this.gbxRFID.Text = "RFID";
            // 
            // clstPage
            // 
            this.clstPage.CheckOnClick = true;
            this.clstPage.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clstPage.FormattingEnabled = true;
            this.clstPage.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17"});
            this.clstPage.Location = new System.Drawing.Point(167, 53);
            this.clstPage.Name = "clstPage";
            this.clstPage.Size = new System.Drawing.Size(140, 123);
            this.clstPage.TabIndex = 38;
            this.clstPage.SelectedIndexChanged += new System.EventHandler(this.clstPage_SelectedIndexChanged);
            // 
            // labPage
            // 
            this.labPage.AutoSize = true;
            this.labPage.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labPage.ForeColor = System.Drawing.Color.DarkBlue;
            this.labPage.Location = new System.Drawing.Point(163, 25);
            this.labPage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labPage.Name = "labPage";
            this.labPage.Size = new System.Drawing.Size(54, 19);
            this.labPage.TabIndex = 36;
            this.labPage.Text = "Page:";
            // 
            // cboCnt
            // 
            this.cboCnt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCnt.Enabled = false;
            this.cboCnt.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cboCnt.FormattingEnabled = true;
            this.cboCnt.Location = new System.Drawing.Point(12, 101);
            this.cboCnt.Name = "cboCnt";
            this.cboCnt.Size = new System.Drawing.Size(140, 29);
            this.cboCnt.TabIndex = 35;
            // 
            // labCnt
            // 
            this.labCnt.AutoSize = true;
            this.labCnt.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labCnt.ForeColor = System.Drawing.Color.DarkBlue;
            this.labCnt.Location = new System.Drawing.Point(8, 79);
            this.labCnt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labCnt.Name = "labCnt";
            this.labCnt.Size = new System.Drawing.Size(54, 19);
            this.labCnt.TabIndex = 34;
            this.labCnt.Text = "RFID:";
            // 
            // labTime
            // 
            this.labTime.AutoSize = true;
            this.labTime.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labTime.ForeColor = System.Drawing.Color.DarkBlue;
            this.labTime.Location = new System.Drawing.Point(8, 133);
            this.labTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labTime.Name = "labTime";
            this.labTime.Size = new System.Drawing.Size(0, 19);
            this.labTime.TabIndex = 32;
            // 
            // btnSend
            // 
            this.btnSend.BackColor = System.Drawing.SystemColors.Control;
            this.btnSend.Enabled = false;
            this.btnSend.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Bold);
            this.btnSend.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSend.Location = new System.Drawing.Point(12, 148);
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
            this.cboCommand.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Bold);
            this.cboCommand.FormattingEnabled = true;
            this.cboCommand.Location = new System.Drawing.Point(12, 47);
            this.cboCommand.Name = "cboCommand";
            this.cboCommand.Size = new System.Drawing.Size(140, 29);
            this.cboCommand.TabIndex = 27;
            this.cboCommand.SelectedIndexChanged += new System.EventHandler(this.cboCommand_SelectedIndexChanged);
            // 
            // TS_HCTRFID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(319, 186);
            this.Controls.Add(this.gbxRFID);
            this.Location = new System.Drawing.Point(800, 300);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "TS_HCTRFID";
            this.Text = "RFID Controller";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TS_HCTE84_FormClosed);
            this.gbxRFID.ResumeLayout(false);
            this.gbxRFID.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxRFID;
        private System.Windows.Forms.Label labTime;
        public System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label labCommad;
        public System.Windows.Forms.ComboBox cboCommand;
        private System.Windows.Forms.Label labPage;
        public System.Windows.Forms.ComboBox cboCnt;
        private System.Windows.Forms.Label labCnt;
        public System.Windows.Forms.CheckedListBox clstPage;
    }
}