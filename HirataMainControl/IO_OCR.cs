using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.IO;
using System.Net.NetworkInformation;
using System.Diagnostics;

namespace HirataMainControl
{
    public partial class IO_OCR : UserControl
    {
        #region Event/delegate

        public delegate void OCREvnet(string PortName, SocketCommand command, bool Result);
        public event OCREvnet ActionComplete;

        #endregion

        #region BG/Queue

        private Thread SocketBG;
        private BlockQueue<string> CommandQueue = new BlockQueue<string>();
        private BlockQueue<string> ReceiverQueue = new BlockQueue<string>();

        #endregion

        #region TCP

        private TcpClient SocketOCR;// = new TcpClient();
        private NetworkStream Stream_OCR;
        private StreamReader Reader_OCR;
        private StreamWriter Writer_OCR;
        private bool IsWait = false;

        #endregion

        #region Variable

        public CommandResult Result = CommandResult.Unknown;
        private string OCRCode = "";
        public string NowErrorMsg = "";
        public ErrorList NowErrorList = ErrorList.MaxCnt;
        public string paraterm = "";

        #region UI

        public string DeviceName = "";
        public string Type;
        private bool Busy = false;
        private string ID = "";
        private bool ConnectStatus = false;

        #endregion

        #region Command

        private bool SocketCloseFlag = false;
        private SocketCommand MainCmd = SocketCommand.MaxCnt;
        private string CMD_string = "";
        private string REC_string = "";
        private int TimeoutCount = 30000; //30S 

        #endregion

        #region Socket
 
        private string IP;
        private int Port;     
        private int PingTimeout = 2000; // 2s 

        #endregion

        #endregion        

        #region Initial

        public IO_OCR()
        {
            InitializeComponent();
        }

        public void Initial(int index)
        {
            gbxOCR.Text = DeviceName = string.Format("{0}{1}", NormalStatic.OCRReader, index + 1);
            Ui_Busy = false;

            string[] strTxt;
            strTxt = AppSetting.LoadSetting(string.Format("{0}{1}", DeviceName, "_IP"), "192.168.0.10,365,Cognex").Split(new string[] { "," }, 16, StringSplitOptions.None);
            IP = strTxt[0];
            Port = int.Parse(strTxt[1]);
            Type = strTxt[2];

            if (ConnectOpen())
            {
                UI.InitialSystem(DeviceName, NormalStatic.True, ErrorList.MaxCnt);
            }
            else
            {
                UI.InitialSystem(DeviceName, NormalStatic.False, ErrorList.AP_SocketError_0382);
            }

            SocketBG = new Thread(SocketBG_DoWork);
            SocketBG.IsBackground = true;
            SocketBG.Start();
        
        }

        #endregion

        #region BG

        private async void Command_DoWork()
        {
            var ok = await Task.Run(() =>
            {
                CMD_string = CommandQueue.DeQueue();
                REC_string = "";

                if (CMD_string != null)
                {
                    ReceiverQueue.Clear();
                    UI.Log(NormalStatic.OCRReader, DeviceName, SystemList.DeviceSend, CMD_string);
                    Send(CMD_string);
                }

                REC_string = ReceiverQueue.DeQueue(TimeoutCount);

                if (REC_string != ErrorList.MaxCnt.ToString())
                {
                    Ui_ID = "Read_Fail";
                    UI.Log(NormalStatic.OCRReader, DeviceName, SystemList.DeviceReceive, REC_string);
                    ResetOCRStatus();
                    ActionComplete(DeviceName, MainCmd, false);
                    Ui_Busy = false;
                    return false;
                }
                else
                {
                    UI.Log(NormalStatic.OCRReader, DeviceName, SystemList.DeviceReceive, OCRCode);
                    Ui_ID = OCRCode;
                    paraterm = "";
                    ActionComplete(DeviceName, MainCmd, true);
                    Ui_Busy = false;
                    return true;
                }
            });
        }

        private void ResetOCRStatus()
        {
            CommandQueue.Clear();
            ReceiverQueue.Clear();
            paraterm = ""; 

        }

        public void Close()
        {
            ResetOCRStatus();
            Ui_Connect = false;
            SocketClose();
            UI.CloseBG(DeviceName);
        }

        #endregion

