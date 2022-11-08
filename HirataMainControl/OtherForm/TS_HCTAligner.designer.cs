namespace HirataMainControl
{
    partial class TS_HCTAligner
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
            this.gbxAligner = new System.Windows.Forms.GroupBox();
            this.txtDegree = new System.Windows.Forms.TextBox();
            this.cboOnOff = new System.Windows.Forms.ComboBox();
            this.labOnOff = new System.Windows.Forms.Label();
            this.cboCnt = new System.Windows.Forms.ComboBox();
            this.labCnt = new System.Windows.Forms.Label();
            this.btnSend = new System.Windows.Forms.Button();
            this.labCommad = new System.Windows.Forms.Label();
            this.labDegree = new System.Windows.Forms.Label();
            this.cboCommand = new System.Windows.Forms.ComboBox();
            this.cboType = new System.Windows.Forms.ComboBox();
            this.labType = new System.Windows.Forms.Label();
            this.gbxAligner.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxAligner
            // 
            this.gbxAligner.Controls.Add(this.txtDegree);
            this.gbxAligner.Controls.Add(this.cboOnOff);
            this.gbxAligner.Controls.Add(this.labOnOff);
            this.gbxAligner.Controls.Add(this.cboCnt);
            this.gbxAligner.Controls.Add(this.labCnt);
            this.gbxAligner.Controls.Add(this.labType);
            this.gbxAligner.Controls.Add(this.cboType);
            this.gbxAligner.Controls.Add(this.btnSend);
            this.gbxAligner.Controls.Add(this.labCommad);
            this.gbxAligner.Controls.Add(this.labDegree);
            this.gbxAligner.Controls.Add(this.cboCommand);
            this.gbxAligner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxAligner.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxAligner.Location = new System.Drawing.Point(0, 0);
            this.gbxAligner.Name = "gbxAligner";
            this.gbxAligner.Size = new System.Drawing.Size(319, 201);
            this.gbxAligner.TabIndex = 29;
            this.gbxAligner.TabStop = false;
            this.gbxAligner.Text = "Aligner";
            // 
            // txtDegree
            // 
            this.txtDegree.Location = new System.Drawing.Point(132, 101);
            this.txtDegree.MaxLength = 5;
            this.txtDegree.Name = "txtDegree";
            this.txtDegree.Size = new System.Drawing.Size(79, 29);
            this.txtDegree.TabIndex = 34;
            this.txtDegree.TextChanged += new System.EventHandler(this.txtDegree_TextChanged);
            this.txtDegree.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDegree_KeyDown);
            // 
            // cboOnOff
            // 
            this.cboOnOff.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOnOff.Enabled = false;
            this.cboOnOff.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboOnOff.FormattingEnabled = true;
            this.cboOnOff.Location = new System.Drawing.Point(217, 101);
            this.cboOnOff.Name = "cboOnOff";
            this.cboOnOff.Size = new System.Drawing.Size(89, 29);
            this.cboOnOff.TabIndex = 33;
            this.cboOnOff.SelectedIndexChanged += new System.EventHandler(this.cboOnOff_SelectedIndexChanged);
            // 
            // labOnOff
            // 
            this.labOnOff.AutoSize = true;
            this.labOnOff.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labOnOff.ForeColor = System.Drawing.Color.DarkBlue;
            this.labOnOff.Location = new System.Drawing.Point(213, 79);
            this.labOnOff.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labOnOff.Name = "labOnOff";
            this.labOnOff.Size = new System.Drawing.Size(72, 19);
            this.labOnOff.TabIndex = 32;
            this.labOnOff.Text = "Vacuum:";
            // 
            // cboCnt
            // 
            this.cboCnt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCnt.Enabled = false;
            this.cboCnt.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboCnt.FormattingEnabled = true;
            this.cboCnt.Location = new System.Drawing.Point(11, 101);
            this.cboCnt.Name = "cboCnt";
            this.cboCnt.Size = new System.Drawing.Size(115, 29);
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
            this.labCnt.Size = new System.Drawing.Size(81, 19);
            this.labCnt.TabIndex = 30;
            this.labCnt.Text = "Aligner:";
            // 
            // btnSend
            // 
            this.btnSend.BackColor = System.Drawing.SystemColors.Control;
            this.btnSend.Enabled = false;
            this.btnSend.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSend.Location = new System.Drawing.Point(133, 155);
            this.btnSend.Margin = new System.Windows.Forms.Padding(4);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(173, 29);
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
            // 
            // labDegree
            // 
            this.labDegree.AutoSize = true;
            this.labDegree.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labDegree.ForeColor = System.Drawing.Color.DarkBlue;
            this.labDegree.Location = new System.Drawing.Point(128, 79);
            this.labDegree.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labDegree.Name = "labDegree";
            this.labDegree.Size = new System.Drawing.Size(72, 19);
            this.labDegree.TabIndex = 20;
            this.labDegree.Text = "Degree:";
            // 
            // cboCommand
            // 
            this.cboCommand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCommand.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboCommand.FormattingEnabled = true;
            this.cboCommand.Location = new System.Drawing.Point(11, 47);
            this.cboCommand.Name = "cboCommand";
            this.cboCommand.Size = new System.Drawing.Size(295, 29);
            this.cboCommand.TabIndex = 27;
            this.cboCommand.SelectedIndexChanged += new System.EventHandler(this.cboCommand_SelectedIndexChanged);
            // 
            // cboType
            // 
            this.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboType.Enabled = false;
            this.cboType.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboType.FormattingEnabled = true;
            this.cboType.Location = new System.Drawing.Point(11, 155);
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(115, 29);
            this.cboType.TabIndex = 26;
            this.cboType.Visible = false;
            this.cboType.SelectedIndexChanged += new System.EventHandler(this.cboType_SelectedIndexChanged);
            // 
            // labType
            // 
            this.labType.AutoSize = true;
            this.labType.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labType.ForeColor = System.Drawing.Color.DarkBlue;
            this.labType.Location = new System.Drawing.Point(7, 133);
            this.labType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labType.Name = "labType";
            this.labType.Size = new System.Drawing.Size(54, 19);
            this.labType.TabIndex = 21;
            this.labType.Text = "Type:";
            this.labType.Visible = false;
            // 
            // TS_HCTAligner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(319, 201);
            this.Controls.Add(this.gbxAligner);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Location = new System.Drawing.Point(800, 300);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TS_HCTAligner";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Aligner Controller";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TS_TexegAligner_FormClosing);
            this.gbxAligner.ResumeLayout(false);
            this.gbxAligner.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxAligner;
        public System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label labCommad;
        private System.Windows.Forms.Label labDegree;
        public System.Windows.Forms.ComboBox cboCommand;
        public System.Windows.Forms.ComboBox cboCnt;
        private System.Windows.Forms.Label labCnt;
        public System.Windows.Forms.TextBox txtDegree;
        public System.Windows.Forms.ComboBox cboOnOff;
        private System.Windows.Forms.Label labOnOff;
        private System.Windows.Forms.Label labType;
        public System.Windows.Forms.ComboBox cboType;
    }
}