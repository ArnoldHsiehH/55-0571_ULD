using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Collections;
using System.Threading;
using System.IO;


namespace HirataMainControl
{
    public partial class HCT_Aligner : UserControl
    {
        #region Class

        public class AngleClass
        {
            public string FindNotch;
            public string ToAngle;
        }

        #endregion

        #region Event/Delegate

        public delegate void AlignerEvent(string PortName, SocketCommand command, bool Result);
        public event AlignerEvent ActionComplete;

        #endregion

        #region COM

        private SerialPort AlignerCOM;

        #endregion

        #region BG/Queue

        private Thread AutoMoveBG;
        private BlockQueue<string[]> CommandQueue = new BlockQueue<string[]>();
        private BlockQueue<AlignerStep[]> AutoStepQueue = new BlockQueue<AlignerStep[]>();
        private BlockQueue<string> ReceiverQueue = new BlockQueue<string>();

        #endregion

        #region List

        private List<AlignerStep> Aligner_StepArray = new List<AlignerStep>();
        private List<byte> ReceiveTemp = new List<byte>();

        #endregion

        #region Public_Variable

        public string DeviceName;
        private All_Device DeviceNameEnum = All_Device.Fn;

        //public AngleClass[] AngleArray = new AngleClass[2];  //Sen Check 
        public ErrorList NowErrorList = ErrorList.MaxCnt;
        public string NowErrorMsg = "";

        private string[] strArr = { };
        private char[] REC = { };
        private string strTemp = string.Empty;
        public string[] strParam = new string[2];
        public string Check_PLC_ReceiveIsOK = "";

        #endregion

        #region Private_Variable

        #region UI

        public CommandResult Result = CommandResult.Unknown;
        private bool ConncetStatus;
        private AlignerStatus Status = AlignerStatus.Unknown;
        private bool Busy = false;
        private bool WaferProtrude = false;
        private WaferStatus Presence = WaferStatus.WithOut;
        private bool Vac = false;
        private string Alarm = "Normal";
        private string NotchAngle = "0";
        private string ToAngle = "0";
        private string WaferInfo = "0";
        private string Type;
        private string Mode;
        private LiftPinEnum LiftPin = LiftPinEnum.Unknown;

        #endregion

        #region Command

        private SocketCommand MainCmd = SocketCommand.MaxCnt;
        private AlignerStep[] MarcoCommand;
        private int NowStepCnt = 0;
        private string REC_string = "";
        private string CMD_string = "";
        private int ReceiveNowStepCnt = NormalStatic.Idle;
        private int TimeoutCount = 50000; // 50S 

        #endregion

        #endregion

        #region Initial

        public HCT_Aligner()
        {
            InitializeComponent();
        }

        public void Initial(ref int number)
        {
            gbxAligner.Text = DeviceName = string.Format("{0}{1}", NormalStatic.Aligner, number + 1);
            DeviceNameEnum = (All_Device)Enum.Parse(typeof(All_Device), string.Format("{0}{1}", NormalStatic.Aligner, number + 1));

            #region RS232_Initial

            int IniCOM = Convert.ToInt32(AppSetting.LoadSetting(string.Format("{0}{1}", NormalStatic.Aligner, "_COM"), "11"));
            AlignerCOM = new SerialPort(string.Format("COM{0}", IniCOM + number));
            AlignerCOM.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(Aligner_COM_DataReceived);
            AlignerCOM.BaudRate = 19200;
            AlignerCOM.DataBits = 8;
            AlignerCOM.Parity = Parity.None;
            AlignerCOM.StopBits = StopBits.One;

            #endregion

            Ui_Mode = "Manual";

            AutoMoveBG = new Thread(AutoBG_DoWork);
            AutoMoveBG.Start();
            AutoMoveBG.IsBackground = true;

            //Connect
            //COM_Connect();
        }

        #endregion

        #region RS232

        public void COM_Connect()
        {
            Ui_Connect = false;
            AlignerCOM.Close();
            try
            {
                AlignerCOM.Open();
                Ui_Connect = true;
            }
            catch
            {
                Ui_Connect = false;
                UI.InitialSystem(DeviceName, NormalStatic.False, ErrorList.AP_SerialError_0381);
            }
        }

