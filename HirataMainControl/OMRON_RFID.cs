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
    public partial class OMRON_RFID : UserControl
    {
        #region Delegate

        public delegate void RFIDEvent(string deviceName, SocketCommand command, bool Result);
        public event RFIDEvent ActionComplete;

        #endregion

        #region BG / Queue


        private List<RFIDStep> RFID_StepArray = new List<RFIDStep>();
        private BlockQueue<RFIDStep[]> CommandQueue = new BlockQueue<RFIDStep[]>();
        private BlockQueue<string> ReceiverQueue = new BlockQueue<string>();

        #endregion       
        
        #region Variable

        public string DeviceName;
        public ErrorList NowErrorList = ErrorList.MaxCnt;
        public string NowErrorMsg = "";
        public string EFEM_Parameter = "";
        public string Now_Page ="";
        private string RS232_Test_string = "12345678";

        //UI
        private string Page_Parameter;
        private bool Connect;
        private bool Busy;
        private string FoupID;

        //RS-232 
        private SerialPort RFID_COM;
        private int ReceiveNowStepCnt = NormalStatic.Idle;
        private List<byte> ReceiveTemp = new List<byte>();
        private int NowStepCnt = 0;
        private SocketCommand MainCmd = SocketCommand.MaxCnt;
        private RFIDStep[] MarcoCommand;

        //Command
        private string CMD_string = "";
        private string REC_string = "";
        private int TimeoutCount = 10000; //10S

        #endregion         
        
        #region Public
        public OMRON_RFID()
        {
            InitializeComponent();
        }

        public void Initial(int number) 
        {
            DeviceName = gbxRFID.Text = string.Format("{0}{1}", NormalStatic.RFID, number + 1 );

            #region Status
            
            Ui_FoupID = "";
            Ui_Connect = false;
            Ui_Busy = false;

            #endregion

            #region  RS232 Setting

            int IniCOM = Convert.ToInt32(AppSetting.LoadSetting(string.Format("{0}{1}", NormalStatic.RFID, "_COM"), "20"));
            RFID_COM = new SerialPort(string.Format("COM{0}", IniCOM + number));
            RFID_COM.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.COM_DataReceived);
            RFID_COM.BaudRate = 9600;
            RFID_COM.DataBits = 8;
            RFID_COM.Parity = Parity.Even;
            RFID_COM.StopBits = StopBits.One;
          //  COM_Connect();

            #endregion

            #region Page

            update_PageParameter(AppSetting.LoadSetting(string.Format("{0}{1}", DeviceName, "_Page"), "1,3"));

            #endregion

        }
        #endregion

        #region Get/Set

        #region Connect

        public bool Ui_Connect
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate() { Ui_Connect = value; }));
                    return;
                }
                labConnect.BackColor = value ? Color.LightGreen : Color.Red;
                labConnect.Text = value ? "Con-C" : "Dis-C";
                Connect = value;
            }
            get { return Connect; }
        }

        #endregion

        #region Busy

        public bool Ui_Busy
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate() { Ui_Busy = value; }));
                    return;
                }
                labBusy.BackColor = value ? Color.Yellow : Color.LightGreen;
                labBusy.Text = value ? "Busy" : "Idle";
                Busy = value;
            }
            get { return Busy; }
        }

        #endregion

        #region ID

        public string Ui_FoupID
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate() { Ui_FoupID = value; }));
                    return;
                }
                labFoupID.Text = value;
                FoupID = value;
            }
            get { return FoupID; }
        }

        #endregion

        #endregion

        #region BG

     
        private async void Command_DoWork()
        {
            var ok = await Task.Run(() =>
            {
                string REC_Code = "";
                string REC_Data = "";

                while (true)
                {
                    MarcoCommand = CommandQueue.DeQueue();


                    while (NowStepCnt < MarcoCommand.Length)
                    {
                        #region Command Handle

                        if (NowStepCnt < MarcoCommand.Length)
                        {
                            #region End

                            if (MarcoCommand[NowStepCnt] == RFIDStep.MaxCnt)
                            {
                                ActionComplete(DeviceName, MainCmd, true);
                                Ui_Busy = false;
                                return true;
                            }

                            #endregion

                            #region Step

                            switch (MarcoCommand[NowStepCnt])
                            {
                                case RFIDStep.Initial:
                                    {
                                        CMD_string = string.Format("10{0}", RS232_Test_string);
                                    }
                                    break;

                                case RFIDStep.ReadData:
                                    {
                                        Ui_FoupID = "";
                                        CMD_string = string.Format("0100{0}", Page_Parameter);
                                    }
                                    break;

                                case RFIDStep.SetPageMap:
                                    {
                                        update_PageParameter(EFEM_Parameter);
                                        CMD_string = string.Format("10{0}", RS232_Test_string);

                                    }
                                    break;
                            }

                            #endregion

                            #region Sending & Recieve

                            if (CMD_string != "")
                            {
                                REC_Data = "";
                                REC_Code = "";
                                REC_string = "";
                                ReceiverQueue.Clear();
                                ReceiveNowStepCnt = NormalStatic.WaitReply;
                                DataSend(CMD_string);

                                REC_string = ReceiverQueue.DeQueue(TimeoutCount);

                                UI.Log(NormalStatic.RFID, DeviceName, SystemList.DeviceReceive, REC_string);

                                if (REC_string != null)
                                {
                                    REC_Code = REC_string.Substring(0, 2);
                                    REC_Data = REC_string.Substring(2, REC_string.Length - 2);

                                    if (REC_Code != Socket_Static.ReplyNormal_00)
                                    {
                                        #region Error

                                        if (REC_Code == "72")
                                        {
                                            Ui_FoupID = "r";
                                        }
                                        else
                                        {
                                            NowErrorList = (ErrorList)HCT_EFEM.EFEM_HasTable.RFID_Error[REC_Code];
                                            Jobfail();
                                        }

                                        #endregion
                                    }
                                    else
                                    {
                                        #region Normal

                                        switch (MarcoCommand[NowStepCnt])
                                        {
                                            case RFIDStep.Initial:
                                            case RFIDStep.SetPageMap:
                                                {
                                                    #region RS-232 Test Fail

                                                    if (REC_Data != RS232_Test_string)
                                                    {
                                                        NowErrorList = ErrorList.RF_SocketError_0671;
                                                        NowErrorMsg = " Test string fail.";
                                                        Jobfail();
                                                    }

                                                    #endregion
                                                }
                                                break;
                                            case RFIDStep.ReadData:
                                                {
                                                    Ui_FoupID = ASCII_toSting(REC_Data);
                                                }
                                                break;
                                        }

                                        #endregion
                                    }
                                    NowStepCnt++;
                                }
                                else
                                {
                                    #region Timeout

                                    if (NowErrorList == ErrorList.MaxCnt)
                                        NowErrorList = ErrorList.RF_ConnectTimeout_0672;
                                    Jobfail();

                                    #endregion
                                }
                            }

                            #endregion
                        }

                        #endregion
                    }
                    return false;
                }
            });
        }


        private void Jobfail() 
        {
            NowStepCnt += 100;
            Ui_Busy = false;
            ActionComplete(DeviceName, MainCmd, false);
        }

        public void Close()
        {
            NowStepCnt = 0;

            COM_Disconnect();
            UI.CloseBG(DeviceName);
        }

        private void update_PageParameter( string str ) 
        {          
            BitArray bitArr = new BitArray(32);
            byte[] byteArr = new byte[4];
            bool[] Page_Map = new bool[17];
            string[] Page_Index = str.Split(new string[] { "," }, StringSplitOptions.None);

            for (int i = 0; i < Page_Index.Length; i++)
            {
                Page_Map[Int32.Parse(Page_Index[i]) -1 ] = true;
                bitArr.Set(Int32.Parse(Page_Index[i]) + 1, true);
            } 
            bitArr.CopyTo(byteArr, 0);
            Page_Parameter = string.Format("00{0}{1}{2}", byteArr[2].ToString("X2"), byteArr[1].ToString("X2"),byteArr[0].ToString("X2"));
            Now_Page = str;
        }

        private string ASCII_toSting(string str) 
        {
            byte[] ByteArray = new byte[ str.Length / 2 ];
            for (int index = 0; index < ByteArray.Length; index++)             
                ByteArray[index] = byte.Parse(str.Substring( index * 2, 2 ), System.Globalization.NumberStyles.HexNumber);           
            return Encoding.ASCII.GetString(ByteArray); ;
        }

        #endregion

        #region RS232

        public void COM_Connect()
        {
            RFID_COM.Close();
            try
            {
                Ui_Connect = true;
                RFID_COM.Open();
            }
            catch
            {
                Ui_Connect = false;
                UI.InitialSystem(DeviceName, NormalStatic.False, ErrorList.RF_PortOpenFail_0670);
            }
        }

        public void COM_Disconnect()
        {
            Ui_Connect = false;
            RFID_COM.Close();
        }

        private void COM_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] raw_byte = new byte[1];
            SerialPort sp = (SerialPort)sender;
            //Thread.Sleep(100);
            while (sp.BytesToRead > 0)
            {
                sp.Read(raw_byte, 0, 1);
                //Array.Resize(ref raw_byte, length);
                DataCombine(raw_byte);
            }
        }

        private void DataSend(string Command_String)
        {
            string Send_String = Command_String;
            List<byte> Command_Byte = new List<byte>(Encoding.ASCII.GetBytes(Send_String));
            Command_Byte.Add(NormalStatic.EndByte_CR);
            RFID_COM.Write(Command_Byte.ToArray(), 0, Command_Byte.Count);
            UI.Log(NormalStatic.RFID, DeviceName, SystemList.DeviceSend, Command_String);
        }

        private void DataCombine(byte[] raw_data)
        {
         //  for (int i = 0; i < raw_data.Length; i++)
            {
                switch (ReceiveNowStepCnt)
                {
                    case NormalStatic.Idle:
                        {
                            UI.Error(DeviceName, ErrorList.RF_SocketError_0671, string.Format(" 0x{0}", Convert.ToString(raw_data[0], 16).ToUpper()));
                        }
                        break;

                    case NormalStatic.WaitReply:
                        {
                            ReceiveTemp.Clear();
                            ReceiveNowStepCnt = NormalStatic.Receiving;
                            ReceiveTemp.Add(raw_data[0]);
                        }
                        break;

                    case NormalStatic.Receiving:
                        {

                            if (raw_data[0] != NormalStatic.EndByte_CR)
                                ReceiveTemp.Add(raw_data[0]);
                            else  //End of the Receiving and check Sum
                            {
                                string CompleteString = Encoding.ASCII.GetString(ReceiveTemp.GetRange(0, ReceiveTemp.Count).ToArray());
                                ReceiveNowStepCnt = NormalStatic.Idle;
                                ReceiverQueue.EnQueue(CompleteString);
                            }
                        }
                        break;
                }
            }
        }

        #endregion

        #region Command_EnQueue

        public void Cmd_EnQueue(SocketCommand cmd)
        {
            UI.Log(NormalStatic.RFID, DeviceName, SystemList.CommandStart, string.Format("{0}:({1})", cmd, ""));

            Ui_Busy = true;
            NowStepCnt = 0;
            MainCmd = cmd;
            RFID_StepArray.Clear();
            CommandQueue.Clear();
            ReceiverQueue.Clear();
            ReceiveNowStepCnt = NormalStatic.Idle;
            NowErrorList = ErrorList.MaxCnt;
            NowErrorMsg = "";

            switch (cmd)
            {

                case SocketCommand.Initial:
                    {
                        RFID_StepArray.Add(RFIDStep.Initial); // OMRON RFID
                    }
                    break;

                case SocketCommand.ReadFoupID:
                    {
                        RFID_StepArray.Add(RFIDStep.ReadData);
                    }
                    break;

                case SocketCommand.SetPageMap:
                    {
                        RFID_StepArray.Add(RFIDStep.SetPageMap);
                    }
                    break;
            }

            RFID_StepArray.Add(RFIDStep.MaxCnt);
            CommandQueue.EnQueue(RFID_StepArray.ToArray<RFIDStep>());
            Command_DoWork();
        }

        #endregion

        #region Tip Info

        private void labFoupID_MouseEnter(object sender, EventArgs e)
        {
            tipInfo.SetToolTip((Label)sender, string.Format("Page : {0}", Now_Page));            
        }

        #endregion

    }
}
