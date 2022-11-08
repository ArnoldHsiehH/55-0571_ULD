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


namespace HirataMainControl
{
    public partial class HCT_Alignment : UserControl
    {
        IO_EtherCat EtherCat;

        #region Event/Delegate

        public delegate void AlignmentEvent(string PortName, AlignmentCommand command, bool Result);
        public event AlignmentEvent ActionComplete; 

        #endregion

        #region BG/Queue

        private BackgroundWorker ControlBG = new BackgroundWorker();
        private BlockQueue<AlignmentStep[]> CommandQueue = new BlockQueue<AlignmentStep[]>();
        private BlockQueue<string> ReceiverQueue = new BlockQueue<string>(); 

        #endregion

        #region Dictionary/List

        private List<AlignmentStep> Alignment_StepArray = new List<AlignmentStep>();
        private List<string[,]> ExcelDI;
        private List<string[,]> ExcelDo;
        private Dictionary<AlignmentDI, bool> ReceiverDiTableDictionay = new Dictionary<AlignmentDI, bool>();
        private Dictionary<AlignmentDO, bool> ReceiverDoTableDictionay = new Dictionary<AlignmentDO, bool>();

        #endregion

        #region Device

        public string DeviceName;

        private int WorkChannel;
        private int WorkPosition;
        private AlignmentCommand MainCmd = AlignmentCommand.MaxCnt;
        private int NowStepCnt = 0;

        private bool Busy;
        private int TimeoutCount = 6000; //6S
        private string WaferInfo = "";

        #endregion

        #region Small

        private bool SmallWaferPresence = false;
        private bool SmallClampStatus = false;

        private bool SmallClampLeft = false;
        private bool SmallClampRight = false; 

        #endregion

        #region Big

        private bool BigWaferPresence = false;
        private bool BigWaferPresence1 = false;
        private bool BigWaferPresence2 = false;

        private bool BigClampStatus = false;
        private bool BigClampLeft = false;
        private bool BigClampRight = false; 

        #endregion

        #region Initial

        public HCT_Alignment()
        {
            InitializeComponent();
        }

        public void Initial(ref int number, ref IO_EtherCat ethercat)
        {
            gbxAlignment.Text = DeviceName = string.Format("{0}{1}", NormalStatic.Alignment, number + 1);
            EtherCat = ethercat;

            HCT_EFEM.ReadExcel(string.Format("{0}{1}{2}{3}{4}{5}", NormalStatic.ExcelPath, NormalStatic.EtherCat, NormalStatic.DI, NormalStatic.UnderLine, DeviceName, ".xlsx"), ref ExcelDI);
            HCT_EFEM.ReadExcel(string.Format("{0}{1}{2}{3}{4}{5}", NormalStatic.ExcelPath, NormalStatic.EtherCat, NormalStatic.DO, NormalStatic.UnderLine, DeviceName, ".xlsx"), ref ExcelDo);

            ControlBG.DoWork += new DoWorkEventHandler(Control_DoWork);
            ControlBG.RunWorkerAsync();

            VCR_S = new IO_VCR();
            VCR_S.Initial(string.Format("{0}{1}", NormalStatic.VCR, "_S"));
            VCR_S.ActionComplete += new IO_VCR.VCREvnet(VCREventContol);
            VCR_B = new IO_VCR();
            VCR_B.Initial(string.Format("{0}{1}", NormalStatic.VCR, "_B"));
            VCR_B.ActionComplete += new IO_VCR.VCREvnet(VCREventContol);
        }

        #endregion

        #region BG/Queue

