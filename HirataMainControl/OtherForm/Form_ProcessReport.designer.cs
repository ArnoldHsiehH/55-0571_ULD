namespace HirataMainControl
{
    partial class Form_ProcessReport
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
            this.prcbBackup = new System.Windows.Forms.ProgressBar();
            this.labProStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // prcbBackup
            // 
            this.prcbBackup.Location = new System.Drawing.Point(2, 14);
            this.prcbBackup.Name = "prcbBackup";
            this.prcbBackup.Size = new System.Drawing.Size(328, 27);
            this.prcbBackup.TabIndex = 0;
            // 
            // labProStatus
            // 
            this.labProStatus.AutoSize = true;
            this.labProStatus.Font = new System.Drawing.Font("Consolas", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labProStatus.Location = new System.Drawing.Point(79, 55);
            this.labProStatus.Name = "labProStatus";
            this.labProStatus.Size = new System.Drawing.Size(165, 32);
            this.labProStatus.TabIndex = 1;
            this.labProStatus.Text = "Processing";
            // 
            // Form_ProcessReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 105);
            this.Controls.Add(this.labProStatus);
            this.Controls.Add(this.prcbBackup);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form_ProcessReport";
            this.Text = "ProcessReport";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar prcbBackup;
        private System.Windows.Forms.Label labProStatus;
    }
}