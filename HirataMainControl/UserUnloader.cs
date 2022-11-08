using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace HirataMainControl
{
    public partial class UserUnloader : UserControl
    {
        //宣告
        private Thread td_TRISSECS;
        private static TcpClient tcp_CLient;

        private bool SECSConnection = false;
        private string TRIS_IP = "";
        private int TRIS_Port = -1;

        public UserUnloader()
        {
            InitializeComponent();

        }

        public void Initail() 
        {
            tcp_CLient = new TcpClient();
            td_TRISSECS = new Thread(() => Do_TCPIP());
            td_TRISSECS.IsBackground = true;
            td_TRISSECS.Start();
        }

        public void Listenser_Close()
        {
            if (tcp_CLient.Client.Connected)
            {
                tcp_CLient.Client.Disconnect(false);
            }
            tcp_CLient.Client.Close();
            tcp_CLient.Client.Dispose();
            tcp_CLient.Close();
            SpinWait.SpinUntil(() => false, 1000);
        }

        private string RemainingString = "";
        private void Do_TCPIP()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(1);

                    if (tcp_CLient.Client != null && tcp_CLient.Connected)
                    {
                        if (tcp_CLient.Client.Poll(5000, SelectMode.SelectRead))
                        {
                            byte[] buff = new byte[1];
                            if (tcp_CLient.Client.Receive(buff, SocketFlags.Peek) == 0)//斷線
                            {
                                Listenser_Close();
                                Wait_Client();
                                continue;
                            }
                            else
                            {
                                if (tcp_CLient.Available > 0)
                                {
                                    byte[] readByte = new byte[tcp_CLient.Available];
                                    tcp_CLient.Client.Receive(readByte, readByte.Length, SocketFlags.None);

                                    RemainingString += Encoding.ASCII.GetString(readByte);
                                    int CRIdx = RemainingString.IndexOf(CR);
                                    if (CRIdx > -1)
                                    {
                                        string Temp = RemainingString.Substring(0, CRIdx);
                                        if (RemainingString.Length > CRIdx + 1)
                                        {
                                            RemainingString = RemainingString.Substring(CRIdx + 1, RemainingString.Length - CRIdx - 1);
                                        }
                                        else 
                                        {
                                            RemainingString = "";
                                        }
                                        HCT_EFEM.bk_EventReceive.EnQueue(Temp);

                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Wait_Client();
                    }
                }
                catch (Exception ex)
                {
                    //Wait_Client();
                }
            }
        }

        private void Wait_Client()
        {
            try
            {
                tcp_CLient = new TcpClient();
                tcp_CLient.Connect("192.168.0.1", 55555);
            }
            catch (Exception ex)
            {

            }
        }

        private static object lk_Send = new object();
        private static string CR = "\u000D";

        public static void SendCommand(string ref_String)
        {
            lock (lk_Send)
            {
                try
                {
                    ref_String += CR;
                    tcp_CLient.Client.Send(Encoding.ASCII.GetBytes(ref_String));
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