        private void Control_DoWork(object sender, DoWorkEventArgs e)
        {
            AlignmentStep[] MarcoCommand;
            string REC_string = "";
            while (true)
            {
                MarcoCommand = CommandQueue.DeQueue();

                if (MarcoCommand[NowStepCnt] == AlignmentStep.End)
                    break;

                while (NowStepCnt < MarcoCommand.Length)
                {
                    REC_string = "";
                    ReceiverQueue.Clear();        

                    if (MarcoCommand[NowStepCnt] == AlignmentStep.MaxCnt)
                    {
                        ActionComplete(DeviceName, MainCmd, true);
                        Ui_Busy = false;
                        break;
                    }

                    switch (MarcoCommand[NowStepCnt])
                    {
                        #region Big_Clamp

                        case AlignmentStep.BigCassetteClamp:
                            {
                                ReceiverDiTableDictionay.Add(AlignmentDI.BigCassetteLeftClamp, true);
                                ReceiverDiTableDictionay.Add(AlignmentDI.BigCassetteRightClamp, true);
                                GetDOInfo(AlignmentDO.BigCassetteUnClamp, out WorkChannel, out WorkPosition);
                                EtherCat.Enqueue(string.Format("{0},{1},{2},{3}",NormalStatic.SetBit, WorkChannel, WorkPosition, 0));
                                GetDOInfo(AlignmentDO.BigCassetteClamp, out WorkChannel, out WorkPosition);
                                EtherCat.Enqueue(string.Format("{0},{1},{2},{3}",NormalStatic.SetBit, WorkChannel, WorkPosition, 1));
                            }
                            break;

                        case AlignmentStep.BigCassetteClampStop:
                            {
                                ReceiverDoTableDictionay.Add(AlignmentDO.BigCassetteClamp, false);
                                GetDOInfo(AlignmentDO.BigCassetteUnClamp, out WorkChannel, out WorkPosition);
                                EtherCat.Enqueue(string.Format("{0},{1},{2},{3}",NormalStatic.SetBit, WorkChannel, WorkPosition, 0));
                                GetDOInfo(AlignmentDO.BigCassetteClamp, out WorkChannel, out WorkPosition);
                                EtherCat.Enqueue(string.Format("{0},{1},{2},{3}",NormalStatic.SetBit, WorkChannel, WorkPosition, 0));
                            }
                            break;

                        #endregion

                        #region Big_UnClamp

                        case AlignmentStep.BigCassetteUnClamp:
                            {
                                ReceiverDiTableDictionay.Add(AlignmentDI.BigCassetteLeftUnClamp, true);
                                ReceiverDiTableDictionay.Add(AlignmentDI.BigCassetteRightUnClamp, true);
                                GetDOInfo(AlignmentDO.BigCassetteClamp, out WorkChannel, out WorkPosition);
                                EtherCat.Enqueue(string.Format("{0},{1},{2},{3}",NormalStatic.SetBit, WorkChannel, WorkPosition, 0));
                                GetDOInfo(AlignmentDO.BigCassetteUnClamp, out WorkChannel, out WorkPosition);
                                EtherCat.Enqueue(string.Format("{0},{1},{2},{3}",NormalStatic.SetBit, WorkChannel, WorkPosition, 1));                            
                            }
                            break;

                        case AlignmentStep.BigCassetteUnClampStop:
                            {
                                ReceiverDoTableDictionay.Add(AlignmentDO.BigCassetteUnClamp, false);
                                GetDOInfo(AlignmentDO.BigCassetteUnClamp, out WorkChannel, out WorkPosition);
                                EtherCat.Enqueue(string.Format("{0},{1},{2},{3}",NormalStatic.SetBit, WorkChannel, WorkPosition, 0));
                                GetDOInfo(AlignmentDO.BigCassetteClamp, out WorkChannel, out WorkPosition);
                                EtherCat.Enqueue(string.Format("{0},{1},{2},{3}",NormalStatic.SetBit, WorkChannel, WorkPosition, 0));          
                            }
                            break;

                        #endregion

                        #region Small_Clamp

                        case AlignmentStep.SmallCassetteClamp:
                            {
                                ReceiverDiTableDictionay.Add(AlignmentDI.SmallCassetteLeftClamp, true);
                                ReceiverDiTableDictionay.Add(AlignmentDI.SmallCassetteRightClamp, true);
                                GetDOInfo(AlignmentDO.SmallCassetteUnClamp, out WorkChannel, out WorkPosition);
                                EtherCat.Enqueue(string.Format("{0},{1},{2},{3}",NormalStatic.SetBit, WorkChannel, WorkPosition, 0));
                                GetDOInfo(AlignmentDO.SmallCassetteClamp, out WorkChannel, out WorkPosition);
                                EtherCat.Enqueue(string.Format("{0},{1},{2},{3}",NormalStatic.SetBit, WorkChannel, WorkPosition, 1)); 
                            }
                            break;

                        case AlignmentStep.SmallCassetteClampStop:
                            {
                                ReceiverDoTableDictionay.Add(AlignmentDO.SmallCassetteClamp, false);
                                GetDOInfo(AlignmentDO.SmallCassetteUnClamp, out WorkChannel, out WorkPosition);
                                EtherCat.Enqueue(string.Format("{0},{1},{2},{3}",NormalStatic.SetBit, WorkChannel, WorkPosition, 0));
                                GetDOInfo(AlignmentDO.SmallCassetteClamp, out WorkChannel, out WorkPosition);
                                EtherCat.Enqueue(string.Format("{0},{1},{2},{3}",NormalStatic.SetBit, WorkChannel, WorkPosition, 0));
                            }
                            break;

                        #endregion

                        #region Small_UnClamp

                        case AlignmentStep.SmallCassetteUnClamp:
                            {
                                ReceiverDiTableDictionay.Add(AlignmentDI.SmallCassetteLeftUnClamp, true);
                                ReceiverDiTableDictionay.Add(AlignmentDI.SmallCassetteRightUnClamp, true);
                                GetDOInfo(AlignmentDO.SmallCassetteClamp, out WorkChannel, out WorkPosition);
                                EtherCat.Enqueue(string.Format("{0},{1},{2},{3}",NormalStatic.SetBit, WorkChannel, WorkPosition, 0));
                                GetDOInfo(AlignmentDO.SmallCassetteUnClamp, out WorkChannel, out WorkPosition);
                                EtherCat.Enqueue(string.Format("{0},{1},{2},{3}",NormalStatic.SetBit, WorkChannel, WorkPosition, 1));
                            }
                            break;

                        case AlignmentStep.SmallCassetteUnClampStop:
                            {
                                ReceiverDoTableDictionay.Add(AlignmentDO.SmallCassetteUnClamp, false);
                                GetDOInfo(AlignmentDO.SmallCassetteUnClamp, out WorkChannel, out WorkPosition);
                                EtherCat.Enqueue(string.Format("{0},{1},{2},{3}",NormalStatic.SetBit, WorkChannel, WorkPosition, 0));
                                GetDOInfo(AlignmentDO.SmallCassetteClamp, out WorkChannel, out WorkPosition);
                                EtherCat.Enqueue(string.Format("{0},{1},{2},{3}",NormalStatic.SetBit, WorkChannel, WorkPosition, 0));
                            }
                            break;

                        #endregion

                        #region Normal

                        case AlignmentStep.Sleep1000mS:
                            {
                                Thread.Sleep(1000);
                                ReceiverQueue.EnQueue(NormalStatic.True);
                            }
                            break;

                        case AlignmentStep.ResetError:
                            {
                                Ui_Busy = false;
                                ReceiverQueue.EnQueue(NormalStatic.True);
                            }
                            break; 

                        #endregion

                        #region VCR

                        case AlignmentStep.ResetVCR:
                            {
                                VCR_S.Cmd_EnQueue(VCRCommand.ResetError);
                                VCR_B.Cmd_EnQueue(VCRCommand.ResetError);
                            }
                            break;

                        case AlignmentStep.SmallCassetteReadVCR:
                            {
                                VCR_S.Cmd_EnQueue(VCRCommand.Read);
                            }
                            break;

                        case AlignmentStep.BigCassetteReadVCR:
                            {
                                VCR_B.Cmd_EnQueue(VCRCommand.Read);
                            }
                            break;
         

                        #endregion
                    }

                    REC_string = ReceiverQueue.DeQueue(TimeoutCount); 

                    if (REC_string == null)
                    {
                        UI.Alarm(DeviceName, AlarmList.Timeout, MarcoCommand[NowStepCnt].ToString());
                        JobFail();
                    }
                    else if (REC_string == NormalStatic.End)
                    {
                        break;
                    }
                    else if (REC_string == NormalStatic.True)
                    {
                        NowStepCnt++;
                    }
                }
               
            }
        }

