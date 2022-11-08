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
using System.IO;

namespace HirataMainControl
{
    class TCP_Server
    {
        private char[] Splitter_Start, Splitter_End;

        public delegate void RecEvent(string RecString);
        public event RecEvent SocketRec;

        public delegate void ConnectEvent(bool ConStatus);
        public event ConnectEvent ConStatusRec;

        private TcpClient Client = null;
        private object Client_lock = new object();

        private TcpListener Listen = null;
        private NetworkStream NetWork;
        private BinaryWriter Sender;

        public bool ConnectStatus = false;
        public BlockQueue<byte[]> SendMessage = new BlockQueue<byte[]>();

        private BackgroundWorker ListenBG = new BackgroundWorker();
        private BackgroundWorker ClientBG = new BackgroundWorker();
        private BackgroundWorker SendBG = new BackgroundWorker();

        public void Initial(string IP, int Port)
        {
            Splitter_Start = Encoding.ASCII.GetChars(new byte[] { NormalStatic.StartByte_STX });
            Splitter_End = Encoding.ASCII.GetChars(new byte[] { NormalStatic.EndByte_ETX });

            if (Listen != null)
            {
                Listen = null;
            }
            Flag_Connect = false;
            Listen = new TcpListener(IPAddress.Parse(IP), Port);
            Listen.Start();
            ListenBG.DoWork += new DoWorkEventHandler(Listen_DoWork);
            ListenBG.RunWorkerAsync();
        }

        private void Listen_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Thread.Sleep(100);

                if (!Listen.Pending())
                {
                    continue;
                }
                else
                {
                    if (Flag_Connect == false)
                    {
                        Client = this.Listen.AcceptTcpClient();
                        NetWork = Client.GetStream();
                        Sender = new BinaryWriter(NetWork);
                        Flag_Connect = true;

                        Client.ReceiveTimeout = 0;

                        if (ClientBG.IsBusy == false)
                        {
                            ClientBG.DoWork += new DoWorkEventHandler(Client_DoWork);
                            ClientBG.RunWorkerAsync();
                        }

                        if (SendBG.IsBusy == false)
                        {
                            SendBG.DoWork += new DoWorkEventHandler(Send_DoWork);
                            SendBG.RunWorkerAsync();
                        }
                    }
                    else
                    {
                        CloseConnect();
                    }
                }
            }
        }

        private void Client_DoWork(object sender, DoWorkEventArgs e)
        {
            int LengthOfBytes;
            string TempString = "";
            string Received = "";
            string[] Result_Start, Result_End;
            byte[] ReadBuffer = new byte[4096];

            while (Flag_Connect)
            {
                try
                {
                    Thread.Sleep(10);

                    if (NetWork.CanRead)// && NetWork.DataAvailable)
                    {
                        LengthOfBytes = NetWork.Read(ReadBuffer, 0, ReadBuffer.Length);
                        if (LengthOfBytes > 0)
                        {
                            lock (Client_lock)
                            {
                                byte[] ReceivedArray = new byte[LengthOfBytes];
                                Array.Copy(ReadBuffer, ReceivedArray, LengthOfBytes);

                                if (TempString == "")
                                    Received = Encoding.ASCII.GetString(ReceivedArray);
                                else
                                {
                                    Received = TempString + Encoding.ASCII.GetString(ReceivedArray);
                                    TempString = "";
                                }

                                SocketRec(Received);
                                //MessageBox.Show(Received);
                            }
                        }
                        else
                        {
                            Flag_Connect = false;
                        }
                        NetWork.Flush();
                    }
                }
                catch (Exception ex)
                {
                    Flag_Connect = false;
                    //UI.Error(NormalStatic.Socket, ErrorList.AP_TryCatchError, string.Format("{0},{1}", "ClientBG", ex.ToString()));
                }
            }
        }

        private void Send_DoWork(object sender, DoWorkEventArgs e)
        {
            byte[] QueuedData;

            while (Flag_Connect)
            {
                lock (Client_lock)
                {
                    try
                    {
                        QueuedData = SendMessage.DeQueue(10);
                        if (QueuedData != null)
                        {
                            Sender.Write(QueuedData, 0, QueuedData.Length);
                            Sender.Flush();
                        }
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
            }
        }
        private void CloseConnect()
        {
            Flag_Connect = false;
            try
            {
                if (NetWork != null)
                {
                    NetWork.Close();
                }

                if (Client != null)
                {
                    Client.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private bool Flag_Connect
        {
            set 
            {
                ConnectStatus = value;
                ConStatusRec(ConnectStatus);
            }
            get { return ConnectStatus; }
        }

    }
}
