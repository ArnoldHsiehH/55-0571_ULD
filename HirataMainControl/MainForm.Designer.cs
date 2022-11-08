namespace HirataMainControl
{
    partial class frmMain
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

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.splTopStatus = new System.Windows.Forms.SplitContainer();
            this.labEFEMMode = new System.Windows.Forms.Label();
            this.labSECS = new System.Windows.Forms.Label();
            this.labPSW = new System.Windows.Forms.Label();
            this.gbxWaferInfo = new System.Windows.Forms.GroupBox();
            this.labUnknow = new System.Windows.Forms.Label();
            this.labUnknownWafer = new System.Windows.Forms.Label();
            this.labHaveWafer = new System.Windows.Forms.Label();
            this.labNoWafer = new System.Windows.Forms.Label();
            this.picFan = new System.Windows.Forms.PictureBox();
            this.picAccount = new System.Windows.Forms.PictureBox();
            this.labAccount = new System.Windows.Forms.Label();
            this.labMode = new System.Windows.Forms.Label();
            this.labEFEMStatut = new System.Windows.Forms.Label();
            this.labStatusTitle = new System.Windows.Forms.Label();
            this.rtfLog = new System.Windows.Forms.RichTextBox();
            this.tmrMain = new System.Windows.Forms.Timer(this.components);
            this.icnMain = new System.Windows.Forms.NotifyIcon(this.components);
            this.dgvRecipe = new System.Windows.Forms.DataGridView();
            this.btnRecipeDelete = new System.Windows.Forms.Button();
            this.txtRecipe = new System.Windows.Forms.TextBox();
            this.btnRecipeAdd = new System.Windows.Forms.Button();
            this.btnRecipeSetting = new System.Windows.Forms.Button();
            this.labRule = new System.Windows.Forms.Label();
            this.cboEQSetting = new System.Windows.Forms.ComboBox();
            this.btnEQSetting = new System.Windows.Forms.Button();
            this.txtMax = new System.Windows.Forms.TextBox();
            this.txtMin = new System.Windows.Forms.TextBox();
            this.labEQmax = new System.Windows.Forms.Label();
            this.labmid = new System.Windows.Forms.Label();
            this.labEQmin = new System.Windows.Forms.Label();
            this.labEQLP = new System.Windows.Forms.Label();
            this.dgvEQSetting = new System.Windows.Forms.DataGridView();
            this.cboLoadPortType = new System.Windows.Forms.ComboBox();
            this.cboLoadPortSetting = new System.Windows.Forms.ComboBox();
            this.splMain = new System.Windows.Forms.SplitContainer();
            this.splTop = new System.Windows.Forms.SplitContainer();
            this.splTopTitle = new System.Windows.Forms.SplitContainer();
            this.picLog = new System.Windows.Forms.PictureBox();
            this.labPLCversion = new System.Windows.Forms.Label();
            this.labPCversion = new System.Windows.Forms.Label();
            this.labTime = new System.Windows.Forms.Label();
            this.tctlMain = new System.Windows.Forms.TabControl();
            this.tapgAccount = new System.Windows.Forms.TabPage();
            this.tctlAccount = new System.Windows.Forms.TabControl();
            this.tapgGuest = new System.Windows.Forms.TabPage();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.labVersion = new System.Windows.Forms.Label();
            this.gbxAccount = new System.Windows.Forms.GroupBox();
            this.btnLogIn = new System.Windows.Forms.Button();
            this.labInPassword = new System.Windows.Forms.Label();
            this.labInName = new System.Windows.Forms.Label();
            this.btnLogOut = new System.Windows.Forms.Button();
            this.txtInPassword = new System.Windows.Forms.TextBox();
            this.txtInUser = new System.Windows.Forms.TextBox();
            this.tapgAdmin = new System.Windows.Forms.TabPage();
            this.gbxAuthEdit = new System.Windows.Forms.GroupBox();
            this.dgvAuth = new System.Windows.Forms.DataGridView();
            this.btnAuthEdit = new System.Windows.Forms.Button();
            this.btnAuthSearch = new System.Windows.Forms.Button();
            this.gbxAccountDel = new System.Windows.Forms.GroupBox();
            this.dgvAccount = new System.Windows.Forms.DataGridView();
            this.btnAccountDel = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.gbxAccountAdd = new System.Windows.Forms.GroupBox();
            this.labAddAuthority = new System.Windows.Forms.Label();
            this.labAddPassword = new System.Windows.Forms.Label();
            this.labAddName = new System.Windows.Forms.Label();
            this.txtAddAuthority = new System.Windows.Forms.ComboBox();
            this.btnAccountAdd = new System.Windows.Forms.Button();
            this.txtAddPassword = new System.Windows.Forms.TextBox();
            this.txtAddName = new System.Windows.Forms.TextBox();
            this.tapgMonitor = new System.Windows.Forms.TabPage();
            this.User_EFEM = new HirataMainControl.HCT_EFEM();
            this.tapgLog = new System.Windows.Forms.TabPage();
            this.splLog = new System.Windows.Forms.SplitContainer();
            this.splLogTime = new System.Windows.Forms.SplitContainer();
            this.cboDirectory = new System.Windows.Forms.ComboBox();
            this.btnLogCopy = new System.Windows.Forms.Button();
            this.btnLogSearch = new System.Windows.Forms.Button();
            this.dtpLogStart = new System.Windows.Forms.DateTimePicker();
            this.dtpLogEnd = new System.Windows.Forms.DateTimePicker();
            this.labLogTo = new System.Windows.Forms.Label();
            this.trvLog = new System.Windows.Forms.TreeView();
            this.User_Log = new HirataMainControl.LogLib();
            this.tapgProcess = new System.Windows.Forms.TabPage();
            this.User_Core = new HirataMainControl.UserCore();
            this.tapgCycle = new System.Windows.Forms.TabPage();
            this.tapgJpSetting = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rb_hand = new System.Windows.Forms.RadioButton();
            this.rb_rf = new System.Windows.Forms.RadioButton();
            this.gbxMemoryWafer = new System.Windows.Forms.GroupBox();
            this.btnUpdateStatus = new System.Windows.Forms.Button();
            this.dgvWaferStatus = new System.Windows.Forms.DataGridView();
            this.Device = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Memory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Sensor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbxPortSetting = new System.Windows.Forms.GroupBox();
            this.splitPortSetting = new System.Windows.Forms.SplitContainer();
            this.dgvLoadPortType = new System.Windows.Forms.DataGridView();
            this.LoadPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PortType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnOMSOut = new System.Windows.Forms.Button();
            this.btnOMSIn = new System.Windows.Forms.Button();
            this.btnDummyWafer = new System.Windows.Forms.Button();
            this.btnRealwafer = new System.Windows.Forms.Button();
            this.gbxRecipeSetting = new System.Windows.Forms.GroupBox();
            this.clsRobotSetting = new System.Windows.Forms.CheckedListBox();
            this.txtOCRDegree = new System.Windows.Forms.TextBox();
            this.labOCRDegree = new System.Windows.Forms.Label();
            this.txtAlignerDegree = new System.Windows.Forms.TextBox();
            this.labAlignerDegree = new System.Windows.Forms.Label();
            this.clsParameter = new System.Windows.Forms.CheckedListBox();
            this.cboModeSetting = new System.Windows.Forms.ComboBox();
            this.tapgProcessHistory = new System.Windows.Forms.TabPage();
            this.splHistory = new System.Windows.Forms.SplitContainer();
            this.gbxWaferSearch = new System.Windows.Forms.GroupBox();
            this.txtHistoryCJ = new System.Windows.Forms.TextBox();
            this.labWaferID = new System.Windows.Forms.Label();
            this.labCarrierID = new System.Windows.Forms.Label();
            this.txtHistoryWafer = new System.Windows.Forms.TextBox();
            this.txtHistoryCarrier = new System.Windows.Forms.TextBox();
            this.labHistoryCJID = new System.Windows.Forms.Label();
            this.dtpHistoryEnd = new System.Windows.Forms.DateTimePicker();
            this.btnHistorySearch = new System.Windows.Forms.Button();
            this.txtHistoryMagazine = new System.Windows.Forms.TextBox();
            this.dtpHistoryStart = new System.Windows.Forms.DateTimePicker();
            this.txtHistoryCassette = new System.Windows.Forms.TextBox();
            this.labTimeEnd = new System.Windows.Forms.Label();
            this.labMagazineID = new System.Windows.Forms.Label();
            this.labTimeStart = new System.Windows.Forms.Label();
            this.labCassetteID = new System.Windows.Forms.Label();
            this.dgvHistoryWafer = new System.Windows.Forms.DataGridView();
            this.tapgPLC = new System.Windows.Forms.TabPage();
            this.tapgSecsView = new System.Windows.Forms.TabPage();
            this.splButton = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splTopStatus)).BeginInit();
            this.splTopStatus.Panel1.SuspendLayout();
            this.splTopStatus.Panel2.SuspendLayout();
            this.splTopStatus.SuspendLayout();
            this.gbxWaferInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAccount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecipe)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEQSetting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splMain)).BeginInit();
            this.splMain.Panel1.SuspendLayout();
            this.splMain.Panel2.SuspendLayout();
            this.splMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splTop)).BeginInit();
            this.splTop.Panel1.SuspendLayout();
            this.splTop.Panel2.SuspendLayout();
            this.splTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splTopTitle)).BeginInit();
            this.splTopTitle.Panel1.SuspendLayout();
            this.splTopTitle.Panel2.SuspendLayout();
            this.splTopTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLog)).BeginInit();
            this.tctlMain.SuspendLayout();
            this.tapgAccount.SuspendLayout();
            this.tctlAccount.SuspendLayout();
            this.tapgGuest.SuspendLayout();
            this.gbxAccount.SuspendLayout();
            this.tapgAdmin.SuspendLayout();
            this.gbxAuthEdit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAuth)).BeginInit();
            this.gbxAccountDel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccount)).BeginInit();
            this.gbxAccountAdd.SuspendLayout();
            this.tapgMonitor.SuspendLayout();
            this.tapgLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splLog)).BeginInit();
            this.splLog.Panel1.SuspendLayout();
            this.splLog.Panel2.SuspendLayout();
            this.splLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splLogTime)).BeginInit();
            this.splLogTime.Panel1.SuspendLayout();
            this.splLogTime.Panel2.SuspendLayout();
            this.splLogTime.SuspendLayout();
            this.tapgProcess.SuspendLayout();
            this.tapgJpSetting.SuspendLayout();
            this.panel1.SuspendLayout();
            this.gbxMemoryWafer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWaferStatus)).BeginInit();
            this.gbxPortSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitPortSetting)).BeginInit();
            this.splitPortSetting.Panel1.SuspendLayout();
            this.splitPortSetting.Panel2.SuspendLayout();
            this.splitPortSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLoadPortType)).BeginInit();
            this.gbxRecipeSetting.SuspendLayout();
            this.tapgProcessHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splHistory)).BeginInit();
            this.splHistory.Panel1.SuspendLayout();
            this.splHistory.Panel2.SuspendLayout();
            this.splHistory.SuspendLayout();
            this.gbxWaferSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistoryWafer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splButton)).BeginInit();
            this.splButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // splTopStatus
            // 
            this.splTopStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(103)))), ((int)(((byte)(214)))));
            resources.ApplyResources(this.splTopStatus, "splTopStatus");
            this.splTopStatus.Name = "splTopStatus";
            // 
            // splTopStatus.Panel1
            // 
            this.splTopStatus.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(103)))), ((int)(((byte)(214)))));
            this.splTopStatus.Panel1.Controls.Add(this.labEFEMMode);
            this.splTopStatus.Panel1.Controls.Add(this.labSECS);
            this.splTopStatus.Panel1.Controls.Add(this.labPSW);
            this.splTopStatus.Panel1.Controls.Add(this.gbxWaferInfo);
            this.splTopStatus.Panel1.Controls.Add(this.picFan);
            this.splTopStatus.Panel1.Controls.Add(this.picAccount);
            this.splTopStatus.Panel1.Controls.Add(this.labAccount);
            this.splTopStatus.Panel1.Controls.Add(this.labMode);
            this.splTopStatus.Panel1.Controls.Add(this.labEFEMStatut);
            this.splTopStatus.Panel1.Controls.Add(this.labStatusTitle);
            // 
            // splTopStatus.Panel2
            // 
            this.splTopStatus.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(103)))), ((int)(((byte)(214)))));
            this.splTopStatus.Panel2.Controls.Add(this.rtfLog);
            resources.ApplyResources(this.splTopStatus.Panel2, "splTopStatus.Panel2");
            // 
            // labEFEMMode
            // 
            this.labEFEMMode.BackColor = System.Drawing.Color.Yellow;
            resources.ApplyResources(this.labEFEMMode, "labEFEMMode");
            this.labEFEMMode.Name = "labEFEMMode";
            // 
            // labSECS
            // 
            this.labSECS.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.labSECS, "labSECS");
            this.labSECS.Name = "labSECS";
            // 
            // labPSW
            // 
            this.labPSW.BackColor = System.Drawing.SystemColors.WindowText;
            this.labPSW.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.labPSW, "labPSW");
            this.labPSW.ForeColor = System.Drawing.Color.Yellow;
            this.labPSW.Name = "labPSW";
            // 
            // gbxWaferInfo
            // 
            this.gbxWaferInfo.Controls.Add(this.labUnknow);
            this.gbxWaferInfo.Controls.Add(this.labUnknownWafer);
            this.gbxWaferInfo.Controls.Add(this.labHaveWafer);
            this.gbxWaferInfo.Controls.Add(this.labNoWafer);
            resources.ApplyResources(this.gbxWaferInfo, "gbxWaferInfo");
            this.gbxWaferInfo.ForeColor = System.Drawing.Color.White;
            this.gbxWaferInfo.Name = "gbxWaferInfo";
            this.gbxWaferInfo.TabStop = false;
            // 
            // labUnknow
            // 
            this.labUnknow.BackColor = System.Drawing.Color.Red;
            this.labUnknow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labUnknow.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.labUnknow, "labUnknow");
            this.labUnknow.Name = "labUnknow";
            // 
            // labUnknownWafer
            // 
            this.labUnknownWafer.BackColor = System.Drawing.Color.Gray;
            this.labUnknownWafer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labUnknownWafer.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.labUnknownWafer, "labUnknownWafer");
            this.labUnknownWafer.Name = "labUnknownWafer";
            // 
            // labHaveWafer
            // 
            this.labHaveWafer.BackColor = System.Drawing.Color.LightGreen;
            this.labHaveWafer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labHaveWafer.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.labHaveWafer, "labHaveWafer");
            this.labHaveWafer.Name = "labHaveWafer";
            // 
            // labNoWafer
            // 
            this.labNoWafer.BackColor = System.Drawing.Color.WhiteSmoke;
            this.labNoWafer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labNoWafer.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.labNoWafer, "labNoWafer");
            this.labNoWafer.Name = "labNoWafer";
            // 
            // picFan
            // 
            this.picFan.Image = global::HirataMainControl.Properties.Resources.Fan;
            resources.ApplyResources(this.picFan, "picFan");
            this.picFan.Name = "picFan";
            this.picFan.TabStop = false;
            // 
            // picAccount
            // 
            this.picAccount.Image = global::HirataMainControl.Properties.Resources.User;
            resources.ApplyResources(this.picAccount, "picAccount");
            this.picAccount.Name = "picAccount";
            this.picAccount.TabStop = false;
            // 
            // labAccount
            // 
            this.labAccount.BackColor = System.Drawing.SystemColors.WindowText;
            this.labAccount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.labAccount, "labAccount");
            this.labAccount.ForeColor = System.Drawing.Color.Yellow;
            this.labAccount.Name = "labAccount";
            // 
            // labMode
            // 
            resources.ApplyResources(this.labMode, "labMode");
            this.labMode.ForeColor = System.Drawing.Color.White;
            this.labMode.Name = "labMode";
            // 
            // labEFEMStatut
            // 
            this.labEFEMStatut.BackColor = System.Drawing.Color.Yellow;
            resources.ApplyResources(this.labEFEMStatut, "labEFEMStatut");
            this.labEFEMStatut.Name = "labEFEMStatut";
            // 
            // labStatusTitle
            // 
            resources.ApplyResources(this.labStatusTitle, "labStatusTitle");
            this.labStatusTitle.ForeColor = System.Drawing.Color.White;
            this.labStatusTitle.Name = "labStatusTitle";
            // 
            // rtfLog
            // 
            this.rtfLog.BackColor = System.Drawing.SystemColors.Window;
            this.rtfLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.rtfLog, "rtfLog");
            this.rtfLog.Name = "rtfLog";
            this.rtfLog.ReadOnly = true;
            this.rtfLog.TabStop = false;
            // 
            // tmrMain
            // 
            this.tmrMain.Interval = 1000;
            this.tmrMain.Tick += new System.EventHandler(this.timeMain_Tick);
            // 
            // icnMain
            // 
            resources.ApplyResources(this.icnMain, "icnMain");
            this.icnMain.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.icnMain_MouseDoubleClick);
            // 
            // dgvRecipe
            // 
            resources.ApplyResources(this.dgvRecipe, "dgvRecipe");
            this.dgvRecipe.Name = "dgvRecipe";
            // 
            // btnRecipeDelete
            // 
            resources.ApplyResources(this.btnRecipeDelete, "btnRecipeDelete");
            this.btnRecipeDelete.Name = "btnRecipeDelete";
            // 
            // txtRecipe
            // 
            resources.ApplyResources(this.txtRecipe, "txtRecipe");
            this.txtRecipe.Name = "txtRecipe";
            // 
            // btnRecipeAdd
            // 
            resources.ApplyResources(this.btnRecipeAdd, "btnRecipeAdd");
            this.btnRecipeAdd.Name = "btnRecipeAdd";
            // 
            // btnRecipeSetting
            // 
            resources.ApplyResources(this.btnRecipeSetting, "btnRecipeSetting");
            this.btnRecipeSetting.Name = "btnRecipeSetting";
            // 
            // labRule
            // 
            resources.ApplyResources(this.labRule, "labRule");
            this.labRule.Name = "labRule";
            // 
            // cboEQSetting
            // 
            resources.ApplyResources(this.cboEQSetting, "cboEQSetting");
            this.cboEQSetting.Name = "cboEQSetting";
            // 
            // btnEQSetting
            // 
            resources.ApplyResources(this.btnEQSetting, "btnEQSetting");
            this.btnEQSetting.Name = "btnEQSetting";
            // 
            // txtMax
            // 
            resources.ApplyResources(this.txtMax, "txtMax");
            this.txtMax.Name = "txtMax";
            // 
            // txtMin
            // 
            resources.ApplyResources(this.txtMin, "txtMin");
            this.txtMin.Name = "txtMin";
            // 
            // labEQmax
            // 
            resources.ApplyResources(this.labEQmax, "labEQmax");
            this.labEQmax.Name = "labEQmax";
            // 
            // labmid
            // 
            resources.ApplyResources(this.labmid, "labmid");
            this.labmid.Name = "labmid";
            // 
            // labEQmin
            // 
            resources.ApplyResources(this.labEQmin, "labEQmin");
            this.labEQmin.Name = "labEQmin";
            // 
            // labEQLP
            // 
            resources.ApplyResources(this.labEQLP, "labEQLP");
            this.labEQLP.Name = "labEQLP";
            // 
            // dgvEQSetting
            // 
            resources.ApplyResources(this.dgvEQSetting, "dgvEQSetting");
            this.dgvEQSetting.Name = "dgvEQSetting";
            // 
            // cboLoadPortType
            // 
            resources.ApplyResources(this.cboLoadPortType, "cboLoadPortType");
            this.cboLoadPortType.Name = "cboLoadPortType";
            // 
            // cboLoadPortSetting
            // 
            resources.ApplyResources(this.cboLoadPortSetting, "cboLoadPortSetting");
            this.cboLoadPortSetting.Name = "cboLoadPortSetting";
            // 
            // splMain
            // 
            this.splMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            resources.ApplyResources(this.splMain, "splMain");
            this.splMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splMain.Name = "splMain";
            // 
            // splMain.Panel1
            // 
            this.splMain.Panel1.Controls.Add(this.splTop);
            // 
            // splMain.Panel2
            // 
            this.splMain.Panel2.Controls.Add(this.tctlMain);
            this.splMain.Panel2.Controls.Add(this.splButton);
            // 
            // splTop
            // 
            this.splTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(103)))), ((int)(((byte)(214)))));
            resources.ApplyResources(this.splTop, "splTop");
            this.splTop.Name = "splTop";
            // 
            // splTop.Panel1
            // 
            this.splTop.Panel1.Controls.Add(this.splTopTitle);
            // 
            // splTop.Panel2
            // 
            this.splTop.Panel2.Controls.Add(this.splTopStatus);
            // 
            // splTopTitle
            // 
            resources.ApplyResources(this.splTopTitle, "splTopTitle");
            this.splTopTitle.Name = "splTopTitle";
            // 
            // splTopTitle.Panel1
            // 
            this.splTopTitle.Panel1.Controls.Add(this.picLog);
            // 
            // splTopTitle.Panel2
            // 
            this.splTopTitle.Panel2.Controls.Add(this.labPLCversion);
            this.splTopTitle.Panel2.Controls.Add(this.labPCversion);
            this.splTopTitle.Panel2.Controls.Add(this.labTime);
            this.splTopTitle.TabStop = false;
            // 
            // picLog
            // 
            this.picLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(103)))), ((int)(((byte)(214)))));
            resources.ApplyResources(this.picLog, "picLog");
            this.picLog.Image = global::HirataMainControl.Properties.Resources.Log_Hirata;
            this.picLog.Name = "picLog";
            this.picLog.TabStop = false;
            // 
            // labPLCversion
            // 
            this.labPLCversion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(103)))), ((int)(((byte)(214)))));
            resources.ApplyResources(this.labPLCversion, "labPLCversion");
            this.labPLCversion.ForeColor = System.Drawing.Color.Yellow;
            this.labPLCversion.Name = "labPLCversion";
            // 
            // labPCversion
            // 
            this.labPCversion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(103)))), ((int)(((byte)(214)))));
            resources.ApplyResources(this.labPCversion, "labPCversion");
            this.labPCversion.ForeColor = System.Drawing.Color.Yellow;
            this.labPCversion.Name = "labPCversion";
            // 
            // labTime
            // 
            this.labTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(103)))), ((int)(((byte)(214)))));
            resources.ApplyResources(this.labTime, "labTime");
            this.labTime.ForeColor = System.Drawing.Color.Yellow;
            this.labTime.Name = "labTime";
            // 
            // tctlMain
            // 
            resources.ApplyResources(this.tctlMain, "tctlMain");
            this.tctlMain.Controls.Add(this.tapgAccount);
            this.tctlMain.Controls.Add(this.tapgMonitor);
            this.tctlMain.Controls.Add(this.tapgLog);
            this.tctlMain.Controls.Add(this.tapgProcess);
            this.tctlMain.Controls.Add(this.tapgCycle);
            this.tctlMain.Controls.Add(this.tapgJpSetting);
            this.tctlMain.Controls.Add(this.tapgProcessHistory);
            this.tctlMain.Controls.Add(this.tapgPLC);
            this.tctlMain.Controls.Add(this.tapgSecsView);
            this.tctlMain.Name = "tctlMain";
            this.tctlMain.SelectedIndex = 0;
            this.tctlMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            // 
            // tapgAccount
            // 
            this.tapgAccount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.tapgAccount.Controls.Add(this.tctlAccount);
            resources.ApplyResources(this.tapgAccount, "tapgAccount");
            this.tapgAccount.Name = "tapgAccount";
            // 
            // tctlAccount
            // 
            resources.ApplyResources(this.tctlAccount, "tctlAccount");
            this.tctlAccount.Controls.Add(this.tapgGuest);
            this.tctlAccount.Controls.Add(this.tapgAdmin);
            this.tctlAccount.Name = "tctlAccount";
            this.tctlAccount.SelectedIndex = 0;
            this.tctlAccount.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            // 
            // tapgGuest
            // 
            this.tapgGuest.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tapgGuest.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tapgGuest.Controls.Add(this.button6);
            this.tapgGuest.Controls.Add(this.button5);
            this.tapgGuest.Controls.Add(this.button4);
            this.tapgGuest.Controls.Add(this.button3);
            this.tapgGuest.Controls.Add(this.button2);
            this.tapgGuest.Controls.Add(this.button1);
            this.tapgGuest.Controls.Add(this.labVersion);
            this.tapgGuest.Controls.Add(this.gbxAccount);
            resources.ApplyResources(this.tapgGuest, "tapgGuest");
            this.tapgGuest.Name = "tapgGuest";
            // 
            // button6
            // 
            resources.ApplyResources(this.button6, "button6");
            this.button6.Name = "button6";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            resources.ApplyResources(this.button5, "button5");
            this.button5.Name = "button5";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            resources.ApplyResources(this.button4, "button4");
            this.button4.Name = "button4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // labVersion
            // 
            this.labVersion.BackColor = System.Drawing.Color.WhiteSmoke;
            resources.ApplyResources(this.labVersion, "labVersion");
            this.labVersion.ForeColor = System.Drawing.Color.Black;
            this.labVersion.Name = "labVersion";
            // 
            // gbxAccount
            // 
            this.gbxAccount.Controls.Add(this.btnLogIn);
            this.gbxAccount.Controls.Add(this.labInPassword);
            this.gbxAccount.Controls.Add(this.labInName);
            this.gbxAccount.Controls.Add(this.btnLogOut);
            this.gbxAccount.Controls.Add(this.txtInPassword);
            this.gbxAccount.Controls.Add(this.txtInUser);
            resources.ApplyResources(this.gbxAccount, "gbxAccount");
            this.gbxAccount.Name = "gbxAccount";
            this.gbxAccount.TabStop = false;
            // 
            // btnLogIn
            // 
            resources.ApplyResources(this.btnLogIn, "btnLogIn");
            this.btnLogIn.Image = global::HirataMainControl.Properties.Resources.AccountIn;
            this.btnLogIn.Name = "btnLogIn";
            this.btnLogIn.UseVisualStyleBackColor = true;
            this.btnLogIn.Click += new System.EventHandler(this.btnLogIn_Click);
            // 
            // labInPassword
            // 
            resources.ApplyResources(this.labInPassword, "labInPassword");
            this.labInPassword.Name = "labInPassword";
            // 
            // labInName
            // 
            resources.ApplyResources(this.labInName, "labInName");
            this.labInName.Name = "labInName";
            // 
            // btnLogOut
            // 
            resources.ApplyResources(this.btnLogOut, "btnLogOut");
            this.btnLogOut.Image = global::HirataMainControl.Properties.Resources.AccountOut;
            this.btnLogOut.Name = "btnLogOut";
            this.btnLogOut.UseVisualStyleBackColor = true;
            this.btnLogOut.Click += new System.EventHandler(this.btnLogOut_Click);
            // 
            // txtInPassword
            // 
            this.txtInPassword.AcceptsTab = true;
            resources.ApplyResources(this.txtInPassword, "txtInPassword");
            this.txtInPassword.Name = "txtInPassword";
            // 
            // txtInUser
            // 
            this.txtInUser.AcceptsTab = true;
            resources.ApplyResources(this.txtInUser, "txtInUser");
            this.txtInUser.Name = "txtInUser";
            // 
            // tapgAdmin
            // 
            this.tapgAdmin.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tapgAdmin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tapgAdmin.Controls.Add(this.gbxAuthEdit);
            this.tapgAdmin.Controls.Add(this.gbxAccountDel);
            this.tapgAdmin.Controls.Add(this.gbxAccountAdd);
            resources.ApplyResources(this.tapgAdmin, "tapgAdmin");
            this.tapgAdmin.Name = "tapgAdmin";
            // 
            // gbxAuthEdit
            // 
            this.gbxAuthEdit.Controls.Add(this.dgvAuth);
            this.gbxAuthEdit.Controls.Add(this.btnAuthEdit);
            this.gbxAuthEdit.Controls.Add(this.btnAuthSearch);
            resources.ApplyResources(this.gbxAuthEdit, "gbxAuthEdit");
            this.gbxAuthEdit.Name = "gbxAuthEdit";
            this.gbxAuthEdit.TabStop = false;
            // 
            // dgvAuth
            // 
            this.dgvAuth.AllowUserToAddRows = false;
            this.dgvAuth.AllowUserToDeleteRows = false;
            this.dgvAuth.AllowUserToResizeColumns = false;
            this.dgvAuth.AllowUserToResizeRows = false;
            this.dgvAuth.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAuth.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.dgvAuth, "dgvAuth");
            this.dgvAuth.MultiSelect = false;
            this.dgvAuth.Name = "dgvAuth";
            this.dgvAuth.RowHeadersVisible = false;
            this.dgvAuth.RowTemplate.Height = 24;
            this.dgvAuth.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            // 
            // btnAuthEdit
            // 
            this.btnAuthEdit.Image = global::HirataMainControl.Properties.Resources.AccountSave;
            resources.ApplyResources(this.btnAuthEdit, "btnAuthEdit");
            this.btnAuthEdit.Name = "btnAuthEdit";
            this.btnAuthEdit.UseVisualStyleBackColor = true;
            this.btnAuthEdit.Click += new System.EventHandler(this.btnAuthEdit_Click);
            // 
            // btnAuthSearch
            // 
            this.btnAuthSearch.Image = global::HirataMainControl.Properties.Resources.AccountSeh;
            resources.ApplyResources(this.btnAuthSearch, "btnAuthSearch");
            this.btnAuthSearch.Name = "btnAuthSearch";
            this.btnAuthSearch.UseVisualStyleBackColor = true;
            this.btnAuthSearch.Click += new System.EventHandler(this.btnAuthSearch_Click);
            // 
            // gbxAccountDel
            // 
            this.gbxAccountDel.Controls.Add(this.dgvAccount);
            this.gbxAccountDel.Controls.Add(this.btnAccountDel);
            this.gbxAccountDel.Controls.Add(this.btnRefresh);
            resources.ApplyResources(this.gbxAccountDel, "gbxAccountDel");
            this.gbxAccountDel.Name = "gbxAccountDel";
            this.gbxAccountDel.TabStop = false;
            // 
            // dgvAccount
            // 
            this.dgvAccount.AllowUserToAddRows = false;
            this.dgvAccount.AllowUserToDeleteRows = false;
            this.dgvAccount.AllowUserToResizeColumns = false;
            this.dgvAccount.AllowUserToResizeRows = false;
            this.dgvAccount.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAccount.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.dgvAccount, "dgvAccount");
            this.dgvAccount.MultiSelect = false;
            this.dgvAccount.Name = "dgvAccount";
            this.dgvAccount.RowHeadersVisible = false;
            this.dgvAccount.RowTemplate.Height = 24;
            this.dgvAccount.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // btnAccountDel
            // 
            this.btnAccountDel.Image = global::HirataMainControl.Properties.Resources.AccountDel;
            resources.ApplyResources(this.btnAccountDel, "btnAccountDel");
            this.btnAccountDel.Name = "btnAccountDel";
            this.btnAccountDel.UseVisualStyleBackColor = true;
            this.btnAccountDel.Click += new System.EventHandler(this.btnAccountDel_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = global::HirataMainControl.Properties.Resources.AccountSeh;
            resources.ApplyResources(this.btnRefresh, "btnRefresh");
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // gbxAccountAdd
            // 
            this.gbxAccountAdd.Controls.Add(this.labAddAuthority);
            this.gbxAccountAdd.Controls.Add(this.labAddPassword);
            this.gbxAccountAdd.Controls.Add(this.labAddName);
            this.gbxAccountAdd.Controls.Add(this.txtAddAuthority);
            this.gbxAccountAdd.Controls.Add(this.btnAccountAdd);
            this.gbxAccountAdd.Controls.Add(this.txtAddPassword);
            this.gbxAccountAdd.Controls.Add(this.txtAddName);
            resources.ApplyResources(this.gbxAccountAdd, "gbxAccountAdd");
            this.gbxAccountAdd.Name = "gbxAccountAdd";
            this.gbxAccountAdd.TabStop = false;
            // 
            // labAddAuthority
            // 
            resources.ApplyResources(this.labAddAuthority, "labAddAuthority");
            this.labAddAuthority.Name = "labAddAuthority";
            // 
            // labAddPassword
            // 
            resources.ApplyResources(this.labAddPassword, "labAddPassword");
            this.labAddPassword.Name = "labAddPassword";
            // 
            // labAddName
            // 
            resources.ApplyResources(this.labAddName, "labAddName");
            this.labAddName.Name = "labAddName";
            // 
            // txtAddAuthority
            // 
            this.txtAddAuthority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.txtAddAuthority, "txtAddAuthority");
            this.txtAddAuthority.FormattingEnabled = true;
            this.txtAddAuthority.Items.AddRange(new object[] {
            resources.GetString("txtAddAuthority.Items"),
            resources.GetString("txtAddAuthority.Items1")});
            this.txtAddAuthority.Name = "txtAddAuthority";
            // 
            // btnAccountAdd
            // 
            resources.ApplyResources(this.btnAccountAdd, "btnAccountAdd");
            this.btnAccountAdd.Image = global::HirataMainControl.Properties.Resources.AccountAdd;
            this.btnAccountAdd.Name = "btnAccountAdd";
            this.btnAccountAdd.UseVisualStyleBackColor = true;
            this.btnAccountAdd.Click += new System.EventHandler(this.btnAccountAdd_Click);
            // 
            // txtAddPassword
            // 
            this.txtAddPassword.AcceptsTab = true;
            resources.ApplyResources(this.txtAddPassword, "txtAddPassword");
            this.txtAddPassword.Name = "txtAddPassword";
            // 
            // txtAddName
            // 
            this.txtAddName.AcceptsTab = true;
            resources.ApplyResources(this.txtAddName, "txtAddName");
            this.txtAddName.Name = "txtAddName";
            // 
            // tapgMonitor
            // 
            resources.ApplyResources(this.tapgMonitor, "tapgMonitor");
            this.tapgMonitor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.tapgMonitor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tapgMonitor.Controls.Add(this.User_EFEM);
            this.tapgMonitor.Name = "tapgMonitor";
            // 
            // User_EFEM
            // 
            resources.ApplyResources(this.User_EFEM, "User_EFEM");
            this.User_EFEM.GetSetEQ_Barcode = "";
            this.User_EFEM.Name = "User_EFEM";
            // 
            // tapgLog
            // 
            this.tapgLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.tapgLog.Controls.Add(this.splLog);
            resources.ApplyResources(this.tapgLog, "tapgLog");
            this.tapgLog.Name = "tapgLog";
            // 
            // splLog
            // 
            resources.ApplyResources(this.splLog, "splLog");
            this.splLog.Name = "splLog";
            // 
            // splLog.Panel1
            // 
            this.splLog.Panel1.Controls.Add(this.splLogTime);
            // 
            // splLog.Panel2
            // 
            this.splLog.Panel2.Controls.Add(this.User_Log);
            // 
            // splLogTime
            // 
            resources.ApplyResources(this.splLogTime, "splLogTime");
            this.splLogTime.Name = "splLogTime";
            // 
            // splLogTime.Panel1
            // 
            this.splLogTime.Panel1.Controls.Add(this.cboDirectory);
            this.splLogTime.Panel1.Controls.Add(this.btnLogCopy);
            this.splLogTime.Panel1.Controls.Add(this.btnLogSearch);
            this.splLogTime.Panel1.Controls.Add(this.dtpLogStart);
            this.splLogTime.Panel1.Controls.Add(this.dtpLogEnd);
            this.splLogTime.Panel1.Controls.Add(this.labLogTo);
            // 
            // splLogTime.Panel2
            // 
            this.splLogTime.Panel2.Controls.Add(this.trvLog);
            // 
            // cboDirectory
            // 
            resources.ApplyResources(this.cboDirectory, "cboDirectory");
            this.cboDirectory.FormattingEnabled = true;
            this.cboDirectory.Name = "cboDirectory";
            // 
            // btnLogCopy
            // 
            resources.ApplyResources(this.btnLogCopy, "btnLogCopy");
            this.btnLogCopy.Name = "btnLogCopy";
            this.btnLogCopy.UseVisualStyleBackColor = true;
            this.btnLogCopy.Click += new System.EventHandler(this.btnLogCopy_Click);
            // 
            // btnLogSearch
            // 
            resources.ApplyResources(this.btnLogSearch, "btnLogSearch");
            this.btnLogSearch.Name = "btnLogSearch";
            this.btnLogSearch.UseVisualStyleBackColor = true;
            this.btnLogSearch.Click += new System.EventHandler(this.btnLogSearch_Click);
            // 
            // dtpLogStart
            // 
            resources.ApplyResources(this.dtpLogStart, "dtpLogStart");
            this.dtpLogStart.MinDate = new System.DateTime(2019, 3, 1, 0, 0, 0, 0);
            this.dtpLogStart.Name = "dtpLogStart";
            this.dtpLogStart.Value = new System.DateTime(2019, 3, 25, 16, 13, 9, 0);
            // 
            // dtpLogEnd
            // 
            resources.ApplyResources(this.dtpLogEnd, "dtpLogEnd");
            this.dtpLogEnd.MinDate = new System.DateTime(2019, 3, 1, 0, 0, 0, 0);
            this.dtpLogEnd.Name = "dtpLogEnd";
            this.dtpLogEnd.Value = new System.DateTime(2019, 3, 25, 23, 59, 59, 0);
            // 
            // labLogTo
            // 
            resources.ApplyResources(this.labLogTo, "labLogTo");
            this.labLogTo.Name = "labLogTo";
            // 
            // trvLog
            // 
            this.trvLog.BackColor = System.Drawing.Color.Gainsboro;
            this.trvLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.trvLog, "trvLog");
            this.trvLog.Name = "trvLog";
            this.trvLog.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.trvLog_AfterCollapse);
            this.trvLog.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.trvLog_BeforeExpand);
            this.trvLog.DoubleClick += new System.EventHandler(this.trvLog_DoubleClick);
            // 
            // User_Log
            // 
            this.User_Log.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            resources.ApplyResources(this.User_Log, "User_Log");
            this.User_Log.Name = "User_Log";
            // 
            // tapgProcess
            // 
            this.tapgProcess.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tapgProcess.Controls.Add(this.User_Core);
            resources.ApplyResources(this.tapgProcess, "tapgProcess");
            this.tapgProcess.Name = "tapgProcess";
            // 
            // User_Core
            // 
            this.User_Core.BackColor = System.Drawing.SystemColors.Control;
            this.User_Core.Busy = false;
            resources.ApplyResources(this.User_Core, "User_Core");
            this.User_Core.Name = "User_Core";
            // 
            // tapgCycle
            // 
            this.tapgCycle.BackColor = System.Drawing.Color.WhiteSmoke;
            resources.ApplyResources(this.tapgCycle, "tapgCycle");
            this.tapgCycle.Name = "tapgCycle";
            // 
            // tapgJpSetting
            // 
            this.tapgJpSetting.Controls.Add(this.panel1);
            this.tapgJpSetting.Controls.Add(this.gbxMemoryWafer);
            this.tapgJpSetting.Controls.Add(this.gbxPortSetting);
            this.tapgJpSetting.Controls.Add(this.gbxRecipeSetting);
            resources.ApplyResources(this.tapgJpSetting, "tapgJpSetting");
            this.tapgJpSetting.Name = "tapgJpSetting";
            this.tapgJpSetting.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rb_hand);
            this.panel1.Controls.Add(this.rb_rf);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // rb_hand
            // 
            resources.ApplyResources(this.rb_hand, "rb_hand");
            this.rb_hand.Name = "rb_hand";
            this.rb_hand.TabStop = true;
            this.rb_hand.UseVisualStyleBackColor = true;
            this.rb_hand.CheckedChanged += new System.EventHandler(this.rb_hand_CheckedChanged);
            // 
            // rb_rf
            // 
            resources.ApplyResources(this.rb_rf, "rb_rf");
            this.rb_rf.Name = "rb_rf";
            this.rb_rf.TabStop = true;
            this.rb_rf.UseVisualStyleBackColor = true;
            this.rb_rf.CheckedChanged += new System.EventHandler(this.rb_rf_CheckedChanged);
            // 
            // gbxMemoryWafer
            // 
            this.gbxMemoryWafer.Controls.Add(this.btnUpdateStatus);
            this.gbxMemoryWafer.Controls.Add(this.dgvWaferStatus);
            resources.ApplyResources(this.gbxMemoryWafer, "gbxMemoryWafer");
            this.gbxMemoryWafer.Name = "gbxMemoryWafer";
            this.gbxMemoryWafer.TabStop = false;
            // 
            // btnUpdateStatus
            // 
            resources.ApplyResources(this.btnUpdateStatus, "btnUpdateStatus");
            this.btnUpdateStatus.Name = "btnUpdateStatus";
            this.btnUpdateStatus.UseVisualStyleBackColor = true;
            this.btnUpdateStatus.Click += new System.EventHandler(this.btnUpdateStatus_Click);
            // 
            // dgvWaferStatus
            // 
            this.dgvWaferStatus.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvWaferStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWaferStatus.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Device,
            this.Memory,
            this.Sensor});
            resources.ApplyResources(this.dgvWaferStatus, "dgvWaferStatus");
            this.dgvWaferStatus.Name = "dgvWaferStatus";
            this.dgvWaferStatus.ReadOnly = true;
            this.dgvWaferStatus.RowTemplate.Height = 24;
            // 
            // Device
            // 
            resources.ApplyResources(this.Device, "Device");
            this.Device.Name = "Device";
            this.Device.ReadOnly = true;
            // 
            // Memory
            // 
            resources.ApplyResources(this.Memory, "Memory");
            this.Memory.Name = "Memory";
            this.Memory.ReadOnly = true;
            // 
            // Sensor
            // 
            resources.ApplyResources(this.Sensor, "Sensor");
            this.Sensor.Name = "Sensor";
            this.Sensor.ReadOnly = true;
            // 
            // gbxPortSetting
            // 
            this.gbxPortSetting.Controls.Add(this.splitPortSetting);
            resources.ApplyResources(this.gbxPortSetting, "gbxPortSetting");
            this.gbxPortSetting.Name = "gbxPortSetting";
            this.gbxPortSetting.TabStop = false;
            // 
            // splitPortSetting
            // 
            resources.ApplyResources(this.splitPortSetting, "splitPortSetting");
            this.splitPortSetting.Name = "splitPortSetting";
            // 
            // splitPortSetting.Panel1
            // 
            this.splitPortSetting.Panel1.Controls.Add(this.dgvLoadPortType);
            // 
            // splitPortSetting.Panel2
            // 
            this.splitPortSetting.Panel2.Controls.Add(this.btnOMSOut);
            this.splitPortSetting.Panel2.Controls.Add(this.btnOMSIn);
            this.splitPortSetting.Panel2.Controls.Add(this.btnDummyWafer);
            this.splitPortSetting.Panel2.Controls.Add(this.btnRealwafer);
            // 
            // dgvLoadPortType
            // 
            this.dgvLoadPortType.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvLoadPortType.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvLoadPortType.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLoadPortType.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LoadPort,
            this.PortType});
            resources.ApplyResources(this.dgvLoadPortType, "dgvLoadPortType");
            this.dgvLoadPortType.Name = "dgvLoadPortType";
            this.dgvLoadPortType.RowTemplate.Height = 24;
            this.dgvLoadPortType.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLoadPortType_CellClick);
            // 
            // LoadPort
            // 
            resources.ApplyResources(this.LoadPort, "LoadPort");
            this.LoadPort.Name = "LoadPort";
            // 
            // PortType
            // 
            resources.ApplyResources(this.PortType, "PortType");
            this.PortType.Name = "PortType";
            // 
            // btnOMSOut
            // 
            resources.ApplyResources(this.btnOMSOut, "btnOMSOut");
            this.btnOMSOut.Name = "btnOMSOut";
            this.btnOMSOut.UseVisualStyleBackColor = true;
            this.btnOMSOut.Click += new System.EventHandler(this.btnOMSOut_Click);
            // 
            // btnOMSIn
            // 
            resources.ApplyResources(this.btnOMSIn, "btnOMSIn");
            this.btnOMSIn.Name = "btnOMSIn";
            this.btnOMSIn.UseVisualStyleBackColor = true;
            this.btnOMSIn.Click += new System.EventHandler(this.btnOMSIn_Click);
            // 
            // btnDummyWafer
            // 
            resources.ApplyResources(this.btnDummyWafer, "btnDummyWafer");
            this.btnDummyWafer.Name = "btnDummyWafer";
            this.btnDummyWafer.UseVisualStyleBackColor = true;
            this.btnDummyWafer.Click += new System.EventHandler(this.btnDummyWafer_Click);
            // 
            // btnRealwafer
            // 
            resources.ApplyResources(this.btnRealwafer, "btnRealwafer");
            this.btnRealwafer.Name = "btnRealwafer";
            this.btnRealwafer.UseVisualStyleBackColor = true;
            this.btnRealwafer.Click += new System.EventHandler(this.btnRealwafer_Click);
            // 
            // gbxRecipeSetting
            // 
            this.gbxRecipeSetting.Controls.Add(this.clsRobotSetting);
            this.gbxRecipeSetting.Controls.Add(this.txtOCRDegree);
            this.gbxRecipeSetting.Controls.Add(this.labOCRDegree);
            this.gbxRecipeSetting.Controls.Add(this.txtAlignerDegree);
            this.gbxRecipeSetting.Controls.Add(this.labAlignerDegree);
            this.gbxRecipeSetting.Controls.Add(this.clsParameter);
            this.gbxRecipeSetting.Controls.Add(this.cboModeSetting);
            resources.ApplyResources(this.gbxRecipeSetting, "gbxRecipeSetting");
            this.gbxRecipeSetting.Name = "gbxRecipeSetting";
            this.gbxRecipeSetting.TabStop = false;
            // 
            // clsRobotSetting
            // 
            this.clsRobotSetting.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.clsRobotSetting.CheckOnClick = true;
            resources.ApplyResources(this.clsRobotSetting, "clsRobotSetting");
            this.clsRobotSetting.FormattingEnabled = true;
            this.clsRobotSetting.Items.AddRange(new object[] {
            resources.GetString("clsRobotSetting.Items"),
            resources.GetString("clsRobotSetting.Items1"),
            resources.GetString("clsRobotSetting.Items2"),
            resources.GetString("clsRobotSetting.Items3")});
            this.clsRobotSetting.Name = "clsRobotSetting";
            this.clsRobotSetting.MouseUp += new System.Windows.Forms.MouseEventHandler(this.clsRobotSetting_MouseUp);
            // 
            // txtOCRDegree
            // 
            resources.ApplyResources(this.txtOCRDegree, "txtOCRDegree");
            this.txtOCRDegree.Name = "txtOCRDegree";
            this.txtOCRDegree.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtOCRDegree_KeyPress);
            // 
            // labOCRDegree
            // 
            resources.ApplyResources(this.labOCRDegree, "labOCRDegree");
            this.labOCRDegree.Name = "labOCRDegree";
            // 
            // txtAlignerDegree
            // 
            resources.ApplyResources(this.txtAlignerDegree, "txtAlignerDegree");
            this.txtAlignerDegree.Name = "txtAlignerDegree";
            this.txtAlignerDegree.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAlignerDegree_KeyPress);
            // 
            // labAlignerDegree
            // 
            resources.ApplyResources(this.labAlignerDegree, "labAlignerDegree");
            this.labAlignerDegree.Name = "labAlignerDegree";
            // 
            // clsParameter
            // 
            this.clsParameter.BackColor = System.Drawing.SystemColors.Window;
            this.clsParameter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.clsParameter.CheckOnClick = true;
            resources.ApplyResources(this.clsParameter, "clsParameter");
            this.clsParameter.FormattingEnabled = true;
            this.clsParameter.Items.AddRange(new object[] {
            resources.GetString("clsParameter.Items"),
            resources.GetString("clsParameter.Items1"),
            resources.GetString("clsParameter.Items2"),
            resources.GetString("clsParameter.Items3"),
            resources.GetString("clsParameter.Items4")});
            this.clsParameter.Name = "clsParameter";
            this.clsParameter.MouseUp += new System.Windows.Forms.MouseEventHandler(this.clsParameter_MouseUp);
            // 
            // cboModeSetting
            // 
            resources.ApplyResources(this.cboModeSetting, "cboModeSetting");
            this.cboModeSetting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboModeSetting.FormattingEnabled = true;
            this.cboModeSetting.Name = "cboModeSetting";
            this.cboModeSetting.SelectedIndexChanged += new System.EventHandler(this.cboModeSetting_SelectedIndexChanged);
            // 
            // tapgProcessHistory
            // 
            this.tapgProcessHistory.Controls.Add(this.splHistory);
            resources.ApplyResources(this.tapgProcessHistory, "tapgProcessHistory");
            this.tapgProcessHistory.Name = "tapgProcessHistory";
            this.tapgProcessHistory.UseVisualStyleBackColor = true;
            // 
            // splHistory
            // 
            resources.ApplyResources(this.splHistory, "splHistory");
            this.splHistory.Name = "splHistory";
            // 
            // splHistory.Panel1
            // 
            this.splHistory.Panel1.Controls.Add(this.gbxWaferSearch);
            // 
            // splHistory.Panel2
            // 
            this.splHistory.Panel2.Controls.Add(this.dgvHistoryWafer);
            // 
            // gbxWaferSearch
            // 
            this.gbxWaferSearch.Controls.Add(this.txtHistoryCJ);
            this.gbxWaferSearch.Controls.Add(this.labWaferID);
            this.gbxWaferSearch.Controls.Add(this.labCarrierID);
            this.gbxWaferSearch.Controls.Add(this.txtHistoryWafer);
            this.gbxWaferSearch.Controls.Add(this.txtHistoryCarrier);
            this.gbxWaferSearch.Controls.Add(this.labHistoryCJID);
            this.gbxWaferSearch.Controls.Add(this.dtpHistoryEnd);
            this.gbxWaferSearch.Controls.Add(this.btnHistorySearch);
            this.gbxWaferSearch.Controls.Add(this.txtHistoryMagazine);
            this.gbxWaferSearch.Controls.Add(this.dtpHistoryStart);
            this.gbxWaferSearch.Controls.Add(this.txtHistoryCassette);
            this.gbxWaferSearch.Controls.Add(this.labTimeEnd);
            this.gbxWaferSearch.Controls.Add(this.labMagazineID);
            this.gbxWaferSearch.Controls.Add(this.labTimeStart);
            this.gbxWaferSearch.Controls.Add(this.labCassetteID);
            resources.ApplyResources(this.gbxWaferSearch, "gbxWaferSearch");
            this.gbxWaferSearch.Name = "gbxWaferSearch";
            this.gbxWaferSearch.TabStop = false;
            // 
            // txtHistoryCJ
            // 
            resources.ApplyResources(this.txtHistoryCJ, "txtHistoryCJ");
            this.txtHistoryCJ.Name = "txtHistoryCJ";
            // 
            // labWaferID
            // 
            resources.ApplyResources(this.labWaferID, "labWaferID");
            this.labWaferID.Name = "labWaferID";
            // 
            // labCarrierID
            // 
            resources.ApplyResources(this.labCarrierID, "labCarrierID");
            this.labCarrierID.Name = "labCarrierID";
            // 
            // txtHistoryWafer
            // 
            resources.ApplyResources(this.txtHistoryWafer, "txtHistoryWafer");
            this.txtHistoryWafer.Name = "txtHistoryWafer";
            // 
            // txtHistoryCarrier
            // 
            resources.ApplyResources(this.txtHistoryCarrier, "txtHistoryCarrier");
            this.txtHistoryCarrier.Name = "txtHistoryCarrier";
            // 
            // labHistoryCJID
            // 
            resources.ApplyResources(this.labHistoryCJID, "labHistoryCJID");
            this.labHistoryCJID.Name = "labHistoryCJID";
            // 
            // dtpHistoryEnd
            // 
            resources.ApplyResources(this.dtpHistoryEnd, "dtpHistoryEnd");
            this.dtpHistoryEnd.Name = "dtpHistoryEnd";
            // 
            // btnHistorySearch
            // 
            resources.ApplyResources(this.btnHistorySearch, "btnHistorySearch");
            this.btnHistorySearch.Name = "btnHistorySearch";
            this.btnHistorySearch.UseVisualStyleBackColor = true;
            this.btnHistorySearch.Click += new System.EventHandler(this.btnHistorySearch_Click);
            // 
            // txtHistoryMagazine
            // 
            resources.ApplyResources(this.txtHistoryMagazine, "txtHistoryMagazine");
            this.txtHistoryMagazine.Name = "txtHistoryMagazine";
            // 
            // dtpHistoryStart
            // 
            resources.ApplyResources(this.dtpHistoryStart, "dtpHistoryStart");
            this.dtpHistoryStart.Name = "dtpHistoryStart";
            // 
            // txtHistoryCassette
            // 
            resources.ApplyResources(this.txtHistoryCassette, "txtHistoryCassette");
            this.txtHistoryCassette.Name = "txtHistoryCassette";
            // 
            // labTimeEnd
            // 
            resources.ApplyResources(this.labTimeEnd, "labTimeEnd");
            this.labTimeEnd.Name = "labTimeEnd";
            // 
            // labMagazineID
            // 
            resources.ApplyResources(this.labMagazineID, "labMagazineID");
            this.labMagazineID.Name = "labMagazineID";
            // 
            // labTimeStart
            // 
            resources.ApplyResources(this.labTimeStart, "labTimeStart");
            this.labTimeStart.Name = "labTimeStart";
            // 
            // labCassetteID
            // 
            resources.ApplyResources(this.labCassetteID, "labCassetteID");
            this.labCassetteID.Name = "labCassetteID";
            // 
            // dgvHistoryWafer
            // 
            this.dgvHistoryWafer.AllowUserToAddRows = false;
            this.dgvHistoryWafer.AllowUserToDeleteRows = false;
            this.dgvHistoryWafer.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvHistoryWafer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.dgvHistoryWafer, "dgvHistoryWafer");
            this.dgvHistoryWafer.Name = "dgvHistoryWafer";
            this.dgvHistoryWafer.ReadOnly = true;
            this.dgvHistoryWafer.RowTemplate.Height = 24;
            // 
            // tapgPLC
            // 
            resources.ApplyResources(this.tapgPLC, "tapgPLC");
            this.tapgPLC.Name = "tapgPLC";
            this.tapgPLC.UseVisualStyleBackColor = true;
            // 
            // tapgSecsView
            // 
            resources.ApplyResources(this.tapgSecsView, "tapgSecsView");
            this.tapgSecsView.Name = "tapgSecsView";
            // 
            // splButton
            // 
            this.splButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            resources.ApplyResources(this.splButton, "splButton");
            this.splButton.Name = "splButton";
            // 
            // splButton.Panel1
            // 
            this.splButton.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            // 
            // splButton.Panel2
            // 
            this.splButton.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            // 
            // frmMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splMain);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmMain";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.splTopStatus.Panel1.ResumeLayout(false);
            this.splTopStatus.Panel1.PerformLayout();
            this.splTopStatus.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splTopStatus)).EndInit();
            this.splTopStatus.ResumeLayout(false);
            this.gbxWaferInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picFan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAccount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecipe)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEQSetting)).EndInit();
            this.splMain.Panel1.ResumeLayout(false);
            this.splMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splMain)).EndInit();
            this.splMain.ResumeLayout(false);
            this.splTop.Panel1.ResumeLayout(false);
            this.splTop.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splTop)).EndInit();
            this.splTop.ResumeLayout(false);
            this.splTopTitle.Panel1.ResumeLayout(false);
            this.splTopTitle.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splTopTitle)).EndInit();
            this.splTopTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picLog)).EndInit();
            this.tctlMain.ResumeLayout(false);
            this.tapgAccount.ResumeLayout(false);
            this.tctlAccount.ResumeLayout(false);
            this.tapgGuest.ResumeLayout(false);
            this.gbxAccount.ResumeLayout(false);
            this.gbxAccount.PerformLayout();
            this.tapgAdmin.ResumeLayout(false);
            this.gbxAuthEdit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAuth)).EndInit();
            this.gbxAccountDel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccount)).EndInit();
            this.gbxAccountAdd.ResumeLayout(false);
            this.gbxAccountAdd.PerformLayout();
            this.tapgMonitor.ResumeLayout(false);
            this.tapgLog.ResumeLayout(false);
            this.splLog.Panel1.ResumeLayout(false);
            this.splLog.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splLog)).EndInit();
            this.splLog.ResumeLayout(false);
            this.splLogTime.Panel1.ResumeLayout(false);
            this.splLogTime.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splLogTime)).EndInit();
            this.splLogTime.ResumeLayout(false);
            this.tapgProcess.ResumeLayout(false);
            this.tapgJpSetting.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gbxMemoryWafer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvWaferStatus)).EndInit();
            this.gbxPortSetting.ResumeLayout(false);
            this.splitPortSetting.Panel1.ResumeLayout(false);
            this.splitPortSetting.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitPortSetting)).EndInit();
            this.splitPortSetting.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLoadPortType)).EndInit();
            this.gbxRecipeSetting.ResumeLayout(false);
            this.gbxRecipeSetting.PerformLayout();
            this.tapgProcessHistory.ResumeLayout(false);
            this.splHistory.Panel1.ResumeLayout(false);
            this.splHistory.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splHistory)).EndInit();
            this.splHistory.ResumeLayout(false);
            this.gbxWaferSearch.ResumeLayout(false);
            this.gbxWaferSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistoryWafer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splButton)).EndInit();
            this.splButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private HCT_EFEM User_EFEM;
        private System.Windows.Forms.SplitContainer splMain;
        private System.Windows.Forms.SplitContainer splTop;
        private System.Windows.Forms.SplitContainer splTopTitle;
        private System.Windows.Forms.Label labEFEMStatut;
        private System.Windows.Forms.SplitContainer splTopStatus;
        private System.Windows.Forms.Label labMode;
        private System.Windows.Forms.Label labStatusTitle;
        private System.Windows.Forms.Label labAccount;
        private System.Windows.Forms.RichTextBox rtfLog;
        private System.Windows.Forms.SplitContainer splButton;
        private System.Windows.Forms.Label labTime;
        private System.Windows.Forms.Timer tmrMain;
        private System.Windows.Forms.PictureBox picLog;
        private System.Windows.Forms.PictureBox picAccount;
        private System.Windows.Forms.PictureBox picFan;
        private System.Windows.Forms.NotifyIcon icnMain;
        private System.Windows.Forms.TabControl tctlMain;
        private System.Windows.Forms.TabPage tapgAccount;
        private System.Windows.Forms.TabControl tctlAccount;
        private System.Windows.Forms.TabPage tapgGuest;
        private System.Windows.Forms.Button btnLogIn;
        private System.Windows.Forms.GroupBox gbxAccount;
        private System.Windows.Forms.Label labInPassword;
        private System.Windows.Forms.Label labInName;
        private System.Windows.Forms.TextBox txtInPassword;
        private System.Windows.Forms.TextBox txtInUser;
        private System.Windows.Forms.Button btnLogOut;
        private System.Windows.Forms.TabPage tapgAdmin;
        private System.Windows.Forms.GroupBox gbxAccountDel;
        private System.Windows.Forms.DataGridView dgvAccount;
        private System.Windows.Forms.Button btnAccountDel;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.GroupBox gbxAccountAdd;
        private System.Windows.Forms.Label labAddAuthority;
        private System.Windows.Forms.Label labAddPassword;
        private System.Windows.Forms.Label labAddName;
        private System.Windows.Forms.ComboBox txtAddAuthority;
        private System.Windows.Forms.Button btnAccountAdd;
        private System.Windows.Forms.TextBox txtAddPassword;
        private System.Windows.Forms.TextBox txtAddName;
        private System.Windows.Forms.TabPage tapgMonitor;
        private System.Windows.Forms.TabPage tapgLog;
        private System.Windows.Forms.SplitContainer splLog;
        private System.Windows.Forms.SplitContainer splLogTime;
        private System.Windows.Forms.Button btnLogSearch;
        private System.Windows.Forms.DateTimePicker dtpLogStart;
        private System.Windows.Forms.DateTimePicker dtpLogEnd;
        private System.Windows.Forms.Label labLogTo;
        private System.Windows.Forms.TreeView trvLog;
        private LogLib User_Log;
        private System.Windows.Forms.TabPage tapgProcess;
        private UserCore User_Core;
        private System.Windows.Forms.TabPage tapgCycle;
        private System.Windows.Forms.TabPage tapgJpSetting;
        private System.Windows.Forms.DataGridView dgvEQSetting;
        private System.Windows.Forms.Button btnEQSetting;
        private System.Windows.Forms.TextBox txtMax;
        private System.Windows.Forms.TextBox txtMin;
        private System.Windows.Forms.Label labEQmax;
        private System.Windows.Forms.Label labmid;
        private System.Windows.Forms.Label labEQmin;
        private System.Windows.Forms.Label labEQLP;
        private System.Windows.Forms.ComboBox cboEQSetting;
        private System.Windows.Forms.GroupBox gbxWaferInfo;
        private System.Windows.Forms.Label labUnknow;
        private System.Windows.Forms.Label labHaveWafer;
        private System.Windows.Forms.Label labNoWafer;
        private System.Windows.Forms.ComboBox cboLoadPortSetting;
        private System.Windows.Forms.ComboBox cboLoadPortType;
        private System.Windows.Forms.Label labUnknownWafer;
        private System.Windows.Forms.Label labRule;
        private System.Windows.Forms.Button btnLogCopy;
        private System.Windows.Forms.GroupBox gbxAuthEdit;
        private System.Windows.Forms.DataGridView dgvAuth;
        private System.Windows.Forms.Button btnAuthEdit;
        private System.Windows.Forms.Button btnAuthSearch;
        public System.Windows.Forms.DataGridView dgvRecipe;
        private System.Windows.Forms.Button btnRecipeDelete;
        private System.Windows.Forms.TextBox txtRecipe;
        private System.Windows.Forms.Button btnRecipeAdd;
        private System.Windows.Forms.Button btnRecipeSetting;
        private System.Windows.Forms.Label labPSW;
        private System.Windows.Forms.TabPage tapgProcessHistory;
        public System.Windows.Forms.DataGridView dgvHistoryWafer;
        private System.Windows.Forms.SplitContainer splHistory;
        private System.Windows.Forms.Button btnHistorySearch;
        private System.Windows.Forms.GroupBox gbxWaferSearch;
        private System.Windows.Forms.DateTimePicker dtpHistoryEnd;
        private System.Windows.Forms.TextBox txtHistoryMagazine;
        private System.Windows.Forms.DateTimePicker dtpHistoryStart;
        private System.Windows.Forms.TextBox txtHistoryCassette;
        private System.Windows.Forms.Label labTimeEnd;
        private System.Windows.Forms.Label labMagazineID;
        private System.Windows.Forms.Label labTimeStart;
        private System.Windows.Forms.Label labCassetteID;
        private System.Windows.Forms.Label labHistoryCJID;
        private System.Windows.Forms.TabPage tapgPLC;
        private System.Windows.Forms.Label labVersion;
        private System.Windows.Forms.ComboBox cboDirectory;
        private System.Windows.Forms.GroupBox gbxRecipeSetting;
        private System.Windows.Forms.CheckedListBox clsParameter;
        public System.Windows.Forms.ComboBox cboModeSetting;
        private System.Windows.Forms.TextBox txtAlignerDegree;
        private System.Windows.Forms.Label labAlignerDegree;
        private System.Windows.Forms.TextBox txtOCRDegree;
        private System.Windows.Forms.Label labOCRDegree;
        private System.Windows.Forms.GroupBox gbxPortSetting;
        private System.Windows.Forms.SplitContainer splitPortSetting;
        private System.Windows.Forms.DataGridView dgvLoadPortType;
        private System.Windows.Forms.Button btnOMSOut;
        private System.Windows.Forms.Button btnOMSIn;
        private System.Windows.Forms.Button btnDummyWafer;
        private System.Windows.Forms.Button btnRealwafer;
        private System.Windows.Forms.DataGridViewTextBoxColumn LoadPort;
        private System.Windows.Forms.DataGridViewTextBoxColumn PortType;
        private System.Windows.Forms.Label labEFEMMode;
        private System.Windows.Forms.Label labSECS;
        private System.Windows.Forms.Label labWaferID;
        private System.Windows.Forms.Label labCarrierID;
        private System.Windows.Forms.TextBox txtHistoryWafer;
        private System.Windows.Forms.TextBox txtHistoryCarrier;
        private System.Windows.Forms.GroupBox gbxMemoryWafer;
        private System.Windows.Forms.Button btnUpdateStatus;
        private System.Windows.Forms.DataGridView dgvWaferStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn Device;
        private System.Windows.Forms.DataGridViewTextBoxColumn Memory;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sensor;
        private System.Windows.Forms.TextBox txtHistoryCJ;
        private System.Windows.Forms.CheckedListBox clsRobotSetting;
        private System.Windows.Forms.Label labPLCversion;
        private System.Windows.Forms.Label labPCversion;
        private System.Windows.Forms.TabPage tapgSecsView;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rb_hand;
        private System.Windows.Forms.RadioButton rb_rf;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
    }
}

