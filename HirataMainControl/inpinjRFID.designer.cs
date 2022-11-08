
namespace HirataMainControl
{
    partial class inpinjRFID
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
            this.txt_port1 = new System.Windows.Forms.TextBox();
            this.txt_port2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txt_port1
            // 
            this.txt_port1.Location = new System.Drawing.Point(97, 17);
            this.txt_port1.Name = "txt_port1";
            this.txt_port1.Size = new System.Drawing.Size(291, 22);
            this.txt_port1.TabIndex = 0;
            this.txt_port1.Text = "0000 0000 0000 0000";
            this.txt_port1.TextChanged += new System.EventHandler(this.txt_port1_TextChanged);
            // 
            // txt_port2
            // 
            this.txt_port2.Location = new System.Drawing.Point(97, 45);
            this.txt_port2.Name = "txt_port2";
            this.txt_port2.Size = new System.Drawing.Size(291, 22);
            this.txt_port2.TabIndex = 1;
            this.txt_port2.TextChanged += new System.EventHandler(this.txt_port2_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "Loadport 1:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Loadport 2:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // inpinjRFID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_port2);
            this.Controls.Add(this.txt_port1);
            this.Name = "inpinjRFID";
            this.Size = new System.Drawing.Size(410, 85);
            this.Load += new System.EventHandler(this.inpinjRFID_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_port1;
        private System.Windows.Forms.TextBox txt_port2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
