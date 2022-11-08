
namespace HirataMainControl
{
    partial class frm_BarcodeFail
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
            this.lb_BarcodeHeader = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btn_BarcodeEnter = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lb_BarcodeHeader
            // 
            this.lb_BarcodeHeader.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lb_BarcodeHeader.Location = new System.Drawing.Point(14, 19);
            this.lb_BarcodeHeader.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lb_BarcodeHeader.Name = "lb_BarcodeHeader";
            this.lb_BarcodeHeader.Size = new System.Drawing.Size(266, 56);
            this.lb_BarcodeHeader.TabIndex = 0;
            this.lb_BarcodeHeader.Text = "BarCode Read Fail , Please enter !";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(19, 90);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(261, 29);
            this.textBox1.TabIndex = 1;
            // 
            // btn_BarcodeEnter
            // 
            this.btn_BarcodeEnter.Location = new System.Drawing.Point(162, 126);
            this.btn_BarcodeEnter.Name = "btn_BarcodeEnter";
            this.btn_BarcodeEnter.Size = new System.Drawing.Size(118, 52);
            this.btn_BarcodeEnter.TabIndex = 2;
            this.btn_BarcodeEnter.Text = "Enter";
            this.btn_BarcodeEnter.UseVisualStyleBackColor = true;
            this.btn_BarcodeEnter.Click += new System.EventHandler(this.btn_BarcodeEnter_Click);
            // 
            // frm_BarcodeFail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 183);
            this.Controls.Add(this.btn_BarcodeEnter);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lb_BarcodeHeader);
            this.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "frm_BarcodeFail";
            this.Text = "frm_BarcodeFail";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lb_BarcodeHeader;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btn_BarcodeEnter;
    }
}