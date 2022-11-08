using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Advantech.Motion;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;

namespace HirataMainControl
{
    public partial class IO_EtherCat : UserControl
    {

        #region DLL

        [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileStringA")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileSection")]
        private static extern int GetPrivateProfileSection(string section, byte[] buffer, int size, string filePath);
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileStringA")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath); 

        #endregion

        #region BG/Queue

        public BackgroundWorker ControlBG = new BackgroundWorker();
        private BackgroundWorker CheckEventBG = new BackgroundWorker();
        private BlockQueue<string> CommandQueue = new BlockQueue<string>(); 

        #endregion

        #region Variable

        private uint DeviceNumber;
        private IntPtr DeviceHandle;
        private uint DeviceErrorCode;
        private UInt32 DeviceEventStatus;
    
        private int DI_Count;
        private int DO_Count;
        private bool[,] DO_Data;
        private bool[,] DI_Data;
        private bool[,] DI_TempData;
        private bool[,] DO_TempData;
        private bool RefreshInitial = true;
        private uint RetryCount = 10;
        //private object EnQueueLock = new object();
        private bool Connect;
        private int LoopCount = 100; //0.1s

        #endregion

        #region Evnet/delegate

        public delegate void EthercatDiEvent(int channel, int point , bool value);
        public event EthercatDiEvent EventDiCallBack;
        public delegate void EthercatDoEvent(int channel, int point, bool value);
        public event EthercatDoEvent EventDoCallBack; 

        #endregion

        #region Initial

        public IO_EtherCat()
        {
            InitializeComponent();
        }

        public void Initial()
        {
            string IniType = AppSetting.LoadSetting(string.Format("{0}{1}{2}", NormalStatic.EtherCat, NormalStatic.UnderLine, "Type"), "0x00");
            DI_Count = Convert.ToInt32(AppSetting.LoadSetting(string.Format("{0}{1}{2}", NormalStatic.EtherCat, NormalStatic.UnderLine, "DI"), "0"));
            DO_Count = Convert.ToInt32(AppSetting.LoadSetting(string.Format("{0}{1}{2}", NormalStatic.EtherCat, NormalStatic.UnderLine, "DO"), "0"));
            uint DeviceType = Convert.ToUInt32(IniType, 16);
            Motion.mAcm_GetDevNum(DeviceType, (uint)0, ref DeviceNumber);
        
            DI_Data = new bool[DI_Count, 8];
            DO_Data = new bool[DO_Count, 8];
            DI_TempData = new bool[DI_Count, 8];
            DO_TempData = new bool[DO_Count, 8];
            ControlBG.DoWork += new DoWorkEventHandler(ControlDoWork);
            ControlBG.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.BG_Completed);

