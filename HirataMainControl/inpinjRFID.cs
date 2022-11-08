using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Impinj.OctaneSdk;
using System.Threading;


namespace HirataMainControl
{
    public partial class inpinjRFID : UserControl
    {
        public delegate void MyInvoke(string str);

        //const string READER_HOSTNAME = "192.168.10.21";  // NEED to set to your speedway!
        // Create an instance of the ImpinjReader class.
        const string READER_HOSTNAME = "192.168.1.22";
        int Read_power = 25;

        ImpinjReader reader = new ImpinjReader();   // ImpinjReader reader = new ImpinjReader();

        private Thread UI;

        public static Stack<int> ID_clear = new Stack<int>();

        public inpinjRFID()
        {
            InitializeComponent();
        }

        private void inpinjRFID_Load(object sender, EventArgs e)
        {
            //   START();
            UI = new Thread(UI_update);
            UI.Start();
        }
        public string P1_UI = "";
        public string P2_UI = "";

        public static void UI_clear(int port)
        {
            ID_clear.Push(port);
        }


        public void UI_update()
        {
            while (true)
            {
                if (ID_clear.Count > 0)
                {

                    txt_clear(ID_clear.Pop());
                    ID_clear.Clear();
                }

            }

        }

        public void port1safty(string str)
        {

            txt_port1.Text = str;

        }
        private void port2safty(string str)
        {
            txt_port2.Text = str;
        }
        public void START()
        {
            try
            {


                // Assign a name to the reader. 
                // This will be used in tag reports. 
                reader.Name = "My Reader #1";

                // Connect to the reader.
                ConnectToReader();

                // Get the default settings.
                // We'll use these as a starting point
                // and then modify the settings we're 
                // interested in.
                Settings settings = reader.QueryDefaultSettings();

                // Start the reader as soon as it's configured.
                // This will allow it to run without a client connected.
                settings.AutoStart.Mode = AutoStartMode.Immediate;
                settings.AutoStop.Mode = AutoStopMode.None;

                settings.Antennas[0].TxPowerInDbm = Read_power;
                settings.Antennas[1].TxPowerInDbm = Read_power;

                // Use Advanced GPO to set GPO #1 
                // when an client (LLRP) connection is present.
                settings.Gpos.GetGpo(1).Mode = GpoMode.LLRPConnectionStatus;
                // settings. = 15;
                FeatureSet Antsetting = reader.QueryFeatureSet();
                TxPowerTableEntry value;
                value.Dbm = 10;
                value.Index = 1;
                Antsetting.TxPowers[0] = value;
                value.Index = 2;
                Antsetting.TxPowers[1] = value;


                // Tell the reader to include the timestamp in all tag reports.
                settings.Report.IncludeFirstSeenTime = true;
                settings.Report.IncludeLastSeenTime = true;
                settings.Report.IncludeSeenCount = true;
                settings.Report.IncludeAntennaPortNumber = true;
                settings.Report.IncludePhaseAngle = true;

                // If this application disconnects from the 
                // reader, hold all tag reports and events.
                settings.HoldReportsOnDisconnect = true;

                // Enable keepalives.
                settings.Keepalives.Enabled = true;
                settings.Keepalives.PeriodInMs = 5000;

                // Enable link monitor mode.
                // If our application fails to reply to
                // five consecutive keepalive messages,
                // the reader will close the network connection.
                settings.Keepalives.EnableLinkMonitorMode = true;
                settings.Keepalives.LinkDownThreshold = 5;

                //settings.Antennas.

                // Assign an event handler that will be called
                // when keepalive messages are received.
                reader.KeepaliveReceived += OnKeepaliveReceived;

                // Assign an event handler that will be called
                // if the reader stops sending keepalives.
                reader.ConnectionLost += OnConnectionLost;

                // Apply the newly modified settings.
                reader.ApplySettings(settings);
                //(Antsetting);
                // Save the settings to the reader's 
                // non-volatile memory. This will
                // allow the settings to persist
                // through a power cycle.
                reader.SaveSettings();

                // Assign the TagsReported event handler.
                // This specifies which method to call
                // when tags reports are available.
                reader.TagsReported += OnTagsReported;


                Console.WriteLine("{0}", settings.Antennas.Length);



                // reader.

                // Wait for the user to press enter.
                //    Console.WriteLine("Press enter to exit.");
                //  Console.ReadLine();

                // Stop reading.
                // reader.Stop();

                // Disconnect from the reader.
                //reader.Disconnect();
            }
            catch (OctaneSdkException e)
            {
                // Handle Octane SDK errors.
                Console.WriteLine("Octane SDK exception: {0}", e.Message);
            }
            catch (Exception e)
            {
                // Handle other .NET errors.
                Console.WriteLine("Exception : {0}", e.Message);
            }
        }
        void End()
        {
            // Stop reading.
            reader.Stop();

            // Disconnect from the reader.
            reader.Disconnect();
        }
        void ConnectToReader()
        {
            try
            {

                Console.WriteLine("Attempting to connect to {0} ({1}).",
                    reader.Name, READER_HOSTNAME);

                // The maximum number of connection attempts
                // before throwing an exception.
                //reader.MaxConnectionAttempts = 15;

                // Number of milliseconds before a 
                // connection attempt times out.
                reader.ConnectTimeout = 10000;
                // Connect to the reader.
                // Change the ReaderHostname constant in SolutionConstants.cs 
                // to the IP address or hostname of your reader.
                reader.Connect(READER_HOSTNAME);
                Console.WriteLine("Successfully connected.");
                MyInvoke mi = new MyInvoke(port1safty);
                this.BeginInvoke(mi, new Object[] { "Successfully connected." });
                // Tell the reader to send us any tag reports and 
                // events we missed while we were disconnected.
                reader.ResumeEventsAndReports();
            }
            catch (OctaneSdkException e)
            {
                Console.WriteLine("Failed to connect.{0}", e);

                MyInvoke mi = new MyInvoke(port1safty);
                this.BeginInvoke(mi, new Object[] { "Failed to connect." });
               // throw e;
            }
        }

