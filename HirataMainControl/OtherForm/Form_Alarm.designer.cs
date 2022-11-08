namespace HirataMainControl
{
    partial class Form_Alarm
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
            this.btnClearAlarm = new System.Windows.Forms.Button();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.ColumnTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnDevice = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvAlarmList = new System.Windows.Forms.ListView();
            this.ColumnCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splMain = new System.Windows.Forms.SplitContainer();
            this.labLogIn = new System.Windows.Forms.Label();
            this.btnBuzzerOff = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splMain)).BeginInit();
            this.splMain.Panel1.SuspendLayout();
            this.splMain.Panel2.SuspendLayout();
            this.splMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClearAlarm
            // 
            this.btnClearAlarm.BackColor = System.Drawing.SystemColors.Control;
            this.btnClearAlarm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnClearAlarm.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClearAlarm.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnClearAlarm.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnClearAlarm.Location = new System.Drawing.Point(621, 3);
            this.btnClearAlarm.Name = "btnClearAlarm";
            this.btnClearAlarm.Size = new System.Drawing.Size(130, 36);
            this.btnClearAlarm.TabIndex = 6;
            this.btnClearAlarm.Text = "Clear Alarm";
            this.btnClearAlarm.UseVisualStyleBackColor = true;
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.BackColor = System.Drawing.SystemColors.Control;
            this.chkSelectAll.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkSelectAll.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSelectAll.Location = new System.Drawing.Point(3, 3);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(126, 36);
            this.chkSelectAll.TabIndex = 2;
            this.chkSelectAll.Text = "Select All";
            this.chkSelectAll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkSelectAll.UseVisualStyleBackColor = false;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.SelectAll_CheckedChanged);
            // 
            // ColumnTime
            // 
            this.ColumnTime.Text = "Time";
            this.ColumnTime.Width = 119;
            // 
            // ColumnDevice
            // 
            this.ColumnDevice.Text = "Device";
            this.ColumnDevice.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColumnDevice.Width = 66;
            // 
            // ColumnMessage
            // 
            this.ColumnMessage.Text = "Alarm Text";
            this.ColumnMessage.Width = 351;
            // 
            // lvAlarmList
            // 
            this.lvAlarmList.AutoArrange = false;
            this.lvAlarmList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvAlarmList.CheckBoxes = true;
            this.lvAlarmList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnTime,
            this.ColumnDevice,
            this.ColumnCode,
            this.ColumnMessage});
            this.lvAlarmList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvAlarmList.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvAlarmList.ForeColor = System.Drawing.Color.White;
            this.lvAlarmList.FullRowSelect = true;
            this.lvAlarmList.GridLines = true;
            this.lvAlarmList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvAlarmList.HideSelection = false;
            this.lvAlarmList.Location = new System.Drawing.Point(0, 0);
            this.lvAlarmList.Margin = new System.Windows.Forms.Padding(10);
            this.lvAlarmList.Name = "lvAlarmList";
            this.lvAlarmList.Size = new System.Drawing.Size(884, 255);
            this.lvAlarmList.TabIndex = 1;
            this.lvAlarmList.UseCompatibleStateImageBehavior = false;
            this.lvAlarmList.View = System.Windows.Forms.View.Details;
            // 
            // ColumnCode
            // 
            this.ColumnCode.Text = "Code";
            this.ColumnCode.Width = 64;
            // 
            // splMain
            // 
            this.splMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splMain.IsSplitterFixed = true;
            this.splMain.Location = new System.Drawing.Point(0, 0);
            this.splMain.Name = "splMain";
            this.splMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splMain.Panel1
            // 
            this.splMain.Panel1.Controls.Add(this.lvAlarmList);
            // 
            // splMain.Panel2
            // 
            this.splMain.Panel2.Controls.Add(this.labLogIn);
            this.splMain.Panel2.Controls.Add(this.chkSelectAll);
            this.splMain.Panel2.Controls.Add(this.btnClearAlarm);
            this.splMain.Panel2.Controls.Add(this.btnBuzzerOff);
            this.splMain.Panel2.Padding = new System.Windows.Forms.Padding(3);
            this.splMain.Size = new System.Drawing.Size(884, 301);
            this.splMain.SplitterDistance = 255;
            this.splMain.TabIndex = 4;
            // 
            // labLogIn
            // 
            this.labLogIn.AutoSize = true;
            this.labLogIn.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labLogIn.Location = new System.Drawing.Point(307, 12);
            this.labLogIn.Name = "labLogIn";
            this.labLogIn.Size = new System.Drawing.Size(135, 19);
            this.labLogIn.TabIndex = 7;
            this.labLogIn.Text = "LogIn Engineer";
            // 
            // btnBuzzerOff
            // 
            this.btnBuzzerOff.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnBuzzerOff.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnBuzzerOff.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnBuzzerOff.Image = global::HirataMainControl.Properties.Resources.BuzzerOff;
            this.btnBuzzerOff.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBuzzerOff.Location = new System.Drawing.Point(751, 3);
            this.btnBuzzerOff.Name = "btnBuzzerOff";
            this.btnBuzzerOff.Size = new System.Drawing.Size(130, 36);
            this.btnBuzzerOff.TabIndex = 1;
            this.btnBuzzerOff.Text = "Buzzer Off";
            this.btnBuzzerOff.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnBuzzerOff.UseVisualStyleBackColor = true;
            // 
            // Form_Alarm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(884, 301);
            this.ControlBox = false;
            this.Controls.Add(this.splMain);
            this.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Location = new System.Drawing.Point(900, 600);
            this.MaximizeBox = false;
            this.Name = "Form_Alarm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Alarm/Warning List";
            this.TopMost = true;
            this.splMain.Panel1.ResumeLayout(false);
            this.splMain.Panel2.ResumeLayout(false);
            this.splMain.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splMain)).EndInit();
            this.splMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Button btnBuzzerOff;
        public System.Windows.Forms.Button btnClearAlarm;
        private System.Windows.Forms.CheckBox chkSelectAll;
        private System.Windows.Forms.ColumnHeader ColumnTime;
        private System.Windows.Forms.ColumnHeader ColumnDevice;
        private System.Windows.Forms.ColumnHeader ColumnMessage;
        private System.Windows.Forms.ListView lvAlarmList;
        private System.Windows.Forms.ColumnHeader ColumnCode;
        private System.Windows.Forms.SplitContainer splMain;
        private System.Windows.Forms.Label labLogIn;

    }
}