        private void ResetAlignmnentStatus()
        {
            Alignment_StepArray.Clear();
            CommandQueue.Clear();
            ReceiverQueue.Clear();
            NowStepCnt = 100;
            ReceiverDoTableDictionay.Clear();
            ReceiverDiTableDictionay.Clear();
        }

        private void JobFail()
        {
            ResetAlignmnentStatus();
        }

        public void Close()
        {
            JobFail();
            NowStepCnt = 0;
            CommandQueue.EnQueue(new AlignmentStep[] { AlignmentStep.End });
            ReceiverQueue.EnQueue(NormalStatic.End);
            VCR_S.Close();
            VCR_B.Close();
        }

        public void Cmd_EnQueue(AlignmentCommand command)
        {
            UI.System(DeviceName, SystemList.CommandStart, command.ToString());
            Ui_Busy = true;
            NowStepCnt = 0;
            MainCmd = command;
            Alignment_StepArray.Clear();
            ReceiverQueue.Clear();
            ReceiverDoTableDictionay.Clear();
            ReceiverDiTableDictionay.Clear();

            switch (command)
            {
                case AlignmentCommand.Alignment_InitialHome:
                case AlignmentCommand.Alignment_Home:
                   {
                       if (command == AlignmentCommand.Alignment_InitialHome)
                       {
                           Alignment_StepArray.Add(AlignmentStep.ResetError);
                           //Alignment_StepArray.Add(AlignmentStep.ResetVCR);//Mike
                       }

                        if (BigClampStatus)
                        {
                            Alignment_StepArray.Add(AlignmentStep.BigCassetteUnClamp);
                            Alignment_StepArray.Add(AlignmentStep.BigCassetteUnClampStop);
                        }

                        if(SmallClampStatus)
                        {
                            Alignment_StepArray.Add(AlignmentStep.SmallCassetteUnClamp);
                            Alignment_StepArray.Add(AlignmentStep.SmallCassetteUnClampStop);
                        }
                    }
                    break;

                case AlignmentCommand.BigCassetteAlignment:
                    {
                        if (BigClampStatus == false)
                        {
                            Alignment_StepArray.Add(AlignmentStep.BigCassetteClamp);
                            Alignment_StepArray.Add(AlignmentStep.BigCassetteClampStop);
                        }
                        Alignment_StepArray.Add(AlignmentStep.Sleep1000mS);
                        Alignment_StepArray.Add(AlignmentStep.BigCassetteUnClamp);
                        Alignment_StepArray.Add(AlignmentStep.BigCassetteUnClampStop);
                        //Alignment_StepArray.Add(AlignmentStep.BigCassetteReadVCR);Wayne
                    }
                    break;

                case AlignmentCommand.BigCassetteClamp:
                    {
                        if (BigClampStatus == false)
                        {
                            Alignment_StepArray.Add(AlignmentStep.BigCassetteClamp);
                            Alignment_StepArray.Add(AlignmentStep.BigCassetteClampStop);
                        }
                    }
                    break;

                case AlignmentCommand.BigCassetteUnClamp:
                    {
                        if (BigClampStatus == true)
                        {
                            Alignment_StepArray.Add(AlignmentStep.BigCassetteUnClamp);
                            Alignment_StepArray.Add(AlignmentStep.BigCassetteUnClampStop);
                        }
                    }
                    break;

                case AlignmentCommand.SmallCassetteAlignment:
                    {
                        if (SmallClampStatus == false)
                        {
                            Alignment_StepArray.Add(AlignmentStep.SmallCassetteClamp);
                            Alignment_StepArray.Add(AlignmentStep.SmallCassetteClampStop);
                        }
                        Alignment_StepArray.Add(AlignmentStep.Sleep1000mS);
                        Alignment_StepArray.Add(AlignmentStep.SmallCassetteUnClamp);
                        Alignment_StepArray.Add(AlignmentStep.SmallCassetteUnClampStop);
                        //Alignment_StepArray.Add(AlignmentStep.SmallCassetteReadVCR);  //Mike
                    }
                    break;

                case AlignmentCommand.SmallCassetteClamp:
                    {
                        if (SmallClampStatus == false)
                        {
                            Alignment_StepArray.Add(AlignmentStep.SmallCassetteClamp);
                            Alignment_StepArray.Add(AlignmentStep.SmallCassetteClampStop);
                        }
                    }
                    break;

                case AlignmentCommand.SmallCassetteUnClamp:
                    {
                        if (SmallClampStatus == true)
                        {
                            Alignment_StepArray.Add(AlignmentStep.SmallCassetteUnClamp);
                            Alignment_StepArray.Add(AlignmentStep.SmallCassetteUnClampStop);
                        }
                    }
                    break;

                case AlignmentCommand.Alignment_ResetError:
                    {
                        Alignment_StepArray.Add(AlignmentStep.ResetError);
                        //Alignment_StepArray.Add(AlignmentStep.ResetVCR);//Mike
                    }
                    break;
            }

            Alignment_StepArray.Add(AlignmentStep.MaxCnt);
            CommandQueue.EnQueue(Alignment_StepArray.ToArray<AlignmentStep>());

        }