        void OnConnectionLost(ImpinjReader reader)
        {
            // This event handler is called if the reader  
            // stops sending keepalive messages (connection lost).
            Console.WriteLine("Connection lost : {0} ({1})", reader.Name, reader.Address);

            // Cleanup
            reader.Disconnect();

            // Try reconnecting to the reader
            ConnectToReader();
        }


        void OnKeepaliveReceived(ImpinjReader reader)
        {
            // This event handler is called when a keepalive 
            // message is received from the reader.
            Console.WriteLine("Keepalive received from {0} ({1})", reader.Name, reader.Address);


            if (frmMain.IDreader == readkind.Hand)
                return;
            P1_ID = "";
            P2_ID = "";

            MyInvoke mi = new MyInvoke(port1safty);
            this.BeginInvoke(mi, new Object[] { P1_ID });
            //  HT.LP_RFID[0] = P1_ID;

            mi = new MyInvoke(port2safty);
            this.BeginInvoke(mi, new Object[] { P2_ID });
            //   HT.LP_RFID[1] = P2_ID;

        }

        public string P1_ID = "";
        public string P2_ID = "";


        void OnTagsReported(ImpinjReader sender, TagReport report)
        {
            // This event handler is called asynchronously 
            // when tag reports are available.
            // Loop through each tag in the report 
            // and print the data.
            //antenna
            foreach (Tag tag in report)
            {


                Console.WriteLine("Antenna: {0}  EPC : {1} Timestamp : {2} PhaseAngle: {3}",
                    tag.AntennaPortNumber, tag.Epc, tag.LastSeenTime, tag.PhaseAngleInRadians);
                if (tag.AntennaPortNumber == 1)
                {
                    if (P1_ID != string.Format("{0}", tag.Epc))
                    {
                        P1_ID = string.Format("{0}", tag.Epc);

                        if (frmMain.IDreader != readkind.Hand)
                        {
                            MyInvoke mi = new MyInvoke(port1safty);
                            //      P1_UI
                            this.BeginInvoke(mi, new Object[] { P1_ID });
                            HT.LP_RFID[0] = P1_ID;

                        }
                    }
                }
                if (tag.AntennaPortNumber == 2)
                {
                    if (P2_ID != string.Format("{0}", tag.Epc))
                    {
                        P2_ID = string.Format("{0}", tag.Epc);

                        if (frmMain.IDreader != readkind.Hand)
                        {
                            MyInvoke mi = new MyInvoke(port2safty);
                            this.BeginInvoke(mi, new Object[] { P2_ID });
                            HT.LP_RFID[1] = P2_ID;
                        }
                    }
                }
            }
        }
        public void txt_clear(int port)
        {
            if (frmMain.IDreader != readkind.Hand)
                return;

            MyInvoke mi = new MyInvoke(port1safty);

            if (port == 0)
            {
                P1_ID = "";

                this.BeginInvoke(mi, new Object[] { P1_ID });
                HT.LP_RFID[0] = P1_ID;
            }
            if (port == 1)
            {
                P2_ID = "";
                mi = new MyInvoke(port2safty);
                this.BeginInvoke(mi, new Object[] { P2_ID });
                HT.LP_RFID[1] = P2_ID;

            }

        }
        private void label1_Click(object sender, EventArgs e)
        {
            if (!reader.IsConnected)
                START();
        }
        private void label2_Click(object sender, EventArgs e)
        {
        }
        private void txt_port1_TextChanged(object sender, EventArgs e)
        {
            if (frmMain.IDreader != readkind.Hand)
                return;
            HT.LP_RFID[0] = txt_port1.Text;
            P1_ID = HT.LP_RFID[0];
        }

        private void txt_port2_TextChanged(object sender, EventArgs e)
        {
            if (frmMain.IDreader != readkind.Hand)
                return;
            HT.LP_RFID[1] = txt_port2.Text;

            P2_ID = HT.LP_RFID[1];
        }
    }
}
