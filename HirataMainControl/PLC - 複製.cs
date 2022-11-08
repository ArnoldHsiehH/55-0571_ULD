using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using System.Net.Sockets;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace HirataMainControl
{
    public partial class PLC : UserControl
    {
        SocketServer Sk_RT;
        BlockQueue<string> BK_RTRecv;
        BlockQueue<string> BK_RTSend;
        Thread TD_RTRecv;
        Thread TD_RTSend;

        private static SocketServer Sk_PLCCmd;
        BlockQueue<string> BK_CmdRecv;
        static BlockQueue<string> BK_CmdSend;
        Thread TD_CmdSend;

        BackgroundWorker BG_UIUpdate;

        Thread TD_Interface;
        Thread TD_CheckAlarm;
        Thread TD_AlarmReset;
        static BlockQueue<string[]> BK_AlarmReset;

        Thread TD_TracePLC;
        BackgroundWorker BG_Trace;
        BackgroundWorker BG_TraceRead;

        //Value temp
        int Range_B = 1000;
        int B_TotalTimes = 1;
        int B_NowTimes = 0;
        int B_Total ;
        int Range_W = 1000;
        int W_TotalTimes = 10;
        int W_NowTimes = 0;
        int Range_LR = 1000;
        int LR_Total;
        public static bool[] B;
        public static int[] W;
        public static bool[] LR;
        private bool Connection = false;
        public static bool ST_Conn = false;
        private bool Alarm_Flag = false;
        static bool ST_AlarmFlag=false;

        //InterfaceCheck
        bool InterRecv = false;
        bool InterfaceNextStep = false;
        string InterfaceRecvstr = "";
        public static bool Initialing = false;
        //AlarmReset
        public static bool AlarmReseting = false;
        static bool AlarmResetRecv = false;
        static bool AlarmResetNextStep = false;
        static string AlarmRecvstr = "";
        //Initial Interface
        static bool InitialRecv = false;
        static string InitialRecvstr = "";
        bool TracePLC_Start = false;
		private static int InitialStepNo;
        private static List<InitialStep> InitialStep_List = new List<HirataMainControl.InitialStep>();

        //ReadFinish
        public static bool PLCRead = false;

        bool SearchStart = false;
        int Range=-1;
        int Statr = -1;

        private PLC_RTDevice RT_Device = PLC_RTDevice.Fn;

        //CPLC_Cmd val
        private static All_Device PLC_CmdDev = All_Device.Fn;
        private static bool PLC_CmdNextStep = true;
        public delegate void Evt_Cmd_Recv(string recv_str,All_Device ref_Device);
        public event Evt_Cmd_Recv CmdRecv;

        private byte CR = 0x0D;

        Stopwatch sw = new Stopwatch();
        DataTable Dt_trace;
        StreamReader smr_TraceRead;

        public PLC()
        {
            InitializeComponent();
            tc_PLC = new TabControl();
            tc_PLC.SelectedIndexChanged += new EventHandler(tc_PLCIdxChange);
            tc_PLC.Dock = DockStyle.Fill;
            this.Controls.Add(tc_PLC);
        }

        private void PLC_Load(object sender, EventArgs e)
        {
        }
        public void init() 
        {          
            B_Total = Range_B * 16 * B_TotalTimes;
            LR_Total=Range_LR*16;
            stslb_PLCConn.Text = Sk_Connstr.Connect.ToString();
            B = new bool[B_Total];
            W = new int[Range_W * (W_TotalTimes+1)];
            LR = new bool[LR_Total];

            Sk_RT = new SocketServer();
            Sk_RT.PLC_RTRecv += new SocketServer.Evt_PLC_RTRecv(plc_RTRecv);
            Sk_RT.UI += new SocketServer.UI_Change(UI_Status);
            Sk_RT.Open("192.168.0.10", 8501, Sk_Device.PLC_RT);

            BK_RTRecv = new BlockQueue<string>();
            BK_RTSend = new BlockQueue<string>();

            TD_RTRecv = new Thread(Dowrok_RTRecv);
            TD_RTSend = new Thread(Dowork_RTSend);
            TD_RTRecv.Start();
            TD_RTSend.Start();

            Sk_PLCCmd = new SocketServer();
            Sk_PLCCmd.PLC_CmdRecv += new SocketServer.Evt_PLC_CmdRecv(Plc_CmdRecv);
            Sk_PLCCmd.UI += new SocketServer.UI_Change(UI_Status);
            Sk_PLCCmd.Open("192.168.0.10", 8501, Sk_Device.PLC_Cmd);

            BK_CmdRecv = new BlockQueue<string>();
            BK_CmdSend = new BlockQueue<string>();
            TD_CmdSend = new Thread(Dowork_CmdSend);
            TD_CmdSend.IsBackground = true;
            TD_CmdSend.Start();  
        }

        //PLC RT BG.BK Dowork
        public void plc_RTSend(string str)
        {
            BK_RTSend.EnQueue(str);
        }

        private void Dowork_RTSend()
        {
            while (true)
            {
                string[] str = BK_RTSend.DeQueue(System.Threading.Timeout.Infinite).Split(',');         

                if (Sk_RT.tcpclt.Connected && Sk_RT.tcpclt.GetStream().CanWrite)
                {
                    string temp = str[0];
                    byte[] tmpByte = System.Text.Encoding.UTF8.GetBytes(temp);
                    byte[] msgByte = new byte[tmpByte.Length + 1];
                    for (int i = 0; i < tmpByte.Length; i++)
                    {
                        msgByte[i] = tmpByte[i];
                    }
                    msgByte[tmpByte.Length] = CR;
                    Enum.TryParse(str[1], out RT_Device);
                    Sk_RT.tcpclt.Client.Send(msgByte);
                }
                else 
                {
                    //UI.Alarm(Sk_Device.PLC_RT.ToString(),ErrorList.ConnectionError, "PLC DisConnection");
                }
            }
        }
        private void plc_RTRecv(string str)
        {
            BK_RTRecv.EnQueue(str);
        }
        private void Dowrok_RTRecv() 
        {
            while (true) 
            {
                string recv_str = BK_RTRecv.DeQueue(System.Threading.Timeout.Infinite);
                if (recv_str == null)
                {
                    Console.WriteLine(1);
                }

                string[] splite = recv_str.Split(' ');
                switch (RT_Device) 
                {
                    case PLC_RTDevice.B1:

                        int B_Shift = B_NowTimes * 1000;

                        for (int i = 0; i < splite.Length; i++)
                        {
                            string HexToBit = Convert.ToString(Convert.ToInt32(splite[i].ToString()), 2).PadLeft(16, '0');
                           
                            for (int j = 16; j > 0; j--)
                            {
                                try
                                {
                                    B[(i * 16) + B_Shift + 16 - j] = HexToBit.Substring(j - 1, 1) == "1" ? true : false;
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show(e.ToString() + "PLC try catch error");
                                }
                            }
                        }
                        if (B_NowTimes == B_TotalTimes-1)
                        {
                            B_NowTimes = 0;
                            plc_RTSend(string.Format("RDS {0}0000.U {1},{2}", PLC_Device.W, Range_W, PLC_RTDevice.W1));
                        }
                        else 
                        {
                            B_NowTimes++;
                            plc_RTSend(string.Format("RDS {0}{1}.U {2},{3}", PLC_Device.B, (B_Shift + 1000).ToString("X"), Range_B, PLC_RTDevice.B1));
                        }


                        break;
                    case PLC_RTDevice.W1:

                        int W_Shift = W_NowTimes * 1000;
                        
                        for (int i = 0; i < splite.Length; i++)
                        {
                            W[i + W_Shift] = Convert.ToInt32(splite[i].ToString());
                        }
                        
                        if (W_NowTimes == W_TotalTimes-1)
                        {
                            W_NowTimes = 0;
                            plc_RTSend(string.Format("RDS {0}0000.U {1},{2}", PLC_Device.B, Range_B, PLC_RTDevice.B1));
                            PLCRead = true;
                        }
                        else 
                        {
                            W_NowTimes++;
                            plc_RTSend(string.Format("RDS {0}{1}.U {2},{3}", PLC_Device.W, (W_Shift+1000).ToString("X"), Range_W, PLC_RTDevice.W1));
                        }
                        
                        break;
                    case PLC_RTDevice.W2:
                        for (int i = 0; i < splite.Length; i++)
                        {
                            W[i + 1000] = Convert.ToInt32(splite[i].ToString());
                        }
                        plc_RTSend(string.Format("RDS {0}0000.U {1},{2}", PLC_Device.LR, Range_LR, PLC_RTDevice.LR1));
                        break;
                    case PLC_RTDevice.LR1:
                        for (int i = 0; i < splite.Length; i++) 
                        {
                            string HexToBit = Convert.ToString(Convert.ToInt32(splite[i].ToString()), 2).PadLeft(16, '0');

                            for (int j = 16; j > 0; j--)
                            {
                                LR[(i * 16) + 16 - j] = HexToBit.Substring(j - 1, 1) == "1" ? true : false;
                            }
                        }
                        plc_RTSend(string.Format("RDS {0}0000.U {1},{2}", PLC_Device.B, Range_B, PLC_RTDevice.B1));
                        break;
                }
            }
        }

        static object testlk = new object();
        //PLC Cmd BG.BK Dowork
        public static void Plc_CmdSend(All_Device ref_ADev,PLC_Device ref_Dev, int ref_Devno, int ref_val)
        {
            lock (testlk)
            {
                string send = string.Format("{0},WR {1}{2} {3}", ref_ADev, ref_Dev, ref_Devno.ToString("X"), ref_val);
                BK_CmdSend.EnQueue(send);
            }
        }

        public static void Plc_CmdSend_multi(All_Device ref_ADev, PLC_Device ref_Dev, int ref_DevStart, int ref_Devval, int ref_val)
        {
            lock (testlk)
            {
                string send = string.Format("{0},WRS {1}{2} {3}", ref_ADev, ref_Dev, ref_DevStart.ToString("X"), ref_Devval);
                for (int i = 0; i < ref_Devval; i++)
                {
                    send += " " + ref_val;
                }
                BK_CmdSend.EnQueue(send);
            }

        }

        private  void Dowork_CmdSend()
        {
            while (true)
            {
                string[] str = BK_CmdSend.DeQueue(System.Threading.Timeout.Infinite).Split(',');

                if (Sk_PLCCmd.tcpclt.Connected && Sk_PLCCmd.tcpclt.GetStream().CanWrite)
                {
                    if (str[0] != All_Device.PLCInterface.ToString()) 
                    {
                        string [] plcsplite=str[1].Split(' ');
                        UI.Log(NormalStatic.PLC, NormalStatic.PLC, SystemList.DeviceSend, string.Format("Device:{0}\tPLC:{1}\tValue:{2}", str[0], plcsplite[1], plcsplite[2]));
                    }
                    string temp = str[1];
                    byte[] tmpByte = System.Text.Encoding.UTF8.GetBytes(temp);
                    byte[] msgByte = new byte[tmpByte.Length + 1];
                    for (int i = 0; i < tmpByte.Length; i++)
                    {
                        msgByte[i] = tmpByte[i];
                    }
                    msgByte[tmpByte.Length] = CR;
                    Enum.TryParse(str[0], out PLC_CmdDev);
                    Sk_PLCCmd.tcpclt.Client.Send(msgByte);
                    PLC_CmdNextStep = true;
                }
                else
                {
                    //UI.Alarm(Sk_Device.PLC_RT.ToString(), ErrorList.ConnectionError, "PLC DisConnection");
                    PLC_CmdNextStep = false;
                }


                while (PLC_CmdNextStep)
                {
                    string Cmd_Recv = BK_CmdRecv.DeQueue(30000);
                    if (Cmd_Recv == null)
                    {
                        CmdRecv("Error", PLC_CmdDev);
                        break;
                    }

                    //Wayne Test8/5
                    if (PLC_CmdDev != All_Device.PLCInterface)
                    {
                        UI.Log(NormalStatic.PLC, NormalStatic.PLC, SystemList.DeviceReceive, string.Format("Device:{0}\tRecv:{1}", PLC_CmdDev, Cmd_Recv));
                    }
                    if (PLC_CmdDev == All_Device.PLCInterface)
                    {
                        InterRecv = true;
                        InterfaceRecvstr = Cmd_Recv;
                        break;
                    }
                    if (PLC_CmdDev == All_Device.PLCAlarmReset)
                    {
                        AlarmResetRecv = true;
                        AlarmRecvstr = Cmd_Recv;
                        break;
                    }
                    if (PLC_CmdDev == All_Device.PLCInitial)
                    {
                        InitialRecv = true;
                        InitialRecvstr = Cmd_Recv;
                        break;
                    }

                    CmdRecv(Cmd_Recv, PLC_CmdDev);
                    PLC_CmdNextStep = false;
                }
            }
        }

        private void Plc_CmdRecv(string str)
        {
            BK_CmdRecv.EnQueue(str);
        }      

        private void UI_Status(string ref_str)
        {
            string[] spref = ref_str.Split(',');
            UIChange temp = UIChange.Null;
            Enum.TryParse(spref[0], out temp);
            switch (temp)
            {
                case UIChange.PLC_RTStatus:
                    Color tempcolor = Color.Gray;
                    string stsstr = "";
                    switch (spref[1])
                    {
                        case NormalStatic.True:
                            {
                                plc_RTSend(string.Format("RDS {0}0000.U {1},{2}", PLC_Device.B, Range_B, PLC_RTDevice.B1));
                                stsstr = Sk_Connstr.Connect.ToString();
                                tempcolor = Color.Green;
                                Connection = ST_Conn = true;
                            }
                            break;

                        case NormalStatic.False:
                            {
                                if (stslb_PLCConn.Text == Sk_Connstr.Connect.ToString())
                                {
                                    UI.Alarm(NormalStatic.PLC, ErrorList.AP_SocketError_0382);
                                }
                                stsstr = Sk_Connstr.DisConnect.ToString();
                                tempcolor = Color.Red;
                                Connection = ST_Conn = true;
                            }
                            break;

                        default:
                            {
                                stsstr = Sk_Connstr.Unknow.ToString();
                                tempcolor = Color.Red;
                                Connection = false;
                            }
                            break;
                    }
                    if (InvokeRequired)
                    {
                        Invoke(new MethodInvoker(delegate { stslb_PLCConn.BackColor = tempcolor; }));
                        Invoke(new MethodInvoker(delegate { stslb_PLCConn.Text = stsstr; }));
                    }
                    else
                    {
                        stslb_PLCConn.BackColor = tempcolor;
                        stslb_PLCConn.Text = stsstr;
                    }
                    break;
            }
        }
    }
}
