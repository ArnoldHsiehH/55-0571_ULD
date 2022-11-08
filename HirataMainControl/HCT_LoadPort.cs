using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Collections;
using System.Threading;
using System.Collections.Concurrent;

namespace HirataMainControl
{
    public partial class HCT_LoadPort : UserControl
    {

        #region delegate

        public delegate void LoadPortEvent(string deviceName, SocketCommand command, bool Result, string ReplyMessage);
        public event LoadPortEvent ActionComplete;

        #endregion

        #region Class Mapp Type & Wafer Slot

        public class mapp_parameter
        {
            public int WaferThick;
            public int Pitch;
            public Int16 Offset;
            public int Pitch_Thick_TOL;
            public int Position_TOL;
            public int SlotNumber;
            public int WaferType;
        }

        private mapp_parameter Type1 = new mapp_parameter();
        private mapp_parameter Type2 = new mapp_parameter();
        private mapp_parameter Type3 = new mapp_parameter();
        private mapp_parameter Type4 = new mapp_parameter();
        private mapp_parameter Type5 = new mapp_parameter();
        private mapp_parameter[] MappParameter = new mapp_parameter[5];

        #endregion

        #region BG / Queue

        private BackgroundWorker Command_BG = new BackgroundWorker();
        private List<LPStep> LoadPort_StepArray = new List<LPStep>();
        private BlockQueue<LPStep[]> CommandQueue = new BlockQueue<LPStep[]>();
        private BlockQueue<string> ReceiverQueue = new BlockQueue<string>();

        #endregion

        #region Variable

        public string DeviceName;
        private int DeviceNo = -1;
        public ErrorList NowErrorList = ErrorList.MaxCnt;
        public string NowErrorMsg = "";
        public string DataSend_Parameter_string = "";

        private int[] SlotInfo = new int[30];
        private int[] SlotInfo_Thick = new int[30];
        private int[] SlotInfo_Position = new int[30];

        //UI
        private bool Connect;
        private LPType NowType;
        private double NowGap;
        private bool Mode;
        private LPFoupMount FoupPresent;
        private LPPosition Status;
        private string Alarm;
        private string Interlock;
        private bool Busy;

        //Reply value

        private string reply_mode = "";
        private string reply_status = "";
        private string reply_foup = "";
        private string reply_clamp = "";
        private string reply_door = "";
        private string reply_Led_status = "";
        private string reply_Z_axis = "";
        private string reply_protrusion = "";

        //  For RS-232 Region
        private SerialPort LoadPortCOM;
        private int ReceiveNowStepCnt = NormalStatic.WaitReply;
        private List<byte> ReceiveTemp = new List<byte>();
        private int NowStepCnt = 0;
        private SocketCommand MainCmd = SocketCommand.MaxCnt;
        private LPStep[] MarcoCommand;

        // Command
        private string CMD_string = "";
        private string REC_string = "";
        private int TimeoutCount = 15000;

        #endregion    

        #region Get/Set

        #region Ui Connect

