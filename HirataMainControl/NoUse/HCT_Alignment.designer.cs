namespace HirataMainControl
{
    partial class HCT_Alignment
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
            this.labBusy = new System.Windows.Forms.Label();
            this.picBigAlignment = new System.Windows.Forms.PictureBox();
            this.gbxAlignment = new System.Windows.Forms.GroupBox();
            this.labWaferInfo = new System.Windows.Forms.Label();
            this.VCR_B = new HirataMainControl.IO_VCR();
            this.VCR_S = new HirataMainControl.IO_VCR();
            this.labBigClamp = new System.Windows.Forms.Label();
            this.labSmallClamp = new System.Windows.Forms.Label();
            this.picSmallAlignment = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picBigAlignment)).BeginInit();
            this.gbxAlignment.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSmallAlignment)).BeginInit();
            this.SuspendLayout();
            // 
            // labBusy
            // 
            this.labBusy.BackColor = System.Drawing.Color.Yellow;
            this.labBusy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labBusy.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labBusy.Location = new System.Drawing.Point(26, 18);
            this.labBusy.Name = "labBusy";
            this.labBusy.Size = new System.Drawing.Size(60, 25);
            this.labBusy.TabIndex = 44;
            this.labBusy.Text = "Busy";
            this.labBusy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picBigAlignment
            // 
            this.picBigAlignment.Image = global::HirataMainControl.Properties.Resources.BigAlignment_WithOut;
            this.picBigAlignment.Location = new System.Drawing.Point(118, 83);
            this.picBigAlignment.Name = "picBigAlignment";
            this.picBigAlignment.Size = new System.Drawing.Size(108, 104);
            this.picBigAlignment.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picBigAlignment.TabIndex = 45;
            this.picBigAlignment.TabStop = false;
            // 
            // gbxAlignment
            // 
            this.gbxAlignment.Controls.Add(this.labWaferInfo);
            this.gbxAlignment.Controls.Add(this.VCR_B);
            this.gbxAlignment.Controls.Add(this.VCR_S);
            this.gbxAlignment.Controls.Add(this.labBigClamp);
            this.gbxAlignment.Controls.Add(this.labSmallClamp);
            this.gbxAlignment.Controls.Add(this.picSmallAlignment);
            this.gbxAlignment.Controls.Add(this.picBigAlignment);
            this.gbxAlignment.Controls.Add(this.labBusy);
            this.gbxAlignment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxAlignment.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxAlignment.Location = new System.Drawing.Point(0, 0);
            this.gbxAlignment.Name = "gbxAlignment";
            this.gbxAlignment.Size = new System.Drawing.Size(240, 365);
            this.gbxAlignment.TabIndex = 46;
            this.gbxAlignment.TabStop = false;
            this.gbxAlignment.Text = "groupBox1";
            // 
            // labWaferInfo
            // 
            this.labWaferInfo.BackColor = System.Drawing.Color.LightGreen;
            this.labWaferInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labWaferInfo.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labWaferInfo.Location = new System.Drawing.Point(116, 18);
            this.labWaferInfo.Name = "labWaferInfo";
            this.labWaferInfo.Size = new System.Drawing.Size(100, 25);
            this.labWaferInfo.TabIndex = 52;
            this.labWaferInfo.Text = ",";
            this.labWaferInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // VCR_B
            // 
            this.VCR_B.BackColor = System.Drawing.SystemColors.Control;
            this.VCR_B.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.VCR_B.Location = new System.Drawing.Point(16, 228);
            this.VCR_B.Name = "VCR_B";
            this.VCR_B.Size = new System.Drawing.Size(200, 29);
            this.VCR_B.TabIndex = 51;
            this.VCR_B.Ui_Busy = false;
            this.VCR_B.Ui_Connect = false;
            this.VCR_B.Ui_ID = "";
            // 
            // VCR_S
            // 
            this.VCR_S.BackColor = System.Drawing.SystemColors.Control;
            this.VCR_S.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.VCR_S.Location = new System.Drawing.Point(16, 193);
            this.VCR_S.Name = "VCR_S";
            this.VCR_S.Size = new System.Drawing.Size(200, 29);
            this.VCR_S.TabIndex = 50;
            this.VCR_S.Ui_Busy = false;
            this.VCR_S.Ui_Connect = false;
            this.VCR_S.Ui_ID = "";
            // 
            // labBigClamp
            // 
            this.labBigClamp.BackColor = System.Drawing.Color.LightGreen;
            this.labBigClamp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labBigClamp.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labBigClamp.Location = new System.Drawing.Point(139, 55);
            this.labBigClamp.Name = "labBigClamp";
            this.labBigClamp.Size = new System.Drawing.Size(60, 25);
            this.labBigClamp.TabIndex = 49;
            this.labBigClamp.Text = "Clamp";
            this.labBigClamp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labSmallClamp
            // 
            this.labSmallClamp.BackColor = System.Drawing.Color.LightGreen;
            this.labSmallClamp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labSmallClamp.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labSmallClamp.Location = new System.Drawing.Point(26, 55);
            this.labSmallClamp.Name = "labSmallClamp";
            this.labSmallClamp.Size = new System.Drawing.Size(60, 25);
            this.labSmallClamp.TabIndex = 48;
            this.labSmallClamp.Text = "Clamp";
            this.labSmallClamp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picSmallAlignment
            // 
            this.picSmallAlignment.Image = global::HirataMainControl.Properties.Resources.SmallAlignment_WithOut;
            this.picSmallAlignment.Location = new System.Drawing.Point(6, 83);
            this.picSmallAlignment.Name = "picSmallAlignment";
            this.picSmallAlignment.Size = new System.Drawing.Size(108, 104);
            this.picSmallAlignment.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picSmallAlignment.TabIndex = 47;
            this.picSmallAlignment.TabStop = false;
            // 
            // HCT_Alignment
            // 
            this.Controls.Add(this.gbxAlignment);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.Name = "HCT_Alignment";
            this.Size = new System.Drawing.Size(240, 365);
            ((System.ComponentModel.ISupportInitialize)(this.picBigAlignment)).EndInit();
            this.gbxAlignment.ResumeLayout(false);
            this.gbxAlignment.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSmallAlignment)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labBusy;
        private System.Windows.Forms.PictureBox picBigAlignment;
        private System.Windows.Forms.GroupBox gbxAlignment;
        private System.Windows.Forms.PictureBox picSmallAlignment;
        private System.Windows.Forms.Label labBigClamp;
        private System.Windows.Forms.Label labSmallClamp;
        private IO_VCR VCR_S;
        private IO_VCR VCR_B;
        private System.Windows.Forms.Label labWaferInfo;


    }
}
