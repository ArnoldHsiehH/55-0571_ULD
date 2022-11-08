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
using NPOI.HSSF.UserModel;
using System.Runtime.InteropServices;
using System.IO;
using Impinj.OctaneSdk;
using System.IO.Ports;

namespace HirataMainControl
{
    public partial class HCT_EFEM : UserControl
    {
        frm_BarcodeFail frm_BarcodeFail = new frm_BarcodeFail();

        #region TS_Form

        private TS_HCTRobot TS_Robot;
        private TS_HCTLoadPort TS_LoadPort;
        private TS_HCTAligner TS_Aligner;
        private TS_HCTStage TS_Stage;
      
        private TS_HCTD900 TS_D900;
        private TS_HCTRFID TS_RFID;


        #endregion

        #region IO_Obj

       
        private static IO_Adam6000[] Adam;
        #endregion

        #region Event_Delegate

        //public delegate void EFEMEvent(EFEM_DI index, bool Status);//Wayne 20190915
        public delegate void EFEMEvent(Adam6050_DI index, bool Status);
        public event EFEMEvent EFEM_StatusChange;

        //Joanne 20220830
        public delegate void Initial_Event(string ref_Command);
        public event Initial_Event InitialCmd;

        //Joanne 20220908 Add
        public delegate void EFEMModeChangeEvent(EFEMMode _mode);
        public event EFEMModeChangeEvent ModeChange;

        /// <summary>
        /// 從PLC取得版本資訊(Walson 20201211)
        /// </summary>
        /// <param name="plc_version"></param>
        //    public delegate void UpdateEvent(string plc_version);
        //  public event UpdateEvent Update_PLC_Version;

        //public delegate void Evt_PLC_SetValue(All_Device ref_ADev, PLC_Device ref_dev, int Devno, int val);
        //public event Evt_PLC_SetValue PLC_SetValue;
        //public delegate int Evt_PLC_GetValue(PLC_Device ref_dev, int Devno);
        //public event Evt_PLC_GetValue PLC_GetValue;

        #endregion

        #region BG_Queue

        private Thread CommandBQ;
        private BackgroundWorker UiUpdateBG = new BackgroundWorker();
        private BlockQueue<CmdStruct> CommandQueue = new BlockQueue<CmdStruct>();

        public BlockQueue<Boolean>[] Robot_RecQueue = new BlockQueue<Boolean>[2];
        public BlockQueue<Boolean> Aligner_RecQueue = new BlockQueue<Boolean>();
        public BlockQueue<Boolean>[] LP_RecQueue = new BlockQueue<Boolean>[2];
        public BlockQueue<Boolean> Alignment_RecQueue = new BlockQueue<Boolean>();
       


        private Thread td_StageProtrude;
        private Thread td_Stage2Protrude;

        #endregion

        #region HasTable

        public static HashTable EFEM_HasTable = null;// = new HashTable();



        #endregion

        #region User_Obj

        public HCT_Aligner[] Aligner;
        public Robot1[] Robot;
        public HCT_LoadPort[] LP;
        public OMRON_RFID[] RFID;
        public inpinjRFID[] inpRFID;
        //  public inpinjRFID[] inpRFID;
        #endregion

        #region ExcelList

        public static List<string[,]>[] ExcelAdmam;
        public static List<string[,]>[] ExcelLogMessage;
        public static List<string[,]>[] ExcelPLC;

        #endregion

        static object Write_Excel = new object();

        public static int RobotCount,
                            AlignerCount,
                            AlignmentCount,
                            MagazineCount,
                            CassetteCount,
                            BufferCount,
                            OCRCount,
                            StageCount,
                            AdamCount,
                            D900Count,
                            ScannerCount,
                            LPCount,
                            RFIDCount,
                            E84Count
            ;


        //Wayne 20190927 add
        //public static EFEM_MainStatus_Str EFEM_MainStatus = new EFEM_MainStatus_Str();

        CmdStruct EFEM_Data = new CmdStruct();

        #region Initial
        ULD_protocol ULD = new ULD_protocol();
        Thread ULD_LD;

        private Thread td_LDULDEvent;
        public static BlockQueue<string> bk_EventReceive = new BlockQueue<string>();

        public HCT_EFEM()
        {
            InitializeComponent();

            //ULD.Initial();//建立與Loader的通信
            ULD_LD = new Thread(ULD_recive);
            //ULD_LD.Start();

            inpRFID = new inpinjRFID[] { inpinjRFID1 };

            Robot = new Robot1[] { Robot1 };
            RobotCount = Robot.Length;

            LP = new HCT_LoadPort[] { P1, P2 };
            LPCount = LP.Length;
            HT.LoadPortData = new LoadPortInfo[LPCount];


            RFID = new OMRON_RFID[] { RFID1, RFID1 };
            RFIDCount = RFID.Length;

            Adam = new IO_Adam6000[] { User_Adam6050_1, User_Adam6050_2, User_Adam6050_3 };
            AdamCount = Adam.Length;

            Aligner = new HCT_Aligner[] { Aligner1 };
            AlignerCount = Aligner.Length;

           
            EFEM_HasTable = new HashTable();

            try
            {
                ReadExcel(string.Format("{0}{1}{2}", NormalStatic.ExcelPath, NormalStatic.Adam, ".xls"), ref ExcelAdmam);
                ReadExcel(string.Format("{0}{1}{2}", NormalStatic.ExcelPath, "LogMessage", ".xls"), ref ExcelLogMessage);
                ReadExcel(string.Format("{0}{1}{2}", NormalStatic.ExcelPath, "PLCAlarm", ".xls"), ref ExcelPLC);
                UI.InitialSystem(NormalStatic.EFEM, NormalStatic.True, ErrorList.MaxCnt);

            }
            catch
            {
                UI.InitialSystem(NormalStatic.EFEM, NormalStatic.False, ErrorList.AP_ExcelError_0384);
            }

        }

        public void Initial()
        {
            int CommandBtn_YLocation = 0;

            inpRFID[0].START();

            #region Robot

            if (RobotCount > 0)
            {
                TS_Robot = new TS_HCTRobot();
                TS_Robot.Initial();
                TS_Robot.btnSend.MouseUp += new System.Windows.Forms.MouseEventHandler(SendRobotCommand_Click);
                Button btnRobot = new Button();
                btnRobot.Location = new Point(0, CommandBtn_YLocation);
                btnRobot.Text = NormalStatic.Robot;
                btnRobot.Height = 25;
                btnRobot.Width = 100;
                btnRobot.Click += new EventHandler(btnRobot_Click);
                pnlButton.Controls.Add(btnRobot);

                for (int i = 0; i < RobotCount; i++)
                {
                    Robot[i].Initial(i);
                    Robot_RecQueue[i] = new BlockQueue<bool>();
                    Robot[i].ActionComplete += new Robot1.RobotEvent(RobotEventContol);
                    // Robot[i].MappingDataComplete += new HCT_RB.MappingDataEvent(MappingDataEventContol);   
                }

                CommandBtn_YLocation += 35;
            }

            #endregion

            #region LoadPort

            if (LPCount > 0)
            {
                TS_LoadPort = new TS_HCTLoadPort();
                TS_LoadPort.Initial();
                TS_LoadPort.btnSend.MouseUp += new System.Windows.Forms.MouseEventHandler(SendLPCommand_Click);
                Button btnLoadPort = new Button();
                btnLoadPort.Location = new Point(0, CommandBtn_YLocation);
                btnLoadPort.Text = NormalStatic.LP;
                btnLoadPort.Height = 25;
                btnLoadPort.Width = 100;
                btnLoadPort.Click += new EventHandler(btnLoadPort_Click);
                pnlButton.Controls.Add(btnLoadPort);
                //LP
                for (int i = 0; i < LPCount; i++)
                {
                    LP[i].Initial(i);
                    LP[i].ActionComplete += new HCT_LoadPort.LoadPortEvent(LP_EventContol);
                }

                CommandBtn_YLocation += 35;
            }
            #endregion

            #region RFID

            if (RFIDCount > 0)
            {
                TS_RFID = new TS_HCTRFID();
                TS_RFID.Initial();
                TS_RFID.btnSend.Click += new System.EventHandler(SendRFIDCommand_Click);
                Button btnRFID = new Button();
                btnRFID.Text = NormalStatic.RFID;
                btnRFID.Location = new Point(0, CommandBtn_YLocation);
                btnRFID.Height = 25;
                btnRFID.Width = 100;
                btnRFID.Click += new EventHandler(btnRFID_Click);
                pnlButton.Controls.Add(btnRFID);

                for (int i = 0; i < RFIDCount; i++)
                {
                    RFID[i].Initial(i);
                    RFID[i].ActionComplete += new OMRON_RFID.RFIDEvent(RFIDEventContol);
                }
                CommandBtn_YLocation += 35;

            }

            #endregion

            #region Aligner

            //if (AlignerCount > 0)
            //{
            //    TS_Aligner = new TS_HCTAligner();
            //    TS_Aligner.Initial();
            //    TS_Aligner.btnSend.MouseUp += new System.Windows.Forms.MouseEventHandler(SendAlignerCommand_Click);
            //    Button btnAligner = new Button();
            //    btnAligner.Text = NormalStatic.Aligner;
            //    btnAligner.Location = new Point(0, CommandBtn_YLocation);
            //    btnAligner.Height = 25;
            //    btnAligner.Width = 100;
            //    btnAligner.Click += new EventHandler(btnAligner_Click);
            //    pnlButton.Controls.Add(btnAligner);

            //    for (int i = 0; i < AlignerCount; i++)
            //    {
            //        Aligner[i].Initial(ref i);
            //        Aligner[i].ActionComplete += new HCT_Aligner.AlignerEvent(AlignerEventContol);
            //    }
            //    CommandBtn_YLocation += 35;

            //}
            #endregion


            #region Adam

            for (int i = 0; i < AdamCount; i++)
            {
                Adam[i].Initial(i, 502, 1000);
                Adam[i].EvnetAdmaSetResult += new IO_Adam6000.SetResultEvnet(AdamSetResult);
                Adam[i].EvnetAdmaDIChange += new IO_Adam6000.DiChangeEvent(AdamDiChange);

            }
            EQ_INI();
            #endregion


            #region BG

            //CommandBQ.DoWork += new DoWorkEventHandler(this.EFEM_Command_DoWork);
            //CommandBQ.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.EFEM_Command_Completed);
            //CommandBQ.RunWorkerAsync(); 
            // EFEM_Command_DoWork();

            //UiUpdateBG = new BackgroundWorker();
            //// UiUpdateBG.DoWork += new DoWorkEventHandler(Dowork_UiUpdate);
            //UiUpdateBG.DoWork += new DoWorkEventHandler(PC_UiUpdateBG);
            //UiUpdateBG.WorkerReportsProgress = true;
            ////UiUpdateBG.ProgressChanged += new ProgressChangedEventHandler(PC_UiUpdateBG);

            UiUpdateBG.RunWorkerAsync();
            CommandBQ = new Thread(EFEM_Command_DoWork);
            CommandBQ.Start();
            CommandBQ.IsBackground = true;
            #endregion


            userUnloader1.Initail();

            td_LDULDEvent = new Thread(() => DoLoaderUnloaderEvent());
            td_LDULDEvent.IsBackground = true;
            td_LDULDEvent.Start();

            td_BarcodeRecive = new Thread(() => Do_BarcodeReceive());
            td_BarcodeRecive.IsBackground = true;
            td_BarcodeRecive.Start();
        }

        #endregion

        ErrorList Error;
        string ErrorMsg;

        #region BG
        CmdStruct QueuedData;