        #region Comnmand

        public void Cmd_EnQueue(SocketCommand command)
        {
            UI.Log(NormalStatic.OCRReader, DeviceName, SystemList.CommandStart, command.ToString());
            UI.Log(NormalStatic.System, DeviceName, SystemList.CommandStart, command.ToString());

            Ui_Busy = true;
            MainCmd = command;
            CommandQueue.Clear();
            ReceiverQueue.Clear();
            NowErrorList = ErrorList.MaxCnt;
            NowErrorMsg = "";
            Result = CommandResult.Unknown;

            switch (command)
            {
                case SocketCommand.ResetError:
                    {
                        Ui_ID = "";
                        paraterm = "";
                        ActionComplete(DeviceName, MainCmd, true);
                        Ui_Busy = false;
                    }
                    break;

                case SocketCommand.Connect:
                    {
                        Ui_ID = "";
                        paraterm = "";

                        if (ConnectStatus == false)
                        {
                            NowErrorList = ErrorList.AP_SocketError_0382;
                            UI.Log(NormalStatic.OCRReader, DeviceName, SystemList.DeviceSend, ErrorList.AP_SocketError_0382.ToString() + "Command"); //Vincent 20201231
                        }

                        ActionComplete(DeviceName, MainCmd, ConnectStatus);
                        Ui_Busy = false;
                    }
                    break;

                case SocketCommand.Read:   
                    {
                        Ui_ID = "";
                        Qry_Read();
                        Command_DoWork();
                    }
                    break;
            }
        }

        #endregion

        #region Signal_Command

        private void Qry_Read()
        {
            if(Type == NormalStatic.Cognex && paraterm != "")
                CommandQueue.EnQueue(string.Format("READ(0)", paraterm));
            else
                CommandQueue.EnQueue("READ");
        }

        #endregion

        #region Get/Set

