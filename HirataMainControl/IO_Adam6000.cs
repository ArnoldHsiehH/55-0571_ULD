using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Advantech.Adam;


namespace HirataMainControl
{
    public partial class IO_Adam6000 : UserControl
    {
        #region Event/delegate

        public delegate void DiChangeEvent(Adam6050_DI Index, bool Result);
        public delegate void SetResultEvnet(int index, SocketCommand command, string Paratemer, bool Result);
        public event DiChangeEvent EvnetAdmaDIChange;
        public event SetResultEvnet EvnetAdmaSetResult;

        #endregion

        #region BG/Queue

        private AdamSocket Adam6000s;
        private BackgroundWorker ControlBG = new BackgroundWorker();
        private BackgroundWorker ReadBG = new BackgroundWorker();
        private BlockQueue<string> CommandBQ = new BlockQueue<string>();

        #endregion

        #region Variable

        public bool[] Old_DI_data;
        private bool[] DI_data;
        private bool[] DO_data;
        private int[] Old_AI_data;
        private int[] AI_data;
        private int[] AO_data;
        private int DOCount = 2;
        private int DICount = 2;
        private int AICount = 6;
        private int AOCount = 2;
        private int DIStartAddress = 1;
        private int DOStartAddress = 17;
        private int AIStartAddress = 1;
        private int AOStartAddress = 11;
        private int AOUseCount = 0;
        private float AOInitialValue = 0F;

        public string DeviceName = "Adam6050";
        private int DeviceIndex = 0;
        private string AdamIP = "192.168.0.1";
        private int AdamPort = 502;
        private bool ConnectStatus = false;
        private bool RefreshInitialOutput = true;
        private bool RefreshAIType = true;
        private uint RetryCount;
        private Adam6024_InputRange[] AITypes;
        private Adam6024_OutputRange[] AOTypes;
        private int LoopCount = 100; //0.1S
        private bool ReadBG_Cloas = false;
        private object AdamLock = new object();
        private bool ReadResult = false;
        public ErrorList NowErrorList = ErrorList.MaxCnt;

        #endregion

        #region Enum

        public enum AnalogIO_Type { Voltage, Current_0_20mA, Current_4_20mA }
        public enum AdamSeries { Adam6050, Adam6052, Adam6024, Adam6224 }

        #endregion

        #region Class

        private class IO_ExcelListClass
        {
            public string IO_Name;
            public string IO_Type;
            public int IO_Position;
        }

        private class DO_FlashListClass
        {
            public int DO_Position;
            public bool DO_FlashFlag;
        }

        #endregion

        #region DataTable/List

        private DataTable IO_DataTable;
        private List<IO_ExcelListClass> IO_ExcelList;
        private DataGridView IO_DGV;
        private List<DO_FlashListClass> DO_FlashList;

        #endregion

        #region Initial

        public IO_Adam6000()
        {
            InitializeComponent();
        }

