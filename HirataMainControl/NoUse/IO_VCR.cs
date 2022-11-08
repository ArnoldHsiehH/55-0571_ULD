using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading;
using System.Net;

namespace HirataMainControl
{
    public partial class IO_VCR : UserControl
    {
        #region Event/delegate

        public delegate void VCREvnet(string PortName, VCRCommand command, bool Result);
        public event VCREvnet ActionComplete;

        #endregion

        #region BG/Queue

        private BackgroundWorker SocketBG = new BackgroundWorker();

        private BackgroundWorker ControlBG = new BackgroundWorker();
        private BlockQueue<string> CommandQueue = new BlockQueue<string>();
        private BlockQueue<string> ReceiverQueue = new BlockQueue<string>(); 

        #endregion

        #region TCP

        private TcpClient SocketVGR = new TcpClient();
        private NetworkStream Stream_VCR;
        private bool IsWait = false;

        #endregion

        #region Variable

        #region UI

        public string DeviceName = "";
        private bool Busy = false;
        private string ID = "";
        private bool ConnectStatus = false;

        #endregion

        #region Command

        private bool SocketCloseFlag = false;
        private VCRCommand MainCmd = VCRCommand.MaxCnt;
        private string CMD_string = "";
        private string REC_string = "";
        private int TimeoutCount = 5000; //5S 

        #endregion

        #region Socket

        private string IP = "192.168.0.1";
        private int PingTimeout = 1000; // 3s 

        #endregion

        #endregion

        #region Initial

        public IO_VCR()
        {
            InitializeComponent();
        }

        public void Initial(string name)
        {
            IP = AppSetting.LoadSetting(string.Format("{0}{1}", name, "_IP"), "192.168.0.10");
            DeviceName = name;

            if(SocketOpen())
                UI.InitialSystem(DeviceName, NormalStatic.True, NormalStatic.Space);
            else
                UI.InitialSystem(DeviceName, NormalStatic.False, " Socket link Fail");

            PingTimeout = 2000;
            ControlBG.DoWork += new DoWorkEventHandler(ControlBG_DoWork);
            ControlBG.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ControlBG_Completed);
            ControlBG.RunWorkerAsync();

            SocketBG.DoWork += new DoWorkEventHandler(SocketBG_DoWork);
            SocketBG.RunWorkerCompleted += new RunWorkerCompletedEventHandler(SocketBG_Completed);
            SocketBG.RunWorkerAsync();
        }

        #endregion

        #region BG

        private void ControlBG_DoWork(object sender, DoWorkEventArgs e)
        {    
            while (true)
            {
                CMD_string = CommandQueue.DeQueue();
                REC_string = "";

                if (CMD_string == NormalStatic.End)
                    break;

                if (CMD_string != null)
                {
                    ReceiverQueue.Clear();
                    Send(string.Format("{0}{1}",CMD_string,"\r"));
                }

                REC_string = ReceiverQueue.DeQueue(TimeoutCount);

                if (REC_string == null)
                {
                    UI.Alarm(DeviceName, AlarmList.Timeout, MainCmd.ToString());
                    ResetVCRStatus();
                }
                else if (REC_string == NormalStatic.End)
                {
                    break;
                }
                else
                    ReceiveHandler();
            }
        }

        private void ReceiveHandler()
        {
            switch (REC_string)
            {
                case "":
                    {
                        Ui_ID = "Read_Fail";
                        ActionComplete(DeviceName, MainCmd, false);
                    }
                    break;

                default:
                    {
                        ActionComplete(DeviceName, MainCmd, true);
                        Ui_ID = REC_string;
                        Ui_Busy = false;
                    }
                    break;
            }
        }

