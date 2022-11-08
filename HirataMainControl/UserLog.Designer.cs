namespace HirataMainControl
{
    partial class LogLib
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
            this.tbctLog = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // tbctLog
            // 
            this.tbctLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbctLog.Location = new System.Drawing.Point(0, 0);
            this.tbctLog.Name = "tbctLog";
            this.tbctLog.SelectedIndex = 0;
            this.tbctLog.Size = new System.Drawing.Size(108, 57);
            this.tbctLog.TabIndex = 0;
            // 
            // LogLib
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.tbctLog);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "LogLib";
            this.Size = new System.Drawing.Size(108, 57);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbctLog;
    }
}