        public void COM_Disconnect()
        {
            Ui_Connect = false;
           // AlignerCOM.Close();
        }

        private void Aligner_DataSend(string Command_String)
        {
            string Send_String = string.Format("0000{0}", Command_String);
            //List<byte> Sum_Byte = new List<Byte>(Encoding.ASCII.GetBytes(Send_String));
            //string check_sum = Convert.ToString(CheckSum(Sum_Byte.ToArray()), 16).ToUpper();
            string check_sum = CheckSum(Send_String);
            Send_String = string.Format("{0}{1}", Send_String, check_sum);
            List<byte> Command_Byte = new List<Byte>(Encoding.ASCII.GetBytes(Send_String));
            //加2byte 前後各1byte
            Command_Byte.Insert(0, NormalStatic.StartByte_SOH);
            Command_Byte.Add(NormalStatic.EndByte_CR);

            AlignerCOM.Write(Command_Byte.ToArray(), 0, Command_Byte.Count);
            UI.Log(NormalStatic.Aligner, DeviceName, SystemList.DeviceSend, Command_String);
        }

        private string CheckSum(string strTmp)
        {
            int sum = 0;
            foreach (char Character in strTmp)
                sum += Convert.ToByte(Character);

            sum = sum & 0xff;
            string hexstring = string.Format("{0:X2}", sum);
            return hexstring;
        }

        private void Aligner_COM_DataReceived(object sender, SerialDataReceivedEventArgs e)
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

