namespace HirataMainControl
{
    partial class HCT_EFEM
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HCT_EFEM));
            this.tapgEFEM = new System.Windows.Forms.TabPage();
            this.dgv_BarcodeList = new System.Windows.Forms.DataGridView();
            this.LoaderBarcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tb_EQBarcode = new System.Windows.Forms.TextBox();
            this.lb_EQ_Barcode = new System.Windows.Forms.Label();
            this.inpinjRFID1 = new HirataMainControl.inpinjRFID();
            this.userUnloader1 = new HirataMainControl.UserUnloader();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.RFID2 = new HirataMainControl.OMRON_RFID();
            this.RFID1 = new HirataMainControl.OMRON_RFID();
            this.P2 = new HirataMainControl.HCT_LoadPort();
            this.P1 = new HirataMainControl.HCT_LoadPort();
            this.OCR1 = new HirataMainControl.IO_OCR();
            this.Robot1 = new HirataMainControl.Robot1();
            this.Aligner1 = new HirataMainControl.HCT_Aligner();
            this.gbxButton = new System.Windows.Forms.GroupBox();
            this.pnlButton = new System.Windows.Forms.Panel();
            this.tctlEFEM = new System.Windows.Forms.TabControl();
            this.tapgAdam = new System.Windows.Forms.TabPage();
            this.User_Adam6050_3 = new HirataMainControl.IO_Adam6000();
            this.User_Adam6050_2 = new HirataMainControl.IO_Adam6000();
            this.User_Adam6050_1 = new HirataMainControl.IO_Adam6000();
            this.tapgEFEM.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_BarcodeList)).BeginInit();
            this.gbxButton.SuspendLayout();
            this.tctlEFEM.SuspendLayout();
            this.tapgAdam.SuspendLayout();
            this.SuspendLayout();
            // 
            // tapgEFEM
            // 
            this.tapgEFEM.BackColor = System.Drawing.SystemColors.Control;
            this.tapgEFEM.Controls.Add(this.dgv_BarcodeList);
            this.tapgEFEM.Controls.Add(this.tb_EQBarcode);
            this.tapgEFEM.Controls.Add(this.lb_EQ_Barcode);
            this.tapgEFEM.Controls.Add(this.inpinjRFID1);
            this.tapgEFEM.Controls.Add(this.userUnloader1);
            this.tapgEFEM.Controls.Add(this.button2);
            this.tapgEFEM.Controls.Add(this.button1);
            this.tapgEFEM.Controls.Add(this.RFID2);
            this.tapgEFEM.Controls.Add(this.RFID1);
            this.tapgEFEM.Controls.Add(this.P2);
            this.tapgEFEM.Controls.Add(this.P1);
            this.tapgEFEM.Controls.Add(this.OCR1);
            this.tapgEFEM.Controls.Add(this.Robot1);
            this.tapgEFEM.Controls.Add(this.Aligner1);
            this.tapgEFEM.Controls.Add(this.gbxButton);
            this.tapgEFEM.Location = new System.Drawing.Point(4, 5);
            this.tapgEFEM.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tapgEFEM.Name = "tapgEFEM";
            this.tapgEFEM.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tapgEFEM.Size = new System.Drawing.Size(2292, 941);
            this.tapgEFEM.TabIndex = 0;
            this.tapgEFEM.Text = "EFEM";
            // 
            // dgv_BarcodeList
            // 
            this.dgv_BarcodeList.AllowUserToAddRows = false;
            this.dgv_BarcodeList.AllowUserToDeleteRows = false;
            this.dgv_BarcodeList.AllowUserToResizeColumns = false;
            this.dgv_BarcodeList.AllowUserToResizeRows = false;
            this.dgv_BarcodeList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgv_BarcodeList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgv_BarcodeList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_BarcodeList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LoaderBarcode});
            this.dgv_BarcodeList.Location = new System.Drawing.Point(320, 641);
            this.dgv_BarcodeList.Name = "dgv_BarcodeList";
            this.dgv_BarcodeList.RowHeadersVisible = false;
            this.dgv_BarcodeList.RowTemplate.Height = 24;
            this.dgv_BarcodeList.Size = new System.Drawing.Size(410, 293);
            this.dgv_BarcodeList.TabIndex = 104;
            // 
            // LoaderBarcode
            // 
            this.LoaderBarcode.HeaderText = "LoaderBarcode";
            this.LoaderBarcode.Name = "LoaderBarcode";
            this.LoaderBarcode.ReadOnly = true;
            this.LoaderBarcode.Width = 123;
            // 
            // tb_EQBarcode
            // 
            this.tb_EQBarcode.Location = new System.Drawing.Point(744, 437);
            this.tb_EQBarcode.Name = "tb_EQBarcode";
            this.tb_EQBarcode.ReadOnly = true;
            this.tb_EQBarcode.Size = new System.Drawing.Size(177, 23);
            this.tb_EQBarcode.TabIndex = 103;
            // 
            // lb_EQ_Barcode
            // 
            this.lb_EQ_Barcode.AutoSize = true;
            this.lb_EQ_Barcode.Location = new System.Drawing.Point(639, 437);
            this.lb_EQ_Barcode.Name = "lb_EQ_Barcode";
            this.lb_EQ_Barcode.Size = new System.Drawing.Size(98, 15);
            this.lb_EQ_Barcode.TabIndex = 102;
            this.lb_EQ_Barcode.Text = "EQ_Barcode : ";
            // 
            // inpinjRFID1
            // 
            this.inpinjRFID1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.inpinjRFID1.Location = new System.Drawing.Point(1025, 399);
            this.inpinjRFID1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.inpinjRFID1.Name = "inpinjRFID1";
            this.inpinjRFID1.Size = new System.Drawing.Size(494, 103);
            this.inpinjRFID1.TabIndex = 88;
            // 
            // userUnloader1
            // 
            this.userUnloader1.Font = new System.Drawing.Font("微軟正黑體", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.userUnloader1.Location = new System.Drawing.Point(993, 296);
            this.userUnloader1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.userUnloader1.Name = "userUnloader1";
            this.userUnloader1.Size = new System.Drawing.Size(526, 107);
            this.userUnloader1.TabIndex = 87;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(9, 339);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 86;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(9, 300);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 85;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // RFID2
            // 
            this.RFID2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.RFID2.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RFID2.Location = new System.Drawing.Point(656, 7);
            this.RFID2.Name = "RFID2";
            this.RFID2.Size = new System.Drawing.Size(280, 50);
            this.RFID2.TabIndex = 84;
            this.RFID2.Ui_Busy = false;
            this.RFID2.Ui_Connect = false;
            this.RFID2.Ui_FoupID = null;
            this.RFID2.Visible = false;
            // 
            // RFID1
            // 
            this.RFID1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.RFID1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RFID1.Location = new System.Drawing.Point(320, 7);
            this.RFID1.Name = "RFID1";
            this.RFID1.Size = new System.Drawing.Size(280, 50);
            this.RFID1.TabIndex = 83;
            this.RFID1.Ui_Busy = false;
            this.RFID1.Ui_Connect = false;
            this.RFID1.Ui_FoupID = null;
            this.RFID1.Visible = false;
            // 
            // P2
            // 
            this.P2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.P2.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.P2.Location = new System.Drawing.Point(656, 63);
            this.P2.Name = "P2";
            this.P2.Size = new System.Drawing.Size(330, 340);
            this.P2.TabIndex = 82;
            this.P2.Ui_Alarm = null;
            this.P2.Ui_Busy = false;
            this.P2.Ui_Connect = false;
            this.P2.Ui_FoupPresent = HirataMainControl.LPFoupMount.Absent;
            this.P2.Ui_Interlock = null;
            this.P2.Ui_LoadStatus = HirataMainControl.LPPosition.InProcess;
            this.P2.Ui_Mode = false;
            this.P2.Ui_NowGap = 0D;
            this.P2.Ui_Type = HirataMainControl.LPType.Type1;
            // 
            // P1
            // 
            this.P1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.P1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.P1.Location = new System.Drawing.Point(320, 63);
            this.P1.Name = "P1";
            this.P1.Size = new System.Drawing.Size(330, 340);
            this.P1.TabIndex = 81;
            this.P1.Ui_Alarm = null;
            this.P1.Ui_Busy = false;
            this.P1.Ui_Connect = false;
            this.P1.Ui_FoupPresent = HirataMainControl.LPFoupMount.Absent;
            this.P1.Ui_Interlock = null;
            this.P1.Ui_LoadStatus = HirataMainControl.LPPosition.InProcess;
            this.P1.Ui_Mode = false;
            this.P1.Ui_NowGap = 0D;
            this.P1.Ui_Type = HirataMainControl.LPType.Type1;
            // 
            // OCR1
            // 
            this.OCR1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OCR1.Location = new System.Drawing.Point(992, 7);
            this.OCR1.Name = "OCR1";
            this.OCR1.Size = new System.Drawing.Size(230, 60);
            this.OCR1.TabIndex = 39;
            this.OCR1.Ui_Busy = false;
            this.OCR1.Ui_Connect = false;
            this.OCR1.Ui_ID = "";
            // 
            // Robot1
            // 
            this.Robot1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Robot1.BackgroundImage")));
            this.Robot1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Robot1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.Robot1.Location = new System.Drawing.Point(6, 541);
            this.Robot1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Robot1.Name = "Robot1";
            this.Robot1.Size = new System.Drawing.Size(2280, 100);
            this.Robot1.TabIndex = 21;
            this.Robot1.Ui_ArmStatusR = HirataMainControl.ArmStatus.Arm_Home;
            this.Robot1.Ui_ArmStatusX = HirataMainControl.ArmStatus.Arm_Home;
            this.Robot1.Ui_ArmStatusY = HirataMainControl.ArmStatus.Arm_Home;
            this.Robot1.Ui_Busy = false;
            this.Robot1.Ui_Connect = false;
            this.Robot1.Ui_LowerWaferInfo = null;
            this.Robot1.Ui_LowerWaferPresent = HirataMainControl.WaferStatus.Unknown;
            this.Robot1.Ui_Remote = false;
            this.Robot1.Ui_RobotPos = HirataMainControl.RobotPosition.Home;
            this.Robot1.Ui_Status = null;
            this.Robot1.Ui_UpperWaferInfo = null;
            this.Robot1.Ui_UpperWaferPresent = HirataMainControl.WaferStatus.Unknown;
            // 
            // Aligner1
            // 
            this.Aligner1.BackColor = System.Drawing.SystemColors.Control;
            this.Aligner1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Aligner1.Location = new System.Drawing.Point(992, 136);
            this.Aligner1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Aligner1.Name = "Aligner1";
            this.Aligner1.Size = new System.Drawing.Size(400, 145);
            this.Aligner1.TabIndex = 12;
            this.Aligner1.Ui_Alarm = "Error";
            this.Aligner1.Ui_Busy = false;
            this.Aligner1.Ui_Connect = false;
            this.Aligner1.Ui_LiftPin = HirataMainControl.LiftPinEnum.Unknown;
            this.Aligner1.Ui_Mode = null;
            this.Aligner1.Ui_NotchAngle = "0";
            this.Aligner1.Ui_Presence = HirataMainControl.WaferStatus.WithOut;
            this.Aligner1.Ui_Status = HirataMainControl.AlignerStatus.Unknown;
            this.Aligner1.Ui_ToAngle = "0";
            this.Aligner1.Ui_Type = null;
            this.Aligner1.Ui_Vac = false;
            this.Aligner1.Ui_WaferInfo = "";
            this.Aligner1.Ui_WaferProtrude = false;
            this.Aligner1.Visible = false;
            // 
            // gbxButton
            // 
            this.gbxButton.Controls.Add(this.pnlButton);
            this.gbxButton.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxButton.ForeColor = System.Drawing.Color.Black;
            this.gbxButton.Location = new System.Drawing.Point(6, 4);
            this.gbxButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbxButton.Name = "gbxButton";
            this.gbxButton.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbxButton.Size = new System.Drawing.Size(116, 277);
            this.gbxButton.TabIndex = 3;
            this.gbxButton.TabStop = false;
            this.gbxButton.Text = "Command";
            // 
            // pnlButton
            // 
            this.pnlButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlButton.Location = new System.Drawing.Point(3, 23);
            this.pnlButton.Name = "pnlButton";
            this.pnlButton.Size = new System.Drawing.Size(110, 250);
            this.pnlButton.TabIndex = 25;
            // 
            // tctlEFEM
            // 
            this.tctlEFEM.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tctlEFEM.Controls.Add(this.tapgEFEM);
            this.tctlEFEM.Controls.Add(this.tapgAdam);
            this.tctlEFEM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tctlEFEM.ItemSize = new System.Drawing.Size(0, 1);
            this.tctlEFEM.Location = new System.Drawing.Point(0, 0);
            this.tctlEFEM.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tctlEFEM.Name = "tctlEFEM";
            this.tctlEFEM.SelectedIndex = 0;
            this.tctlEFEM.Size = new System.Drawing.Size(2300, 950);
            this.tctlEFEM.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tctlEFEM.TabIndex = 15;
            // 
            // tapgAdam
            // 
            this.tapgAdam.BackColor = System.Drawing.SystemColors.Control;
            this.tapgAdam.Controls.Add(this.User_Adam6050_3);
            this.tapgAdam.Controls.Add(this.User_Adam6050_2);
            this.tapgAdam.Controls.Add(this.User_Adam6050_1);
            this.tapgAdam.Location = new System.Drawing.Point(4, 5);
            this.tapgAdam.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tapgAdam.Name = "tapgAdam";
            this.tapgAdam.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tapgAdam.Size = new System.Drawing.Size(2292, 941);
            this.tapgAdam.TabIndex = 1;
            this.tapgAdam.Text = "Adam";
            // 
            // User_Adam6050_3
            // 
            this.User_Adam6050_3.BackColor = System.Drawing.SystemColors.Control;
            this.User_Adam6050_3.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.User_Adam6050_3.Location = new System.Drawing.Point(858, 8);
            this.User_Adam6050_3.Name = "User_Adam6050_3";
            this.User_Adam6050_3.Size = new System.Drawing.Size(420, 937);
            this.User_Adam6050_3.TabIndex = 2;
            this.User_Adam6050_3.Ui_Connect = false;
            // 
            // User_Adam6050_2
            // 
            this.User_Adam6050_2.BackColor = System.Drawing.SystemColors.Control;
            this.User_Adam6050_2.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.User_Adam6050_2.Location = new System.Drawing.Point(432, 8);
            this.User_Adam6050_2.Name = "User_Adam6050_2";
            this.User_Adam6050_2.Size = new System.Drawing.Size(420, 937);
            this.User_Adam6050_2.TabIndex = 1;
            this.User_Adam6050_2.Ui_Connect = false;
            // 
            // User_Adam6050_1
            // 
            this.User_Adam6050_1.BackColor = System.Drawing.SystemColors.Control;
            this.User_Adam6050_1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.User_Adam6050_1.Location = new System.Drawing.Point(6, 8);
            this.User_Adam6050_1.Name = "User_Adam6050_1";
            this.User_Adam6050_1.Size = new System.Drawing.Size(420, 937);
            this.User_Adam6050_1.TabIndex = 0;
            this.User_Adam6050_1.Ui_Connect = false;
            // 
            // HCT_EFEM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tctlEFEM);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "HCT_EFEM";
            this.Size = new System.Drawing.Size(2300, 950);
            this.tapgEFEM.ResumeLayout(false);
            this.tapgEFEM.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_BarcodeList)).EndInit();
            this.gbxButton.ResumeLayout(false);
            this.tctlEFEM.ResumeLayout(false);
            this.tapgAdam.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tapgEFEM;
        private IO_OCR OCR1;
        private Robot1 Robot1;
        private HCT_Aligner Aligner1;
        private System.Windows.Forms.GroupBox gbxButton;
        private System.Windows.Forms.Panel pnlButton;
        private System.Windows.Forms.TabControl tctlEFEM;
        private System.Windows.Forms.TabPage tapgAdam;
        private HCT_LoadPort P1;
        private HCT_LoadPort P2;
        private OMRON_RFID RFID2;
        private OMRON_RFID RFID1;
        private IO_Adam6000 User_Adam6050_1;
        private IO_Adam6000 User_Adam6050_3;
        private IO_Adam6000 User_Adam6050_2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private UserUnloader userUnloader1;
        private HirataMainControl.inpinjRFID inpinjRFID1;
        private System.Windows.Forms.TextBox tb_EQBarcode;
        private System.Windows.Forms.Label lb_EQ_Barcode;
        private System.Windows.Forms.DataGridView dgv_BarcodeList;
        private System.Windows.Forms.DataGridViewTextBoxColumn LoaderBarcode;
    }
}
