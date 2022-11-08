using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace HirataMainControl
{
    public static class ULD_TCP
    {

    }

    internal class ULD_protocol
    {
        private static Thread send_T;
        static TCP_client client = new TCP_client();
        static subscriber user_txb = new subscriber("txt");//新增訂閱用戶

        private static object _sendLock = new object();
        private static object _Foup_reportLock = new object();

        public void Initial()
        {
            client.ini();
            client.create_subscriber(user_txb, "txt");
            client.add_subscribe(user_txb);
        }
        public void read_log(out string str)
        {
            str = "";
            client.read(user_txb, out str);
        }
        private static string send_str = "";
        public static void send(string send_str)
        {
            lock (_sendLock)
            {
                client.send(send_str);
            }
        }
        public static void send_only(string str)
        {

            send(str);
            //lock (_sendLock)
            //{
            //    send_str = str;
            //    send_T = new Thread(send);
            //    send_T.IsBackground = true;
            //    send_T.Start();
            //}

        }
        public static communication RB_Remote(string str)
        {
            if (client.alaive != true)
                return communication.unconnect;

            send_only(string.Format("{0},{1},{2}", "RB_Remote", "RB1", str));

            return communication.success;
        }
        public static communication Ui_Busy(bool status)
        {
            UserUnloader.SendCommand(string.Format("{0},{1},{2}", "Ui_Busy", "RB1", "Busy"));

            if (client.alaive != true)
                return communication.unconnect;

            if (status)
                send_only(string.Format("{0},{1},{2}", "Ui_Busy", "RB1", "Busy"));
            else
                send_only(string.Format("{0},{1},{2}", "Ui_Busy", "RB1", "Idle"));

            return communication.success;
        }
        public communication ini_complete()
        {
            return Foup_report("ini_complete", "", "");
        }
        public static communication RBreceive(string CMD, string REC)
        {
            UserUnloader.SendCommand(string.Format("{0},{1},{2}", "RBreceive", CMD, REC));

            if (client.alaive != true)
                return communication.unconnect;


            send_only(string.Format("{0},{1},{2}", "RBreceive", CMD, REC));

            return communication.success;

        }
        public static communication RBwaferPresent(string status)
        {
            UserUnloader.SendCommand(string.Format("{0},{1},{2}", "RBwaferPresent", "RB1", status));

            if (client.alaive != true)
                return communication.unconnect;


            send_only(string.Format("{0},{1},{2}", "RBwaferPresent", "RB1", status));

            return communication.success;
        }
        public static communication Ui_RobotPos(string status)
        {
            UserUnloader.SendCommand(string.Format("{0},{1},{2}", "Ui_RobotPos", "RB1", status));

            if (client.alaive != true)
                return communication.unconnect;


            send_only(string.Format("{0},{1},{2}", "Ui_RobotPos", "RB1", status));

            return communication.success;
        }
        public static communication ArmStatus(string status)
        {
            UserUnloader.SendCommand(string.Format("{0},{1},{2}", "ArmStatus", "RB1", status));

            if (client.alaive != true)
                return communication.unconnect;


            send_only(string.Format("{0},{1},{2}", "ArmStatus", "RB1", status));

            return communication.success;

        }
        public communication WaferPUT(string port, string Data)
        {
            switch (port)
            {
                case "LP1":
                    port = "port1";
                    break;
                case "LP2":
                    port = "port2";
                    break;
            }
            // return Foup_report("WaferPUT", port, Data);
            UserUnloader.SendCommand(string.Format("{0},{1},{2}", "WaferPUT", port, Data));

            send_only(string.Format("{0},{1},{2}", "WaferPUT", port, Data));

            return communication.success;
        }

        public static communication Foup_report(string commend, string port, string status)
        {
            if (client.alaive != true)
                return communication.unconnect;

            timmmer t1 = new timmmer();
            string anser = "";
            string[] anser_Split;
            subscriber Subscriber = new subscriber(commend);

            client.add_subscribe(Subscriber);//順序需要測試
            client.send(string.Format("{0},{1},{2}", commend, port, status));
            while (t1.start(500))//約10秒
            {
                client.read(Subscriber, out anser);
                anser_Split = anser.Split(',');
                if (anser_Split[0] == commend && anser_Split.Length == 2)
                {
                    if (anser_Split[1] != "success")
                    {
                        client.unsubscribe(Subscriber);
                        return communication.fail;
                    }

                    break;
                }
            }
            client.unsubscribe(Subscriber);
            if (!t1.start(500))
            {
                t1.reset();
                return communication.timeout;
            }
            t1.reset();

            return communication.success;

        }

        #region Loadport

        public static communication LPstatus(string port, string status)
        {
            if (client.alaive != true)
                return communication.unconnect;

            send_only(string.Format("{0},{1},{2}", "LPstatus", port, status));

            return communication.success;
        }
        public static communication FoupID(string port, string RFID)
        {
            return Foup_report("FoupID", port, RFID);
        }
        public static communication MappingData(string port, string Data)
        {
            //string port = "unknow";//DeviceName
            switch (port)
            {
                case "LP1":
                    port = "port1";
                    break;
                case "LP2":
                    port = "port2";
                    break;
            }
            UserUnloader.SendCommand(string.Format("{0},{1},{2}", "MappingData", port, Data));

            if (client.alaive != true)
                return communication.unconnect;

            send_only(string.Format("{0},{1},{2}", "MappingData", port, Data));

            return communication.success;
        }
        public static communication LP_FP_IN(string port)
        {
            string Oport;
            checkport(out Oport, port);
            UserUnloader.SendCommand(string.Format("{0},{1},{2}", "LP_FP_IN", port, ""));

            send_only(string.Format("{0},{1},{2}", "LP_FP_IN", port, ""));

            return communication.success;
        }
        public static communication LP_FP_OUT(string port)
        {
            string Oport;
            checkport(out Oport, port);
            UserUnloader.SendCommand(string.Format("{0},{1},{2}", "LP_FP_OUT", port, ""));
            send_only(string.Format("{0},{1},{2}", "LP_FP_OUT", port, ""));

            return communication.success;
        }

        public communication FoupRemote(string port, string Remote)
        {
            return Foup_report("FoupRemote", port, Remote);
        }

        public communication Foup_EQ_Arrive(string port)
        {
            return Foup_report("Foup_EQ_Arrive", port, "Arrive");
        }

        #endregion



        //Joanne 20220830
        public static communication SendJobComplete()
        {
            if (client.alaive != true)
                return communication.unconnect;


            send_only(string.Format("{0},{1}", "JobComplete", ""));

            return communication.success;
        }

        public static communication ULD_LPType()
        {
            if (client.alaive != true)
                return communication.unconnect;


            send_only(string.Format("{0},{1},{2},{3}", "LPType", "port2", "1", "13"));

            return communication.success;
        }

        private static void checkport(out string Oport, string port)
        {
            Oport = port;
            switch (port)
            {
                case "LP1":
                    Oport = "port1";
                    break;
                case "LP2":
                    Oport = "port2";
                    break;
            }

        }

    }


    public enum communication
    {
        success,
        fail,
        timeout,
        unconnect
    }

    #region connection
    class TCP_client
    {
        newspaper_office Newsboy = new newspaper_office("ULD");  // 發送者   (將接收的資料發送給訂閱者 ) 
        // subscriber subscriber_1 = new subscriber("log");                 // 訂閱者1 未使用
        // subscriber subscriber_2 = new subscriber("Ben");                 // 訂閱者2 未使用

        //Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Socket clientSocket;
        private Thread TD_Server;
        
        public bool alaive;
        public void ini()
        {
            alaive = false;
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //clientSocket.Connect(new IPEndPoint(IPAddress.Parse("192.168.1.23"), 502));

            try
            {
                //  clientSocket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4004));
                clientSocket.Connect(new IPEndPoint(IPAddress.Parse("192.168.0.1"), 4004));

                alaive = true;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            // Newsboy.Newsboy_Event += new newspaper_office.Newsboy_Handler(subscriber_1.get_paper);

            TD_Server = new Thread(Receive);
            TD_Server.IsBackground = true;
            TD_Server.Start();
        }
        public void create_subscriber(subscriber A, string name)
        {
            A = new subscriber(name);
        }
        public void add_subscribe(subscriber A)
        {
            Newsboy.Newsboy_Event += new newspaper_office.Newsboy_Handler(A.get_paper);
            Console.WriteLine("add_subscribe: {0}", A.name);
        }
        public void unsubscribe(subscriber A)
        {
            Newsboy.Newsboy_Event -= new newspaper_office.Newsboy_Handler(A.get_paper);
            Console.WriteLine("unsubscribe: {0}", A.name);
        }
        public void read(subscriber A, out string message)
        {
            string str = "";
            A.read_news(out str);
            message = str;
        }
        public void send(string str)
        {

                byte[] date = new byte[1024];
                date = Encoding.UTF8.GetBytes(str);
                if (clientSocket != null && clientSocket.Connected)
                    clientSocket.Send(date);
                Thread.Sleep(10);
      

        }

        private void Receive()
        {
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[1024 * 1024 * 2];
                    int r = clientSocket.Receive(buffer);
                    if (r == 0)
                    {
                        break;
                    }
                    string str = Encoding.UTF8.GetString(buffer, 0, r);
                    //Console.WriteLine(str);
                    Newsboy.send_paper(str);
                    Thread.Sleep(1);
                }
            }
            catch
            {
                clientSocket.Close();
            }
        }

    }
    class newspaper_office
    {
        private string name;
        public newspaper_office(string name)
        {
            this.name = name;
        }
        // 委派(方法類別)
        public delegate void Newsboy_Handler(string a);

        // 事件(方法變數)
        public event Newsboy_Handler Newsboy_Event;

        public void send_paper(string str)
        {
            Console.WriteLine("Hi i am {0}", name);
            if (Newsboy_Event != null)
            {
                Newsboy_Event(str);
            }
        }

    }
    class subscriber
    {
        Queue<string> news = new Queue<string>();
        public string name;
        public subscriber(string name)
        {
            this.name = name;
        }
        public void get_paper(string a)//(string newspaper)
        {
            // string newspaper = "aaa";
            news.Enqueue(a);
            Console.WriteLine("send {1} to {0}", name, a);
        }
        public void get_paper(string a, Queue<string> otherQ)//(string newspaper)
        {
            // string newspaper = "aaa";
            otherQ.Enqueue(a);
            Console.WriteLine("send {1} to {0}", name, a);
        }

        public void read_news(out string message)
        {
            if (news.Count > 0)
                message = news.Dequeue();
            else
                message = "";
        }
    }


    #endregion

}