        #endregion

        #region Get Do

        private void GetDOInfo(AlignmentDO ref_DO, out int WorkChannel, out int WorkPosition)
        {
            WorkChannel = -1;
            WorkPosition = -1;

            WorkChannel = Convert.ToInt32(ExcelDI[0][(int)ref_DO, 2]);
            WorkPosition = Convert.ToInt32(ExcelDI[0][(int)ref_DO, 3]);
            
        }

        #endregion

        #region Check

        public void CheckDiReceiveTable(ref AlignmentDI pos, ref bool value)
        {
            if (ReceiverDiTableDictionay.Count > 0)
            {
                if (ReceiverDiTableDictionay.ContainsKey(pos) && ReceiverDiTableDictionay[pos] == value)
                {
                    ReceiverDiTableDictionay.Remove(pos);

                    if (ReceiverDiTableDictionay.Count == 0)
                        ReceiverQueue.EnQueue(NormalStatic.True);
                }
            }
        }

        public void CheckDoReceiveTable(ref AlignmentDO pos, ref bool value)
        {
            if (ReceiverDoTableDictionay.Count > 0)
            {
                if (ReceiverDoTableDictionay.ContainsKey(pos) && ReceiverDoTableDictionay[pos] == value)
                {
                    ReceiverDoTableDictionay.Remove(pos);

                    if (ReceiverDoTableDictionay.Count == 0)
                        ReceiverQueue.EnQueue(NormalStatic.True);
                }
            }
        }

