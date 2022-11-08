namespace HirataMainControl
{
    partial class IO_Adam6000
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
            this.components = new System.ComponentModel.Container();
            this.gbxTable = new System.Windows.Forms.GroupBox();
            this.labStatus = new System.Windows.Forms.Label();
            this.cboDo = new System.Windows.Forms.ComboBox();
            this.labDoChannel = new System.Windows.Forms.Label();
            this.btnDoSend = new System.Windows.Forms.Button();
            this.gbxDo = new System.Windows.Forms.GroupBox();
            this.gbxAo = new System.Windows.Forms.GroupBox();
            this.labAoValue = new System.Windows.Forms.Label();
            this.trkbAo = new System.Windows.Forms.TrackBar();
            this.labAoChannel = new System.Windows.Forms.Label();
            this.btnAoSend = new System.Windows.Forms.Button();
            this.cboAo = new System.Windows.Forms.ComboBox();
            this.tmr1s = new System.Windows.Forms.Timer(this.components);
            this.cboDoValue = new System.Windows.Forms.ComboBox();
            this.gbxDo.SuspendLayout();
            this.gbxAo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkbAo)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxTable
            // 
            this.gbxTable.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.gbxTable.Location = new System.Drawing.Point(3, 189);
            this.gbxTable.Name = "gbxTable";
            this.gbxTable.Size = new System.Drawing.Size(408, 548);
            this.gbxTable.TabIndex = 0;
            this.gbxTable.TabStop = false;
            this.gbxTable.Text = "Adam";
            // 
            // labStatus
            // 
            this.labStatus.BackColor = System.Drawing.Color.Red;
            this.labStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labStatus.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.labStatus.Location = new System.Drawing.Point(318, 24);
            this.labStatus.Name = "labStatus";
            this.labStatus.Size = new System.Drawing.Size(93, 35);
            this.labStatus.TabIndex = 50;
            this.labStatus.Text = "DisConnect";
            this.labStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboDo
            // 
            this.cboDo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDo.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboDo.FormattingEnabled = true;
            this.cboDo.Location = new System.Drawing.Point(79, 19);
            this.cboDo.Name = "cboDo";
            this.cboDo.Size = new System.Drawing.Size(81, 27);
            this.cboDo.TabIndex = 53;
            // 
            // labDoChannel
            // 
            this.labDoChannel.AutoSize = true;
            this.labDoChannel.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labDoChannel.ForeColor = System.Drawing.Color.DarkBlue;
            this.labDoChannel.Location = new System.Drawing.Point(8, 26);
            this.labDoChannel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labDoChannel.Name = "labDoChannel";
            this.labDoChannel.Size = new System.Drawing.Size(63, 14);
            this.labDoChannel.TabIndex = 52;
            this.labDoChannel.Text = "Channel:";
            // 
            // btnDoSend
            // 
            this.btnDoSend.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDoSend.Location = new System.Drawing.Point(254, 19);
            this.btnDoSend.Margin = new System.Windows.Forms.Padding(5);
            this.btnDoSend.Name = "btnDoSend";
            this.btnDoSend.Size = new System.Drawing.Size(50, 27);
            this.btnDoSend.TabIndex = 51;
            this.btnDoSend.Text = "Do ";
            this.btnDoSend.UseVisualStyleBackColor = true;
            this.btnDoSend.Click += new System.EventHandler(this.btnDoSend_Click);
            // 
            // gbxDo
            // 
            this.gbxDo.Controls.Add(this.cboDoValue);
            this.gbxDo.Controls.Add(this.labDoChannel);
            this.gbxDo.Controls.Add(this.btnDoSend);
            this.gbxDo.Controls.Add(this.cboDo);
            this.gbxDo.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxDo.Location = new System.Drawing.Point(3, 10);
            this.gbxDo.Name = "gbxDo";
            this.gbxDo.Size = new System.Drawing.Size(309, 57);
            this.gbxDo.TabIndex = 1;
            this.gbxDo.TabStop = false;
            this.gbxDo.Text = "DO";
            // 
            // gbxAo
            // 
            this.gbxAo.Controls.Add(this.labAoValue);
            this.gbxAo.Controls.Add(this.trkbAo);
            this.gbxAo.Controls.Add(this.labAoChannel);
            this.gbxAo.Controls.Add(this.btnAoSend);
            this.gbxAo.Controls.Add(this.cboAo);
            this.gbxAo.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxAo.Location = new System.Drawing.Point(3, 75);
            this.gbxAo.Name = "gbxAo";
            this.gbxAo.Size = new System.Drawing.Size(408, 115);
            this.gbxAo.TabIndex = 51;
            this.gbxAo.TabStop = false;
            this.gbxAo.Text = "AO";
            // 
            // labAoValue
            // 
            this.labAoValue.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labAoValue.ForeColor = System.Drawing.Color.DarkBlue;
            this.labAoValue.Location = new System.Drawing.Point(315, 15);
            this.labAoValue.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labAoValue.Name = "labAoValue";
            this.labAoValue.Size = new System.Drawing.Size(85, 37);
            this.labAoValue.TabIndex = 54;
            this.labAoValue.Text = "0";
            this.labAoValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trkbAo
            // 
            this.trkbAo.LargeChange = 16;
            this.trkbAo.Location = new System.Drawing.Point(7, 56);
            this.trkbAo.Maximum = 4095;
            this.trkbAo.Name = "trkbAo";
            this.trkbAo.Size = new System.Drawing.Size(393, 45);
            this.trkbAo.TabIndex = 1;
            this.trkbAo.Scroll += new System.EventHandler(this.trkbAo_Scroll);
            // 
            // labAoChannel
            // 
            this.labAoChannel.AutoSize = true;
            this.labAoChannel.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labAoChannel.ForeColor = System.Drawing.Color.DarkBlue;
            this.labAoChannel.Location = new System.Drawing.Point(8, 31);
            this.labAoChannel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labAoChannel.Name = "labAoChannel";
            this.labAoChannel.Size = new System.Drawing.Size(63, 14);
            this.labAoChannel.TabIndex = 52;
            this.labAoChannel.Text = "Channel:";
            // 
            // btnAoSend
            // 
            this.btnAoSend.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAoSend.Location = new System.Drawing.Point(207, 23);
            this.btnAoSend.Margin = new System.Windows.Forms.Padding(5);
            this.btnAoSend.Name = "btnAoSend";
            this.btnAoSend.Size = new System.Drawing.Size(82, 27);
            this.btnAoSend.TabIndex = 51;
            this.btnAoSend.Text = "Ao";
            this.btnAoSend.UseVisualStyleBackColor = true;
            this.btnAoSend.Click += new System.EventHandler(this.btnAoSend_Click);
            // 
            // cboAo
            // 
            this.cboAo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAo.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboAo.FormattingEnabled = true;
            this.cboAo.Location = new System.Drawing.Point(79, 23);
            this.cboAo.Name = "cboAo";
            this.cboAo.Size = new System.Drawing.Size(81, 27);
            this.cboAo.TabIndex = 53;
            // 
            // tmr1s
            // 
            this.tmr1s.Interval = 1000;
            this.tmr1s.Tick += new System.EventHandler(this.AdamTime1000ms_Tick);
            // 
            // cboDoValue
            // 
            this.cboDoValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDoValue.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboDoValue.FormattingEnabled = true;
            this.cboDoValue.Location = new System.Drawing.Point(166, 19);
            this.cboDoValue.Name = "cboDoValue";
            this.cboDoValue.Size = new System.Drawing.Size(80, 27);
            this.cboDoValue.TabIndex = 54;
            // 
            // IO_Adam6000
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.gbxAo);
            this.Controls.Add(this.labStatus);
            this.Controls.Add(this.gbxDo);
            this.Controls.Add(this.gbxTable);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "IO_Adam6000";
            this.Size = new System.Drawing.Size(420, 937);
            this.gbxDo.ResumeLayout(false);
            this.gbxDo.PerformLayout();
            this.gbxAo.ResumeLayout(false);
            this.gbxAo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkbAo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxTable;
        private System.Windows.Forms.Label labStatus;
        public System.Windows.Forms.ComboBox cboDo;
        private System.Windows.Forms.Label labDoChannel;
        public System.Windows.Forms.Button btnDoSend;
        private System.Windows.Forms.GroupBox gbxDo;
        private System.Windows.Forms.GroupBox gbxAo;
        private System.Windows.Forms.Label labAoChannel;
        public System.Windows.Forms.Button btnAoSend;
        public System.Windows.Forms.ComboBox cboAo;
        private System.Windows.Forms.TrackBar trkbAo;
        private System.Windows.Forms.Label labAoValue;
        private System.Windows.Forms.Timer tmr1s;
        public System.Windows.Forms.ComboBox cboDoValue;




    }
}