        public bool Ui_Busy
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate() { Ui_Busy = value; }));
                    return;
                }
                labOcrID.BackColor = value ? Color.Yellow : SystemColors.Control;
                Busy = value;
            }
            get { return Busy; }
        }

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
                ConnectStatus = value;
            }
            get { return ConnectStatus; }
        }

        public string Ui_ID
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate() { Ui_ID = value; }));
                    return;
                }
                labOcrID.Text = value;
                ID = value;
            }
            get { return ID; }
        }

        #endregion

        #region Socket

        #region BG

        private void SocketBG_DoWork()
        {
            while (true)
            {
                Thread.Sleep(2000);
                bool closed = false;
                byte[] testByte = new byte[1];

                if (SocketCloseFlag)
                {
                    SocketCloseConnect();
                    break;
                }

                if (SocketOCR.Client != null)
                {
                    if (SocketOCR.Connected == false)
                    {
                        closed = true;
                    }
                    else if (SocketOCR.Connected && !Ping_IOSS())
                    {
                        closed = true;
                    }
                    //else if (SocketOCR.Connected && SocketOCR.Client.Poll(0, SelectMode.SelectRead))
                    //{
                    //    try
                    //    {
                    //        closed = SocketOCR.Client.Receive(testByte, SocketFlags.Peek) == 0;
                    //    }
                    //    catch
                    //    {
                    //        closed = true;
                    //    }
                    //}
                }
                else
                {
                    closed = true;
                }

                if (closed)
                {
                    SocketCloseConnect();
                    SocketOpenConnect();
                }

                CheckConnectStatus();
            }
        }
 
        #endregion

        #region Connect

        private bool SocketOpenConnect()
        {
            if (!ConnectStatus)
            {
                if (IsWait) return true;
                IsWait = true;
                ConnectStatus = false;
                //SocketOCR = null;
                SocketOCR = new TcpClient();

                IAsyncResult MyResult = SocketOCR.BeginConnect(IP, Port, null, SocketOCR);

                MyResult.AsyncWaitHandle.WaitOne(PingTimeout, true);

                if (!MyResult.IsCompleted)
                {
                    IsWait = false;
                    Ui_Connect = false;

                    if (Stream_OCR != null)
                    {
                        Stream_OCR.Close();
                    }
                    SocketOCR.Close();

                    return false;
                }
                else if (SocketOCR.Connected == true)
                {
                    //Connect
                    IsWait = false;

                    //Stream_OCR = SocketOCR.GetStream();
                    Stream_OCR = new NetworkStream(SocketOCR.Client);
                    Reader_OCR = new StreamReader(Stream_OCR);
                    Writer_OCR = new StreamWriter(Stream_OCR);
                    Reader_OCR.BaseStream.ReadTimeout = 15000;
                    Writer_OCR.BaseStream.WriteTimeout = 15000;
                    
                    Ui_Connect = true;

                    return true;
                }

            }
            return false;
        }

        private void SocketCloseConnect()
        {
            try
            {
                IsWait = false;

                if (Stream_OCR != null)
                {
                    Stream_OCR.Close();
                }

                if (SocketOCR != null)
                {
                    SocketOCR.Close();
                }
            }
            catch (Exception ex)
            {
                UI.Alarm(NormalStatic.OCRReader, ErrorList.AP_TryCatchError, string.Format("{0},{1}", "Close", ex.ToString()));
            }
        }

        private bool CheckConnectStatus()
        {
            bool TempConnect = false;

            if (SocketOCR.Client != null)
            {
                if (SocketOCR.Connected)
                {
                    TempConnect = true;
                }
                else
                {
                    TempConnect = false;
                }
            }

            if (ConnectStatus != TempConnect)
            {
                Ui_Connect = TempConnect;

                if (TempConnect == false)
                {
                    UI.Log(NormalStatic.OCRReader, DeviceName, SystemList.DeviceReceive, "Disconnect!");
                }

            }

            return ConnectStatus ? true : false;
        }

        #endregion

        #region Send

        private void Send(string _Text)
        {            
            string strOCRReaderFilePath = string.Format("{0}{1}", NormalStatic.OCRFailPath, System.DateTime.Now.ToString(NormalStatic.FileFormat));
            //_ReplyMessage = String.Empty;

            if (ConnectStatus)
            {
                #region Cognex

                if (SocketOCR.Connected)
                {
                    try
                    {
                        Writer_OCR.WriteLine(_Text);
                        Writer_OCR.Flush();
                        OCRCode = Reader_OCR.ReadLine();//ON123OR3CGA1,1.000"
                        Reader_OCR.DiscardBufferedData();
                        string[] strTxt2 = OCRCode.Split(new string[] { NormalStatic.Comma }, 16, StringSplitOptions.None);

                        if (strTxt2[0].Contains("*"))
                        {
                            //NowErrorList = ErrorList.OC_Alarm_0505;
                            //NowErrorMsg = strTxt2[0];
                            OCRCode = strTxt2[0];
                            NowErrorList = ErrorList.MaxCnt;
                        }
                        else
                        {
                            if (strTxt2.Length > 1)
                            {
                                switch (strTxt2[1])
                                {
                                    case "0.000":
                                        {
                                            NowErrorList = ErrorList.OC_CheckFail_1101;
                                            NowErrorMsg = strTxt2[0];
                                        }
                                        break;

                                    default:
                                        {
                                            OCRCode = strTxt2[0];
                                            NowErrorList = ErrorList.MaxCnt;
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                OCRCode = strTxt2[0];
                                NowErrorList = ErrorList.MaxCnt;
                            }
                        }


                    }
                    catch
                    {
                        NowErrorList = ErrorList.Timeout_1010;
                    }
                    ReceiverQueue.EnQueue(NowErrorList.ToString());

                }
                #endregion Cognex

            }

        }

        #endregion
      
        #region Socke_OpenClose

        private bool ConnectOpen()
        {
            bool Return = false;
  
            SocketCloseConnect();
            Return = SocketOpenConnect();

            return Return;
        }

        private bool SocketOpen()
        {
            SocketCloseConnect();
            if (SocketOpenConnect())
                return true;
            else
                return false;
        }

        private void SocketClose()
        {
            SocketCloseFlag = true;
        }

        #endregion

        #region Ping_IOSS

        private bool Ping_IOSS()
        {
            Ping PingOCR = new Ping();
            PingReply Reply = PingOCR.Send(IP,1000);
            PingOCR.Dispose();
            if (Reply.Status != IPStatus.Success)
                return false;
            else
                return true;
        }

        #endregion

        #endregion
    }
}
