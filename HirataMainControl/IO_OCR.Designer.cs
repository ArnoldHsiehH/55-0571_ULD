namespace HirataMainControl
{
    partial class IO_OCR
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
            this.gbxOCR = new System.Windows.Forms.GroupBox();
            this.labConnect = new System.Windows.Forms.Label();
            this.labOcrID = new System.Windows.Forms.Label();
            this.gbxOCR.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxOCR
            // 
            this.gbxOCR.Controls.Add(this.labConnect);
            this.gbxOCR.Controls.Add(this.labOcrID);
            this.gbxOCR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxOCR.Location = new System.Drawing.Point(0, 0);
            this.gbxOCR.Name = "gbxOCR";
            this.gbxOCR.Size = new System.Drawing.Size(230, 60);
            this.gbxOCR.TabIndex = 0;
            this.gbxOCR.TabStop = false;
            this.gbxOCR.Text = "groupBox1";
            // 
            // labConnect
            // 
            this.labConnect.BackColor = System.Drawing.Color.Red;
            this.labConnect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labConnect.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labConnect.Location = new System.Drawing.Point(7, 21);
            this.labConnect.Name = "labConnect";
            this.labConnect.Size = new System.Drawing.Size(58, 29);
            this.labConnect.TabIndex = 47;
            this.labConnect.Text = "Dis-C";
            this.labConnect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labOcrID
            // 
            this.labOcrID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labOcrID.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labOcrID.Location = new System.Drawing.Point(78, 21);
            this.labOcrID.Name = "labOcrID";
            this.labOcrID.Size = new System.Drawing.Size(142, 29);
            this.labOcrID.TabIndex = 46;
            this.labOcrID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // IO_OCR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbxOCR);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "IO_OCR";
            this.Size = new System.Drawing.Size(230, 60);
            this.gbxOCR.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxOCR;
        private System.Windows.Forms.Label labConnect;
        private System.Windows.Forms.Label labOcrID;
    }
}
