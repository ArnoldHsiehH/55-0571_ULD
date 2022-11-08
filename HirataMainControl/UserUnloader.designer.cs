
namespace HirataMainControl
{
    partial class UserUnloader
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tb_LDULDConnect = new System.Windows.Forms.TextBox();
            this.lb_LDULD_Connect = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tb_LDULDConnect
            // 
            this.tb_LDULDConnect.Enabled = false;
            this.tb_LDULDConnect.Location = new System.Drawing.Point(28, 58);
            this.tb_LDULDConnect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tb_LDULDConnect.Name = "tb_LDULDConnect";
            this.tb_LDULDConnect.ReadOnly = true;
            this.tb_LDULDConnect.Size = new System.Drawing.Size(454, 31);
            this.tb_LDULDConnect.TabIndex = 3;
            // 
            // lb_LDULD_Connect
            // 
            this.lb_LDULD_Connect.AutoSize = true;
            this.lb_LDULD_Connect.Location = new System.Drawing.Point(24, 15);
            this.lb_LDULD_Connect.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lb_LDULD_Connect.Name = "lb_LDULD_Connect";
            this.lb_LDULD_Connect.Size = new System.Drawing.Size(264, 24);
            this.lb_LDULD_Connect.TabIndex = 2;
            this.lb_LDULD_Connect.Text = "LD <-> ULD Connect Status:";
            // 
            // UserUnloader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tb_LDULDConnect);
            this.Controls.Add(this.lb_LDULD_Connect);
            this.Font = new System.Drawing.Font("微軟正黑體", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "UserUnloader";
            this.Size = new System.Drawing.Size(526, 107);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_LDULDConnect;
        private System.Windows.Forms.Label lb_LDULD_Connect;
    }
}
