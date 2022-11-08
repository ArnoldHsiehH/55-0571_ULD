using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Resources;
using Ionic.Zip;

namespace HirataMainControl
{
    public partial class frmMain : Form
    {
        public static readkind IDreader;

        #region Object

        Form_Initial InitPanel;
        Form_Alarm AlarmPanel;
        Form_CheckKey ChechHirataKey;
        Form_ProcessReport ProcessPanel;
        BackgroundWorker LogBG;

        #endregion

        #region Enun

        private enum MainFunc { Mincnt, Operation, Form, Setting, About, Maxcnt };
        private enum SubFunc
        {
            Mincnt_1 = 0, Initial, Ready, Auto, Stop, Continue, Remote_Local, Exit, Maxcnt_1,
            Mincnt_2 = 10, Monitor, Log, Account, Process, History, Maxcnt_2,
            Mincnt_3 = 20, UserEdit, IO, EFEM_Setting, PLC, SECS, Maxcnt_3,
            Mincnt_4 = 30, Manual, Maxcnt_4
        };

        #endregion

        #region Variable

        private MainFunc FnucSelect = MainFunc.Maxcnt;
        private int LogCount;
        private UI JobDelegate;

        private int Port_Index;
        private Button[] Mainbtn;
        private Label[] FFUlab;
        private ComboBox[] FFUCbo;
        private Button[] Subbtn;
        private string[] FFU;

        public static double[] PSW_AIvalue = new double[2];
        //private string[] LogDir = new string[] { NormalStatic.System,   //socket - EFEM
        //                                    NormalStatic.Robot,         //Device
        //                                    NormalStatic.CstPort,       //Device
        //                                    NormalStatic.MagazinePort,  //Device
        //                                    NormalStatic.Aligner,       //Device
        //                                    NormalStatic.E84,           //Device
        //                                    NormalStatic.Stage,          //Device
        //                                    NormalStatic.OCRReader,     //Device
        //                                    NormalStatic.IO,            //IO
        //                                    NormalStatic.Error,         //Woring
        //                                    NormalStatic.Alarm,         //Alarm
        //                                    NormalStatic.Operation,
        //                                    NormalStatic.PLC};   //PLC
        #endregion

        #region Initial

        public frmMain()
        {
            InitializeComponent();
            tctlMain.SelectTab("tapgAccount");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

            HT.EFEM.Initial();

            labVersion.Text = string.Format("Version:{0}", NormalStatic.Version);
            labTime.Text = DateTime.Now.ToString(NormalStatic.TimeFormat);
            tmrMain.Enabled = true;
            tmrMain.Interval = 1000;
            tmrMain.Start();

            ChechHirataKey = new Form_CheckKey();
            ChechHirataKey.Form_CheckKey_Load(sender, e);
            ChechHirataKey.CloseEvent += new Form_CheckKey.CheckKeyEvent(CheckKeyCloseControl);

            if (!SQLite.ConnOpen())
            {
                Application.Exit();
            }

            InitPanel = new Form_Initial(HT.EFEM.Initial_Count);
            //InitPanel.Location = new System.Drawing.Point(600, 300);
            InitPanel.Show();
            InitPanel.FormClosed += new FormClosedEventHandler(InitPanel_FormClosed);
            InitPanel.CloseEvent += new Form_Initial.InitialEvent(InitialCloseControl);

            User_Log.Initial();
            for (int i = 0; i < (int)LogDir.MaxCnt; i++)
            {
                cboDirectory.Items.Add(LogDir.System + i);
            }
            cboDirectory.SelectedIndex = 0;

            JobDelegate = new UI();
            JobDelegate.EventInitialSystem += new UI.InitialSystemEvent(InitialControl);
            JobDelegate.EventLog += new UI.LogEvent(LogControl);
            JobDelegate.EventAlarm += new UI.AlarmEvent(AlarmControl);
            JobDelegate.EventClosing += new UI.CloseEvent(CloseContol);
            JobDelegate.EventOperation += new UI.OperationEvent(OperationControl);
            JobDelegate.EventOperationButton += new UI.OperationButtonEvent(OperationButtonComplete);
            JobDelegate.init();
            //Wayne 20190730


            //CalTimeStart();

            User_Core.Initial(User_EFEM);
            User_Core.Change_EFEM_Mode += new UserCore.Mode_Change_Event(EFEMModeChange);
            
            User_EFEM.EFEM_StatusChange += new HCT_EFEM.EFEMEvent(EFEMStatusChange);
            User_EFEM.InitialCmd += new HCT_EFEM.Initial_Event(LDTriggerAllHome);   //Joanne 20220830
            User_EFEM.ModeChange += new HCT_EFEM.EFEMModeChangeEvent(EFEMModeChange);   //Joanne 20220908 Add

            Initial_UI();
            Ui_EFEMStatus = EFEMStatus.Unknown;
            AlarmPanel = new Form_Alarm();
            AlarmPanel.VisibleChanged += new EventHandler(AlarmPanel_VisibleChanged);
            AlarmPanel.btnClearAlarm.Click += new EventHandler(AlarmPanel_ClearClick);
            AlarmPanel.btnBuzzerOff.Click += new EventHandler(AlarmPanel_BuzzerOffClick);

            AppSetting.CheckDefaultList();
            UI.Log(NormalStatic.System, NormalStatic.EFEM, SystemList.ProgramOpen, NormalStatic.EFEM);

            if (AppSetting.LoadSetting("IDreader", NormalStatic.True) == NormalStatic.True)
            {
                rb_hand.Checked = true;
                IDreader = readkind.Hand;
            }
            else
            {
                rb_rf.Checked = true;

                IDreader = readkind.RFID;
            }

        }

        private void icnMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Maximized;
            icnMain.Visible = false;
        }

        private void CheckKeyCloseControl()
        {
            ChechHirataKey.Close();
            if (ChechHirataKey.CheckFlag)
            {
                this.Enabled = true;
            }
            else
            {
                User_Core.Close();
            }
        }

        private void InitialCloseControl()
        {
            InitPanel.Close();
            User_Core.Close();
        }

        private void InitPanel_FormClosed(object sender, EventArgs e)
        {
            if (ChechHirataKey.CheckFlag)
            {
                this.Enabled = true;
                btnLogIn.Enabled = true;
                //if (labAccount.Text == "")
                //{
                //    txtInUser.Text = "Admin";
                //    txtInPassword.Text = "Hirata";
                //    btnLogIn_Click(sender, e);
                //}

                User_EFEM.EFEMPageChange(0);
                splButton.Enabled = true;
                if (HT.EFEM.IsInitialSuccess == true)
                    Ui_EFEMStatus = EFEMStatus.SysCheck_Finish;
                else
                    Ui_EFEMStatus = EFEMStatus.SysCheck_Fail;
            }
        }

        private void Initial_UI()
        {
            string strTemp;
            string[] SplitTemp;

            //User_SecsView.Initial(User_SECS);

            #region Log

            dtpLogEnd.Value = DateTime.Now;
            dtpLogStart.Value = DateTime.Now.AddDays(-7);
            rtfLog.Text = "";

            #endregion

            #region History

            dtpHistoryEnd.Value = DateTime.Now;
            dtpHistoryStart.Value = DateTime.Now.AddDays(-7);

            #endregion

            #region JP Setting

            #region Recipe

            cboModeSetting.Items.Clear();
            for (int i = 0; i < (int)PJ_Type.MaxCnt; i++)
                cboModeSetting.Items.Add(((PJ_Type)i).ToString());
            cboModeSetting.Text = AppSetting.LoadSetting("Recipe_Mode", "Load");
            HT.Recipe.OCR_Degree = float.Parse(AppSetting.LoadSetting("Recipe_OCR_Degree", "0.0"));
            HT.Recipe.Aligner_Degree = float.Parse(AppSetting.LoadSetting("Recipe_Aligner_Degree", "0.0"));

            HT.Recipe.IsFlip = AppSetting.LoadSetting("Recipe_IsFlip", NormalStatic.False) == NormalStatic.True ? true : false;
            clsParameter.SetItemChecked(0, HT.Recipe.IsFlip);
            HT.Recipe.IsAligner = AppSetting.LoadSetting("Recipe_IsAligner", NormalStatic.False) == NormalStatic.True ? true : false;
            clsParameter.SetItemChecked(1, HT.Recipe.IsAligner);
            HT.Recipe.IsOCR_Up = AppSetting.LoadSetting("Recipe_IsOCR_Up", NormalStatic.False) == NormalStatic.True ? true : false;
            clsParameter.SetItemChecked(2, HT.Recipe.IsOCR_Up);
            HT.Recipe.IsOCR_Down = AppSetting.LoadSetting("Recipe_IsOCR_Down", NormalStatic.False) == NormalStatic.True ? true : false;
            clsParameter.SetItemChecked(3, HT.Recipe.IsOCR_Down);
            HT.Recipe.IsOCR_Stage = AppSetting.LoadSetting("Recipe_IsOCR_Stage", NormalStatic.False) == NormalStatic.True ? true : false;
            clsParameter.SetItemChecked(4, HT.Recipe.IsOCR_Stage);

            HT.Recipe.IsUseStage1 = AppSetting.LoadSetting("Recipe_UseStage1", NormalStatic.True) == NormalStatic.True ? true : false;
            clsRobotSetting.SetItemChecked(0, HT.Recipe.IsUseStage1);
            HT.Recipe.IsUseStage2 = AppSetting.LoadSetting("Recipe_UseStage2", NormalStatic.True) == NormalStatic.True ? true : false;
            clsRobotSetting.SetItemChecked(1, HT.Recipe.IsUseStage2);
            HT.Recipe.IsUseLower = AppSetting.LoadSetting("Recipe_UseLower", NormalStatic.True) == NormalStatic.True ? true : false;
            clsRobotSetting.SetItemChecked(2, HT.Recipe.IsUseLower);
            HT.Recipe.IsUseUpper = AppSetting.LoadSetting("Recipe_UseUpper", NormalStatic.True) == NormalStatic.True ? true : false;
            clsRobotSetting.SetItemChecked(3, HT.Recipe.IsUseUpper);


            txtOCRDegree.Text = HT.Recipe.OCR_Degree.ToString("f1");
            txtAlignerDegree.Text = HT.Recipe.Aligner_Degree.ToString("f1");

            #endregion

            #region PortType

            Port_Index = 0;
            dgvLoadPortType.Rows.Add((int)IOLPDevice.MaxCnt);
            dgvLoadPortType.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvLoadPortType.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvLoadPortType.AllowUserToAddRows = false;
            dgvLoadPortType.Columns[0].ReadOnly = true;
            dgvLoadPortType.Columns[1].ReadOnly = true;
            dgvLoadPortType.CellClick += new DataGridViewCellEventHandler(dgvLoadPortType_CellClick);
            for (int i = 0; i < (int)IOLPDevice.MaxCnt; i++)
            {
                dgvLoadPortType.Rows[i].Cells[0].Value = ((IOLPDevice)i).ToString();
                dgvLoadPortType.Rows[i].Cells[1].Value = AppSetting.LoadSetting(string.Format("{0}_Type", (IOLPDevice)i), CassetterPortType.Real.ToString());
            }

     


            int stage_retry = int.Parse(AppSetting.LoadSetting("Stage_Retry", "3"));

     
            #endregion

            #region WaferStatus

            dgvWaferStatus.Rows.Add((int)DeviceWaferStatus.Maxcnt);
            dgvWaferStatus.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvWaferStatus.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvWaferStatus.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvWaferStatus.AllowUserToAddRows = false;
            dgvWaferStatus.Columns[0].ReadOnly = true;
            dgvWaferStatus.Columns[1].ReadOnly = true;
            dgvWaferStatus.Columns[2].ReadOnly = true;
            for (int i = 0; i < (int)DeviceWaferStatus.Maxcnt; i++)
            {
                dgvWaferStatus.Rows[i].Cells[0].Value = ((DeviceWaferStatus)i).ToString();
            }

            #endregion

            #region FFU

            //CreateFFUSetting();

            #endregion
            #endregion

            Ui_EFEMMode = HT.EFEM.Mode;
            CreateMainFuncbtn();
        }

