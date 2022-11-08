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
using System.Net.Sockets;
using System.Net;

namespace HirataMainControl
{
    public partial class SocketClient : UserControl
    {
        #region Delegate

        public delegate void RecSend(string sendText);
        public RecSend RecSendEvent;

        #endregion

        #region BG

        private BackgroundWorker DetectConnectBG = new BackgroundWorker();

        #endregion

        #region Variable

        private string DeviceName = "";
        private string SocketIP = "";
        private int SocketPort = 0;
        private int RecStart = 0x02;
        private int RecEnd = 0x03;
        public static bool ConnectStatus = false;//220803 Arnold
        public static BlockQueue<byte[]> SendMessage = new BlockQueue<byte[]>();
        private bool CloseFlag = false;
        private TcpClient TCPClient = new TcpClient();
        private NetworkStream NetWork;
        private bool IsWait = false;

        #endregion

        #region Initial

        public SocketClient()
        {
            InitializeComponent();
        }

        public void Initial(string ip, int port)
        {
            SocketIP = ip;
            SocketPort = port;
            //RecStart = start;
            //RecEnd = end;
            DetectConnectBG.DoWork += new DoWorkEventHandler(BG_DetectConnect_DoWork);
            DetectConnectBG.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BG_Completed);
            DetectConnectBG.RunWorkerAsync();
        }

        #endregion

        #region BG

        string ReceiveData = "";