        private void EFEM_Command_DoWork()
        {
            string[] parameter = new string[1];

            bool WorkFlog = false;


            while (true)
            {
                QueuedData = (CmdStruct)CommandQueue.DeQueue();

                WorkFlog = false;
                Error = ErrorList.MaxCnt;
                ErrorMsg = "";

                if (QueuedData.command == SocketCommand.MaxCnt)
                    break;

                if (QueuedData.Parameter != null)
                {
                    parameter = QueuedData.Parameter.Split(new string[] { NormalStatic.Comma }, StringSplitOptions.RemoveEmptyEntries);
                }

                switch (QueuedData.obj)
                {
                    case NormalStatic.Robot:
                        {
                            #region Robot

                            switch (QueuedData.command)
                            {
                                case SocketCommand.ResetError:
                                    {
                                        WorkFlog = RobotResetError(QueuedData.port);
                                    }
                                    break;

                                case SocketCommand.Stop:
                                    {
                                        WorkFlog = RobotStop(QueuedData.port);
                                    }
                                    break;

                                case SocketCommand.ReStart:
                                    {
                                        WorkFlog = RobotRestart(QueuedData.port);
                                    }
                                    break;


                                case SocketCommand.Home:
                                    {
                                        WorkFlog = RobotHome(QueuedData.port);
                                    }
                                    break;

                                case SocketCommand.Initial:
                                    {
                                        WorkFlog = RobotInitial(QueuedData.port);
                                    }
                                    break;

                                case SocketCommand.InitialHome:
                                    {
                                        WorkFlog = RobotInitialHome(QueuedData.port);
                                    }
                                    break;

                                case SocketCommand.SetRobotSpeed:
                                    {
                                        WorkFlog = RobotSetSpeed(QueuedData.port, int.Parse(parameter[0]));
                                    }
                                    break;

                                //case SocketCommand.EdgeGripOff:
                                case SocketCommand.BernoulliOff:
                                case SocketCommand.VacuumOff:
                                case SocketCommand.BernoulliOn:
                                //  case SocketCommand.EdgeGripOn:
                                case SocketCommand.VacuumOn:
                                    {
                                        WorkFlog = RobotSetArm(QueuedData.port, bool.Parse(parameter[0]), QueuedData.command);
                                    }
                                    break;

                                case SocketCommand.ArmSafetyPosition:
                                case SocketCommand.CheckWaferPresence:
                                case SocketCommand.ReadPosition:
                                case SocketCommand.CheckArmOnSafetyPos:
                                case SocketCommand.GetStatus:
                                    {
                                        WorkFlog = RobotGetCommand(QueuedData.command, QueuedData.port);
                                    }
                                    break;

                                case SocketCommand.GetRobotMappingResult:
                                case SocketCommand.GetRobotMappingResult2:
                                case SocketCommand.GetRobotMappingErrorResult:
                                case SocketCommand.GetRobotMappingErrorResult2:
                                    {
                                        WorkFlog = RobotGetMappingCommand(QueuedData.command, QueuedData.port, QueuedData.Parameter);
                                    }
                                    break;
                                case SocketCommand.RobotMapping:
                                    {
                                        WorkFlog = RobotMapping(QueuedData.port, parameter[0]);
                                    }
                                    break;

                                case SocketCommand.GetStandby:
                                case SocketCommand.WaferGet:
                                case SocketCommand.PutStandby:
                                case SocketCommand.WaferPut:
                                case SocketCommand.TopGetStandby:
                                case SocketCommand.TopWaferGet:
                                case SocketCommand.TopWaferPut:
                                case SocketCommand.TopPutStandby:
                                    {
                                        WorkFlog = RobotWaferGPT(QueuedData.port, bool.Parse(parameter[0]), parameter[1], int.Parse(parameter[2]), QueuedData.command);
                                        //ULD.WaferPUT("port1", parameter[2]);

                                    }
                                    break;
                                case SocketCommand.WaferPut_EQ:
                                    {
                                        WorkFlog = RobotWaferGPT(QueuedData.port, bool.Parse(parameter[0]), parameter[1], int.Parse(parameter[2]), QueuedData.command);
                                    }
                                    break;


                                default:
                                    {
                                        Error = ErrorList.RB_CaseDefault_0314;
                                    }
                                    break;
                            }

                            if (QueuedData.Core == true)
                                Robot_RecQueue[QueuedData.port].EnQueue(WorkFlog);
                            #endregion
                        }
                        break;
                    case NormalStatic.RFID:
                        {
                            #region RFID

                            switch (QueuedData.command)
                            {
                                case SocketCommand.Initial:
                                    {
                                        RFIDInitial(QueuedData.port, QueuedData.command);
                                    }
                                    break;

                                case SocketCommand.ReadFoupID:
                                    {
                                        RFIDReadFoupID(QueuedData.port, QueuedData.command);
                                    }
                                    break;

                                case SocketCommand.SetPageMap:
                                    {
                                        RFID[QueuedData.port].EFEM_Parameter = QueuedData.Parameter;
                                        RFIDSetPageMap(QueuedData.port, QueuedData.command);
                                    }
                                    break;
                            }

                            #endregion
                        }
                        break;
                    case NormalStatic.LP:
                        {
                            #region LP

                            switch (QueuedData.command)
                            {
                                case SocketCommand.Initial:
                                    {
                                        WorkFlog = LP_Initial(QueuedData.port);
                                    }
                                    break;

                                case SocketCommand.InitialHome:
                                    {
                                        WorkFlog = LP_InitialHomeCommand(QueuedData.port);
                                    }
                                    break;


                                case SocketCommand.ResetError:
                                    {
                                        WorkFlog = LP_ResetError(QueuedData.port);
                                    }
                                    break;

                                case SocketCommand.UnClamp:
                                case SocketCommand.Clamp:
                                    {
                                        WorkFlog = LP_MoveClampCommand(QueuedData.port, QueuedData.command); // Walson 20191114
                                    }
                                    break;

                                case SocketCommand.Unload:
                                    {
                                        WorkFlog = LP_MoveUnLoadCommand(QueuedData.port, QueuedData.command);
                                    }
                                    break;

                                case SocketCommand.Home:
                                    {
                                        WorkFlog = LP_HomeCommand(QueuedData.port);
                                    }
                                    break;

                                case SocketCommand.Load:
                                case SocketCommand.Map:
                                    {
                                        WorkFlog = LP_MoveLoadCommand(QueuedData.port, QueuedData.command);
                                    }
                                    break;

                                //case SocketCommand.CloseOutDoor:
                                //case SocketCommand.OpenOutDoor:
                                //    {
                                //        WorkFlog = LP_MoveOutDoorCommand(QueuedData.port, QueuedData.command);
                                //    }
                                //    break;
                                case SocketCommand.GetStatus:
                                case SocketCommand.GetProtrusionSensor:
                                case SocketCommand.GetWaferSlot: //20211209_Elijah
                                case SocketCommand.GetWaferThickness:
                                case SocketCommand.GetWaferPosition:
                                case SocketCommand.GetMapp:
                                    {
                                        WorkFlog = LP_GetCommand(QueuedData.port, QueuedData.Parameter, QueuedData.command);
                                    }
                                    break;

                                case SocketCommand.SetType:
                                    {
                                        WorkFlog = LP_SetUnLoadCommand(QueuedData.port, QueuedData.Parameter, QueuedData.command);
                                    }
                                    break;

                                    //case SocketCommand.SetInterlock:
                                    //    {
                                    //        WorkFlog = LP_SetCommand(QueuedData.port, QueuedData.Parameter, QueuedData.command);
                                    //    }
                                    //    break;
                            }

                            #endregion
                        }
                        break;

                    case NormalStatic.Aligner:
                        {
                            #region Aligner

                            switch (QueuedData.command)
                            {
                                case SocketCommand.Initial:
                                    {
                                        WorkFlog = AlignerInitial();
                                    }
                                    break;

                                case SocketCommand.ResetError:
                                    {
                                        WorkFlog = AlignerNotLockMethod();
                                    }
                                    break;


                                case SocketCommand.Home:
                                case SocketCommand.InitialHome:
                                case SocketCommand.LiftPinUp:
                                case SocketCommand.LiftPinDown:
                                case SocketCommand.CycleHomeCheckDegree:
                                case SocketCommand.CycleAlignmentOCR:
                                case SocketCommand.CycleAlignmentFinish:
                                case SocketCommand.Alignment:
                                case SocketCommand.FindNotch:
                                case SocketCommand.ToAngle:

                                    {
                                        WorkFlog = AlignerMoveMethod();
                                    }
                                    break;

                                case SocketCommand.GetStatus:
                                case SocketCommand.GetIDReaderDegree:
                                case SocketCommand.GetAlignerDegree:
                                case SocketCommand.GetAlignerWaferType:
                                    {
                                        WorkFlog = AlignerGetMethod();
                                    }
                                    break;

                                case SocketCommand.AlignerVacuum:
                                case SocketCommand.SetIDReaderDegree:
                                case SocketCommand.SetAlignerWaferType:
                                case SocketCommand.SetAlignerDegree:

                                    {
                                        WorkFlog = AlignerSetMethod();
                                    }
                                    break;
                                case SocketCommand.AlignerVacuum_on:
                                    WorkFlog = AlignerSetMethod();
                                    break;
                                case SocketCommand.AlignerVacuum_off:
                                    WorkFlog = AlignerSetMethod();
                                    break;

                            }

                            if (QueuedData.Core == true)
                                Aligner_RecQueue.EnQueue(WorkFlog);

                            #endregion
                        }
                        break;
                  


                }

                if (Error != ErrorList.MaxCnt)
                    UI.Alarm(string.Format("{0}{1}", QueuedData.obj, QueuedData.port + 1), Error, ErrorMsg);


            }
        }

        public void Command_EnQueue(CmdStruct queue)
        {
            if (InvokeRequired)
                queue.Core = true;
            else
                queue.Core = false;

            CommandQueue.EnQueue(queue);
        }

        #endregion

        #region EFEM_Close

        public void Close()
        {
            //for (int i = 0; i < AdamCount; i++)
            //    Adam[i].Close();

            for (int i = 0; i < RobotCount; i++)
                Robot[i].Close();


            //UI.CloseBG(NormalStatic.PLC);

            EFEM_Data.command = SocketCommand.MaxCnt;

            Command_EnQueue(EFEM_Data);

        }

        #endregion

        #region Method

        public bool Ready
        {
            get
            {
                bool ready = true;

                //for (int i = 0; i < AlignerCount; i++)
                //{
                //    ready &= Aligner[i].Ui_Connect;
                //}

                for (int i = 0; i < RobotCount; i++)
                {
                    ready &= Robot[i].Ui_Connect;
                    ready &= Robot[i].Ui_Remote;
                }

                for (int i = 0; i < AdamCount; i++)
                {
                    //ready &= Adam[i].Ui_Connect;
                }



                return ready;
            }
        }

        public bool CheckEFEMStatus()
        {
            if (!Ready)
            {
                UI.Alarm(NormalStatic.EFEM, ErrorList.AP_SocketError_0382);
                return false;
            }

            for (int i = 0; i < HCT_EFEM.RobotCount; i++)
            {
                if (RobotBusy(0))
                {
                    UI.Alarm(Robot[i].DeviceName, ErrorList.DeviceIsBusy_0301);
                    return false;
                }

            }
            if (AlignerBusy(0))
            {
                UI.Alarm(Aligner[0].DeviceName, ErrorList.DeviceIsBusy_0301);
                return false;
            }


            return true;
        }

        #endregion

        #region Robot

        #region Public

        public string RobotDevice(int Index) { return Robot[Index].DeviceName; }
        public bool RobotBusy(int Index) { return Robot[Index].Ui_Busy; }
        public WaferStatus Robot_ArmPresence(int Index, bool arm) { return arm ? Robot[Index].Ui_UpperWaferPresent : Robot[Index].Ui_LowerWaferPresent; }
        public string Robot_GetStatus(int Index) { return Robot[Index].Ui_Status; }
        public ArmStatus Robot_ArmExtend(int Index, bool arm) { return arm ? Robot[Index].Ui_ArmStatusX : Robot[Index].Ui_ArmStatusY; }
        public ArmStatus Robot_ArmTurn(int Index) { return Robot[Index].Ui_ArmStatusR; }
        public RobotPosition Robot_NowPosition(int Index) { return Robot[Index].Ui_RobotPos; }

        public void Robot_SetInfo(int Index, int arm)  // Walson20201124修改
        {
            string info = "";

            if (arm == 0)
                Robot[Index].Ui_LowerWaferInfo = info;
            else
                Robot[Index].Ui_UpperWaferInfo = info;
        }



        public void Robot_RetryCount(int index, int value)
        {
            Robot[index].Retry_Count = value;
        }

        #endregion

        #region Event

