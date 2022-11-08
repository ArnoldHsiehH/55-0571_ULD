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

namespace HirataMainControl
{
    public partial class Sick_IDM120 : UserControl
    {
        #region Event_Delegate

        public delegate void SickEvent(string ReplyMessage);
        public event SickEvent ScannerComplete;

        #endregion

        #region COM

        private SerialPort ScannerCOM;

        #endregion

        #region List

        private List<byte> ReceiveTemp = new List<byte>();

        #endregion

        #region Variable

        public string NowErrorMsg = "";
        public ErrorList NowErrorList = ErrorList.MaxCnt;
        private string DeviceName;
        private string ScannerName = NormalStatic.Scanner;
        private int ReceiveNowStepCnt = NormalStatic.Idle;
        private bool ConncetStatus;

        #endregion

        #region Initial

        public Sick_IDM120()
        {
            InitializeComponent();
        }

        public void Initial(int port)
        {
            DeviceName = string.Format("{0}{1}", NormalStatic.Scanner, port + 1);

            #region RS232_Initial

            ScannerName = string.Format("{0}{1}", DeviceName, NormalStatic.UnderLine);
            string strTemp = AppSetting.LoadSetting(string.Format("{0}{1}", ScannerName, "COM"), "COM5,9600,8,None,One");
            string[] SplitTemp = strTemp.Split(',');
            ScannerCOM = new SerialPort(SplitTemp[0]);
            ScannerCOM.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.COM_DataReceived);
            ScannerCOM.BaudRate = Convert.ToInt32(SplitTemp[1]);
            ScannerCOM.DataBits = Convert.ToInt32(SplitTemp[2]);
            ScannerCOM.Parity = (Parity)Enum.Parse(typeof(Parity), SplitTemp[3], true);
            ScannerCOM.StopBits = (StopBits)Enum.Parse(typeof(StopBits), SplitTemp[4], true);

            #endregion

            COM_Connect();
        } 

        #endregion

        #region RS232

        public void Close()
        {
            COM_Disconnect();
            UI.CloseBG(DeviceName);
        }

        private void COM_Connect()
        {
            ScannerCOM.Close();

            try
            {
                ScannerCOM.Open();
                Ui_Connect = true;
            }
            catch (Exception ex)
            {
                Ui_Connect = false;
                UI.InitialSystem(DeviceName, NormalStatic.False, ErrorList.AP_SerialError_0381);
            }
        }

        private void COM_Disconnect()
        {
            Ui_Connect = false;
            ScannerCOM.Close();
        }

        private void COM_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] raw_byte = new byte[1];
            SerialPort sp = (SerialPort)sender;

            while (sp.BytesToRead > 0)
            {
                sp.Read(raw_byte, 0, 1);
                // Int32 length = sp.Read(raw_byte, 0, raw_byte.Length);
                // Array.Resize(ref raw_byte, length);
                DataCombine(raw_byte);
            }
        }

        private void DataCombine(byte[] raw_data)
        {

            switch (ReceiveNowStepCnt)
            {
                case NormalStatic.Idle:
                    {
                        UI.Error(DeviceName, ErrorList.LP_SerialError_0909, string.Format("0x{0}", Convert.ToString(raw_data[0], 16).ToUpper()));

                    }
                    break;

                case NormalStatic.WaitReply://1
                    {
                        if (raw_data[0] != NormalStatic.StartByte_STX)
                        {
                            NowErrorMsg += string.Format("0x{0}", Convert.ToString(raw_data[0], 16).ToUpper());
                            NowErrorList = ErrorList.LP_SerialError_0909;
                        }
                        ReceiveTemp.Clear();
                        ReceiveNowStepCnt = NormalStatic.Receiving;
                    }
                    break;

                case NormalStatic.Receiving://2
                    {
                        ReceiveTemp.Add(raw_data[0]);
                        if (raw_data[0] == NormalStatic.EndByte_ETX)
                            ReceiveNowStepCnt = NormalStatic.WaitCheckSum;
                    }
                    break;

                case NormalStatic.WaitCheckSum://3
                    {
                        ReceiveNowStepCnt = NormalStatic.Idle;

                        if (raw_data[0] == NormalStatic.EndByte_CR)
                        {
                            string CompleteString = Encoding.ASCII.GetString(ReceiveTemp.GetRange(0, ReceiveTemp.Count - 1).ToArray());
                            ScannerComplete(CompleteString);
                            NowErrorMsg = "";
                            NowErrorList = ErrorList.MaxCnt;
                        }
                        else
                        {
                           // NowErrorMsg = string.Format("0x{0}-0x{1}", Convert.ToString(raw_data[0], 16).ToUpper(), Convert.ToString(Check, 16).ToUpper());
                            NowErrorList = ErrorList.LP_checksunError_0101;
                        }

                    }
                    break;

                default:
                    break;
            }
        }
    

        #endregion

        #region Get/Set

        public bool Ui_Connect
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate() { Ui_Connect = value; }));
                    return;
                }
                labConnect.BackColor = (value ? Color.LightGreen : Color.Red);
                labConnect.Text = (value ? "Con-C" : "Dis-C");
                ConncetStatus = value;
            }
            get { return ConncetStatus; }
        } 

        #endregion
    }
}