        private void DataCombine(byte[] raw_data)
        {
            //  for (int i = 0; i < raw_data.Length; i++)
            {
                switch (ReceiveNowStepCnt)
                {
                    case NormalStatic.Idle:
                        {
                            UI.Alarm(DeviceName, ErrorList.AP_SerialError_0381, string.Format(" 0x{0}", Convert.ToString(raw_data[0], 16).ToUpper()));
                        }
                        break;

                    case NormalStatic.WaitReply:
                        {
                            if (raw_data[0] != NormalStatic.StartByte_SOH)
                            {
                                NowErrorMsg += string.Format(" 0x{0}", Convert.ToString(raw_data[0], 16).ToUpper());
                                NowErrorList = ErrorList.AP_SerialError_0381;
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
                            else
                            {
                                string CompleteString = Encoding.ASCII.GetString(ReceiveTemp.GetRange(0, ReceiveTemp.Count - 2).ToArray());
                                string iCheckSum = CheckSum(CompleteString);
                                string recCheckSum = Encoding.ASCII.GetString(ReceiveTemp.ToArray(), ReceiveTemp.Count - 2, 2);
                                string recCommand = CompleteString.Substring(4, 3);
                                string strError = CompleteString.Substring(0, 2);
                                strArr = (CompleteString.Split(new string[] { "/", ";" }, 16, StringSplitOptions.None));

                                ReceiveNowStepCnt = NormalStatic.WaitReply;
                                if (iCheckSum == recCheckSum) //1.檢查CheckSum 
                                {
                                    if (strError == Socket_Static.ReplyNormal_00) //檢查Error Code
                                    {
                                        //帶參數, INF, ABS 皆後丟處理
                                        if ((strArr.Length > 2) || (recCommand == Socket_Static.ReplyINF) || (recCommand == Socket_Static.ReplyABS))
                                        {
                                            ReceiverQueue.EnQueue(CompleteString);
                                        }
                                    }
                                    else
                                    {   //後丟 報錯
                                        ReceiverQueue.EnQueue(CompleteString);
                                    }
                                }
                                else
                                {   //回報CheckSum錯誤
                                    NowErrorMsg = string.Format("0x{0}-0x{1}", Convert.ToString(raw_data[0], 16).ToUpper(), recCheckSum.ToUpper());
                                    NowErrorList = ErrorList.ChecksumError_0101;
                                }
                            }
                        }
                        break;
                }
            }
        }

        private void ReceiveHandler()
        {
            string[] CMD = (strArr[0].Substring(4, strArr[0].Length - 4).Replace(";", "")).Split(new string[] { ":" }, StringSplitOptions.None);
            string rErrorCode = strArr[0].Substring(0, 2);
            string rEven = CMD[1];

            //1.確認是否有response codes
            if (HCT_EFEM.EFEM_HasTable.Aligner_Codes.Contains(rErrorCode))
            {
                Ui_Alarm = "Error";
                NowErrorList = (ErrorList)HCT_EFEM.EFEM_HasTable.Aligner_Codes[rErrorCode];
                ActionComplete(DeviceName, MainCmd, false);
                JobFail();

            }
            else if (rErrorCode == Socket_Static.ReplyInterlock_04)
            {
                Ui_Alarm = "Error";
                NowErrorList = (ErrorList)HCT_EFEM.EFEM_HasTable.Aligner_InterLock[strArr[1]];
                ActionComplete(DeviceName, MainCmd, false);
                JobFail();
            }
            else
            {
                switch (CMD[0])
                {
                    case Socket_Static.ReplyINF:
                        {
                            if (CMD[1] == "RSET")
                            {
                                Ui_Alarm = "Normal";
                            }
                            else if (CMD[1] == "ORGN")
                            {
                                Ui_Status = AlignerStatus.Home;
                            }
                            else if (CMD[1] == "ARLD")
                            {
                                Ui_Status = AlignerStatus.Alignment;
                            }
                            else if (CMD[1] == "OFSE")
                            {
                                double vOut = Convert.ToDouble(int.Parse(strParam[0]));
                                Ui_ToAngle = (vOut / 10).ToString();
                            }
                            else if (CMD[1] == "OFS2")
                            {
                                double vOut = Convert.ToDouble(int.Parse(strParam[1]));
                                Ui_NotchAngle = (vOut / 10).ToString();

                            }
                            else if (CMD[1] == "ACCL")
                            {
                                Ui_Vac = true;
                            }
                            else if (CMD[1] == "ACOP")
                            {
                                Ui_Vac = false;
                            }


                        }
                        break;

                    case Socket_Static.ReplyABS:
                        {
                            if (HCT_EFEM.EFEM_HasTable.Aligner_Error.Contains(strArr[1]))
                            {
                                Ui_Alarm = "Error";
                                NowErrorList = (ErrorList)HCT_EFEM.EFEM_HasTable.Aligner_Error[strArr[1]];
                                ActionComplete(DeviceName, MainCmd, false);
                                JobFail();
                            }
                        }
                        break;

                    #region case "GET"

                    case Socket_Static.ReplyGET:
                        {
                            if (strArr.Length > 1)
                            {
                                REC = strArr[1].ToCharArray();
                            }

                            switch (CMD[1])
                            {

                                case "STAS":
                                    {
                                       // ULD_protocol.ALstatus(CMD[1], REC_string);//CompleteString
                                        //如Status狀態為A1=Error, Show Alarm, 帶出Error Code錯誤.
                                        if (HCT_EFEM.EFEM_HasTable.Aligner_Getstatus.Contains(string.Format("A{0}", REC[0])))
                                        {
                                            if (HCT_EFEM.EFEM_HasTable.Aligner_Getstatus[string.Format("A{0}", REC[0])].ToString() == "Error")
                                            {
                                                Ui_Alarm = HCT_EFEM.EFEM_HasTable.Aligner_Getstatus[string.Format("A{0}", REC[0])].ToString();

                                                if (HCT_EFEM.EFEM_HasTable.Aligner_Getstatus.Contains(string.Format("E{0}{1}", REC[4], REC[5])))
                                                {
                                                    Ui_Alarm = "Error";
                                                    NowErrorList = (ErrorList)HCT_EFEM.EFEM_HasTable.Aligner_Error[string.Format("E{0}{1}", REC[4], REC[5]).ToString()];
                                                    JobFail();
                                                }
                                            }
                                            Ui_Alarm = HCT_EFEM.EFEM_HasTable.Aligner_Getstatus[string.Format("A{0}", REC[0])].ToString();
                                        }

                                        if (HCT_EFEM.EFEM_HasTable.Aligner_Getstatus.Contains(string.Format("B{0}", REC[1])))
                                        {
                                            Ui_Mode = HCT_EFEM.EFEM_HasTable.Aligner_Getstatus[string.Format("B{0}", REC[1])].ToString();
                                        }

                                        strTemp = string.Format("F{0}{1}{2}", REC[6], REC[7], REC[8]);
                                        if (HCT_EFEM.EFEM_HasTable.Aligner_Getstatus.Contains(strTemp))
                                        {
                                            if (HCT_EFEM.EFEM_HasTable.Aligner_Getstatus[strTemp].ToString() == "Without wafer")
                                                Ui_Presence = WaferStatus.WithOut;
                                            else
                                                Ui_Presence = WaferStatus.With;
                                        }

                                        if (HCT_EFEM.EFEM_HasTable.Aligner_Getstatus.Contains(string.Format("G{0}", REC[9])))
                                        {
                                            if (HCT_EFEM.EFEM_HasTable.Aligner_Getstatus[string.Format("G{0}", REC[9])].ToString() == "Origin status")
                                            {
                                                Ui_Status = AlignerStatus.Home;
                                            }
                                            else
                                                Ui_Status = AlignerStatus.Unknown;
                                        }

                                        if (HCT_EFEM.EFEM_HasTable.Aligner_Getstatus.Contains(string.Format("H{0}", REC[10])))
                                        {
                                            if (HCT_EFEM.EFEM_HasTable.Aligner_Getstatus[string.Format("H{0}", REC[10])].ToString() == "OFF")
                                                Ui_Vac = false;
                                            else
                                                Ui_Vac = true;
                                        }

                                        strTemp = string.Format("J{0}{1}{2}", REC[12], REC[13], REC[14]);
                                        if (HCT_EFEM.EFEM_HasTable.Aligner_Getstatus.Contains(strTemp))
                                        {
                                            switch (strTemp)
                                            {
                                                case "J100":
                                                case "J300":
                                                    {
                                                        Ui_Status = AlignerStatus.FindNotch;
                                                    }
                                                    break;

                                                case "J200":
                                                    {
                                                        Ui_Status = AlignerStatus.ToAngle;
                                                    }
                                                    break;

                                                case "J000":
                                                    {
                                                        Ui_Status = AlignerStatus.Home;
                                                    }
                                                    break;

                                                default:
                                                    Ui_Status = AlignerStatus.Unknown;
                                                    break;
                                            }

                                        }
                                    }
                                    break;

                                case "TYPE":
                                    {
                                        if (HCT_EFEM.EFEM_HasTable.Aligner_GetType.Contains(REC[0].ToString()))
                                        {
                                            Ui_Type = HCT_EFEM.EFEM_HasTable.Aligner_GetType[REC[0].ToString()].ToString();
                                        }
                                        else
                                            Ui_Type = strArr[1];
                                    }
                                    break;

                                case "OFSE":
                                    {
                                        double vOut = Convert.ToDouble(int.Parse(strArr[1]));
                                        Ui_ToAngle = (vOut / 10).ToString();
                                    }
                                    break;

                                case "OFS2":
                                    {
                                        double vOut = Convert.ToDouble(int.Parse(strArr[1]));
                                        Ui_NotchAngle = (vOut / 10).ToString();
                                    }
                                    break;

                            }
                        }
                        break;

                        #endregion

                        #region case "SET"

                        //case Socket_Static.ReplySET:
                        //    {
                        //        if (CMD[1].Contains("OFSE"))
                        //        {
                        //            float vOut = Convert.ToSingle(int.Parse(strParam) / 10);
                        //            Ui_ToAngle = strParam;
                        //        }
                        //        else if (CMD[1].Contains("OFS2"))
                        //        {
                        //            Ui_NotchAngle = CMD[1].Substring(4, 4);
                        //        } 
                        //    }
                        //    break;

                        #endregion

                }
            }
            NowStepCnt++;
        }

        #endregion

        #region Get/Set

        public bool Ui_Busy
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_Busy = value; }));
                    return;
                }
                labBusy.BackColor = value ? Color.Yellow : Color.LightGreen;
                labBusy.Text = value ? "Busy" : "Idle";
                Busy = value;
            }
            get { return Busy; }
        }

        public string Ui_WaferInfo
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_WaferInfo = value; }));
                    return;
                }
                labWaferInfo.Text = value;
                WaferInfo = value;
            }
            get { return WaferInfo; }
        }

        public string Ui_NotchAngle
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_NotchAngle = value; }));
                    return;
                }
                labNotchAngle.Text = value;
                NotchAngle = value;
            }
            get { return NotchAngle; }
        }

        public string Ui_ToAngle
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_ToAngle = value; }));
                    return;
                }
                labToAngle.Text = value;
                ToAngle = value;
            }
            get { return ToAngle; }
        }

        public string Ui_Alarm
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_Alarm = value; }));
                    return;
                }
                switch (value)
                {
                    case "Normal":
                        labAlarm.BackColor = Color.LightGreen;
                        break;
                    default:
                        labAlarm.BackColor = Color.Red;
                        break;
                }
                labAlarm.Text = value;
                Alarm = value;
            }
            get { return Alarm; }
        }

        public AlignerStatus Ui_Status    //Sen Check 
        {
            set
            {
                //ULD_protocol.AL_Status(value.ToString());
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_Status = value; }));
                    return;
                }

                switch (value)
                {
                    case AlignerStatus.Home:
                        labStatus.BackColor = Color.LightGreen;
                        break;

                    case AlignerStatus.ToAngle:
                    case AlignerStatus.FindNotch:
                        labStatus.BackColor = Color.Yellow;
                        break;

                }
                labStatus.Text = value.ToString();
                Status = value;
            }
            get { return Status; }
        }

        public string Ui_Type
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_Type = value; }));
                    return;
                }
                labType.Text = value;
                Type = value;
            }
            get { return Type; }
        }

        public string Ui_Mode
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_Mode = value; }));
                    return;
                }
                labMode.Text = value;
                Mode = value;
            }
            get { return Mode; }
        }

        public WaferStatus Ui_Presence
        {
            set
            {
                //ULD_protocol.AL_WaferStatus(value == WaferStatus.With ? "With" : "WithOut");
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_Presence = value; }));
                    return;
                }
                picAligner.Image = value == WaferStatus.With ? (global::HirataMainControl.Properties.Resources.Aligner_With) : (global::HirataMainControl.Properties.Resources.Aligner_WithOut);
                Presence = value;
            }
            get { return Presence; }
        }

        public bool Ui_Vac
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_Vac = value; }));
                    return;
                }
                //labVac.BackColor = value ? Color.Yellow : Color.LightGreen;
                //labVac.Text = value ? "VacOn" : "VacOff";
                Vac = value;

            }
            get { return Vac; }
        }

        public bool Ui_WaferProtrude
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_WaferProtrude = value; }));
                    return;
                }
                picAligner.BackColor = value ? Color.Red : SystemColors.Control;
                WaferProtrude = value;
            }
            get { return WaferProtrude; }
        }

        public bool Ui_Connect
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_Connect = value; }));
                    return;
                }
                labConnect.BackColor = (value ? Color.LightGreen : Color.Red);    //Sen Check 
                labConnect.Text = (value ? "Con-C" : "Dis-C");
                ConncetStatus = value;
            }
            get { return ConncetStatus; }
        }

        public LiftPinEnum Ui_LiftPin
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_LiftPin = value; }));
                    return;
                }

                switch (value)
                {
                    case LiftPinEnum.Up:
                        labLiftPin.BackColor = Color.Yellow;
                        break;

                    case LiftPinEnum.Down:
                        labLiftPin.BackColor = Color.Yellow;
                        break;

                    case LiftPinEnum.Unknown:
                        labLiftPin.BackColor = Color.Red;
                        break;
                }

                labLiftPin.Text = value.ToString();
                LiftPin = value;

            }
            get { return LiftPin; }
        }

        #endregion

        #region BG

        private void AutoBG_DoWork()
        {
            while (true)
            {
                MarcoCommand = AutoStepQueue.DeQueue();

                if (MarcoCommand[NowStepCnt] == AlignerStep.End)
                {
                    UI.CloseBG(DeviceName);
                    break;
                }

                while (NowStepCnt < MarcoCommand.Length)
                {
                    ReceiveNowStepCnt = NormalStatic.WaitReply;
                    REC_string = "";
                    ReceiverQueue.Clear();

                    if (MarcoCommand[NowStepCnt] == AlignerStep.MaxCnt)
                    {
                        ActionComplete(DeviceName, MainCmd, true);
                        Ui_Busy = false;
                        break;
                    }

                    ReceiveTemp.Clear();

                    switch (MarcoCommand[NowStepCnt])
                    {
                        #region Action

                        case AlignerStep.Home:
                            {
                                Aligner_DataSend("MOV:ORGN;");
                            }
                            break;

                        case AlignerStep.Alignment:
                            {
                                Aligner_DataSend(string.Format("MOV:ARLD{0};", strParam[1]));
                            }
                            break;

                        case AlignerStep.FindNotch:
                            {
                                Aligner_DataSend("MOV:OTN2;");
                            }
                            break;

                        case AlignerStep.ToAngle:
                            {
                                Aligner_DataSend("MOV:N2TS;");
                            }
                            break;

                        case AlignerStep.AlignerVacuumOn:
                            {
                                Aligner_DataSend("MOV:ACCL;");
                            }
                            break;

                        case AlignerStep.AlignerVacuumOff:
                            {
                                Aligner_DataSend("MOV:ACOP;");
                            }
                            break;

                        #endregion

                        #region Set

                        case AlignerStep.ResetError:
                            {
                                Aligner_DataSend("SET:RSET;");
                            }
                            break;
                        case AlignerStep.SetAlignerDegree:
                            {
                                Aligner_DataSend(string.Format("SET:OFSE{0};", strParam[0]));
                            }
                            break;
                        case AlignerStep.SetIDReaderDegree:
                            {
                                Aligner_DataSend(string.Format("SET:OFS2{0};", strParam[1]));
                            }
                            break;

                        case AlignerStep.SetAlignerWaferType:
                            {
                                //  Aligner_DataSend(string.Format("SET:TYPE{0};", strParam));
                            }
                            break;

                        case AlignerStep.SetAlignerWaferTypeFix_8:
                            {
                                Aligner_DataSend("SET:TYPE04;");
                            }
                            break;
                        case AlignerStep.SetAlignerWaferTypeFix_12:
                            {
                                Aligner_DataSend("SET:TYPE05;");
                            }
                            break;
                        #endregion

                        #region Get

                        case AlignerStep.GetStatus:
                            {
                                Aligner_DataSend("GET:STAS;");
                            }
                            break;

                        case AlignerStep.GetAlignerDegree:
                            {
                                //double RealAngle = Math.Abs(Convert.ToDouble(AngleArray[(int)TempType].FindNotch) - Convert.ToDouble(AngleArray[(int)TempType].ToAngle));
                                //Aligner_COM_DataSend(string.Format("C {0}", Convert.ToInt32(Math.Round((RealAngle / 0.045), 0, MidpointRounding.AwayFromZero)).ToString()));
                                Aligner_DataSend("GET:OFSE;");
                            }
                            break;

                        case AlignerStep.GetIDReaderDegree:
                            {
                                Aligner_DataSend("GET:OFS2;");
                            }
                            break;

                        case AlignerStep.GetAlignerWaferType:
                            {
                                Aligner_DataSend("GET:TYPE;");
                            }
                            break;

                            #endregion


                    }

                    REC_string = ReceiverQueue.DeQueue(TimeoutCount);

                    UI.Log(NormalStatic.Aligner, DeviceName, SystemList.DeviceReceive, REC_string);
                    ReceiveNowStepCnt = NormalStatic.Idle;
                    if (REC_string == null)
                    {
                        if (NowErrorList == ErrorList.MaxCnt)
                            NowErrorList = ErrorList.Timeout_1010;
                        JobFail();
                        ActionComplete(DeviceName, MainCmd, false);
                    }
                    else if (REC_string == NormalStatic.True)
                        NowStepCnt++;
                    else if (REC_string == NormalStatic.End)
                        break;
                    else
                        ReceiveHandler();
                }
            }
        }

        private void ResetAlignerStatus()
        {
            Aligner_StepArray.Clear();
            NowStepCnt = 0;
            NowErrorList = ErrorList.MaxCnt;
            Result = CommandResult.Unknown;
            NowErrorMsg = "";
        }

        public void Close()
        {
            JobFail();
            NowStepCnt = 0;
            AutoStepQueue.EnQueue(new AlignerStep[] { AlignerStep.End });
            ReceiverQueue.EnQueue(NormalStatic.End);
            COM_Disconnect();
        }

        public void JobFail()
        {
            ReceiveNowStepCnt = NormalStatic.Idle;
            CommandQueue.Clear();
            AutoStepQueue.Clear();
            ReceiverQueue.Clear();
            NowStepCnt = 100;
        }

        #endregion

        #region Queue/Command

        public void Cmd_EnQueue(SocketCommand cmd)
        {
            UI.Log(NormalStatic.Aligner, DeviceName, SystemList.CommandStart, string.Format("{0}:({1})", cmd, ""));
            UI.Log(NormalStatic.System, DeviceName, SystemList.CommandStart, string.Format("{0}:({1})", cmd, ""));

            Ui_Busy = true;

            JobFail();
            MainCmd = cmd;
            ResetAlignerStatus();

            switch (cmd)
            {
                case SocketCommand.Initial:
                    {
                        Aligner_StepArray.Add(AlignerStep.ResetError);
                        Aligner_StepArray.Add(AlignerStep.GetStatus);
                        //Aligner_StepArray.Add(AlignerStep.SetAlignerWaferTypeFix_8);
                        Aligner_StepArray.Add(AlignerStep.SetAlignerWaferTypeFix_12);
                        Aligner_StepArray.Add(AlignerStep.GetAlignerWaferType);
                        Aligner_StepArray.Add(AlignerStep.GetAlignerDegree);
                        Aligner_StepArray.Add(AlignerStep.GetIDReaderDegree);

                    }
                    break;

                case SocketCommand.InitialHome:
                case SocketCommand.Home:
                    {
                        if (cmd == SocketCommand.InitialHome)
                            Aligner_StepArray.Add(AlignerStep.ResetError);

                        Aligner_StepArray.Add(AlignerStep.Home);
                    //    Aligner_StepArray.Add(AlignerStep.Set_PLC_Up);
                        Aligner_StepArray.Add(AlignerStep.GetStatus);
                    }
                    break;

                case SocketCommand.CycleHomeCheckDegree:
                    {
                    //    Aligner_StepArray.Add(AlignerStep.Set_PLC_Up);
                        Aligner_StepArray.Add(AlignerStep.SetAlignerDegree);
                        Aligner_StepArray.Add(AlignerStep.SetIDReaderDegree);

                        Aligner_StepArray.Add(AlignerStep.Home);
                        Aligner_StepArray.Add(AlignerStep.GetStatus);
                    }
                    break;

                case SocketCommand.CycleAlignmentFinish:
                    {
                  //      Aligner_StepArray.Add(AlignerStep.Set_PLC_Down);
                        Aligner_StepArray.Add(AlignerStep.FindNotch);
                        Aligner_StepArray.Add(AlignerStep.ToAngle);
                      //  Aligner_StepArray.Add(AlignerStep.Set_PLC_Up);
                        Aligner_StepArray.Add(AlignerStep.GetStatus);
                    }
                    break;

                case SocketCommand.CycleAlignmentOCR:
                    {
                     //   Aligner_StepArray.Add(AlignerStep.Set_PLC_Down);
                        Aligner_StepArray.Add(AlignerStep.FindNotch);
                    //    Aligner_StepArray.Add(AlignerStep.Set_PLC_Up);
                        Aligner_StepArray.Add(AlignerStep.GetStatus);
                    }
                    break;

                case SocketCommand.GetStatus:
                    {
                        Aligner_StepArray.Add(AlignerStep.GetStatus);
                    }
                    break;

                case SocketCommand.ResetError:
                    {
                        Aligner_StepArray.Add(AlignerStep.ResetError);
                    }
                    break;

                case SocketCommand.AlignerVacuum:
                    {
                        if (strParam[0] == NormalStatic.On)   //Sen Check 
                        {
                            Aligner_StepArray.Add(AlignerStep.AlignerVacuumOn);
                        }
                        else
                        {
                            Aligner_StepArray.Add(AlignerStep.AlignerVacuumOff);
                        }

                        Aligner_StepArray.Add(AlignerStep.GetStatus);

                    }
                    break;

                case SocketCommand.Alignment:
                    {
                      //  Aligner_StepArray.Add(AlignerStep.Set_PLC_Down);
                        Aligner_StepArray.Add(AlignerStep.Alignment);
                        Aligner_StepArray.Add(AlignerStep.GetAlignerDegree);
                     //   Aligner_StepArray.Add(AlignerStep.Set_PLC_Up);
                        Aligner_StepArray.Add(AlignerStep.GetStatus);
                    }
                    break;

                case SocketCommand.FindNotch:
                    {
                       // Aligner_StepArray.Add(AlignerStep.Set_PLC_Down);
                        Aligner_StepArray.Add(AlignerStep.FindNotch);
                        Aligner_StepArray.Add(AlignerStep.GetStatus);
                    }
                    break;

                case SocketCommand.ToAngle:
                    {
                        Aligner_StepArray.Add(AlignerStep.ToAngle);
                       // Aligner_StepArray.Add(AlignerStep.Set_PLC_Up);
                        Aligner_StepArray.Add(AlignerStep.GetStatus);
                    }
                    break;

                case SocketCommand.GetAlignerDegree:
                    {
                        Aligner_StepArray.Add(AlignerStep.GetAlignerDegree);
                    }
                    break;

                case SocketCommand.SetAlignerDegree:
                    {
                        Aligner_StepArray.Add(AlignerStep.SetAlignerDegree);
                    }
                    break;

                case SocketCommand.GetIDReaderDegree:
                    {
                        Aligner_StepArray.Add(AlignerStep.GetIDReaderDegree);
                    }
                    break;

                case SocketCommand.SetIDReaderDegree:
                    {
                        Aligner_StepArray.Add(AlignerStep.SetIDReaderDegree);
                    }
                    break;

                case SocketCommand.SetAlignerWaferType:
                    {
                        Aligner_StepArray.Add(AlignerStep.SetAlignerWaferType);
                    }
                    break;

                case SocketCommand.GetAlignerWaferType:
                    {
                        Aligner_StepArray.Add(AlignerStep.GetAlignerWaferType);
                    }
                    break;

                case SocketCommand.LiftPinDown:
                    {
                       // Aligner_StepArray.Add(AlignerStep.Set_PLC_Down);
                    }
                    break;

                case SocketCommand.LiftPinUp:
                    {
                     //   Aligner_StepArray.Add(AlignerStep.Set_PLC_Up);
                    }
                    break;
                case SocketCommand.AlignerVacuum_on:
                    {// AlignerVacuumOn
                        Aligner_StepArray.Add(AlignerStep.AlignerVacuumOn);
                    }
                    break;
                case SocketCommand.AlignerVacuum_off:
                    {// AlignerVacuumOn
                        Aligner_StepArray.Add(AlignerStep.AlignerVacuumOff);
                    }
                    break;
            }
            Aligner_StepArray.Add(AlignerStep.MaxCnt);
            AutoStepQueue.EnQueue(Aligner_StepArray.ToArray<AlignerStep>());
        }

        #endregion   
    }
}