            CheckEventBG.DoWork += new DoWorkEventHandler(CheckEvnetDoWork);
        }

        #endregion

        #region Device

        private void OpenDevice()
        {
            //RetryCount = 3;
            //do
            //{
            //    DeviceErrorCode = Motion.mAcm_DevOpen(DeviceNumber, ref DeviceHandle);
            //    if (DeviceErrorCode != (uint)ErrorCode.SUCCESS)
            //    {
            //        Ui_Connect = false;
            //        RetryCount--;
            //        if (RetryCount == 0)
            //            break;

            //        System.Threading.Thread.Sleep(500);
            //    }
            //    else
            //    {
            //        Motion.mAcm_DevEnableEvent(DeviceHandle, 1);
            //        Ui_Connect = true;
            //        break;
            //    }

            //} while (RetryCount > 0);
            //uint DevEnableEvt = 0;
            RetryCount = 3;
            uint SlaveOnRing0 = 0, SlaveOnRing1 = 0;

            do
            {
                DeviceErrorCode = Motion.mAcm_DevOpen(DeviceNumber, ref DeviceHandle);
                RetryCount--;
                if (DeviceErrorCode == (uint)ErrorCode.SUCCESS)
                {
                    Motion.mAcm_GetU32Property(DeviceHandle, (uint)PropertyID.FT_MasCyclicCnt_R0, ref SlaveOnRing0);
                    Motion.mAcm_GetU32Property(DeviceHandle, (uint)PropertyID.FT_MasCyclicCnt_R1, ref SlaveOnRing1);
                }
                
                if (SlaveOnRing0 != 0 || SlaveOnRing1 != 8)
                {
                    Motion.mAcm_DevReOpen(DeviceHandle);
                    //Motion.mAcm_DevClose( ref DeviceHandle);
                   // System.Threading.Thread.Sleep(1000);
                }

                if (RetryCount == 0)
                {
                    Connect = false;
                    break;
                }

                System.Threading.Thread.Sleep(100);
            } while (DeviceErrorCode != 0 || SlaveOnRing0 != 0 || SlaveOnRing1 != 8);

            if (RetryCount != 0)
            {
                Connect = true;
                if (CheckEventBG.IsBusy == false)
                  CheckEventBG.RunWorkerAsync();
                Motion.mAcm_DevEnableEvent(DeviceHandle, 2);
            }
        }

  
        #endregion

        #region BG/Queue

        private void ControlDoWork(object sender, DoWorkEventArgs e)
        {
            string cmd;

            OpenDevice();

            if (Connect == false)
            {
                UI.InitialSystem(NormalStatic.EtherCat, NormalStatic.False, " link Fail");
            }
            else
            {
                UI.InitialSystem(NormalStatic.EtherCat, NormalStatic.True, NormalStatic.Space);
            }
    
            while (true)
            {
                cmd = CommandQueue.DeQueue(LoopCount);

                if (cmd != null)
                {
                    if (cmd == NormalStatic.End)
                        break;

                    string[] Received = cmd.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                    switch (Received[0])
                    {
                        case NormalStatic.SetBit:
                            {
                                Motion.mAcm_DaqDoSetBit(DeviceHandle, (ushort)((Convert.ToUInt16(Received[1]) * 8) + Convert.ToUInt16(Received[2])), Convert.ToByte(Received[3]));
                            }
                            break;

                        case "SetByte":
                            {
                                Motion.mAcm_DaqDoSetByte(DeviceHandle, (Convert.ToUInt16(Received[1])), Convert.ToByte(Received[2]));
                            }
                            break;

                        default:
                            break;
                    }
                }

                if (Connect)
                {
                    ReadDi();
                    ReadDo();
                    RefreshInitial = false;
                }
                else
                {
                    OpenDevice();
                    Thread.Sleep(1000);
                }
            }
        }

        private void CheckEvnetDoWork(object sender, DoWorkEventArgs e)
        {
            while (Connect)
            {
                DeviceErrorCode = Motion.mAcm_DevCheckEvent(DeviceHandle, ref DeviceEventStatus, int.MaxValue);
                if (DeviceErrorCode == (uint)ErrorCode.SUCCESS)
                {
                    if ((DeviceEventStatus & (uint)EventType.EVT_DEV_DISCONNET) > 0 || (DeviceEventStatus & (uint)EventType.EVT_DEV_IO_DISCONNET) > 0)
                    {
                        Connect = false;
                        UI.Alarm(NormalStatic.EtherCat, AlarmList.DeviceDisconnect);
                    }
                }
            }
        }

        private void BG_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            Connect = false;
            Motion.mAcm_DevClose(ref DeviceHandle);
            UI.CloseBG(NormalStatic.EtherCat);  
        }

        public void Close()
        {
            CommandQueue.EnQueue(NormalStatic.End);
        }

     

        #endregion

        #region Method

        public void Enqueue(string cmd)
        {
            CommandQueue.EnQueue(cmd);
        } 

        #endregion

        #region Read

        private void ReadDi()
        {
            for (int i = 0; i < DI_Count; i++)
            {
                byte DiReadTemp = 0;
                uint Result = Motion.mAcm_DaqDiGetByte(DeviceHandle, (ushort)i, ref DiReadTemp);

                if (Result == (uint)ErrorCode.SUCCESS)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        DI_Data[i, j] = ((DiReadTemp >> j) & 0x01) != 0;

                        if ((DI_Data[i, j] != DI_TempData[i, j]) || RefreshInitial)
                        {
                            EventDiCallBack(i, j, DI_Data[i, j]);
                            DI_TempData[i, j] = DI_Data[i, j];
                        }
                    }
                }
            }
        }

        private void ReadDo()
        {
            for (int i = 0; i < DO_Count; i++)
            {
                byte DoReadTemp = 0;
                uint Result = Motion.mAcm_DaqDoGetByte(DeviceHandle, (ushort)i, ref DoReadTemp);

                if (Result == (uint)ErrorCode.SUCCESS)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        DO_Data[i, j] = ((DoReadTemp >> j) & 0x01) != 0;

                        if ((DO_Data[i, j] != DO_TempData[i, j]) || RefreshInitial)
                        {
                            EventDoCallBack(i, j, DO_Data[i, j]);
                            DO_TempData[i, j] = DO_Data[i, j];
                        }
                    }
                }
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
                Connect = value;
            }
            get { return Connect; }
        }

        public bool[,] ReadDiData
        {
            get { return DI_Data; }
        }

        public bool[,] ReadDoData
        {
            get { return DO_Data; }
        }

        #endregion

    }
}