        private void RobotEventContol(string Reply, bool Result, string PortName)
        {
            string[] token = Reply.Split(',');
            SocketCommand RobotAction = (SocketCommand)Enum.Parse(typeof(SocketCommand), token[0]);
            string dest = token[2];
            string slot = token[3];
            int arm = int.Parse(token[1]);
            int port;
            int Index = Convert.ToInt16(PortName.Substring(NormalStatic.Robot.Length, 1)) - 1;
            bool GetFinish = false;
            bool PutFinish = false;
            UI.Log(NormalStatic.Robot, string.Format("{0}{1}", NormalStatic.Robot, Index + 1), SystemList.CommandComplete, string.Format("{0}:({1})({2})({3})({4})", RobotAction, Result, arm, dest, slot));

            #region 取放片完成後檢查是否有凸片

            #endregion


            if (Result)
            {
                switch (RobotAction)
                {
                    case SocketCommand.Initial:
                        {
                            Robot[Index].Ui_Connect = true;
                            UI.InitialSystem(Robot[Index].DeviceName, NormalStatic.True, ErrorList.MaxCnt);
                        }
                        break;

                    case SocketCommand.Home:
                    case SocketCommand.InitialHome:
                        {
                            switch (Index)
                            {
                                case 0:
                                    {
                                        for (int i = 0; i < AlignerCount; i++)
                                            Aligner[i].Ui_Busy = false;


                                    }
                                    break;

                                case 1:
                                    {

                                    }
                                    break;

                            }

                            if (RobotAction == SocketCommand.Home)
                            {
                                if (Robot[Index].NowNeedHomeChangeData == true)
                                {
                                    UI.Log(NormalStatic.Robot, string.Format("{0}{1}", NormalStatic.Robot, Index + 1), SystemList.CommandParameter, string.Format("{0},{1}", Robot[Index].NowNeedHomeChangeData, Robot[Index].NowGPTCommandType));
                                    switch (Robot[Index].NowGPTCommandType)
                                    {
                                        case RobotGPT.Get:
                                        case RobotGPT.TopGet:
                                            {
                                                GetFinish = true;
                                                PutFinish = false;
                                            }
                                            break;

                                        case RobotGPT.Put:
                                        case RobotGPT.TopPut:
                                            {
                                                GetFinish = false;
                                                PutFinish = true;
                                            }
                                            break;
                                    }

                                    Robot[Index].NowNeedHomeChangeData = false;
                                }
                            }
                        }
                        break;

                    case SocketCommand.WaferGet:
                    case SocketCommand.TopWaferGet:
                        {
                            GetFinish = true;
                            PutFinish = false;
                        }
                        break;

                    case SocketCommand.WaferPut:
                    case SocketCommand.TopWaferPut:
                        {
                            #region End_Status

                            GetFinish = false;
                            PutFinish = true;

                            #endregion
                        }
                        break;

                    case SocketCommand.GetStandby:
                    case SocketCommand.PutStandby:
                    case SocketCommand.TopGetStandby:
                    case SocketCommand.TopPutStandby:
                        {

                            if (dest.IndexOf(NormalStatic.Aligner) == 0)
                            {
                                port = Convert.ToInt16(dest.Substring(NormalStatic.Aligner.Length, 1)) - 1;
                                Aligner[port].Ui_Busy = false;
                            }
                            else if (dest.IndexOf(NormalStatic.Stage) == 0)
                            {
                                port = Convert.ToInt16(dest.Substring(NormalStatic.Stage.Length, 1)) - 1;
                                //Stage[port].Ui_Busy = false;
                            }
                        }
                        break;
                    case SocketCommand.RobotMapping:
                        {
                            dest = token[2];
                            int SlotCount;
                            string SlotPresent;
                            string SlotError;
                            switch (Index)
                            {
                                case 1:

                                    break;

                                case 0:
                                    {

                                    }
                                    break;
                            }
                        }
                        break;
                }

                #region Get
                if (GetFinish)
                {

                    if (dest.IndexOf(NormalStatic.LP) == 0)
                    {
                        port = Convert.ToInt16(dest.Substring(NormalStatic.LP.Length, 1)) - 1;

                        LP[port].SetSlotData(Convert.ToInt32(slot), (int)WaferStatus.WithOut);

                        UserUnloader.SendCommand(string.Format("LPGet,{0},{1}", port, slot));

                        //Wayne Add For Flow Test 20220822
                        if (UserCore.CurrentJobIndex < UserCore.ls_JobInfo.Count) //Step 轉換
                        {
                            UserCore.ls_JobInfo[UserCore.CurrentJobIndex].CurrentStep = SQLWaferInforStep.PutAL_Send;
                        }
                    }
                    else if (dest.IndexOf(NormalStatic.Aligner) == 0)
                    {
                        port = Convert.ToInt16(dest.Substring(NormalStatic.Aligner.Length, 1)) - 1;
                        //Wayne Add For Flow Test 20220822
                        if (UserCore.CurrentJobIndex < UserCore.ls_JobInfo.Count) //Step 轉換
                        {
                            UserCore.ls_JobInfo[UserCore.CurrentJobIndex].CurrentStep = SQLWaferInforStep.AL_Home; // WayneTest
                        }
                        Aligner[port].Ui_WaferInfo = "";

                        Aligner[port].Ui_Busy = false;
                    }
                    else if (dest.IndexOf(NormalStatic.EQ) > -1)
                    {
                        //Wayne Add For Flow Test 20220822
                        if (UserCore.CurrentJobIndex < UserCore.ls_JobInfo.Count) //Step 轉換
                        {
                            UserCore.ls_JobInfo[UserCore.CurrentJobIndex].CurrentStep = SQLWaferInforStep.PutLP_Send; // WayneTest
                        }
                    }

                    Robot_SetInfo(Index, arm);

                    //Walson 20201011 修改 End
                }
                #endregion

                #region Put
                if (PutFinish)
                {
                    if (dest.IndexOf(NormalStatic.LP) == 0)
                    {
                        port = Convert.ToInt16(dest.Substring(NormalStatic.LP.Length, 1)) - 1;

                        LP[port].SetSlotData(Convert.ToInt32(slot), (int)WaferStatus.With);
                        UserUnloader.SendCommand(string.Format("LPPut,{0},{1}", port, slot));

                        if (UserCore.CurrentJobIndex < UserCore.ls_JobInfo.Count) //Step 轉換
                        {
                            UserCore.ls_JobInfo[UserCore.CurrentJobIndex].CurrentStep = SQLWaferInforStep.Finish;
                        }
                    }
                    else if (dest.IndexOf(NormalStatic.Aligner) == 0)
                    {
                        port = Convert.ToInt16(dest.Substring(NormalStatic.Aligner.Length, 1)) - 1;
                        //Wayne Add For Flow Test 20220822
                        if (UserCore.CurrentJobIndex < UserCore.ls_JobInfo.Count) //Step 轉換
                        {
                            UserCore.ls_JobInfo[UserCore.CurrentJobIndex].CurrentStep = SQLWaferInforStep.AL_GetStatus;
                        }

                        if (arm == 1)
                        {
                            Aligner[0].Ui_WaferInfo = Robot[Index].Ui_UpperWaferInfo;
                        }
                        else
                        {
                            Aligner[0].Ui_WaferInfo = Robot[Index].Ui_LowerWaferInfo;
                        }

                        Aligner[port].Ui_Busy = false;
                    }
                    else if (dest.IndexOf(NormalStatic.Stage) == 0)
                    {
                        port = Convert.ToInt16(dest.Substring(NormalStatic.Stage.Length, 1)) - 1;

                        switch (Index)
                        {
                            case 0:
                                {


                                }
                                break;

                            case 1:
                                { }
                                break;
                        }
                    }
                    else if (dest.IndexOf(NormalStatic.EQ) == 0)
                    {
                        if (UserCore.CurrentJobIndex < UserCore.ls_JobInfo.Count) //Step 轉換
                        {
                            UserCore.ls_JobInfo[UserCore.CurrentJobIndex].CurrentStep = SQLWaferInforStep.Finish;
                        }
                    }

                    if (arm == 1)
                    {
                        Robot[Index].Ui_UpperWaferInfo = "";
                    }
                    else
                    {
                        Robot[Index].Ui_LowerWaferInfo = "";
                    }
                }

                #endregion

                UI.Log(NormalStatic.System, Robot[Index].DeviceName, SystemList.CommandComplete, RobotAction.ToString());
            }
            else
            {
                switch (RobotAction)
                {
                    case SocketCommand.Initial:
                        {
                            Robot[Index].Ui_Connect = false;
                            UI.InitialSystem(Robot[Index].DeviceName, NormalStatic.False, ErrorList.AP_InitialFail_0393);
                        }
                        break;

                    case SocketCommand.Home:
                    case SocketCommand.InitialHome:
                    case SocketCommand.WaferGet:
                    case SocketCommand.TopWaferGet:
                    case SocketCommand.WaferPut:
                    case SocketCommand.TopWaferPut:
                    case SocketCommand.GetStandby:
                    case SocketCommand.PutStandby:
                    case SocketCommand.TopGetStandby:
                    case SocketCommand.TopPutStandby:
                        //    case SocketCommand.Move_OCRReadPosition:
                        break;
                }
                UI.Alarm(Robot[Index].DeviceName, Robot[Index].NowErrorList, Robot[Index].NowErrorMsg);
            }

            Robot[Index].ClearMainJob();
        }

        #endregion

        #region Check

        private bool Robot1_DeviceCheckArm()
        {
            if (Robot[0].Ui_Connect == false)
            {
                Error = ErrorList.AP_SerialError_0381;
                return true;
            }

            if ((Robot[0].Ui_RobotPos >= RobotPosition.P1 && Robot[0].Ui_RobotPos <= RobotPosition.P10_Map && QueuedData.obj == NormalStatic.CstPort && ((int)(Robot[0].Ui_RobotPos - RobotPosition.P1) / 2) == QueuedData.port)
             || (Robot[0].Ui_RobotPos == RobotPosition.Aligner1 && QueuedData.obj == NormalStatic.Aligner)
             || (Robot[0].Ui_RobotPos >= RobotPosition.Stage1 && Robot[0].Ui_RobotPos <= RobotPosition.Stage2 && QueuedData.obj == NormalStatic.Stage && ((int)(Robot[0].Ui_RobotPos - RobotPosition.Stage1)) == QueuedData.port)
             || (Robot[0].Ui_RobotPos == RobotPosition.Home)
             || (Robot[0].Ui_RobotPos == RobotPosition.Unknown))
            {
                if (Robot[0].Ui_ArmStatusX != ArmStatus.Arm_Home || Robot[0].Ui_ArmStatusY != ArmStatus.Arm_Home)
                {
                    Error = ErrorList.RB_ArmExtend_0316;
                    return true;
                }
            }
            return false;
        }



        private bool RobotCheckBusy(int port)
        {
            if (Robot[port].Ui_Busy)
            {
                Error = ErrorList.DeviceIsBusy_0301;
            }
            return Robot[port].Ui_Busy;
        }

        public bool CheckRobotArmSafety(int port, SocketCommand command)
        {
            for (int ArmCnt = 0; ArmCnt < Robot[port].IniArmCnt; ArmCnt++)
            {
                switch (ArmCnt)
                {
                    case 0:
                        {
                            if (Robot[port].Ui_ArmStatusX != ArmStatus.Arm_Home)
                            {
                                Error = ErrorList.RB_ArmExtend_0316;
                                return false;
                            }

                            if (command != SocketCommand.TopGetStandby && command != SocketCommand.TopPutStandby && command != SocketCommand.TopWaferGet && command != SocketCommand.TopWaferPut)
                            {
                                if (Robot[port].Ui_ArmStatusR != ArmStatus.Arm_Home)
                                {
                                    Error = ErrorList.RB_LowerTurn_0317;
                                    return false;
                                }
                            }

                        }
                        break;

                    case 1:
                        {
                            if (Robot[port].Ui_ArmStatusY != ArmStatus.Arm_Home)
                            {
                                Error = ErrorList.RB_ArmExtend_0316;
                                return false;
                            }

                        }
                        break;
                }
            }
            return true;
        }

        private bool CheckRobotArmNoWafer(ref int index, ref bool arm)
        {
            if (arm == false)
            {
                if (Robot[index].Ui_LowerWaferPresent == WaferStatus.With)
                {
                    Error = ErrorList.RB_With_0121;
                    return false;
                }

            }
            else
            {
                if (Robot[index].Ui_UpperWaferPresent == WaferStatus.With)
                {
                    Error = ErrorList.RB_With_0121;
                    return false;
                }
            }
            return true;
        }

        private bool CheckRobotArmHasWafer(ref int index, ref bool arm)
        {
            if (arm == false)
            {
                if (Robot[index].Ui_LowerWaferPresent == WaferStatus.WithOut)
                {
                    Error = ErrorList.RB_Without_0120;
                    return false;
                }
            }
            else
            {
                if (Robot[index].Ui_UpperWaferPresent == WaferStatus.WithOut)
                {
                    Error = ErrorList.RB_Without_0120;
                    return false;
                }
            }


            return true;
        }

        private bool CheckRobotGPT_Stage(int port, int Index, string dest, bool arm, SocketCommand command, int slot)
        {


            if (arm == true)
            {
                if (command == SocketCommand.TopWaferPut || command == SocketCommand.TopPutStandby
                 || command == SocketCommand.TopWaferGet || command == SocketCommand.TopGetStandby)
                {
                    Error = ErrorList.RB_UpperNotTurn_0304;
                    return false;
                }
            }

            switch (command)
            {
                case SocketCommand.WaferGet:
                case SocketCommand.TopWaferGet:
                    {

                    }
                    break;

                case SocketCommand.WaferPut:
                case SocketCommand.TopWaferPut:
                    {

                    }
                    break;
            }




            //Stage[port].Ui_Busy = true;
            return true;
        }

        #endregion

        #region Command

        private void Robot_ReConnect(int Index)
        {
            Robot[Index].COM_Connect();
        }

        private void Robot_Disconnect(int Index)
        {
            Robot[Index].COM_Disconnect();
        }

        private bool RobotInitial(int Index)
        {
            Robot_ReConnect(Index);

            if (Robot[Index].Ui_Connect == true)
            {
                Robot[Index].ProcessSingleCommand(SocketCommand.Initial);
            }
            return true;
        }

        private bool RobotInitialHome(int Index)
        {
            Robot[Index].ProcessSingleCommand(SocketCommand.InitialHome);
            return true;
        }

        private bool RobotHome(int Index)
        {
            if (RobotCheckBusy(Index))
                return false;

            switch (Index)
            {
                case 0:
                    {

                        for (int i = 0; i < AlignerCount; i++)
                        {
                            Aligner[i].Ui_Busy = true;
                        }

                    }
                    break;

                case 1:
                    {

                    }
                    break;
            }

            Robot[Index].ProcessSingleCommand(SocketCommand.Home);
            return true;
        }

        private bool RobotResetError(int Index)
        {
            Robot[Index].ProcessSingleCommand(SocketCommand.ResetError);
            return true;
        }

        private bool RobotStop(int Index)
        {
            if (Robot[Index].Ui_Stop == true)
            {
                Error = ErrorList.RB_StopOn_0103;
                return false;
            }

            Robot[Index].ProcessSingleCommand(SocketCommand.Stop);
            return true;
        }

        private bool RobotRestart(int Index)
        {
            if (Robot[Index].Ui_Stop == false)
            {
                Error = ErrorList.RB_NowNotStop_0116;
                return false;
            }

            if (Robot[Index].IniStopRestart == false)
            {
                if (RobotCheckBusy(Index))
                    return false;
            }

            Robot[Index].ProcessSingleCommand(SocketCommand.ReStart);
            return true;
        }

        private bool RobotSetSpeed(int Index, int speed)
        {
            if (RobotCheckBusy(Index))
                return false;

            Robot[Index].SetNowSpeed(speed);

            Robot[Index].ProcessSingleCommand(SocketCommand.SetRobotSpeed);
            return true;
        }

        private bool RobotSetArm(int Index, bool arm, SocketCommand command)
        {
            if (RobotCheckBusy(Index))
                return false;

            if (Index == 0 && arm == false && (command == SocketCommand.BernoulliOff || command == SocketCommand.VacuumOff))
            {
                if (Robot[0].Ui_ArmStatusR != ArmStatus.Arm_Home)
                {
                    Error = ErrorList.RB_LowerTurn_0317;
                    return false;
                }
            }

            Robot[Index].SetNowUseArm(arm == false ? 0 : 1);

            Robot[Index].ProcessSingleCommand(command);


            return true;
        }

        private bool RobotGetCommand(SocketCommand command, int Index)
        {
            if (RobotCheckBusy(Index))
                return false;

            Robot[Index].ProcessSingleCommand(command);
            return true;
        }

        private bool RobotGetMappingCommand(SocketCommand command, int Index, string des)
        {
            if (RobotCheckBusy(Index))
                return false;

            Robot[Index].SetNowUseObj(des);
            Robot[Index].ProcessSingleCommand(command);
            return true;
        }

