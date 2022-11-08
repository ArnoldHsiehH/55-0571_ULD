namespace HirataMainControl
{
    partial class UserCore
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.labEFEMBusy = new System.Windows.Forms.Label();
            this.labRobot1Busy = new System.Windows.Forms.Label();
            this.labEFEMWork = new System.Windows.Forms.Label();
            this.btnRefreshJob = new System.Windows.Forms.Button();
            this.btnDeleteWaferJob = new System.Windows.Forms.Button();
            this.cboWaferStatus = new System.Windows.Forms.ComboBox();
            this.btnWaferChange = new System.Windows.Forms.Button();
            this.labRobot1Work = new System.Windows.Forms.Label();
            this.gbxCoreContrl = new System.Windows.Forms.GroupBox();
            this.btnCarrierChange = new System.Windows.Forms.Button();
            this.cboCarrierStatus = new System.Windows.Forms.ComboBox();
            this.labRobot2Work = new System.Windows.Forms.Label();
            this.labRobot2Busy = new System.Windows.Forms.Label();
            this.dgvQueuePJ = new System.Windows.Forms.DataGridView();
            this.labQueuePJ = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.ckbDrymode = new System.Windows.Forms.CheckBox();
            this.gbxCycle = new System.Windows.Forms.GroupBox();
            this.labNowCount = new System.Windows.Forms.Label();
            this.labCount = new System.Windows.Forms.Label();
            this.txtCycleCount = new System.Windows.Forms.TextBox();
            this.gbxCoreContrl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueuePJ)).BeginInit();
            this.gbxCycle.SuspendLayout();
            this.SuspendLayout();
            // 
            // labEFEMBusy
            // 
            this.labEFEMBusy.AutoSize = true;
            this.labEFEMBusy.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labEFEMBusy.Location = new System.Drawing.Point(236, 107);
            this.labEFEMBusy.Name = "labEFEMBusy";
            this.labEFEMBusy.Size = new System.Drawing.Size(45, 19);
            this.labEFEMBusy.TabIndex = 1;
            this.labEFEMBusy.Text = "EFEM";
            // 
            // labRobot1Busy
            // 
            this.labRobot1Busy.AutoSize = true;
            this.labRobot1Busy.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labRobot1Busy.Location = new System.Drawing.Point(236, 29);
            this.labRobot1Busy.Name = "labRobot1Busy";
            this.labRobot1Busy.Size = new System.Drawing.Size(63, 19);
            this.labRobot1Busy.TabIndex = 2;
            this.labRobot1Busy.Text = "Robot1";
            // 
            // labEFEMWork
            // 
            this.labEFEMWork.AutoSize = true;
            this.labEFEMWork.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labEFEMWork.Location = new System.Drawing.Point(328, 107);
            this.labEFEMWork.Name = "labEFEMWork";
            this.labEFEMWork.Size = new System.Drawing.Size(36, 19);
            this.labEFEMWork.TabIndex = 3;
            this.labEFEMWork.Text = "TBD";
            // 
            // btnRefreshJob
            // 
            this.btnRefreshJob.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefreshJob.Location = new System.Drawing.Point(17, 29);
            this.btnRefreshJob.Name = "btnRefreshJob";
            this.btnRefreshJob.Size = new System.Drawing.Size(180, 40);
            this.btnRefreshJob.TabIndex = 72;
            this.btnRefreshJob.Text = "Refresh Work Job";
            this.btnRefreshJob.UseVisualStyleBackColor = true;
            this.btnRefreshJob.Click += new System.EventHandler(this.btnRefreshJob_Click);
            // 
            // btnDeleteWaferJob
            // 
            this.btnDeleteWaferJob.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteWaferJob.Location = new System.Drawing.Point(497, 58);
            this.btnDeleteWaferJob.Name = "btnDeleteWaferJob";
            this.btnDeleteWaferJob.Size = new System.Drawing.Size(140, 40);
            this.btnDeleteWaferJob.TabIndex = 76;
            this.btnDeleteWaferJob.Text = "Delete Wafer";
            this.btnDeleteWaferJob.UseVisualStyleBackColor = true;
            this.btnDeleteWaferJob.Visible = false;
            this.btnDeleteWaferJob.Click += new System.EventHandler(this.btnDeleteWaferJob_Click);
            // 
            // cboWaferStatus
            // 
            this.cboWaferStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboWaferStatus.FormattingEnabled = true;
            this.cboWaferStatus.Location = new System.Drawing.Point(6, 26);
            this.cboWaferStatus.Name = "cboWaferStatus";
            this.cboWaferStatus.Size = new System.Drawing.Size(146, 27);
            this.cboWaferStatus.TabIndex = 78;
            // 
            // btnWaferChange
            // 
            this.btnWaferChange.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWaferChange.Location = new System.Drawing.Point(6, 67);
            this.btnWaferChange.Name = "btnWaferChange";
            this.btnWaferChange.Size = new System.Drawing.Size(146, 40);
            this.btnWaferChange.TabIndex = 77;
            this.btnWaferChange.Text = "Wafer Status";
            this.btnWaferChange.UseVisualStyleBackColor = true;
            this.btnWaferChange.Click += new System.EventHandler(this.btnJobChange_Click);
            // 
            // labRobot1Work
            // 
            this.labRobot1Work.AutoSize = true;
            this.labRobot1Work.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labRobot1Work.Location = new System.Drawing.Point(328, 29);
            this.labRobot1Work.Name = "labRobot1Work";
            this.labRobot1Work.Size = new System.Drawing.Size(36, 19);
            this.labRobot1Work.TabIndex = 81;
            this.labRobot1Work.Text = "TBD";
            // 
            // gbxCoreContrl
            // 
            this.gbxCoreContrl.Controls.Add(this.btnCarrierChange);
            this.gbxCoreContrl.Controls.Add(this.cboCarrierStatus);
            this.gbxCoreContrl.Controls.Add(this.btnWaferChange);
            this.gbxCoreContrl.Controls.Add(this.cboWaferStatus);
            this.gbxCoreContrl.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxCoreContrl.Location = new System.Drawing.Point(684, 29);
            this.gbxCoreContrl.Name = "gbxCoreContrl";
            this.gbxCoreContrl.Size = new System.Drawing.Size(315, 117);
            this.gbxCoreContrl.TabIndex = 82;
            this.gbxCoreContrl.TabStop = false;
            this.gbxCoreContrl.Text = "Control";
            // 
            // btnCarrierChange
            // 
            this.btnCarrierChange.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCarrierChange.Location = new System.Drawing.Point(158, 67);
            this.btnCarrierChange.Name = "btnCarrierChange";
            this.btnCarrierChange.Size = new System.Drawing.Size(146, 40);
            this.btnCarrierChange.TabIndex = 80;
            this.btnCarrierChange.Text = "Carrier Status";
            this.btnCarrierChange.UseVisualStyleBackColor = true;
            this.btnCarrierChange.Click += new System.EventHandler(this.btnCarrierChange_Click);
            // 
            // cboCarrierStatus
            // 
            this.cboCarrierStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCarrierStatus.FormattingEnabled = true;
            this.cboCarrierStatus.Location = new System.Drawing.Point(158, 26);
            this.cboCarrierStatus.Name = "cboCarrierStatus";
            this.cboCarrierStatus.Size = new System.Drawing.Size(146, 27);
            this.cboCarrierStatus.TabIndex = 79;
            // 
            // labRobot2Work
            // 
            this.labRobot2Work.AutoSize = true;
            this.labRobot2Work.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labRobot2Work.Location = new System.Drawing.Point(328, 69);
            this.labRobot2Work.Name = "labRobot2Work";
            this.labRobot2Work.Size = new System.Drawing.Size(36, 19);
            this.labRobot2Work.TabIndex = 88;
            this.labRobot2Work.Text = "TBD";
            // 
            // labRobot2Busy
            // 
            this.labRobot2Busy.AutoSize = true;
            this.labRobot2Busy.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labRobot2Busy.Location = new System.Drawing.Point(236, 69);
            this.labRobot2Busy.Name = "labRobot2Busy";
            this.labRobot2Busy.Size = new System.Drawing.Size(63, 19);
            this.labRobot2Busy.TabIndex = 87;
            this.labRobot2Busy.Text = "Robot2";
            // 
            // dgvQueuePJ
            // 
            this.dgvQueuePJ.AllowUserToAddRows = false;
            this.dgvQueuePJ.AllowUserToDeleteRows = false;
            this.dgvQueuePJ.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvQueuePJ.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvQueuePJ.Location = new System.Drawing.Point(17, 163);
            this.dgvQueuePJ.Name = "dgvQueuePJ";
            this.dgvQueuePJ.ReadOnly = true;
            this.dgvQueuePJ.RowHeadersWidth = 55;
            this.dgvQueuePJ.RowTemplate.Height = 24;
            this.dgvQueuePJ.Size = new System.Drawing.Size(1480, 473);
            this.dgvQueuePJ.TabIndex = 91;
            // 
            // labQueuePJ
            // 
            this.labQueuePJ.AutoSize = true;
            this.labQueuePJ.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labQueuePJ.Location = new System.Drawing.Point(13, 141);
            this.labQueuePJ.Name = "labQueuePJ";
            this.labQueuePJ.Size = new System.Drawing.Size(81, 19);
            this.labQueuePJ.TabIndex = 92;
            this.labQueuePJ.Text = "Queue PJ";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(562, 107);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 93;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // ckbDrymode
            // 
            this.ckbDrymode.AutoSize = true;
            this.ckbDrymode.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.ckbDrymode.Location = new System.Drawing.Point(16, 21);
            this.ckbDrymode.Name = "ckbDrymode";
            this.ckbDrymode.Size = new System.Drawing.Size(91, 23);
            this.ckbDrymode.TabIndex = 94;
            this.ckbDrymode.Text = "DryMode";
            this.ckbDrymode.UseVisualStyleBackColor = true;
            this.ckbDrymode.CheckedChanged += new System.EventHandler(this.ckbDrymode_CheckedChanged);
            // 
            // gbxCycle
            // 
            this.gbxCycle.Controls.Add(this.labNowCount);
            this.gbxCycle.Controls.Add(this.labCount);
            this.gbxCycle.Controls.Add(this.txtCycleCount);
            this.gbxCycle.Controls.Add(this.ckbDrymode);
            this.gbxCycle.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.gbxCycle.Location = new System.Drawing.Point(1192, 36);
            this.gbxCycle.Name = "gbxCycle";
            this.gbxCycle.Size = new System.Drawing.Size(227, 110);
            this.gbxCycle.TabIndex = 95;
            this.gbxCycle.TabStop = false;
            this.gbxCycle.Text = "cycle";
            // 
            // labNowCount
            // 
            this.labNowCount.AutoSize = true;
            this.labNowCount.Location = new System.Drawing.Point(68, 81);
            this.labNowCount.Name = "labNowCount";
            this.labNowCount.Size = new System.Drawing.Size(18, 19);
            this.labNowCount.TabIndex = 97;
            this.labNowCount.Text = "0";
            // 
            // labCount
            // 
            this.labCount.AutoSize = true;
            this.labCount.Location = new System.Drawing.Point(12, 49);
            this.labCount.Name = "labCount";
            this.labCount.Size = new System.Drawing.Size(54, 19);
            this.labCount.TabIndex = 96;
            this.labCount.Text = "Count";
            // 
            // txtCycleCount
            // 
            this.txtCycleCount.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.txtCycleCount.Location = new System.Drawing.Point(72, 46);
            this.txtCycleCount.Name = "txtCycleCount";
            this.txtCycleCount.Size = new System.Drawing.Size(100, 26);
            this.txtCycleCount.TabIndex = 95;
            this.txtCycleCount.TextChanged += new System.EventHandler(this.txtCycleCount_TextChanged);
            // 
            // UserCore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.gbxCycle);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnDeleteWaferJob);
            this.Controls.Add(this.labQueuePJ);
            this.Controls.Add(this.dgvQueuePJ);
            this.Controls.Add(this.labRobot2Work);
            this.Controls.Add(this.labRobot2Busy);
            this.Controls.Add(this.gbxCoreContrl);
            this.Controls.Add(this.labRobot1Work);
            this.Controls.Add(this.btnRefreshJob);
            this.Controls.Add(this.labEFEMWork);
            this.Controls.Add(this.labRobot1Busy);
            this.Controls.Add(this.labEFEMBusy);
            this.Name = "UserCore";
            this.Size = new System.Drawing.Size(1500, 639);
            this.gbxCoreContrl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueuePJ)).EndInit();
            this.gbxCycle.ResumeLayout(false);
            this.gbxCycle.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labEFEMBusy;
        private System.Windows.Forms.Label labRobot1Busy;
        private System.Windows.Forms.Label labEFEMWork;
        private System.Windows.Forms.Button btnRefreshJob;
        private System.Windows.Forms.Button btnDeleteWaferJob;
        private System.Windows.Forms.ComboBox cboWaferStatus;
        private System.Windows.Forms.Button btnWaferChange;
        private System.Windows.Forms.Label labRobot1Work;
        public System.Windows.Forms.GroupBox gbxCoreContrl;
        private System.Windows.Forms.Label labRobot2Work;
        private System.Windows.Forms.Label labRobot2Busy;
        public System.Windows.Forms.DataGridView dgvQueuePJ;
        private System.Windows.Forms.Label labQueuePJ;
        private System.Windows.Forms.Button btnCarrierChange;
        private System.Windows.Forms.ComboBox cboCarrierStatus;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox ckbDrymode;
        private System.Windows.Forms.GroupBox gbxCycle;
        private System.Windows.Forms.Label labCount;
        private System.Windows.Forms.TextBox txtCycleCount;
        private System.Windows.Forms.Label labNowCount;
    }
}
