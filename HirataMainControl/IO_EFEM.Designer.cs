namespace HirataMainControl
{
    partial class IO_EFEM
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
            this.components = new System.ComponentModel.Container();
            this.gbxTable = new System.Windows.Forms.GroupBox();
            this.tmr1s = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // gbxTable
            // 
            this.gbxTable.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.gbxTable.Location = new System.Drawing.Point(4, 3);
            this.gbxTable.Name = "gbxTable";
            this.gbxTable.Size = new System.Drawing.Size(408, 446);
            this.gbxTable.TabIndex = 52;
            this.gbxTable.TabStop = false;
            this.gbxTable.Text = "IO Monitor";
            // 
            // IO_EFEM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbxTable);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.Name = "IO_EFEM";
            this.Size = new System.Drawing.Size(419, 452);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxTable;
        private System.Windows.Forms.Timer tmr1s;
    }
}