        private bool RobotMapping(int Index, string dest)
        {
            int StartGPAddress;
            bool arm = false;
            //Robot
            if (Robot[Index].IniMappingSup == false)
            {
                Error = ErrorList.AP_CommandNoSup_0288;
                return false;
            }

            if (RobotCheckBusy(Index))
                return false;

            if (CheckRobotArmSafety(Index, SocketCommand.RobotMapping) == false)
                return false;

            //if (CheckRobotArmNoWafer(ref Index, ref arm) == false)
            //    return false;

            if (Robot[Index].Check_LPAddress(string.Format("{0}{1}", dest, NormalStatic.MAP)) == false)
            {
                Error = ErrorList.RB_DevicePosUnKnown_0309;
                return false;
            }
            else
            {
                Error = ErrorList.AP_ParameterFail_0291;
                return false;
            }


            Robot[Index].SetNowUseArm(1);
            Robot[Index].ProcessSingleCommand(SocketCommand.RobotMapping);

            return true;
        }

        private bool RobotWaferGPT(int Index, bool arm, string dest, int slot, SocketCommand command)
        {
            //double OffsetTemp = 0;
            int StartGPAddress;
            //robot
            if (RobotCheckBusy(Index) == true)
                return false;

            if (CheckRobotArmSafety(Index, command) == false)
                return false;

            switch (command)
            {

                case SocketCommand.TopGetStandby:
                case SocketCommand.TopWaferGet:
                case SocketCommand.WaferGet:
                case SocketCommand.GetStandby:
                    {
                        if (CheckRobotArmNoWafer(ref Index, ref arm) == false)
                            return false;
                    }
                    break;

                case SocketCommand.TopWaferPut:
                case SocketCommand.TopPutStandby:
                case SocketCommand.WaferPut:
                case SocketCommand.PutStandby:
                    {
                        if (CheckRobotArmHasWafer(ref Index, ref arm) == false)
                            return false;
                    }
                    break;
            }

            switch (command)
            {

                case SocketCommand.TopGetStandby:
                case SocketCommand.TopWaferGet:
                case SocketCommand.TopPutStandby:
                case SocketCommand.TopWaferPut:
                    {
                        if (Robot[Index].Check_OtherAddress(Robot_Static.TurnPos) == false || Robot[Index].Check_OtherAddress(Robot_Static.TurnBackPos) == false)
                        {
                            Error = ErrorList.RB_DevicePosUnKnown_0309;
                            return false;
                        }
                    }
                    break;
            }

            if (dest.IndexOf(NormalStatic.LP) >= 0)
            {
                if (Robot[Index].Check_LPAddress(string.Format("{0}{1}", dest, NormalStatic.POS)) == false)
                {
                    Error = ErrorList.RB_DevicePosUnKnown_0309;
                    return false;
                }
                else
                {
                    StartGPAddress = Robot[Index].Get_LPAddress(string.Format("{0}{1}", dest, NormalStatic.POS), 0);
                }

                int port = Convert.ToInt16(dest.Substring(NormalStatic.CstPort.Length, dest.Length - NormalStatic.CstPort.Length)) - 1;
                if (command == SocketCommand.WaferGet)
                {
                    if (LP[port].GetSlotData1(slot - 1) != 1)
                    {
                        Error = ErrorList.RB_DeviceWithout_0307;
                        return false;
                    }
                }
                else if (command == SocketCommand.WaferPut)
                {
                    if (LP[port].GetSlotData1(slot - 1) != 0)
                    {
                        Error = ErrorList.RB_DeviceWithout_0307;
                        return false;
                    }
                }

                double LoadPortGap = LP[port].Ui_NowGap;

                Robot[Index].GPT_Start((int)PLC_B.CSTInvasionStart + port, dest, slot, LoadPortGap, StartGPAddress);
            }
            else if (dest.IndexOf(NormalStatic.Aligner) >= 0)
            {
                if (Robot[Index].Check_OtherAddress(dest) == false)
                {
                    Error = ErrorList.RB_DevicePosUnKnown_0309;
                    return false;
                }

                //   if (CheckRobotGPT_Aligner(ref dest, ref arm, ref command) == false)
                //       return false;

                StartGPAddress = Robot[Index].Get_OtherAddress(dest);
                Robot[Index].GPT_Start((int)PLC_B.MaxCnt, dest, slot = 1, Robot_Static.AlignerOffset, StartGPAddress);

            }
            else if (dest.IndexOf(NormalStatic.Stage) >= 0)
            {
                if (Robot[Index].Check_OtherAddress(dest) == false)
                {
                    Error = ErrorList.RB_DevicePosUnKnown_0309;
                    return false;
                }

                int port = Convert.ToInt16(dest.Substring(NormalStatic.Stage.Length, 1)) - 1;

                if (CheckRobotGPT_Stage(port, Index, dest, arm, command, slot) == false)
                    return false;

                StartGPAddress = Robot[Index].Get_OtherAddress(dest);
                Robot[Index].GPT_Start((int)PLC_B.Stage1WaferInvasion + (port * 2) + Index, dest, slot, Robot_Static.StageOffset, StartGPAddress);
            }
            else if (dest.IndexOf(NormalStatic.EQ) >= 0)
            {
                if (Robot[Index].Check_OtherAddress(dest) == false)
                {
                    Error = ErrorList.RB_DevicePosUnKnown_0309;
                    return false;
                }

                int port = 0;
                StartGPAddress = Robot[Index].Get_OtherAddress(dest);
                Robot[Index].GPT_Start((int)PLC_B.Stage1WaferInvasion + (port * 2) + Index, dest, slot, Robot_Static.StageOffset, StartGPAddress);
            }
            else
            {
                Error = ErrorList.AP_ParameterFail_0291;
                return false;
            }

            Robot[Index].SetNowUseArm(arm == false ? 0 : 1);

            Robot[Index].ProcessSingleCommand(command);

            return true;
        }

        private bool RobotWaferOCR(int Index, bool arm, string dest, int slot, SocketCommand command)
        {

            int StartGPAddress;
            //robot
            if (RobotCheckBusy(Index) == true)
                return false;

            if (CheckRobotArmSafety(Index, command) == false)
                return false;

            if (CheckRobotArmHasWafer(ref Index, ref arm) == false)
                return false;

            if (dest.IndexOf(NormalStatic.Aligner) >= 0)
            {
                if (Robot[Index].Check_OtherAddress(dest) == false)
                {
                    Error = ErrorList.RB_DevicePosUnKnown_0309;
                    return false;
                }

                StartGPAddress = Robot[Index].Get_OtherAddress(dest);
                Robot[Index].GPT_Start((int)PLC_B.MaxCnt, dest, slot = 1, Robot_Static.AlignerOffset, StartGPAddress);

            }
            else
            {
                Error = ErrorList.AP_ParameterFail_0291;
                return false;
            }

            Robot[Index].SetNowUseArm(arm == false ? 0 : 1);

            Robot[Index].ProcessSingleCommand(command);

            return true;
        }

        #endregion

        #region TS_UI

        private void SendRobotCommand_Click(object sender, EventArgs e)
        {
            int Index = Convert.ToInt32(TS_Robot.cboCnt.Text) - 1;

            if (Robot[Index].Ui_Connect == false)
            {
                UI.Alarm(Robot[Index].DeviceName, ErrorList.AP_SerialError_0381);
                return;
            }

            if (Robot[Index].Ui_Remote == false)
            {
                UI.Alarm(Robot[Index].DeviceName, ErrorList.RB_ManualMode_0702);
                return;
            }

            bool arm = TS_Robot.cboArm.Text == Robot_Static.Lower ? false : true;

            EFEM_Data.command = (SocketCommand)Enum.Parse(typeof(SocketCommand), TS_Robot.cboCommand.Text);
            EFEM_Data.port = Index;
            EFEM_Data.obj = NormalStatic.Robot;

            switch (EFEM_Data.command)
            {
                #region Normal

                case SocketCommand.Home:
                    {
                        DialogResult HomeResult = MessageBox.Show("Please check Robot arm is safety",
                                                 "Error",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Error);

                        if (HomeResult == DialogResult.Yes)
                        {
                            Command_EnQueue(EFEM_Data);
                        }
                    }
                    break;

                case SocketCommand.ArmSafetyPosition:
                    {

                        DialogResult HomeResult = MessageBox.Show("This command will force the Arm level retraction action. Please confirm that the Robot Arm has no interference state. Does the above describe whether to perform this action?",
                                                 "Error",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Error);

                        if (HomeResult == DialogResult.Yes)
                        {
                            Command_EnQueue(EFEM_Data);
                        }
                    }
                    break;

                case SocketCommand.GetStatus:
                case SocketCommand.ResetError:
                case SocketCommand.Stop:
                case SocketCommand.ReStart:
                case SocketCommand.ReadPosition:
                case SocketCommand.CheckWaferPresence:
                case SocketCommand.CheckArmOnSafetyPos:
                    {
                        Command_EnQueue(EFEM_Data);
                    }
                    break;

                case SocketCommand.GetRobotMappingResult:
                case SocketCommand.GetRobotMappingResult2:
                case SocketCommand.GetRobotMappingErrorResult:
                case SocketCommand.GetRobotMappingErrorResult2:
                    {
                        EFEM_Data.Parameter = TS_Robot.cboDest.Text;
                        Command_EnQueue(EFEM_Data);
                    }
                    break;

                #endregion

                #region Set

                case SocketCommand.SetRobotSpeed:
                    {
                        EFEM_Data.Parameter = TS_Robot.cboSpeed.Text;
                        Command_EnQueue(EFEM_Data);
                    }
                    break;

                case SocketCommand.BernoulliOff:
                case SocketCommand.BernoulliOn:
                //      case SocketCommand.EdgeGripOff:
                //     case SocketCommand.EdgeGripOn:
                case SocketCommand.VacuumOff:
                case SocketCommand.VacuumOn:
                    {
                        EFEM_Data.Parameter = arm.ToString();
                        Command_EnQueue(EFEM_Data);
                    }
                    break;

                #endregion

                #region Mov

                case SocketCommand.RobotMapping:
                    {
                        EFEM_Data.Parameter = TS_Robot.cboDest.Text;
                        Command_EnQueue(EFEM_Data);
                    }
                    break;

                case SocketCommand.PutStandby:
                case SocketCommand.WaferPut:
                case SocketCommand.TopPutStandby:
                case SocketCommand.TopWaferPut:
                case SocketCommand.GetStandby:
                case SocketCommand.WaferGet:
                case SocketCommand.TopGetStandby:
                case SocketCommand.TopWaferGet:
                    //     case SocketCommand.Move_OCRReadPosition:
                    {
                        EFEM_Data.Parameter = string.Format("{0},{1},{2}", arm, TS_Robot.cboDest.Text, TS_Robot.cboSlot.Text);
                        Command_EnQueue(EFEM_Data);
                    }
                    break;

                    #endregion
            }

            UI.Operate(string.Format("{0}{1}", EFEM_Data.obj, TS_Robot.cboCnt.Text),
                        string.Format("{0}:{1}", TS_Robot.cboCommand.Text, EFEM_Data.Parameter));
        }

        private void btnRobot_Click(object sender, EventArgs e)
        {
            if (!TS_Robot.Visible)
            {
                TS_Robot.Location = new System.Drawing.Point(300, 300);
                TS_Robot.Show();
            }
            else
            {
                if (TS_Robot.WindowState == FormWindowState.Minimized)
                    TS_Robot.WindowState = FormWindowState.Normal;
                else
                    TS_Robot.Hide();
            }
        }

        #endregion

        #endregion

        #region LP

        #region Event

        private void LP_EventContol(string PortName, SocketCommand command, bool result, string reply)
        {
            int Index = Convert.ToInt16(PortName.Substring(NormalStatic.LP.Length, 1)) - 1;

            UI.Log(NormalStatic.LP, PortName, SystemList.CommandComplete, string.Format("{0}:({1})", command, result));

            if (result)
            {
                //LP[Index].Result = CommandResult.OK;//Elijah

                switch (command)
                {
                    case SocketCommand.Initial:
                        {
                            UI.InitialSystem(LP[Index].DeviceName, NormalStatic.True, ErrorList.MaxCnt);
                            return;
                        }
                    default:
                        {
                            SocketClient.Send_NormalReceive(command, PortName, reply);
                            return;
                        }
                }
            }
            else
            {
                switch (command)
                {
                    case SocketCommand.Initial:
                        {
                            UI.InitialSystem(LP[Index].DeviceName, NormalStatic.False, LP[Index].NowErrorList);
                            //LP[Index].Result = CommandResult.Fail; //Elijah
                            return;
                        }
                }
                SocketClient.Send_AlarmReceive(command, PortName, LP[Index].NowErrorList, LP[Index].NowErrorMsg);
                //LP[Index].Result = CommandResult.Fail; //Elijah
            }
        }

        #endregion

        #region Interlock

        private bool LP_CheckRobot_Interlock(ref int port)
        {
            if (Robot[0].Ui_Connect == false)
            {
                Error = ErrorList.LP_RobotUnknown_0613;
                return true;
            }

            if (Robot[0].Ui_RobotPos >= RobotPosition.P1)// && Robot[0].Ui_RobotPos <= RobotPosition.P2) //20211221_Elijah
            {
                if (((int)(Robot[0].Ui_RobotPos - RobotPosition.P1) % 6) == port && (Robot[0].Ui_ArmStatusX != ArmStatus.Arm_Home || Robot[0].Ui_ArmStatusY != ArmStatus.Arm_Home))
                {
                    Error = ErrorList.LP_RobotIntrude_0614;
                    return true;
                }
            }

            return false;
        }

