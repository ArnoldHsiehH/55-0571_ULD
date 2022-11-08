
namespace HirataMainControl
{
    partial class HCT_LoadPort
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
            this.tipInfo = new System.Windows.Forms.ToolTip(this.components);
            this.tapgLoadPort = new System.Windows.Forms.TabPage();
            this.lvWaferStatus = new System.Windows.Forms.ListView();
            this.gbxLPstatus = new System.Windows.Forms.GroupBox();
            this.labAuto = new System.Windows.Forms.Label();
            this.labpitch = new System.Windows.Forms.Label();
            this.labConnect = new System.Windows.Forms.Label();
            this.labBusy = new System.Windows.Forms.Label();
            this.labPresent = new System.Windows.Forms.Label();
            this.labType = new System.Windows.Forms.Label();
            this.labAlarm = new System.Windows.Forms.Label();
            this.labStatus = new System.Windows.Forms.Label();
            this.tctlLoadport = new System.Windows.Forms.TabControl();
            this.tapgE84 = new System.Windows.Forms.TabPage();
            this.labInterlock = new System.Windows.Forms.Label();
            this.tapgLoadPort.SuspendLayout();
            this.gbxLPstatus.SuspendLayout();
            this.tctlLoadport.SuspendLayout();
            this.SuspendLayout();
            // 
            // tapgLoadPort
            // 
            this.tapgLoadPort.BackColor = System.Drawing.SystemColors.Control;
            this.tapgLoadPort.Controls.Add(this.lvWaferStatus);
            this.tapgLoadPort.Controls.Add(this.gbxLPstatus);
            this.tapgLoadPort.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.tapgLoadPort.Location = new System.Drawing.Point(4, 23);
            this.tapgLoadPort.Name = "tapgLoadPort";
            this.tapgLoadPort.Padding = new System.Windows.Forms.Padding(3);
            this.tapgLoadPort.Size = new System.Drawing.Size(322, 313);
            this.tapgLoadPort.TabIndex = 0;
            this.tapgLoadPort.Text = "LoadPort1";
            // 
            // lvWaferStatus
            // 
            this.lvWaferStatus.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.lvWaferStatus.Location = new System.Drawing.Point(3, 3);
            this.lvWaferStatus.Name = "lvWaferStatus";
            this.lvWaferStatus.Size = new System.Drawing.Size(179, 308);
            this.lvWaferStatus.TabIndex = 88;
            this.lvWaferStatus.UseCompatibleStateImageBehavior = false;
            // 
            // gbxLPstatus
            // 
            this.gbxLPstatus.Controls.Add(this.labInterlock);
            this.gbxLPstatus.Controls.Add(this.labAuto);
            this.gbxLPstatus.Controls.Add(this.labpitch);
            this.gbxLPstatus.Controls.Add(this.labConnect);
            this.gbxLPstatus.Controls.Add(this.labBusy);
            this.gbxLPstatus.Controls.Add(this.labPresent);
            this.gbxLPstatus.Controls.Add(this.labType);
            this.gbxLPstatus.Controls.Add(this.labAlarm);
            this.gbxLPstatus.Controls.Add(this.labStatus);
            this.gbxLPstatus.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.gbxLPstatus.Location = new System.Drawing.Point(187, 3);
            this.gbxLPstatus.Name = "gbxLPstatus";
            this.gbxLPstatus.Size = new System.Drawing.Size(125, 304);
            this.gbxLPstatus.TabIndex = 91;
            this.gbxLPstatus.TabStop = false;
            this.gbxLPstatus.Text = "LoadPort";
            // 
            // labAuto
            // 
            this.labAuto.BackColor = System.Drawing.Color.Red;
            this.labAuto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labAuto.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.labAuto.Location = new System.Drawing.Point(62, 21);
            this.labAuto.Name = "labAuto";
            this.labAuto.Size = new System.Drawing.Size(55, 29);
            this.labAuto.TabIndex = 88;
            this.labAuto.Text = "Manual";
            this.labAuto.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labpitch
            // 
            this.labpitch.BackColor = System.Drawing.SystemColors.Control;
            this.labpitch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labpitch.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.labpitch.Location = new System.Drawing.Point(62, 61);
            this.labpitch.Name = "labpitch";
            this.labpitch.Size = new System.Drawing.Size(55, 29);
            this.labpitch.TabIndex = 87;
            this.labpitch.Text = "10";
            this.labpitch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labpitch.MouseEnter += new System.EventHandler(this.labpitch_MouseEnter);
            // 
            // labConnect
            // 
            this.labConnect.BackColor = System.Drawing.Color.Red;
            this.labConnect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labConnect.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.labConnect.Location = new System.Drawing.Point(7, 21);
            this.labConnect.Name = "labConnect";
            this.labConnect.Size = new System.Drawing.Size(55, 29);
            this.labConnect.TabIndex = 86;
            this.labConnect.Text = "Dis-C";
            this.labConnect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labBusy
            // 
            this.labBusy.BackColor = System.Drawing.Color.Yellow;
            this.labBusy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labBusy.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.labBusy.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labBusy.Location = new System.Drawing.Point(7, 148);
            this.labBusy.Name = "labBusy";
            this.labBusy.Size = new System.Drawing.Size(110, 29);
            this.labBusy.TabIndex = 84;
            this.labBusy.Text = "Busy";
            this.labBusy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labPresent
            // 
            this.labPresent.BackColor = System.Drawing.Color.LightGreen;
            this.labPresent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labPresent.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.labPresent.Location = new System.Drawing.Point(7, 90);
            this.labPresent.Name = "labPresent";
            this.labPresent.Size = new System.Drawing.Size(110, 29);
            this.labPresent.TabIndex = 85;
            this.labPresent.Text = "None Foup";
            this.labPresent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labType
            // 
            this.labType.BackColor = System.Drawing.SystemColors.Control;
            this.labType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labType.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.labType.Location = new System.Drawing.Point(7, 61);
            this.labType.Name = "labType";
            this.labType.Size = new System.Drawing.Size(55, 29);
            this.labType.TabIndex = 81;
            this.labType.Text = "Type_1";
            this.labType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labType.MouseEnter += new System.EventHandler(this.labType_MouseEnter);
            // 
            // labAlarm
            // 
            this.labAlarm.BackColor = System.Drawing.Color.Red;
            this.labAlarm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labAlarm.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.labAlarm.Location = new System.Drawing.Point(7, 177);
            this.labAlarm.Name = "labAlarm";
            this.labAlarm.Size = new System.Drawing.Size(110, 29);
            this.labAlarm.TabIndex = 83;
            this.labAlarm.Text = "Error:10";
            this.labAlarm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labStatus
            // 
            this.labStatus.BackColor = System.Drawing.Color.LightGreen;
            this.labStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labStatus.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.labStatus.Location = new System.Drawing.Point(7, 119);
            this.labStatus.Name = "labStatus";
            this.labStatus.Size = new System.Drawing.Size(110, 29);
            this.labStatus.TabIndex = 82;
            this.labStatus.Text = "Unload";
            this.labStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tctlLoadport
            // 
            this.tctlLoadport.Controls.Add(this.tapgLoadPort);
            this.tctlLoadport.Controls.Add(this.tapgE84);
            this.tctlLoadport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tctlLoadport.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.tctlLoadport.Location = new System.Drawing.Point(0, 0);
            this.tctlLoadport.Name = "tctlLoadport";
            this.tctlLoadport.SelectedIndex = 0;
            this.tctlLoadport.Size = new System.Drawing.Size(330, 340);
            this.tctlLoadport.TabIndex = 93;
            // 
            // tapgE84
            // 
            this.tapgE84.BackColor = System.Drawing.SystemColors.Control;
            this.tapgE84.Location = new System.Drawing.Point(4, 23);
            this.tapgE84.Name = "tapgE84";
            this.tapgE84.Padding = new System.Windows.Forms.Padding(3);
            this.tapgE84.Size = new System.Drawing.Size(322, 313);
            this.tapgE84.TabIndex = 1;
            this.tapgE84.Text = "E84 ";
            // 
            // labInterlock
            // 
            this.labInterlock.BackColor = System.Drawing.Color.Yellow;
            this.labInterlock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labInterlock.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.labInterlock.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labInterlock.Location = new System.Drawing.Point(7, 206);
            this.labInterlock.Name = "labInterlock";
            this.labInterlock.Size = new System.Drawing.Size(110, 29);
            this.labInterlock.TabIndex = 89;
            this.labInterlock.Text = "Interlock:1E";
            this.labInterlock.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // HCT_LoadPort
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.tctlLoadport);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.Name = "HCT_LoadPort";
            this.Size = new System.Drawing.Size(330, 340);
            this.tapgLoadPort.ResumeLayout(false);
            this.gbxLPstatus.ResumeLayout(false);
            this.tctlLoadport.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip tipInfo;
        private System.Windows.Forms.TabPage tapgLoadPort;
        private System.Windows.Forms.ListView lvWaferStatus;
        private System.Windows.Forms.GroupBox gbxLPstatus;
        private System.Windows.Forms.Label labAuto;
        private System.Windows.Forms.Label labpitch;
        private System.Windows.Forms.Label labConnect;
        private System.Windows.Forms.Label labBusy;
        private System.Windows.Forms.Label labPresent;
        private System.Windows.Forms.Label labType;
        private System.Windows.Forms.Label labAlarm;
        private System.Windows.Forms.Label labStatus;
        private System.Windows.Forms.TabControl tctlLoadport;
        private System.Windows.Forms.TabPage tapgE84;
        private System.Windows.Forms.Label labInterlock;

    }
}