        #endregion

        #region Event

        public void SmallPresenceEvent(ref bool value)
        {
            Ui_SmallWaferInfo = value;
        }

        public void BigPresenceEvent(ref AlignmentDI pos , ref bool value)
        {
            switch (pos)
            {
                case AlignmentDI.BigCassettePresence1:
                    BigWaferPresence1 = value;
                    break;
                case AlignmentDI.BigCassettePresence2:
                    BigWaferPresence2 = value;
                    break;
            }
 
            if (BigWaferPresence1 == true && BigWaferPresence2 == true)
            {
                Ui_BigWaferInfo = true;
            }
            else
            {
                Ui_BigWaferInfo = false;
            }
        }

        public void BigClampEvent(ref AlignmentDI pos, ref bool value)
        {
            switch (pos)
            {
                case AlignmentDI.BigCassetteLeftClamp:
                    BigClampLeft = value;
                    break;
                case AlignmentDI.BigCassetteLeftUnClamp:
                    BigClampLeft = !value;
                    break;
                case AlignmentDI.BigCassetteRightClamp:
                    BigClampRight = value;
                    break;
                case AlignmentDI.BigCassetteRightUnClamp:
                    BigClampRight = !value;
                    break;
            }

            if (BigClampLeft == true && BigClampRight == true && Ui_BigClamp == false)
            {
                Ui_BigClamp = true;
            }
            else if (BigClampLeft == false && BigClampRight == false && Ui_BigClamp == true)
            {
                Ui_BigClamp = false;
            }
        }