        private bool LP_CheckMovNormal_Interlock(ref int port)
        {
            if (LP[port].Ui_Busy)
            {
                Error = ErrorList.LP_IsBusy_0610;

                //if (LP[port].Result == CommandResult.Unknown)
                //{
                //    ErrorMsg = string.Format("(Execute Command:{0})", LP[port].NowCommand());
                //}
                //else if (LP[port].Result == CommandResult.Fail)
                //{
                //    ErrorMsg = string.Format("(Error Occurred:{0})", LP[port].NowErrorList);
                //}
                //else if (LP[port].Result == CommandResult.OK)
                //{
                //    ErrorMsg = string.Format("(Robot Command:{0})", Robot[0].GetCommand());
                //}

                return true;
            }

            //if (EtherCat1.Ui_Connect == false)
            //{
            //    Error = ErrorList.LP_IoDeviceDisc_0600;
            //    return true;
            //}
            return false;
        }

        private bool LP_CheckGetNormal_Interlock(ref int port)
        {
            if (LP[port].Ui_Busy)
            {
                Error = ErrorList.LP_IsBusy_0610;

                //if (LP[port].Result == CommandResult.Unknown)
                //{
                //    ErrorMsg = string.Format("(Execute Command:{0})", LP[port].NowCommand());
                //}
                //else if (LP[port].Result == CommandResult.Fail)
                //{
                //    ErrorMsg = string.Format("(Error Occurred:{0})", LP[port].NowErrorList);
                //}
                //else if (LP[port].Result == CommandResult.OK)
                //{
                //    ErrorMsg = string.Format("(Robot Command:{0})", Robot[0].GetCommand());
                //}

                return true;
            }


            return false;
        }

        private bool LP_CheckOutdoor_Interlock(ref int port)
        {
            //if (LP[port].Ui_OutDoor != LPDoor.Close)
            //{
            //    Error = ErrorList.LP_OutdoorOpened_0618;

            //    return true;
            //}
            return false;
        }

        private bool LP_CheckIndoor_Interlock(ref int port)
        {
            //if (LP[port].Ui_InDoor != LPDoor.Close)
            //{
            //    Error = ErrorList.LP_IndoorOpened_0619;
            //    return true;
            //}
            return false;
        }

        private bool LP_CheckHome_Interlock(ref int port)
        {
            if (LP[port].Ui_LoadStatus != LPPosition.Unload)
            {
                Error = ErrorList.LP_04Interlock_0447;
                return true;
            }
            return false;
        }

        private bool LP_CheckCassette_Interlock(ref int port)
        {
            //if (LP[port].Ui_Protrude)
            //{
            //    Error = ErrorList.LP_Protrude_0615;
            //    return true;
            //}

            //if (LP[port].Ui_Presence == LPFoupMount.Absent)
            //{
            //    Error = ErrorList.LP_CstAbsent_0611;
            //    return true;
            //}
            //if (LP[port].Ui_Presence == LPFoupMount.Unknown)
            //{
            //    Error = ErrorList.LP_CstAbnormal_0612;
            //    return true;
            //}

            return false;
        }

        private bool LP_CheckGrating_Interlock(ref int port)
        {
            //if ( LP[port].Ui_Grating )
            //{
            //    Error = ErrorList.LP_Grating_0616;
            //    return true;
            //}
            return false;
        }

        #endregion

        #region Command

        private bool LP_Initial(int port)
        {
            LP[port].Cmd_EnQueue(SocketCommand.Initial);
            return true;
        }

        private bool LP_ResetError(int port)
        {
            LP[port].Cmd_EnQueue(SocketCommand.ResetError);

            return true;
        }

        private bool LP_InitialHomeCommand(int port)
        {
            if (LP_CheckMovNormal_Interlock(ref port))
            {
                return false;
            }

            if (LP_CheckGrating_Interlock(ref port))
            {
                return false;
            }

            //if (LP[port].Ui_Protrude)
            //{
            //    Error = ErrorList.LP_Protrude_0615;
            //    return false;
            //}

            LP[port].Cmd_EnQueue(SocketCommand.InitialHome);
            return true;
        }

        private bool LP_HomeCommand(int port)
        {
            if (LP_CheckMovNormal_Interlock(ref port))
            {
                return false;
            }

            if (LP_CheckRobot_Interlock(ref port))
            {
                return false;
            }

            //if(LP[port].Ui_OutDoor != LPDoor.Close) //20201028 Walson追加，只有外門未關閉時才需要判斷光柵Interlock
            //{                
            //    if (LP_CheckGrating_Interlock(ref port))
            //    {
            //        return false;
            //    }
            //}

            //if (LP[port].Ui_InDoor != LPDoor.Close) //20201028 Walson追加，只有內門未關閉時才需要判斷突片Interlock
            //{
            //    if (LP[port].Ui_Protrude)
            //    {
            //        Error = ErrorList.LP_Protrude_0615;
            //        return false;
            //    }
            //}

            LP[port].Cmd_EnQueue(SocketCommand.Home);
            return true;
        }

        private bool LP_MoveOutDoorCommand(int port, SocketCommand cmd)
        {
            if (LP_CheckMovNormal_Interlock(ref port))
            {
                return false;
            }
            if (LP_CheckIndoor_Interlock(ref port))
            {
                return false;
            }

            if (LP_CheckGrating_Interlock(ref port))
            {
                return false;
            }

            LP[port].Cmd_EnQueue(cmd);
            return true;
        }

        private bool LP_MoveClampCommand(int port, SocketCommand cmd)
        {
            if (LP_CheckMovNormal_Interlock(ref port))
            {
                return false;
            }

            if (cmd != SocketCommand.UnClamp)
            {
                if (LP_CheckOutdoor_Interlock(ref port))
                {
                    return false;
                }
            }


            if (LP_CheckIndoor_Interlock(ref port))
            {
                return false;
            }

            LP[port].Cmd_EnQueue(cmd);
            return true;
        }

        private bool LP_MoveUnLoadCommand(int port, SocketCommand cmd)
        {
            if (LP_CheckMovNormal_Interlock(ref port))
            {
                return false;
            }

            if (LP_CheckRobot_Interlock(ref port))
            {
                return false;
            }

            if (LP_CheckOutdoor_Interlock(ref port))
            {
                return false;
            }

            if (LP_CheckCassette_Interlock(ref port))
            {
                return false;
            }

            LP[port].Cmd_EnQueue(cmd);
            return true;
        }

        private bool LP_MoveLoadCommand(int port, SocketCommand cmd)
        {
            if (LP_CheckMovNormal_Interlock(ref port))
            {
                return false;
            }

            if (LP_CheckOutdoor_Interlock(ref port))
            {
                return false;
            }

            if (LP_CheckCassette_Interlock(ref port))
            {
                return false;
            }

            LP[port].Cmd_EnQueue(cmd);
            return true;
        }
        private bool LP_GetCommand(int port, string str, SocketCommand cmd)
        {
            //20211210_Elijah
            //if (LP_CheckMappingDone_Interlock(ref port))
            //{
            //    return false;
            //}
            //if (EtherCat1.Ui_Connect == false)
            //{
            //    Error = ErrorList.LP_IoDeviceDisc_0600;
            //    return false;
            //}
            LP[port].DataSend_Parameter_string = str;
            LP[port].Cmd_EnQueue(cmd); //20211123_Elijah
            return true;
        }

        private bool LP_SetUnLoadCommand(int port, string str, SocketCommand cmd)
        {
            if (LP_CheckGetNormal_Interlock(ref port))
            {
                return false;
            }

            if (LP_CheckIndoor_Interlock(ref port))
            {
                return false;
            }

            //20211213_Elijah
            if (LP_CheckHome_Interlock(ref port))
            {
                return false;
            }


            LP[port].DataSend_Parameter_string = str;
            LP[port].Cmd_EnQueue(cmd);
            return true;
        }

        private bool LP_SetCommand(int port, string str, SocketCommand cmd)
        {
            if (LP_CheckGetNormal_Interlock(ref port))  // Walson 20191029
            {
                return false;
            }
            LP[port].DataSend_Parameter_string = str;
            LP[port].Cmd_EnQueue(cmd);
            return true;
        }

        #endregion

        #region public
        public int[] LP_SlotData(int port) { return LP[port].GetSlotData; }
        public int LP_SlotCnt(int port) { return LP[port].GetSlotData.Length; }
        public string LPDevice(int port) { return LP[port].DeviceName; }
        public bool LPBusy(int port) { return LP[port].Ui_Busy; }
        public void LP_SetSlotData(int port, int slot, int Value)
        {
            LP[port].SetSlotData(slot, Value);
        }

        //public LPDoor LP_GetInDoorOpen(int port) { return LP[port].Ui_InDoor; }
        //public LPPosition LPStatus(int port) { return (LP[port].Ui_InDoor == LPDoor.Open) ? LPPosition.Load : LPPosition.InProcess; }

        //public LPFoupMount LPFoup(int port) { return LP[port].Ui_Presence; }

        #endregion

        #region TS_UI

        private void SendLPCommand_Click(object sender, EventArgs e)
        {
            int port = Convert.ToInt32(TS_LoadPort.cboCnt.Text) - 1;

            //if (EtherCat1.Ui_Connect == false)
            //{
            //    UI.Error(NormalStatic.EtherCat, ErrorList.LP_IoDeviceDisc_0600);
            //    return;
            //}

            EFEM_Data.command = (SocketCommand)Enum.Parse(typeof(SocketCommand), TS_LoadPort.cboCommand.Text);
            EFEM_Data.port = port;
            EFEM_Data.obj = NormalStatic.LP;

            switch (EFEM_Data.command)
            {
                case SocketCommand.Initial:
                case SocketCommand.InitialHome:
                case SocketCommand.ResetError:
                case SocketCommand.Home:
                case SocketCommand.GetStatus:
                case SocketCommand.Clamp:
                case SocketCommand.UnClamp:
                case SocketCommand.Load:
                case SocketCommand.Unload:
                case SocketCommand.Map: //20211209_Elijah
                case SocketCommand.GetProtrusionSensor:
                case SocketCommand.GetLEDStatus:
                case SocketCommand.GetWaferSlot: //20211209_Elijah
                case SocketCommand.GetWaferThickness:
                case SocketCommand.GetWaferPosition:
                case SocketCommand.GetMapp:
                    //case SocketCommand.OpenOutDoor:
                    //case SocketCommand.CloseOutDoor:
                    {
                        Command_EnQueue(EFEM_Data);
                    }
                    break;

                case SocketCommand.SetType:
                    {
                        EFEM_Data.Parameter = (Int32.Parse(TS_LoadPort.cboType.Text) - 1).ToString();
                        //Command_EnQueue(EFEM_Data); //20220330_Elijah

                        //20220330_Elijah
                        if (LP_SetUnLoadCommand(EFEM_Data.port, EFEM_Data.Parameter, EFEM_Data.command))
                        {
                            ////20211208_Elijah added for auto change aligner wafer type
                            //EFEM_Data.command = SocketCommand.SetAlignerWaferType;
                            //EFEM_Data.port = 0;
                            //EFEM_Data.obj = NormalStatic.Aligner;
                            //if (EFEM_Data.Parameter == "0") // 12 inch
                            //{
                            //    EFEM_Data.Parameter = "05";

                            //    DateTime cmdTime = DateTime.Now;
                            //    while (true)
                            //    {
                            //        SpinWait.SpinUntil(() => false, 100);
                            //        if ((DateTime.Now - cmdTime).TotalSeconds > 10)
                            //        {
                            //            Error = ErrorList.AL_05Error_0452;
                            //            break;
                            //        }
                            //        Application.DoEvents();
                            //    }

                            //}

                            //Command_EnQueue(EFEM_Data);
                        }

                        //Command_EnQueue();
                    }
                    break;
            }

            UI.Operate(string.Format("{0}{1}", EFEM_Data.obj, TS_LoadPort.cboCnt.Text),
                       string.Format("{0}:{1}", TS_LoadPort.cboCommand.Text, EFEM_Data.Parameter));
        }

        private void btnLoadPort_Click(object sender, EventArgs e)
        {
            if (!TS_LoadPort.Visible)
            {
                TS_LoadPort.Location = new System.Drawing.Point(1300, 300);
                TS_LoadPort.Show();
            }
            else
                TS_LoadPort.Hide();
        }

        #endregion

        #endregion

        #region Aligner

        public string AlignerDevice(int port) { return Aligner[port].DeviceName; }
        public bool AlignerBusy(int port) { return Aligner[port].Ui_Busy; }
        public WaferStatus AlignerPresence(int port) { return Aligner[port].Ui_Presence; }
        public AlignerStatus AlignerUnitStatus(int port) { return Aligner[port].Ui_Status; }
        public string AlignerOCRDegree(int port) { return Aligner[port].Ui_NotchAngle; }
        public string AlignerEQDegree(int port) { return Aligner[port].Ui_ToAngle; }
        public LiftPinEnum AlignerLiftPin(int port) { return Aligner[port].Ui_LiftPin; }
        public CommandResult AlignerCommandResult(int port) { return Aligner[port].Result; }

        public void Aligner_SetInfor(string value) //Walson 20201124
        {
            if (value == ",0/,0")
                value = "";

            Aligner[0].Ui_WaferInfo = value;
        }

        #region Event

