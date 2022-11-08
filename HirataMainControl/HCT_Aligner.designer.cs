namespace HirataMainControl
{
    partial class HCT_Aligner
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
            this.labStatus = new System.Windows.Forms.Label();
            this.labBusy = new System.Windows.Forms.Label();
            this.gbxAligner = new System.Windows.Forms.GroupBox();
            this.lab90 = new System.Windows.Forms.Label();
            this.lab0 = new System.Windows.Forms.Label();
            this.labLiftPin = new System.Windows.Forms.Label();
            this.labToAngle = new System.Windows.Forms.Label();
            this.labNotchAngle = new System.Windows.Forms.Label();
            this.labType = new System.Windows.Forms.Label();
            this.labWaferInfo = new System.Windows.Forms.Label();
            this.labAlarm = new System.Windows.Forms.Label();
            this.labConnect = new System.Windows.Forms.Label();
            this.labMode = new System.Windows.Forms.Label();
            this.picAligner = new System.Windows.Forms.PictureBox();
            this.gbxAligner.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAligner)).BeginInit();
            this.SuspendLayout();
            // 
            // labStatus
            // 
            this.labStatus.BackColor = System.Drawing.Color.Yellow;
            this.labStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labStatus.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labStatus.Location = new System.Drawing.Point(316, 14);
            this.labStatus.Name = "labStatus";
            this.labStatus.Size = new System.Drawing.Size(81, 29);
            this.labStatus.TabIndex = 1;
            this.labStatus.Text = "Unknown";
            this.labStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labBusy
            // 
            this.labBusy.BackColor = System.Drawing.Color.Yellow;
            this.labBusy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labBusy.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labBusy.Location = new System.Drawing.Point(240, 14);
            this.labBusy.Name = "labBusy";
            this.labBusy.Size = new System.Drawing.Size(58, 29);
            this.labBusy.TabIndex = 2;
            this.labBusy.Text = "Busy";
            this.labBusy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gbxAligner
            // 
            this.gbxAligner.Controls.Add(this.lab90);
            this.gbxAligner.Controls.Add(this.lab0);
            this.gbxAligner.Controls.Add(this.labLiftPin);
            this.gbxAligner.Controls.Add(this.labToAngle);
            this.gbxAligner.Controls.Add(this.labNotchAngle);
            this.gbxAligner.Controls.Add(this.labType);
            this.gbxAligner.Controls.Add(this.labWaferInfo);
            this.gbxAligner.Controls.Add(this.labAlarm);
            this.gbxAligner.Controls.Add(this.labConnect);
            this.gbxAligner.Controls.Add(this.labMode);
            this.gbxAligner.Controls.Add(this.labStatus);
            this.gbxAligner.Controls.Add(this.labBusy);
            this.gbxAligner.Controls.Add(this.picAligner);
            this.gbxAligner.Cursor = System.Windows.Forms.Cursors.Default;
            this.gbxAligner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxAligner.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.gbxAligner.Location = new System.Drawing.Point(0, 0);
            this.gbxAligner.Name = "gbxAligner";
            this.gbxAligner.Size = new System.Drawing.Size(400, 145);
            this.gbxAligner.TabIndex = 4;
            this.gbxAligner.TabStop = false;
            this.gbxAligner.Text = "groupBox1";
            // 
            // lab90
            // 
            this.lab90.AutoSize = true;
            this.lab90.Location = new System.Drawing.Point(78, 14);
            this.lab90.Name = "lab90";
            this.lab90.Size = new System.Drawing.Size(24, 18);
            this.lab90.TabIndex = 57;
            this.lab90.Text = "90";
            // 
            // lab0
            // 
            this.lab0.AutoSize = true;
            this.lab0.Location = new System.Drawing.Point(3, 72);
            this.lab0.Name = "lab0";
            this.lab0.Size = new System.Drawing.Size(16, 18);
            this.lab0.TabIndex = 56;
            this.lab0.Text = "0";
            // 
            // labLiftPin
            // 
            this.labLiftPin.BackColor = System.Drawing.Color.Yellow;
            this.labLiftPin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labLiftPin.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labLiftPin.Location = new System.Drawing.Point(179, 107);
            this.labLiftPin.Name = "labLiftPin";
            this.labLiftPin.Size = new System.Drawing.Size(81, 29);
            this.labLiftPin.TabIndex = 55;
            this.labLiftPin.Text = "Up";
            this.labLiftPin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labToAngle
            // 
            this.labToAngle.BackColor = System.Drawing.SystemColors.ControlLight;
            this.labToAngle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labToAngle.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labToAngle.Location = new System.Drawing.Point(240, 72);
            this.labToAngle.Name = "labToAngle";
            this.labToAngle.Size = new System.Drawing.Size(58, 29);
            this.labToAngle.TabIndex = 54;
            this.labToAngle.Text = "0";
            this.labToAngle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labNotchAngle
            // 
            this.labNotchAngle.BackColor = System.Drawing.SystemColors.ControlLight;
            this.labNotchAngle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labNotchAngle.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labNotchAngle.Location = new System.Drawing.Point(179, 72);
            this.labNotchAngle.Name = "labNotchAngle";
            this.labNotchAngle.Size = new System.Drawing.Size(58, 29);
            this.labNotchAngle.TabIndex = 53;
            this.labNotchAngle.Text = "0";
            this.labNotchAngle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labType
            // 
            this.labType.BackColor = System.Drawing.SystemColors.ControlLight;
            this.labType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labType.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labType.Location = new System.Drawing.Point(240, 43);
            this.labType.Name = "labType";
            this.labType.Size = new System.Drawing.Size(58, 29);
            this.labType.TabIndex = 52;
            this.labType.Text = "4";
            this.labType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labWaferInfo
            // 
            this.labWaferInfo.BackColor = System.Drawing.Color.White;
            this.labWaferInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labWaferInfo.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labWaferInfo.Location = new System.Drawing.Point(262, 107);
            this.labWaferInfo.Name = "labWaferInfo";
            this.labWaferInfo.Size = new System.Drawing.Size(134, 29);
            this.labWaferInfo.TabIndex = 50;
            this.labWaferInfo.Text = ", / ,";
            this.labWaferInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labAlarm
            // 
            this.labAlarm.BackColor = System.Drawing.SystemColors.ControlLight;
            this.labAlarm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labAlarm.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labAlarm.Location = new System.Drawing.Point(316, 43);
            this.labAlarm.Name = "labAlarm";
            this.labAlarm.Size = new System.Drawing.Size(81, 29);
            this.labAlarm.TabIndex = 50;
            this.labAlarm.Text = "Normal";
            this.labAlarm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labConnect
            // 
            this.labConnect.BackColor = System.Drawing.Color.Red;
            this.labConnect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labConnect.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labConnect.Location = new System.Drawing.Point(179, 14);
            this.labConnect.Name = "labConnect";
            this.labConnect.Size = new System.Drawing.Size(58, 29);
            this.labConnect.TabIndex = 1;
            this.labConnect.Text = "Dis-C";
            this.labConnect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labMode
            // 
            this.labMode.BackColor = System.Drawing.SystemColors.ControlLight;
            this.labMode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labMode.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labMode.Location = new System.Drawing.Point(179, 43);
            this.labMode.Name = "labMode";
            this.labMode.Size = new System.Drawing.Size(58, 29);
            this.labMode.TabIndex = 1;
            this.labMode.Text = "Manual";
            this.labMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picAligner
            // 
            this.picAligner.Image = global::HirataMainControl.Properties.Resources.Aligner_WithOut;
            this.picAligner.Location = new System.Drawing.Point(6, 19);
            this.picAligner.Name = "picAligner";
            this.picAligner.Size = new System.Drawing.Size(170, 118);
            this.picAligner.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picAligner.TabIndex = 0;
            this.picAligner.TabStop = false;
            // 
            // HCT_Aligner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbxAligner);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "HCT_Aligner";
            this.Size = new System.Drawing.Size(400, 145);
            this.gbxAligner.ResumeLayout(false);
            this.gbxAligner.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAligner)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picAligner;
        private System.Windows.Forms.Label labStatus;
        public System.Windows.Forms.Label labBusy;
        private System.Windows.Forms.GroupBox gbxAligner;
        private System.Windows.Forms.Label labAlarm;
        private System.Windows.Forms.Label labToAngle;
        private System.Windows.Forms.Label labNotchAngle;
        private System.Windows.Forms.Label labType;
        private System.Windows.Forms.Label labWaferInfo;
        private System.Windows.Forms.Label labMode;
        private System.Windows.Forms.Label labConnect;
        private System.Windows.Forms.Label labLiftPin;
        private System.Windows.Forms.Label lab90;
        private System.Windows.Forms.Label lab0;
    }
}