        public void Initial(int index, int Port = 502, int SetTimeout = 1000)
        {
            string strTemp;
            string[] SplitTemp;
            DeviceIndex = index;
            strTemp = AppSetting.LoadSetting(string.Format("{0}{1}{2}", NormalStatic.Adam, index + 1, "_IP"), "Adam6050,192.168.0.5,1,0");
            SplitTemp = strTemp.Split(',');
            DeviceName = SplitTemp[0];
            RetryCount = 1;
            switch (DeviceName.Substring(0, 8))
            {
                case "Adam6024":
                    {
                        DOCount = 2;
                        DICount = 2;
                        AICount = 6;
                        AOCount = 2;
                        DIStartAddress = 1;
                        DOStartAddress = 17;
                        AIStartAddress = 1;
                        AOStartAddress = 11;
                        AOUseCount = int.Parse(SplitTemp[2]);
                        AOInitialValue = float.Parse(SplitTemp[3]);
                    }
                    break;

                case "Adam6050":
                    {
                        DOCount = 6;
                        DICount = 12;
                        AICount = 0;
                        AOCount = 0;
                        DIStartAddress = 1;
                        DOStartAddress = 17;
                        AIStartAddress = 1;
                        AOStartAddress = 11;
                    }
                    break;

                case "Adam6052":
                    {
                        DOCount = 8;
                        DICount = 8;
                        AICount = 0;
                        AOCount = 0;
                        DIStartAddress = 1;
                        DOStartAddress = 17;
                        AIStartAddress = 1;
                        AOStartAddress = 11;
                    }
                    break;

                case "Adam6224":
                    {
                        DOCount = 0;
                        DICount = 4;
                        AICount = 0;
                        AOCount = 4;
                        DIStartAddress = 1;
                        DOStartAddress = 17;
                        AIStartAddress = 1;
                        AOStartAddress = 1;
                    }
                    break;
            }

            #region Socket

            Adam6000s = new Advantech.Adam.AdamSocket(Advantech.Adam.AdamType.Adam6000);
            Adam6000s.SetTimeout(SetTimeout, SetTimeout, SetTimeout);
            AdamIP = SplitTemp[1];
            AdamPort = Port;
            bool a = Adam6000s.Connect(AdamIP, System.Net.Sockets.ProtocolType.Tcp, AdamPort);


            #endregion

            #region UI

            gbxTable.Text = DeviceName;
            gbxTable.Enabled = false;

            DI_data = new bool[DICount];
            Old_DI_data = new bool[DICount];
            Old_AI_data = new int[AICount];
            DO_data = new bool[DOCount];
            AI_data = new int[AICount];
            AO_data = new int[AOCount];
            AITypes = new Adam6024_InputRange[AICount];
            AOTypes = new Adam6024_OutputRange[AOCount];

            if (AOCount == 0)
            {
                gbxAo.Visible = false;
            }
            else
            {
                for (int i = 0; i < AOCount; i++)
                {
                    cboAo.Items.Add(i);
                }
                cboAo.SelectedItem = 0;

            }

            if (DOCount == 0)
            {
                gbxDo.Visible = false;
            }
            else
            {
                DO_FlashList = new List<DO_FlashListClass>();
                for (int i = 0; i < DOCount; i++)
                {
                    cboDo.Items.Add(i);
                    DO_FlashList.Add(new DO_FlashListClass
                    {
                        DO_Position = i,
                        DO_FlashFlag = false,
                    });
                }

                cboDoValue.Items.Add(NormalStatic.Off);
                cboDoValue.Items.Add(NormalStatic.On);
                cboDoValue.Items.Add(NormalStatic.Flash);
                cboDoValue.SelectedItem = 0;
                cboDo.SelectedItem = 0;
            }

            IO_DGV = new DataGridView();
            IO_DGV.Dock = System.Windows.Forms.DockStyle.Fill;
            IO_DGV.AllowUserToResizeColumns = false;
            IO_DGV.AllowUserToResizeRows = false;
            IO_DGV.RowHeadersVisible = false;
            IO_DGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            IO_DataTable = new DataTable();
            IO_DataTable.Columns.Add(Adam_Static.IOType);
            IO_DataTable.Columns.Add(Adam_Static.IOName);
            IO_DataTable.Columns.Add(Adam_Static.Channel);
            IO_DataTable.Columns.Add(Adam_Static.IOValue);

            IO_DGV.DataSource = IO_DataTable;
            gbxTable.Controls.Add(IO_DGV);

            IO_ExcelList = new List<IO_ExcelListClass>();

            for (int j = 0; j < HCT_EFEM.ExcelAdmam[index][0].GetLength(0); j++)
            {
                IO_ExcelList.Add(new IO_ExcelListClass
                {
                    IO_Name = HCT_EFEM.ExcelAdmam[index][0][j, 0],
                    IO_Type = HCT_EFEM.ExcelAdmam[index][0][j, 1],
                    IO_Position = Convert.ToInt32(HCT_EFEM.ExcelAdmam[index][0][j, 2])
                });
            }

            for (int Dioidx = 0; Dioidx < IO_ExcelList.Count; Dioidx++)
            {
                DataRow dr = IO_DataTable.NewRow();
                dr[Adam_Static.IOType] = IO_ExcelList[Dioidx].IO_Type;
                dr[Adam_Static.IOName] = IO_ExcelList[Dioidx].IO_Name;
                dr[Adam_Static.Channel] = IO_ExcelList[Dioidx].IO_Position;
                IO_DataTable.Rows.Add(dr);
            }

            #endregion

            #region BG

            ControlBG.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Control_DoWork);
            ControlBG.RunWorkerAsync();