        private void AlignerEventContol(string PortName, SocketCommand command, bool Result)
        {
            int port = Convert.ToInt16(PortName.Substring(NormalStatic.Aligner.Length, 1)) - 1;

            Aligner[port].Result = Result ? CommandResult.OK : CommandResult.Fail;

            if (Result)
            {
                switch (command)
                {
                    case SocketCommand.Initial:
                        {
                            Aligner[port].Ui_Connect = true;
                            UI.InitialSystem(Aligner[port].DeviceName, NormalStatic.True, ErrorList.MaxCnt);
                        }
                        break;

                    case SocketCommand.Auto:
                        {
                            #region Update_SQL

                            //if (frmMain.EFEM_NowStatus == EFEMStatus.Run_Now)
                            //{
                            //    SQL_AlignerEventUpdate(port); 
                            //}

                            #endregion
                        }
                        break;
                    case SocketCommand.Alignment:
                        //Wayne Add For Flow Test 20220822
                        if (UserCore.CurrentJobIndex < UserCore.ls_JobInfo.Count) //Step 轉換
                        {
                            UserCore.ls_JobInfo[UserCore.CurrentJobIndex].CurrentStep = SQLWaferInforStep.ReadWaferID_Send;
                        }

                        break;
                }
                UI.Log(NormalStatic.System, Aligner[port].DeviceName, SystemList.CommandComplete, command.ToString());
            }
            else
            {
                switch (command)
                {
                    case SocketCommand.Initial:
                        {
                            Aligner[port].Ui_Connect = false;
                            UI.InitialSystem(Aligner[port].DeviceName, NormalStatic.False, ErrorList.AP_InitialFail_0393);
                        }
                        break;

                }

                UI.Alarm(Aligner[port].DeviceName, Aligner[port].NowErrorList);
            }
        }

        #endregion

        #region Interlock

        private bool AlignerCheckBusy()
        {
            if (Aligner[QueuedData.port].Ui_Busy)
            {
                Error = ErrorList.DeviceIsBusy_0301;
                return true;
            }

            if (Aligner[QueuedData.port].Ui_Connect == false)
            {
                Error = ErrorList.AP_SerialError_0381;
                return true;
            }

            return false;
        }

        private bool AlignerCheckPresence(WaferStatus flag)
        {
            if (Aligner[QueuedData.port].Ui_Presence == flag)
                return false;

            if (Aligner[QueuedData.port].Ui_Presence == WaferStatus.With)
                Error = ErrorList.RB_DeviceWith_0306;
            else
                Error = ErrorList.RB_DeviceWithout_0307;

            return true;
        }

        private bool AlignerCheckHome(AlignerStatus status)
        {
            if (Aligner[QueuedData.port].Ui_Status == status)
                return false;
            else
            {
                Error = ErrorList.AL_NotHome_0341;
            }
            return true;
        }

        private bool AlignerCheckVacuum(ref int port, bool flag)
        {
            if (Aligner[port].Ui_Vac == flag)
                return false;
            else
            {
                Error = ErrorList.AL_VauccmFail_0342;
            }

            return true;
        }

        #endregion

        #region Command

        private void Aligner_ReConnect(int port)
        {
            Aligner[port].COM_Connect();
        }

        private void Aligner_Disconnect(int port)
        {
            Aligner[port].COM_Disconnect();
        }

        private bool AlignerInitial()
        {
            Aligner_ReConnect(QueuedData.port);

            if (Aligner[QueuedData.port].Ui_Connect == true)
            {
                Aligner[QueuedData.port].Cmd_EnQueue(QueuedData.command);
            }

            return true;
        }

        private bool AlignerNotLockMethod()
        {
            Aligner[QueuedData.port].Cmd_EnQueue(QueuedData.command);
            return true;
        }

        private bool AlignerSetMethod()
        {
            if (AlignerCheckBusy())
                return false;

            if (AlignerCheckHome(AlignerStatus.Home))
                return false;

            switch (QueuedData.command)
            {
                case SocketCommand.SetAlignerDegree:
                    Aligner[QueuedData.port].strParam[0] = QueuedData.Parameter;
                    break;
                case SocketCommand.SetIDReaderDegree:
                    Aligner[QueuedData.port].strParam[1] = QueuedData.Parameter;
                    break;
            }

            Aligner[QueuedData.port].Cmd_EnQueue(QueuedData.command);

            return true;
        }

        private bool AlignerMoveMethod()
        {
            if (AlignerCheckBusy())
                return false;

            if (QueuedData.command != SocketCommand.Home
            && QueuedData.command != SocketCommand.InitialHome
            && QueuedData.command != SocketCommand.LiftPinUp
            && QueuedData.command != SocketCommand.LiftPinDown
            && QueuedData.command != SocketCommand.CycleHomeCheckDegree
                )
            {
                if (AlignerCheckPresence(WaferStatus.With))
                    return false;
            }

            if (Robot1_DeviceCheckArm())
            {
                return false;
            }

            if (QueuedData.command == SocketCommand.CycleHomeCheckDegree)
            {
                string[] SplitTemp = QueuedData.Parameter.Split(',');
                Aligner[QueuedData.port].strParam[0] = SplitTemp[0];
                Aligner[QueuedData.port].strParam[1] = SplitTemp[1];
            }

            Aligner[QueuedData.port].Cmd_EnQueue(QueuedData.command);
            return true;
        }

        private bool AlignerGetMethod()
        {
            if (AlignerCheckBusy())
                return false;

            Aligner[QueuedData.port].Cmd_EnQueue(QueuedData.command);

            return true;
        }

        #endregion

        #region TS_UI

        private void SendAlignerCommand_Click(object sender, EventArgs e)
        {
            int port = Convert.ToInt32(TS_Aligner.cboCnt.Text) - 1;

            if (Aligner[port].Ui_Connect == false)
            {
                Error = ErrorList.AP_SerialError_0381;
                return;
            }

            EFEM_Data.command = (SocketCommand)Enum.Parse(typeof(SocketCommand), TS_Aligner.cboCommand.Text);
            EFEM_Data.port = port;
            EFEM_Data.obj = NormalStatic.Aligner;

            switch (EFEM_Data.command)
            {
                case SocketCommand.ResetError:
                case SocketCommand.Home:
                case SocketCommand.GetStatus:
                case SocketCommand.GetAlignerDegree:
                case SocketCommand.GetIDReaderDegree:
                case SocketCommand.GetAlignerWaferType:
                case SocketCommand.FindNotch:
                case SocketCommand.ToAngle:
                case SocketCommand.LiftPinDown:
                case SocketCommand.LiftPinUp:
                case SocketCommand.AlignerVacuum_on:
                case SocketCommand.AlignerVacuum_off:
                    {
                        Command_EnQueue(EFEM_Data);
                    }
                    break;

                case SocketCommand.Alignment:
                case SocketCommand.SetAlignerDegree:
                case SocketCommand.SetIDReaderDegree:
                    {
                        EFEM_Data.Parameter = ((Math.Round(Convert.ToDouble(TS_Aligner.txtDegree.Text), 1, MidpointRounding.AwayFromZero) * 10).ToString()).PadLeft(4, '0');
                        Command_EnQueue(EFEM_Data);
                    }
                    break;

                case SocketCommand.SetAlignerWaferType:
                    {
                        if (HCT_EFEM.EFEM_HasTable.Aligner_GetType.Contains(TS_Aligner.cboType.Text))
                        {
                            EFEM_Data.Parameter = HCT_EFEM.EFEM_HasTable.Aligner_GetType[TS_Aligner.cboType.Text].ToString();
                        }
                        //EFEM_Data.Parameter = TS_Aligner.cboType.Text;
                        Command_EnQueue(EFEM_Data);
                    }
                    break;

                case SocketCommand.AlignerVacuum:
                    {
                        EFEM_Data.Parameter = TS_Aligner.cboOnOff.Text;
                        Command_EnQueue(EFEM_Data);
                    }
                    break;
            }

            UI.Operate(string.Format("{0}{1}", EFEM_Data.obj, TS_Aligner.cboCnt.Text),
                       string.Format("{0}:{1}", TS_Aligner.cboCommand.Text, EFEM_Data.Parameter));
        }

        private void btnAligner_Click(object sender, EventArgs e)
        {
            if (!TS_Aligner.Visible)
            {
                TS_Aligner.Location = new System.Drawing.Point(600, 600);
                TS_Aligner.Show();
            }
            else
            {
                if (TS_Aligner.WindowState == FormWindowState.Minimized)
                    TS_Aligner.WindowState = FormWindowState.Normal;
                else
                    TS_Aligner.Hide();
            }
        }

        #endregion

        #endregion

        #region RFID

        #region Event
        private void RFIDEventContol(string PortName, SocketCommand command, bool Result)
        {
            int port = Convert.ToInt16(PortName.Substring(NormalStatic.RFID.Length, 1)) - 1;

            UI.Log(NormalStatic.RFID, PortName, SystemList.CommandComplete, string.Format("{0}:({1})", command, Result));

            if (Result)
            {
                switch (command)
                {
                    case SocketCommand.Initial:
                        {
                            RFID[port].Ui_Connect = true;
                            UI.InitialSystem(RFID[port].DeviceName, NormalStatic.True, ErrorList.MaxCnt);
                        }
                        break;

                    case SocketCommand.ReadFoupID:
                        {
                            SocketClient.Send_NormalReceive(command, RFID[port].DeviceName, RFID[port].Ui_FoupID);
                        }
                        break;

                    case SocketCommand.SetPageMap:
                        {
                            AppSetting.SaveSetting(string.Format("{0}_Page", PortName), RFID[port].Now_Page);
                            SocketClient.Send_NormalReceive(command, RFID[port].DeviceName, "");
                        }
                        break;
                }
            }
            else
            {
                switch (command)
                {
                    case SocketCommand.Initial:
                        {
                            RFID[port].Ui_Connect = false;
                            UI.InitialSystem(RFID[port].DeviceName, NormalStatic.False, RFID[port].NowErrorList);
                        }
                        break;

                    default:
                        {
                            //  SocketClient.Send_AlarmReceive(command, RFID[port].DeviceName, RFID[port].NowErrorList, RFID[port].NowErrorMsg);
                        }
                        break;
                }
            }
        }

        #endregion

        #region Public

        #endregion

        #region Interlock

        private bool RFIDCheckInterlock(ref int port)
        {
            if (RFID[port].Ui_Connect == false)
            {
                Error = ErrorList.RF_ComPortDisabled_0673;
                return true;
            }

            if (RFID[port].Ui_Busy)
            {
                Error = ErrorList.RF_IsBusy_0680;
                return true;
            }

            return false;
        }

        #endregion

        #region Command

        private void RFID_ReConnect(int port)
        {
            RFID[port].COM_Connect();
        }

        private void RFID_Disconnect(int port)
        {
            RFID[port].COM_Disconnect();
        }

        private bool RFIDInitial(int port, SocketCommand cmd)
        {
            RFID_ReConnect(port);

            if (RFID[port].Ui_Connect == true)
            {
                RFID[port].Cmd_EnQueue(SocketCommand.Initial);
            }
            return true;
        }

        private bool RFIDReadFoupID(int port, SocketCommand cmd)
        {
            if (RFIDCheckInterlock(ref port))
            {
                return false;
            }

            RFID[port].Cmd_EnQueue(cmd);
            return true;
        }

        private bool RFIDSetPageMap(int port, SocketCommand cmd)
        {
            if (RFIDCheckInterlock(ref port))
            {
                return false;
            }

            RFID[port].Cmd_EnQueue(cmd);
            return true;
        }

        #endregion

        #region TS_UI

        private void SendRFIDCommand_Click(object sender, EventArgs e)
        {
            int port = Convert.ToInt32(TS_RFID.cboCnt.Text) - 1;

            if (RFID[port].Ui_Connect == false)
            {
                UI.Error(RFID[port].DeviceName, ErrorList.RF_ComPortDisabled_0673);
                return;
            }

            EFEM_Data.command = (SocketCommand)Enum.Parse(typeof(SocketCommand), TS_RFID.cboCommand.Text);
            EFEM_Data.port = port;
            EFEM_Data.obj = NormalStatic.RFID;

            switch (EFEM_Data.command)
            {
                case SocketCommand.ReadFoupID:
                    {
                        Command_EnQueue(EFEM_Data);
                    }
                    break;

                case SocketCommand.SetPageMap:
                    {
                        string strCollected = string.Empty;
                        for (int i = 0; i < TS_RFID.clstPage.Items.Count; i++)
                        {
                            if (TS_RFID.clstPage.GetItemChecked(i))
                            {
                                if (strCollected == string.Empty)
                                {
                                    strCollected = TS_RFID.clstPage.GetItemText(TS_RFID.clstPage.Items[i]);
                                }
                                else
                                {
                                    strCollected = string.Format("{0},{1}", strCollected, TS_RFID.clstPage.GetItemText(TS_RFID.clstPage.Items[i]));
                                }
                            }
                        }
                        EFEM_Data.Parameter = strCollected;
                        Command_EnQueue(EFEM_Data);
                    }
                    break;

            }

            UI.Operate(string.Format("{0}{1}", EFEM_Data.obj, TS_RFID.cboCnt.Text),
                       string.Format("{0}:{1}", TS_RFID.cboCommand.Text, EFEM_Data.Parameter));
        }

        private void btnRFID_Click(object sender, EventArgs e)
        {
            if (!TS_RFID.Visible)
            {
                TS_RFID.Location = new System.Drawing.Point(1300, 300);
                TS_RFID.Show();
            }
            else
                TS_RFID.Hide();
        }

        #endregion

        #endregion

        #region Adam

        #region EQIO
        static bool _Do_01, _Do_02, _Do_03, _Do_04, _Do_05, _Do_06;
        // bool _Di_01, _Di_02, _Di_03, _Di_04, _Di_05, _Di_06;
        //true 換成讀取IO的指令
        public static bool EQ_Di_01
        {
            get
            {
                bool[] value;
                Adam[1].readTheDI(out value);
                return value[1];
            }
        } //下流出版信號
        public static bool EQ_Di_02
        {
            get
            {
                bool[] value;
                Adam[1].readTheDI(out value);
                return value[2];
            }
        } //空
        public static bool EQ_Di_03
        {
            get
            {
                bool[] value;
                Adam[1].readTheDI(out value);
                return value[3];
            }
        } //下流停止作業
        public static bool EQ_Di_04
        {
            get
            {
                bool[] value;
                Adam[1].readTheDI(out value);
                return value[4];
            }
        } //空
        public static bool EQ_Di_05
        {
            get
            {
                bool[] value;
                Adam[1].readTheDI(out value);
                return value[5];
            }
        } //空
        public static bool EQ_Di_06
        {
            get
            {
                bool[] value;
                Adam[1].readTheDI(out value);
                return value[6];
            }
        } //下流異常信號


