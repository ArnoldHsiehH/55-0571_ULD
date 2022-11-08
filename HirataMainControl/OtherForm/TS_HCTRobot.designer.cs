namespace HirataMainControl
{
    partial class TS_HCTRobot
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
            this.gbxRobot = new System.Windows.Forms.GroupBox();
            this.labMSpeed = new System.Windows.Forms.Label();
            this.labSlot = new System.Windows.Forms.Label();
            this.cboCnt = new System.Windows.Forms.ComboBox();
            this.labCnt = new System.Windows.Forms.Label();
            this.labDest = new System.Windows.Forms.Label();
            this.cboSlot = new System.Windows.Forms.ComboBox();
            this.cboArm = new System.Windows.Forms.ComboBox();
            this.cboDest = new System.Windows.Forms.ComboBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.cboSpeed = new System.Windows.Forms.ComboBox();
            this.labCommad = new System.Windows.Forms.Label();
            this.labArm = new System.Windows.Forms.Label();
            this.cboCommand = new System.Windows.Forms.ComboBox();
            this.gbxRobot.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxRobot
            // 
            this.gbxRobot.Controls.Add(this.labMSpeed);
            this.gbxRobot.Controls.Add(this.labSlot);
            this.gbxRobot.Controls.Add(this.cboCnt);
            this.gbxRobot.Controls.Add(this.labCnt);
            this.gbxRobot.Controls.Add(this.labDest);
            this.gbxRobot.Controls.Add(this.cboSlot);
            this.gbxRobot.Controls.Add(this.cboArm);
            this.gbxRobot.Controls.Add(this.cboDest);
            this.gbxRobot.Controls.Add(this.btnSend);
            this.gbxRobot.Controls.Add(this.cboSpeed);
            this.gbxRobot.Controls.Add(this.labCommad);
            this.gbxRobot.Controls.Add(this.labArm);
            this.gbxRobot.Controls.Add(this.cboCommand);
            this.gbxRobot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxRobot.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxRobot.Location = new System.Drawing.Point(0, 0);
            this.gbxRobot.Margin = new System.Windows.Forms.Padding(0);
            this.gbxRobot.Name = "gbxRobot";
            this.gbxRobot.Size = new System.Drawing.Size(319, 251);
            this.gbxRobot.TabIndex = 29;
            this.gbxRobot.TabStop = false;
            this.gbxRobot.Text = "Robot";
            // 
            // labMSpeed
            // 
            this.labMSpeed.AutoSize = true;
            this.labMSpeed.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labMSpeed.ForeColor = System.Drawing.Color.DarkBlue;
            this.labMSpeed.Location = new System.Drawing.Point(7, 187);
            this.labMSpeed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labMSpeed.Name = "labMSpeed";
            this.labMSpeed.Size = new System.Drawing.Size(63, 19);
            this.labMSpeed.TabIndex = 34;
            this.labMSpeed.Text = "Speed:";
            // 
            // labSlot
            // 
            this.labSlot.AutoSize = true;
            this.labSlot.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labSlot.ForeColor = System.Drawing.Color.DarkBlue;
            this.labSlot.Location = new System.Drawing.Point(162, 133);
            this.labSlot.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labSlot.Name = "labSlot";
            this.labSlot.Size = new System.Drawing.Size(54, 19);
            this.labSlot.TabIndex = 32;
            this.labSlot.Text = "Slot:";
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
            this.cboCnt.TabIndex = 31;
            this.cboCnt.SelectedIndexChanged += new System.EventHandler(this.cboCnt_SelectedIndexChanged);
            // 
            // labCnt
            // 
            this.labCnt.AutoSize = true;
            this.labCnt.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labCnt.ForeColor = System.Drawing.Color.DarkBlue;
            this.labCnt.Location = new System.Drawing.Point(7, 79);
            this.labCnt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labCnt.Name = "labCnt";
            this.labCnt.Size = new System.Drawing.Size(63, 19);
            this.labCnt.TabIndex = 30;
            this.labCnt.Text = "Robot:";
            // 
            // labDest
            // 
            this.labDest.AutoSize = true;
            this.labDest.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labDest.ForeColor = System.Drawing.Color.DarkBlue;
            this.labDest.Location = new System.Drawing.Point(7, 133);
            this.labDest.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labDest.Name = "labDest";
            this.labDest.Size = new System.Drawing.Size(117, 19);
            this.labDest.TabIndex = 21;
            this.labDest.Text = "Destination:";
            // 
            // cboSlot
            // 
            this.cboSlot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSlot.Enabled = false;
            this.cboSlot.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cboSlot.FormattingEnabled = true;
            this.cboSlot.Location = new System.Drawing.Point(166, 155);
            this.cboSlot.Name = "cboSlot";
            this.cboSlot.Size = new System.Drawing.Size(140, 28);
            this.cboSlot.TabIndex = 25;
            this.cboSlot.SelectedIndexChanged += new System.EventHandler(this.cboSlot_SelectedIndexChanged);
            // 
            // cboArm
            // 
            this.cboArm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboArm.Enabled = false;
            this.cboArm.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cboArm.FormattingEnabled = true;
            this.cboArm.Location = new System.Drawing.Point(166, 101);
            this.cboArm.Name = "cboArm";
            this.cboArm.Size = new System.Drawing.Size(140, 28);
            this.cboArm.TabIndex = 25;
            // 
            // cboDest
            // 
            this.cboDest.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDest.Enabled = false;
            this.cboDest.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cboDest.FormattingEnabled = true;
            this.cboDest.Location = new System.Drawing.Point(11, 155);
            this.cboDest.Name = "cboDest";
            this.cboDest.Size = new System.Drawing.Size(140, 28);
            this.cboDest.TabIndex = 26;
            this.cboDest.SelectedIndexChanged += new System.EventHandler(this.cboDest_SelectedIndexChanged);
            // 
            // btnSend
            // 
            this.btnSend.BackColor = System.Drawing.SystemColors.Control;
            this.btnSend.Enabled = false;
            this.btnSend.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSend.Location = new System.Drawing.Point(166, 209);
            this.btnSend.Margin = new System.Windows.Forms.Padding(4);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(140, 29);
            this.btnSend.TabIndex = 19;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = false;
            
            // 
            // cboSpeed
            // 
            this.cboSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSpeed.Enabled = false;
            this.cboSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cboSpeed.FormattingEnabled = true;
            this.cboSpeed.Location = new System.Drawing.Point(11, 209);
            this.cboSpeed.Name = "cboSpeed";
            this.cboSpeed.Size = new System.Drawing.Size(140, 28);
            this.cboSpeed.TabIndex = 26;
            this.cboSpeed.SelectedIndexChanged += new System.EventHandler(this.cboMSpeed_SelectedIndexChanged);
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
            // 
            // labArm
            // 
            this.labArm.AutoSize = true;
            this.labArm.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labArm.ForeColor = System.Drawing.Color.DarkBlue;
            this.labArm.Location = new System.Drawing.Point(162, 79);
            this.labArm.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labArm.Name = "labArm";
            this.labArm.Size = new System.Drawing.Size(45, 19);
            this.labArm.TabIndex = 20;
            this.labArm.Text = "Arm:";
            // 
            // cboCommand
            // 
            this.cboCommand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCommand.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboCommand.FormattingEnabled = true;
            this.cboCommand.Location = new System.Drawing.Point(11, 47);
            this.cboCommand.Name = "cboCommand";
            this.cboCommand.Size = new System.Drawing.Size(295, 28);
            this.cboCommand.TabIndex = 27;
            this.cboCommand.SelectedIndexChanged += new System.EventHandler(this.cboCommand_SelectedIndexChanged);
            // 
            // TS_HCTRobot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(319, 251);
            this.Controls.Add(this.gbxRobot);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Location = new System.Drawing.Point(800, 300);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TS_HCTRobot";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Robot Controller";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TS_EFEMRobot_FormClosing);
            this.gbxRobot.ResumeLayout(false);
            this.gbxRobot.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxRobot;
        private System.Windows.Forms.Label labDest;
        public System.Windows.Forms.ComboBox cboSlot;
        public System.Windows.Forms.ComboBox cboArm;
        public System.Windows.Forms.ComboBox cboDest;
        public System.Windows.Forms.Button btnSend;
        public System.Windows.Forms.ComboBox cboSpeed;
        private System.Windows.Forms.Label labCommad;
        private System.Windows.Forms.Label labArm;
        public System.Windows.Forms.ComboBox cboCommand;
        public System.Windows.Forms.ComboBox cboCnt;
        private System.Windows.Forms.Label labCnt;
        private System.Windows.Forms.Label labMSpeed;
        private System.Windows.Forms.Label labSlot;
    }
}