        public void SmallClampEvent(ref AlignmentDI pos, ref bool value)
        {
            switch (pos)
            {
                case AlignmentDI.SmallCassetteLeftClamp:
                    SmallClampLeft = value;
                    break;
                case AlignmentDI.SmallCassetteLeftUnClamp:
                    SmallClampLeft = !value;
                    break;
                case AlignmentDI.SmallCassetteRightClamp:
                    SmallClampRight = value;
                    break;
                case AlignmentDI.SmallCassetteRightUnClamp:
                    SmallClampRight = !value;
                    break;
            }

            if (SmallClampLeft == true && SmallClampRight == true && Ui_SmallClamp == false)
            {
                Ui_SmallClamp = true;
            }
            else if (SmallClampLeft == false && SmallClampRight == false && Ui_SmallClamp == true)
            {
                Ui_SmallClamp = false;
            }
        } 

        #endregion

        #region Get/Set

        public bool Ui_BigWaferInfo
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate() { Ui_BigWaferInfo = value; }));
                    return;
                }

                picBigAlignment.Image = (value ? Properties.Resources.BigAlignment_With : Properties.Resources.BigAlignment_WithOut);

                BigWaferPresence = value;
            }
            get { return BigWaferPresence; }
        }

        public bool Ui_SmallWaferInfo
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate() { Ui_SmallWaferInfo = value; }));
                    return;
                }

                picSmallAlignment.Image = (value ? Properties.Resources.SmallAlignment_With : Properties.Resources.SmallAlignment_WithOut);

                SmallWaferPresence = value;
            }
            get { return SmallWaferPresence; }
        }

        public bool Ui_Busy
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate() { Ui_Busy = value; }));
                    return;
                }
                labBusy.BackColor = value ? Color.Yellow : Color.LightGreen;
                labBusy.Text = value ? "Busy" : "Idle";
                Busy = value;
            }
            get { return Busy; }
        }

        public bool Ui_SmallClamp
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate() { Ui_SmallClamp = value; }));
                    return;
                }
                labSmallClamp.BackColor = value ? Color.Red : Color.LightGreen;
                labSmallClamp.Text = value ? "Clamp" : "UnClamp";
                SmallClampStatus = value;
            }
            get { return SmallClampStatus; }
        }

        public bool Ui_BigClamp
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate() { Ui_BigClamp = value; }));
                    return;
                }
                labBigClamp.BackColor = value ? Color.Red : Color.LightGreen;
                labBigClamp.Text = value ? "Clamp" : "UnClamp";
                BigClampStatus = value;
            }
            get { return BigClampStatus; }
        }

        public string Ui_WaferInfo
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate() { Ui_WaferInfo = value; }));
                    return;
                }
                if (value == "")
                {
                    if (frmMain.EFEM_Type == IOLPType.SmallCassette)
                        VCR_S.Ui_ID = "";
                    else
                        VCR_B.Ui_ID = "";
                }
                labWaferInfo.Text = value;
                WaferInfo = value;
            }
            get { return WaferInfo; }
        }

        #endregion

        #region VCR

        private void VCREventContol(string PortName, VCRCommand command, bool Result)
        {
            if (Result)
            {
                ReceiverQueue.EnQueue("OK");
            }
            else
            {
                ReceiverQueue.EnQueue("");
            }
        }

        #endregion
    }
}