        public static bool EQ_Do_01
        {
            get { return _Do_01; }
            set
            {
                string set_value = (value == true) ? "1" : "0";

                Adam[1].SetDO(SocketCommand.MaxCnt, "", 0, set_value);

                Console.WriteLine("set Do_1 {0}", value);
                _Do_01 = value;
            }
        }
        // ^ Ready                                                             
        public static bool EQ_Do_02
        {
            get { return _Do_02; }
            set
            {
                string set_value = (value == true) ? "1" : "0";

                Adam[1].SetDO(SocketCommand.MaxCnt, "", 1, set_value);
                Console.WriteLine("set Do_2 {0}", value);
                _Do_02 = value;
            }
        }
        // ^ 出版完成                                                          
        public static bool EQ_Do_03
        {
            get { return _Do_03; }
            set
            {
                string set_value = (value == true) ? "1" : "0";

                Adam[1].SetDO(SocketCommand.MaxCnt, "", 2, set_value);
                Console.WriteLine("set Do_3 {0}", value);
                _Do_03 = value;
            }
        }
        // ^ 回收作業完成                                                      
        public static bool EQ_Do_04
        {
            get { return _Do_04; }
            set
            {
                string set_value = (value == true) ? "1" : "0";

                Adam[1].SetDO(SocketCommand.MaxCnt, "", 3, set_value);
                Console.WriteLine("set Do_4 {0}", value);
                _Do_04 = value;
            }
        }
        // ^ 空                                                                
        public static bool EQ_Do_05
        {
            get { return _Do_05; }
            set
            {
                string set_value = (value == true) ? "1" : "0";

                Adam[1].SetDO(SocketCommand.MaxCnt, "", 4, set_value);
                Console.WriteLine("set Do_6 {0}", value);
                _Do_05 = value;
            }
        }
        // ^ 安全信號                                                          
        public static bool EQ_Do_06
        {
            get { return _Do_06; }
            set
            {
                string set_value = (value == true) ? "1" : "0";

                Adam[1].SetDO(SocketCommand.MaxCnt, "", 5, set_value);
                Console.WriteLine("set Do_7 {0}", value);
                _Do_06 = value;
            }
        }

        public void SetAdamDo(int refAdamIdx, int DoIndex, string Value)
        {
            Adam[refAdamIdx].SetDO(SocketCommand.MaxCnt, "", DoIndex, Value);
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        // ^ 異常信號
        #endregion

        #region EQ_commend

        public void EQ_INI()
        {
            EQ_Do_01 = false;
            EQ_Do_02 = false;
            EQ_Do_03 = false;
            EQ_Do_04 = false;
            EQ_Do_05 = true;
            EQ_Do_06 = true;



        }

        public static int common_docking0()
        {
            timmmer t1 = new timmmer();
            if (EQ_Di_06 != true)
                return 1;//結合異常
            EQ_Do_05 = true;
            EQ_Do_01 = true;

            while (t1.start(5000))
            {
                if (EQ_Di_01 == true)
                    break;
                //    else
                //         return 2;  //下流出板信號time out
            }
            if (t1.start(100))
            {
                // UI.InitialSystem(NormalStatic.Error, NormalStatic.True, ErrorList.IO_EQtimeout_0902);
                t1.reset();
            }
            t1.reset();

            EQ_Do_05 = false;
            EQ_Do_01 = false;

            return 0;
        }

        public static int common_docking1()
        {
            timmmer t1 = new timmmer();

            ////手臂置入
            //while (t1.start(100))//需修改成完成前
            //{
            //    if (EQ_Di_01 == false && EQ_Di_06 == true)
            //    {

            //        //手臂回收
            //        //回收完成
            //        EQ_Do_05 = true;
            //        return 3;
            //    }
            //    else if (EQ_Di_06 == false)//結合異常
            //    {
            //        while (EQ_Di_06 == true)//結合機異常排除
            //        { }

            //        //手臂回收
            //        //回收完成
            //        EQ_Do_05 = true;
            //        return 4;
            //    }
            //}
            //t1.reset();
            //手臂回收
            Thread.Sleep(100);
            //回收完成

            EQ_Do_05 = true;
            EQ_Do_02 = true;
            while (t1.start(100))
            {
                if (EQ_Di_01 == false)
                    break;
                //            else
                //             return 5;  //下流出板信號time out
            }
            if (t1.start(100))
                // UI.InitialSystem(NormalStatic.Error, NormalStatic.True, ErrorList.IO_EQtimeout_0902);

                t1.reset();

            EQ_Do_02 = false;


            return 0;
        }
        public static int process_stop()
        {
            timmmer t1 = new timmmer();
            if (EQ_Di_06 != true)
                return 1;//結合異常
            if (EQ_Di_03 != true)
                return 2;//正常不用退片

            //退片流程
            EQ_Do_05 = false;
            //開始退片
            Thread.Sleep(100);
            //退片完成
            EQ_Do_05 = true;
            EQ_Do_03 = true;

            while (t1.start(3000))
            {
                if (EQ_Di_03 == false)
                    break;

            }
            EQ_Do_03 = false;

            return 0;
        }




        public static int common_undocking()
        {
            timmmer t1 = new timmmer();

            if (EQ_Di_01 != true)
                return 0;//不須要退片
            EQ_Do_01 = true;
            EQ_Do_05 = false;
            //取片動作
            //while (t1.start(100))//需修改成完成前
            //{
            //    if (Di_06 == false)//EQ發生異常
            //    {
            //        Do_01 = false;
            //        while (Di_06 == true)
            //        {
            //        }
            //        //手臂賦歸開始
            //        //手臂賦歸完成
            //        Do_05 = true;
            //        return 1; // 分離機異常流程完畢
            //    }
            //    if (false)//自己發生異常
            //    {
            //        Do_06 = false;
            //    }
            //}
            //t1.reset();
            ////取片完成
            //Do_05 = true;
            //Do_02 = true;
            //Thread.Sleep(100);
            //Do_01 = false;
            //Thread.Sleep(100);
            //Do_02 = false;
            return 0;
        }

        public static int common_undockingEnd()
        {
            timmmer t1 = new timmmer();

            //if (Di_01 != true)
            //    return 0;//不須要退片
            //Do_01 = true;
            //Do_05 = false;
            ////取片動作
            //while (t1.start(100))//需修改成完成前
            //{
            //    if (Di_06 == false)//EQ發生異常
            //    {
            //        Do_01 = false;
            //        while (Di_06 == true)
            //        {
            //        }
            //        //手臂賦歸開始
            //        //手臂賦歸完成
            //        Do_05 = true;
            //        return 1; // 分離機異常流程完畢
            //    }
            //    if (false)//自己發生異常
            //    {
            //        Do_06 = false;
            //    }
            //}
            //t1.reset();
            //取片完成
            EQ_Do_05 = true;
            EQ_Do_02 = true;
            Thread.Sleep(100);
            EQ_Do_01 = false;
            Thread.Sleep(100);
            EQ_Do_02 = false;
            return 0;
        }
        #endregion

        public void EFEMPageChange(int index)
        {
            tctlEFEM.SelectTab(index);
        }

        public void AuthorityChange(bool CoreBusy)
        {
            if (InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate () { AuthorityChange(CoreBusy); }));
                return;
            }

            if (HT.EFEM.Authority == AuthorityTable.Operator || CoreBusy || HT.EFEM.Status == EFEMStatus.Power_Off || HT.EFEM.Mode != EFEMMode.Local)
                gbxButton.Visible = false; //Mike
            else
                gbxButton.Visible = true;
        }
        private void AdamSetResult(int Index, SocketCommand Cmd, string paramater, bool Result)
        {
            if (Cmd == SocketCommand.MaxCnt)  // 20191029
            {
                //if (paramater == "TR_Off")
                //{
                //    if (Robot[0].Ui_Busy && Adam[0].DO[(int)Adam6050_DO.TR_1] == false)
                //        Robot[0].SetTrResult = true;
                //}

                //if (paramater == "TR_On")
                //{
                //    if (Robot[0].Ui_Busy && Adam[0].DO[(int)Adam6050_DO.TR_1])
                //        Robot[0].SetTrResult = true;
                //}
                //return;

            }

            if (!Result)
            {
                SocketClient.Send_AlarmReceive(Cmd, NormalStatic.IO, Adam[Index].NowErrorList, "");
            }
            else
            {
                SocketClient.Send_NormalReceive(Cmd, NormalStatic.IO, paramater);
            }
        }

        private void AdamDiChange(Adam6050_DI index, bool Result)
        {
            UI.Log(NormalStatic.IO, string.Format("{0}_Di", NormalStatic.Adam), SystemList.DiReceive, string.Format("{0}:({1})", Result == true ? "1" : "0", index));

            switch (index)
            {
                case Adam6050_DI.Pressure:
                case Adam6050_DI.Vacuum:
                case Adam6050_DI.Ionizer1:
                case Adam6050_DI.FFU1:
                //case Adam6050_DI.FFU2:
                //case Adam6050_DI.FFU3:
                //case Adam6050_DI.FFU4:
                case Adam6050_DI.Door:
                case Adam6050_DI.EMO:
                //case Adam6050_DI.FlowMeter1:
                //case Adam6050_DI.FlowMeter2:

                case Adam6050_DI.WaferLoad:
                case Adam6050_DI.WaferUnload:
                case Adam6050_DI.Z_HighLimit:
                case Adam6050_DI.X_HighLimit:
                case Adam6050_DI.Y_HighLimit:
                case Adam6050_DI.WaferDetect:
                case Adam6050_DI.MicroscopeEMO:
                    {
                        SocketClient.Send_NormalReceive(SocketCommand.Event,
                                                        string.Format("{0},{1}", NormalStatic.EFEM, index),
                                                        Result == true ? "1" : "0");
                    }
                    break;

                case Adam6050_DI.WaferShift: //20220125_Elijah
                    {

                    }
                    break;

                case Adam6050_DI.RobotMode:
                    {
                        if (Result == false)
                        {
                            for (int i = 0; i < RobotCount; i++)
                            {
                                Robot[i].Ui_Remote = false;
                            }
                        }

                        SocketClient.Send_NormalReceive(SocketCommand.Event,
                                                        string.Format("{0},{1}", NormalStatic.EFEM, index),
                                                        Result == true ? "1" : "0");
                    }
                    break;

                case Adam6050_DI.Power:
                    {
                        if (Result == false)
                        {
                            AuthorityChange(true);

                            EFEM_StatusChange(index, Result);

                        }
                        else
                        {
                            AuthorityChange(false);
                            EFEM_StatusChange(index, Result);

                        }

                        SocketClient.Send_NormalReceive(SocketCommand.Event,
                                 string.Format("{0},{1}", NormalStatic.EFEM, index),
                                 Result == true ? "1" : "0");
                    }
                    break;

            }
        }
        public bool GetAdamDIValue(int Adamindex, int DIIndex)
        {
            return Adam[Adamindex].Old_DI_data[DIIndex];
        }
        public bool GetAdamDOValue(int Adamindex, int DOIndex)
        {
            return Adam[Adamindex].DO[DOIndex];
        }

        public void PowerOffDeviceDisconnect()
        {
            for (int i = 0; i < RobotCount; i++)
            {
                //Robot_Disconnect(i);
            }

            //for (int i = 0; i < AlignerCount; i++)
            //{
            //    Aligner_Disconnect(i);
            //}
            for (int i = 0; i < RFIDCount; i++)
            {
                //RFID[i].Ui_Connect = false;
            }
        }

        public void PowerOnDeviceInitial()
        {
            CmdStruct EFEM_Data = new CmdStruct();
            for (int i = 0; i < RobotCount; i++)
            {
                EFEM_Data.port = i;
                EFEM_Data.obj = NormalStatic.Robot;

                EFEM_Data.command = SocketCommand.Initial;
                Command_EnQueue(EFEM_Data);
            }
            for (int i = 0; i < LPCount; i++)
            {
                EFEM_Data.port = i;
                EFEM_Data.obj = NormalStatic.LP;

                EFEM_Data.command = SocketCommand.Initial;
                Command_EnQueue(EFEM_Data);
            }

            //for (int i = 0; i < AlignerCount; i++)
            //{
            //    EFEM_Data.port = i;
            //    EFEM_Data.obj = NormalStatic.Aligner;

            //    EFEM_Data.command = SocketCommand.Initial;
            //    Command_EnQueue(EFEM_Data);
            //}

        }

        #endregion


        #region 多載 ReadExcel

        public static void ReadExcel(string ref_FilePath, ref List<string[,]> TempList)
        {
            FileStream ExcelFS = new FileStream(ref_FilePath, FileMode.Open);
            HSSFWorkbook WB = new HSSFWorkbook(ExcelFS);

            HSSFSheet St = (HSSFSheet)WB.GetSheetAt(0);
            int RowCount = St.LastRowNum + 1;
            int ColCount = St.GetRow(0).Cells.Count;
            string[,] tempstr = new string[RowCount, ColCount];
            TempList = new List<string[,]>();

            for (int row = 0; row < RowCount; row++)
            {
                for (int col = 0; col < ColCount; col++)
                {
                    tempstr[row, col] = St.GetRow(row).GetCell(col).ToString();
                }
            }
            TempList.Add(tempstr);

            ExcelFS.Close();
        }

