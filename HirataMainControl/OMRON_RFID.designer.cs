namespace HirataMainControl
{
    partial class OMRON_RFID
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
            this.gbxRFID = new System.Windows.Forms.GroupBox();
            this.labBusy = new System.Windows.Forms.Label();
            this.labFoupID = new System.Windows.Forms.Label();
            this.labConnect = new System.Windows.Forms.Label();
            this.tipInfo = new System.Windows.Forms.ToolTip(this.components);
            this.gbxRFID.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxRFID
            // 
            this.gbxRFID.Controls.Add(this.labBusy);
            this.gbxRFID.Controls.Add(this.labFoupID);
            this.gbxRFID.Controls.Add(this.labConnect);
            this.gbxRFID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxRFID.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.gbxRFID.Location = new System.Drawing.Point(0, 0);
            this.gbxRFID.Name = "gbxRFID";
            this.gbxRFID.Size = new System.Drawing.Size(280, 50);
            this.gbxRFID.TabIndex = 95;
            this.gbxRFID.TabStop = false;
            this.gbxRFID.Text = "RFID";
            // 
            // labBusy
            // 
            this.labBusy.BackColor = System.Drawing.Color.LightGreen;
            this.labBusy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labBusy.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.labBusy.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labBusy.Location = new System.Drawing.Point(227, 18);
            this.labBusy.Name = "labBusy";
            this.labBusy.Size = new System.Drawing.Size(50, 25);
            this.labBusy.TabIndex = 89;
            this.labBusy.Text = "Idle";
            this.labBusy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labFoupID
            // 
            this.labFoupID.BackColor = System.Drawing.SystemColors.Control;
            this.labFoupID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labFoupID.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.labFoupID.Location = new System.Drawing.Point(62, 18);
            this.labFoupID.Name = "labFoupID";
            this.labFoupID.Size = new System.Drawing.Size(159, 25);
            this.labFoupID.TabIndex = 88;
            this.labFoupID.Text = "Foup ID";
            this.labFoupID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labFoupID.MouseEnter += new System.EventHandler(this.labFoupID_MouseEnter);
            // 
            // labConnect
            // 
            this.labConnect.BackColor = System.Drawing.Color.Red;
            this.labConnect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labConnect.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.labConnect.Location = new System.Drawing.Point(6, 18);
            this.labConnect.Name = "labConnect";
            this.labConnect.Size = new System.Drawing.Size(50, 25);
            this.labConnect.TabIndex = 87;
            this.labConnect.Text = "Dis-C";
            this.labConnect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // OMRON_RFID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.gbxRFID);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "OMRON_RFID";
            this.Size = new System.Drawing.Size(280, 50);
            this.gbxRFID.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxRFID;
        private System.Windows.Forms.Label labConnect;
        private System.Windows.Forms.ToolTip tipInfo;
        private System.Windows.Forms.Label labFoupID;
        private System.Windows.Forms.Label labBusy;

    }
}