        private void BG_DetectConnect_DoWork(object sender, DoWorkEventArgs e)
        {
            bool closed = false;
            byte[] testByte = new byte[1];

            while (true)
            {
                Thread.Sleep(200);

                if (CloseFlag)
                    break;

                if (TCPClient.Client != null)
                {
                    if (!TCPClient.Connected)
                    {
                        closed = true;
                    }
                    else if (TCPClient.Connected && TCPClient.Client.Poll(0, SelectMode.SelectRead))
                    {
                        try
                        {
                            closed = TCPClient.Client.Receive(testByte, SocketFlags.Peek) == 0;
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
                    CloseConnect();
                    OpenConnect();
                }

                CheckConnectStatus();
                ReceiveData += Received();

                if (ReceiveData != "")
                {
                    ReceiveData = ReceiveData.Trim('\0');
                    while (true)
                    {
                        //int stx_index = ReceiveData.IndexOf((Char)RecStart);
                        //int etx_index = ReceiveData.IndexOf((Char)RecEnd);
                        //if ((stx_index == -1) || (etx_index == -1))
                        //{
                        //    break;
                        //}

                        //string txt = ReceiveData.Substring(stx_index + 1, etx_index - stx_index - 1);
                        //ReceiveData = ReceiveData.Substring(etx_index + 1, ReceiveData.Length - etx_index - 1);
                        if (RecSendEvent != null)
                        {
                            RecSendEvent(ReceiveData);
                        }
                    }
                }
            }
        }

        private void BG_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            CloseConnect();
        }

        #endregion

        #region Send_Method

        public static void Send_NormalReceive(SocketCommand cmd, string obj, string msg)
        {

            try
            {
                string str;

                if (msg != "")
                {
                    str = String.Format("{0},{1},{2},{3}",
                                        Socket_Static.ReplyNormal_00,
                                        cmd,
                                        obj,
                                        msg);
                }
                else
                {
                    str = String.Format("{0},{1},{2}",
                                          Socket_Static.ReplyNormal_00,
                                          cmd,
                                          obj);
                }

                if (ConnectStatus == true)
                {
                    byte[] data = Encoding.UTF8.GetBytes(string.Format("{0}{1}{2}", "\x02", str, "\x03"));
                    SendMessage.EnQueue(data);
                }

                UI.Log(NormalStatic.System, NormalStatic.Socket, SystemList.SocketSend, str);

            }
            catch (Exception ex)
            {
                UI.Error(NormalStatic.Socket, ErrorList.AP_ExceptionOccur_0199, string.Format("{0},{1}", "Normal", ex.ToString()));
            }


        }

        public static void Send_AlarmReceive(SocketCommand cmd, string obj, ErrorList list, string msg)
        {
            try
            {
                string str;

                if (msg != "")
                {
                    str = String.Format("{0},{1},{2},{3},{4}{5}",
                        HCT_EFEM.ExcelLogMessage[(int)LogExcelTable.Error][0][(int)list, 0],
                        cmd,
                        obj,
                        HCT_EFEM.ExcelLogMessage[(int)LogExcelTable.Error][0][(int)list, 1],
                        HCT_EFEM.ExcelLogMessage[(int)LogExcelTable.Error][0][(int)list, 2],
                        msg);
                }
                else
                {
                    str = String.Format("{0},{1},{2},{3},{4}",
                        HCT_EFEM.ExcelLogMessage[(int)LogExcelTable.Error][0][(int)list, 0],
                        cmd,
                        obj,
                        HCT_EFEM.ExcelLogMessage[(int)LogExcelTable.Error][0][(int)list, 1],
                        HCT_EFEM.ExcelLogMessage[(int)LogExcelTable.Error][0][(int)list, 2]);
                }
                if (ConnectStatus == true || cmd == SocketCommand.Event)
                {
                    byte[] data = Encoding.UTF8.GetBytes(string.Format("{0}{1}{2}", "\x02", str, "\x03"));
                    SendMessage.EnQueue(data);
                }

                UI.Log(NormalStatic.System, NormalStatic.Socket, SystemList.SocketSend, str);

                switch (HCT_EFEM.ExcelLogMessage[(int)LogExcelTable.Error][0][(int)list, 0])
                {

                    case Socket_Static.ReplyDeviceError_01:
                    case Socket_Static.ReplyFormatError_02:
                    case Socket_Static.ReplyAlarmTable_03:
                    case Socket_Static.ReplyInterlock_04:
                    case Socket_Static.ReplyInAlarm_05:
                    case Socket_Static.ReplyInProcess_06:
                    case Socket_Static.ReplyModeError_07:
                    case Socket_Static.ReplyMappingError_08:
                    case Socket_Static.ReplySerialError_09:
                    case Socket_Static.ReplyTimeout_10:
                        {
                            UI.Alarm(obj, list, msg);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                UI.Error(NormalStatic.Socket, ErrorList.AP_ExceptionOccur_0199, string.Format("{0},{1}", "Alarm", ex.ToString()));
            }
        }

        #endregion

        #region Connect

        private bool OpenConnect()
        {
            if (!ConnectStatus)
            {
                if (IsWait) return true;
                IsWait = true;
                ConnectStatus = false;
                IPAddress SocketIPAddress = IPAddress.Parse(SocketIP);
                IPEndPoint SocketIPEndPoint = new IPEndPoint(SocketIPAddress, SocketPort);
                TCPClient = null;
                TCPClient = new TcpClient();

                IAsyncResult MyResult = TCPClient.BeginConnect(SocketIP, SocketPort, null, null);

                MyResult.AsyncWaitHandle.WaitOne(3000, true);

                if (!MyResult.IsCompleted)
                {
                    IsWait = false;
                    Ui_Connect = false;

                    if (NetWork != null)
                    {
                        NetWork.Close();
                    }
                    TCPClient.Close();

                    return false;
                }
                else if (TCPClient.Connected == true)
                {
                    //Connect      
                    IsWait = false;

                    NetWork = TCPClient.GetStream();

                    NetWork = new NetworkStream(TCPClient.Client);
                    Ui_Connect = true;

                    return true;
                }

            }
            return false;
        }

        private void CloseConnect()
        {
            try
            {
                IsWait = false;

                if (NetWork != null)
                {
                    NetWork.Close();
                }

                if (TCPClient != null)
                {
                    TCPClient.Close();
                }
            }
            catch (Exception ex)
            {
                UI.Alarm(NormalStatic.Socket, ErrorList.AP_TryCatchError, string.Format("{0},{1}", "close", ex.ToString()));
            }
        }

        public bool Open()
        {
            CloseConnect();
            if (OpenConnect())
                return true;
            else
                return false;
        }

        public void Close()
        {
            CloseFlag = true;
        }

        private bool CheckConnectStatus()
        {
            bool TempConnect = false;

            if (TCPClient.Client != null)
            {
                if (TCPClient.Connected)
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

        public bool Send(string _Text)
        {
            bool Return = false;
            //_ReplyMessage = String.Empty;

            if (ConnectStatus)
            {
                try
                {
                    Byte[] TextBytes = Encoding.ASCII.GetBytes(_Text);
                    NetWork.Write(TextBytes, 0, TextBytes.Length);
                    Return = true;
                }
                catch (Exception ex)
                {
                    UI.Alarm(NormalStatic.Socket, ErrorList.AP_TryCatchError, string.Format("{0},{1}", "send", ex.ToString()));
                    Return = false;
                }
            }

            return Return;
        }

        #endregion

        #region Receive

        public string Received()
        {
            String recvTxt = String.Empty;
            int bufferSize;
            byte[] ReceivedPackage = new byte[1024];
            try
            {
                if (ConnectStatus)
                {

                    if (TCPClient.Available > 0)
                    {
                        bufferSize = TCPClient.ReceiveBufferSize;
                        ReceivedPackage = new byte[bufferSize];
                        NetWork.Read(ReceivedPackage, 0, bufferSize);
                        recvTxt = System.Text.Encoding.UTF8.GetString(ReceivedPackage);
                    }
                }
            }
            catch (Exception ex)
            {
                UI.Alarm(NormalStatic.Socket, ErrorList.AP_TryCatchError, string.Format("{0},{1}", "Received", ex.ToString()));
            }
            return recvTxt;
        }

        #endregion

        #region Get/Set

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

                ConnectStatus = value;
            }
            get { return ConnectStatus; }
        }

        #endregion
    }
}
