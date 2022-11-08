namespace HirataMainControl
{
    partial class TS_HCTMagazinerPort
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
            this.gbxLoadPort = new System.Windows.Forms.GroupBox();
            this.cboCnt = new System.Windows.Forms.ComboBox();
            this.labCnt = new System.Windows.Forms.Label();
            this.btnSend = new System.Windows.Forms.Button();
            this.labCommad = new System.Windows.Forms.Label();
            this.cboCommand = new System.Windows.Forms.ComboBox();
            this.gbxLoadPort.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxLoadPort
            // 
            this.gbxLoadPort.Controls.Add(this.cboCnt);
            this.gbxLoadPort.Controls.Add(this.labCnt);
            this.gbxLoadPort.Controls.Add(this.btnSend);
            this.gbxLoadPort.Controls.Add(this.labCommad);
            this.gbxLoadPort.Controls.Add(this.cboCommand);
            this.gbxLoadPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxLoadPort.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxLoadPort.Location = new System.Drawing.Point(0, 0);
            this.gbxLoadPort.Name = "gbxLoadPort";
            this.gbxLoadPort.Size = new System.Drawing.Size(319, 151);
            this.gbxLoadPort.TabIndex = 29;
            this.gbxLoadPort.TabStop = false;
            this.gbxLoadPort.Text = "MagazinePort";
            // 
            // cboCnt
            // 
            this.cboCnt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCnt.Enabled = false;
            this.cboCnt.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cboCnt.FormattingEnabled = true;
            this.cboCnt.Location = new System.Drawing.Point(11, 101);
            this.cboCnt.Name = "cboCnt";
            this.cboCnt.Size = new System.Drawing.Size(140, 29);
            this.cboCnt.TabIndex = 31;
            // 
            // labCnt
            // 
            this.labCnt.AutoSize = true;
            this.labCnt.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labCnt.ForeColor = System.Drawing.Color.DarkBlue;
            this.labCnt.Location = new System.Drawing.Point(7, 79);
            this.labCnt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labCnt.Name = "labCnt";
            this.labCnt.Size = new System.Drawing.Size(126, 19);
            this.labCnt.TabIndex = 30;
            this.labCnt.Text = "MagazinePort:";
            this.labCnt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSend
            // 
            this.btnSend.BackColor = System.Drawing.SystemColors.Control;
            this.btnSend.Enabled = false;
            this.btnSend.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSend.Location = new System.Drawing.Point(166, 101);
            this.btnSend.Margin = new System.Windows.Forms.Padding(4);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(140, 29);
            this.btnSend.TabIndex = 19;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = false;
            // 
            // labCommad
            // 
            this.labCommad.AutoSize = true;
            this.labCommad.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labCommad.ForeColor = System.Drawing.Color.DarkBlue;
            this.labCommad.Location = new System.Drawing.Point(7, 25);
            this.labCommad.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labCommad.Name = "labCommad";
            this.labCommad.Size = new System.Drawing.Size(81, 19);
            this.labCommad.TabIndex = 23;
            this.labCommad.Text = "Command:";
            this.labCommad.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboCommand
            // 
            this.cboCommand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCommand.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cboCommand.FormattingEnabled = true;
            this.cboCommand.Location = new System.Drawing.Point(11, 47);
            this.cboCommand.Name = "cboCommand";
            this.cboCommand.Size = new System.Drawing.Size(295, 29);
            this.cboCommand.TabIndex = 27;
            this.cboCommand.SelectedIndexChanged += new System.EventHandler(this.cboCommand_SelectedIndexChanged);
            // 
            // TS_HCTMagazinerPort
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(319, 151);
            this.Controls.Add(this.gbxLoadPort);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Location = new System.Drawing.Point(800, 300);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TS_HCTMagazinerPort";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Magazine Controller";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TS_HCTMagazinerPort_FormClosing);
            this.gbxLoadPort.ResumeLayout(false);
            this.gbxLoadPort.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxLoadPort;
        public System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label labCommad;
        public System.Windows.Forms.ComboBox cboCommand;
        public System.Windows.Forms.ComboBox cboCnt;
        private System.Windows.Forms.Label labCnt;
    }
}