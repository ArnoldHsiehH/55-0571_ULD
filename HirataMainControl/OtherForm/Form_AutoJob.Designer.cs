namespace HirataMainControl
{
    partial class Form_AutoJob
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
            this.pnlForm = new System.Windows.Forms.Panel();
            this.gbxSource = new System.Windows.Forms.GroupBox();
            this.cboSource = new System.Windows.Forms.ComboBox();
            this.pnlSource = new System.Windows.Forms.Panel();
            this.labSource = new System.Windows.Forms.Label();
            this.btnRun = new System.Windows.Forms.Button();
            this.gbxTotal = new System.Windows.Forms.GroupBox();
            this.pnlForm.SuspendLayout();
            this.gbxSource.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlForm
            // 
            this.pnlForm.Controls.Add(this.gbxSource);
            this.pnlForm.Controls.Add(this.btnRun);
            this.pnlForm.Controls.Add(this.gbxTotal);
            this.pnlForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlForm.Location = new System.Drawing.Point(0, 0);
            this.pnlForm.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlForm.Name = "pnlForm";
            this.pnlForm.Size = new System.Drawing.Size(1111, 911);
            this.pnlForm.TabIndex = 1;
            // 
            // gbxSource
            // 
            this.gbxSource.Controls.Add(this.cboSource);
            this.gbxSource.Controls.Add(this.pnlSource);
            this.gbxSource.Controls.Add(this.labSource);
            this.gbxSource.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxSource.Location = new System.Drawing.Point(5, 389);
            this.gbxSource.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbxSource.Name = "gbxSource";
            this.gbxSource.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbxSource.Size = new System.Drawing.Size(469, 506);
            this.gbxSource.TabIndex = 38;
            this.gbxSource.TabStop = false;
            this.gbxSource.Text = "SourcePort";
            // 
            // cboSource
            // 
            this.cboSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSource.FormattingEnabled = true;
            this.cboSource.Location = new System.Drawing.Point(72, 18);
            this.cboSource.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboSource.Name = "cboSource";
            this.cboSource.Size = new System.Drawing.Size(160, 27);
            this.cboSource.TabIndex = 5;
            this.cboSource.SelectedIndexChanged += new System.EventHandler(this.cboSoc_SelectedIndexChanged);
            // 
            // pnlSource
            // 
            this.pnlSource.Location = new System.Drawing.Point(12, 48);
            this.pnlSource.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlSource.Name = "pnlSource";
            this.pnlSource.Size = new System.Drawing.Size(449, 445);
            this.pnlSource.TabIndex = 4;
            // 
            // labSource
            // 
            this.labSource.AutoSize = true;
            this.labSource.Location = new System.Drawing.Point(8, 21);
            this.labSource.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labSource.Name = "labSource";
            this.labSource.Size = new System.Drawing.Size(54, 20);
            this.labSource.TabIndex = 0;
            this.labSource.Text = "Port:";
            // 
            // btnRun
            // 
            this.btnRun.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnRun.Location = new System.Drawing.Point(951, 539);
            this.btnRun.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(145, 48);
            this.btnRun.TabIndex = 37;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // gbxTotal
            // 
            this.gbxTotal.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxTotal.Location = new System.Drawing.Point(5, 5);
            this.gbxTotal.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbxTotal.Name = "gbxTotal";
            this.gbxTotal.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbxTotal.Size = new System.Drawing.Size(1091, 376);
            this.gbxTotal.TabIndex = 2;
            this.gbxTotal.TabStop = false;
            this.gbxTotal.Text = "Auto Job";
            // 
            // Form_AutoJob
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1111, 911);
            this.Controls.Add(this.pnlForm);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form_AutoJob";
            this.Text = "Form_AutoJob";
            this.pnlForm.ResumeLayout(false);
            this.gbxSource.ResumeLayout(false);
            this.gbxSource.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlForm;
        private System.Windows.Forms.GroupBox gbxTotal;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.GroupBox gbxSource;
        private System.Windows.Forms.ComboBox cboSource;
        private System.Windows.Forms.Panel pnlSource;
        private System.Windows.Forms.Label labSource;
    }
}