            ReadBG.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Read_DoWork);
            ReadBG.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Read_RunWorkerCompleted);
            ReadBG.RunWorkerAsync();

            #endregion

            if (Adam6000s.Connected)
            {
                for (int i = 0; i < AICount; i++)
                {
                    SetAiRange(i, Adam6024_InputRange.V_Neg10To10);
                }

                for (int i = 0; i < AOCount; i++)
                {
                    SetAoRange(i, Adam6024_OutputRange.V_0To10);
                }

                for (int i = 0; i < AOUseCount; i++)
                {
                    SetAO(SocketCommand.MaxCnt, i, Convert.ToInt16(409.5f * AOInitialValue));
                }

                Ui_Connect = true;
                UI.InitialSystem(DeviceName, NormalStatic.True, ErrorList.MaxCnt);
            }
            else
            {
                if (DeviceName.Contains("6050"))
                    UI.InitialSystem(DeviceName, NormalStatic.False, ErrorList.IO_6050ConnectFail_0900);
                else
                    UI.InitialSystem(DeviceName, NormalStatic.False, ErrorList.IO_6024ConnectFail_0901);
            }
        }

        public void OpenDevice()
        {
            RetryCount = 3;

            do
            {
                if (Adam6000s.Connected)
                    Adam6000s.Disconnect();

                RetryCount--;
                if (Adam6000s.Connect(AdamIP, System.Net.Sockets.ProtocolType.Tcp, AdamPort))
                {
                    DI_data = new bool[DICount];
                    Old_DI_data = new bool[DICount];
                    Old_AI_data = new int[AICount];
                    DO_data = new bool[DOCount];
                    AI_data = new int[AICount];
                    AO_data = new int[AOCount];
                    Ui_Connect = true;
                    RefreshInitialOutput = true;
                    break;
                }

                if (RetryCount == 0)
                {
                    Ui_Connect = false;
                    break;
                }

                System.Threading.Thread.Sleep(100);

            } while (RetryCount > 0);

        }

        #endregion

        #region BG

        private void Control_DoWork(object sender, DoWorkEventArgs e)
        {
            int CloseBGCount = 0;
            bool CmdSuccess;
            string Process;
            string[] Received;

            while (true)
            {
                CmdSuccess = false;
                Process = CommandBQ.DeQueue();
                if (Process != null)
                {
                    if (Process == NormalStatic.End)
                    {
                        ReadBG_Cloas = true;
                        break;
                    }

                    else if (Process == NormalStatic.Close)
                    {
                        CloseBGCount++;
                        if (CloseBGCount >= DOCount)
                            CommandBQ.EnQueue(NormalStatic.End);
                    }
                    else
                    {
                        Received = Process.Split(new string[] { NormalStatic.Comma }, StringSplitOptions.None);
                        SocketCommand cmd = (SocketCommand)Enum.Parse(typeof(SocketCommand), Received[0]);
                        switch (Received[2])
                        {
                            #region DO

                            case Adam_Static.SetDO:
                            case Adam_Static.SetDO_Flash:
                                {
                                    int Do_index = Convert.ToInt32(Received[3]);
                                    try
                                    {
                                        CmdSuccess = WriteDO_Single(Do_index, !(Received[4] == "0"));

                                        if (CmdSuccess)
                                        {
                                            DO_data[Do_index] = !(Received[4] == "0");
                                            IO_DataTable.Rows[Do_index + DICount][Adam_Static.IOValue] = DO_data[Do_index] ? "1" : "0";
                                        }
                                        else
                                        {
                                            switch (cmd)
                                            {
                                                case SocketCommand.SignalTower:
                                                    NowErrorList = ErrorList.IO_SetSignalTowerFail_0961;
                                                    break;
                                                case SocketCommand.Buzzer:
                                                    NowErrorList = ErrorList.IO_SetBuzzerFail_0960;
                                                    break;
                                                case SocketCommand.SetEFEMInterlock:
                                                    NowErrorList = ErrorList.IO_SetInterlockFail_0962;
                                                    break;
                                            }
                                        }

                                    }
                                    catch
                                    {
                                        CmdSuccess = false;

                                        switch (cmd)
                                        {
                                            case SocketCommand.SignalTower:
                                                NowErrorList = ErrorList.IO_SetSignalTowerFail_0961;
                                                break;
                                            case SocketCommand.Buzzer:
                                                NowErrorList = ErrorList.IO_SetBuzzerFail_0960;
                                                break;
                                            case SocketCommand.SetEFEMInterlock:
                                                NowErrorList = ErrorList.IO_SetInterlockFail_0962;
                                                break;
                                        }
                                    }

                                    UI.Log(NormalStatic.IO, DeviceName, SystemList.CommandComplete, string.Format("{0}:({1})({2})({3})({4})", CmdSuccess, cmd, Received[2], Do_index, Received[4]));

                                    //  EvnetAdmaSetResult(DeviceIndex, cmd, Received[1], CmdSuccess);

                                    if (Received[2] == Adam_Static.SetDO_Flash && CmdSuccess)
                                    {
                                        if (tmr1s.Enabled == false)
                                        {
                                            this.Invoke((MethodInvoker)delegate () { tmr1s.Enabled = true; });
                                        }
                                    }
                                }
                                break;

                            case Adam_Static.SetDO_All:
                                {
                                    try
                                    {
                                        int SetValue = Convert.ToInt32(Received[3]);
                                        CmdSuccess = WriteDO(SetValue);

                                        if (CmdSuccess)
                                        {
                                            for (int i = 0; i < DO_data.Length; i++)
                                            {
                                                DO_data[i] = ((SetValue % 2) == 1);
                                                SetValue /= 2;
                                            }
                                        }
                                        else
                                        {
                                            NowErrorList = ErrorList.IO_SetSignalTowerFail_0961;
                                        }
                                    }
                                    catch
                                    {
                                        CmdSuccess = false;

                                        NowErrorList = ErrorList.IO_SetSignalTowerFail_0961;
                                    }
                                    UI.Log(NormalStatic.IO, DeviceName, SystemList.CommandComplete, string.Format("{0}:({1})({2})({3})", CmdSuccess, cmd, Received[2], Received[3]));

                                    EvnetAdmaSetResult(DeviceIndex, cmd, Received[1], CmdSuccess);

                                    if (Received[1] == SignalTown.AllFlash.ToString() && CmdSuccess)
                                    {
                                        if (tmr1s.Enabled == false)
                                        {
                                            this.Invoke((MethodInvoker)delegate () { tmr1s.Enabled = true; });
                                        }
                                    }

                                }
                                break;

                            case Adam_Static.SetDO_AllFlash:
                                {
                                    int SetValue = Convert.ToInt32(Received[3]);
                                    CmdSuccess = WriteDO(SetValue);

                                    if (CmdSuccess)
                                    {
                                        for (int i = 0; i < DO_data.Length; i++)
                                        {
                                            DO_data[i] = ((SetValue % 2) == 1);
                                            IO_DataTable.Rows[i + DICount][Adam_Static.IOValue] = DO_data[i] ? "1" : "0";

                                            SetValue /= 2;
                                        }
                                    }
                                }
                                break;

                            #endregion

                            #region AO

                            case Adam_Static.SetAO:
                                {
                                    double OutputAO = 0;
                                    try
                                    {

                                        int Ao_index = Convert.ToInt32(Received[3]);
                                        CmdSuccess = WriteAO_Single(Ao_index, Convert.ToInt32(Received[4]));

                                        if (CmdSuccess)
                                        {
                                            AO_data[Ao_index] = Convert.ToInt32(Received[4]);

                                            if (AOTypes[Ao_index] == Adam6024_OutputRange.V_0To10)
                                                OutputAO = (double)(AO_data[Ao_index] * 10.0 / 4095.0);
                                            else if (AOTypes[Ao_index] == Adam6024_OutputRange.mA_0To20)
                                                OutputAO = (double)(AO_data[Ao_index] * 20.0 / 4095.0);
                                            else
                                                OutputAO = (double)(AO_data[Ao_index] * 16.0 / 4095.0) + 4;

                                            IO_DataTable.Rows[Ao_index + DICount + DOCount + AICount][Adam_Static.IOValue] = Math.Round(OutputAO, 2);

                                            //      if (AOUseCount - 1 == Ao_index)
                                            //          frmMain.EFEM_FFUvalue = IO_DataTable.Rows[Ao_index + DICount + DOCount + AICount][Adam_Static.IOValue].ToString();
                                        }
                                        else
                                        {
                                            switch (cmd)
                                            {
                                                case SocketCommand.SetFFUVoltage:
                                                    NowErrorList = ErrorList.IO_SetFFUFail_0963;
                                                    break;
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        CmdSuccess = false;
                                        NowErrorList = ErrorList.IO_SetFFUFail_0963;
                                    }

                                    UI.Log(NormalStatic.IO, DeviceName, SystemList.CommandComplete, string.Format("{0}:({1})({2})({3})({4})", CmdSuccess, cmd, Received[2], Received[3], OutputAO));

                                    EvnetAdmaSetResult(DeviceIndex, cmd, Received[1], CmdSuccess);

                                }
                                break;

                            #endregion

                            #region Range

                            case Adam_Static.SetAORange:
                                {
                                    try
                                    {
                                        int Ao_index = Convert.ToInt32(Received[3]);
                                        CmdSuccess = WriteAO_Range(Ao_index, (Adam6024_OutputRange)Enum.Parse(typeof(Adam6024_OutputRange), Received[4]));

                                        if (CmdSuccess)
                                        {
                                            AOTypes[Ao_index] = (Adam6024_OutputRange)Enum.Parse(typeof(Adam6024_OutputRange), Received[4]);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        UI.Error(DeviceName, ErrorList.AP_ExceptionOccur_0199, string.Format("{0}-{1}", cmd, ex));
                                    }
                                }
                                break;


                            case Adam_Static.SetAIRange:
                                {
                                    try
                                    {
                                        int Ao_index = Convert.ToInt32(Received[3]);
                                        CmdSuccess = WriteAI_Range(Ao_index, (Adam6024_InputRange)Enum.Parse(typeof(Adam6024_InputRange), Received[4]));
                                        if (CmdSuccess)
                                        {
                                            AITypes[Ao_index] = (Adam6024_InputRange)Enum.Parse(typeof(Adam6024_InputRange), Received[4]);
                                            RefreshAIType = true;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        UI.Error(DeviceName, ErrorList.AP_ExceptionOccur_0199, string.Format("{0}-{1}", cmd, ex));
                                    }
                                }
                                break;

                                #endregion
                        }
                    }
                }
            }
        }

        private void Read_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (ReadBG_Cloas == true)
                    break;

                if (Adam6000s.Connected && ConnectStatus == true)
                {
                    System.Threading.Thread.Sleep(LoopCount);
                    try
                    {
                        ReadDI();
                        ReadAI();
                        if (RefreshInitialOutput)
                        {
                            ReadDO();
                            ReadAO();
                            RefreshInitialOutput = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        UI.Error(DeviceName, ErrorList.AP_ExceptionOccur_0199, string.Format("{0},{1}", "ReadBG", ex.ToString()));
                    }
                }
                else
                {
                    System.Threading.Thread.Sleep(1000);
                    CommandBQ.Clear();
                    OpenDevice();
                }
            }
        }

        private void Read_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Ui_Connect = false;
            Adam6000s.Disconnect();
            UI.CloseBG(DeviceName);
        }

        public void Close()
        {
            if (DOCount != 0 && ConnectStatus)
            {
                for (int i = 0; i < DOCount; i++)
                    CommandBQ.EnQueue(NormalStatic.Close);
            }
            else
                CommandBQ.EnQueue(NormalStatic.End);
        }

        #endregion

        #region Write

        private bool WriteDO_Single(int DO, bool value)
        {
            if (DO < DOCount)
            {
                lock (AdamLock)
                {
                    return Adam6000s.Modbus().ForceSingleCoil(DOStartAddress + DO, value);
                }
            }
            else
                return false;
        }

        private bool WriteDO(int value)
        {
            bool[] WriteValue = new bool[DOCount];
            for (int i = 0; i < DOCount; i++)
            {
                WriteValue[i] = ((value % 2) == 1);
                value /= 2;
            }

            lock (AdamLock)
            {
                return Adam6000s.Modbus().ForceMultiCoils(DOStartAddress, WriteValue);
            }
        }

        private bool WriteAO_Single(int AO, int value)
        {
            if (AO < AOCount)
            {
                lock (AdamLock)
                {
                    return Adam6000s.Modbus().PresetSingleReg(AOStartAddress + AO, value);
                }
            }
            else
                return false;
        }

        private bool WriteAO_Range(int AO, Adam6024_OutputRange range)
        {
            if (AO < AOCount)
            {
                lock (AdamLock)
                {
                    return Adam6000s.AnalogOutput().SetConfiguration(AO, (byte)range);
                }
            }
            else
                return false;
        }

        private bool WriteAI_Range(int AI, Adam6024_InputRange range)
        {
            if (AI < AICount)
            {
                lock (AdamLock)
                {
                    return Adam6000s.AnalogInput().SetInputRange(AI, (byte)range);
                }
            }
            else
                return false;
        }

        #endregion

        #region Read

        public bool readTheDI(out bool[] bDiData)//hugo 20150412
        {
            int iDiStart = 1;//, iDoStart = 17;

            //  int iChTotal;
            //    bool[] bDiData;
            lock (AdamLock)//Hugo 20160418
            {
                return Adam6000s.Modbus().ReadCoilStatus(iDiStart, 12, out bDiData);
            }
            //return bDiData;        
        }

        private void ReadDI()
        {
            if (DICount > 0)
            {
                lock (AdamLock)
                {
                    ReadResult = Adam6000s.Modbus().ReadInputStatus(DIStartAddress, DICount, out DI_data);
                }
                if (ReadResult)
                {
                    for (int i = 0; i < DICount; i++)
                    {
                        if ((Old_DI_data[i] != DI_data[i]) || RefreshInitialOutput)
                        {
                            Old_DI_data[i] = DI_data[i];
                            IO_DataTable.Rows[i][Adam_Static.IOValue] = DI_data[i] ? "1" : "0";
                            EvnetAdmaDIChange(((Adam6050_DI)i + (DeviceIndex * DICount)), DI_data[i]);
                        }
                    }

                }
                else
                {
                    if (Adam6000s.Connect(AdamIP, System.Net.Sockets.ProtocolType.Tcp, AdamPort) == false)
                    {
                        CommandBQ.Clear();
                        if (DeviceName.Contains("6050"))
                            SocketClient.Send_AlarmReceive(SocketCommand.Event, NormalStatic.EFEM, ErrorList.IO_6050Disconnect_0902, string.Format("->{0}", AdamIP));
                        else
                            SocketClient.Send_AlarmReceive(SocketCommand.Event, NormalStatic.EFEM, ErrorList.IO_6024Disconnect_0903, string.Format("->{0}", AdamIP));


                        Ui_Connect = false;
                    }
                }
            }
        }

        private void ReadDO()
        {
            if (DOCount > 0)
            {
                lock (AdamLock)
                {
                    ReadResult = Adam6000s.Modbus().ReadCoilStatus(DOStartAddress, DOCount, out DO_data);
                }

                if (ReadResult)
                {
                    for (int i = 0; i < DOCount; i++)
                    {
                        IO_DataTable.Rows[i + DICount][Adam_Static.IOValue] = DO_data[i] ? "1" : "0";
                    }
                }

            }
        }

        private void ReadAI()
        {
            if (AICount > 0)
            {
                lock (AdamLock)
                {
                    ReadResult = Adam6000s.Modbus().ReadHoldingRegs(AIStartAddress, AICount, out AI_data);
                }

                if (ReadResult)
                {
                    for (int i = 0; i < AICount; i++)
                    {
                        if (Math.Abs(Old_AI_data[i] - AI_data[i]) > 30 || RefreshAIType)
                        {
                            Old_AI_data[i] = AI_data[i];

                            double[] OutputAI = new double[AICount];
                            if (AITypes[i] == Adam6024_InputRange.V_Neg10To10)
                                OutputAI[i] = (double)(AI_data[i] * 20.0 / 65535.0) - 10;
                            else if (AITypes[i] == Adam6024_InputRange.mA_0To20)
                                OutputAI[i] = (double)(AI_data[i] * 20.0 / 65535.0);
                            else
                                OutputAI[i] = (double)(AI_data[i] * 16.0 / 65535.0) + 4;

                            IO_DataTable.Rows[i + DICount + DOCount][Adam_Static.IOValue] = Math.Round(OutputAI[i], 3);

                            if (i < 2)
                                frmMain.PSW_AIvalue[i] = Math.Round(OutputAI[i], 3);
                        }
                    }

                    RefreshAIType = false;
                }
            }
        }

        private void ReadAO()
        {
            if (AOCount > 0)
            {
                lock (AdamLock)
                {
                    ReadResult = Adam6000s.Modbus().ReadInputRegs(AOStartAddress, AOCount, out AO_data);
                }

                if (ReadResult)
                {
                    for (int i = 0; i < AOCount; i++)
                    {
                        IO_DataTable.Rows[i + DICount + DOCount + AICount][Adam_Static.IOValue] = AO_data[i];
                    }
                }
            }
        }

        #endregion

        #region Set
        //Signal IO
        public void SetDO(SocketCommand command, string Parameter, int DO_Index, string value)
        {
            if (DO_Index < DOCount)
            {
                CommandBQ.EnQueue(string.Format("{0},{1},{2},{3},{4}", command,
                                                                        Parameter,
                                                                        Adam_Static.SetDO,
                                                                        DO_Index,
                                                                        value));
            }

            DO_FlashList[DO_Index].DO_FlashFlag = false;
        }
        //Signal flash
        public void SetDO_Flash(SocketCommand command, string Parameter, int DO_Index)
        {
            if (DO_Index < DOCount)
            {
                CommandBQ.EnQueue(string.Format("{0},{1},{2},{3},{4}", command,
                                                                    Parameter,
                                                                    Adam_Static.SetDO_Flash,
                                                                    DO_Index,
                                                                    (DO_data[DO_Index] ? "0" : "1")));
                DO_FlashList[DO_Index].DO_FlashFlag = true;
            }
        }
        //Signal All
        public void SetDO_All(SocketCommand command, string Parameter)
        {
            double value = 0;
            int red = (int)Adam6050_DO.ST_Red % 6;
            int yellow = (int)Adam6050_DO.ST_Yellow % 6;
            int green = (int)Adam6050_DO.ST_Green % 6;
            //int blue = (int)Adam6050_DO.ST_Blue % 6;    //int.Parse(HCT_EFEM.ExcelAdmam[(int)Adam6050_DO.ST_Blue / 6][0][(int)Adam6050_DO. % 6, 2]);

            tmr1s.Enabled = false;

            for (int i = 0; i < DOCount; i++)
            {
                DO_FlashList[i].DO_FlashFlag = false;
                if (i == red || i == yellow || i == green)
                {
                    if (Parameter == SignalTown.AllOn.ToString())
                    {
                        value += Math.Pow(2, i);
                    }
                    else if (Parameter == SignalTown.AllFlash.ToString())
                    {
                        DO_FlashList[i].DO_FlashFlag = true;
                    }

                }
                else
                {
                    if (DO_data[i] == true)
                        value += Math.Pow(2, i);
                }
            }

            CommandBQ.EnQueue(string.Format("{0},{1},{2},{3}", command,
                                                                Parameter,
                                                                Adam_Static.SetDO_All,
                                                                value));
        }
        //FFU value
        public void SetAO(SocketCommand command, int AO_Index, double value)
        {
            //Command =  SetAO,AO_index,(0~4095)
            //Reply = SetAO,AO_index,(0~4095),(0/error message)
            if (AO_Index < AOCount)
            {
                if (value < 0)
                    value = 0;
                else if (value > 4095)
                    value = 4095;

                CommandBQ.EnQueue(string.Format("{0},{1},{2},{3},{4}", command,
                                                                      "",
                                                                      Adam_Static.SetAO,
                                                                      AO_Index,
                                                                      value));

            }

        }

        private void SetAiRange(int AI_Index, Adam6024_InputRange range)
        {

            CommandBQ.EnQueue(string.Format("{0},{1},{2},{3},{4}", SocketCommand.MaxCnt,
                                                                  "",
                                                                  Adam_Static.SetAIRange,
                                                                  AI_Index,
                                                                  range));
        }

        private void SetAoRange(int AI_Index, Adam6024_OutputRange range)
        {
            CommandBQ.EnQueue(string.Format("{0},{1},{2},{3},{4}", SocketCommand.MaxCnt,
                                                      "",
                                                      Adam_Static.SetAORange,
                                                      AI_Index,
                                                      range));

        }

        #endregion

        #region Get/Set

        public bool[] DI
        {
            get { return DI_data; }
        }

        public bool[] DO
        {
            get { return DO_data; }
        }

        public double[] AI
        {
            get
            {
                double[] OutputAI = new double[AICount];
                for (int i = 0; i < AICount; i++)
                {
                    if (AITypes[i] == Adam6024_InputRange.V_Neg10To10)
                        OutputAI[i] = (double)(AI_data[i] * 20.0 / 65535.0) - 10;
                    else if (AITypes[i] == Adam6024_InputRange.mA_0To20)
                        OutputAI[i] = (double)(AI_data[i] * 20.0 / 65535.0);
                    else
                        OutputAI[i] = (double)(AI_data[i] * 16.0 / 65535.0) + 4;
                }
                return OutputAI;
            }
        }

        public double[] AO
        {
            get
            {
                double[] OutputAO = new double[AOCount];
                for (int i = 0; i < AOCount; i++)
                {
                    if (AOTypes[i] == Adam6024_OutputRange.V_0To10)
                        OutputAO[i] = (double)(AO_data[i] * 10.0 / 4095.0);
                    else if (AOTypes[i] == Adam6024_OutputRange.mA_0To20)
                        OutputAO[i] = (double)(AO_data[i] * 20.0 / 4095.0);
                    else
                        OutputAO[i] = (double)(AO_data[i] * 16.0 / 4095.0) + 4;
                }
                return OutputAO;
            }
        }

        public bool Ui_Connect
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_Connect = value; }));
                    return;
                }
                labStatus.BackColor = (value ? Color.LightGreen : Color.Red);
                labStatus.Text = (value ? "Connect" : "DisConnect");
                gbxDo.Enabled = value;
                gbxAo.Enabled = value;
                ConnectStatus = value;
            }
            get { return ConnectStatus; }
        }

        #endregion

        #region UI

        private void btnDoSend_Click(object sender, EventArgs e)
        {
            if (cboDoValue.SelectedIndex == -1)
                return;

            switch (cboDoValue.Text)
            {
                case NormalStatic.On:
                case NormalStatic.Off:
                    {
                        SetDO(SocketCommand.MaxCnt, "", cboDo.SelectedIndex, cboDoValue.Text == NormalStatic.Off ? "0" : "1");
                    }
                    break;

                case NormalStatic.Flash:
                    {
                        SetDO_Flash(SocketCommand.MaxCnt, "", cboDo.SelectedIndex);
                    }
                    break;
            }
            UI.Operate(DeviceName, string.Format("DO:{0}:{1}", cboDo.SelectedIndex, cboDoValue.Text));
        }

        private void btnAoSend_Click(object sender, EventArgs e)
        {
            SetAO(SocketCommand.MaxCnt, cboAo.SelectedIndex, trkbAo.Value);
            UI.Operate(DeviceName, string.Format("AO:{0}:{1}", cboAo.SelectedIndex, labAoValue.Text));
        }

        private void trkbAo_Scroll(object sender, EventArgs e)
        {
            float fValue = Convert.ToSingle(trkbAo.Value);

            fValue = fValue * 10 / trkbAo.Maximum;

            labAoValue.Text = fValue.ToString("0.000");
        }

        #endregion

        #region Flash

        private void AdamTime1000ms_Tick(object sender, EventArgs e)
        {
            double value = 0;
            bool StopTick = true;
            for (int i = 0; i < DOCount; i++)
            {
                if (DO_FlashList[i].DO_FlashFlag == true)
                {
                    if (DO_data[i] == false)
                        value += Math.Pow(2, i);
                    StopTick = false;
                }
                else
                {
                    if (DO_data[i] == true)
                        value += Math.Pow(2, i);

                }
            }

            if (StopTick == true)
                this.Invoke((MethodInvoker)delegate () { tmr1s.Enabled = false; });
            else
                CommandBQ.EnQueue(string.Format("{0},{1},{2},{3}", SocketCommand.MaxCnt,
                                                             "",
                                                             Adam_Static.SetDO_AllFlash,
                                                             (int)value));

        }

        #endregion
    }
}
