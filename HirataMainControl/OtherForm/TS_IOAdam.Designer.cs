namespace HirataMainControl.OtherForm
{
    partial class TS_IOAdam
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
            this.gbxAo = new System.Windows.Forms.GroupBox();
            this.labAoValue = new System.Windows.Forms.Label();
            this.trkbAo = new System.Windows.Forms.TrackBar();
            this.labAoChannel = new System.Windows.Forms.Label();
            this.btnAoSend = new System.Windows.Forms.Button();
            this.cboAo = new System.Windows.Forms.ComboBox();
            this.labStatus = new System.Windows.Forms.Label();
            this.gbxDo = new System.Windows.Forms.GroupBox();
            this.labDoChannel = new System.Windows.Forms.Label();
            this.btnDoSend = new System.Windows.Forms.Button();
            this.cboDo = new System.Windows.Forms.ComboBox();
            this.cboAdamSelect = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.gbxAdam = new System.Windows.Forms.GroupBox();
            this.gbxAo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkbAo)).BeginInit();
            this.gbxDo.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.gbxAdam.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxAo
            // 
            this.gbxAo.Controls.Add(this.labAoValue);
            this.gbxAo.Controls.Add(this.trkbAo);
            this.gbxAo.Controls.Add(this.labAoChannel);
            this.gbxAo.Controls.Add(this.btnAoSend);
            this.gbxAo.Controls.Add(this.cboAo);
            this.gbxAo.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxAo.Location = new System.Drawing.Point(6, 106);
            this.gbxAo.Name = "gbxAo";
            this.gbxAo.Size = new System.Drawing.Size(350, 99);
            this.gbxAo.TabIndex = 54;
            this.gbxAo.TabStop = false;
            this.gbxAo.Text = "AO";
            // 
            // labAoValue
            // 
            this.labAoValue.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labAoValue.ForeColor = System.Drawing.Color.DarkBlue;
            this.labAoValue.Location = new System.Drawing.Point(287, 48);
            this.labAoValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labAoValue.Name = "labAoValue";
            this.labAoValue.Size = new System.Drawing.Size(56, 32);
            this.labAoValue.TabIndex = 54;
            this.labAoValue.Text = "0";
            this.labAoValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trkbAo
            // 
            this.trkbAo.LargeChange = 16;
            this.trkbAo.Location = new System.Drawing.Point(6, 48);
            this.trkbAo.Maximum = 4095;
            this.trkbAo.Name = "trkbAo";
            this.trkbAo.Size = new System.Drawing.Size(299, 45);
            this.trkbAo.TabIndex = 1;
            // 
            // labAoChannel
            // 
            this.labAoChannel.AutoSize = true;
            this.labAoChannel.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labAoChannel.ForeColor = System.Drawing.Color.DarkBlue;
            this.labAoChannel.Location = new System.Drawing.Point(7, 27);
            this.labAoChannel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labAoChannel.Name = "labAoChannel";
            this.labAoChannel.Size = new System.Drawing.Size(63, 14);
            this.labAoChannel.TabIndex = 52;
            this.labAoChannel.Text = "Channel:";
            // 
            // btnAoSend
            // 
            this.btnAoSend.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAoSend.Location = new System.Drawing.Point(164, 20);
            this.btnAoSend.Margin = new System.Windows.Forms.Padding(4);
            this.btnAoSend.Name = "btnAoSend";
            this.btnAoSend.Size = new System.Drawing.Size(70, 27);
            this.btnAoSend.TabIndex = 51;
            this.btnAoSend.Text = "Ao";
            this.btnAoSend.UseVisualStyleBackColor = true;
            // 
            // cboAo
            // 
            this.cboAo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAo.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboAo.FormattingEnabled = true;
            this.cboAo.Location = new System.Drawing.Point(77, 20);
            this.cboAo.Name = "cboAo";
            this.cboAo.Size = new System.Drawing.Size(70, 27);
            this.cboAo.TabIndex = 53;
            // 
            // labStatus
            // 
            this.labStatus.BackColor = System.Drawing.Color.Red;
            this.labStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labStatus.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.labStatus.Location = new System.Drawing.Point(5, 18);
            this.labStatus.Name = "labStatus";
            this.labStatus.Size = new System.Drawing.Size(80, 25);
            this.labStatus.TabIndex = 53;
            this.labStatus.Text = "DisConnect";
            this.labStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gbxDo
            // 
            this.gbxDo.Controls.Add(this.labDoChannel);
            this.gbxDo.Controls.Add(this.btnDoSend);
            this.gbxDo.Controls.Add(this.cboDo);
            this.gbxDo.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxDo.Location = new System.Drawing.Point(6, 51);
            this.gbxDo.Name = "gbxDo";
            this.gbxDo.Size = new System.Drawing.Size(255, 49);
            this.gbxDo.TabIndex = 52;
            this.gbxDo.TabStop = false;
            this.gbxDo.Text = "DO";
            // 
            // labDoChannel
            // 
            this.labDoChannel.AutoSize = true;
            this.labDoChannel.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labDoChannel.ForeColor = System.Drawing.Color.DarkBlue;
            this.labDoChannel.Location = new System.Drawing.Point(7, 22);
            this.labDoChannel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labDoChannel.Name = "labDoChannel";
            this.labDoChannel.Size = new System.Drawing.Size(63, 14);
            this.labDoChannel.TabIndex = 52;
            this.labDoChannel.Text = "Channel:";
            // 
            // btnDoSend
            // 
            this.btnDoSend.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDoSend.Location = new System.Drawing.Point(164, 15);
            this.btnDoSend.Margin = new System.Windows.Forms.Padding(4);
            this.btnDoSend.Name = "btnDoSend";
            this.btnDoSend.Size = new System.Drawing.Size(70, 27);
            this.btnDoSend.TabIndex = 51;
            this.btnDoSend.Text = "Do ";
            this.btnDoSend.UseVisualStyleBackColor = true;
            // 
            // cboDo
            // 
            this.cboDo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDo.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboDo.FormattingEnabled = true;
            this.cboDo.Location = new System.Drawing.Point(77, 15);
            this.cboDo.Name = "cboDo";
            this.cboDo.Size = new System.Drawing.Size(70, 27);
            this.cboDo.TabIndex = 53;
            // 
            // cboAdamSelect
            // 
            this.cboAdamSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAdamSelect.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboAdamSelect.FormattingEnabled = true;
            this.cboAdamSelect.Location = new System.Drawing.Point(101, 18);
            this.cboAdamSelect.Name = "cboAdamSelect";
            this.cboAdamSelect.Size = new System.Drawing.Size(70, 27);
            this.cboAdamSelect.TabIndex = 55;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(6, 211);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(350, 318);
            this.tabControl1.TabIndex = 56;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(342, 292);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(192, 74);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // gbxAdam
            // 
            this.gbxAdam.Controls.Add(this.labStatus);
            this.gbxAdam.Controls.Add(this.tabControl1);
            this.gbxAdam.Controls.Add(this.gbxDo);
            this.gbxAdam.Controls.Add(this.cboAdamSelect);
            this.gbxAdam.Controls.Add(this.gbxAo);
            this.gbxAdam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxAdam.Location = new System.Drawing.Point(0, 0);
            this.gbxAdam.Name = "gbxAdam";
            this.gbxAdam.Size = new System.Drawing.Size(368, 520);
            this.gbxAdam.TabIndex = 57;
            this.gbxAdam.TabStop = false;
            this.gbxAdam.Text = "groupBox1";
            // 
            // TS_IOAdam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(368, 520);
            this.Controls.Add(this.gbxAdam);
            this.Name = "TS_IOAdam";
            this.Text = "Adam";
            this.gbxAo.ResumeLayout(false);
            this.gbxAo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkbAo)).EndInit();
            this.gbxDo.ResumeLayout(false);
            this.gbxDo.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.gbxAdam.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxAo;
        private System.Windows.Forms.Label labAoValue;
        private System.Windows.Forms.TrackBar trkbAo;
        private System.Windows.Forms.Label labAoChannel;
        public System.Windows.Forms.Button btnAoSend;
        public System.Windows.Forms.ComboBox cboAo;
        private System.Windows.Forms.Label labStatus;
        private System.Windows.Forms.GroupBox gbxDo;
        private System.Windows.Forms.Label labDoChannel;
        public System.Windows.Forms.Button btnDoSend;
        public System.Windows.Forms.ComboBox cboDo;
        public System.Windows.Forms.ComboBox cboAdamSelect;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox gbxAdam;
    }
}