        public static void ReadExcel(string ref_FilePath, ref List<string[,]>[] TempList)
        {
            lock (Write_Excel)
            {
                FileStream ExcelFS = new FileStream(ref_FilePath, FileMode.Open);
                HSSFWorkbook WB = new HSSFWorkbook(ExcelFS);


                int Ex_StCount = WB.Count;
                TempList = new List<string[,]>[Ex_StCount];

                for (int StCnt = 0; StCnt < Ex_StCount; StCnt++)
                {

                    HSSFSheet St = (HSSFSheet)WB.GetSheetAt(StCnt);
                    int RowCount = St.LastRowNum + 1;
                    int ColCount = St.GetRow(0).Cells.Count;
                    string[,] tempstr = new string[RowCount, ColCount];
                    TempList[StCnt] = new List<string[,]>();

                    for (int row = 0; row < RowCount; row++)
                    {
                        for (int col = 0; col < ColCount; col++)
                        {
                            if (St.GetRow(row).GetCell(col) == null)
                                tempstr[row, col] = null;
                            else
                                tempstr[row, col] = St.GetRow(row).GetCell(col).ToString();
                        }
                    }
                    TempList[StCnt].Add(tempstr);
                }

                ExcelFS.Close();
            }
        }


        public static void WirteExcel(int ref_sheet, int ref_row, int ref_col, string ref_value)
        {
            lock (Write_Excel)
            {
                FileStream ExcelFS = new FileStream(string.Format("{0}{1}{2}", NormalStatic.ExcelPath, NormalStatic.VID, ".xls"), FileMode.OpenOrCreate);
                HSSFWorkbook WB = new HSSFWorkbook(ExcelFS);
                ExcelFS.Close();
                HSSFSheet st = (HSSFSheet)WB.GetSheetAt(ref_sheet);
                st.GetRow(ref_row).GetCell(ref_col).SetCellValue(ref_value);


                ExcelFS = new FileStream(string.Format("{0}{1}{2}", NormalStatic.ExcelPath, NormalStatic.VID, ".xls"), FileMode.Create);
                WB.Write(ExcelFS);
                WB.Close();
                ExcelFS.Close();
            }
        }
        #endregion

        #region TS_AllClose

        public void CloseTSForm()
        {
            if (RobotCount > 0 && TS_Robot.Visible == true)
                TS_Robot.Hide();

            //if (AlignerCount > 0 && TS_Aligner.Visible == true)
            //    TS_Aligner.Hide();

            if (StageCount > 0 && TS_Stage.Visible == true)
                TS_Stage.Hide();
        }

        #endregion
        private void ULD_recive()
        {
            string str = "A";
            string[] commend;
            while (true)
            {
                SpinWait.SpinUntil(() => false, 10);
                ULD.read_log(out str);
                if (str == "")
                    continue;
                commend = str.Split(',');
                switch (commend[0])
                {
                    case "SET_mode_auto":
                        InitialCmd("SET_mode_auto");
                        break;
                    case "SET_mode_manul":
                        break;
                    case "Load":
                        EFEM_Data.command = SocketCommand.Load;
                        EFEM_Data.port = int.Parse(commend[1]) - 1;
                        EFEM_Data.obj = NormalStatic.LP;
                        Command_EnQueue(EFEM_Data);
                        break;
                    case "AllHome":
                        InitialCmd("AllHome");   //Joanne 20220830
                        break;
                    case "JobInfo":
                        break;
                    case "AlarmReset":
                        break;
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //  ULD.WaferPUT("port1", "1");
            ULD_protocol.Ui_Busy(true);
        }

        private void DoLoaderUnloaderEvent()
        {
            string str = "A";
            string[] commend;
            while (true)
            {
                try
                {
                    string Temp = bk_EventReceive.DeQueue(-1);

                    commend = Temp.Split(',');
                    switch (commend[0])
                    {
                        case "SET_mode_auto":
                            InitialCmd("SET_mode_auto");
                            break;

                        case "SET_mode_manual":
                            InitialCmd("SET_mode_manual");
                            break;

                        case "Load":
                            EFEM_Data.command = SocketCommand.Load;
                            EFEM_Data.port = int.Parse(commend[1]);
                            EFEM_Data.obj = NormalStatic.LP;
                            Command_EnQueue(EFEM_Data);
                            break;

                        case "Unload":  //Joanne 20220912 Add
                            EFEM_Data.command = SocketCommand.Unload;
                            EFEM_Data.port = int.Parse(commend[1]);
                            EFEM_Data.obj = NormalStatic.LP;
                            Command_EnQueue(EFEM_Data);
                            break;

                        case "AllHome":
                            InitialCmd("AllHome");   //Joanne 20220830
                            break;

                        case "JobInfo":

                            UserCore.ls_JobInfo = new List<UserCore.cs_JobInfo>();

                            for (int i = 1; i < commend.Length; i++)
                            {
                                string[] TempJobInfo = commend[i].Split('/');

                                int TempSocPort = -1;
                                int TempSocSlot = Convert.ToInt32(TempJobInfo[1]);

                                if (TempJobInfo[0] == "1")
                                {
                                    TempSocPort = 2;
                                }
                                else
                                {
                                    TempSocPort = 1;
                                }

                                UserCore.ls_JobInfo.Add(new UserCore.cs_JobInfo
                                {
                                    SourcePort = TempSocPort,
                                    SourceSlot = TempSocSlot,
                                    TargetPort = TempSocPort,
                                    TargetSlot = TempSocSlot,
                                    NeedAligner = false,
                                    UseOCR = false,
                                    OCRAngle = 90,
                                    UnloadAngle = 180,
                                    CurrentStep = SQLWaferInforStep.GetEQ_Send
                                });
                            }
                            InitialCmd("JobInfo");   //Joanne 20220830

                            break;

                        case "AlarmReset":
                            break;

                        case "LPTypeChange":

                            int PortIdx = int.Parse(commend[1]);

                            EFEM_Data.command = SocketCommand.SetType;
                            EFEM_Data.port = PortIdx;
                            EFEM_Data.Parameter = commend[2];
                            EFEM_Data.obj = NormalStatic.LP;
                            Command_EnQueue(EFEM_Data);
                            break;

                        case "LoaderBarcode":

                            ls_LoaderBarcode.Add(commend[1]);
                            if (InvokeRequired)
                            {
                                Invoke(new MethodInvoker(() =>
                                {
                                    RefreshBarcodeList();

                                }));
                            }
                            else
                            {

                                RefreshBarcodeList();
                            }
                            break;

                        //Joanne 20220908 Add
                        case "Set_EFEMMode_Local":
                            {
                                ModeChange(EFEMMode.Local);
                            }
                            break;

                        //Joanne 20220908 Add
                        case "Set_EFEMMode_Remote":
                            {
                                ModeChange(EFEMMode.Remote);
                            }
                            break;
                    }

                    Console.WriteLine(Temp);
                }
                catch (Exception ex)
                {
                }
            }
        }

        public void UnloaderClose()
        {
            userUnloader1.Listenser_Close();
        }

        //Barcode
        private SerialPort BarcodeCom;
        private Thread td_BarcodeRecive;

        private static string CR = "\u000D";

        private void Do_BarcodeReceive()
        {
            BarcodeCom = new SerialPort();
            this.BarcodeCom.BaudRate = 115200;         // 9600, 19200, 38400, 57600 or 115200
            this.BarcodeCom.DataBits = 8;              // 7 or 8
            this.BarcodeCom.Parity = Parity.Even;    // Even or Odd
            this.BarcodeCom.StopBits = StopBits.One;   // One or Two
            this.BarcodeCom.PortName = "COM34";

            string RemainingString = "";
            BarcodeCom.Open();

            while (true)
            {
                try
                {
                    Thread.Sleep(1);

                    if (BarcodeCom.IsOpen)
                    {
                        while (BarcodeCom.BytesToRead > 0)
                        {
                            RemainingString += BarcodeCom.ReadExisting();
                        }

                        while (RemainingString.IndexOf(CR) > -1)
                        {
                            int CRIdx = RemainingString.IndexOf(CR);
                            string Temp = RemainingString.Substring(0, CRIdx);
                            if (RemainingString.Length > CRIdx + 1)
                            {
                                RemainingString = RemainingString.Substring(CRIdx + 1, RemainingString.Length - CRIdx - 1);
                            }
                            else
                            {
                                RemainingString = "";
                            }

                            HT.ReadBarcodeing = false;
                            if (Temp == "ERROR") { }
                            else
                            {
                                GetSetEQ_Barcode = Temp;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    BarcodeCom.Close();
                    BarcodeCom.Open();
                }
            }
        }


        public void BarcodeOpen()
        {
            string lon = "\x02LON\x03";   // <STX>LON<ETX>
            BarcodeCom.Write(lon);
        }


        public void BarcodeClose()
        {
            string lon = "\x02LOFF\x03";   // <STX>LON<ETX>
            BarcodeCom.Write(lon);
        }

        public string GetSetEQ_Barcode
        {
            set
            {
                HT.EQ_Barcode = value;

                if (string.IsNullOrEmpty(HT.EQ_Barcode))
                {
                }
                else
                {
                    if (GetAdamDIValue(1, 4) == true)
                    {
                        Adam[1].SetDO(SocketCommand.MaxCnt, "", 3, "1");

                        if (ClearLoaderBarcodeByValue(value) == -1)
                        {
                            UI.Alarm(NormalStatic.EFEM, ErrorList.BC_ReadIDError_0101);
                        }

                        UserUnloader.SendCommand(String.Format("{0},{1}", "BarcodeID", value)); //Joanne 20220912 Add
                    }
                }

                if (InvokeRequired)
                {
                    Invoke(new MethodInvoker(() =>
                    {
                        tb_EQBarcode.Text = value;
                    }));
                }
                else
                {
                    tb_EQBarcode.Text = value;
                }
            }
            get { return HT.EQ_Barcode; }
        }

        private List<string> ls_LoaderBarcode = new List<string>();
        private void RefreshBarcodeList()
        {

            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() =>
                {
                    dgv_BarcodeList.Rows.Clear();
                    for (int i = 0; i < ls_LoaderBarcode.Count; i++)
                    {
                        dgv_BarcodeList.Rows.Add(1);
                        dgv_BarcodeList.Rows[i].Cells[0].Value = ls_LoaderBarcode[i];
                    }
                }));
            }
            else
            {
                dgv_BarcodeList.Rows.Clear();
                for (int i = 0; i < ls_LoaderBarcode.Count; i++)
                {
                    dgv_BarcodeList.Rows.Add(1);
                    dgv_BarcodeList.Rows[i].Cells[0].Value = ls_LoaderBarcode[i];
                }
            }

        }
        public void ClearLoaderBarcode()
        {
            ls_LoaderBarcode.Clear();
            RefreshBarcodeList();
        }

        public int ClearLoaderBarcodeByValue(string ref_UnloaderBarcode)
        {

            int Index = ls_LoaderBarcode.FindIndex(x => x == ref_UnloaderBarcode);
            if (Index > -1)
            {
                ls_LoaderBarcode.RemoveAt(Index);
            }
            RefreshBarcodeList();
            return Index;
        }

        public string TriggerBarcodeFailUI()
        {
            frm_BarcodeFail.Initial();
            frm_BarcodeFail.ShowDialog();
            return frm_BarcodeFail.BarCodeValue;
        }

        public void SignalTowerControl(EFEMStatus eFEMStatus)
        {
            switch (eFEMStatus)
            {
                case EFEMStatus.Unknown:
                    break;
                case EFEMStatus.Power_Off:
                case EFEMStatus.Alarming:
                    Adam[0].SetDO(SocketCommand.MaxCnt, "", 0, "1");
                    Adam[0].SetDO(SocketCommand.MaxCnt, "", 1, "0");
                    Adam[0].SetDO(SocketCommand.MaxCnt, "", 2, "0");
                    Adam[0].SetDO(SocketCommand.MaxCnt, "", 3, "1");
                    break;
                case EFEMStatus.Init_Now:
                case EFEMStatus.Run_Now:
                    Adam[0].SetDO(SocketCommand.MaxCnt, "", 0, "0");
                    Adam[0].SetDO(SocketCommand.MaxCnt, "", 1, "0");
                    Adam[0].SetDO(SocketCommand.MaxCnt, "", 2, "1");
                    Adam[0].SetDO(SocketCommand.MaxCnt, "", 3, "0");

                    break;
                case EFEMStatus.Init_Finish:
                case EFEMStatus.Ready_Now:
                case EFEMStatus.Run_Finish:
                    Adam[0].SetDO(SocketCommand.MaxCnt, "", 0, "0");
                    Adam[0].SetDO(SocketCommand.MaxCnt, "", 1, "1");
                    Adam[0].SetDO(SocketCommand.MaxCnt, "", 2, "0");
                    Adam[0].SetDO(SocketCommand.MaxCnt, "", 3, "0");

                    break;

                default:
                    break;
            }
        }

        public void BuzzerOn()
        {
            Adam[0].SetDO(SocketCommand.MaxCnt, "", 3, "1");
        }
        public void BuzzerOff()
        {
            Adam[0].SetDO(SocketCommand.MaxCnt, "", 3, "0");
        }
    }


    class timmmer
    {
        int times = 0;
        System.Timers.Timer timer = new System.Timers.Timer();
        public timmmer()
        {
            timer.Interval = 1; // 觸發時間
            timer.AutoReset = true; // 重複觸發
            timer.Elapsed += (s, e) => // 觸發時執行的事件
            {
                times++;
                if (times % 100 == 0)
                    Console.WriteLine("T={0}", times);
            };
            timer.Start(); // 啟動定時器
            timer.Close();
        }
        public bool start(int ms)
        {
            if (!timer.Enabled)
            {
                timer.Interval = 1; // 觸發時間
                timer.AutoReset = true; // 重複觸發
                timer.Elapsed += (s, e) => // 觸發時執行的事件
                {
                    // Console.WriteLine("tick");
                    times++;
                };
                timer.Start(); // 啟動定時器
                return true;
            }
            else
            {
                if (ms > times)
                    return true;
                else
                {
                    //  timer.Close();
                    return false;
                }
            }
            //return true;
        }

        public void reset()
        {
            timer.Close();
        }

    }

}
