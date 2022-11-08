using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace HirataMainControl
{
    public partial class UserCore : UserControl
    {
        public delegate void Mode_Change_Event(EFEMMode Mode);
        public event Mode_Change_Event Change_EFEM_Mode;

        public static List<cs_JobInfo> ls_JobInfo = new List<cs_JobInfo>();
        public static bool AutoRunning = false;
        public static int CurrentJobIndex = 0;

        #region Object
        private object Device_Lock = new object();
        private object Robot1MappingLock = new object();
        private object Robot2MappingLock = new object();
        public HCT_EFEM EFEM;
        public Form_AutoJob CreateJob = new Form_AutoJob();
        #endregion

        #region BG

        public BackgroundWorker InitialBG = new BackgroundWorker();
        public BackgroundWorker Robot1_MappingReadyBG = new BackgroundWorker();
        public BackgroundWorker Robot2_MappingReadyBG = new BackgroundWorker();
        public BackgroundWorker Robot1_BG = new BackgroundWorker();
        public BackgroundWorker Robot2_BG = new BackgroundWorker();
        public BackgroundWorker Device_BG = new BackgroundWorker();

        #endregion

        #region Queue/List

        private List<SQLWaferInforStep>[] Carrier_Step = new List<SQLWaferInforStep>[(int)Core_Loop.MaxCnt];
        private List<SQLWaferInforStep>[] Wafer_Step = new List<SQLWaferInforStep>[(int)Core_Loop.MaxCnt];

        //private BlockQueue<SQLWaferInforStep[]>[] Robot_Step = new BlockQueue<SQLWaferInforStep[]>[2];

        private BlockQueue<string>[] Robot_Queue = new BlockQueue<string>[2];
        private BlockQueue<string> Device_Queue = new BlockQueue<string>();
        private BlockQueue<string> Interrupt_Queue = new BlockQueue<string>();

        #endregion

        CmdStruct Robot1_Data = new CmdStruct();
        CmdStruct Robot2_Data = new CmdStruct();
        CmdStruct Device_Data = new CmdStruct();

        #region Timeout
        private int AutoTimeoutCount = 0;
        private const int AUTO_TIMEOUT = 120;
        private const int STAGE_TIMEOUTOUT = 60;
        private const int ROBOT_TIMEOUT = 60;
        private const int IOLP_TIMEOUT = 60;
        private const int ALIGNER_TIMEOUT = 60;

        #endregion

        #region Variable

        //private bool Robot1.NowArm;
        //private string SourceDevice;
        // private int SourecSlot;
        // private string DestinationDevice;
        // private int DestinationSlot;
        // private string Condition; 

        #endregion

        #region Get/Set

        public bool Busy
        {
            set;
            get;
        }

        private string EFEM_Work
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { EFEM_Work = value; }));
                    return;
                }

                labEFEMWork.Text = value;
            }
        }

        private string Robot1_Work
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Robot1_Work = value; }));
                    return;
                }

                labRobot1Work.Text = value;
            }
        }

        private string Robot2_Work
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Robot2_Work = value; }));
                    return;
                }

                labRobot2Work.Text = value;
            }
        }

        #endregion

        //Joanne 20201009 Add Start
        public bool IsSetJobComplete_RB1;
        public bool IsSetJobComplete_RB2;
        //Joanne 20201009 Add End

        public string NowR1StepForLog = "";
        public string NowR2StepForLog = "";

        #region Initial

        public UserCore()
        {
            InitializeComponent();
        }

        public void Initial(HCT_EFEM _EFEM)
        {
            EFEM = _EFEM;
            EFEM.Initial();      

            //Mapping
            Robot1_MappingReadyBG.DoWork += new DoWorkEventHandler(this.Robot1_Ready_DoWork);
            Robot1_MappingReadyBG.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.CompletedBG);

            //Initial
            InitialBG.DoWork += new DoWorkEventHandler(Initial_DoWork);
            InitialBG.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.CompletedBG);

            //Robot1
            Robot_Queue[0] = new BlockQueue<string>();
            Robot1_BG.DoWork += new DoWorkEventHandler(this.Robot1_DoWork);
            Robot1_BG.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.CompletedBG);

            // Refresh_StatusItem();
            Refresh_System();

            for (int i = 0; i < (int)em_JobInfo.Finish; i++)
            {
                dgvQueuePJ.Columns.Add(((em_JobInfo)i).ToString(), ((em_JobInfo)i).ToString());
            }
        }

        #endregion

        #region BG_4

        #region Robot1

        private void Robot1_DoWork(object sender, DoWorkEventArgs e)
        {
            string Robot1_Deq = "";
            AutoTimeoutCount = 0;
            Refresh_System();

            EFEM.Robot_RecQueue[0].Clear();
            Robot_Queue[0].Clear();
            Interrupt_Queue.Clear();

            EFEM.ClearLoaderBarcode();

            UI.Log(NormalStatic.System, NormalStatic.Core, SystemList.ProgramOpen, "Robot1");

            if (EFEM.CheckRobotArmSafety(0, SocketCommand.WaferGet) == false)
            {
                e.Result = string.Format("{0},{1},{2}", NormalStatic.Auto, NormalStatic.False, "Robot Arm Extend");
                Robot1_Work = "TBD";
                return;
            }

            CurrentJobIndex = 0;

            while (true)
            {
                SpinWait.SpinUntil(() => false, 300);

                if(HT.EFEM.Status == EFEMStatus.Run_Now) 
                {
                }
                else 
                {
                    //UI.Log(NormalStatic.System, NormalStatic.Core, SystemList.CommandParameter, "Job Stop");
                    //e.Result = string.Format("{0},{1},{2}", NormalStatic.Auto, NormalStatic.False, "Job Stop");
                    //Robot1_Work = "TBD";
                    //return;
                }

                if (CurrentJobIndex >= ls_JobInfo.Count) //Finish or list 異常
                {
                    CurrentJobIndex = 0;
                    UI.Log(NormalStatic.System, NormalStatic.Core, SystemList.CommandParameter, "Job Finish");
                    e.Result = string.Format("{0},{1},{2}", NormalStatic.Auto, NormalStatic.True, "Job Finsih");
                    Robot1_Work = "TBD";
                    return;
                }

               if (EFEM.GetAdamDIValue(1, 4) == true && EFEM.GetAdamDOValue(1, 3) == false && HT.ReadBarcodeing == false)
                {
                    EFEM.BarcodeOpen();
                    HT.ReadBarcodeing = true;
                    HT.EQ_BarcodeReadTime = DateTime.Now;
                }

               if(EFEM.GetAdamDIValue(1, 4) == false && EFEM.GetAdamDOValue(1,3) == true) 
                {
                    EFEM.SetAdamDo(1, 3, "0");
                }

                if (HT.ReadBarcodeing && (DateTime.Now - HT.EQ_BarcodeReadTime).TotalSeconds > 10) //Fail 彈跳視窗
                {
                    EFEM.BuzzerOn();
                    EFEM.BarcodeClose();
                    EFEM.GetSetEQ_Barcode = EFEM.TriggerBarcodeFailUI();
                    EFEM.BuzzerOff();
                    HT.ReadBarcodeing = false;
                }


                SQLWaferInforStep CurrentSetp = ls_JobInfo[CurrentJobIndex].CurrentStep;

                switch (CurrentSetp)
                {
                    case SQLWaferInforStep.GetLP_Send:
                        if (EFEM.RobotBusy(0) == false)
                        {
                            int PortIdx = ls_JobInfo[CurrentJobIndex].SourcePort;
                            int Slot = ls_JobInfo[CurrentJobIndex].SourceSlot;

                            if (EFEM.Robot_ArmPresence(0, false) == WaferStatus.WithOut  && EFEM.LP_SlotData(PortIdx - 1)[Slot - 1] == 1)
                            {
                                ls_JobInfo[CurrentJobIndex].CurrentStep = SQLWaferInforStep.GetLP_Wait;
                                Robot1_Data.obj = NormalStatic.Robot;
                                Robot1_Data.port = 0;
                                Robot1_Data.Parameter = string.Format("{0},{1},{2}", false, string.Format("LP{0}", PortIdx), Slot);
                                Robot1_Data.command = SocketCommand.WaferGet;
                                EFEM.Command_EnQueue(Robot1_Data);
                            }
                        }
                        break;
                    case SQLWaferInforStep.PutAL_Send:
                        if (EFEM.RobotBusy(0) == false)
                        {
                            if (EFEM.Robot_ArmPresence(0, false) == WaferStatus.With)
                            {
                                if (ls_JobInfo[CurrentJobIndex].NeedAligner == false)
                                {
                                    ls_JobInfo[CurrentJobIndex].CurrentStep = SQLWaferInforStep.PutLP_Send;//Wayne Test for cycle
                                }
                                else
                                {
                                    if (EFEM.AlignerBusy(0) == false && EFEM.AlignerPresence(0) == WaferStatus.WithOut && EFEM.AlignerUnitStatus(0) == AlignerStatus.Home)
                                    {
                                        ls_JobInfo[CurrentJobIndex].CurrentStep = SQLWaferInforStep.PutAL_Wait;
                                        Robot1_Data.obj = NormalStatic.Robot;
                                        Robot1_Data.port = 0;
                                        Robot1_Data.Parameter = string.Format("{0},{1},{2}", false, string.Format("Aligner{0}", 1), 1);
                                        Robot1_Data.command = SocketCommand.WaferPut;
                                        EFEM.Command_EnQueue(Robot1_Data);
                                    }
                                }
                            }
                        }
                        break;
                    case SQLWaferInforStep.Alinger_Send:
                        if (EFEM.AlignerBusy(0) == false && EFEM.AlignerPresence(0) == WaferStatus.With)
                        {
                            ls_JobInfo[CurrentJobIndex].CurrentStep = SQLWaferInforStep.ReadWaferID_Send;
                            Robot1_Data.obj = NormalStatic.Aligner;
                            Robot1_Data.port = 0;
                            Robot1_Data.command = SocketCommand.Alignment;
                            EFEM.Command_EnQueue(Robot1_Data);
                        }
                        break;
                    case SQLWaferInforStep.ReadWaferID_Send:
                        if (EFEM.AlignerBusy(0) == false && EFEM.AlignerPresence(0) == WaferStatus.With)
                        {
                            if (ls_JobInfo[CurrentJobIndex].UseOCR == false)
                            {
                                ls_JobInfo[CurrentJobIndex].CurrentStep = SQLWaferInforStep.GetAL_Send;
                            }
                            else
                            {
                                ls_JobInfo[CurrentJobIndex].CurrentStep = SQLWaferInforStep.ReadWafer_Wait;
                                Robot1_Data.obj = NormalStatic.OCRReader;
                                Robot1_Data.port = 0;
                                Robot1_Data.command = SocketCommand.Read;
                                EFEM.Command_EnQueue(Robot1_Data);
                            }
                        }
                        break;
                    case SQLWaferInforStep.GetAL_Send:
                        if (EFEM.RobotBusy(0) == false)
                        {
                            if (EFEM.Robot_ArmPresence(0, false) == WaferStatus.WithOut && EFEM.AlignerBusy(0) == false && EFEM.AlignerPresence(0) == WaferStatus.With)
                            {
                                ls_JobInfo[CurrentJobIndex].CurrentStep = SQLWaferInforStep.GetAL_Wait;
                                Robot1_Data.obj = NormalStatic.Robot;
                                Robot1_Data.port = 0;
                                Robot1_Data.Parameter = string.Format("{0},{1},{2}", false, string.Format("Aligner{0}", 1), 1);
                                Robot1_Data.command = SocketCommand.WaferGet;
                                EFEM.Command_EnQueue(Robot1_Data);
                            }
                        }
                        break;
                    case SQLWaferInforStep.PutEQ_Send:
                        if (EFEM.RobotBusy(0) == false)
                        {
                            if (EFEM.Robot_ArmPresence(0, false) == WaferStatus.With)//需要增加Wait EQ 要板訊號
                            {
                                ls_JobInfo[CurrentJobIndex].CurrentStep = SQLWaferInforStep.PutEQ_Wait;
                                Robot1_Data.obj = NormalStatic.Robot;
                                Robot1_Data.port = 0;
                                Robot1_Data.Parameter = string.Format("{0},{1},{2}", false, string.Format("EQ{0}", 1), 1);
                                Robot1_Data.command = SocketCommand.WaferPut;
                                EFEM.Command_EnQueue(Robot1_Data);
                            }
                        }
                        break;
                    case SQLWaferInforStep.Finish:
                        CurrentJobIndex++;
                        break;
                    case SQLWaferInforStep.PutLP_Send:
                        if (EFEM.RobotBusy(0) == false)
                        {
                            int PortIdx = ls_JobInfo[CurrentJobIndex].SourcePort;
                            int Slot = ls_JobInfo[CurrentJobIndex].SourceSlot;

                            if (EFEM.Robot_ArmPresence(0, false) == WaferStatus.With && EFEM.LP_SlotData(PortIdx - 1)[Slot - 1] == 0)
                            {
                                ls_JobInfo[CurrentJobIndex].CurrentStep = SQLWaferInforStep.GetLP_Wait;
                                Robot1_Data.obj = NormalStatic.Robot;
                                Robot1_Data.port = 0;
                                Robot1_Data.Parameter = string.Format("{0},{1},{2}", false, string.Format("LP{0}", PortIdx), Slot);
                                Robot1_Data.command = SocketCommand.WaferPut;
                                EFEM.Command_EnQueue(Robot1_Data);
                            }
                        }
                        break;

                    case SQLWaferInforStep.AL_Home:
                        if (EFEM.AlignerBusy(0) == false)
                        {
                            ls_JobInfo[CurrentJobIndex].CurrentStep = SQLWaferInforStep.PutLP_Send; //Wayne Test
                            Robot1_Data.obj = NormalStatic.Aligner;
                            Robot1_Data.port = 0;
                            Robot1_Data.command = SocketCommand.Home;
                            EFEM.Command_EnQueue(Robot1_Data);
                        }
                        break;
                    case SQLWaferInforStep.AL_GetStatus:
                        if (EFEM.AlignerBusy(0) == false)
                        {
                            ls_JobInfo[CurrentJobIndex].CurrentStep = SQLWaferInforStep.Alinger_Send;
                            Robot1_Data.obj = NormalStatic.Aligner;
                            Robot1_Data.port = 0;
                            Robot1_Data.command = SocketCommand.GetStatus;
                            EFEM.Command_EnQueue(Robot1_Data);
                        }
                        break;
                    case SQLWaferInforStep.GetEQ_Send:
                        if (EFEM.RobotBusy(0) == false)
                        {
                            if (EFEM.Robot_ArmPresence(0, false) == WaferStatus.WithOut && EFEM.GetAdamDIValue(1,1) == true)//需要增加Wait EQ 要板訊號
                            {
                                ls_JobInfo[CurrentJobIndex].CurrentStep = SQLWaferInforStep.PutEQ_Wait;
                                Robot1_Data.obj = NormalStatic.Robot;
                                Robot1_Data.port = 0;
                                Robot1_Data.Parameter = string.Format("{0},{1},{2}", false, string.Format("EQ{0}", 1), 1);
                                Robot1_Data.command = SocketCommand.WaferGet;
                                EFEM.Command_EnQueue(Robot1_Data);
                            }
                        }
                        break;
                }
            }
        }

        private bool Robot1_WaferGetPut(string Obj, int slot, bool LowerUseArm, SocketCommand command)
        {
            switch (Obj)
            {
                case "Stage1":
                case "Stage2":
                    {
                        //if (!WaitStageRobot1(STAGE_TIMEOUTOUT, 0))
                        //    return false;
                    }
                    break;

                case "Aligner1":
                    {
                        if (!WaitAligner(ALIGNER_TIMEOUT, 0))
                            return false;
                    }
                    break;


                case "CP1":
                case "CP2":
                case "CP3":
                case "CP4":
                case "CP5":
                case "CP6":
                case "CP7":
                case "CP8":
                case "CP9":
                case "CP10":
                    {
                        if (!WaitCassetteport(IOLP_TIMEOUT, Convert.ToInt16(Obj.Substring(NormalStatic.CstPort.Length, Obj.Length - NormalStatic.CstPort.Length)) - 1))
                            return false;
                    }
                    break;
            }

            Robot1_Data.obj = NormalStatic.Robot;
            Robot1_Data.port = 0;
            Robot1_Data.Parameter = string.Format("{0},{1},{2}", LowerUseArm, Obj, slot);
            Robot1_Data.command = command;

            EFEM.Command_EnQueue(Robot1_Data);

            if (EFEM.Robot_RecQueue[0].DeQueue())
            {
                return true;
            }
            return false;
        }

        private bool Robot1_Auto_Command(SocketCommand command, string dest)
        {
            Robot1_Data.obj = NormalStatic.Robot;
            Robot1_Data.port = 0;
            Robot1_Data.command = command;
            Robot1_Data.Parameter = dest;

            EFEM.Command_EnQueue(Robot1_Data);

            if (EFEM.Robot_RecQueue[0].DeQueue())
            {
                return true;
            }
            return false;
        }

        #endregion





        #region Robot1_Ready

        private void Robot1_Ready_DoWork(object sender, DoWorkEventArgs e)
        {
            Refresh_System();
            EFEM.Robot_RecQueue[0].Clear();

            Interrupt_Queue.Clear();

            #region Robot1 Home

            if (Robot1_Command(SocketCommand.InitialHome) == false)
            {
                e.Result = string.Format("{0},{1}", NormalStatic.Ready, NormalStatic.False);
                return;
            }

            if (!WaitAutoRobot1(ROBOT_TIMEOUT))
            {
                e.Result = string.Format("{0},{1}", NormalStatic.Ready, NormalStatic.False);
                return;
            }

            #endregion

            #region Robot1 Mapping

            for (int i = 0; i < HCT_EFEM.CassetteCount; i++)
            {
            }

            #endregion

            #region Robot1 Home

            if (Robot1_Command(SocketCommand.InitialHome) == false)
            {
                e.Result = string.Format("{0},{1}", NormalStatic.Ready, NormalStatic.False);
                return;
            }

            if (!WaitAutoRobot1(ROBOT_TIMEOUT))
            {
                e.Result = string.Format("{0},{1}", NormalStatic.Ready, NormalStatic.False);
                return;
            }

            #endregion

            Robot1_Work = "TBD";
            e.Result = string.Format("{0},{1}", NormalStatic.Ready, NormalStatic.True);
        }

        private bool Robot1_Ready_Command(SocketCommand command, bool Iswait, string dest)
        {
            Robot1_Data.obj = NormalStatic.Robot;
            Robot1_Data.port = 0;
            Robot1_Data.command = command;
            Robot1_Data.Parameter = dest;

            EFEM.Command_EnQueue(Robot1_Data);

            if (EFEM.Robot_RecQueue[0].DeQueue())
            {
                Robot1_Work = string.Format("{0},{1}", command, dest);

                if (Iswait)
                {
                    if (!WaitAutoRobot1(ROBOT_TIMEOUT))
                        return false;
                }
                return true;
            }
            return false;
        }

        #endregion

        #region Initial

        private void Initial_DoWork(object sender, DoWorkEventArgs e)
        {
            Refresh_System();
            EFEM_Work = NormalStatic.InitialDevice;
            EFEM.Robot_RecQueue[0].Clear();
            EFEM.Aligner_RecQueue.Clear();
            Interrupt_Queue.Clear();

            //Robot Home
            Robot1_Data.obj = NormalStatic.Robot;
            Robot1_Data.port = 0;
            Robot1_Data.command = SocketCommand.Home;
            EFEM.Command_EnQueue(Robot1_Data);
            SpinWait.SpinUntil(() => false, 1000);

            //Check Robot Home Finish
            DateTime cmdTime = DateTime.Now;
            bool TimeOut = false;
            while (true)
            {
                SpinWait.SpinUntil(() => false, 300);
                if ((DateTime.Now - cmdTime).TotalSeconds > 60)
                {
                    TimeOut = true;
                }
                if (EFEM.RobotBusy(0) == false && EFEM.Robot_NowPosition(0) == RobotPosition.Home)
                {
                    break;
                }
            }

            if (TimeOut)
            {
                e.Result = string.Format("{0},{1},{2}", NormalStatic.InitialDevice, NormalStatic.False, "Robot Home Timeout");
            }

            EFEM.SetAdamDo(1, 3, "0");

            //Other ResetError
            for (int i = 0; i < EFEM.LP.Length; i++)
            {
                Robot1_Data.obj = NormalStatic.LP;
                Robot1_Data.port = i;
                Robot1_Data.command = SocketCommand.ResetError;
                EFEM.Command_EnQueue(Robot1_Data);
            }
            //Robot1_Data.obj = NormalStatic.Aligner;
            //Robot1_Data.port = 0;
            //Robot1_Data.command = SocketCommand.ResetError;
            //EFEM.Command_EnQueue(Robot1_Data);

            SpinWait.SpinUntil(() => false, 1000);

            //Check Reset Error Finish
            cmdTime = DateTime.Now;
            TimeOut = false;
            while (true)
            {
                SpinWait.SpinUntil(() => false, 300);
                if ((DateTime.Now - cmdTime).TotalSeconds > 10)
                {
                    TimeOut = true;
                }
                if (EFEM.LP[0].Ui_Busy == false && EFEM.LP[1].Ui_Busy == false)
                {
                    break;
                }
            }

            if (TimeOut)
            {
                e.Result = string.Format("{0},{1},{2}", NormalStatic.InitialDevice, NormalStatic.False, "Reset Error fail");
            }

            //Other Home
            for (int i = 0; i < EFEM.LP.Length; i++)
            {
                Robot1_Data.obj = NormalStatic.LP;
                Robot1_Data.port = i;
                Robot1_Data.command = SocketCommand.Home;
                EFEM.Command_EnQueue(Robot1_Data);
            }
            //Robot1_Data.obj = NormalStatic.Aligner;
            //Robot1_Data.port = 0;
            //Robot1_Data.command = SocketCommand.Home;
            //EFEM.Command_EnQueue(Robot1_Data);

            SpinWait.SpinUntil(() => false, 1000);

            //Check Home Finish 
            cmdTime = DateTime.Now;
            TimeOut = false;
            while (true)
            {
                SpinWait.SpinUntil(() => false, 300);
                if ((DateTime.Now - cmdTime).TotalSeconds > 10)
                {
                    TimeOut = true;
                }
                if (EFEM.LP[0].Ui_Busy == false &&
                    EFEM.LP[0].Ui_LoadStatus == LPPosition.Unload &&
                     EFEM.LP[1].Ui_Busy == false &&
                     EFEM.LP[1].Ui_LoadStatus == LPPosition.Unload )
                     //EFEM.Aligner[0].Ui_Busy == false &&
                     //EFEM.Aligner[0].Ui_Status == AlignerStatus.Home)
                {
                    break;
                }
            }

            if (TimeOut)
            {
                e.Result = string.Format("{0},{1},{2}", NormalStatic.InitialDevice, NormalStatic.False, "Home fail");
            }
            e.Result = string.Format("{0},{1},{2}", NormalStatic.InitialDevice, NormalStatic.True, "Home OK");
            EFEM.ClearLoaderBarcode();

        }

        private bool Robot1_Command(SocketCommand command)
        {
            Robot1_Data.obj = NormalStatic.Robot;
            Robot1_Data.port = 0;
            Robot1_Data.command = command;
            Robot1_Work = command.ToString();

            EFEM.Command_EnQueue(Robot1_Data);

            if (EFEM.Robot_RecQueue[0].DeQueue())
            {
                return true;
            }
            return false;
        }

        private bool Alinger_Command(SocketCommand command, int port, string paramater)
        {
            lock (Device_Lock)
            {
                Device_Data.obj = NormalStatic.Aligner;
                Device_Data.port = port;
                Device_Data.command = command;
                Device_Data.Parameter = paramater;

                EFEM.Command_EnQueue(Device_Data);

                if (EFEM.Aligner_RecQueue.DeQueue(10000))
                {
                    UI.Log(NormalStatic.System, NormalStatic.Core, SystemList.ProgramOpen, string.Format("Aligner Interlock True"));
                    return true;
                }
                UI.Log(NormalStatic.System, NormalStatic.Core, SystemList.ProgramOpen, string.Format("Aligner Interlock false"));
                return false;
            }
        }



        #endregion

        private void CompletedBG(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                Refresh_System();
                string[] Result = e.Result.ToString().Split(',');
                switch (Result[0])
                {
                    case NormalStatic.Auto:
                        {
                            //Robot_Queue[0].Clear();
                            //Robot_Queue[1].Clear();

                            if ((HT.Recipe.AutoMode != PJ_Type.Sortering && Robot1_BG.IsBusy == false && Robot2_BG.IsBusy == false)
                              || (HT.Recipe.AutoMode == PJ_Type.Sortering && Robot1_BG.IsBusy == false))
                            {
                                EFEM.ClearLoaderBarcode();

                                if (Result[1] == NormalStatic.True)
                                {
                                    IO_EFEM.EFEMStatusControl(StatusControl.Idle);

                                    UserUnloader.SendCommand(string.Format("JobComplete,,"));

                                    if (HT.EFEM.Mode == EFEMMode.Remote)
                                    {
                                        #region Remote

                                        //if (Robot1_BG.IsBusy == false)
                                        //    Robot1_BG.RunWorkerAsync();
                                        //if (Robot2_BG.IsBusy == false)
                                        //    Robot2_BG.RunWorkerAsync();

                                        UI.Log(NormalStatic.System, NormalStatic.EFEM, SystemList.ProgramClose, NormalStatic.Auto);

                                        #endregion
                                    }
                                    else
                                    {
                                        if (HT.EFEM.DryRunMode && (HT.EFEM.DryRunTotalCount > HT.EFEM.DryRunNowCount))
                                        {
                                            #region Local + 千傳
                                            HT.EFEM.DryRunNowCount++;

                                            labNowCount.Text = HT.EFEM.DryRunNowCount.ToString();

                                            switch (HT.Recipe.AutoMode)
                                            {
                                                case PJ_Type.Load:
                                                    HT.Recipe.AutoMode = PJ_Type.Unload;
                                                    break;

                                                case PJ_Type.Unload:
                                                    HT.Recipe.AutoMode = PJ_Type.Load;
                                                    break;

                                                case PJ_Type.LoadUnload:
                                                    {
                                                        SQLite.SetSwapTest(WaferInforTableItem.SocSlot, WaferInforTableItem.SwapSlot, "1=1");
                                                        SQLite.SetSwapTest(WaferInforTableItem.SocPort, WaferInforTableItem.SwapPort, "1=1");
                                                    }
                                                    break;

                                                case PJ_Type.Sortering:
                                                    {
                                                        SQLite.SetSwapTest(WaferInforTableItem.SocSlot, WaferInforTableItem.DesSlot, "1=1");
                                                        SQLite.SetSwapTest(WaferInforTableItem.SocPort, WaferInforTableItem.DesPort, "1=1");
                                                    }
                                                    break;
                                            }

                                            SQLite.CopyWaferInfoToHistory(SQLTable.PJ_Pool, string.Format("{0}= '{2}' and {1}= '{2}'", WaferInforTableItem.WaferStatus, WaferInforTableItem.CarrierStatus, SQLWaferInforStep.Finish));
                                            SQLite.LimitWaferInfoToHistory(SQLTable.PJ_History, 100000);

                                            Robot1_BG.RunWorkerAsync();
                                            Robot2_BG.RunWorkerAsync();

                                            #endregion
                                        }
                                        else if (HT.EFEM.IsContinue) //Walson追加
                                        {
                                            #region Continue完成後, 若PJ自動切換成Remote模式

                                            HT.EFEM.IsContinue = false;

                                            SQLite.CopyWaferInfoToHistory(SQLTable.PJ_Pool, string.Format("{0}= '{2}' or {1}= '{2}'", WaferInforTableItem.WaferStatus, WaferInforTableItem.CarrierStatus, SQLWaferInforStep.Finish));
                                            SQLite.LimitWaferInfoToHistory(SQLTable.PJ_History, 100000);

                                            Change_EFEM_Mode(EFEMMode.Remote);

                                            if (Robot1_BG.IsBusy == false)
                                                Robot1_BG.RunWorkerAsync();
                                            if (Robot2_BG.IsBusy == false)
                                                Robot2_BG.RunWorkerAsync();

                                            UI.Log(NormalStatic.System, NormalStatic.EFEM, SystemList.ProgramClose, NormalStatic.Auto);

                                            #endregion
                                        }
                                        else
                                        {
                                            var data = ls_JobInfo.GroupBy(x => x.SourcePort);
                                            int TempCount = 0;
                                            foreach (var Port in data)
                                            {
                                                Robot1_Data.obj = NormalStatic.LP;
                                                Robot1_Data.port = Convert.ToInt32(Port.Key.ToString()) - 1;
                                                Robot1_Data.command = SocketCommand.Unload;
                                                EFEM.Command_EnQueue(Robot1_Data);
                                                TempCount++;
                                                Application.DoEvents();
                                            }
                                            UI.AutoButton(EFEMStatus.Run_Finish, Result[1]);

                                            SQLite.CopyWaferInfoToHistory(SQLTable.PJ_Pool, string.Format("{0}= '{2}' or {1}= '{2}'", WaferInforTableItem.WaferStatus, WaferInforTableItem.CarrierStatus, SQLWaferInforStep.Finish));
                                            UI.Log(NormalStatic.System, NormalStatic.EFEM, SystemList.ProgramClose, NormalStatic.Cycle);
                                        }
                                    }

                                    //Joanne 20201009 Add Start
                                    IsSetJobComplete_RB1 = false;
                                    IsSetJobComplete_RB2 = false;
                                    //Joanne 20201009 Add End

                                }
                                else
                                {
                                    UI.AutoButton(EFEMStatus.Run_Finish, Result[1]);
                                    UI.Alarm(NormalStatic.EFEM, ErrorList.ProcessError, Result[2]);
                                }
                            }
                        }
                        break;

                    case NormalStatic.Ready:
                        {
                            if (Robot1_MappingReadyBG.IsBusy == false && Robot2_MappingReadyBG.IsBusy == false)
                            {
                                if (HT.EFEM.Status == EFEMStatus.Continue_Now)
                                {
                                    HT.continueFlag = false; //Wayne 20210922
                                    UI.AutoButton(EFEMStatus.Continue_Finish, Result[1]);
                                }
                                else
                                    UI.AutoButton(EFEMStatus.Ready_Finish, Result[1]);

                                if (Result[1] == NormalStatic.True)
                                    UI.Log(NormalStatic.System, NormalStatic.EFEM, SystemList.ProgramClose, NormalStatic.Ready);
                                else
                                    UI.Alarm(NormalStatic.EFEM, ErrorList.ProcessError, NormalStatic.Ready);
                            }

                        }
                        break;

                    case NormalStatic.InitialDevice:
                        {
                            UI.AutoButton(EFEMStatus.Init_Finish, Result[1]);

                            if (Result[1] == NormalStatic.True)
                                UI.Log(NormalStatic.System, NormalStatic.EFEM, SystemList.ProgramClose, NormalStatic.InitialDevice);
                            else
                                UI.Alarm(NormalStatic.EFEM, ErrorList.ProcessError, string.Format("{0}:{1}", Result[2], Result[3]));
                        }
                        break;

                    case NormalStatic.Device:
                        {
                            UI.Log(NormalStatic.System, NormalStatic.EFEM, SystemList.ProgramClose, NormalStatic.Device);

                            //  if (Robot1_BG.IsBusy == false) 
                            //      UI.AutoButton(EFEMStatus.Run_Finish, Result[1]);

                            //  if (Result[1] == NormalStatic.False)
                            //      UI.Alarm(NormalStatic.EFEM, ErrorList.ProcessError, Result[2]);
                        }
                        break;
                }

            }
            catch (Exception ex)
            {
                UI.Alarm(NormalStatic.Core, ErrorList.AP_TryCatchError, string.Format("{0},{1}", "BG", ex.ToString()));
            }
        }

        #endregion

        #region Task




        #endregion

        #region Wait

        private bool Wait_EFEM()
        {
            string Interrupt = Interrupt_Queue.DeQueue(100);
            if (Interrupt == null)
                return true;

            UI.Alarm(NormalStatic.Core, ErrorList.ProcessError, "User Interrupt");
            return false;
        }
        //60
        private bool WaitNoEFEMRobot2(int timeout)
        {
            int i = 0;
            int time = timeout * 10;
            while (EFEM.RobotBusy(1))
            {
                if (i > time)
                {
                    UI.Alarm(EFEM.RobotDevice(1), ErrorList.Timeout_1010, string.Format("{0}S", ROBOT_TIMEOUT));
                    return false;
                }

                System.Threading.Thread.Sleep(100);
                i++;
            }
            return true;
        }
        //60
        private bool WaitNoEFEMRobot1(int timeout)
        {
            int i = 0;
            int time = timeout * 10;
            while (EFEM.RobotBusy(0))
            {
                if (i > time)
                {
                    UI.Alarm(EFEM.RobotDevice(0), ErrorList.Timeout_1010, string.Format("{0}S", ROBOT_TIMEOUT));
                    return false;
                }

                System.Threading.Thread.Sleep(100);
                i++;
            }
            return true;
        }
        //60
        private bool WaitAutoRobot2(int timeout)
        {
            int i = 0;
            int time = timeout * 10;
            while (EFEM.RobotBusy(1))
            {
                if (i > time)
                {
                    UI.Alarm(EFEM.RobotDevice(1), ErrorList.Timeout_1010, string.Format("{0}S", ROBOT_TIMEOUT));
                    return false;
                }

                if (!Wait_EFEM())
                    return false;
                i++;
            }
            return true;
        }
        //60
        private bool WaitAutoRobot1(int timeout)
        {
            int i = 0;
            int time = timeout * 10;
            while (EFEM.RobotBusy(0))
            {
                if (i > time)
                {
                    UI.Alarm(EFEM.RobotDevice(0), ErrorList.Timeout_1010, string.Format("{0}S", ROBOT_TIMEOUT));
                    return false;
                }
                if (!Wait_EFEM())
                    return false;
                i++;
            }
            return true;
        }
        //60
        private bool WaitCstInitialHome(int timeout)
        {
            int i = 0;
            int time = timeout * 10;

            return true;
        }
        //60

        //60
        private bool WaitCassetteport(int timeout, int Index)
        {
            int i = 0;
            int time = timeout * 10;

            return true;
        }
        //60
        private bool WaitMagazineport(int timeout, int Index)
        {
            int i = 0;
            int time = timeout * 10;

            return true;
        }
        //60
        private bool WaitAligner(int timeout, int index)
        {
            int i = 0;
            int time = timeout * 10;
            while (EFEM.AlignerBusy(index))
            {
                if (i > time)
                {
                    UI.Alarm(EFEM.AlignerDevice(index), ErrorList.Timeout_1010, string.Format("{0}S", ALIGNER_TIMEOUT));
                    return false;
                }
                if (!Wait_EFEM())//外部中斷
                    return false;
                i++;
            }
            return true;
        }
        //60
        private bool WaitStageRobot1(int timeout, int index)
        {
            int i = 0;
            int time = timeout * 10;

            return true;
        }
        //60
        //Joanne 20210825

        #endregion

        #region Refresh

        private void btnRefreshJob_Click(object sender, EventArgs e)
        {
            Refresh_System();
            Refresh_PJ();
            UI.Operate(NormalStatic.Core, "Refresh_PJ");
        }

        public void Refresh_System()
        {
            if (InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate () { Refresh_System(); }));
                return;
            }

            labEFEMBusy.BackColor = (InitialBG.IsBusy || Robot1_MappingReadyBG.IsBusy || Robot2_MappingReadyBG.IsBusy) ? Color.Red : Color.LightGreen;
            labRobot1Busy.BackColor = Robot1_BG.IsBusy ? Color.Red : Color.LightGreen;
            labRobot2Busy.BackColor = Robot2_BG.IsBusy ? Color.Red : Color.LightGreen;
            Busy = (InitialBG.IsBusy || Robot1_MappingReadyBG.IsBusy || Robot2_MappingReadyBG.IsBusy || Robot1_BG.IsBusy || Robot2_BG.IsBusy || Device_BG.IsBusy);

            labNowCount.Text = HT.EFEM.DryRunNowCount.ToString();
            //labNowCount_2.Text = HT.EFEM.DryRunNowCount_2.ToString();

            //EFEM.AuthorityChange(Busy);
            Refresh_AuthorityCondition();
        }

        public void Refresh_AuthorityCondition()
        {
            if (HT.EFEM.Authority == AuthorityTable.Operator || Busy)
            {
                gbxCoreContrl.Visible = false;
                gbxCycle.Enabled = false;
            }
            else
            {
                if (HT.EFEM.Status == EFEMStatus.Run_Fail || HT.EFEM.Status == EFEMStatus.SysCheck_Finish)
                    gbxCoreContrl.Visible = true;
                else
                    gbxCoreContrl.Visible = false;

                if (HT.EFEM.Authority == AuthorityTable.Engineer)
                    gbxCycle.Enabled = false;
                else
                    gbxCycle.Enabled = true;
            }

        }

        public void Refresh_PJ()
        {
            if (InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate () { Refresh_PJ(); }));
                return;
            }

            dgvQueuePJ.Rows.Clear();
            for (int i = 0; i < ls_JobInfo.Count; i++)
            {
                dgvQueuePJ.Rows.Add();
                dgvQueuePJ.Rows[i].Cells[em_JobInfo.SourcePort.ToString()].Value = ls_JobInfo[i].SourcePort;
                dgvQueuePJ.Rows[i].Cells[em_JobInfo.SourceSlot.ToString()].Value = ls_JobInfo[i].SourceSlot;
                dgvQueuePJ.Rows[i].Cells[em_JobInfo.NeedAligner.ToString()].Value = ls_JobInfo[i].NeedAligner;
                dgvQueuePJ.Rows[i].Cells[em_JobInfo.UnloadAngle.ToString()].Value = ls_JobInfo[i].UnloadAngle;
                dgvQueuePJ.Rows[i].Cells[em_JobInfo.UseOCR.ToString()].Value = ls_JobInfo[i].UseOCR;
                dgvQueuePJ.Rows[i].Cells[em_JobInfo.OCRAngle.ToString()].Value = ls_JobInfo[i].OCRAngle;
                dgvQueuePJ.Rows[i].Cells[em_JobInfo.CurrentStep.ToString()].Value = ls_JobInfo[i].CurrentStep;
            }
        }

        public void Refresh_WaferItem()
        {
            if (InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate () { Refresh_WaferItem(); }));
                return;
            }

            cboWaferStatus.Items.Clear();
            cboWaferStatus.Items.Add(SQLWaferInforStep.Finish);

            //for (int i = 0; i < Wafer_Step[(int)HT.Recipe.AutoMode].Count; i++)
            //{
            //    cboWaferStatus.Items.Add(Wafer_Step[(int)HT.Recipe.AutoMode][i]);
            //}
        }


        #endregion

        #region Method

        public void Interrupt()
        {
            Refresh_System();

            if (Busy)
            {
                Robot_Queue[0].EnQueue(NormalStatic.Stop);
                Interrupt_Queue.EnQueue(NormalStatic.Stop);
            }
        }

        public void Close()
        {
            Interrupt();
            EFEM.Close();
        }

        #endregion

        #region UI

        #region Delete

        private void btnDeleteWaferJob_Click(object sender, EventArgs e)
        {
            if (dgvQueuePJ.CurrentRow == null || dgvQueuePJ.SelectedRows.Count == 0)
                return;

            string Device = dgvQueuePJ.Rows[dgvQueuePJ.CurrentRow.Index].Cells[(int)WaferInforTableItem.SocPort].Value.ToString();
            int Slot = Convert.ToInt32(dgvQueuePJ.Rows[dgvQueuePJ.CurrentRow.Index].Cells[(int)WaferInforTableItem.SocSlot].Value.ToString());
            DialogResult result = MessageBox.Show(String.Format("Are yor sure delete selected Item Count {0}", dgvQueuePJ.SelectedRows.Count),
                                                 "Notice",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                string Delete_Com = "";
                for (int row = 0; row < dgvQueuePJ.SelectedRows.Count; row++)
                {
                    int current_row = dgvQueuePJ.SelectedRows[row].Index;
                    Device = dgvQueuePJ.Rows[current_row].Cells[(int)WaferInforTableItem.SocPort].Value.ToString();
                    Slot = Convert.ToInt32(dgvQueuePJ.Rows[current_row].Cells[(int)WaferInforTableItem.SocSlot].Value.ToString());
                    Delete_Com += string.Format("Delete from {0} where {1}= '{2}' and {3}= {4} ;", SQLTable.PJ_Pool, WaferInforTableItem.SocPort, Device, WaferInforTableItem.SocSlot, Slot);
                    UI.Operate(NormalStatic.Core, string.Format("Delete wafer infor :{0}-{1}", Device, Slot));
                }

                SQLite.Multi_DeleteWaferInfo(Delete_Com);
                Refresh_PJ();
            }
        }

        #endregion

        #region Create

        //private void btnCreateJob_Click(object sender, EventArgs e)
        //{
        //    CreaterJobAutoForm();
        //}

        //private void AutoPanel_FormClosed(object sender, EventArgs e)
        //{
        //    Refresh_PJ();
        //}

        public void CreaterJobAutoForm()
        {
            SQLite.Delete(SQLTable.PJ_Pool, "1=1");
            CreateJob = new Form_AutoJob();
            List<IOLPDevice> OMS_In = new List<IOLPDevice>();
            List<IOLPDevice> OMS_Out = new List<IOLPDevice>();
            List<int[]> Slot = new List<int[]>();
            int[] NowSlot = new int[12];
        }

        #endregion

        #region Status

        private void btnJobChange_Click(object sender, EventArgs e)
        {
            if (dgvQueuePJ.CurrentRow == null || cboWaferStatus.SelectedIndex == -1)
                return;

            string Device = dgvQueuePJ.Rows[dgvQueuePJ.CurrentRow.Index].Cells[(int)WaferInforTableItem.SocPort].Value.ToString();
            int Slot = Convert.ToInt32(dgvQueuePJ.Rows[dgvQueuePJ.CurrentRow.Index].Cells[(int)WaferInforTableItem.SocSlot].Value.ToString());
            DialogResult result = MessageBox.Show(String.Format("Are yor sure to change the {0}-Slot_{1} status for {2}", Device, Slot, cboWaferStatus.Text),
                                                 "Notice",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                string condition = string.Format("{0}= '{1}' and {2}= {3}", WaferInforTableItem.SocPort, Device, WaferInforTableItem.SocSlot, Slot);
                Update_PJ(condition, WaferInforTableItem.WaferStatus, cboWaferStatus.Text);
                Refresh_PJ();
                cboWaferStatus.SelectedIndex = -1;
                UI.Operate(NormalStatic.Core, string.Format("Change Status Wafer:{0}", condition));
            }
        }

        private void btnCarrierChange_Click(object sender, EventArgs e)
        {
            if (dgvQueuePJ.CurrentRow == null || cboCarrierStatus.SelectedIndex == -1)
                return;

            string Device = dgvQueuePJ.Rows[dgvQueuePJ.CurrentRow.Index].Cells[(int)WaferInforTableItem.DesPort].Value.ToString();
            int Slot = Convert.ToInt32(dgvQueuePJ.Rows[dgvQueuePJ.CurrentRow.Index].Cells[(int)WaferInforTableItem.DesSlot].Value.ToString());
            DialogResult result = MessageBox.Show(String.Format("Are yor sure to change the {0}-Slot_{1} status for {2}", Device, Slot, cboWaferStatus.Text),
                                                 "Notice",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                string condition = string.Format("{0}= '{1}' and {2}= {3}", WaferInforTableItem.DesPort, Device, WaferInforTableItem.DesSlot, Slot);
                Update_PJ(condition, WaferInforTableItem.CarrierStatus, cboCarrierStatus.Text);
                Refresh_PJ();
                cboCarrierStatus.SelectedIndex = -1;
                UI.Operate(NormalStatic.Core, string.Format("Change Status Wafer:{0}", condition));
            }
        }
        #endregion 

        #endregion

        #region SQL

        private void Update_PJ(string condition, WaferInforTableItem Item, string content)
        {
            SQLite.SetWaferData(Item, condition, string.Format("'{0}'", content));
        }

        #endregion

        private void button1_Click_1(object sender, EventArgs e)
        {
            CreateJob = new Form_AutoJob();
            List<IOLPDevice> OMS_In = new List<IOLPDevice>();
            List<IOLPDevice> OMS_Out = new List<IOLPDevice>();
            List<int[]> Slot = new List<int[]>();
            int[] NowSlot = new int[12];
        }

        private void ckbDrymode_CheckedChanged(object sender, EventArgs e)
        {
            HT.EFEM.DryRunMode = ckbDrymode.Checked;
            if (ckbDrymode.Checked == false)
            {
                labNowCount.Text = "0";
                HT.EFEM.DryRunNowCount = 0;
            }
        }

        private void txtCycleCount_TextChanged(object sender, EventArgs e)
        {
            int temp;
            if (int.TryParse(txtCycleCount.Text, out temp))
                HT.EFEM.DryRunTotalCount = temp;
            else
                HT.EFEM.DryRunTotalCount = 0;
        }

        // Joanne 20201011 Add Start
        private void SetJobInfo()
        {
            if (InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate () { SetJobInfo(); }));
                return;
            }

            int PortIdx;
            DataTable DT = SQLite.ReadDataTable(SQLTable.PJ_Pool,
                string.Format("{0}!= '{2}' and {1}!= '{2}'", WaferInforTableItem.WaferStatus, WaferInforTableItem.CarrierStatus, SQLWaferInforStep.Finish));

            if (DT.Rows.Count == 0)
            {
                return;
            }

            for (int RowIdx = 0; RowIdx < DT.Rows.Count; RowIdx++)
            {
                string SocPort = DT.Rows[RowIdx][(int)WaferInforTableItem.SocPort].ToString();
                string SocSlot = DT.Rows[RowIdx][(int)WaferInforTableItem.SocSlot].ToString();
                string DesPort = DT.Rows[RowIdx][(int)WaferInforTableItem.DesPort].ToString();
                string DesSlot = DT.Rows[RowIdx][(int)WaferInforTableItem.DesSlot].ToString();

            }

            IsSetJobComplete_RB1 = true;
        }
        // Joanne 20201011 Add End


        private void HostTriggerStop(bool ref_Value)
        {
            UI.Log(NormalStatic.SECS, NormalStatic.SECS, SystemList.CommandStart, ", Host send stop command");
            UI.Alarm(NormalStatic.SECS, ErrorList.AP_ParameterFail_0291, ", Host send stop command");

            Interrupt();
        }

        public class cs_JobInfo
        {
            public int SourcePort { get; set; }
            public int SourceSlot { get; set; }
            public int TargetPort { get; set; }
            public int TargetSlot { get; set; }
            public bool NeedAligner { get; set; }
            public bool UseOCR { get; set; }
            public float OCRAngle { get; set; }
            public float UnloadAngle { get; set; }
            public SQLWaferInforStep CurrentStep { get; set; }
        }
        public enum em_JobInfo
        {
            SourcePort,
            SourceSlot,
            NeedAligner,
            UseOCR,
            OCRAngle,
            UnloadAngle,
            CurrentStep,
            Finish
        }
    }
}