        public bool Ui_Connect
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_Connect = value; }));
                    return;
                }
                labConnect.BackColor = (value ? Color.LightGreen : Color.Red);
                labConnect.Text = (value ? "Con-C" : "Dis-C");
                Connect = value;
            }
            get { return Connect; }
        }

        #endregion

        #region Ui Type

        public LPType Ui_Type
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_Type = value; }));
                    return;
                }
                NowType = value;
                labType.Text = value.ToString();
            }
            get { return NowType; }
        }

        #endregion

        #region Ui Mode

        public bool Ui_Mode
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_Mode = value; }));
                    return;
                }
                labAuto.BackColor = (value ? Color.LightGreen : Color.Red);
                labAuto.Text = (value ? "Auto" : "Manual");
                Mode = value;
            }
            get { return Mode; }
        }

        #endregion

        #region Ui Foup Present

        public LPFoupMount Ui_FoupPresent
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_FoupPresent = value; }));
                    return;
                }

                switch (value)
                {
                    case LPFoupMount.Absent:
                        {
                            labPresent.BackColor = Color.LightGreen;
                            labPresent.Text = "None Foup";
                        }
                        break;
                    case LPFoupMount.Present:
                        {
                            labPresent.BackColor = Color.Yellow;
                            labPresent.Text = "Present";
                        }
                        break;
                    case LPFoupMount.Unknown:
                        {
                            labPresent.BackColor = Color.Red;
                            labPresent.Text = "Abnormal";
                        }
                        break;
                }
                FoupPresent = value;
            }
            get { return FoupPresent; }
        }

        #endregion

        #region Ui Load Statue

        public LPPosition Ui_LoadStatus
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_LoadStatus = value; }));
                    return;
                }

                switch (value)
                {
                    case LPPosition.Load:
                        {
                            labStatus.Text = "Load";
                            labStatus.BackColor = Color.Yellow;
                        }
                        break;

                    case LPPosition.Unload:
                        {
                            labStatus.Text = "Unload";
                            labStatus.BackColor = Color.LightGreen;
                        }
                        break;

                    case LPPosition.InProcess:
                        {
                            labStatus.Text = "In Process";
                            labStatus.BackColor = Color.Red;
                        }
                        break;
                }

                Status = value;
            }
            get { return Status; }
        }

        #endregion

        #region Ui Alarm
        public string Ui_Alarm
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_Alarm = value; }));
                    return;
                }

                if (value == Socket_Static.ReplyNormal_00)
                {
                    labAlarm.Visible = false;
                }
                else
                {
                    labAlarm.Text = string.Format("Error:{0}", value);
                    labAlarm.Visible = true;
                }

                Alarm = value;
            }
            get { return Alarm; }
        }
        #endregion

        #region Ui Interlock
        public string Ui_Interlock
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_Interlock = value; }));
                    return;
                }

                if (value == Socket_Static.ReplyNormal_00)
                {
                    labInterlock.Visible = false;
                }
                else
                {
                    labInterlock.Text = string.Format("Interlock:{0}", value);
                    labInterlock.Visible = true;
                }
                Interlock = value;
            }
            get { return Interlock; }
        }
        #endregion

        #region Ui Busy

        public bool Ui_Busy
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_Busy = value; }));
                    return;
                }
                UserUnloader.SendCommand(string.Format("LPBusy,{0},{1}",DeviceNo, value ? "1" : "0"));
                labBusy.Text = value ? "Busy" : "Idle";
                labBusy.BackColor = value ? Color.Yellow : Color.LightGreen;
                Busy = value;
            }
            get { return Busy; }
        }

        #endregion

        #region Ui NowGap

        public double Ui_NowGap
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_NowGap = value; }));
                    return;
                }
                labpitch.Text = value.ToString();
                NowGap = value;
            }
            get { return NowGap; }
        }

        #endregion

        #endregion

        #region Initial

        public HCT_LoadPort()
        {
            InitializeComponent();
            MappParameter = new mapp_parameter[] { Type1, Type2, Type3, Type4, Type5 };
            if (HCT_EFEM.E84Count == 0)
            {
                tctlLoadport.TabPages.Remove(tapgE84);
            }
        }

        public void Initial(int number)
        {
            DeviceName = tapgLoadPort.Text = string.Format("{0}{1}", NormalStatic.LP, number + 1);
            DeviceNo = number;

            #region status initial

            Ui_FoupPresent = LPFoupMount.Absent;
            Ui_LoadStatus = LPPosition.InProcess;
            Ui_Busy = false;
            Ui_Alarm = Socket_Static.ReplyNormal_00;
            Ui_Interlock = Socket_Static.ReplyNormal_00;

            #endregion

            #region  RS232 Setting

            int IniCOM = Convert.ToInt32(AppSetting.LoadSetting(string.Format("{0}_{1}", NormalStatic.LP, "COM"), "15"));
            LoadPortCOM = new SerialPort(string.Format("COM{0}", IniCOM + number));
            LoadPortCOM.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.COM_DataReceived);
            LoadPortCOM.BaudRate = 9600;
            LoadPortCOM.DataBits = 8;
            LoadPortCOM.Parity = Parity.None;
            LoadPortCOM.StopBits = StopBits.One;
            COM_Connect();

            #endregion

            #region Background

            Command_BG.DoWork += new DoWorkEventHandler(this.DoWork_Command_BG);
            Command_BG.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.completed_Command_BG);
            Command_BG.RunWorkerAsync();

            #endregion 

            #region List View

            lvWaferStatus.View = View.Details;
            lvWaferStatus.FullRowSelect = true;
            lvWaferStatus.GridLines = true;
            lvWaferStatus.ShowItemToolTips = true;
            lvWaferStatus.Columns.Add("Slot", -2, HorizontalAlignment.Left);
            lvWaferStatus.Columns.Add("Wafer Info", -2, HorizontalAlignment.Left);

            //for (int i = 0; i < 25; i++)
            //{
            //    lvWaferStatus.Items.Insert(0, string.Format("{0}", i + 1));
            //    lvWaferStatus.Items[0].SubItems.Add("Unknown");
            //    lvWaferStatus.Items[0].BackColor = Color.Gray;
            //}

            #endregion      
        }

        #endregion

        #region Slot

        public int[] GetSlotData
        {
            get { return SlotInfo; }
        }

        public int GetSlotData1(int ref_Slot)
        {
            return SlotInfo[ref_Slot];
        }

        public void SetSlotData(int slot_index, int status)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate () { SetSlotData(slot_index, status); }));
                return;
            }

            SlotInfo[slot_index - 1] = status;
            SlotInfo_Thick[slot_index - 1] = 0;
            SlotInfo_Position[slot_index - 1] = 0;
            UpdateSlotUi(slot_index);
        }

        #endregion

        #region BG

        private void DoWork_Command_BG(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                MarcoCommand = CommandQueue.DeQueue();
                NowStepCnt = 0;
                ReceiverQueue.Clear();
                //CommandQueue.Clear();
                //ReceiveNowStepCnt = NormalStatic.Idle;

                if (MarcoCommand[NowStepCnt] == LPStep.End)
                    break;

                while (NowStepCnt < MarcoCommand.Length)
                {
                    #region Commnad Handle

                    #region End Command

                    if (MarcoCommand[NowStepCnt] == LPStep.End)
                    {
                        break;
                    }
                    // Command Complete
                    if (MarcoCommand[NowStepCnt] == LPStep.MaxCnt)
                    {
                        Ui_Busy = false;
                        JobSuccess();
                        break;
                    }
                    // Event End
                    if (MarcoCommand[NowStepCnt] == LPStep.EventEnd)
                    {
                        NowStepCnt = 0;
                        break;
                    }

                    #endregion

                    #region MicroCommnad Handle

                    CMD_string = "";
                    REC_string = "";
                    ReceiverQueue.Clear();

                    switch (MarcoCommand[NowStepCnt])
                    {

                        #region GetStatus
                        case LPStep.GetStatus: // Get LoadPort Status 
                            {
                                CMD_string = "GET:STAS;";
                            }
                            break;
                        #endregion

                        #region ResetSlotData
                        case LPStep.ResetSlotData:
                            {
                                ResetSlotData();
                                CMD_string = "GET:STAS;";
                            }
                            break;
                        #endregion

                        #region GetWaferStatus
                        case LPStep.GetWaferStatus: // Get Wafer Status (when Mapping is done)
                            {
                                CMD_string = "GET:MAPR;";
                            }
                            break;
                        #endregion

                        #region Get LED Status
                        case LPStep.GetLEDStatus: // Get LED Status
                            {
                                CMD_string = "GET:LEST;";
                            }
                            break;
                        #endregion

                        #region Get Z Axis
                        case LPStep.GetZAxisPos: // Get LED Status
                            {
                                CMD_string = "GET:POSD01;";
                            }
                            break;
                        #endregion

                        #region ResetError
                        case LPStep.ResetError: // Reset Error and Clear Busy
                            {
                                CMD_string = "SET:RSET;";
                            }
                            break;
                        #endregion

                        #region SetMappParameter
                        case LPStep.SetMappParameter: // Setting Mapp Parameter
                            {
                                string parameter_str = SetMapp_Parameter();
                                CMD_string = string.Format("SET:MAPP{0};", parameter_str);
                            }
                            break;
                        #endregion

                        #region Set Type
                        case LPStep.SetType1:
                            CMD_string = "SET:TYP1;";
                            break;
                        case LPStep.SetType2:
                            CMD_string = "SET:TYP2;";
                            break;
                        case LPStep.SetType3:
                            CMD_string = "SET:TYP3;";
                            break;
                        case LPStep.SetType4:
                            CMD_string = "SET:TYP4;";
                            break;
                        case LPStep.SetType5:
                            CMD_string = "SET:TYP5;";
                            break;
                        #endregion

                        #region Get Type Data
                        case LPStep.GetType1:
                            CMD_string = "GET:MAPP00;";
                            break;
                        case LPStep.GetType2:
                            CMD_string = "GET:MAPP01;";
                            break;
                        case LPStep.GetType3:
                            CMD_string = "GET:MAPP02;";
                            break;
                        case LPStep.GetType4:
                            CMD_string = "GET:MAPP03;";
                            break;
                        case LPStep.GetType5:
                            CMD_string = "GET:MAPP04;";
                            break;
                        #endregion

                        #region Set LED
                        case LPStep.LoadLED_On:
                            CMD_string = "SET:LPLD;";
                            break;
                        case LPStep.LoadLED_Off:
                            CMD_string = "SET:LOLD;";
                            break;
                        case LPStep.LoadLED_Blink:
                            CMD_string = "SET:BLLD;";
                            break;
                        case LPStep.UnloadLED_On:
                            CMD_string = "SET:LPUD;";
                            break;
                        case LPStep.UnloadLED_Off:
                            CMD_string = "SET:LOUD;";
                            break;
                        case LPStep.UnloadLED_Blink:
                            CMD_string = "SET:BLUD;";
                            break;
                        case LPStep.SwitchLED_On:
                            CMD_string = "SET:LPSW;";
                            break;
                        case LPStep.SwitchLED_Blink:
                            CMD_string = "SET:BLSW;";
                            break;
                        case LPStep.SwitchLED_Off:
                            CMD_string = "SET:LOSW;";
                            break;
                        case LPStep.Status1LED_On:
                            CMD_string = "SET:LPS1;";
                            break;
                        case LPStep.Status1LED_Blink:
                            CMD_string = "SET:BLS1;";
                            break;
                        case LPStep.Status1LED_Off:
                            CMD_string = "SET:LOS1;";
                            break;
                        case LPStep.Status2LED_On:
                            CMD_string = "SET:LPS2;";
                            break;
                        case LPStep.Status2LED_Blink:
                            CMD_string = "SET:BLS2;";
                            break;
                        case LPStep.Status2LED_Off:
                            CMD_string = "SET:LOS2;";
                            break;
                        #endregion

                        #region Get Wafer Thick

                        case LPStep.GetWaferThick01:
                            {
                                CMD_string = "GET:MDAH01;";
                            }
                            break;

                        case LPStep.GetWaferThick02:
                            {
                                CMD_string = "GET:MDAH02;";
                            }
                            break;

                        case LPStep.GetWaferThick03:
                            {
                                CMD_string = "GET:MDAH03;";
                            }
                            break;

                        case LPStep.GetWaferThick04:
                            {
                                CMD_string = "GET:MDAH04;";
                            }
                            break;

                        case LPStep.GetWaferThick05:
                            {
                                CMD_string = "GET:MDAH05;";
                            }
                            break;

                        case LPStep.GetWaferThick06:
                            {
                                CMD_string = "GET:MDAH06;";
                            }
                            break;

                        #endregion

                        #region Get Wafer Position

                        case LPStep.GetWaferPosition01:
                            {
                                CMD_string = "GET:MDAP01;";
                            }
                            break;

                        case LPStep.GetWaferPosition02:
                            {
                                CMD_string = "GET:MDAP02;";
                            }
                            break;

                        case LPStep.GetWaferPosition03:
                            {
                                CMD_string = "GET:MDAP03;";
                            }
                            break;

                        case LPStep.GetWaferPosition04:
                            {
                                CMD_string = "GET:MDAP04;";
                            }
                            break;

                        case LPStep.GetWaferPosition05:
                            {
                                CMD_string = "GET:MDAP05;";
                            }
                            break;
                        case LPStep.GetWaferPosition06:
                            {
                                CMD_string = "GET:MDAP06;";
                            }
                            break;

                        #endregion

                        #region MOV

                        case LPStep.Home:
                            {
                                CMD_string = "MOV:ORGN;";
                            }
                            break;

                        case LPStep.Load:
                            {
                                CMD_string = "MOV:FPML;";
                            }
                            break;

                        case LPStep.Unload:
                            {
                                CMD_string = "MOV:FPUL;";
                            }
                            break;

                        case LPStep.Clmap:
                            {
                                CMD_string = "MOV:FCCL;";
                            }
                            break;
                        case LPStep.Unclmap:
                            {
                                CMD_string = "MOV:FCOP;";
                            }
                            break;

                        case LPStep.DoMapping:
                            {
                                CMD_string = "MOV:MAPP;";
                            }
                            break;

                            #endregion

                    }

                    // Command Sending
                    if (CMD_string != "")
                    {
                        DataSend(CMD_string);
                        REC_string = ReceiverQueue.DeQueue(TimeoutCount);
                        if (REC_string == null)
                        {
                            if (NowErrorList == ErrorList.MaxCnt)
                                NowErrorList = ErrorList.LP_ConnectTimeout_0402;
                            JobFail();
                        }
                        else if (REC_string == NormalStatic.End)
                        {
                            NowStepCnt += 100;
                        }
                        else
                        {
                            ReceiveHandler();
                        }
                    }
                    #endregion

                    #endregion
                }
            }
        }

        private void completed_Command_BG(object sender, RunWorkerCompletedEventArgs e)
        {
            UI.CloseBG(DeviceName);
        }

        public void Close()
        {
            NowStepCnt = 0;
            CommandQueue.Clear();
            CommandQueue.EnQueue(new LPStep[] { LPStep.End });
            ReceiverQueue.EnQueue(NormalStatic.End);
            COM_Disconnect();
        }

        private void JobSuccess()
        {
            string Reply_string = "";

            switch (MainCmd)
            {
                case SocketCommand.Initial:
                case SocketCommand.SetType:
                case SocketCommand.SetMapp:
                case SocketCommand.Home:
                case SocketCommand.ResetError:
                case SocketCommand.Load:
                case SocketCommand.Unload:
                case SocketCommand.Clamp:
                case SocketCommand.UnClamp:
                case SocketCommand.Map:
                    {
                    }
                    break;

                case SocketCommand.LEDLoad:
                case SocketCommand.LEDUnLoad:
                case SocketCommand.LEDStatus1:
                case SocketCommand.LEDStatus2:
                case SocketCommand.SetOperatorAccessButton:
                    {
                        Reply_string = DataSend_Parameter_string;
                    }
                    break;

                case SocketCommand.GetStatus:
                    {
                        #region GetStatus

                        switch (Ui_Alarm)
                        {
                            case "00": reply_status = "No Error"; break;
                            default: reply_status = Ui_Alarm; break;
                        }

                        Reply_string = string.Format("{0},{1},{2},{3},{4},{5}",
                            reply_mode, reply_status, Ui_FoupPresent, reply_clamp, reply_door, (int)(LPType)Ui_Type);

                        #endregion
                    }
                    break;

                case SocketCommand.GetWaferSlot:
                case SocketCommand.GetWaferSlot2:
                    {
                        #region GetWaferSlot

                        for (int index = 0; index < SlotInfo.Length; index++)
                        {
                            if (MainCmd == SocketCommand.GetWaferSlot)
                            {
                                Reply_string = string.Format("{0}{1}", GetSlotData[index], Reply_string);
                                if (index < MappParameter[(int)(LPType)Ui_Type].SlotNumber - 1)
                                    Reply_string = string.Format("{0}{1}", NormalStatic.Comma, Reply_string);
                            }
                            else
                            {
                                Reply_string = string.Format("{0}{1}", Reply_string, GetSlotData[index]);
                                if (index < MappParameter[(int)(LPType)Ui_Type].SlotNumber - 1)
                                    Reply_string = string.Format("{0}{1}", Reply_string, NormalStatic.Comma);
                            }
                        }

                        #endregion
                    }
                    break;

                case SocketCommand.GetWaferThickness:
                    {
                        #region GetWaferThickness

                        for (int index = 0; index < SlotInfo.Length; index++)
                        {
                            Reply_string = string.Format("{0}{1}", SlotInfo_Thick[index], Reply_string);
                            if (index < SlotInfo.Length - 1)
                                Reply_string = string.Format("{0}{1}", NormalStatic.Comma, Reply_string);
                        }

                        #endregion
                    }
                    break;

                case SocketCommand.GetWaferPosition:
                    {
                        #region GetWaferPosition

                        for (int index = 0; index < SlotInfo.Length; index++)
                        {
                            Reply_string = string.Format("{0}{1}", SlotInfo_Position[index], Reply_string);
                            if (index < SlotInfo.Length - 1)
                                Reply_string = string.Format("{0}{1}", NormalStatic.Comma, Reply_string);
                        }

                        #endregion
                    }
                    break;

                case SocketCommand.GetLEDStatus:
                    {
                        Reply_string = reply_Led_status;
                    }
                    break;

                case SocketCommand.GetZAxisPos:
                    {
                        Reply_string = reply_Z_axis;
                    }
                    break;

                case SocketCommand.GetMapp:
                    {
                        #region GetMapp

                        Reply_string = string.Format("{0},{1},{2},{3},{4},{5},{6}",
                            MappParameter[Int32.Parse(DataSend_Parameter_string)].WaferThick,
                            MappParameter[Int32.Parse(DataSend_Parameter_string)].Pitch,
                            MappParameter[Int32.Parse(DataSend_Parameter_string)].SlotNumber,
                            MappParameter[Int32.Parse(DataSend_Parameter_string)].Offset,
                            MappParameter[Int32.Parse(DataSend_Parameter_string)].Pitch_Thick_TOL,
                            MappParameter[Int32.Parse(DataSend_Parameter_string)].Position_TOL,
                            MappParameter[Int32.Parse(DataSend_Parameter_string)].WaferType);

                        #endregion
                    }
                    break;

                case SocketCommand.GetProtrusionSensor:
                    {
                        Reply_string = reply_protrusion;
                    }
                    break;

            }
            DataSend_Parameter_string = "";
            ActionComplete(DeviceName, MainCmd, true, Reply_string);
        }

        private void JobFail()
        {
            NowStepCnt += 100;

            if (MarcoCommand[MarcoCommand.Length - 1] != LPStep.EventEnd)
            {
                ActionComplete(DeviceName, MainCmd, false, "");
            }
        }

        private void ReceiveHandler()
        {
            string[] CMD = CMD_string.Split(new string[] { ":", ";", "0" }, StringSplitOptions.None);
            string[] REC = REC_string.Split(new string[] { "," }, StringSplitOptions.None);

            if (REC[0].Length != 2)
            {
                #region Normal Respond
                switch (CMD[0])
                {
                    case Socket_Static.ReplyGET:
                        {
                            #region GET
                            if (REC[0] == CMD[1])
                            {
                                switch (REC[0])
                                {
                                    #region GET STAS

                                    case "STAS":
                                        {
                                            string port = "unknow";
                                            switch (DeviceName)
                                            {
                                                case "LP1":
                                                    port = "port1";
                                                    break;
                                                case "LP2":
                                                    port = "port2";
                                                    break;
                                            }

                                            UserUnloader.SendCommand(string.Format("{0},{1},{2}", "LPstatus", port, REC_string));
                                            ULD_protocol.LPstatus(port, REC_string);//LPstatus 回報給Loader

                                            Ui_Mode = (REC[1][1] == '0') ? true : false;
                                            Ui_Type = (LPType)Enum.Parse(typeof(LPType), string.Format("{0}", REC[1][18]));
                                            Ui_NowGap = (double)MappParameter[(int)Ui_Type].Pitch / 1000.0;
                                            Ui_FoupPresent = (LPFoupMount)Enum.Parse(typeof(LPFoupMount), string.Format("{0}", REC[1][6]));

                                            UserUnloader.SendCommand(string.Format("{0},{1},{2},{3}", "LPParameter", DeviceNo, Ui_Type,Ui_NowGap));


                                            #region Ui_Load
                                            switch (REC[1][2])
                                            {
                                                case '0':
                                                    {
                                                        if (REC[1][13] == '0')
                                                            Ui_LoadStatus = LPPosition.Unload;
                                                        else
                                                            Ui_LoadStatus = LPPosition.InProcess;
                                                        reply_door = string.Format("{0}", LPDoor.Close);
                                                    }
                                                    break;
                                                case '1':
                                                    {
                                                        Ui_LoadStatus = LPPosition.Unload;
                                                        reply_door = string.Format("{0}", LPDoor.Close);
                                                    }
                                                    break;
                                                case '2':
                                                    {
                                                        Ui_LoadStatus = LPPosition.Load;
                                                        reply_door = string.Format("{0}", LPDoor.Open);
                                                    }
                                                    break;
                                            }
                                            #endregion

                                            #region Ui_Alarm

                                            if (string.Format("{0}{1}", REC[1][4], REC[1][5]) != Socket_Static.ReplyNormal_00)
                                            {
                                                Ui_Alarm = string.Format("{0}{1}", REC[1][4], REC[1][5]);
                                                if (NowErrorList == ErrorList.MaxCnt)
                                                    NowErrorList = (ErrorList)HCT_EFEM.EFEM_HasTable.LP_Error[Ui_Alarm];
                                            }
                                            else
                                            {
                                                Ui_Alarm = Socket_Static.ReplyNormal_00;
                                            }
                                            #endregion

                                            #region API reply

                                            switch (REC[1][1])
                                            {
                                                case '0': reply_mode = "Online"; break;
                                                case '1': reply_mode = "Teaching"; break;
                                                case '2': reply_mode = "Maintain"; break;
                                            }
                                            switch (REC[1][7])
                                            {
                                                case '0': reply_clamp = string.Format("{0}", LPClamp.Unclamp); break;
                                                case '1': reply_clamp = string.Format("{0}", LPClamp.Clamp); break;
                                                case '?': reply_clamp = string.Format("{0}", LPClamp.Unknown); break;
                                            }
                                            reply_protrusion = (REC[1][11] == '0') ? "1" : "0";

                                            #endregion
                                        }
                                        break;

                                    #endregion

                                    #region GET MAPR

                                    case "MAPR":
                                        {

                                            ULD_protocol.MappingData(DeviceName, REC[1]);
                                            for (int i = 0; i < REC[1].Count(); i++)
                                            {
                                                SetSlotData(i + 1, Int32.Parse(REC[1].Substring(i, 1)));
                                            }
                                        }
                                        break;

                                    #endregion

                                    #region GET MAPP

                                    case "MAPP":
                                        {
                                            LPType index = LPType.Type1;
                                            switch (CMD[2])
                                            {
                                                case "": index = LPType.Type1; break;
                                                case "1": index = LPType.Type2; break;
                                                case "2": index = LPType.Type3; break;
                                                case "3": index = LPType.Type4; break;
                                                case "4": index = LPType.Type5; break;
                                            }

                                            MappParameter[(int)index].WaferThick = int.Parse(REC[1].Substring(0, 4), System.Globalization.NumberStyles.HexNumber);
                                            MappParameter[(int)index].Pitch = int.Parse(REC[1].Substring(4, 4), System.Globalization.NumberStyles.HexNumber);
                                            MappParameter[(int)index].SlotNumber = int.Parse(REC[1].Substring(8, 4), System.Globalization.NumberStyles.HexNumber);
                                            MappParameter[(Int16)index].Offset = Int16.Parse(REC[1].Substring(12, 4), System.Globalization.NumberStyles.HexNumber);
                                            MappParameter[(int)index].Pitch_Thick_TOL = int.Parse(REC[1].Substring(16, 4), System.Globalization.NumberStyles.HexNumber);
                                            MappParameter[(int)index].Position_TOL = int.Parse(REC[1].Substring(20, 4), System.Globalization.NumberStyles.HexNumber);
                                            MappParameter[(int)index].WaferType = int.Parse(REC[1].Substring(24, 2), System.Globalization.NumberStyles.HexNumber);

                                        }
                                        break;

                                    #endregion

                                    #region GET MDAH

                                    case "MDAH":
                                        {
                                            int thick_index = (Int32.Parse(CMD[2]) - 1) * 5 + 1;
                                            for (int i = 0; i < 5; i++)
                                            {
                                                if ((thick_index + i) <= SlotInfo.Length)
                                                {
                                                    SetSlotData_Thick(thick_index + i, int.Parse(REC[1].Substring(4 * i, 4), System.Globalization.NumberStyles.HexNumber));
                                                }
                                            }
                                        }
                                        break;

                                    #endregion

                                    #region GET MDAP

                                    case "MDAP":
                                        {
                                            int position_index = (Int32.Parse(CMD[2]) - 1) * 5 + 1;
                                            for (int i = 0; i < 5; i++)
                                            {
                                                if ((position_index + i) <= SlotInfo.Length)
                                                {
                                                    SetSlotData_Position(position_index + i, int.Parse(REC[1].Substring(6 * i, 6), System.Globalization.NumberStyles.HexNumber));
                                                }
                                            }
                                        }
                                        break;

                                    #endregion

                                    #region GET LEST

                                    case "LEST":
                                        {
                                            reply_Led_status = REC[1];
                                        }
                                        break;

                                    #endregion

                                    #region GET POSD

                                    case "POSD":
                                        {
                                            reply_Z_axis = string.Format("{0}", int.Parse(REC[1], System.Globalization.NumberStyles.HexNumber));
                                        }
                                        break;

                                        #endregion
                                }
                                NowStepCnt++;
                            }
                            #endregion
                        }
                        break;

                    case Socket_Static.ReplyMOV:
                    case Socket_Static.ReplySET:
                        {
                            #region MOV & SET

                            if (REC[0] == Socket_Static.ReplyINF)
                                NowStepCnt++;

                            #endregion
                        }
                        break;
                }

                #endregion
            }
            else
            {
                #region Error Code Update

                switch (REC[0])
                {
                    case Socket_Static.ReplyDeviceError_01:
                    case Socket_Static.ReplyFormatError_02:
                        {
                            NowErrorList = ErrorList.LP_ChecksumError_0403;
                        }
                        break;

                    case Socket_Static.ReplyInterlock_04:
                        {
                            NowErrorList = ErrorList.LP_04Interlock_0445;
                        }
                        break;
                    case Socket_Static.ReplyInAlarm_05:
                        {
                            NowErrorList = ErrorList.LP_05InAalarm_0444;
                        }
                        break;
                    case Socket_Static.ReplyInProcess_06:
                        {
                            NowErrorList = ErrorList.LP_06InProcess_0441;
                        }
                        break;

                    case Socket_Static.ReplyModeError_07:
                        {
                            Ui_Mode = false;
                            NowErrorList = ErrorList.LP_07ModeError_0442;
                        }
                        break;

                    case Socket_Static.ReplyMappingError_08:
                        {
                            NowErrorList = ErrorList.LP_08MappingError_0443;
                        }
                        break;
                }

                #endregion                    

                JobFail();
            }
        }

        private void ResetSlotData()
        {
            if (labAlarm.InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate () { ResetSlotData(); }));
            }
            else
            {
                
                SlotInfo = new int[MappParameter[(int)Ui_Type].SlotNumber];
                HT.LP_CurrentSlot[DeviceNo] = MappParameter[(int)Ui_Type].SlotNumber;
                UserUnloader.SendCommand(string.Format("LPSlotReset,{0},{1}",DeviceNo, MappParameter[(int)Ui_Type].SlotNumber));
                SlotInfo_Thick = new int[SlotInfo.Length];
                SlotInfo_Position = new int[SlotInfo.Length];

                for (int index = 0; index < SlotInfo.Length; index++)
                {
                    SlotInfo[index] = (int)WaferStatus.Unknown;
                    SlotInfo_Thick[index] = 0;
                    SlotInfo_Position[index] = 0;
                }
                lvWaferStatus.Items.Clear();
                if (Ui_FoupPresent == LPFoupMount.Present)
                {
                    for (int index = 0; index < SlotInfo.Length; index++)
                    {
                        lvWaferStatus.Items.Insert(0, string.Format("{0}", index + 1));
                        lvWaferStatus.Items[0].SubItems.Add("Unknown");
                        lvWaferStatus.Items[0].BackColor = Color.Gray;
                    }
                }
            }
        }

        private void UpdateSlotUi(int slot_index)
        {
            if (lvWaferStatus.InvokeRequired)
                Invoke(new MethodInvoker(delegate () { UpdateSlotUi(slot_index); }));
            else
            {
                switch (SlotInfo[slot_index - 1])
                {
                    case (int)WaferStatus.WithOut:
                        {
                            lvWaferStatus.Items[lvWaferStatus.Items.Count - slot_index].ToolTipText = string.Format("slot {0}: {1}", slot_index, "No Wafer Exist.");
                            lvWaferStatus.Items[lvWaferStatus.Items.Count - slot_index].SubItems[1].Text = "No Wafer";
                            lvWaferStatus.Items[lvWaferStatus.Items.Count - slot_index].BackColor = System.Drawing.Color.WhiteSmoke;
                        }
                        break;

                    case (int)WaferStatus.With:
                        {
                            lvWaferStatus.Items[lvWaferStatus.Items.Count - slot_index].ToolTipText = string.Format("slot {0}: {1}", slot_index, "Wafer Exist.");
                            lvWaferStatus.Items[lvWaferStatus.Items.Count - slot_index].SubItems[1].Text = "Have Wafer";
                            lvWaferStatus.Items[lvWaferStatus.Items.Count - slot_index].BackColor = System.Drawing.Color.LightGreen;
                        }
                        break;

                    case (int)WaferStatus.Cross:
                        {
                            lvWaferStatus.Items[lvWaferStatus.Items.Count - slot_index].ToolTipText = string.Format("slot {0}: {1}", slot_index, "Wafer Cross.");
                            lvWaferStatus.Items[lvWaferStatus.Items.Count - slot_index].SubItems[1].Text = "Cross";
                            lvWaferStatus.Items[lvWaferStatus.Items.Count - slot_index].BackColor = System.Drawing.Color.Red;
                        }
                        break;

                    case (int)WaferStatus.Thickness:
                        {
                            lvWaferStatus.Items[lvWaferStatus.Items.Count - slot_index].ToolTipText = string.Format("slot {0}: {1}", slot_index, "Wafer Thickness.");
                            lvWaferStatus.Items[lvWaferStatus.Items.Count - slot_index].SubItems[1].Text = "Thickness";
                            lvWaferStatus.Items[lvWaferStatus.Items.Count - slot_index].BackColor = System.Drawing.Color.Red;
                        }
                        break;

                    case (int)WaferStatus.Thiness:
                        {
                            lvWaferStatus.Items[lvWaferStatus.Items.Count - slot_index].ToolTipText = string.Format("slot {0}: {1}", slot_index, "Wafer Thiness.");
                            lvWaferStatus.Items[lvWaferStatus.Items.Count - slot_index].SubItems[1].Text = "Thiness";
                            lvWaferStatus.Items[lvWaferStatus.Items.Count - slot_index].BackColor = System.Drawing.Color.Red;
                        }
                        break;

                    case (int)WaferStatus.Position:
                        {
                            lvWaferStatus.Items[lvWaferStatus.Items.Count - slot_index].ToolTipText = string.Format("slot {0}: {1}", slot_index, "Wafer Position Error.");
                            lvWaferStatus.Items[lvWaferStatus.Items.Count - slot_index].SubItems[1].Text = "Position";
                            lvWaferStatus.Items[lvWaferStatus.Items.Count - slot_index].BackColor = System.Drawing.Color.Red;
                        }
                        break;
                }
            }
        }

        private void SetSlotData_Thick(int slot_index, int thick)
        {
            if (lvWaferStatus.InvokeRequired)
                Invoke(new MethodInvoker(delegate () { SetSlotData_Thick(slot_index, thick); }));
            else
            {
                SlotInfo_Thick[slot_index - 1] = thick;
                if (SlotInfo[slot_index - 1] != (int)WaferStatus.WithOut)
                {
                    lvWaferStatus.Items[lvWaferStatus.Items.Count - slot_index].ToolTipText += string.Format("\nThinkness:{0} (um)", thick);
                }
            }
        }

        private void SetSlotData_Position(int slot_index, int position)
        {
            if (lvWaferStatus.InvokeRequired)
                Invoke(new MethodInvoker(delegate () { SetSlotData_Position(slot_index, position); }));
            else
            {
                SlotInfo_Position[slot_index - 1] = position;
                if (SlotInfo[slot_index - 1] != (int)WaferStatus.WithOut)
                {
                    lvWaferStatus.Items[lvWaferStatus.Items.Count - slot_index].ToolTipText += string.Format("\nPosition:{0} (um)", position);
                }
            }
        }

        private string SetMapp_Parameter()
        {
            string[] Parameter = DataSend_Parameter_string.Split(new string[] { "," }, StringSplitOptions.None);

            int para_type = Int32.Parse(Parameter[0]);
            int para_think = Int32.Parse(Parameter[1]);
            int para_pitch = Int32.Parse(Parameter[2]);
            int para_slot = Int32.Parse(Parameter[3]);
            Int16 para_offset = Int16.Parse(Parameter[4]);
            int para_pitch_TOL = Int32.Parse(Parameter[5]);
            int para_position_TOL = Int32.Parse(Parameter[6]);
            int para_sensor = Int32.Parse(Parameter[7]);

            return string.Format("{0:X2}{1:X4}{2:X4}{3:X4}{4:X4}{5:X4}{6:X4}{7:X2}",
                para_type, para_think, para_pitch, para_slot, para_offset, para_pitch_TOL, para_position_TOL, para_sensor);

        }

        #endregion

        #region RS232

        public void COM_Connect()
        {
            Ui_Connect = false;
            LoadPortCOM.Close();
            try
            {
                LoadPortCOM.Open();
                Ui_Connect = true;
            }
            catch
            {
                Ui_Connect = false;
                UI.InitialSystem(DeviceName, NormalStatic.False, ErrorList.LP_PortOpenFail_0400);
            }
        }

        public void COM_Disconnect()
        {
            Ui_Connect = false;
            LoadPortCOM.Close();
        }

        private void COM_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] raw_byte = new byte[1];
            SerialPort sp = (SerialPort)sender;
            while (sp.BytesToRead > 0)
            {
                sp.Read(raw_byte, 0, 1);
                //Array.Resize(ref raw_byte, length);
                DataCombine(raw_byte);
            }
        }

        private void DataSend(string Command_String)
        {
            string Send_String = string.Format("0000{0}", Command_String);
            string CheckSum_String = CheckSum(Send_String);
            Send_String = string.Format("{0}{1}", Send_String, CheckSum_String);
            List<byte> Command_Byte = new List<byte>(Encoding.ASCII.GetBytes(Send_String));
            Command_Byte.Insert(0, NormalStatic.StartByte_SOH);
            Command_Byte.Add(NormalStatic.EndByte_CR);
            LoadPortCOM.Write(Command_Byte.ToArray(), 0, Command_Byte.Count);

            UI.Log(NormalStatic.LP, DeviceName, SystemList.DeviceSend, Command_String);
        }

        private string CheckSum(string Send)
        {
            int sum = 0;
            foreach (char Character in Send)
                sum += Convert.ToByte(Character);

            sum = sum & 0xff;
            string hexstring = string.Format("{0:X2}", sum);
            return hexstring;
        }

        private void DataCombine(byte[] raw_data)
        {
            // for (int i = 0; i < raw_data.Length; i++)
            // {
            switch (ReceiveNowStepCnt)
            {

                case NormalStatic.WaitReply:
                    {
                        if (raw_data[0] != NormalStatic.StartByte_SOH)
                        {
                            NowErrorMsg += string.Format(" 0x{0}", Convert.ToString(raw_data[0], 16).ToUpper());
                            NowErrorList = ErrorList.LP_SocketError_0401;
                            break;
                        }

                        ReceiveTemp.Clear();
                        ReceiveNowStepCnt = NormalStatic.Receiving;
                    }
                    break;

                case NormalStatic.Receiving:
                    {

                        if (raw_data[0] != NormalStatic.EndByte_CR)
                            ReceiveTemp.Add(raw_data[0]);
                        else //End of the Receiving and check Sum
                        {
                            string CompleteString = Encoding.ASCII.GetString(ReceiveTemp.GetRange(0, ReceiveTemp.Count - 2).ToArray());
                            string recieve_check_sum = Encoding.ASCII.GetString(ReceiveTemp.ToArray(), ReceiveTemp.Count - 2, 2);
                            string check_sum = CheckSum(CompleteString);
                            ReceiveNowStepCnt = NormalStatic.WaitReply;
                            if (check_sum == recieve_check_sum)
                            {

                                #region Response / Event Define

                                UI.Log(NormalStatic.LP, DeviceName, SystemList.DeviceReceive, CompleteString);

                                string[] EventDefine = CompleteString.Split(new string[] { "0000", "00", ":", ";", "/" }, StringSplitOptions.None);
                                if (EventDefine[0] != "")
                                {

                                    switch (EventDefine[0])
                                    {
                                        case Socket_Static.ReplyDeviceError_01:   //Response Abnormal Code
                                        case Socket_Static.ReplyFormatError_02:
                                        case Socket_Static.ReplyAlarmTable_03:
                                        case Socket_Static.ReplyInProcess_06:
                                        case Socket_Static.ReplyModeError_07:
                                        case Socket_Static.ReplyMappingError_08:
                                            {
                                                ReceiverQueue.EnQueue(string.Format("{0}", EventDefine[0]));
                                            }
                                            break;

                                        case Socket_Static.ReplyInterlock_04:
                                            {
                                                Ui_Interlock = EventDefine[3];
                                                ReceiverQueue.EnQueue(string.Format("{0},{1}", EventDefine[0], Ui_Interlock));
                                            }
                                            break;
                                        case Socket_Static.ReplyInAlarm_05:
                                            {
                                                ReceiverQueue.EnQueue(string.Format("{0},{1}", EventDefine[0], Ui_Alarm));
                                            }
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (EventDefine[1])
                                    {
                                        case Socket_Static.ReplyINF:
                                            {
                                          
                                                InfCmd_Handler(EventDefine[2]);
                                            }
                                            break;

                                        case Socket_Static.ReplyABS:
                                            {
                                                Ui_Alarm = EventDefine[3];
                                                ReceiverQueue.EnQueue(string.Format("{0},{1}", Socket_Static.ReplyInAlarm_05, EventDefine[3]));
                                            }
                                            break;

                                        case Socket_Static.ReplyGET:
                                            {
                                                string[] GET_str = CompleteString.Split(new string[] { ":", "/", ";" }, StringSplitOptions.None);
                                                ReceiverQueue.EnQueue(string.Format("{0},{1}", GET_str[1], GET_str[2]));
                                            }
                                            break;

                                        case Socket_Static.ReplyMOV:
                                            {
                                                if (EventDefine[2] != "FCCL" && EventDefine[2] != "FCOP")
                                                    Ui_LoadStatus = LPPosition.InProcess;
                                            }
                                            break;
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                NowErrorMsg = string.Format("0x{0}-0x{1}", check_sum.ToUpper(), recieve_check_sum.ToUpper());
                                NowErrorList = ErrorList.LP_ChecksumError_0403;
                            }
                        }
                    }
                    break;
            }

        }

        private void InfCmd_Handler(string command_name)
        {
            switch (command_name)
            {
                case "PDOF":
                    {
                       // ULD_protocol.LP_FP_OUT(DeviceName);
                        Ui_FoupPresent = LPFoupMount.Absent;
                        ULD_protocol.LP_FP_OUT(DeviceName); //Joanne 20220908 Add
                        CommandQueue.EnQueue(new LPStep[] {  LPStep.UnloadLED_Off,
                                                             LPStep.SwitchLED_Off,
                                                             LPStep.ResetSlotData,
                                                             LPStep.EventEnd });
                        //SocketClient.Send_NormalReceive(SocketCommand.Event, DeviceName, HCT_EFEM.EFEM_HasTable.LP_Event[command_name].ToString());
                        SocketClient.Send_NormalReceive(SocketCommand.Event, DeviceName, command_name);

                        
                    }
                    break;

                case "PDON":
                    {
                        
                        Ui_FoupPresent = LPFoupMount.Present;
                        ULD_protocol.LP_FP_IN(DeviceName); //Joanne 20220908 Add

                        if (HT.EFEM.Status == EFEMStatus.Ready_Now || HT.EFEM.Status == EFEMStatus.Ready_Finish || HT.EFEM.Status == EFEMStatus.Run_Now || HT.EFEM.Status == EFEMStatus.Run_Finish)
                        {
                            CommandQueue.EnQueue(new LPStep[] {  LPStep.SwitchLED_Blink,
                                                             LPStep.ResetSlotData,
                                                             LPStep.EventEnd });
                        }
                        //SocketClient.Send_NormalReceive(SocketCommand.Event, DeviceName, HCT_EFEM.EFEM_HasTable.LP_Event[command_name].ToString());
                        SocketClient.Send_NormalReceive(SocketCommand.Event, DeviceName, command_name);
                    }
                    break;

                case "PLAC":
                case "PRES":
                    {
                        ULD_protocol.LP_FP_OUT(DeviceName);
                        Ui_FoupPresent = LPFoupMount.Unknown;


                        inpinjRFID.UI_clear(DeviceNo);
                    }
                    break;

                case "PWON":
                    {
                        CommandQueue.EnQueue(new LPStep[] {  LPStep.GetStatus,
                                                             LPStep.EventEnd });
                        //SocketClient.Send_NormalReceive(SocketCommand.Event, DeviceName, HCT_EFEM.EFEM_HasTable.LP_Event[command_name].ToString());
                        SocketClient.Send_NormalReceive(SocketCommand.Event, DeviceName, command_name);
                    }
                    break;

                case "AIRD":
                case "MESW":
                    {
                        break;
                    }
                case "MNSW":
                    {
                        //Trigger Read ID Wayne Test
                        if (HT.EFEM.Status == EFEMStatus.Ready_Now || HT.EFEM.Status == EFEMStatus.Ready_Finish || HT.EFEM.Status == EFEMStatus.Run_Now || HT.EFEM.Status == EFEMStatus.Run_Finish)
                        {
                            //Joanne 20220908 Edit
                            if (HT.EFEM.Mode == EFEMMode.Local)
                            {
                                if (string.IsNullOrEmpty(HT.LP_RFID[DeviceNo]) == false) //LP_RFID is foup id
                                {
                                    Cmd_EnQueue(SocketCommand.Load);

                                }
                            }
                            else
                            {
                                UserUnloader.SendCommand(string.Format("FoupID,{0},{1}", DeviceName, HT.LP_RFID[DeviceNo]));
                            }
                        }
                    }
                    break;

                default:
                    {
                        switch (command_name)
                        {
                            case "ORGN":
                            case "FPUL":
                                Ui_LoadStatus = LPPosition.Unload;
                                break;
                            case "MAPP":
                            case "FPML":
                                Ui_LoadStatus = LPPosition.Load;
                                break;
                        }

                        ReceiverQueue.EnQueue(string.Format("{0},{1}", Socket_Static.ReplyINF, command_name));
                    }
                    break;
            }
        }

        #endregion

        #region Queue/Command

        public void Cmd_EnQueue(SocketCommand cmd)
        {
            UI.Log(NormalStatic.LP, DeviceName, SystemList.CommandStart, string.Format("{0}:({1})", cmd, ""));

            Ui_Busy = true;
            Ui_Interlock = Socket_Static.ReplyNormal_00;
            MainCmd = cmd;
            LoadPort_StepArray.Clear();
            NowErrorList = ErrorList.MaxCnt;
            NowErrorMsg = "";
            //NowStepCnt = 0;
            //CommandQueue.Clear();
            //ReceiverQueue.Clear();
            //ReceiveNowStepCnt = NormalStatic.Idle;

            switch (cmd)
            {
                case SocketCommand.Initial:
                    {
                        #region Initial

                        LoadPort_StepArray.Add(LPStep.GetType1);
                        LoadPort_StepArray.Add(LPStep.GetType2);
                        LoadPort_StepArray.Add(LPStep.GetType3);
                        LoadPort_StepArray.Add(LPStep.GetType4);
                        LoadPort_StepArray.Add(LPStep.GetType5);
                        LoadPort_StepArray.Add(LPStep.GetStatus);
                        LoadPort_StepArray.Add(LPStep.ResetSlotData);

                        #endregion
                    }
                    break;

                case SocketCommand.InitialHome:
                    {
                        #region Initial Home
                        LoadPort_StepArray.Add(LPStep.ResetError);
                        LoadPort_StepArray.Add(LPStep.GetStatus);
                        LoadPort_StepArray.Add(LPStep.LoadLED_Off);
                        LoadPort_StepArray.Add(LPStep.UnloadLED_Blink);
                        LoadPort_StepArray.Add(LPStep.SwitchLED_Off);
                        LoadPort_StepArray.Add(LPStep.Home);
                        LoadPort_StepArray.Add(LPStep.ResetSlotData);
                        LoadPort_StepArray.Add(LPStep.UnloadLED_On);
                        LoadPort_StepArray.Add(LPStep.GetStatus);
                        #endregion
                    }
                    break;

                case SocketCommand.ResetError:
                    {
                        #region ResetError
                        LoadPort_StepArray.Add(LPStep.ResetError);
                        LoadPort_StepArray.Add(LPStep.GetStatus);
                        #endregion
                    }
                    break;

                case SocketCommand.GetStatus:
                case SocketCommand.GetMapp:
                case SocketCommand.GetProtrusionSensor:
                case SocketCommand.GetWaferSlot:
                case SocketCommand.GetWaferSlot2:
                case SocketCommand.GetWaferPosition:
                case SocketCommand.GetWaferThickness:
                    {
                        LoadPort_StepArray.Add(LPStep.GetStatus);
                    }
                    break;

                case SocketCommand.Home:
                    {
                        #region Home
                        LoadPort_StepArray.Add(LPStep.LoadLED_Off);
                        LoadPort_StepArray.Add(LPStep.UnloadLED_Blink);
                        LoadPort_StepArray.Add(LPStep.SwitchLED_Off);
                        LoadPort_StepArray.Add(LPStep.Home);
                        LoadPort_StepArray.Add(LPStep.ResetSlotData);
                        LoadPort_StepArray.Add(LPStep.UnloadLED_On);
                        LoadPort_StepArray.Add(LPStep.GetStatus);
                        #endregion
                    }
                    break;

                case SocketCommand.Load:
                    {
                        #region Load

                        LoadPort_StepArray.Add(LPStep.UnloadLED_Off);
                        LoadPort_StepArray.Add(LPStep.LoadLED_Blink);
                        LoadPort_StepArray.Add(LPStep.SwitchLED_Off);
                        LoadPort_StepArray.Add(LPStep.Load);
                        LoadPort_StepArray.Add(LPStep.LoadLED_On);
                        DoStep_UpdateSlotInfo();
                        LoadPort_StepArray.Add(LPStep.GetStatus);

                        #endregion
                    }
                    break;

                case SocketCommand.Unload:
                    {
                        #region Unload

                        LoadPort_StepArray.Add(LPStep.LoadLED_Off);
                        LoadPort_StepArray.Add(LPStep.UnloadLED_Blink);
                        LoadPort_StepArray.Add(LPStep.SwitchLED_Off);
                        LoadPort_StepArray.Add(LPStep.Unload);
                        LoadPort_StepArray.Add(LPStep.ResetSlotData);
                        LoadPort_StepArray.Add(LPStep.UnloadLED_On);
                        LoadPort_StepArray.Add(LPStep.GetStatus);

                        #endregion
                    }
                    break;

                case SocketCommand.Clamp:
                    {
                        #region Clamp

                        LoadPort_StepArray.Add(LPStep.Clmap);
                        LoadPort_StepArray.Add(LPStep.GetStatus);

                        #endregion
                    }
                    break;

                case SocketCommand.UnClamp:
                    {
                        #region UnClamp
                        LoadPort_StepArray.Add(LPStep.Unclmap);
                        LoadPort_StepArray.Add(LPStep.GetStatus);
                        #endregion
                    }
                    break;

                case SocketCommand.Map:
                    {
                        #region Map

                        LoadPort_StepArray.Add(LPStep.DoMapping);
                        DoStep_UpdateSlotInfo();
                        LoadPort_StepArray.Add(LPStep.GetStatus);

                        #endregion
                    }
                    break;

                case SocketCommand.SetType:
                    {
                        #region Set Type

                        switch (DataSend_Parameter_string)
                        {
                            case "0":
                                {
                                    LoadPort_StepArray.Add(LPStep.SetType1);
                                }
                                break;
                            case "1":
                                {
                                    LoadPort_StepArray.Add(LPStep.SetType2);
                                }
                                break;
                            case "2":
                                {
                                    LoadPort_StepArray.Add(LPStep.SetType3);
                                }
                                break;
                            case "3":
                                {
                                    LoadPort_StepArray.Add(LPStep.SetType4);
                                }
                                break;
                            case "4":
                                {
                                    LoadPort_StepArray.Add(LPStep.SetType5);
                                }
                                break;
                        }
                        LoadPort_StepArray.Add(LPStep.GetStatus);
                        LoadPort_StepArray.Add(LPStep.ResetSlotData);

                        #endregion
                    }
                    break;

                case SocketCommand.LEDLoad:
                    {
                        #region LEDLoad

                        switch (DataSend_Parameter_string)
                        {
                            case NormalStatic.On:
                                {
                                    LoadPort_StepArray.Add(LPStep.LoadLED_On);
                                }
                                break;
                            case NormalStatic.Off:
                                {
                                    LoadPort_StepArray.Add(LPStep.LoadLED_Off);
                                }
                                break;
                            case NormalStatic.Flash:
                                {
                                    LoadPort_StepArray.Add(LPStep.LoadLED_Blink);
                                }
                                break;
                        }

                        #endregion
                    }
                    break;

                case SocketCommand.LEDUnLoad:
                    {
                        #region LEDUnLoad

                        switch (DataSend_Parameter_string)
                        {
                            case NormalStatic.On:
                                {
                                    LoadPort_StepArray.Add(LPStep.UnloadLED_On);
                                }
                                break;
                            case NormalStatic.Off:
                                {
                                    LoadPort_StepArray.Add(LPStep.UnloadLED_Off);
                                }
                                break;
                            case NormalStatic.Flash:
                                {
                                    LoadPort_StepArray.Add(LPStep.UnloadLED_Blink);
                                }
                                break;
                        }

                        #endregion
                    }
                    break;
                case SocketCommand.LEDStatus1:
                    {
                        #region LEDStatus1

                        switch (DataSend_Parameter_string)
                        {
                            case NormalStatic.On:
                                {
                                    LoadPort_StepArray.Add(LPStep.Status1LED_On);
                                }
                                break;
                            case NormalStatic.Off:
                                {
                                    LoadPort_StepArray.Add(LPStep.Status1LED_Off);
                                }
                                break;
                            case NormalStatic.Flash:
                                {
                                    LoadPort_StepArray.Add(LPStep.Status1LED_Blink);
                                }
                                break;
                        }

                        #endregion
                    }
                    break;
                case SocketCommand.LEDStatus2:
                    {
                        #region LEDStatus2

                        switch (DataSend_Parameter_string)
                        {
                            case NormalStatic.On:
                                {
                                    LoadPort_StepArray.Add(LPStep.Status2LED_On);
                                }
                                break;
                            case NormalStatic.Off:
                                {
                                    LoadPort_StepArray.Add(LPStep.Status2LED_Off);
                                }
                                break;
                            case NormalStatic.Flash:
                                {
                                    LoadPort_StepArray.Add(LPStep.Status2LED_Blink);
                                }
                                break;
                        }

                        #endregion
                    }
                    break;
                case SocketCommand.SetOperatorAccessButton:
                    {
                        #region SetOperatorAccessButton

                        switch (DataSend_Parameter_string)
                        {
                            case NormalStatic.On:
                                {
                                    LoadPort_StepArray.Add(LPStep.SwitchLED_On);
                                }
                                break;
                            case NormalStatic.Off:
                                {
                                    LoadPort_StepArray.Add(LPStep.SwitchLED_Off);
                                }
                                break;
                            case NormalStatic.Flash:
                                {
                                    LoadPort_StepArray.Add(LPStep.SwitchLED_Blink);
                                }
                                break;
                        }

                        #endregion
                    }
                    break;

                case SocketCommand.GetLEDStatus:
                    {
                        LoadPort_StepArray.Add(LPStep.GetLEDStatus);
                    }
                    break;

                case SocketCommand.GetZAxisPos:
                    {
                        LoadPort_StepArray.Add(LPStep.GetZAxisPos);
                    }
                    break;

                case SocketCommand.SetMapp:
                    {
                        #region Set Mapp

                        LoadPort_StepArray.Add(LPStep.SetMappParameter);
                        switch (DataSend_Parameter_string[0])
                        {
                            case '0':
                                LoadPort_StepArray.Add(LPStep.GetType1);
                                break;
                            case '1':
                                LoadPort_StepArray.Add(LPStep.GetType2);
                                break;
                            case '2':
                                LoadPort_StepArray.Add(LPStep.GetType3);
                                break;
                            case '3':
                                LoadPort_StepArray.Add(LPStep.GetType4);
                                break;
                            case '4':
                                LoadPort_StepArray.Add(LPStep.GetType5);
                                break;
                        }
                        LoadPort_StepArray.Add(LPStep.ResetSlotData);

                        #endregion
                    }
                    break;
            }
            LoadPort_StepArray.Add(LPStep.MaxCnt);
            CommandQueue.EnQueue(LoadPort_StepArray.ToArray<LPStep>());
        }

        private void DoStep_UpdateSlotInfo()
        {
            LoadPort_StepArray.Add(LPStep.ResetSlotData);
            LoadPort_StepArray.Add(LPStep.GetWaferStatus);
            LoadPort_StepArray.Add(LPStep.GetWaferThick01);
            LoadPort_StepArray.Add(LPStep.GetWaferPosition01);

            if (SlotInfo.Length > 5)
            {
                LoadPort_StepArray.Add(LPStep.GetWaferThick02);
                LoadPort_StepArray.Add(LPStep.GetWaferPosition02);
            }
            if (SlotInfo.Length > 10)
            {
                LoadPort_StepArray.Add(LPStep.GetWaferThick03);
                LoadPort_StepArray.Add(LPStep.GetWaferPosition03);
            }
            if (SlotInfo.Length > 15)
            {
                LoadPort_StepArray.Add(LPStep.GetWaferThick04);
                LoadPort_StepArray.Add(LPStep.GetWaferPosition04);
            }
            if (SlotInfo.Length > 20)
            {
                LoadPort_StepArray.Add(LPStep.GetWaferThick05);
                LoadPort_StepArray.Add(LPStep.GetWaferPosition05);
            }
            if (SlotInfo.Length > 25)
            {
                LoadPort_StepArray.Add(LPStep.GetWaferThick06);
                LoadPort_StepArray.Add(LPStep.GetWaferPosition06);
            }
        }

        #endregion   

        #region Tool Tip TypeInfo

        private void labType_MouseEnter(object sender, EventArgs e)
        {
            string Tooltip_Msg;
            Tooltip_Msg = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                string.Format("Wafer Thick:{0} (um)\n", MappParameter[(int)(LPType)Ui_Type].WaferThick),
                string.Format("Wafer Pitch:{0} (um)\n", MappParameter[(int)(LPType)Ui_Type].Pitch),
                string.Format("Wafer SlotNumber:{0}\n", MappParameter[(int)(LPType)Ui_Type].SlotNumber),
                string.Format("Wafer Offset:{0} (um)\n", MappParameter[(int)(LPType)Ui_Type].Offset),
                string.Format("Wafer Thick TOL:{0} (um)\n", MappParameter[(int)(LPType)Ui_Type].Pitch_Thick_TOL),
                string.Format("Wafer Position TOL:{0} (um)\n", MappParameter[(int)(LPType)Ui_Type].Position_TOL),
                string.Format("Wafer Type:{0} (Inch)\n", ((MappParameter[(int)(LPType)Ui_Type].WaferType == 0) ? 12 : 8))
                );
            tipInfo.SetToolTip((Label)sender, Tooltip_Msg);
        }

        private void labpitch_MouseEnter(object sender, EventArgs e)
        {
            tipInfo.SetToolTip((Label)sender, "Wafer Position Gap (mm)");
        }

        #endregion

    }
}
