namespace HirataMainControl
{
    partial class Robot1
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
            this.labBusy = new System.Windows.Forms.Label();
            this.labConnect = new System.Windows.Forms.Label();
            this.labStatus = new System.Windows.Forms.Label();
            this.labRemote = new System.Windows.Forms.Label();
            this.labLowerXLocation = new System.Windows.Forms.Label();
            this.labUpperYLocation = new System.Windows.Forms.Label();
            this.labLowerRLocation = new System.Windows.Forms.Label();
            this.picRobot = new System.Windows.Forms.PictureBox();
            this.pnlRobot = new System.Windows.Forms.Panel();
            this.labLowerWaferInfo = new System.Windows.Forms.Label();
            this.labUpperWaferInfo = new System.Windows.Forms.Label();
            this.labRbLocation = new System.Windows.Forms.Label();
            this.labSpeed = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picRobot)).BeginInit();
            this.pnlRobot.SuspendLayout();
            this.SuspendLayout();
            // 
            // labBusy
            // 
            this.labBusy.BackColor = System.Drawing.Color.Yellow;
            this.labBusy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labBusy.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labBusy.Location = new System.Drawing.Point(0, 40);
            this.labBusy.Name = "labBusy";
            this.labBusy.Size = new System.Drawing.Size(58, 20);
            this.labBusy.TabIndex = 43;
            this.labBusy.Text = "Busy";
            this.labBusy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labConnect
            // 
            this.labConnect.BackColor = System.Drawing.Color.Red;
            this.labConnect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labConnect.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labConnect.Location = new System.Drawing.Point(0, 20);
            this.labConnect.Name = "labConnect";
            this.labConnect.Size = new System.Drawing.Size(58, 20);
            this.labConnect.TabIndex = 44;
            this.labConnect.Text = "Dis-C";
            this.labConnect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labStatus
            // 
            this.labStatus.BackColor = System.Drawing.SystemColors.ControlLight;
            this.labStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labStatus.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labStatus.Location = new System.Drawing.Point(0, 78);
            this.labStatus.Name = "labStatus";
            this.labStatus.Size = new System.Drawing.Size(58, 20);
            this.labStatus.TabIndex = 45;
            this.labStatus.Text = "0601";
            this.labStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labRemote
            // 
            this.labRemote.BackColor = System.Drawing.SystemColors.ControlLight;
            this.labRemote.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labRemote.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labRemote.Location = new System.Drawing.Point(0, 0);
            this.labRemote.Name = "labRemote";
            this.labRemote.Size = new System.Drawing.Size(58, 20);
            this.labRemote.TabIndex = 49;
            this.labRemote.Text = "On-L";
            this.labRemote.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labLowerXLocation
            // 
            this.labLowerXLocation.BackColor = System.Drawing.SystemColors.ControlLight;
            this.labLowerXLocation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labLowerXLocation.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labLowerXLocation.Location = new System.Drawing.Point(0, 35);
            this.labLowerXLocation.Name = "labLowerXLocation";
            this.labLowerXLocation.Size = new System.Drawing.Size(70, 29);
            this.labLowerXLocation.TabIndex = 57;
            this.labLowerXLocation.Text = "Home";
            this.labLowerXLocation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labUpperYLocation
            // 
            this.labUpperYLocation.BackColor = System.Drawing.SystemColors.ControlLight;
            this.labUpperYLocation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labUpperYLocation.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labUpperYLocation.Location = new System.Drawing.Point(0, 0);
            this.labUpperYLocation.Name = "labUpperYLocation";
            this.labUpperYLocation.Size = new System.Drawing.Size(70, 29);
            this.labUpperYLocation.TabIndex = 58;
            this.labUpperYLocation.Text = "Home";
            this.labUpperYLocation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labLowerRLocation
            // 
            this.labLowerRLocation.BackColor = System.Drawing.SystemColors.ControlLight;
            this.labLowerRLocation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labLowerRLocation.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labLowerRLocation.Location = new System.Drawing.Point(0, 69);
            this.labLowerRLocation.Name = "labLowerRLocation";
            this.labLowerRLocation.Size = new System.Drawing.Size(70, 29);
            this.labLowerRLocation.TabIndex = 59;
            this.labLowerRLocation.Text = "Home";
            this.labLowerRLocation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picRobot
            // 
            this.picRobot.BackColor = System.Drawing.Color.Transparent;
            this.picRobot.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.picRobot.Image = global::HirataMainControl.Properties.Resources.Robot_DWithOut;
            this.picRobot.Location = new System.Drawing.Point(58, 0);
            this.picRobot.Name = "picRobot";
            this.picRobot.Size = new System.Drawing.Size(189, 101);
            this.picRobot.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picRobot.TabIndex = 60;
            this.picRobot.TabStop = false;
            // 
            // pnlRobot
            // 
            this.pnlRobot.BackColor = System.Drawing.Color.Transparent;
            this.pnlRobot.Controls.Add(this.labLowerWaferInfo);
            this.pnlRobot.Controls.Add(this.labUpperWaferInfo);
            this.pnlRobot.Controls.Add(this.labLowerRLocation);
            this.pnlRobot.Controls.Add(this.labRbLocation);
            this.pnlRobot.Controls.Add(this.labLowerXLocation);
            this.pnlRobot.Controls.Add(this.labUpperYLocation);
            this.pnlRobot.Controls.Add(this.picRobot);
            this.pnlRobot.Location = new System.Drawing.Point(583, 0);
            this.pnlRobot.Name = "pnlRobot";
            this.pnlRobot.Size = new System.Drawing.Size(463, 100);
            this.pnlRobot.TabIndex = 63;
            // 
            // labLowerWaferInfo
            // 
            this.labLowerWaferInfo.BackColor = System.Drawing.Color.White;
            this.labLowerWaferInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labLowerWaferInfo.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labLowerWaferInfo.Location = new System.Drawing.Point(253, 69);
            this.labLowerWaferInfo.Name = "labLowerWaferInfo";
            this.labLowerWaferInfo.Size = new System.Drawing.Size(172, 29);
            this.labLowerWaferInfo.TabIndex = 66;
            this.labLowerWaferInfo.Text = ",/,";
            this.labLowerWaferInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labUpperWaferInfo
            // 
            this.labUpperWaferInfo.BackColor = System.Drawing.Color.White;
            this.labUpperWaferInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labUpperWaferInfo.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labUpperWaferInfo.Location = new System.Drawing.Point(253, 0);
            this.labUpperWaferInfo.Name = "labUpperWaferInfo";
            this.labUpperWaferInfo.Size = new System.Drawing.Size(172, 29);
            this.labUpperWaferInfo.TabIndex = 62;
            this.labUpperWaferInfo.Text = ",/,";
            this.labUpperWaferInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labRbLocation
            // 
            this.labRbLocation.BackColor = System.Drawing.SystemColors.ControlLight;
            this.labRbLocation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labRbLocation.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labRbLocation.Location = new System.Drawing.Point(253, 35);
            this.labRbLocation.Name = "labRbLocation";
            this.labRbLocation.Size = new System.Drawing.Size(93, 29);
            this.labRbLocation.TabIndex = 65;
            this.labRbLocation.Text = "Home";
            this.labRbLocation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labSpeed
            // 
            this.labSpeed.BackColor = System.Drawing.SystemColors.ControlLight;
            this.labSpeed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labSpeed.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labSpeed.Location = new System.Drawing.Point(0, 60);
            this.labSpeed.Name = "labSpeed";
            this.labSpeed.Size = new System.Drawing.Size(58, 20);
            this.labSpeed.TabIndex = 64;
            this.labSpeed.Text = "5-5";
            this.labSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Robot1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::HirataMainControl.Properties.Resources.Robot_Route;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.labSpeed);
            this.Controls.Add(this.pnlRobot);
            this.Controls.Add(this.labConnect);
            this.Controls.Add(this.labBusy);
            this.Controls.Add(this.labStatus);
            this.Controls.Add(this.labRemote);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Robot1";
            this.Size = new System.Drawing.Size(2280, 100);
            ((System.ComponentModel.ISupportInitialize)(this.picRobot)).EndInit();
            this.pnlRobot.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labBusy;
        private System.Windows.Forms.Label labConnect;
        private System.Windows.Forms.Label labStatus;
        private System.Windows.Forms.Label labRemote;
        private System.Windows.Forms.Label labLowerXLocation;
        private System.Windows.Forms.Label labUpperYLocation;
        private System.Windows.Forms.Label labLowerRLocation;
        private System.Windows.Forms.PictureBox picRobot;
        private System.Windows.Forms.Panel pnlRobot;
        private System.Windows.Forms.Label labSpeed;
        private System.Windows.Forms.Label labUpperWaferInfo;
        private System.Windows.Forms.Label labRbLocation;
        private System.Windows.Forms.Label labLowerWaferInfo;
    }
}
