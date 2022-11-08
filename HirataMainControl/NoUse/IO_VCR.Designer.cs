namespace HirataMainControl
{
    partial class IO_VCR
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
            this.labVcrID = new System.Windows.Forms.Label();
            this.labConnect = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labVcrID
            // 
            this.labVcrID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labVcrID.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labVcrID.Location = new System.Drawing.Point(75, 0);
            this.labVcrID.Name = "labVcrID";
            this.labVcrID.Size = new System.Drawing.Size(122, 29);
            this.labVcrID.TabIndex = 0;
            this.labVcrID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labConnect
            // 
            this.labConnect.BackColor = System.Drawing.Color.Red;
            this.labConnect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labConnect.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labConnect.Location = new System.Drawing.Point(3, 0);
            this.labConnect.Name = "labConnect";
            this.labConnect.Size = new System.Drawing.Size(58, 29);
            this.labConnect.TabIndex = 45;
            this.labConnect.Text = "Dis-C";
            this.labConnect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // IO_VCR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.labVcrID);
            this.Controls.Add(this.labConnect);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.Name = "IO_VCR";
            this.Size = new System.Drawing.Size(200, 29);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labVcrID;
        private System.Windows.Forms.Label labConnect;

    }
}