        private void MainFormClose(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure close program?",
                                  "Notice",
                                  MessageBoxButtons.YesNo,
                                  MessageBoxIcon.Warning,
                                  MessageBoxDefaultButton.Button1,
                                  MessageBoxOptions.ServiceNotification);

            if (result == DialogResult.Yes)
            {
                UI.Operate(NormalStatic.System, SystemList.ProgramClose.ToString());

                if (labAccount.Text != "")
                    btnLogOut_Click(sender, e);

                UI.Log(NormalStatic.System, NormalStatic.EFEM, SystemList.ProgramClose, NormalStatic.EFEM);

                tmrMain.Stop();
                User_Core.Close();
            }
        }

        #endregion

        #region UI_Event

        private void InitialControl(string devicemame, string status, string msg)
        {
            if (InitPanel.Visible)
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { InitialControl(devicemame, status, msg); }));
                    return;
                }

                if (devicemame == "Adam6050_1" && status == NormalStatic.False)
                {
                    EFEMStatusChange(Adam6050_DI.Power, true);
                }
                InitPanel.SetMessage(devicemame, status, msg);
            }
        }

        private void LogControl(string filetype, string datatime, string device, string description)
        {
            this.Invoke(new MethodInvoker(delegate () { User_Log.Write(filetype, datatime, device, description); }));

            switch (filetype)
            {
                case NormalStatic.System:
                    this.Invoke(new MethodInvoker(delegate () { User_Log.WriteLogToUi(rtfLog, datatime, device, description, ref LogCount); }));
                    break;
                case NormalStatic.Error:
                    this.Invoke(new MethodInvoker(delegate () { User_Log.WriteLogToWarningUi(rtfLog, datatime, device, description, ref LogCount); }));
                    break;
            }

        }

        private void AlarmControl(string alarmCode, string datatime, string device, string description)
        {
            //string ALTX = MySQL.Read_SQL_data("ALTX", "am", "alid = " + ALID);//SQL撈Alarm_description 撈不到重建 撈到就取代;
            //if (ALTX == null)
            //{
            //    ALTX = Alarm_description;
            //    MySQL.Insert_SQL_Alarm(ALID, ALTX);
            //}
            int valueTemp;
            bool IsWarning;
            try
            {
                valueTemp = int.Parse(alarmCode);
                IsWarning = valueTemp >= 1100 ? true : false;
            }
            catch
            {
                IsWarning = false;
            }

            IO_EFEM.EFEMStatusControl(StatusControl.Alarm);
            Ui_EFEMStatus = EFEMStatus.Run_Fail;
            User_EFEM.SignalTowerControl(EFEMStatus.Alarming);
            //Joanne 20210922
            HT.continueFlag = false;

            HT.JobEnd_MappingFlag = false;
            if (User_Core.Busy && IsWarning == false)
            {
                //if (HT.EFEM.Status == EFEMStatus.Run_Now && HT.EFEM.Mode == EFEMMode.Remote) //Mike
                //{
                //    UserSECS.SendEvent(StreamNo.S6, FunctionNo.F11, CEID_Item.PJ_Completed, "");
                //    UserSECS.SendEvent(StreamNo.S6, FunctionNo.F11, CEID_Item.CJ_Completed, "");
                //}
            }

            this.Invoke(new MethodInvoker(delegate () { AlarmPanel.CallAlarm(datatime, device, alarmCode, description, IsWarning); }));
            this.Invoke(new MethodInvoker(delegate () { User_Log.Write(NormalStatic.Alarm, datatime, device, string.Format("{0}{1}{2}{3}", NormalStatic.LeftSquare, alarmCode, NormalStatic.RightSquare, description)); }));
            //SA.SendAlarm(ALID);
            //MySQL.Insert_SQL_Alarm_history(ALID, ALTX);

        }

        private void OperationControl(string datatime, string device, string description)  //Operation txt
        {
            this.Invoke(new MethodInvoker(delegate () { User_Log.Write(NormalStatic.Operation, datatime, labAccount.Text, string.Format("{0}{1}{2}{3}", NormalStatic.LeftSquare, device, NormalStatic.RightSquare, description)); }));
        }

        private void CloseContol(string description)
        {
            this.Invoke(new MethodInvoker(delegate () { User_Log.Write(NormalStatic.System, DateTime.Now.ToString(NormalStatic.TimeFormat), NormalStatic.System, description); }));

            if (HT.EFEM.Close_Count > 0)
            {
                if (HT.EFEM.Close_Count == 1)
                    JobDelegate.Close();

                HT.EFEM.Close_Count--;
            }
            if (description == "MainForm End")
            {
                System.GC.Collect();
                this.Close();
                Environment.Exit(0);
            }
        }

        #endregion

        #region Operation_Button

        private void OperationChangeUI()
        {
            User_Core.Refresh_System();
            Subbtn[(int)SubFunc.Stop].Enabled = true;
            Subbtn[(int)SubFunc.Initial].BackColor = Color.WhiteSmoke;
            Subbtn[(int)SubFunc.Ready].BackColor = Color.WhiteSmoke;
            Subbtn[(int)SubFunc.Auto].BackColor = Color.WhiteSmoke;
            Subbtn[(int)SubFunc.Continue].BackColor = Color.WhiteSmoke;
            gbxPortSetting.Enabled = false;
            gbxRecipeSetting.Enabled = false;
            gbxMemoryWafer.Enabled = false;
            gbxAccount.Enabled = true;
            switch (Ui_EFEMStatus)
            {

                case EFEMStatus.Power_Off:
                case EFEMStatus.Unknown:
                case EFEMStatus.SysCheck_Fail:
                    {
                        User_EFEM.AuthorityChange(true);
                        if (FnucSelect == MainFunc.Operation)
                        {
                            Subbtn[(int)SubFunc.Initial].Enabled = true;
                            Subbtn[(int)SubFunc.Ready].Visible = false;
                            Subbtn[(int)SubFunc.Auto].Visible = false;
                            Subbtn[(int)SubFunc.Remote_Local].Visible = false;
                            Subbtn[(int)SubFunc.Continue].Visible = false;
                        }
                    }
                    break;

                case EFEMStatus.SysCheck_Finish:
                    {
                        if (FnucSelect == MainFunc.Operation)
                        {
                            Subbtn[(int)SubFunc.Initial].Enabled = true;
                            Subbtn[(int)SubFunc.Ready].Visible = false;
                            Subbtn[(int)SubFunc.Auto].Visible = false;
                            Subbtn[(int)SubFunc.Remote_Local].Visible = false;
                            Subbtn[(int)SubFunc.Continue].Visible = false;
                        }
                    }
                    break;

                case EFEMStatus.SysCheck_Now:
                case EFEMStatus.Init_Now:
                    {
                        User_EFEM.AuthorityChange(true);

                        if (FnucSelect == MainFunc.Operation)
                        {
                            //Subbtn[(int)SubFunc.Initial].BackColor = Color.MediumBlue;
                            Subbtn[(int)SubFunc.Initial].Enabled = false;
                            Subbtn[(int)SubFunc.Ready].Visible = false;
                            Subbtn[(int)SubFunc.Auto].Visible = false;
                            Subbtn[(int)SubFunc.Continue].Visible = false;
                            Subbtn[(int)SubFunc.Remote_Local].Visible = false;
                        }
                        gbxAccount.Enabled = false;
                    }
                    break;

                case EFEMStatus.Init_Fail:
                    {
                        if (FnucSelect == MainFunc.Operation)
                        {
                            //Subbtn[(int)SubFunc.Initial].BackColor = Color.Red;
                            Subbtn[(int)SubFunc.Initial].Enabled = true;
                            Subbtn[(int)SubFunc.Ready].Visible = false;
                            Subbtn[(int)SubFunc.Auto].Visible = false;
                            Subbtn[(int)SubFunc.Continue].Visible = false;
                        }
                            gbxMemoryWafer.Enabled = false;
                            User_EFEM.AuthorityChange(false);
                        
                    }
                    break;

                case EFEMStatus.Init_Finish:
                    {
                        if (FnucSelect == MainFunc.Operation)
                        {
                            Subbtn[(int)SubFunc.Initial].Enabled = true;
                            Subbtn[(int)SubFunc.Remote_Local].Visible = false;
                        }

                        User_EFEM.AuthorityChange(false);

                        if (HT.EFEM.Mode == EFEMMode.Local)
                        {
                            if (FnucSelect == MainFunc.Operation)
                            {
                                Subbtn[(int)SubFunc.Auto].Visible = false;
                                Subbtn[(int)SubFunc.Ready].Visible = false;
                                Subbtn[(int)SubFunc.Continue].Visible = false;
                            }
                            gbxRecipeSetting.Enabled = false;
                            gbxPortSetting.Enabled = false;
                            gbxMemoryWafer.Enabled = false;
                        }
                        else
                        {
                            if (FnucSelect == MainFunc.Operation)
                            {
                                Subbtn[(int)SubFunc.Auto].Visible = false;
                                Subbtn[(int)SubFunc.Ready].Visible = false;
                                Subbtn[(int)SubFunc.Continue].Visible = false;
                            }
                        }
                    
                    }
                    break;

                case EFEMStatus.Ready_Now:
                    {
                        if (FnucSelect == MainFunc.Operation)
                        {
                            //Subbtn[(int)SubFunc.Ready].BackColor = Color.MediumBlue;
                            Subbtn[(int)SubFunc.Initial].Enabled = false;
                            Subbtn[(int)SubFunc.Ready].Visible = false;
                            Subbtn[(int)SubFunc.Auto].Visible = false;
                            Subbtn[(int)SubFunc.Continue].Visible = false;

                            Subbtn[(int)SubFunc.Remote_Local].Visible = false;
                        }
                        gbxAccount.Enabled = false;
                        User_EFEM.AuthorityChange(true);

                    }
                    break;

                case EFEMStatus.Ready_Fail:
                    {
                        if (FnucSelect == MainFunc.Operation)
                        {
                            //Subbtn[(int)SubFunc.Ready].BackColor = Color.Red;
                            Subbtn[(int)SubFunc.Initial].Enabled = true;
                            Subbtn[(int)SubFunc.Ready].Visible = false;
                            Subbtn[(int)SubFunc.Auto].Visible = false;
                            Subbtn[(int)SubFunc.Continue].Visible = false;
                            Subbtn[(int)SubFunc.Remote_Local].Visible = false;
                        }
                        gbxMemoryWafer.Enabled = false;
                        User_EFEM.AuthorityChange(true);

                    }
                    break;

                case EFEMStatus.Ready_Finish:
                    {
                        if (FnucSelect == MainFunc.Operation)
                        {
                            Subbtn[(int)SubFunc.Initial].Enabled = false;
                            Subbtn[(int)SubFunc.Ready].Visible = false;
                            Subbtn[(int)SubFunc.Auto].Visible = false;
                            Subbtn[(int)SubFunc.Continue].Visible = false;
                            Subbtn[(int)SubFunc.Remote_Local].Visible = false;
                        }
                        User_EFEM.AuthorityChange(true);

                    }
                    break;

                case EFEMStatus.Run_Now:
                    {
                        if (FnucSelect == MainFunc.Operation)
                        {
                            // Subbtn[(int)SubFunc.Auto].BackColor = Color.MediumBlue;
                            Subbtn[(int)SubFunc.Initial].Enabled = false;
                            Subbtn[(int)SubFunc.Ready].Visible = false;
                            Subbtn[(int)SubFunc.Auto].Visible = false;
                            Subbtn[(int)SubFunc.Continue].Visible = false;
                            Subbtn[(int)SubFunc.Remote_Local].Visible = false;
                        }
                        gbxAccount.Enabled = false;
                        User_EFEM.AuthorityChange(true);

                    }
                    break;

                case EFEMStatus.Run_Fail:
                    {
                        if (FnucSelect == MainFunc.Operation)
                        {
                            //Subbtn[(int)SubFunc.Auto].BackColor = Color.Red;
                            Subbtn[(int)SubFunc.Initial].Enabled = false;
                            Subbtn[(int)SubFunc.Ready].Visible = false;
                            Subbtn[(int)SubFunc.Auto].Visible = false;
                            Subbtn[(int)SubFunc.Continue].Visible = false;
                            Subbtn[(int)SubFunc.Remote_Local].Visible = false;
                        }
                        gbxMemoryWafer.Enabled = false;
                        User_EFEM.AuthorityChange(true);

                    }
                    break;

                case EFEMStatus.Run_Finish:
                    {
                        if (FnucSelect == MainFunc.Operation)
                        {
                            Subbtn[(int)SubFunc.Remote_Local].Visible = false;

                            Subbtn[(int)SubFunc.Initial].Enabled = true;
                        }

                        if (HT.EFEM.Mode == EFEMMode.Local)
                        {
                            if (FnucSelect == MainFunc.Operation)
                            {
                                Subbtn[(int)SubFunc.Ready].Visible = false;
                                Subbtn[(int)SubFunc.Auto].Visible = false;
                            }
                            gbxRecipeSetting.Enabled = false;
                        }
                        else
                        {
                            if (FnucSelect == MainFunc.Operation)
                            {
                                Subbtn[(int)SubFunc.Ready].Visible = false;
                                Subbtn[(int)SubFunc.Auto].Visible = false;
                            }
                        }
                        User_EFEM.AuthorityChange(true);


                    }
                    break;

                case EFEMStatus.Continue_Now:
                    {
                        if (FnucSelect == MainFunc.Operation)
                        {
                            //Subbtn[(int)SubFunc.Continue].BackColor = Color.MediumBlue;
                            Subbtn[(int)SubFunc.Initial].Enabled = false;
                            Subbtn[(int)SubFunc.Ready].Visible = false;
                            Subbtn[(int)SubFunc.Auto].Visible = false;
                            Subbtn[(int)SubFunc.Continue].Visible = false;
                            Subbtn[(int)SubFunc.Remote_Local].Visible = false;
                        }
                        gbxAccount.Enabled = false;
                    }
                    break;

                case EFEMStatus.Continue_Fail:
                    {
                        if (FnucSelect == MainFunc.Operation)
                        {
                            //Subbtn[(int)SubFunc.Continue].BackColor = Color.Red;
                            Subbtn[(int)SubFunc.Initial].Enabled = true;
                            Subbtn[(int)SubFunc.Ready].Visible = false;
                            Subbtn[(int)SubFunc.Auto].Visible = false;
                            Subbtn[(int)SubFunc.Continue].Visible = false;
                            Subbtn[(int)SubFunc.Remote_Local].Visible = false;
                        }
                        gbxMemoryWafer.Enabled = false;
                    }
                    break;

                case EFEMStatus.Continue_Finish:
                    {
                        if (FnucSelect == MainFunc.Operation)
                        {
                            Subbtn[(int)SubFunc.Initial].Enabled = true;
                            Subbtn[(int)SubFunc.Ready].Visible = false;
                            Subbtn[(int)SubFunc.Auto].Visible = false;
                            Subbtn[(int)SubFunc.Continue].Visible = false;
                            Subbtn[(int)SubFunc.Remote_Local].Visible = false;
                        }
                    }
                    break;
            }
            // Subbtn[(int)SubFunc.Auto].Enabled = true;
            User_EFEM.SignalTowerControl(Ui_EFEMStatus);
        }

 
        private void OperationButtonComplete(string item, string status)
        {
            if (InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate () { OperationButtonComplete(item, status); }));
                return;
            }

            if (HT.EFEM.Status != EFEMStatus.Power_Off)
                Ui_EFEMStatus = (status == NormalStatic.False ? ((EFEMStatus)Enum.Parse(typeof(EFEMStatus), item)) - 1 : ((EFEMStatus)Enum.Parse(typeof(EFEMStatus), item)));

            switch (Ui_EFEMStatus)
            {
                case EFEMStatus.Ready_Finish:
                    {
                        User_Core.CreaterJobAutoForm();
                    }
                    break;
            }


            if (FnucSelect == MainFunc.Operation)
            {
                CreateSubFuncbtn(MainFunc.Operation, (int)SubFunc.Mincnt_1, (int)SubFunc.Maxcnt_1);

                OperationChangeUI();
            }
            else if (FnucSelect == MainFunc.Setting)
            {
            }
        }

        #endregion

        #region Log_Tab

        #region Log

        //private void cboDirectory_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    trvLog.Nodes.Clear();
        //    trvLog.Nodes.Add("");
        //}

        private void btnLogSearch_Click(object sender, EventArgs e)
        {
            trvLog.Nodes.Clear();

            if (dtpLogStart.Value.Date > dtpLogEnd.Value.Date)
            {
                MessageBox.Show("Wrong date range (End > Start)",
                                "Notice",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            double datecount = dtpLogEnd.Value.AddDays(1).Date.Subtract(dtpLogStart.Value.Date).TotalDays;
            if (datecount > 31)
            {
                MessageBox.Show("The scope is too large, the rule is within 1 month",
                                "Notice",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            for (int i = 0; i < datecount + 1; i++)
            {
                List<string> dirs = new List<string>(Directory.EnumerateDirectories(string.Format("{0}\\{1}", User_Log.LogPath, cboDirectory.Text), dtpLogStart.Value.Date.AddDays(i).ToString(NormalStatic.FileFormat)));
                foreach (var dir in dirs)
                {
                    TreeNode TestNode = new TreeNode();
                    TestNode.Nodes.Add("");
                    TestNode.Text = dir.Substring(dir.LastIndexOf("\\") + 1);
                    trvLog.Nodes.Add(TestNode);
                }
            }

            UI.Operate(NormalStatic.Log, string.Format("{0}_{1}", cboDirectory.Text, "Search Button"));
        }

        private void trvLog_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            e.Node.Nodes.Clear();

            // Joanne 20200507 Add
            if (cboDirectory.Text == "")
            {
                MessageBox.Show("Directory not selected");
            }
            else
            {
                string file_path = string.Format("{0}\\{1}\\{2}", User_Log.LogPath, cboDirectory.Text, e.Node.Text);
                List<string> dirs1 = new List<string>(Directory.EnumerateFiles(file_path));
                foreach (var dir1 in dirs1)
                    e.Node.Nodes.Add(dir1.Substring(dir1.LastIndexOf("\\") + 1));
            }
        }

        private void trvLog_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            e.Node.Nodes.Clear();
            e.Node.Nodes.Add("");
        }

        private void trvLog_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                // Joanne 20200507 Add
                if (cboDirectory.Text == "")
                {
                    MessageBox.Show("Directory not selected");
                }
                else
                {
                    if (trvLog.SelectedNode.Level == 1)
                    {
                        Process.Start(string.Format("{0}\\{1}\\{2}\\{3}", User_Log.LogPath, cboDirectory.Text, trvLog.SelectedNode.Parent.Text, trvLog.SelectedNode.Text));
                    }
                }
            }
            catch (Exception ex)
            {
                UI.Alarm(NormalStatic.Log, ErrorList.AP_TryCatchError, string.Format("{0},{1}", "click", ex.ToString()));
            }
        }

        #endregion

        #region BackUp

        private void btnLogCopy_Click(object sender, EventArgs e)
        {
            if (dtpLogStart.Value.Date > dtpLogEnd.Value.Date)
            {
                MessageBox.Show("Wrong date range (End > Start)",
                               "Notice",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Warning);
                return;
            }

            if (User_EFEM.RobotBusy(0) || User_EFEM.RobotBusy(1))
            {

                MessageBox.Show("Warning：Robot now is busy.",
                                 "Notice",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Warning);
                return;
            }

            if (User_Core.Busy)
            {
                string Busy_BG = "";

                if (User_Core.Robot1_BG.IsBusy)
                    Busy_BG = "Robot1 background process";
                if (User_Core.Robot2_BG.IsBusy)
                    Busy_BG = "Robot2 background process";
                if (User_Core.Device_BG.IsBusy)
                    Busy_BG = "Device background process";
                if (User_Core.Robot1_MappingReadyBG.IsBusy)
                    Busy_BG = "Robot1 mapping background process";
                if (User_Core.Robot2_MappingReadyBG.IsBusy)
                    Busy_BG = "Robot2 mapping background process";
                if (User_Core.InitialBG.IsBusy)
                    Busy_BG = "Initial background process";

                MessageBox.Show(string.Format("Warning：{0} is busy.", Busy_BG),
                                 "Notice",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Warning);
                return;
            }

            double datecount = dtpLogEnd.Value.AddDays(1).Date.Subtract(dtpLogStart.Value.Date).TotalDays;
            if (datecount > 8)
            {
                MessageBox.Show("The scope is too large, the rule is within 1 week",
                                "Notice",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }
            LogBG = new BackgroundWorker();
            LogBG.DoWork += new DoWorkEventHandler(LogDowork);
            LogBG.ProgressChanged += new ProgressChangedEventHandler(ProcessChange);
            LogBG.RunWorkerAsync();
            LogBG.WorkerReportsProgress = true;
            LogBG.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CopyLogCMP);
            btnLogCopy.Enabled = false;
        }

        private void LogDowork(object obj, DoWorkEventArgs e)
        {
            UI.Operate(NormalStatic.Log, "CopyLog Button Start");

            try
            {
                #region Folder

                if (Directory.Exists(string.Format("{0}_{1}", User_Log.LogPath, "CopyLog")))
                {
                    Directory.Delete(string.Format("{0}_{1}", User_Log.LogPath, "CopyLog"), true);
                    Directory.CreateDirectory(string.Format("{0}_{1}", User_Log.LogPath, "CopyLog"));
                }
                else
                {
                    Directory.CreateDirectory(string.Format("{0}_{1}", User_Log.LogPath, "CopyLog"));
                }

                #endregion

                #region ListItem

                ZipFile zip = new ZipFile();
                zip.ParallelDeflateThreshold = -1;
                double datecount = dtpLogEnd.Value.AddDays(1).Date.Subtract(dtpLogStart.Value.Date).TotalDays;
                LogBG.ReportProgress(10);
                Directory.CreateDirectory(string.Format("{0}{1}", User_Log.LogPath, @"_CopyLog\ListItem")); ;
                string[] file = Directory.GetFiles(NormalStatic.ExcelPath);
                foreach (string filename in file)
                {
                    File.Copy(filename, string.Format("{0}{1}", User_Log.LogPath + @"_CopyLog\ListItem\", Path.GetFileName(filename)));
                }
                LogBG.ReportProgress(30);

                #endregion

                #region Config/DB

                Directory.CreateDirectory(string.Format("{0}{1}", User_Log.LogPath, @"_CopyLog\Config"));
                File.Copy(NormalStatic.ConfigPath, string.Format("{0}{1}", User_Log.LogPath + @"_CopyLog\Config\", @"\HirataMainControl.EXE.config"));
                LogBG.ReportProgress(50);
                File.Copy(NormalStatic.DbPath, string.Format("{0}{1}", User_Log.LogPath + @"_CopyLog\Config\", @"\Hirata.db"));
                LogBG.ReportProgress(60);

                #endregion

                #region SECS_Ini_Log

                Directory.CreateDirectory(string.Format("{0}{1}", User_Log.LogPath, @"_CopyLog\Ini"));
                File.Copy(NormalStatic.ConfigPath, string.Format("{0}{1}", User_Log.LogPath + @"_CopyLog\Ini\", @"\secs.ini"));
                LogBG.ReportProgress(65);

                Directory.CreateDirectory(string.Format("{0}{1}", User_Log.LogPath, @"_CopyLog\SECS_Log")); ;
                string[] Secsfile = Directory.GetFiles(NormalStatic.SECSLogPath);
                foreach (string filename in Secsfile)
                {
                    File.Copy(filename, string.Format("{0}{1}", User_Log.LogPath + @"_CopyLog\SECS_Log\", Path.GetFileName(filename)));
                }
                LogBG.ReportProgress(70);

                #endregion

                #region Log

                for (int j = 0; j < (int)LogDir.MaxCnt; j++)
                {
                    List<string> directory = new List<string>(Directory.EnumerateDirectories(User_Log.LogPath, (LogDir.System + j).ToString()));

                    foreach (var dir in directory)
                    {
                        for (int i = 0; i < datecount + 1; i++)
                        {
                            List<string> dirs = new List<string>(Directory.EnumerateDirectories(dir, dtpLogStart.Value.Date.AddDays(i).ToString(NormalStatic.FileFormat)));
                            foreach (var date in dirs)
                            {
                                string CopylogPath = string.Format("{0}_{1}/{2}/{3}", User_Log.LogPath, "CopyLog", LogDir.System + j, dtpLogStart.Value.Date.AddDays(i).ToString(NormalStatic.FileFormat));
                                Directory.CreateDirectory(CopylogPath);
                                file = Directory.GetFiles(date);

                                foreach (string filestr in file)
                                {
                                    File.Copy(filestr, string.Format("{0}/{1}", CopylogPath, Path.GetFileName(filestr)));
                                }
                            }
                        }
                    }

                }

                #endregion

                #region Zip

                zip.AddDirectory(string.Format("{0}_{1}", User_Log.LogPath, "CopyLog"));
                zip.Save(string.Format("D:/HirataMain_Log/{0}_CopyLogBy_{1}.zip", NormalStatic.Version, DateTime.Now.ToString("yyyyMMdd_HHmm")));
                LogBG.ReportProgress(100);
                Directory.Delete(string.Format("{0}_{1}", User_Log.LogPath, "CopyLog"), true);
                e.Result = "";

                #endregion
            }
            catch (Exception ex)
            {
                UI.Alarm(NormalStatic.Log, ErrorList.AP_TryCatchError, string.Format("{0},{1}", "Backup", ex.ToString()));
                return;
            }
            UI.Operate(NormalStatic.Log, "CopyLog Button End");
        }

        private void ProcessChange(object obj, ProgressChangedEventArgs e)
        {
            if (ProcessPanel == null)
            {
                ProcessPanel = new Form_ProcessReport();
            }
            else
            {
                ProcessPanel.Location = new System.Drawing.Point(1200, 500);
                ProcessPanel.Show();
            }

            ProcessPanel.ProcessChange(e.ProgressPercentage);
        }

        private void CopyLogCMP(object obj, RunWorkerCompletedEventArgs e)
        {
            btnLogCopy.Enabled = true;
        }

        private void CopyFile(string soc, string des)
        {
            File.Copy(soc, des);
        }

        #endregion

        #endregion

        #region Time1S_Tick

        private void timeMain_Tick(object sender, EventArgs e)
        {
            labTime.Text = DateTime.Now.ToString(NormalStatic.TimeFormat);
            //CaluculationPSW();
            labPSW.Text = string.Format("{0}{1}", HT.EFEM.EFEM_PSW_Pa, "_Pa");

          
        }

        private void CaluculationPSW()
        {
            //HT.EFEM.EFEM_PSW_Pa = HT.EFEM.PSW_AIvalue <= FFU_PSW_Zero ? "0.0" : Math.Round((Math.Abs(HT.EFEM.PSW_AIvalue - FFU_PSW_Zero) * FFU_PSW_Slope), 1, MidpointRounding.AwayFromZero).ToString();
        }

        #endregion

        #region UI_Button

        private void CreateMainFuncbtn()
        {
            Mainbtn = new Button[(int)MainFunc.Maxcnt];
            ResourceManager rm = new ResourceManager(typeof(HirataMainControl.Properties.Resources));
            for (int i = (int)MainFunc.Maxcnt - 1; i > (int)MainFunc.Mincnt; i--)
            {
                Mainbtn[i] = new Button();

                Mainbtn[i].Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Mainbtn[i].ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
                Mainbtn[i].Dock = System.Windows.Forms.DockStyle.Top;
                Mainbtn[i].Image = rm.GetObject((MainFunc.Mincnt + i).ToString()) as Image;
                Mainbtn[i].ImageAlign = ContentAlignment.MiddleLeft;
                Mainbtn[i].Name = i.ToString();
                Mainbtn[i].Text = (MainFunc.Mincnt + i).ToString();
                Mainbtn[i].TextAlign = ContentAlignment.MiddleRight;
                Mainbtn[i].TabIndex = i;
                Mainbtn[i].AutoSize = true;
                Mainbtn[i].Click += new EventHandler(btnMainFnuc_Click);
                splButton.Panel2.Controls.Add(Mainbtn[i]);
                Mainbtn[i].Visible = false;
            }
        }

        private void btnMainFnuc_Click(object sender, EventArgs e)
        {
            switch (((Button)(sender)).TabIndex)
            {
                case (int)MainFunc.Operation:
                    {
                        CreateSubFuncbtn(MainFunc.Operation, (int)SubFunc.Mincnt_1, (int)SubFunc.Maxcnt_1);

                        OperationChangeUI();
                    }
                    break;

                case (int)MainFunc.Form:
                    {
                        CreateSubFuncbtn(MainFunc.Form, (int)SubFunc.Mincnt_2, (int)SubFunc.Maxcnt_2);

                    }
                    break;

                case (int)MainFunc.Setting:
                    {
                        CreateSubFuncbtn(MainFunc.Setting, (int)SubFunc.Mincnt_3, (int)SubFunc.Maxcnt_3);
                    }
                    break;

                case (int)MainFunc.About:
                    {
                        CreateSubFuncbtn(MainFunc.About, (int)SubFunc.Mincnt_4, (int)SubFunc.Maxcnt_4);
                    }
                    break;
            }
        }

        private void CreateSubFuncbtn(MainFunc temp, int start, int end)
        {
            if (FnucSelect != temp)
            {
                FnucSelect = temp;
                splButton.Panel1.Controls.Clear();
                Subbtn = new Button[end - start];
                ResourceManager rm = new ResourceManager(typeof(HirataMainControl.Properties.Resources));
                for (int i = (int)end - 1 - start; i > 0; i--)
                {
                    Subbtn[i] = new Button();
                    Subbtn[i].AutoSize = true;
                    Subbtn[i].Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    Subbtn[i].ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
                    Subbtn[i].Dock = System.Windows.Forms.DockStyle.Top;
                    Subbtn[i].Image = rm.GetObject((SubFunc.Mincnt_1 + i + start).ToString()) as Image;
                    Subbtn[i].ImageAlign = ContentAlignment.MiddleLeft;
                    Subbtn[i].BackColor = Color.WhiteSmoke;
                    Subbtn[i].Name = i.ToString();
                    Subbtn[i].Text = (SubFunc.Mincnt_1 + i + start).ToString();
                    Subbtn[i].TextAlign = ContentAlignment.MiddleRight;
                    Subbtn[i].TabIndex = i + start;
                    Subbtn[i].Dock = DockStyle.Top;
                    Subbtn[i].Click += new EventHandler(btnSubFnuc_Click);
                    splButton.Panel1.Controls.Add(Subbtn[i]);
                }
                SubBtnAuthChange();
                Label Lab = new Label();
                Lab.Dock = System.Windows.Forms.DockStyle.Top;
                Lab.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Lab.Size = new System.Drawing.Size(160, 30);
                Lab.Name = "labButtonTitle";
                Lab.Text = FnucSelect.ToString();
                Lab.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                splButton.Panel1.Controls.Add(Lab);
            }
        }

        private void btnSubFnuc_Click(object sender, EventArgs e)
        {
            UI.Operate(FnucSelect.ToString(), ((Button)(sender)).Text);
          
            switch (FnucSelect)
            {
                case MainFunc.Operation:
                    {
                        User_EFEM.CloseTSForm();
                        switch (((Button)(sender)).TabIndex)
                        {
                            case (int)SubFunc.Initial:
                                {
                                    if (!User_EFEM.Ready)
                                    {
                                        UI.Alarm(NormalStatic.EFEM, ErrorList.AP_InitialFail_0393);
                                        break;
                                    }

                                    if (User_Core.Busy)
                                    {
                                        string Busy_BG = "";

                                        if (User_Core.Robot1_BG.IsBusy)
                                            Busy_BG = "Robot1 background process";
                               
                                        if (User_Core.Device_BG.IsBusy)
                                            Busy_BG = "Device background process";
                                        if (User_Core.Robot1_MappingReadyBG.IsBusy)
                                            Busy_BG = "Robot1 mapping background process";
                                  
                                        if (User_Core.InitialBG.IsBusy)
                                            Busy_BG = "Initial background process";

                                        MessageBox.Show(string.Format("Warning：{0} is busy.", Busy_BG),
                                                         "Notice",
                                                         MessageBoxButtons.OK,
                                                         MessageBoxIcon.Warning);
                                        break;
                                    }

                                    if (!User_Core.Busy)
                                    {

                                        DialogResult myResult = MessageBox.Show("Warning：Are you sure to perfrom initialization EFEM (All Device Home)?",
                                                                                "Initial",
                                                                                MessageBoxButtons.YesNo,
                                                                                MessageBoxIcon.Warning);
                                        if (myResult == DialogResult.Yes)
                                        {
                                            Ui_EFEMStatus = EFEMStatus.Init_Now;
                                            UI.Log(NormalStatic.System, NormalStatic.Core, SystemList.ProgramOpen, NormalStatic.InitialDevice);
                                            IO_EFEM.EFEMStatusControl(StatusControl.Run);
                                            OperationChangeUI();
                                            User_Core.InitialBG.RunWorkerAsync();
                                            //User_Core.Device_BG.RunWorkerAsync(); // Mike

                                        }

                                    }
                                }
                                break;

                            case (int)SubFunc.Ready:
                                {
                                    if (!User_EFEM.CheckEFEMStatus())
                                    {
                                        break;
                                    }

                                    if (User_Core.Busy)
                                    {
                                        string Busy_BG = "";

                                        if (User_Core.Robot1_BG.IsBusy)
                                            Busy_BG = "Robot1 background process";
                                        if (User_Core.Robot2_BG.IsBusy)
                                            Busy_BG = "Robot2 background process";
                                        if (User_Core.Device_BG.IsBusy)
                                            Busy_BG = "Device background process";
                                        if (User_Core.Robot1_MappingReadyBG.IsBusy)
                                            Busy_BG = "Robot1 mapping background process";
                                        if (User_Core.Robot2_MappingReadyBG.IsBusy)
                                            Busy_BG = "Robot2 mapping background process";
                                        if (User_Core.InitialBG.IsBusy)
                                            Busy_BG = "Initial background process";

                                        MessageBox.Show(string.Format("Warning：{0} is busy.", Busy_BG),
                                                         "Notice",
                                                         MessageBoxButtons.OK,
                                                         MessageBoxIcon.Warning);
                                        break;
                                    }

                                    if (!User_Core.Busy)
                                    {
                                        DialogResult myResult = MessageBox.Show("Warning：Are you sure to perfrom Ready EFEM(Robot Mapping)?",
                                                                              "Ready",
                                                                              MessageBoxButtons.YesNo,
                                                                              MessageBoxIcon.Warning);
                                        if (myResult == DialogResult.Yes)
                                        {
                                            Ui_EFEMStatus = EFEMStatus.Ready_Now;
                                            IO_EFEM.EFEMStatusControl(StatusControl.Run);
                                            UI.Log(NormalStatic.System, NormalStatic.Core, SystemList.ProgramOpen, NormalStatic.Ready);
                                            OperationChangeUI();
                                            User_Core.Robot1_MappingReadyBG.RunWorkerAsync();

                                            if (HT.Recipe.AutoMode != PJ_Type.Sortering)
                                                User_Core.Robot2_MappingReadyBG.RunWorkerAsync();
                                        }

                                    }

                                }
                                break;
                            case (int)SubFunc.Auto:
                                {
                                    if (!User_EFEM.CheckEFEMStatus())
                                    {
                                        break;
                                    }

                                    if (WithoutDeviceCheck())
                                    {
                                        UI.Alarm(NormalStatic.EFEM, ErrorList.AP_DeviceNotWithoutNoAuto_0552);
                                        break;
                                    }

                                    if (HT.EFEM.Mode != EFEMMode.Remote)
                                    {
                                        DataTable WaferDt = SQLite.ReadDataTable(SQLTable.PJ_Pool, "1=1");
                                        if (WaferDt.Rows.Count == 0)
                                        {
                                            MessageBox.Show("Warning：Procss Job is Empty!",
                                                             "Notice",
                                                             MessageBoxButtons.OK,
                                                             MessageBoxIcon.Warning);
                                            break;
                                        }
                                    }

                                    if (User_Core.Busy)
                                    {
                                        string Busy_BG = "";

                                        if (User_Core.Robot1_BG.IsBusy)
                                            Busy_BG = "Robot1 background process";
                                        if (User_Core.Robot2_BG.IsBusy)
                                            Busy_BG = "Robot2 background process";
                                        if (User_Core.Device_BG.IsBusy)
                                            Busy_BG = "Device background process";
                                        if (User_Core.Robot1_MappingReadyBG.IsBusy)
                                            Busy_BG = "Robot1 mapping background process";
                                        if (User_Core.Robot2_MappingReadyBG.IsBusy)
                                            Busy_BG = "Robot2 mapping background process";
                                        if (User_Core.InitialBG.IsBusy)
                                            Busy_BG = "Initial background process";

                                        MessageBox.Show(string.Format("Warning：{0} is busy.", Busy_BG),
                                                         "Notice",
                                                         MessageBoxButtons.OK,
                                                         MessageBoxIcon.Warning);
                                        break;
                                    }

                                    if (HT.EFEM.Mode == EFEMMode.Remote)
                                    {
                                        HT.EFEM.DryRunMode = false;

                                        if (HT.EFEM.IsContinue)
                                        {
                                            HT.EFEM.IsContinue = false;
                                        }
                                        else
                                        {
                                            SQLite.Delete(SQLTable.PJ_Pool, "1=1");

                                            //Joanne 20201009 Add Start
                                            User_Core.IsSetJobComplete_RB1 = false;
                                            User_Core.IsSetJobComplete_RB2 = false;
                                            //Joanne 20201009 Add End
                                        }
                                    }
                                    else
                                    {
                                        HT.EFEM.DryRunNowCount = 0;
                                        //Walson 20201211 Add Start
                                        User_Core.IsSetJobComplete_RB1 = false;
                                        User_Core.IsSetJobComplete_RB2 = false;
                                        //Walson 20201211 Add End
                                    }

                                    if (!User_Core.Busy)
                                    {
                                        DialogResult myResult = MessageBox.Show("Warning：Are you sure to perfrom Run EFEM?",
                                                                              "Run",
                                                                              MessageBoxButtons.YesNo,
                                                                              MessageBoxIcon.Warning);
                                        if (myResult == DialogResult.Yes)
                                        {
                                            Ui_EFEMStatus = EFEMStatus.Run_Now;
                                            IO_EFEM.EFEMStatusControl(StatusControl.Run);
                                            UI.Log(NormalStatic.System, NormalStatic.Core, SystemList.ProgramOpen, NormalStatic.Auto);
                                            OperationChangeUI();
                                            User_Core.Robot1_BG.RunWorkerAsync();
                                            if (HT.Recipe.AutoMode != PJ_Type.Sortering || HT.EFEM.Mode == EFEMMode.Remote)
                                                User_Core.Robot2_BG.RunWorkerAsync();
                                        }

                                    }

                                }
                                break;

                            case (int)SubFunc.Continue:
                                {
                                    if (!User_EFEM.CheckEFEMStatus())
                                    {
                                        break;
                                    }

                                    if (WithoutDeviceCheck())
                                    {
                                        UI.Alarm(NormalStatic.EFEM, ErrorList.AP_DeviceNotWithoutNoAuto_0552);
                                        break;
                                    }

                                    if (!User_Core.Busy)
                                    {
                                        DialogResult myResult = MessageBox.Show("Warning：Are you sure to perfrom Continue EFEM?",
                                                                              "Continue",
                                                                              MessageBoxButtons.YesNo,
                                                                              MessageBoxIcon.Warning);

                                        if (myResult == DialogResult.Yes)
                                        {
                                            HT.continueFlag = true; //Wayne 20210922
                                            Ui_EFEMStatus = EFEMStatus.Continue_Now;
                                            IO_EFEM.EFEMStatusControl(StatusControl.Run);
                                            UI.Log(NormalStatic.System, NormalStatic.Core, SystemList.ProgramOpen, "Continue");
                                            OperationChangeUI();
                                            User_Core.Robot1_MappingReadyBG.RunWorkerAsync();
                                            User_Core.Robot2_MappingReadyBG.RunWorkerAsync();
                                        }

                                    }
                                }
                                break;

                            case (int)SubFunc.Stop:
                                {
                                    btnSubCoreStop();
                                }
                                break;

                            case (int)SubFunc.Remote_Local:
                                {
                                    if (User_Core.Busy)
                                    {
                                        string Busy_BG = "";

                                        if (User_Core.Robot1_BG.IsBusy)
                                            Busy_BG = "Robot1 background process";
                                        if (User_Core.Robot2_BG.IsBusy)
                                            Busy_BG = "Robot2 background process";
                                        if (User_Core.Device_BG.IsBusy)
                                            Busy_BG = "Device background process";
                                        if (User_Core.Robot1_MappingReadyBG.IsBusy)
                                            Busy_BG = "Robot1 mapping background process";
                                        if (User_Core.Robot2_MappingReadyBG.IsBusy)
                                            Busy_BG = "Robot2 mapping background process";
                                        if (User_Core.InitialBG.IsBusy)
                                            Busy_BG = "Initial background process";

                                        DialogResult myResult = MessageBox.Show(string.Format("Warning：{0} is busy", Busy_BG),
                                                                        "Remote_Local",
                                                                        MessageBoxButtons.OK,
                                                                        MessageBoxIcon.Error);
                                    }
                                    else
                                    {
                                        switch (HT.EFEM.Mode)
                                        {
                                            case EFEMMode.Local:
                                                {
                                                    EFEMModeChange(EFEMMode.Remote);
                                                }
                                                break;

                                            case EFEMMode.Remote:
                                            case EFEMMode.Unknown:
                                                {
                                                    EFEMModeChange(EFEMMode.Local);
                                                }
                                                break;

                                        }
                                        OperationChangeUI();
                                        UI.Operate(NormalStatic.System, string.Format("EFEM Mode change :{0}", HT.EFEM.Mode));
                                    }

                                }
                                break;

                            case (int)SubFunc.Exit:
                                {
                                    if (User_EFEM.RobotBusy(0))
                                    {
                                        DialogResult myResult = MessageBox.Show("Warning：Robot now is busy",
                                                                        "Exit",
                                                                        MessageBoxButtons.OK,
                                                                        MessageBoxIcon.Error);
                                    }
                                    else if (User_Core.Busy)
                                    {
                                        string Busy_BG = "";

                                        if (User_Core.Robot1_BG.IsBusy)
                                            Busy_BG = "Robot1 background process";
                                        if (User_Core.Robot2_BG.IsBusy)
                                            Busy_BG = "Robot2 background process";
                                        if (User_Core.Device_BG.IsBusy)
                                            Busy_BG = "Device background process";
                                        if (User_Core.Robot1_MappingReadyBG.IsBusy)
                                            Busy_BG = "Robot1 mapping background process";
                                        if (User_Core.Robot2_MappingReadyBG.IsBusy)
                                            Busy_BG = "Robot2 mapping background process";
                                        if (User_Core.InitialBG.IsBusy)
                                            Busy_BG = "Initial background process";

                                        MessageBox.Show(string.Format("Warning：{0} is busy.", Busy_BG),
                                                         "Notice",
                                                         MessageBoxButtons.OK,
                                                         MessageBoxIcon.Warning);
                                    }
                                    else
                                    {
                                        User_EFEM.UnloaderClose();
                                        MainFormClose(sender, e);
                                    }
                                }
                                break;
                        }
                    }
                    break;
                case MainFunc.Form:
                    {
                        switch (((Button)(sender)).TabIndex)
                        {
                            case (int)SubFunc.Monitor:
                                {
                                    User_EFEM.EFEMPageChange(0);
                                    tctlMain.SelectTab(tapgMonitor);
                                    //User_EFEM.AuthorityChange(User_Core.Busy);
                                }
                                break;

                            case (int)SubFunc.Log:
                                {
                                    dtpLogEnd.Value = DateTime.Now;
                                    dtpLogStart.Value = DateTime.Now.AddDays(-7);
                                    tctlMain.SelectTab(tapgLog);
                                }
                                break;

                            case (int)SubFunc.Account:
                                {
                                    tctlMain.SelectTab(tapgAccount);
                                    tctlAccount.SelectTab(tapgGuest);
                                }
                                break;

                            case (int)SubFunc.Process:
                                {
                                    tctlMain.SelectTab(tapgProcess);
                                    User_Core.Refresh_PJ();
                                    User_Core.Refresh_AuthorityCondition();
                                }
                                break;

                            case (int)SubFunc.History:
                                {
                                    dtpHistoryEnd.Value = DateTime.Now;
                                    dtpHistoryStart.Value = DateTime.Now.AddDays(-7);
                                    tctlMain.SelectTab(tapgProcessHistory);
                                }
                                break;

                        }
                    }
                    break;

                case MainFunc.Setting:
                    {

                        switch (((Button)(sender)).TabIndex)
                        {
                            case (int)SubFunc.UserEdit:
                                {
                                    tctlMain.SelectTab(tapgAccount);
                                    tctlAccount.SelectTab(tapgAdmin);
                                }
                                break;

                            case (int)SubFunc.IO:
                                {
                                    User_EFEM.EFEMPageChange(1);
                                    tctlMain.SelectTab(tapgMonitor);
                                }
                                break;

                            case (int)SubFunc.EFEM_Setting:
                                {
                                
                                    tctlMain.SelectTab(tapgJpSetting);
                                    gbxRecipeSetting.Visible = false;
                                    gbxPortSetting.Visible = false;
                                    gbxMemoryWafer.Visible = false;
                                }
                                break;

                            case (int)SubFunc.PLC:
                                {
                                    tctlMain.SelectTab(tapgPLC);
                                }
                                break;

                            case (int)SubFunc.SECS:
                                {
                                    tctlMain.SelectTab(tapgSecsView);
                                }
                                break;
                        }
                    }
                    break;

                case MainFunc.About:
                    {

                        switch (((Button)(sender)).TabIndex)
                        {
                            case (int)SubFunc.Manual:
                                {
                                    try
                                    {
                                        Process.Start(string.Format("{0}{1}{2}", NormalStatic.ManualPath, "EFEM", ".pdf"));
                                    }
                                    catch
                                    {
                                        MessageBox.Show("Can not find the manual description file",
                                                        "Notice",
                                                        MessageBoxButtons.OK,
                                                        MessageBoxIcon.Error);
                                    }
                                }
                                break;
                        }
                    }
                    break;
            }
        }

        private bool btnSubCoreStop()
        {
            if (User_Core.Busy == false)
            {
                MessageBox.Show("Not currently busy",
                                "Notice",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }

            DialogResult myResult = MessageBox.Show("Error：Are you sure interrupt program?",
                                                    "Interrupt",
                                                    MessageBoxButtons.YesNo,
                                                    MessageBoxIcon.Error);
            if (myResult == DialogResult.Yes)
            {
                UI.Operate(NormalStatic.Operation, "Press Stop Button");
                User_Core.Interrupt();
                return true;
            }
            return false;
        }

        private void SubBtnAuthChange()
        {
            int Col = -1;
            switch (HT.EFEM.Authority)
            {
                case AuthorityTable.Engineer:
                    Col = 2;
                    break;
                case AuthorityTable.Admin:
                    Col = 3;
                    break;
                case AuthorityTable.Operator:
                    Col = 4;
                    break;
            }

            foreach (Control Ctrl in splButton.Panel1.Controls)
            {
                string condition = string.Format("{0} = {1}", SQLTableItem.UIIndex, Ctrl.TabIndex);
                DataTable AuthDt = SQLite.ReadDataTable(SQLTable.Authority, condition);
                if (AuthDt.Rows[0][Col].ToString() == "Y")
                {
                    Ctrl.Visible = true;
                }
                else
                {
                    Ctrl.Visible = false;
                }

                if (AuthDt.Rows[0][5].ToString() == "False")
                {
                    Ctrl.Enabled = false;
                }
            }

        }

        #endregion

        #region Alarm_Page

        private void AlarmPanel_VisibleChanged(object sender, EventArgs e)
        {
            if (!AlarmPanel.Visible)
            {
                IO_EFEM.EFEMStatusControl(StatusControl.Idle);
            }
        }

        private void AlarmPanel_ClearClick(object sender, EventArgs e)
        {
            User_EFEM.BuzzerOff();

            AlarmPanel.ClearAlarmClick();

            UI.Operate(NormalStatic.Alarm, "Clear Alarm");
        }

        private void AlarmPanel_BuzzerOffClick(object sender, EventArgs e)
        {
            User_EFEM.BuzzerOff();

            IO_EFEM.EFEMStatusControl(StatusControl.BuzzerOff);
            UI.Operate(NormalStatic.Alarm, "Buzzer Off");
        }

        #endregion

        #region Account_Page

        private void AuthorityManage()
        {
            switch (HT.EFEM.Authority)
            {
                case AuthorityTable.Admin:
                    {
                        AlarmPanel.ClearAlarmEnable(true);
                        picAccount.Image = Properties.Resources.Admin;
                    }
                    break;

                case AuthorityTable.Engineer:
                    {
                        AlarmPanel.ClearAlarmEnable(true);
                        picAccount.Image = Properties.Resources.Engineer;
                    }
                    break;

                case AuthorityTable.Operator:
                    {
                        picAccount.Image = Properties.Resources.User;
                    }
                    break;

                case AuthorityTable.Null:
                    {

                        AlarmPanel.ClearAlarmEnable(false);
                        picAccount.Image = Properties.Resources.User;
                    }
                    break;
            }
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtInUser.Text) || string.IsNullOrEmpty(txtInPassword.Text))
            {
                MessageBox.Show("Please enter the correct ID and Password",
                                "Notice",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            string User = txtInUser.Text;
            string Password = txtInPassword.Text;
            string Condition = string.Format("{0}='{1}' and {2}='{3}'", SQLTableItem.Name, User, SQLTableItem.PassWord, Password);
            DataTable AccountDt = SQLite.ReadDataTable(SQLTable.Account, Condition);
            if (AccountDt.Rows.Count == 0)
            {
                txtInUser.Text = "";
                txtInPassword.Text = "";

                MessageBox.Show("Check that there is no such username or password error, please re-enter",
                                "Notice",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);




                HT.EFEM.Authority = AuthorityTable.Admin;
                for (int i = 1; i < Mainbtn.Length; i++)
                {
                    Mainbtn[i].Visible = true;
                }

                AuthorityManage();

                UI.Log(NormalStatic.System, NormalStatic.EFEM, SystemList.UserLogin, string.Format(":{0}{1}{2}", HT.EFEM.Authority, NormalStatic.UnderLine, labAccount.Text));

                UI.Operate(NormalStatic.System, SystemList.UserLogin.ToString());



                return;
            }
            else
            {
                labAccount.Text = User;
                txtInUser.Enabled = false;
                txtInUser.Text = "";
                txtInPassword.Enabled = false;
                txtInPassword.Text = "";
                btnLogIn.Enabled = false;
                btnLogOut.Enabled = true;

                HT.EFEM.Authority = ((AuthorityTable)Enum.Parse(typeof(AuthorityTable), AccountDt.Rows[0][2].ToString()));
                for (int i = 1; i < Mainbtn.Length; i++)
                {
                    Mainbtn[i].Visible = true;
                }

                AuthorityManage();

                UI.Log(NormalStatic.System, NormalStatic.EFEM, SystemList.UserLogin, string.Format(":{0}{1}{2}", HT.EFEM.Authority, NormalStatic.UnderLine, labAccount.Text));

                UI.Operate(NormalStatic.System, SystemList.UserLogin.ToString());
            }
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            if (User_Core.Busy)
            {
                string Busy_BG = "";

                if (User_Core.Robot1_BG.IsBusy)
                    Busy_BG = "Robot1 background process";
                if (User_Core.Robot2_BG.IsBusy)
                    Busy_BG = "Robot2 background process";
                if (User_Core.Device_BG.IsBusy)
                    Busy_BG = "Device background process";
                if (User_Core.Robot1_MappingReadyBG.IsBusy)
                    Busy_BG = "Robot1 mapping background process";
                if (User_Core.Robot2_MappingReadyBG.IsBusy)
                    Busy_BG = "Robot2 mapping background process";
                if (User_Core.InitialBG.IsBusy)
                    Busy_BG = "Initial background process";

                MessageBox.Show(string.Format("Warning：{0} is busy so it cannot be logged out", Busy_BG),
                                 "Notice",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Warning);
                return;
            }

            FnucSelect = MainFunc.Maxcnt;
            UI.Log(NormalStatic.System, NormalStatic.EFEM, SystemList.UserLogOut, string.Format(":{0}{1}{2}", HT.EFEM.Authority, NormalStatic.UnderLine, labAccount.Text));

            labAccount.Text = "";
            txtInUser.Text = "";
            txtInPassword.Text = "";
            txtInUser.Enabled = true;
            txtInPassword.Enabled = true;
            btnLogOut.Enabled = false;
            btnLogIn.Enabled = true;

            for (int i = 1; i < Mainbtn.Length; i++)
            {
                Mainbtn[i].Visible = false;
            }
            splButton.Panel1.Controls.Clear();

            HT.EFEM.Authority = AuthorityTable.Null;

            AuthorityManage();

            User_EFEM.CloseTSForm();

            UI.Operate(NormalStatic.System, SystemList.UserLogOut.ToString());
        }

        private void btnAccountAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAddName.Text) || string.IsNullOrEmpty(txtAddPassword.Text) || string.IsNullOrEmpty(txtAddAuthority.Text))
            {
                txtAddName.Text = "";
                txtAddPassword.Text = "";
                txtAddAuthority.SelectedIndex = -1;

                MessageBox.Show("Incorrect information input",
                                "Notice",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            string Name = txtAddName.Text;
            string condition = string.Format("Name = '{0}' ", Name);
            if (SQLite.ReadDataTableCount(SQLTable.Account, condition) > 0)
            {
                MessageBox.Show("Please re-enter the user account",
                                "Notice",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            string Password = txtAddPassword.Text;
            string Authority = txtAddAuthority.Text;
            string InsertValue = string.Format("('{0}','{1}','{2}')", Name, Password, Authority);
            SQLite.Insert(SQLTable.Account, InsertValue);
            btnRefresh.PerformClick();
            txtAddName.Text = "";
            txtAddPassword.Text = "";
            txtAddAuthority.SelectedIndex = -1;


            UI.Operate(NormalStatic.System, string.Format("Account Add :{0}", txtAddName.Text));
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            DataTable tempDt = SQLite.ReadDataTable(SQLTable.Account, string.Format("{0}!='{1}'", SQLTableItem.Authority, SQLTableItem.Admin));
            tempDt.Columns.RemoveAt(1);
            dgvAccount.ReadOnly = true;
            dgvAccount.DataSource = tempDt;

            UI.Operate(NormalStatic.System, "Refresh Account");
        }

        private void btnAccountDel_Click(object sender, EventArgs e)
        {
            if (dgvAccount.CurrentRow == null || dgvAccount.CurrentRow.Index == -1)
            {
                return;
            }

            string name = dgvAccount.Rows[dgvAccount.CurrentRow.Index].Cells[0].Value.ToString();

            DialogResult UserResult = MessageBox.Show(string.Format("Are you sure delete {0} account?", name),
                                      "Notice",
                                      MessageBoxButtons.YesNo,
                                      MessageBoxIcon.Error);

            if (UserResult == DialogResult.Yes)
            {
                SQLite.Delete(SQLTable.Account, string.Format("{0}= '{1}'", SQLTableItem.Name, name));
            }

            UI.Operate(NormalStatic.System, string.Format("Account delete :{0}", name));
            btnRefresh.PerformClick();
        }

        private void btnAuthSearch_Click(object sender, EventArgs e)
        {
            DataTable tempDt = SQLite.ReadDataTable(SQLTable.Authority, "1=1");
            dgvAuth.Columns.Clear();
            dgvAuth.DataSource = tempDt;
            DataGridViewComboBoxColumn comboboxColumn = new DataGridViewComboBoxColumn();
            comboboxColumn.Items.Add("Y");
            comboboxColumn.Items.Add("N");
            comboboxColumn.HeaderText = "Engineer";
            dgvAuth.Columns.Insert(1, comboboxColumn);
            comboboxColumn = new DataGridViewComboBoxColumn();
            comboboxColumn.Items.Add("Y");
            comboboxColumn.Items.Add("N");
            comboboxColumn.HeaderText = "Operator";
            dgvAuth.Columns.Insert(2, comboboxColumn);

            for (int row = 0; row < tempDt.Rows.Count; row++)
            {
                if (tempDt.Rows[row][2].ToString() == "Y")
                {
                    dgvAuth.Rows[row].Cells[1].Value = "Y";
                }
                else
                {
                    dgvAuth.Rows[row].Cells[1].Value = "N";
                }

                if (tempDt.Rows[row][4].ToString() == "Y")
                {
                    dgvAuth.Rows[row].Cells[2].Value = "Y";
                }
                else
                {
                    dgvAuth.Rows[row].Cells[2].Value = "N";
                }
            }

            dgvAuth.Columns.RemoveAt(7);
            dgvAuth.Columns.RemoveAt(6);
            dgvAuth.Columns.RemoveAt(5);
            dgvAuth.Columns.RemoveAt(4);
            dgvAuth.Columns.RemoveAt(3);
            dgvAuth.Columns[0].ReadOnly = true;
            dgvAuth.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;

            UI.Operate(NormalStatic.System, "Refresh Auth");

        }

        private void btnAuthEdit_Click(object sender, EventArgs e)
        {
            if (dgvAuth.Columns.Count == 0)
            {
                MessageBox.Show("Please check if there is no information.",
                                "Notice",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            string updatestr = "";
            for (int row = 0; row < dgvAuth.Rows.Count; row++)
            {
                updatestr += string.Format("update {0} set {1}='{2}' , {3}='{4}' where {5}='{6}';",
                                        SQLTableItem.Authority,
                                        SQLTableItem.Engineer,
                                        dgvAuth.Rows[row].Cells[1].Value.ToString(),
                                        SQLTableItem.Operator,
                                        dgvAuth.Rows[row].Cells[2].Value.ToString(),
                                        SQLTableItem.UIName,
                                        dgvAuth.Rows[row].Cells[0].Value.ToString());
            }
            SQLite.Multi_SetData(updatestr);
            MessageBox.Show("Update completed",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

            UI.Operate(NormalStatic.System, string.Format("Auth Update :{0}", updatestr));
        }

        #endregion

        #region Mike_Test

        //  System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//引用stopwatch物件

        static Stopwatch sw = new Stopwatch();

        public static void CalTimeStart()
        {
            sw.Reset();//碼表歸零

            sw.Start();//碼表開始計時
        }

        public static void CalTimeEnd()
        {

            sw.Stop();//碼錶停止

            //印出所花費的總豪秒數

            string result1 = sw.Elapsed.TotalMilliseconds.ToString();

            Console.Write(string.Format("\r\n{0}", result1));
            //  MessageBox.Show(result1);
        }

        #endregion

        #region JP Setting

        private void txtAlignerDegree_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            float Value_temp;
            if (float.TryParse(txtAlignerDegree.Text, out Value_temp))
            {
                if (Value_temp > (float)360)
                    Value_temp = (float)360;
                HT.Recipe.Aligner_Degree = float.Parse(Value_temp.ToString("f1"));
                UI.Log(NormalStatic.IO, NormalStatic.Config, SystemList.CommandParameter, string.Format("{0}:({1})", "Aligner_Degree", HT.Recipe.Aligner_Degree));
            }
            txtAlignerDegree.Text = HT.Recipe.Aligner_Degree.ToString("f1");
        }

        private void txtOCRDegree_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            float Value_temp;
            if (float.TryParse(txtOCRDegree.Text, out Value_temp))
            {
                if (Value_temp > (float)360)
                    Value_temp = (float)360;
                HT.Recipe.OCR_Degree = float.Parse(Value_temp.ToString("f1"));
                UI.Log(NormalStatic.IO, NormalStatic.Config, SystemList.CommandParameter, string.Format("{0}:({1})", "OCR_Degree", HT.Recipe.OCR_Degree));

            }
            txtOCRDegree.Text = HT.Recipe.OCR_Degree.ToString("f1");
        }

        private void clsParameter_MouseUp(object sender, MouseEventArgs e)
        {
            int select = clsParameter.SelectedIndex;
            bool value = clsParameter.GetItemChecked(select);
            switch (select)
            {
                case 0:
                    {
                        if (HT.Recipe.IsUseLower == false && value == true)
                        {
                            HT.Recipe.IsUseLower = value;
                            clsRobotSetting.SetItemChecked(2, value);
                        }

                        HT.Recipe.IsFlip = value;
                    }
                    break;

                case 1:
                    {

                        if ((HT.Recipe.IsOCR_Down || HT.Recipe.IsOCR_Up) && value == false)
                        {
                            HT.Recipe.IsOCR_Up = value;
                            HT.Recipe.IsOCR_Down = value;
                            clsParameter.SetItemChecked(2, false);
                            clsParameter.SetItemChecked(3, false);
                        }

                        HT.Recipe.IsAligner = value;
                    }
                    break;

                case 2:
                    {
                        if (value == true)
                        {
                            HT.Recipe.IsAligner = true;
                            clsParameter.SetItemChecked(1, true);
                        }
                        HT.Recipe.IsOCR_Up = value;
                        //HT.Recipe.IsOCR_Down = false;
                        //clsParameter.SetItemChecked(3, false);
                    }
                    break;

                case 3:
                    {
                        if (value == true)
                        {
                            HT.Recipe.IsAligner = true;
                            clsParameter.SetItemChecked(1, true);
                        }

                        HT.Recipe.IsOCR_Down = value;
                        //HT.Recipe.IsOCR_Up = false;
                        //clsParameter.SetItemChecked(2, false);
                    }
                    break;

                case 4:
                    {
                        HT.Recipe.IsOCR_Stage = value;
                    }
                    break;
            }
            UI.Log(NormalStatic.IO, NormalStatic.Config, SystemList.CommandParameter, string.Format("{0}:({1})", clsParameter.GetItemText(select), value));
        }

        private void clsRobotSetting_MouseUp(object sender, MouseEventArgs e)
        {
            int select = clsRobotSetting.SelectedIndex;
            bool value = clsRobotSetting.GetItemChecked(select);
            switch (select)
            {

                case 0:
                    {
                        if (HT.Recipe.IsUseStage2 == false)
                        {
                            clsRobotSetting.SetItemChecked(0, true);
                            break;
                        }
                        HT.Recipe.IsUseStage1 = value;
                    }
                    break;

                case 1:
                    {

                        if (HT.Recipe.IsUseStage1 == false)
                        {
                            clsRobotSetting.SetItemChecked(1, true);
                            break;
                        }
                        HT.Recipe.IsUseStage2 = value;
                    }
                    break;

                case 2:
                    {
                        if (HT.Recipe.IsUseUpper == false || (HT.Recipe.AutoMode == PJ_Type.LoadUnload && value == false))
                        {
                            clsRobotSetting.SetItemChecked(2, true);
                            break;
                        }

                        HT.Recipe.IsUseLower = value;
                    }
                    break;

                case 3:
                    {
                        if (HT.Recipe.IsUseLower == false || (HT.Recipe.AutoMode == PJ_Type.LoadUnload && value == false))
                        {
                            clsRobotSetting.SetItemChecked(3, true);
                            break;
                        }

                        HT.Recipe.IsUseUpper = value;
                    }
                    break;

            }
            UI.Log(NormalStatic.IO, NormalStatic.Config, SystemList.CommandParameter, string.Format("{0}:({1})", clsRobotSetting.GetItemText(select), value));
        }

        private void dgvLoadPortType_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Port_Index = e.RowIndex;
            if (Port_Index <= (int)IOLPDevice.CP10)
            {
                btnDummyWafer.Enabled = true;
                btnOMSIn.Enabled = false;
                btnOMSOut.Enabled = false;
            }
            else
            {
                btnDummyWafer.Enabled = false;
                btnOMSIn.Enabled = true;
                btnOMSOut.Enabled = true;
            }
        }

        private void btnRealwafer_Click(object sender, EventArgs e)
        {
            if (Port_Index == -1)
                return;
            if (Port_Index >= (int)IOLPDevice.MaxCnt)
                return;

            if (Port_Index >= (int)IOLPDevice.MP1)
            {
                if (dgvLoadPortType.Rows[Port_Index].Cells[1].Value.ToString() == MagazinePortType.OMS_Out.ToString())
                {
                    if (CheckMpTypeCnt(MagazinePortType.OMS_Out) <= 1)
                        return;
                }
                if (dgvLoadPortType.Rows[Port_Index].Cells[1].Value.ToString() == MagazinePortType.OMS_In.ToString())
                {
                    if (CheckMpTypeCnt(MagazinePortType.OMS_In) <= 1)
                        return;
                }
            }
            else
            {


                if (dgvLoadPortType.Rows[Port_Index].Cells[1].Value.ToString() == CassetterPortType.Dummy.ToString())
                {
                    if (CheckCpTypeCnt(CassetterPortType.Dummy) <= 1)
                        return;
                }

            }

            dgvLoadPortType.Rows[Port_Index].Cells[1].Value = MagazinePortType.Real.ToString();
            AppSetting.SaveSetting(string.Format("{0}_Type", (IOLPDevice)Port_Index), MagazinePortType.Real.ToString());
            UI.Operate(NormalStatic.Mode, string.Format("{0},{1}", (IOLPDevice)Port_Index, MagazinePortType.Real));
        }

        private void btnDummyWafer_Click(object sender, EventArgs e)
        {
            if (Port_Index == -1)
                return;
            if (Port_Index >= (int)IOLPDevice.MaxCnt)
                return;
            if (Port_Index > (int)IOLPDevice.CP10)
                return;



            if (dgvLoadPortType.Rows[Port_Index].Cells[1].Value.ToString() == CassetterPortType.Real.ToString())
            {
                if (CheckCpTypeCnt(CassetterPortType.Real) <= 1)
                    return;
            }
            HCT_EFEM.WirteExcel((int)IDExcelTable.VID, (int)VID_Item.CST1_PortType + Port_Index, (int)VID_Col.VID_Value, "Dummy Wafer");


            dgvLoadPortType.Rows[Port_Index].Cells[1].Value = CassetterPortType.Dummy.ToString();
            AppSetting.SaveSetting(string.Format("{0}_Type", (IOLPDevice)Port_Index), CassetterPortType.Dummy.ToString());
            UI.Operate(NormalStatic.Mode, string.Format("{0},{1}", (IOLPDevice)Port_Index, CassetterPortType.Dummy));
        }

        private void btnOMSIn_Click(object sender, EventArgs e)
        {
            if (Port_Index == -1)
                return;
            if (Port_Index >= (int)IOLPDevice.MaxCnt)
                return;
            if (Port_Index <= (int)IOLPDevice.CP10)
                return;

            if (dgvLoadPortType.Rows[Port_Index].Cells[1].Value.ToString() == MagazinePortType.OMS_Out.ToString())
            {
                if (CheckMpTypeCnt(MagazinePortType.OMS_Out) <= 1)
                    return;
            }
            if (dgvLoadPortType.Rows[Port_Index].Cells[1].Value.ToString() == MagazinePortType.Real.ToString())
            {
                if (CheckMpTypeCnt(MagazinePortType.Real) <= 1)
                    return;
            }
            HCT_EFEM.WirteExcel((int)IDExcelTable.VID, (int)VID_Item.MGZ1_PortType + (Port_Index - (int)IOLPDevice.MP1), (int)VID_Col.VID_Value, "OMS In");


            dgvLoadPortType.Rows[Port_Index].Cells[1].Value = MagazinePortType.OMS_In.ToString();
            AppSetting.SaveSetting(string.Format("{0}_Type", (IOLPDevice)Port_Index), MagazinePortType.OMS_In.ToString());
            UI.Operate(NormalStatic.Mode, string.Format("{0},{1}", (IOLPDevice)Port_Index, MagazinePortType.OMS_In));
        }

        private void btnOMSOut_Click(object sender, EventArgs e)
        {
            if (Port_Index == -1)
                return;
            if (Port_Index >= (int)IOLPDevice.MaxCnt)
                return;
            if (Port_Index <= (int)IOLPDevice.CP10)
                return;

            if (dgvLoadPortType.Rows[Port_Index].Cells[1].Value.ToString() == MagazinePortType.OMS_In.ToString())
            {
                if (CheckMpTypeCnt(MagazinePortType.OMS_In) <= 1)
                    return;
            }
            if (dgvLoadPortType.Rows[Port_Index].Cells[1].Value.ToString() == MagazinePortType.Real.ToString())
            {
                if (CheckMpTypeCnt(MagazinePortType.Real) <= 1)
                    return;
            }

            HCT_EFEM.WirteExcel((int)IDExcelTable.VID, (int)VID_Item.MGZ1_PortType + (Port_Index - (int)IOLPDevice.MP1), (int)VID_Col.VID_Value, "OMS Out");

            dgvLoadPortType.Rows[Port_Index].Cells[1].Value = MagazinePortType.OMS_Out.ToString();
            AppSetting.SaveSetting(string.Format("{0}_Type", (IOLPDevice)Port_Index), MagazinePortType.OMS_Out.ToString());

            UI.Operate(NormalStatic.Mode, string.Format("{0},{1}", (IOLPDevice)Port_Index, MagazinePortType.OMS_Out));
        }

        private void cboModeSetting_SelectedIndexChanged(object sender, EventArgs e)
        {

            PJ_Type Select = (PJ_Type)Enum.Parse(typeof(PJ_Type), cboModeSetting.Text);

            if (HT.Recipe.AutoMode != Select)
            {
                AppSetting.SaveSetting("Recipe_Mode", Select.ToString());
                HT.Recipe.AutoMode = Select;
                UI.Log(NormalStatic.IO, NormalStatic.Config, SystemList.CommandParameter, string.Format("{0}:({1})", "Mode", Select));
                UI.Operate(NormalStatic.Stage, "Sortering Button");
            }

            if (HT.Recipe.AutoMode == PJ_Type.LoadUnload)
            {
                HT.Recipe.IsUseLower = true;
                clsRobotSetting.SetItemChecked(2, true);
                HT.Recipe.IsUseUpper = true;
                clsRobotSetting.SetItemChecked(3, true);
            }
        }

        private int CheckCpTypeCnt(CassetterPortType type)
        {
            string _str = type.ToString();
            int _Cnt = 0;
            for (int i = (int)IOLPDevice.CP1; i < (int)IOLPDevice.MP1; i++)
            {
                if (_str == dgvLoadPortType.Rows[i].Cells[1].Value.ToString())
                    _Cnt += 1;
            }

            return _Cnt;
        }

        private int CheckMpTypeCnt(MagazinePortType type)
        {
            string _str = type.ToString();
            int _Cnt = 0;
            for (int i = (int)IOLPDevice.MP1; i < (int)IOLPDevice.MaxCnt; i++)
            {
                if (_str == dgvLoadPortType.Rows[i].Cells[1].Value.ToString())
                    _Cnt += 1;
            }
            return _Cnt;
        }

        #endregion

        #region Get/Set

        public EFEMStatus Ui_EFEMStatus
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_EFEMStatus = value; }));
                    return;
                }

                switch (value)
                {
                    case EFEMStatus.Unknown:
                    case EFEMStatus.SysCheck_Now:
                    case EFEMStatus.Init_Now:
                    case EFEMStatus.Ready_Now:
                    case EFEMStatus.Run_Now:
                        {
                            labEFEMStatut.BackColor = Color.Yellow;
                        }
                        break;

                    case EFEMStatus.SysCheck_Finish:
                    case EFEMStatus.Init_Finish:
                    case EFEMStatus.Ready_Finish:
                    case EFEMStatus.Run_Finish:
                        {
                            labEFEMStatut.BackColor = Color.LightGreen;
                        }
                        break;

                    case EFEMStatus.Power_Off:
                    case EFEMStatus.SysCheck_Fail:
                    case EFEMStatus.Init_Fail:
                    case EFEMStatus.Ready_Fail:
                    case EFEMStatus.Run_Fail:
                        {
                            labEFEMStatut.BackColor = Color.Red;
                        }
                        break;
                }

                labEFEMStatut.Text = value.ToString();
                HT.EFEM.Status = value;

                UserUnloader.SendCommand(string.Format("EFEMStatus,{0},", value));
            }
            get { return HT.EFEM.Status; }
        }

        public EFEMMode Ui_EFEMMode
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_EFEMMode = value; }));
                    return;
                }

                switch (value)
                {
                    case EFEMMode.Remote:
                        {
                            labEFEMMode.BackColor = Color.Yellow;
                        }
                        break;

                    case EFEMMode.Local:
                        {
                            labEFEMMode.BackColor = Color.LightGreen;
                        }
                        break;

                    case EFEMMode.Unknown:
                        {
                            labEFEMMode.BackColor = Color.Red;
                        }
                        break;
                }

                labEFEMMode.Text = value.ToString();
                HT.EFEM.Mode = value;
            }
            get { return HT.EFEM.Mode; }
        }

        #endregion

        #region EFEM status/Mode

        private void EFEMModeChange(EFEMMode mode)
        {
            if (InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate () { EFEMModeChange(mode); }));
                return;
            }

            Ui_EFEMMode = mode;


        }

        private void EFEMStatusChange(Adam6050_DI index, bool Result)
        {
            if (InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate () { EFEMStatusChange(index, Result); }));
                return;
            }

            if (!Result)
            {
                switch (index)
                {
                    case Adam6050_DI.Power:
                        {
                            if (InitPanel.Visible == false)
                            {
                                Ui_EFEMStatus = EFEMStatus.Power_Off;
                                User_EFEM.PowerOffDeviceDisconnect();
                            }
                            else
                            {
                                if (Ui_EFEMStatus != EFEMStatus.Init_Now)
                                {
                                    User_EFEM.PowerOnDeviceInitial();
                                    Ui_EFEMStatus = EFEMStatus.Init_Now;
                                }
                            }
                        }
                        break;
                }
            }
            else
            {

                switch (index)
                {

                    case Adam6050_DI.Power:
                        {
                            Ui_EFEMStatus = EFEMStatus.Init_Now;

                            if (InitPanel.Visible == false)
                            {
                                InitPanel = new Form_Initial(HT.EFEM.Again_Count);
                                InitPanel.FormClosed += new FormClosedEventHandler(InitPanel_FormClosed);
                                InitPanel.Show();
                                Thread.Sleep(6000);
                            }
                            User_EFEM.PowerOnDeviceInitial();
                        }
                        break;

                }
            }
        }

        #endregion

        #region Process_History

        private void btnHistorySearch_Click(object sender, EventArgs e)
        {
            List<string> conditionList = new List<string>();

            double datecount = dtpHistoryEnd.Value.AddDays(1).Date.Subtract(dtpHistoryStart.Value.Date).TotalDays;
            if (datecount > 31)
            {
                MessageBox.Show("The scope is too large, the rule is within 1 month");
                return;
            }

            if (txtHistoryMagazine.Text != "")
                conditionList.Add(string.Format("{0} = '{1}'", WaferInforTableItem.DesPortID, txtHistoryMagazine.Text));

            if (txtHistoryCarrier.Text != "")
                conditionList.Add(string.Format("{0} = '{1}'", WaferInforTableItem.DesSlotID, txtHistoryCarrier.Text));

            if (txtHistoryCassette.Text != "")
                conditionList.Add(string.Format("{0} = '{1}'", WaferInforTableItem.SocPortID, txtHistoryCassette.Text));

            if (txtHistoryWafer.Text != "")
                conditionList.Add(string.Format("{0} = '{4}' OR {1} = '{4}' OR {2} = '{4}' OR {3} = '{4}' ",
                                                WaferInforTableItem.SocSlotID_Up,
                                                WaferInforTableItem.SocSlotID_Down,
                                                WaferInforTableItem.SwapSlotID_Up,
                                                WaferInforTableItem.SwapSlotID_Down,
                                                txtHistoryWafer.Text));

            if (txtHistoryCJ.Text != "")
                conditionList.Add(string.Format("{0} = '{1}'", WaferInforTableItem.CJID, txtHistoryCJ.Text));

            conditionList.Add(string.Format("(({0} BETWEEN '{1}' AND '{2}') OR ({3} BETWEEN '{4}' AND '{5}'))",
                                        WaferInforTableItem.StartTime,
                                        dtpHistoryStart.Value.ToString("yyyy-MM-dd"),
                                        dtpHistoryEnd.Value.AddDays(1).ToString("yyyy-MM-dd"),
                                        WaferInforTableItem.EndTime,
                                        dtpHistoryStart.Value.ToString("yyyy-MM-dd"),
                                        dtpHistoryEnd.Value.AddDays(1).ToString("yyyy-MM-dd")));

            string condition = WT_Library.StringArrayCombine(conditionList.ToArray(), " and ");


            condition += string.Format("order by {0} desc", WaferInforTableItem.StartTime);
            DataTable historydata = SQLite.ReadDataTable(SQLTable.PJ_History, condition);

            if (historydata.Rows.Count == 0)
            {
                MessageBox.Show("No search any data", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                dgvHistoryWafer.DataSource = null;
                return;
            }

            dgvHistoryWafer.DataSource = historydata;

            UI.Operate(NormalStatic.Core, "Refresh_WaferHistory");
        }

        #endregion

 
        #region INI

        //Wayne 20190919

        private bool WithoutDeviceCheck()
        {
            //Robot
            bool Error = false;
            for (int i = 0; i < (int)DeviceWaferStatus.Maxcnt; i++)
            {
                if (dgvWaferStatus.Rows[i].Cells[2].Value.ToString() != WaferStatus.WithOut.ToString())
                    Error = true;
            }
            return Error;
        }

        private void btnUpdateStatus_Click(object sender, EventArgs e)
        {
            DialogResult myResult = MessageBox.Show("Are you Update now sensor status to Memory ?", "Warning",
                                                    MessageBoxButtons.YesNo,
                                                    MessageBoxIcon.Warning);
        }



        #endregion

        #region FFU

        #endregion

        #region Puge_Setting


        #endregion

        //Joanne 20220830
        private void LDTriggerAllHome(string ref_Command)
        {
            switch (ref_Command)
            {
                case "AllHome":
                    Ui_EFEMStatus = EFEMStatus.Init_Now;
                    if (User_Core.InitialBG.IsBusy == false)
                    {
                        User_Core.InitialBG.RunWorkerAsync();
                    }
                    break;
                case "SET_mode_auto":

                    Ui_EFEMStatus = EFEMStatus.Ready_Now;
                    if (InvokeRequired)
                    {
                        Invoke(new MethodInvoker(() =>
                        {
                            OperationChangeUI();
                        }));
                    }
                    else
                    {
                        OperationChangeUI();
                    }
                    break;
                case "SET_mode_manual":

                    Ui_EFEMStatus = EFEMStatus.Run_Fail;
                    if (InvokeRequired)
                    {
                        Invoke(new MethodInvoker(() =>
                        {
                            OperationChangeUI();
                        }));
                    }
                    else
                    {
                        OperationChangeUI();
                    }
                    break;
                case "JobInfo":
                    Ui_EFEMStatus = EFEMStatus.Run_Now;
                    User_Core.Robot1_BG.RunWorkerAsync();
                    break;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

            //User_EFEM.BarcodeOpen();

            UserCore.ls_JobInfo.Add(new UserCore.cs_JobInfo
            {
                SourcePort = 1,
                SourceSlot = 1,
                CurrentStep = SQLWaferInforStep.GetEQ_Send
            });
            User_Core.Robot1_BG.RunWorkerAsync();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            User_EFEM.BarcodeClose();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            User_EFEM.BarcodeOpen();
        }

        private void rb_rf_CheckedChanged(object sender, EventArgs e)
        {
            if (!rb_rf.Checked)
                return;
            IDreader = readkind.RFID;
            AppSetting.SaveSetting("IDreader", NormalStatic.False);
        }

        private void rb_hand_CheckedChanged(object sender, EventArgs e)
        {
            if (!rb_hand.Checked)
                return;
            IDreader = readkind.Hand;
            AppSetting.SaveSetting("IDreader", NormalStatic.True);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UserUnloader.SendCommand(string.Format("LPPut,{0},{1}", 0, 1));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            UserUnloader.SendCommand(string.Format("LPPut,{0},{1}", 0, 24));
        }

        private void button6_Click(object sender, EventArgs e)
        {
            UserUnloader.SendCommand(string.Format("JobComplete,,"));
        }
    }
}