        private void ControlBG_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            Ui_Connect = false;
            SocketClose();
            UI.CloseBG(DeviceName);
        }

        private void ResetVCRStatus()
        {
            CommandQueue.Clear();
            ReceiverQueue.Clear();
        }

        public void Close()
        {
            ResetVCRStatus();
            CommandQueue.EnQueue(NormalStatic.End);
            ReceiverQueue.EnQueue(NormalStatic.End);
        }

        #endregion

        #region Comnmand

        public void Cmd_EnQueue(VCRCommand command)
        {
            UI.System(DeviceName, SystemList.CommandStart, command.ToString());
            Ui_Busy = true;
            MainCmd = command;
            ReceiverQueue.Clear();

            switch (command)
            {
                case VCRCommand.ResetError:
                    {
                        Ui_Busy = false;
                        Ui_ID = "";
                        ActionComplete(DeviceName, MainCmd, true);
                    }
                    break;

                case VCRCommand.Read:
                    {
                        Ui_ID = "";
                        Qry_Read();
                    }
                    break;
            }
        } 

        #endregion

        #region Signal_Command

        private void Qry_Read()
        {
            CommandQueue.EnQueue("m");
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
                labVcrID.BackColor = value ? Color.Yellow : SystemColors.Control;
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
                labVcrID.Text = value;
                ID = value;
            }
            get { return ID; }
        }

        #endregion

        #region Socket

        #region BG

        private void SocketBG_DoWork(object sender, DoWorkEventArgs e)
        {
            string ReceiveData = "";

            while (true)
            {
                Thread.Sleep(200);
                bool closed = false;
                byte[] testByte = new byte[1];

                if (SocketCloseFlag)
                    break;

                if (SocketVGR.Client != null)
                {
                    if (!SocketVGR.Connected)
                    {
                        closed = true;
                    }
                    else if (SocketVGR.Connected && SocketVGR.Client.Poll(0, SelectMode.SelectRead))
                    {
                        try
                        {
                            closed = SocketVGR.Client.Receive(testByte, SocketFlags.Peek) == 0;
                        }
                        catch (SocketException ex)
                        {
                            closed = true;
                        }
                    }
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
                ReceiveData += Received();

                if (ReceiveData != "")
                {
                    ReceiveData = ReceiveData.Trim('\0');
                    while (true)
                    {
                        int stx_index = ReceiveData.IndexOf((char)NormalStatic.StartByte_STX);
                        int etx_index = ReceiveData.IndexOf((char)NormalStatic.EndByte_ETX);
                        if ((stx_index == -1) || (etx_index == -1))
                        {
                            ReceiveData = "";
                            break;
                        }
                        string txt = ReceiveData.Substring(stx_index + 1, etx_index - stx_index - 1);

                        ReceiveData = ReceiveData.Substring(etx_index + 1, ReceiveData.Length - etx_index - 1);

                        ReceiverQueue.EnQueue(txt);
                  
                    }
                }
            }
        }

        private void SocketBG_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            SocketCloseConnect();
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
                SocketVGR = null;
                SocketVGR = new TcpClient();

                IAsyncResult MyResult = SocketVGR.BeginConnect(IP, 9876, null, null);

                MyResult.AsyncWaitHandle.WaitOne(PingTimeout, true);

                if (!MyResult.IsCompleted)
                {
                    IsWait = false;
                    Ui_Connect = false;

                    if (Stream_VCR != null)
                    {
                        Stream_VCR.Close();
                    }
                    SocketVGR.Close();

                    return false;
                }
                else if (SocketVGR.Connected == true)
                {
                    //Connect      
                    IsWait = false;

                    Stream_VCR = SocketVGR.GetStream();

                    Stream_VCR = new NetworkStream(SocketVGR.Client);

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

                if (Stream_VCR != null)
                {
                    Stream_VCR.Close();
                }

                if (SocketVGR != null)
                {
                    SocketVGR.Close();
                }
            }
            catch (Exception ex)
            {
                UI.Warning(NormalStatic.Socket, WarningList.TryCatchError, ex.ToString());
            }
        }

        private bool CheckConnectStatus()
        {
            bool TempConnect = false;

            if (SocketVGR.Client != null)
            {
                if (SocketVGR.Connected)
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
                ConnectStatus = TempConnect;
            }

            return ConnectStatus ? true : false;
        }

        #endregion

        #region Send

        private bool Send(string _Text)
        {
            bool Return = false;
            //_ReplyMessage = String.Empty;

            if (ConnectStatus)
            {
                try
                {
                    Byte[] TextBytes = Encoding.ASCII.GetBytes(_Text);
                    Stream_VCR.Write(TextBytes, 0, TextBytes.Length);
                    Return = true;
                }
                catch (Exception ex)
                {
                    UI.Warning(NormalStatic.Socket, WarningList.TryCatchError, ex.ToString());
                    Return = false;
                }
            }

            return Return;
        }

        #endregion

        #region Receive

        private string Received()
        {
            String recvTxt = String.Empty;
            //int bufferSize;
            byte[] ReceivedPackage;
            try
            {
                if (ConnectStatus)
                {
                    if (SocketVGR.Available > 0)
                    {
                        //bufferSize = SocketVGR.ReceiveBufferSize;
                        ReceivedPackage = new byte[1024];
                        Stream_VCR.Read(ReceivedPackage, 0, 1024);
                        recvTxt = System.Text.Encoding.UTF8.GetString(ReceivedPackage);
                    }
                }
            }
            catch (Exception ex)
            {
                UI.Warning(NormalStatic.Socket, WarningList.TryCatchError, ex.ToString());
            }
            return recvTxt;
        }

        #endregion

        #region Control

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

        #endregion
    }
}
