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

namespace HirataMainControl
{
    public partial class Robot1 : UserControl
    {

        #region Event_Delegate

        public delegate void RobotEvent(string ReplyMessage, bool Result, string deviceName);
        public event RobotEvent ActionComplete;

        //   public delegate void MappingDataEvent(SocketCommand command, string ReplyMessage, bool Normal, string dest);
        //   public event MappingDataEvent MappingDataComplete;

        #endregion

        #region BG_Queue

        private Thread AutoMoveBG;
        private Thread CommandBG;

        private BlockQueue<string> CommandQueue = new BlockQueue<string>();
        private BlockQueue<string> ReceiverQueue = new BlockQueue<string>();
        private BlockQueue<RobotStep[]> AutoStepQueue = new BlockQueue<RobotStep[]>();
        private BlockQueue<string> StepRecQueue = new BlockQueue<string>();

        #endregion

        #region Dictionary/List

        private List<RobotStep> Robot_GPTArray = new List<RobotStep>();
        private List<byte> ReceiveTemp = new List<byte>();
        private Dictionary<string, int> Other_StartAddressDictionay = new Dictionary<string, int>();
        private Dictionary<string, string[]> LP_StartAddressDictionay = new Dictionary<string, string[]>();
        private Dictionary<string, string> Axis_PosAddressDictionay = new Dictionary<string, string>();

        #endregion

        #region Variable
        //=====Robot狀態Flag=====
        private bool Moving = false;                                       //這是機台回報Status內的參數 4401[17]
        private bool Stop = false;                                         //這是機台回報Status內的參數 4401[9]
        private bool Check_DI = false;
        private bool Check_PLC = false;
        public string Check_PLC_ReceiveIsOK = "";
        private bool CheckFlagIsWith = false;
        private DateTime Chech_NowTime;
        private int TimeoutCount = 10000;//10000;//500000; //10S
        //RS232
        private SerialPort RobotCOM;
        private string RobotName = NormalStatic.Robot;
        private string CMD_string = "";
        private string REC_string = "";
        private int ReceiveNowStepCnt = NormalStatic.Idle;
        private string ReplyMsg = "";

        //Ini
        private int Speed = 10;                                             //Now Robot Speed
        private string[] PosAddressMap;
        private int AddressMapCnt = 0;
        private int IniRetryCount = 0;
        public bool IniMappingSup = false;
        public bool IniTopSup = false;
        public bool IniStopRestart = false;
        public int IniArmCnt;                                              //Now Robot Arm Cnt
        //Arm
        public ArmType[] IniUseArmType;                                    //Ini Use Arm Type
        private ArmType[,] IniArmOutType;                                   //ini Out Type
        private int[,] IniArmOutOn;                                         //ini Out_On
        private int[,] IniArmOutOff;                                        //Ini Out_Off
        private int[] IniArmInCnt;                                          //Ini In_Cnt
        private int[,] IniArmInCntBit;                                      //Ini In_Bit
        private int[,] IniArmInFlag;                                        //Ini In_Flag

        //Now
        private int NowUseArm = 0;                                          //Now Use Arm
        private string NowUseObj = "Unknown";                               //Now Robot Object
        private int NowUseSlotID = 1;                                       //Now Robot SlotGP
        private double NowUseSlotGap = 10;                                    //Now Robot SlotGP   
        public string[] RobotNodeAxisPos = new string[(int)RobotAxis.MaxCnt]; //Now Robot Axis Pos
        private string[] RobotMoveAxisPos = new string[(int)RobotAxis.MaxCnt];//Now Robot Axis Pos
        private string[] RobotBackGPAxisPos = new string[(int)RobotAxis.MaxCnt];  //Home Back Robot GP Pos
        private string[] RobotHomeAxisPos = new string[(int)RobotAxis.MaxCnt];//Now Robot Axis Pos
        private int NowGPRobotAddress = 0;                                    //base + step = Now Robot address   
        private int NowGPBaseAddress = 0;                                      //Robot base address
        private int NowTempHomeBackAddress = 0;                               //Home  back temp address
        public string NowErrorMsg = "";
        public ErrorList NowErrorList = ErrorList.MaxCnt;
        private bool NowStopAction = false;
        private int NowPLCInvasionStart = 0;
        //Robot
        private RobotStep[] MarcoCommand;
        private SocketCommand NowMainJab = SocketCommand.MaxCnt;              //Robot main Command 
        private SocketCommand NowResumeJab = SocketCommand.MaxCnt;            //Robot Resume  Command
        private int NowStepCnt = 0;                                           //Robot new Step cnt
        private int NowStepGPCnt = 0;
        private bool NextStepFlag = false;
        public RobotGPT NowGPTCommandType = RobotGPT.MaxCnt;
        public bool NowNeedHomeChangeData = false;
        public string DeviceName;
        private All_Device DeviceNameEnum = All_Device.Fn;

        private bool RetryFlag = false;                                     //Get Retry Flag
        private int RetryCnt = 0;                                           //Ini Retry Count
        private string TempDi_1;                                            //Di_0_Temp
        private string TempDi_2;                                            //Di_1_Temp
        private string TempDi_All;                                          //Di_1 + Di_0  //32 bit 

        //ArmStatus
        private WaferStatus LowerWaferPresent;
        private WaferStatus UpperWaferPresent;
        private string LowerWaferInfo;
        private string UpperWaferInfo;
        private RobotPosition NowRobotPos;                                  //Now Robot Pos
        private ArmStatus NowArmStatusX;                                    //Now Robot Lower Status X
        private ArmStatus NowArmStatusR;                                    //Now Robot Lower Status R
        private ArmStatus NowArmStatusY;                                    //Now Robot Upper Status

        // RobotMapping  
        private string MappPresent;
        private string MappError;

        #endregion

        #region Enum_GPT

        #region Normal_GP

        private RobotStep[] GP_0 ={
                                      RobotStep.GP_Normal_0,
                                      RobotStep.Update_RobotPosition,
                                      };
        private RobotStep[] GP_1 ={
                                      RobotStep.GP_Normal_1,
                                      RobotStep.Update_RobotPosition,
                                      };
        private RobotStep[] GP_2 ={
                                      RobotStep.GP_Normal_2,
                                      RobotStep.Update_RobotPosition,
                                      };
        private RobotStep[] GP_3 ={
                                      RobotStep.GP_Normal_3,
                                      RobotStep.Update_RobotPosition,
                                      };
        private RobotStep[] GP_4 ={
                                      RobotStep.GP_Normal_4,
                                      RobotStep.Update_RobotPosition,
                                      };
        private RobotStep[] GP_5 ={
                                      RobotStep.GP_Normal_5,
                                      RobotStep.Update_RobotPosition,
                                      };
        private RobotStep[] GP_6 ={
                                      RobotStep.GP_Normal_6,
                                      RobotStep.Update_RobotPosition,
                                      };
        private RobotStep[] GP_7 ={
                                      RobotStep.GP_Normal_7,
                                      RobotStep.Update_RobotPosition,
                                      };
        private RobotStep[] GP_8 ={
                                      RobotStep.GP_Normal_8,
                                      RobotStep.Update_RobotPosition,
                                      };
        private RobotStep[] GP_9 ={
                                      RobotStep.GP_Normal_9,
                                      RobotStep.Update_RobotPosition,
                                      };

        #endregion
        #region EQ_LD
        private RobotStep[] EQ_LDstart = {
                                          RobotStep.EQ_LD_start,
                                          RobotStep.Update_RobotPosition,
                                         };
        private RobotStep[] EQ_LDend = {
                                          RobotStep.EQ_LD_End,
                                          RobotStep.Update_RobotPosition,
                                         };

        private RobotStep[] EQ_ULDstart =
    {
                                          RobotStep.EQ_ULD_start,
                                          RobotStep.Update_RobotPosition,
         };
        private RobotStep[] EQ_ULDend =
                    {
                                          RobotStep.EQ_ULD_End,
                                          RobotStep.Update_RobotPosition,
         };

        #endregion
        #region Top_GP

        private RobotStep[] GP_Top0 ={
                                      RobotStep.GP_Top_0,
                                      RobotStep.Update_RobotPosition,
                                      };
        private RobotStep[] GP_Top1 ={
                                      RobotStep.GP_Top_1,
                                      RobotStep.Update_RobotPosition,
                                      };
        private RobotStep[] GP_Top2 ={
                                      RobotStep.GP_Top_2,
                                      RobotStep.Update_RobotPosition,
                                      };
        private RobotStep[] GP_Top3 ={
                                      RobotStep.GP_Top_3,
                                      RobotStep.Update_RobotPosition,
                                      };
        private RobotStep[] GP_Top4 ={
                                      RobotStep.GP_Top_4,
                                      RobotStep.Update_RobotPosition,
                                      };
        private RobotStep[] GP_Top5 ={
                                      RobotStep.GP_Top_5,
                                      RobotStep.Update_RobotPosition,
                                      };
        private RobotStep[] GP_Top6 ={
                                      RobotStep.GP_Top_6,
                                      RobotStep.Update_RobotPosition,
                                      };
        private RobotStep[] GP_Top7 ={
                                      RobotStep.GP_Top_7,
                                      RobotStep.Update_RobotPosition,
                                      };
        private RobotStep[] GP_Top8 ={
                                      RobotStep.GP_Top_8,
                                      RobotStep.Update_RobotPosition,
                                      };
        private RobotStep[] GP_Top9 ={
                                      RobotStep.GP_Top_9,
                                      RobotStep.Update_RobotPosition,
                                      };

        #endregion

        #region Turn_GP

        private RobotStep[] Turn_0 ={
                                      RobotStep.GP_Turn_0,
                                      RobotStep.Update_RobotPosition,
                                      };
        private RobotStep[] Turn_1 ={
                                      RobotStep.GP_Turn_1,
                                      RobotStep.Update_RobotPosition,
                                      };
        private RobotStep[] Turn_2 ={
                                      RobotStep.GP_Turn_2,
                                      RobotStep.Update_RobotPosition,
                                      };
        private RobotStep[] Turn_3 ={
                                      RobotStep.GP_Turn_3,
                                      RobotStep.Update_RobotPosition,
                                      };

        #endregion

        #region Turn_Back_GP

        private RobotStep[] TurnBack_0 ={
                                      RobotStep.GP_TurnBack_0,
                                      RobotStep.Update_RobotPosition,
                                      };
        private RobotStep[] TurnBack_1 ={
                                      RobotStep.GP_TurnBack_1,
                                      RobotStep.Update_RobotPosition,
                                      };
        private RobotStep[] TurnBack_2 ={
                                      RobotStep.GP_TurnBack_2,
                                      RobotStep.Update_RobotPosition,
                                      };
        private RobotStep[] TurnBack_3 ={
                                      RobotStep.GP_TurnBack_3,
                                      RobotStep.Update_RobotPosition,
                                      };

        #endregion

        #region Mapping_GP

        private RobotStep[] Mapping_0 ={
                                      RobotStep.GP_Mapping_0,
                                      RobotStep.Update_RobotPosition,
                                      };
        private RobotStep[] Mapping_1 ={
                                      RobotStep.GP_Mapping_1,
                                       RobotStep.Update_RobotPosition,
                                      };
        private RobotStep[] Mapping_2 ={
                                      RobotStep.GP_Mapping_2,
                                       RobotStep.Update_RobotPosition,
                                      };
        private RobotStep[] Mapping_3 ={
                                      RobotStep.GP_Mapping_3,
                                      RobotStep.Update_RobotPosition,
                                      };
        #endregion

        #region Get_DI

        private RobotStep[] Get_DI ={
                                      RobotStep.Get_DI_0,
                                      RobotStep.Get_DI_1,
                                      };

        #endregion

        #region Set_Speed

        private RobotStep[] Set_Speed ={
                                RobotStep.Set_Speed,
                                RobotStep.Update_RobotPosition,
                                };

        #endregion

        #region Get_Position

        private RobotStep[] Get_Position_Step ={
                                RobotStep.Update_RobotPosition,
                                };

        #endregion

        #region Step_End

        private RobotStep[] End_Step ={
                                      RobotStep.MaxCnt,
                                      };

        #endregion

        #region Set_Vac

        private RobotStep[] Set_VacOn ={
                                RobotStep.Get_Vac,
                                RobotStep.Set_VacOn,
                                RobotStep.Get_Vac,
                                RobotStep.Check_VacOn,
                                };

        private RobotStep[] Set_VacOff ={
                                RobotStep.Get_Vac,
                                RobotStep.Set_VacOff,
                                RobotStep.Get_Vac,
                                RobotStep.Check_VacOff,
                                };

        private RobotStep[] Check_LowerBernoulli ={
                                RobotStep.SetLowerArm,
                                RobotStep.Get_Bernoulli,
                                RobotStep.Set_BernoulliOn,
                                RobotStep.Get_Bernoulli,
                                RobotStep.Check_BernoulliOn,
                                RobotStep.Wait_BernoulliOn,
                                RobotStep.Get_DI_0,
                                RobotStep.Get_DI_1,
                                RobotStep.Update_ArmPresence,
                                RobotStep.Check_ArmWith_Jump12,
                                RobotStep.Get_Bernoulli,
                                RobotStep.Set_BernoulliOff,
                                RobotStep.Get_Bernoulli,
                                RobotStep.Check_BernoulliOff,
                                RobotStep.Wait_BernoulliOff,
                                RobotStep.Jump_Check1,
                                };

        private RobotStep[] Check_UpperBernoulli ={
                                RobotStep.SetUpperArm,
                                RobotStep.Get_Bernoulli,
                                RobotStep.Set_VacOn,
                                RobotStep.Get_Bernoulli,
                                RobotStep.Check_BernoulliOn,
                                RobotStep.Wait_BernoulliOn,
                                RobotStep.Get_DI_0,
                                RobotStep.Get_DI_1,
                                RobotStep.Update_ArmPresence,
                                RobotStep.Check_ArmWith_Jump12,
                                RobotStep.Get_Bernoulli,
                                RobotStep.Set_BernoulliOff,
                                RobotStep.Get_Bernoulli,
                                RobotStep.Check_BernoulliOff,
                                RobotStep.Wait_BernoulliOff,
                                RobotStep.Jump_Check2,
                                };

        private RobotStep[] Check_LowerEdgeGrip ={
                                RobotStep.SetLowerArm,
                                RobotStep.Get_Edge,
                                RobotStep.Set_EdgeOn,
                                RobotStep.Wait_VacOn,
                                RobotStep.Get_Edge,
                                RobotStep.Check_EdgeOn,
                                RobotStep.Get_DI_0,
                                RobotStep.Get_DI_1,
                                RobotStep.Update_ArmPresence,
                                RobotStep.Check_ArmWith_Jump12,
                                RobotStep.Get_Edge,
                                RobotStep.Set_EdgeOff,
                                RobotStep.Wait_VacOff,
                                RobotStep.Get_Edge,
                                RobotStep.Check_EdgeOff,
                                RobotStep.Jump_Check1,
                                };

        private RobotStep[] Check_UpperEdgeGrip ={
                                RobotStep.SetUpperArm,
                                RobotStep.Get_Edge,
                                RobotStep.Set_EdgeOn,
                                RobotStep.Wait_VacOn,
                                RobotStep.Get_Edge,
                                RobotStep.Check_EdgeOn,
                                RobotStep.Get_DI_0,
                                RobotStep.Get_DI_1,
                                RobotStep.Update_ArmPresence,
                                RobotStep.Check_ArmWith_Jump12,
                                RobotStep.Get_Edge,
                                RobotStep.Set_EdgeOff,
                                RobotStep.Wait_VacOff,
                                RobotStep.Get_Edge,
                                RobotStep.Check_EdgeOff,
                                RobotStep.Jump_Check2,
                                };
        #endregion

        #region Set_Edge

        private RobotStep[] Set_EdgeOn ={
                                RobotStep.Get_Edge,
                                RobotStep.Set_EdgeOn,
                                RobotStep.Wait_VacOn,
                                RobotStep.Get_Edge,
                                RobotStep.Check_EdgeOn,

                                };

        private RobotStep[] Set_EdgeOff ={
                                RobotStep.Get_Edge,
                                RobotStep.Set_EdgeOff,
                                RobotStep.Wait_VacOff,
                                RobotStep.Get_Edge,
                                RobotStep.Check_EdgeOff,
                                };

        #endregion

        #region Set_Bernoulli

        private RobotStep[] Set_BernoulliOn ={
                                RobotStep.Get_Bernoulli,
                                RobotStep.Set_BernoulliOn,
                                RobotStep.Get_Bernoulli,
                                RobotStep.Check_BernoulliOn,
                                };

        private RobotStep[] Set_BernoulliOff ={
                                RobotStep.Get_Bernoulli,
                                RobotStep.Set_BernoulliOff,
                                RobotStep.Get_Bernoulli,
                                RobotStep.Check_BernoulliOff,
                                };
        #endregion

        #region Check_With

        private RobotStep[] Check_With ={
                                      RobotStep.Check_WithLoop1s,
                                      RobotStep.Check_With,
                                      };


        private RobotStep[] Check_WithABN_Start ={
                                      RobotStep.Check_WithLoop1s,
                                      RobotStep.Check_With_ABN,
                                      };

        private RobotStep[] Check_WithABN_End ={
                                      RobotStep.Check_With_ABN_RetryEnd,
                                      };
        #endregion

        #region Check_Without

        private RobotStep[] Check_Without ={
                                      RobotStep.Check_WithoutLoop1s,
                                      RobotStep.Check_Without,
                                      };



        private RobotStep[] Check_WithoutABN ={
                                      RobotStep.Check_WithoutLoop1s,
                                      RobotStep.Check_Without_ABN,
                                      };

        #endregion

        #region Check_Is_Turn_Jump

        private RobotStep[] Check_TopRTurn_Jump1 ={
                                      RobotStep.Check_TopR_IsTurn_Jump1,
                                      };

        #endregion

        #region Check_Error_Standby

        private RobotStep[] Check_AllInterLock ={
                                      RobotStep.CheckError_IsTurn,
                                      RobotStep.CheckError_IsExtend,
                                      };

        #endregion

        #region Check_Extend_Error_Standby

        private RobotStep[] Check_ExtendInterLock ={
                                      RobotStep.CheckError_IsExtend,
                                      };

        #endregion

        #region Jump_Start

        private RobotStep[] Jump_Check1 ={
                                      RobotStep.Jump_Check1,
                                      };

        private RobotStep[] Jump_Check2 ={
                                      RobotStep.Jump_Check2,
                                      };

        private RobotStep[] Jump_AbnStart ={
                                      RobotStep.Jump_AbnStart,
                                      };

        private RobotStep[] Jump_AbnEnd ={
                                      RobotStep.Jump_AbnEnd,
                                      };

        #endregion

        #region Aligner

        //private RobotStep[] GPT_Set_AL_LiftPinUp_Step ={                             
        //                              RobotStep.Set_Aligner_LiftPin_Up,
        //                              };

        //private RobotStep[] GPT_Set_AL_LiftPinDown_Step ={                             
        //                              RobotStep.Set_Aligner_LiftPin_Down,
        //                              };

        //private RobotStep[] GPT_Check_AL_LiftPinUp_Step ={  
        //                              RobotStep.Set_Sleep_500ms,
        //                              RobotStep.Check_Aligner_LiftPin_Up,
        //                              };

        //private RobotStep[] GPT_Check_AL_LiftPinDown_Step ={    
        //                              RobotStep.Set_Sleep_500ms,
        //                              RobotStep.Check_Aligner_LiftPin_Down,
        //                              };

        #endregion

        #endregion

        #region Initial

        public Robot1()
        {
            InitializeComponent();
        }

        public void Initial(int Index)
        {
            string strTemp;
            string[] SplitTemp;
            Ui_Busy = false;
            DeviceName = string.Format("{0}{1}", NormalStatic.Robot, Index + 1);
            DeviceNameEnum = (All_Device)Enum.Parse(typeof(All_Device), string.Format("{0}{1}", NormalStatic.Robot, Index + 1));
            RobotName = string.Format("{0}{1}", DeviceName, NormalStatic.UnderLine);

            strTemp = AppSetting.LoadSetting(string.Format("{0}{1}", RobotName, Robot_Static.PosParamter), "Get,");
            SplitTemp = strTemp.Split(',');
            NowGPTCommandType = ((RobotGPT)Enum.Parse(typeof(RobotGPT), SplitTemp[0]));
            NowGPRobotAddress = Convert.ToInt32(SplitTemp[1]);
            NowUseArm = Convert.ToInt32(SplitTemp[2]);
            NowUseObj = SplitTemp[3];
            NowUseSlotGap = Convert.ToDouble(SplitTemp[4]);
            NowUseSlotID = Convert.ToInt32(SplitTemp[5]);

            #region BG

            AutoMoveBG = new Thread(AutoBG_DoWork);
            AutoMoveBG.Start();
            AutoMoveBG.IsBackground = true;
            //AutoMoveBG.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.AutoBG_Completed);
            //AutoMoveBG.RunWorkerAsync();

            CommandBG = new Thread(CommandBG_DoWork);
            CommandBG.Start();
            CommandBG.IsBackground = true;

            #endregion

            #region RS232_Initial

            int IniCOM = Convert.ToInt32(AppSetting.LoadSetting(string.Format("{0}{1}", RobotName, "COM"), "10"));
            RobotCOM = new SerialPort(string.Format("COM{0}", IniCOM));
            RobotCOM.DataReceived += new SerialDataReceivedEventHandler(COM_DataReceived);
            RobotCOM.BaudRate = 19200;
            RobotCOM.DataBits = 7;
            RobotCOM.Parity = Parity.Even;
            RobotCOM.StopBits = StopBits.One;

            #endregion

            #region Status_Initial

            PosAddressMap = (AppSetting.LoadSetting(string.Format("{0}{1}", RobotName, Robot_Static.PosAddress), "").Split(','));
            IniRetryCount = Convert.ToInt32(AppSetting.LoadSetting(string.Format("{0}{1}", RobotName, Robot_Static.RetryCount), "0"));
            IniStopRestart = Convert.ToBoolean(AppSetting.LoadSetting(string.Format("{0}{1}", RobotName, Robot_Static.StopRestart), "False"));
            Speed = Convert.ToInt32(AppSetting.LoadSetting(string.Format("{0}{1}", RobotName, Robot_Static.Speed), "10"));
            Ui_Speed = string.Format("{0}", Speed);

            #endregion

            #region Arm_Initial

            IniMappingSup = Convert.ToBoolean(AppSetting.LoadSetting(string.Format("{0}{1}", RobotName, Robot_Static.RobotMapping), "False"));
            IniTopSup = Convert.ToBoolean(AppSetting.LoadSetting(string.Format("{0}{1}", RobotName, Robot_Static.RobotTop), "False"));
            IniArmCnt = Convert.ToInt32(AppSetting.LoadSetting(string.Format("{0}{1}", RobotName, Robot_Static.ArmNumber), "1"));
            IniUseArmType = new ArmType[IniArmCnt];
            IniArmOutType = new ArmType[IniArmCnt, 2];
            IniArmInCnt = new int[IniArmCnt];

            for (int ArmCnt = 0; ArmCnt < IniArmCnt; ArmCnt++)
            {
                if (ArmCnt == 0)
                {
                    IniUseArmType[ArmCnt] = new ArmType();
                    IniUseArmType[ArmCnt] = (ArmType)Convert.ToInt16(AppSetting.LoadSetting(string.Format("{0}{1}", RobotName, Robot_Static.ArmTypeLower), "0"));

                    IniArmInCnt[ArmCnt] = new int();
                    IniArmInCnt[ArmCnt] = Convert.ToInt16(AppSetting.LoadSetting(string.Format("{0}{1}", RobotName, Robot_Static.ArmTypeLowerInCnt), "2"));


                    IniArmOutOn = new int[IniArmCnt, 2];
                    IniArmOutOff = new int[IniArmCnt, 2];
                    IniArmInCntBit = new int[IniArmCnt, 4];
                    IniArmInFlag = new int[IniArmCnt, 4];

                    strTemp = AppSetting.LoadSetting(string.Format("{0}{1}", RobotName, Robot_Static.ArmTypeLowerOutType), "0,0");
                    SplitTemp = strTemp.Split(',');

                    IniArmOutType[ArmCnt, 0] = new ArmType();
                    IniArmOutType[ArmCnt, 0] = (ArmType)Convert.ToInt16(SplitTemp[0]);
                    IniArmOutType[ArmCnt, 1] = new ArmType();
                    IniArmOutType[ArmCnt, 1] = (ArmType)Convert.ToInt16(SplitTemp[1]);

                    strTemp = AppSetting.LoadSetting(string.Format("{0}{1}", RobotName, Robot_Static.ArmTypeLowerOutOn), "0,0");
                    SplitTemp = strTemp.Split(',');

                    IniArmOutOn[ArmCnt, 0] = new int();
                    IniArmOutOn[ArmCnt, 0] = Convert.ToInt16(SplitTemp[0]);
                    IniArmOutOn[ArmCnt, 1] = new int();
                    IniArmOutOn[ArmCnt, 1] = Convert.ToInt16(SplitTemp[1]);

                    strTemp = AppSetting.LoadSetting(string.Format("{0}{1}", RobotName, Robot_Static.ArmTypeLowerOutOff), "0,0");
                    SplitTemp = strTemp.Split(',');

                    IniArmOutOff[ArmCnt, 0] = new int();
                    IniArmOutOff[ArmCnt, 0] = Convert.ToInt16(SplitTemp[0]);
                    IniArmOutOff[ArmCnt, 1] = new int();
                    IniArmOutOff[ArmCnt, 1] = Convert.ToInt16(SplitTemp[1]);


                    strTemp = AppSetting.LoadSetting(string.Format("{0}{1}", RobotName, Robot_Static.ArmTypeLowerIn), "0,0,0,0");
                    SplitTemp = strTemp.Split(',');

                    IniArmInCntBit[ArmCnt, 0] = Convert.ToInt16(SplitTemp[0]);
                    IniArmInCntBit[ArmCnt, 1] = Convert.ToInt16(SplitTemp[1]);
                    IniArmInCntBit[ArmCnt, 2] = Convert.ToInt16(SplitTemp[2]);
                    IniArmInCntBit[ArmCnt, 3] = Convert.ToInt16(SplitTemp[3]);

                    strTemp = AppSetting.LoadSetting(string.Format("{0}{1}", RobotName, Robot_Static.ArmTypeLowerInWaferFlag), "0,0,0,0");
                    SplitTemp = strTemp.Split(',');

                    IniArmInFlag[ArmCnt, 0] = Convert.ToInt16(SplitTemp[0]);
                    IniArmInFlag[ArmCnt, 1] = Convert.ToInt16(SplitTemp[1]);
                    IniArmInFlag[ArmCnt, 2] = Convert.ToInt16(SplitTemp[2]);
                    IniArmInFlag[ArmCnt, 3] = Convert.ToInt16(SplitTemp[3]);


                }
                else
                {
                    IniUseArmType[ArmCnt] = new ArmType();
                    IniUseArmType[ArmCnt] = (ArmType)Convert.ToInt16(AppSetting.LoadSetting(string.Format("{0}{1}", RobotName, Robot_Static.ArmTypeUpper), "0"));

                    IniArmInCnt[ArmCnt] = new int();
                    IniArmInCnt[ArmCnt] = Convert.ToInt16(AppSetting.LoadSetting(string.Format("{0}{1}", RobotName, Robot_Static.ArmTypeUpperInCnt), "2"));

                    strTemp = AppSetting.LoadSetting(string.Format("{0}{1}", RobotName, Robot_Static.ArmTypeUpperOutType), "0,0");
                    SplitTemp = strTemp.Split(',');

                    IniArmOutType[ArmCnt, 0] = new ArmType();
                    IniArmOutType[ArmCnt, 0] = (ArmType)Convert.ToInt16(SplitTemp[0]);
                    IniArmOutType[ArmCnt, 1] = new ArmType();
                    IniArmOutType[ArmCnt, 1] = (ArmType)Convert.ToInt16(SplitTemp[1]);

                    strTemp = AppSetting.LoadSetting(string.Format("{0}{1}", RobotName, Robot_Static.ArmTypeUpperOutOn), "0,0");
                    SplitTemp = strTemp.Split(',');

                    IniArmOutOn[ArmCnt, 0] = new int();
                    IniArmOutOn[ArmCnt, 0] = Convert.ToInt16(SplitTemp[0]);
                    IniArmOutOn[ArmCnt, 1] = new int();
                    IniArmOutOn[ArmCnt, 1] = Convert.ToInt16(SplitTemp[1]);

                    strTemp = AppSetting.LoadSetting(string.Format("{0}{1}", RobotName, Robot_Static.ArmTypeUpperOutOff), "0,0");
                    SplitTemp = strTemp.Split(',');

                    IniArmOutOff[ArmCnt, 0] = new int();
                    IniArmOutOff[ArmCnt, 0] = Convert.ToInt16(SplitTemp[0]);
                    IniArmOutOff[ArmCnt, 1] = new int();
                    IniArmOutOff[ArmCnt, 1] = Convert.ToInt16(SplitTemp[1]);

                    strTemp = AppSetting.LoadSetting(string.Format("{0}{1}", RobotName, Robot_Static.ArmTypeUpperIn), "0,0,0,0");
                    SplitTemp = strTemp.Split(',');

                    IniArmInCntBit[ArmCnt, 0] = Convert.ToInt16(SplitTemp[0]);
                    IniArmInCntBit[ArmCnt, 1] = Convert.ToInt16(SplitTemp[1]);
                    IniArmInCntBit[ArmCnt, 2] = Convert.ToInt16(SplitTemp[2]);
                    IniArmInCntBit[ArmCnt, 3] = Convert.ToInt16(SplitTemp[3]);

                    strTemp = AppSetting.LoadSetting(string.Format("{0}{1}", RobotName, Robot_Static.ArmTypeUpperInWaferFlag), "0,0,0,0");
                    SplitTemp = strTemp.Split(',');

                    IniArmInFlag[ArmCnt, 0] = Convert.ToInt16(SplitTemp[0]);
                    IniArmInFlag[ArmCnt, 1] = Convert.ToInt16(SplitTemp[1]);
                    IniArmInFlag[ArmCnt, 2] = Convert.ToInt16(SplitTemp[2]);
                    IniArmInFlag[ArmCnt, 3] = Convert.ToInt16(SplitTemp[3]);

                }

            }

            #endregion

            #region StartAddress_Initial

            strTemp = AppSetting.LoadSetting(string.Format("{0}{1}", RobotName, Robot_Static.StartAddress), "");
            SplitTemp = strTemp.Split(',');
            for (int i = 0; i < SplitTemp.Length; i += 2)
            {
                Set_OtherAddress(SplitTemp[i], SplitTemp[i + 1]);
            }

            string deviceTemp;

            switch (Index)
            {
                case 0:
                    {
                        for (int i = 1; i <= HCT_EFEM.LPCount; i++)
                        {
                            deviceTemp = string.Format("{0}{1}{2}", NormalStatic.LP, i, NormalStatic.POS);
                            strTemp = AppSetting.LoadSetting(deviceTemp, "");
                            string[] address = strTemp.Split(new string[] { "," }, StringSplitOptions.None);

                            Set_LPAddress(deviceTemp, address);

                            deviceTemp = string.Format("{0}{1}{2}", NormalStatic.LP, i, NormalStatic.MAP);
                            strTemp = AppSetting.LoadSetting(deviceTemp, "");
                            address = strTemp.Split(new string[] { "," }, StringSplitOptions.None);
                            Set_LPAddress(deviceTemp, address);
                        }
                    }
                    break;

                case 1:
                    {
                        for (int i = 1; i <= HCT_EFEM.MagazineCount; i++)
                        {
                            deviceTemp = string.Format("{0}{1}{2}", NormalStatic.MagazinePort, i, NormalStatic.POS);
                            strTemp = AppSetting.LoadSetting(deviceTemp, "");
                            string[] address = strTemp.Split(new string[] { "," }, StringSplitOptions.None);

                            Set_LPAddress(deviceTemp, address);

                            deviceTemp = string.Format("{0}{1}{2}", NormalStatic.MagazinePort, i, NormalStatic.MAP);
                            strTemp = AppSetting.LoadSetting(deviceTemp, "");
                            address = strTemp.Split(new string[] { "," }, StringSplitOptions.None);
                            Set_LPAddress(deviceTemp, address);
                        }
                    }
                    break;
            }

            #endregion

            //COM_Connect();
        }

        #endregion

        #region Dictionay_Address

        #region Other/AL/AM/Stage

        private void Set_OtherAddress(string _objName, string _address)
        {
            Other_StartAddressDictionay.Add(_objName, Convert.ToInt16(_address));
        }

        public bool Check_OtherAddress(string _objName)
        {
            return Other_StartAddressDictionay.ContainsKey(_objName);
        }

        public int Get_OtherAddress(string _objName)
        {
            return Other_StartAddressDictionay[_objName];
        }

        #endregion

        #region LP

        private void Set_LPAddress(string _objName, string[] _addressList)
        {
            LP_StartAddressDictionay.Add(_objName, _addressList);
        }

        public bool Check_LPAddress(string _objName)
        {
            return LP_StartAddressDictionay.ContainsKey(_objName);
        }

        public int Get_LPAddress(string _objName, int type)
        {
            return Convert.ToInt32(LP_StartAddressDictionay[_objName][type]);
        }

        #endregion

        #region Axis

        private void CleanPosAddress()
        {
            AddressMapCnt = 0;
            Axis_PosAddressDictionay.Clear();
        }

        #endregion

        #endregion

        #region RS232

        public void COM_Connect()
        {
            RobotCOM.Close();
            try
            {
                RobotCOM.Open();
                Ui_Connect = true;
            }
            catch
            {
                Ui_Connect = false;
                UI.InitialSystem(DeviceName, NormalStatic.False, ErrorList.AP_SerialError_0381);
            }
        }

        public void COM_Disconnect()
        {
            Ui_Connect = false;
            RobotCOM.Close();
        }

        private void COM_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] raw_byte = new byte[1];
            SerialPort sp = (SerialPort)sender;
            Thread.Sleep(10);

            while (sp.BytesToRead > 0)
            {
                sp.Read(raw_byte, 0, 1);
                // Int32 length = sp.Read(raw_byte, 0, raw_byte.Length);
                // Array.Resize(ref raw_byte, length);
                DataCombine(raw_byte);
            }
        }

        private void DataSend(string Command_String)
        {
            string Send_String = string.Format("{0}{1}", Robot_Static.RobotAddress, Command_String);
            List<byte> Command_Byte = new List<byte>(Encoding.ASCII.GetBytes(Send_String));
            Command_Byte.Add(NormalStatic.EndByte_ETX);
            byte check_sum = CheckSum(Command_Byte.ToArray());
            Command_Byte.Insert(0, NormalStatic.StartByte_STX);
            Command_Byte.Add(check_sum);
            RobotCOM.Write(Command_Byte.ToArray(), 0, Command_Byte.Count);

        }

        private byte CheckSum(byte[] data)
        {
            byte return_data = 0x00;
            foreach (byte SingleByte in data)
                return_data ^= SingleByte;
            return return_data;
        }

        private void DataCombine(byte[] raw_data)
        {

            switch (ReceiveNowStepCnt)
            {
                case NormalStatic.Idle:
                    {
                        UI.Alarm(DeviceName, ErrorList.AP_SerialError_0381, string.Format("0x{0}", Convert.ToString(raw_data[0], 16).ToUpper()));
                    }
                    break;

                case NormalStatic.WaitReply://1
                    {
                        if (raw_data[0] != NormalStatic.StartByte_STX)
                        {
                            UI.Log(NormalStatic.Robot, DeviceName, SystemList.DeviceReceive, string.Format("Wait Reply SerialError"));
                            NowErrorMsg += string.Format("0x{0}", Convert.ToString(raw_data[0], 16).ToUpper());
                            NowErrorList = ErrorList.AP_SerialError_0381;
                            break;
                        }
                        ReceiveTemp.Clear();
                        ReceiveNowStepCnt = NormalStatic.Receiving;
                    }
                    break;

                case NormalStatic.Receiving://2
                    {
                        ReceiveTemp.Add(raw_data[0]);
                        if (raw_data[0] == NormalStatic.EndByte_ETX)
                            ReceiveNowStepCnt = NormalStatic.WaitCheckSum;
                    }
                    break;

                case NormalStatic.WaitCheckSum://3
                    {
                        byte Check = CheckSum(ReceiveTemp.ToArray());
                        string CompleteString = Encoding.ASCII.GetString(ReceiveTemp.GetRange(0, ReceiveTemp.Count - 1).ToArray());
                        if (raw_data[0] != Check)
                        {
                            NowErrorMsg = string.Format("0x{0}-0x{1}", Convert.ToString(raw_data[0], 16).ToUpper(), Convert.ToString(Check, 16).ToUpper());
                            NowErrorList = ErrorList.ChecksumError_0101;
                        }
                        else if (CompleteString.Substring(0, 4) != Robot_Static.RobotAddress)
                        {
                            UI.Log(NormalStatic.Robot, DeviceName, SystemList.DeviceReceive, string.Format("CheckSum SerialError"));

                            NowErrorMsg = CompleteString;
                            NowErrorList = ErrorList.AP_SerialError_0381;
                        }
                        else
                        {
                            ReceiveNowStepCnt = NormalStatic.Idle;
                            ReceiverQueue.EnQueue(CompleteString.Substring(4));
                        }

                    }
                    break;
            }

        }

        private void ReceiveHandler()
        {
            string[] CMD = CMD_string.Split(new string[] { " " }, StringSplitOptions.None);
            string[] REC = REC_string.Split(new string[] { " " }, StringSplitOptions.None);


            switch (CMD[0])
            {
                case "GD":  //Stop
                case "LS":  //GetStatus;  
                case "GP":  //Get/Put
                    {
                        Status_update(REC[0].Substring(0, 4));
                        Status_Check(CMD[0], REC[0].Substring(0, 4));
                    }
                    break;

                case "GE":  //Restart
                    {
                        //ULD_protocol.RBreceive(CMD_string, REC_string);
                        if (CheckRestartCommand())
                        {
                            NowMainJab = NowResumeJab;
                            NowStepCnt = NowStepGPCnt;
                            Status_update(REC[0].Substring(0, 4));
                            AutoStepQueue.EnQueue(MarcoCommand);
                            return;
                        }
                        else
                            Status_update(REC[0].Substring(0, 4));
                    }
                    break;

                //case "LB":
                //    {
                //        if (REC[0] == "0")
                //        {
                //            NowStopAction = true;
                //        }
                //    }
                //    break;

                case "LBD":  //LB
                    {
                        ReplyMsg = REC[0];
                    }
                    break;

                case "LID0":
                    {
                        //     ULD_protocol.RBreceive(CMD_string, REC_string);
                        TempDi_1 = Convert.ToString(Convert.ToInt32(REC[0]), 2).PadLeft(8, '0');
                    }
                    break;

                case "LID1":
                    {
                        //    ULD_protocol.RBreceive(CMD_string, REC_string);
                        TempDi_2 = Convert.ToString(Convert.ToInt32(REC[0]), 2).PadLeft(8, '0');
                        TempDi_All = string.Format("{0}{1}", TempDi_2, TempDi_1);
                    }
                    break;

                case "LD0":
                    {
                        // ULD_protocol.RBreceive(CMD_string, REC_string);
                        if (REC_string.IndexOf("-") >= 0)
                        {
                            REC_string = REC_string.Replace("-", " -");
                        }

                        REC_string = REC_string.Replace("L", "").Replace("R", "");
                        Axis_PosAddressDictionay.Add(PosAddressMap[AddressMapCnt], REC_string);

                        REC = REC_string.Split(new string[] { " " }, StringSplitOptions.None);

                        for (int i = 0; i < (int)RobotAxis.MaxCnt; i++)
                        {
                            RobotHomeAxisPos[i] = REC[i];
                        }

                        AddressMapCnt++;
                    }
                    break;

                case "LR"://position
                    {
                        if (REC_string.IndexOf("-") >= 0)
                        {
                            REC_string = REC_string.Replace("-", " -");
                        }

                        REC = REC_string.Split(new string[] { " " }, StringSplitOptions.None);

                        if (!Moving)
                        {
                            for (int i = 0; i < (int)RobotAxis.MaxCnt; i++)
                            {
                                if (i == (int)RobotAxis.C)
                                {
                                    RobotNodeAxisPos[i] = REC[i].Replace("L", "").Replace("R", "");
                                }
                                else
                                    RobotNodeAxisPos[i] = REC[i];
                            }

                        }
                        else
                        {
                            for (int i = 0; i < (int)RobotAxis.MaxCnt; i++)
                            {
                                if (i == (int)RobotAxis.C)
                                {
                                    RobotMoveAxisPos[i] = REC[i].Replace("L", "").Replace("R", "");
                                }
                                else
                                    RobotMoveAxisPos[i] = REC[i];
                            }

                            if (!CheckRobotPositionRule())
                            {
                                NowErrorList = ErrorList.RB_AddressMovStop_0318;
                                NowStopAction = true;
                            }
                        }
                        UpdateRobotPosition();
                    }
                    break;

                case "SP":
                    {
                        ULD_protocol.RBreceive(CMD_string, REC_string);
                        Ui_Speed = string.Format("{0}", Speed);
                    }
                    break;


                case "LOD":
                case "SOD0":
                case "SOD1":
                    {
                        ReplyMsg = REC[0];
                    }
                    break;

                case "CL":
                    {
                        //   ULD_protocol.RBreceive(CMD_string, REC_string);
                        Status_update(REC[0].Substring(0, 4));
                        Ui_Connect = true;
                    }
                    break;

                case "LE110":
                    {
                        int RecData = int.Parse(REC[0]);
                        MappPresent = Convert.ToString(RecData, 2);
                    }
                    break;

                case "LE111":
                    {
                        int RecData = int.Parse(REC[0]);
                        MappError = Convert.ToString(RecData, 2);
                    }
                    break;


                default:
                    {
                        if (CMD[0].IndexOf("LD") >= 0)
                        {
                            if (REC_string.IndexOf("-") >= 0)
                            {
                                REC_string = REC_string.Replace("-", " -");
                            }
                            REC_string = REC_string.Replace("L", "").Replace("R", "");

                            if (MarcoCommand[NowStepCnt] == RobotStep.Update_HomeBackGPPosition)
                            {
                                for (int i = 0; i < (int)RobotAxis.MaxCnt; i++)
                                {
                                    if (i == (int)RobotAxis.C)
                                    {
                                        RobotBackGPAxisPos[i] = REC[i].Replace("L", "").Replace("R", "");
                                    }
                                    else
                                        RobotBackGPAxisPos[i] = REC[i];
                                }
                            }
                            else
                            {

                                Axis_PosAddressDictionay.Add(PosAddressMap[AddressMapCnt], REC_string);
                                AddressMapCnt++;
                            }
                        }
                    }
                    break;
            }

            if (Ui_Busy || NowMainJab != SocketCommand.MaxCnt)
            {
                UI.Log(NormalStatic.Robot, DeviceName, SystemList.CommandParameter, string.Format("Step receive queue enqueue : {0}", CMD_string));
                StepRecQueue.EnQueue(CMD_string);
            }
        }

        #endregion

        #region Get/Set

        #region Busy
        private bool BUSY;
        public bool Ui_Busy
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_Busy = value; }));
                    return;
                }

                // Joanne 20200623 Edit
                //labBusy.BackColor = value ? Color.Yellow : Color.LightGreen;
                labBusy.BackColor = value ? Color.Yellow : System.Drawing.SystemColors.ControlLight;
                labBusy.Text = value ? "Busy" : "Idle";
                UserUnloader.SendCommand(string.Format("RBBusy,,{0}", value ? "1" : "0"));
                if (BUSY != value)
                    ULD_protocol.Ui_Busy(value);

                BUSY = value;
            }
            get { return BUSY; }
        }
        #endregion

        #region Speed
        public string Ui_Speed
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_Speed = value; }));
                    return;
                }
                labSpeed.Text = value;
            }
        }
        #endregion

        #region ArmWaferInfor

        public WaferStatus Ui_LowerWaferPresent
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_LowerWaferPresent = value; }));
                    return;
                }
                if (UpperWaferPresent == WaferStatus.With)
                {

                    picRobot.Image = (value == WaferStatus.With ? Properties.Resources.Robot_DWith : Properties.Resources.Robot_UWith);
                }

                else
                {

                    picRobot.Image = (value == WaferStatus.With ? Properties.Resources.Robot_LWith : Properties.Resources.Robot_DWithOut);
                }
                if (value == WaferStatus.With)
                    ULD_protocol.RBwaferPresent("With");
                else
                    ULD_protocol.RBwaferPresent("WithOut");

                LowerWaferPresent = value;
            }
            get { return LowerWaferPresent; }
        }

        public WaferStatus Ui_UpperWaferPresent
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_UpperWaferPresent = value; }));
                    return;
                }

                if (LowerWaferPresent == WaferStatus.With)
                    picRobot.Image = (value == WaferStatus.With ? Properties.Resources.Robot_DWith : Properties.Resources.Robot_LWith);
                else
                    picRobot.Image = (value == WaferStatus.With ? Properties.Resources.Robot_UWith : Properties.Resources.Robot_DWithOut);

                UpperWaferPresent = value;
            }
            get { return UpperWaferPresent; }
        }

        public string Ui_LowerWaferInfo
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_LowerWaferInfo = value; }));
                    return;
                }
                labLowerWaferInfo.Text = value;
                LowerWaferInfo = value;
            }
            get { return LowerWaferInfo; }
        }

        public string Ui_UpperWaferInfo
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_UpperWaferInfo = value; }));
                    return;
                }
                labUpperWaferInfo.Text = value;
                UpperWaferInfo = value;
            }
            get { return UpperWaferInfo; }
        }

        public RobotPosition Ui_RobotPos
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_RobotPos = value; }));
                    return;
                }

                switch (value)
                {

                    case RobotPosition.Home:
                        {
                            pnlRobot.Location = new Point(1000, pnlRobot.Location.Y);
                        }
                        break;

                    case RobotPosition.P1:
                    case RobotPosition.P1_Map:
                    case RobotPosition.P2:
                    case RobotPosition.P2_Map:
                    case RobotPosition.P3:
                    case RobotPosition.P3_Map:
                    case RobotPosition.P4:
                    case RobotPosition.P4_Map:
                    case RobotPosition.P5:
                    case RobotPosition.P5_Map:
                    case RobotPosition.P6:
                    case RobotPosition.P6_Map:
                    case RobotPosition.P7:
                    case RobotPosition.P7_Map:
                    case RobotPosition.P8:
                    case RobotPosition.P8_Map:
                    case RobotPosition.P9:
                    case RobotPosition.P9_Map:
                    case RobotPosition.P10:
                    case RobotPosition.P10_Map:
                        {
                            if (DeviceName == "Robot1")
                                pnlRobot.Location = new Point(80 + ((int)(value - RobotPosition.P1) / 2) * 200, pnlRobot.Location.Y);
                            else
                                pnlRobot.Location = new Point(1700 - ((int)(value - RobotPosition.P1) / 2) * 200, pnlRobot.Location.Y);
                        }
                        break;

                    case RobotPosition.Turn_S1:
                    case RobotPosition.Stage1:
                        {
                            pnlRobot.Location = new Point(180, pnlRobot.Location.Y);
                        }
                        break;

                    case RobotPosition.Turn_S2:
                    case RobotPosition.Stage2:
                        {
                            pnlRobot.Location = new Point(1400, pnlRobot.Location.Y);
                        }
                        break;

                    case RobotPosition.Turn_AL:
                    case RobotPosition.Aligner1:
                        {
                            pnlRobot.Location = new Point(1000, pnlRobot.Location.Y);
                        }
                        break;
                }

                labRbLocation.Text = value.ToString();
                ULD_protocol.Ui_RobotPos(labRbLocation.Text);
                NowRobotPos = value;
            }
            get { return NowRobotPos; }
        }

        public ArmStatus Ui_ArmStatusX
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_ArmStatusX = value; }));
                    return;
                }

                switch (value)
                {
                    case ArmStatus.Arm_Home:
                        {
                            // Joanne 20200623 Edit
                            //labLowerXLocation.BackColor = Color.LightGreen;
                            labLowerXLocation.BackColor = System.Drawing.SystemColors.ControlLight;
                            labLowerXLocation.Text = "Home";
                        }
                        break;

                    case ArmStatus.Arm_Extend:
                        {
                            labLowerXLocation.BackColor = Color.Red;
                            labLowerXLocation.Text = "Extend";
                        }
                        break;
                }
                ULD_protocol.ArmStatus(labLowerXLocation.Text);
                NowArmStatusX = value;
            }
            get { return NowArmStatusX; }
        }

        public ArmStatus Ui_ArmStatusR
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_ArmStatusR = value; }));
                    return;
                }

                switch (value)
                {

                    case ArmStatus.Arm_Home:
                        {
                            //Joanne 20200623 Edit
                            //labLowerRLocation.BackColor = Color.LightGreen;
                            labLowerRLocation.BackColor = System.Drawing.SystemColors.ControlLight;
                            labLowerRLocation.Text = "Home";
                        }
                        break;

                    case ArmStatus.Arm_Now_Turn:
                        {
                            labLowerRLocation.BackColor = Color.Red;
                            labLowerRLocation.Text = "Turning";
                        }
                        break;

                    case ArmStatus.Arm_Turn:
                        {
                            labLowerRLocation.BackColor = Color.Yellow;
                            labLowerRLocation.Text = "Turn";
                        }
                        break;
                }

                NowArmStatusR = value;
            }
            get { return NowArmStatusR; }
        }

        public ArmStatus Ui_ArmStatusY
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_ArmStatusY = value; }));
                    return;
                }

                switch (value)
                {
                    case ArmStatus.Arm_Home:
                        {
                            // Joanne 20200623 Edit
                            //labUpperYLocation.BackColor = Color.LightGreen;
                            labUpperYLocation.BackColor = System.Drawing.SystemColors.ControlLight;
                            labUpperYLocation.Text = "Home";
                        }
                        break;

                    case ArmStatus.Arm_Extend:
                        {
                            labUpperYLocation.BackColor = Color.Red;
                            labUpperYLocation.Text = "Extend";
                        }
                        break;
                }

                NowArmStatusY = value;
            }
            get { return NowArmStatusY; }
        }

        #endregion

        #region Connect
        private bool CONNECT;
        public bool Ui_Connect
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_Connect = value; }));
                    return;
                }

                // Joanne 20200623 Edit
                //labConnect.BackColor = (value ? Color.LightGreen : Color.Red);
                labConnect.BackColor = (value ? System.Drawing.SystemColors.ControlLight : Color.Red);
                labConnect.Text = (value ? "Con-C" : "Dis-C");
                CONNECT = value;
            }
            get { return CONNECT; }
        }

        #endregion

        #region Remote
        private bool REMOTE;
        public bool Ui_Remote
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_Remote = value; }));
                    return;
                }

                //if(REMOTE!=value)
                if (REMOTE != value)
                    ULD_protocol.RB_Remote(value ? "Auto" : "Manual");

                REMOTE = value;

                // Joanne 20200623 Edit
                //labRemote.BackColor = (value ? Color.LightGreen : Color.Red);
                labRemote.BackColor = (value ? System.Drawing.SystemColors.ControlLight : Color.Red);
                labRemote.Text = (value ? "Auto" : "Manual");

            }
            get { return REMOTE; }
        }
        #endregion

        #region Status
        private string STATUS;
        public string Ui_Status
        {
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate () { Ui_Status = value; }));
                    return;
                }
                switch (value)
                {
                    case "0601":
                        //labStatus.BackColor = Color.LightGreen;
                        labStatus.BackColor = System.Drawing.SystemColors.ControlLight;
                        break;

                    case "4401":
                        labStatus.BackColor = Color.Yellow;
                        break;

                    default:
                        labStatus.BackColor = Color.Red;
                        break;
                }
                labStatus.Text = value;
                STATUS = value;
            }
            get { return STATUS; }
        }

        public bool Ui_Moving
        {
            get { return Moving; }
        }

        public bool Ui_Stop
        {
            get { return Stop; }
        }

        public int Retry_Count
        {
            set
            {
                IniRetryCount = value;
            }
        }

        #endregion

        #endregion

        #region BG_DoWork

        private void AutoBG_DoWork()
        {
            // int StatusRetryLimit = 400;
            // int StatusRetry = 0;
            // bool ReadLB_XY = false;
            while (true)
            {
                MarcoCommand = AutoStepQueue.DeQueue();

                if (MarcoCommand[NowStepCnt] == RobotStep.End)//關程式用的
                    break;

                while (NowStepCnt < MarcoCommand.Length)
                {
                    //StatusRetry = 0;
                    StepRecQueue.Clear();         //不能砍這行

                    if (MarcoCommand[NowStepCnt] == RobotStep.MaxCnt)//確定傳完
                    {
                        if (RetryFlag)
                        {
                            NowErrorList = ErrorList.RB_Abn_0117;
                            NowErrorMsg = NowErrorMsg + " Retry:" + IniRetryCount.ToString(); // Walson 20201124
                            ActionComplete(string.Format("{0},{1},{2},{3}", NowMainJab, NowUseArm, NowUseObj, NowUseSlotID), false, DeviceName);
                        }
                        else if (NowMainJab == SocketCommand.RobotMapping)
                        {
                            UI.Log(NormalStatic.Robot, DeviceName, SystemList.CommandParameter, string.Format("Process MaxCnt RobotMapping Robot Busy set true"));
                            ActionComplete(string.Format("{0},{1},{2},{3},{4}", NowMainJab, NowUseArm, NowUseObj, MappPresent, MappError), true, DeviceName);
                            Ui_Busy = false;
                        }
                        else
                        {
                            UI.Log(NormalStatic.Robot, DeviceName, SystemList.CommandParameter, string.Format("Process MaxCnt Else Robot Busy set true"));
                            ActionComplete(string.Format("{0},{1},{2},{3}", NowMainJab, NowUseArm, NowUseObj, NowUseSlotID), true, DeviceName);
                            Ui_Busy = false;
                        }
                        break;
                    }

                    //Chcek Obj Cassete Present
                    if (NowMainJab == SocketCommand.WaferGet || NowMainJab == SocketCommand.WaferPut)
                    {
                        //if (CheckDestinationPresent(NowUseObj)) //Check Object Cassette Present Error,Send Stop
                        //{
                        //    UI.Log(NormalStatic.Robot, DeviceName, SystemList.CommandParameter,
                        //           string.Format("Destination casette error , Trigger Stop at Not Moving)"));
                        //    Qry_Stop();
                        //    SpinWait.SpinUntil(() => false, 1000);

                        //    JobFail();
                        //    NowErrorList = ErrorList.RB_ActionNotCmd_0113;
                        //    NowErrorMsg = "Destination presnet Error";
                        //    ActionComplete(string.Format("{0},{1},{2},{3}", NowMainJab, NowUseArm, NowUseObj, NowUseSlotID), false, DeviceName);
                        //    continue;
                        //}
                    }

                    NextStepFlag = false;

                    switch (MarcoCommand[NowStepCnt])
                    {
                        #region GP

                        case RobotStep.GP_Home_0:
                        case RobotStep.GP_Home_1:
                        case RobotStep.GP_AlignerOCR_Up:
                        case RobotStep.GP_AlignerOCR_Down:
                        case RobotStep.GP_TopHomeZ_Back:
                            {
                                GPT_HomeStepPdu(MarcoCommand[NowStepCnt]);
                                NowStepGPCnt = NowStepCnt;
                            }
                            break;

                        case RobotStep.GP_Turn_0:
                        case RobotStep.GP_Turn_1:
                        case RobotStep.GP_Turn_2:
                        case RobotStep.GP_Turn_3:
                        case RobotStep.GP_TurnBack_0:
                        case RobotStep.GP_TurnBack_1:
                        case RobotStep.GP_TurnBack_2:
                        case RobotStep.GP_TurnBack_3:
                            {
                                GPT_TurnStepPdu(MarcoCommand[NowStepCnt]);
                                NowStepGPCnt = NowStepCnt;
                            }
                            break;

                        case RobotStep.GP_Normal_0:
                        case RobotStep.GP_Normal_1:
                        case RobotStep.GP_Normal_2:
                        case RobotStep.GP_Normal_3:
                        case RobotStep.GP_Normal_4:
                        case RobotStep.GP_Normal_5:
                        case RobotStep.GP_Normal_6:
                        case RobotStep.GP_Normal_7:
                        case RobotStep.GP_Normal_8:
                        case RobotStep.GP_Normal_9:
                        case RobotStep.GP_NormalHomeZ_Back:
                        case RobotStep.GP_Top_0:
                        case RobotStep.GP_Top_1:
                        case RobotStep.GP_Top_2:
                        case RobotStep.GP_Top_3:
                        case RobotStep.GP_Top_4:
                        case RobotStep.GP_Top_5:
                        case RobotStep.GP_Top_6:
                        case RobotStep.GP_Top_7:
                        case RobotStep.GP_Top_8:
                        case RobotStep.GP_Top_9:
                            {
                                GPT_StepPdu(MarcoCommand[NowStepCnt]);
                                NowStepGPCnt = NowStepCnt;
                            }
                            break;

                        #endregion

                        #region Mapping

                        case RobotStep.GP_Mapping_0:
                        case RobotStep.GP_Mapping_1:
                        case RobotStep.GP_Mapping_2:
                        case RobotStep.GP_Mapping_3:
                            {
                                GPT_MappingStepPdu(MarcoCommand[NowStepCnt]);
                                NowStepGPCnt = NowStepCnt;
                            }
                            break;

                        #endregion

                        #region Home

                        case RobotStep.GP_Home_SnakeMotion:
                            {
                                int _LBD = Int32.Parse(ReplyMsg);

                                int[] bits = new int[8];

                                for (int idx = 0; idx < 8; idx++)
                                {
                                    bits[idx] = (_LBD >> idx) & 0x01;
                                }

                                if (bits[0] == 1)
                                {
                                    if (bits[2] == 1)
                                        GPT_HomeStepPdu(RobotStep.GP_Home_SnakeMotion_LP1);
                                    else if (bits[3] == 1)
                                        GPT_HomeStepPdu(RobotStep.GP_Home_SnakeMotion_LP2);
                                    else if (bits[4] == 1)
                                        GPT_HomeStepPdu(RobotStep.GP_Home_SnakeMotion_EQ1);
                                    else if (bits[5] == 1)
                                        GPT_HomeStepPdu(RobotStep.GP_Home_SnakeMotion_EQ2);
                                    else
                                    {
                                        AutoStepFail();
                                    }
                                }
                                else if (bits[1] == 1)
                                {
                                    if (bits[2] == 1)
                                        GPT_HomeStepPdu(RobotStep.GP_Home_SnakeMotion_LP1);
                                    else if (bits[3] == 1)
                                        GPT_HomeStepPdu(RobotStep.GP_Home_SnakeMotion_LP2);
                                    else if (bits[4] == 1)
                                        GPT_HomeStepPdu(RobotStep.GP_Home_SnakeMotion_EQ1);
                                    else if (bits[5] == 1)
                                        GPT_HomeStepPdu(RobotStep.GP_Home_SnakeMotion_EQ2);
                                    else
                                    {
                                        AutoStepFail();
                                    }
                                }
                                else
                                {
                                    Qry_Getstatus();
                                }

                            }
                            break;

                        #endregion

                        #region Get
                        case RobotStep.Get_Status:
                            {
                                Qry_Getstatus();
                            }
                            break;

                        case RobotStep.Get_LBD:
                            {
                                Qry_ReadLBD();
                            }
                            break;

                        case RobotStep.Get_DI_0:
                            {
                                GetDiBytePdu(0);
                            }
                            break;

                        case RobotStep.Get_DI_1:
                            {
                                GetDiBytePdu(1);
                            }
                            break;

                        case RobotStep.Get_Vac:
                            {
                                GetDoProcess(ArmType.Vacuum);
                            }
                            break;

                        case RobotStep.Get_Edge:
                            {
                                GetDoProcess(ArmType.EdgeGrip);
                            }
                            break;

                        case RobotStep.Get_Bernoulli:
                            {
                                GetDoProcess(ArmType.Bernoulli);
                            }
                            break;

                        case RobotStep.Get_MappingData:
                            {
                                Qry_MappingData();
                            }
                            break;

                        case RobotStep.Get_MappingErrorData:
                            {
                                Qry_MappingErrorData();
                            }
                            break;

                        #endregion

                        #region Set

                        case RobotStep.Set_Stop:
                            {
                                Qry_Stop();
                            }
                            break;

                        case RobotStep.Set_Restart:
                            {
                                Qry_Restart();
                            }
                            break;

                        case RobotStep.Set_RsetError:
                            {
                                Qry_ResetError();
                            }
                            break;

                        case RobotStep.Set_Speed:
                            {
                                Qry_SetSpeed();
                            }
                            break;
                        case RobotStep.Set_BackSpeed:
                            {
                                Qry_SetLowerSpeed_5();
                            }
                            break;

                        case RobotStep.Set_VacOn:
                            {
                                #region Set_VacOn

                                SetDoProcess(ReplyMsg, ArmType.Vacuum, true);

                                #endregion
                            }
                            break;

                        case RobotStep.Set_VacOff:
                            {
                                #region Set_VacOff

                                SetDoProcess(ReplyMsg, ArmType.Vacuum, false);

                                #endregion
                            }
                            break;

                        case RobotStep.Set_EdgeOn:
                            {
                                #region Set_EdgeOn

                                SetDoProcess(ReplyMsg, ArmType.EdgeGrip, true);

                                #endregion
                            }
                            break;

                        case RobotStep.Set_EdgeOff:
                            {
                                #region Set_DO_Edge_Off

                                SetDoProcess(ReplyMsg, ArmType.EdgeGrip, false);

                                #endregion
                            }
                            break;

                        case RobotStep.Set_BernoulliOn:
                            {
                                #region Set_BernoulliOn

                                //SetDoProcess(ReplyMsg, ArmType.Bernoulli, true);
                                Set_DoBernoulliOn();
                                #endregion
                            }
                            break;

                        case RobotStep.Set_BernoulliOff:
                            {
                                #region Set_BernoulliOff
                                Set_DoBernoulliOff();
                                //SetDoProcess(ReplyMsg, ArmType.Bernoulli, false);

                                #endregion
                            }
                            break;

                        case RobotStep.SetLowerArm:
                            {
                                #region SetArm

                                NowUseArm = 0;
                                Qry_Getstatus();

                                #endregion
                            }
                            break;

                        case RobotStep.SetUpperArm:
                            {
                                #region SetArm

                                NowUseArm = 1;
                                Qry_Getstatus();

                                #endregion
                            }
                            break;

                        case RobotStep.Set_Sleep_500ms:
                            {
                                Thread.Sleep(500);
                                Qry_Getstatus();
                            }
                            break;

                        #endregion

                        #region Check

                        case RobotStep.Check_VacOn:
                            {
                                #region Check_VacOn

                                if (CheckDoProcess(ReplyMsg, ArmType.Vacuum, true) == false)
                                {
                                    NowErrorList = ErrorList.RB_ArmOnFail_0118;
                                    AutoStepFail();
                                }
                                else
                                    Qry_Getstatus();

                                #endregion
                            }
                            break;

                        case RobotStep.Check_VacOff:
                            {
                                #region Check_VacOff

                                if (CheckDoProcess(ReplyMsg, ArmType.Vacuum, false) == false)
                                {
                                    NowErrorList = ErrorList.RB_ArmOffFail_0119;
                                    AutoStepFail();
                                }
                                else
                                    Qry_Getstatus();

                                #endregion
                            }
                            break;

                        case RobotStep.Check_EdgeOn:
                            {
                                #region Check_EdgeOn

                                if (CheckDoProcess(ReplyMsg, ArmType.EdgeGrip, true) == false)
                                {
                                    NowErrorList = ErrorList.RB_ArmOnFail_0118;
                                    AutoStepFail();
                                }
                                else
                                    Qry_Getstatus();

                                #endregion
                            }
                            break;

                        case RobotStep.Check_EdgeOff:
                            {
                                #region Check_Edge_Off

                                if (CheckDoProcess(ReplyMsg, ArmType.EdgeGrip, false) == false)
                                {
                                    NowErrorList = ErrorList.RB_ArmOffFail_0119;
                                    AutoStepFail();
                                }
                                else
                                    Qry_Getstatus();

                                #endregion
                            }
                            break;

                        case RobotStep.Check_BernoulliOn:
                            {
                                //20220811 Arnold
                                #region Check_BernoulliOn
                                if (!Check_DoBernoulliOn(ReplyMsg))
                                // if (CheckDoProcess(ReplyMsg, ArmType.Bernoulli, true) == false)
                                {
                                    NowErrorList = ErrorList.RB_ArmOnFail_0118;
                                    AutoStepFail();
                                }
                                else
                                    Qry_Getstatus();

                                #endregion
                            }
                            break;

                        case RobotStep.Check_BernoulliOff:
                            {
                                #region Check_BernoulliOff
                                if (!Check_DoBernoulliOff(ReplyMsg))
                                //if (CheckDoProcess(ReplyMsg, ArmType.Bernoulli, false) == false)
                                {
                                    NowErrorList = ErrorList.RB_ArmOffFail_0119;
                                    AutoStepFail();
                                }
                                else
                                    Qry_Getstatus();

                                #endregion
                            }
                            break;

                        case RobotStep.Check_WithLoop1s:
                            {
                                #region Check_WithLoop

                                WaferStatus waferstatus = CheckSingleArmPresence(TempDi_All);
                                if (waferstatus != WaferStatus.With)
                                {
                                    Check_DI = true;
                                    CheckFlagIsWith = true;
                                    Chech_NowTime = DateTime.Now;
                                }
                                Qry_Getstatus();

                                #endregion
                            }
                            break;

                        case RobotStep.Check_With:
                            {
                                #region Check_With

                                WaferStatus waferstatus = CheckSingleArmPresence(TempDi_All);
                                if (waferstatus == WaferStatus.With)
                                {
                                    Qry_Getstatus();
                                }
                                else
                                {
                                    AutoStepFail();
                                }

                                #endregion
                            }
                            break;

                        case RobotStep.Check_With_ABN:
                            {
                                #region Check_With_Wafer_ABN

                                WaferStatus waferstatus_abn = CheckSingleArmPresence(TempDi_All);
                                if (waferstatus_abn == WaferStatus.With)
                                {
                                    RetryFlag = false;
                                    Qry_Getstatus();
                                }
                                else
                                {
                                    NowStepCnt = Robot_GPTArray.FindIndex(x => x == RobotStep.Jump_AbnStart);
                                    if (NowStepCnt == -1)
                                    {
                                        AutoStepFail();
                                    }
                                    else
                                        Qry_Getstatus();
                                }

                                #endregion
                            }
                            break;

                        case RobotStep.Check_With_ABN_RetryEnd:
                            {
                                #region Check_With_ABN_RetryEnd

                                if (RetryCnt == 0)
                                {
                                    RetryFlag = true;
                                    Qry_Getstatus();
                                }
                                else
                                {
                                    NowStepCnt = Robot_GPTArray.FindIndex(x => x == RobotStep.Jump_AbnEnd);
                                    if (NowStepCnt == -1)
                                    {
                                        AutoStepFail();
                                    }
                                    else
                                    {
                                        RetryCnt--;
                                        RetryFlag = true;
                                        Qry_Getstatus();
                                    }
                                }

                                #endregion
                            }
                            break;

                        case RobotStep.Check_WithoutLoop1s:
                            {
                                #region Check_WithLoop

                                WaferStatus waferstatus = CheckSingleArmPresence(TempDi_All);
                                if (waferstatus != WaferStatus.WithOut)
                                {
                                    Check_DI = true;
                                    CheckFlagIsWith = false;
                                    Chech_NowTime = DateTime.Now;
                                }
                                Qry_Getstatus();

                                #endregion
                            }
                            break;

                        case RobotStep.Check_Without:
                            {
                                #region Check_Without

                                WaferStatus waferstatus = CheckSingleArmPresence(TempDi_All);
                                if (waferstatus == WaferStatus.WithOut)
                                {
                                    Qry_Getstatus();
                                }
                                else
                                {
                                    AutoStepFail();
                                }

                                #endregion
                            }
                            break;

                        case RobotStep.Check_Without_ABN:
                            {
                                #region Check_Without_ABN

                                WaferStatus waferstatus = CheckSingleArmPresence(TempDi_All);
                                if (waferstatus == WaferStatus.WithOut)
                                {
                                    RetryFlag = false;
                                    Qry_Getstatus();
                                }
                                else
                                {
                                    NowStepCnt = Robot_GPTArray.FindIndex(x => x == RobotStep.Jump_AbnEnd);
                                    if (NowStepCnt == -1)
                                    {
                                        AutoStepFail();
                                    }
                                    else
                                        Qry_Getstatus();

                                }

                                #endregion Check WithOut Wafer Presence
                            }
                            break;

                        case RobotStep.Check_TopR_IsTurn_Jump1:
                            {
                                #region Check_Not_Turn_Jump

                                if (NowArmStatusR == ArmStatus.Arm_Turn)
                                {

                                    NowStepCnt = Robot_GPTArray.FindIndex(x => x == RobotStep.Jump_Check1);
                                    if (NowStepCnt == -1)
                                    {
                                        AutoStepFail();
                                    }
                                    else
                                        Qry_Getstatus();
                                }
                                else
                                {
                                    Qry_Getstatus();
                                }

                                #endregion
                            }
                            break;

                        case RobotStep.Check_HomeR_NormalOrTurn_Jump1:
                            {
                                #region Check_HomeR_NormalOrTurn_Jump

                                if (NowArmStatusR > ArmStatus.Arm_Home)
                                {

                                    NowStepCnt = Robot_GPTArray.FindIndex(x => x == RobotStep.Jump_Check1);
                                    if (NowStepCnt == -1)
                                    {
                                        AutoStepFail();
                                    }
                                    else
                                        Qry_Getstatus();
                                }
                                else
                                {
                                    Qry_Getstatus();
                                }

                                #endregion
                            }
                            break;

                        case RobotStep.Check_HoemR_FinalOrUndone_Jump2:
                            {
                                #region Check_HomeR_FinalOrUndone_Jump

                                if (NowArmStatusR == ArmStatus.Arm_Now_Turn)
                                {

                                    NowStepCnt = Robot_GPTArray.FindIndex(x => x == RobotStep.Jump_Check2);
                                    if (NowStepCnt == -1)
                                    {
                                        AutoStepFail();
                                    }
                                    else
                                        Qry_Getstatus();
                                }
                                else
                                {
                                    Qry_Getstatus();
                                }

                                #endregion
                            }
                            break;

                        case RobotStep.CheckError_IsTurn:
                            {
                                #region Check_Turn_Error

                                if (NowArmStatusR == ArmStatus.Arm_Turn)
                                {
                                    AutoStepFail();
                                }
                                else
                                {
                                    Qry_Getstatus();
                                }

                                #endregion
                            }
                            break;

                        case RobotStep.CheckError_IsExtend:
                            {
                                #region Check_Extend_Error

                                if (NowArmStatusX == ArmStatus.Arm_Extend && IniArmCnt > 0)
                                {
                                    AutoStepFail();
                                }
                                else if (NowArmStatusY == ArmStatus.Arm_Extend && IniArmCnt > 1)
                                {
                                    AutoStepFail();
                                }
                                else
                                    Qry_Getstatus();

                                #endregion
                            }
                            break;

                        case RobotStep.Check_ArmWith_Jump12:
                            {
                                #region Check_ArmWith_Jump12

                                if (NowUseArm == 0 ? Ui_LowerWaferPresent == WaferStatus.WithOut : Ui_UpperWaferPresent == WaferStatus.WithOut)
                                {
                                    Qry_Getstatus();
                                }
                                else
                                {
                                    if (NowUseArm == 0)
                                        NowStepCnt = Robot_GPTArray.FindIndex(x => x == RobotStep.Jump_Check1);
                                    else
                                        NowStepCnt = Robot_GPTArray.FindIndex(x => x == RobotStep.Jump_Check2);

                                    if (NowStepCnt == -1)
                                    {
                                        AutoStepFail();
                                    }
                                    else
                                        Qry_Getstatus();

                                }

                                #endregion
                            }
                            break;

                        case RobotStep.Check_Home_GPNeedJumpEnd:
                            {
                                #region Check_NormalHome_GPNeedJumpEnd

                                //if (NowGPRobotAddress == 0 && (Ui_ArmStatusX == ArmStatus.Arm_Extend || Ui_ArmStatusY == ArmStatus.Arm_Extend))
                                //{
                                //    NowErrorList = ErrorList.RB_NowAddressNotSafty_0319;
                                //    AutoStepFail();
                                //}
                                //else
                                {
                                    bool ZBackUpResult = true;

                                    if (NowGPRobotAddress >= 100)
                                    {
                                        switch (NowGPTCommandType)
                                        {
                                            case RobotGPT.Get:
                                                {
                                                    switch (IniUseArmType[NowUseArm])
                                                    {
                                                        case ArmType.EdgeGrip:
                                                            {
                                                                #region EdgeGrip

                                                                switch (NowGPRobotAddress % 10)
                                                                {
                                                                    case 0:
                                                                    case 1:
                                                                        {
                                                                            ZBackUpResult = false;
                                                                        }
                                                                        break;

                                                                    case 4:
                                                                        {
                                                                            ZBackUpResult = false;
                                                                            NowNeedHomeChangeData = true;
                                                                        }
                                                                        break;

                                                                    case 2:
                                                                    case 3:
                                                                        {
                                                                            if ((NowUseArm == 1 && Ui_UpperWaferPresent == WaferStatus.WithOut) || (NowUseArm == 0 && Ui_LowerWaferPresent == WaferStatus.WithOut))
                                                                            {
                                                                                NowTempHomeBackAddress = NowGPRobotAddress;
                                                                                NowTempHomeBackAddress -= 1;
                                                                            }
                                                                            else
                                                                            {
                                                                                ZBackUpResult = false;
                                                                                NowNeedHomeChangeData = true;
                                                                            }
                                                                        }
                                                                        break;
                                                                }

                                                                #endregion
                                                            }
                                                            break;

                                                        default: //Pad
                                                            {
                                                                #region Pad

                                                                switch (NowGPRobotAddress % 10)
                                                                {
                                                                    case 0:
                                                                    case 1:
                                                                        {
                                                                            ZBackUpResult = false;
                                                                        }
                                                                        break;

                                                                    case 3:
                                                                        {
                                                                            ZBackUpResult = false;
                                                                            NowNeedHomeChangeData = true;
                                                                        }
                                                                        break;

                                                                    case 2:
                                                                        {
                                                                            NowTempHomeBackAddress = NowGPRobotAddress;
                                                                            NowTempHomeBackAddress -= 1;
                                                                        }
                                                                        break;
                                                                }

                                                                #endregion
                                                            }
                                                            break;
                                                    }

                                                }
                                                break;

                                            case RobotGPT.Put:
                                                {
                                                    switch (IniUseArmType[NowUseArm])
                                                    {
                                                        case ArmType.EdgeGrip:
                                                            {
                                                                #region EdgeGrip

                                                                switch (NowGPRobotAddress % 10)
                                                                {
                                                                    case 0:
                                                                        {
                                                                            ZBackUpResult = false;
                                                                        }
                                                                        break;

                                                                    case 2:
                                                                        {
                                                                            NowTempHomeBackAddress = NowGPRobotAddress;
                                                                            NowTempHomeBackAddress += 1;
                                                                            NowNeedHomeChangeData = true;
                                                                        }
                                                                        break;

                                                                    case 3:
                                                                    case 4:
                                                                        {
                                                                            ZBackUpResult = false;
                                                                            NowNeedHomeChangeData = true;
                                                                        }
                                                                        break;

                                                                    case 1:
                                                                        {
                                                                            if ((NowUseArm == 1 && Ui_UpperWaferPresent == WaferStatus.WithOut) || (NowUseArm == 0 && Ui_LowerWaferPresent == WaferStatus.WithOut))
                                                                            {
                                                                                NowTempHomeBackAddress = NowGPRobotAddress;
                                                                                NowTempHomeBackAddress += 1;
                                                                                NowNeedHomeChangeData = true;
                                                                            }
                                                                            else
                                                                            {
                                                                                ZBackUpResult = false;
                                                                            }
                                                                        }
                                                                        break;
                                                                }

                                                                #endregion
                                                            }
                                                            break;

                                                        default:
                                                            {
                                                                #region Pad

                                                                switch (NowGPRobotAddress % 10)
                                                                {
                                                                    case 0:
                                                                    case 1:
                                                                        {
                                                                            ZBackUpResult = false;
                                                                        }
                                                                        break;

                                                                    case 3:
                                                                        {
                                                                            ZBackUpResult = false;
                                                                            NowNeedHomeChangeData = true;
                                                                        }
                                                                        break;

                                                                    case 2:
                                                                        {
                                                                            NowTempHomeBackAddress = NowGPRobotAddress;
                                                                            NowTempHomeBackAddress -= 1;
                                                                        }
                                                                        break;
                                                                }

                                                                #endregion
                                                            }
                                                            break;
                                                    }

                                                }
                                                break;

                                            case RobotGPT.TopPut:
                                                {
                                                    #region TopPut

                                                    switch (NowGPRobotAddress % 10)
                                                    {
                                                        case 0:
                                                        case 1:
                                                            {
                                                                ZBackUpResult = false;
                                                            }
                                                            break;

                                                        case 4:
                                                            {
                                                                ZBackUpResult = false;
                                                                NowNeedHomeChangeData = true;
                                                            }
                                                            break;

                                                        case 2:
                                                            {
                                                                NowTempHomeBackAddress = NowGPRobotAddress;
                                                                if ((NowUseArm == 1 && Ui_UpperWaferPresent == WaferStatus.WithOut) || (NowUseArm == 0 && Ui_LowerWaferPresent == WaferStatus.WithOut))
                                                                    NowTempHomeBackAddress += 1;
                                                                else if ((NowUseArm == 1 && Ui_UpperWaferPresent == WaferStatus.With) || (NowUseArm == 0 && Ui_LowerWaferPresent == WaferStatus.With))
                                                                {
                                                                    NowTempHomeBackAddress -= 1;
                                                                    NowNeedHomeChangeData = true;
                                                                }
                                                                else
                                                                {
                                                                    NowErrorList = ErrorList.RB_ArmUnknown_0122;
                                                                    AutoStepFail();
                                                                }

                                                            }
                                                            break;

                                                        case 3:
                                                            {
                                                                NowTempHomeBackAddress = NowGPRobotAddress;
                                                                NowNeedHomeChangeData = true;
                                                            }
                                                            break;
                                                    }

                                                    #endregion
                                                }
                                                break;

                                            case RobotGPT.TopGet:
                                                {
                                                    #region TopGet

                                                    switch (NowGPRobotAddress % 10)
                                                    {
                                                        case 0:
                                                        case 1:
                                                            {
                                                                ZBackUpResult = false;
                                                            }
                                                            break;
                                                        case 4:
                                                            {
                                                                ZBackUpResult = false;
                                                                NowNeedHomeChangeData = true;
                                                            }
                                                            break;

                                                        case 2:
                                                            {
                                                                NowTempHomeBackAddress = NowGPRobotAddress;
                                                                if ((NowUseArm == 1 && Ui_UpperWaferPresent == WaferStatus.WithOut) || (NowUseArm == 0 && Ui_LowerWaferPresent == WaferStatus.WithOut))
                                                                    NowTempHomeBackAddress -= 1;
                                                                else if ((NowUseArm == 1 && Ui_UpperWaferPresent == WaferStatus.With) || (NowUseArm == 0 && Ui_LowerWaferPresent == WaferStatus.With))
                                                                {
                                                                    NowTempHomeBackAddress += 1;
                                                                    NowNeedHomeChangeData = true;
                                                                }
                                                                else
                                                                {
                                                                    NowErrorList = ErrorList.RB_ArmUnknown_0122;
                                                                    AutoStepFail();
                                                                }
                                                            }
                                                            break;

                                                        case 3:
                                                            {
                                                                NowTempHomeBackAddress = NowGPRobotAddress;
                                                                NowNeedHomeChangeData = true;
                                                            }
                                                            break;
                                                    }

                                                    #endregion
                                                }
                                                break;

                                            default:
                                                {
                                                    ZBackUpResult = false;
                                                }
                                                break;
                                        }
                                    }
                                    else
                                        ZBackUpResult = false;


                                    if (((NowUseArm == 1 && Ui_ArmStatusY == ArmStatus.Arm_Home) || (NowUseArm == 0 && Ui_ArmStatusX == ArmStatus.Arm_Home)) && NowNeedHomeChangeData == true)
                                        NowNeedHomeChangeData = false;

                                    if (ZBackUpResult == false)
                                    {
                                        if (Ui_ArmStatusR == ArmStatus.Arm_Turn)
                                            NowStepCnt = Robot_GPTArray.FindIndex(x => x == RobotStep.Jump_TopHomeEnd);
                                        else
                                            NowStepCnt = Robot_GPTArray.FindIndex(x => x == RobotStep.Jump_NornalHomeEnd);

                                        if (NowStepCnt == -1)
                                        {
                                            AutoStepFail();
                                        }
                                        else
                                            Qry_Getstatus();
                                    }
                                    else
                                        Qry_Getstatus();

                                }
                                #endregion
                            }
                            break;

                        case RobotStep.Check_Home_GPJumpStart:
                            {

                                NowStepCnt = Robot_GPTArray.FindIndex(x => x == RobotStep.Jump_NornalHomeStart);

                                if (NowStepCnt == -1)
                                {
                                    AutoStepFail();
                                }
                                else
                                    Qry_Getstatus();
                            }
                            break;

                        case RobotStep.Check_Home_PosRuleNeedJumpEnd:
                            {
                                #region Check_Home_PosRuleNeedJumpEnd

                                if (!CheckRobotHomeBackRule())
                                {
                                    NowErrorList = ErrorList.RB_NowAddressNotSafty_0319;
                                    AutoStepFail();
                                }
                                else
                                {
                                    NowGPRobotAddress = NowTempHomeBackAddress;
                                    Qry_Getstatus();
                                }

                                #endregion
                            }
                            break;

                        #endregion

                        #region Update

                        case RobotStep.Update_RobotPosition:
                            {
                                Qry_ReadPos();
                            }
                            break;

                        case RobotStep.Update_HomeBackGPPosition:
                            {
                                Qry_ReadHomeBackGPPos();
                            }
                            break;

                        case RobotStep.Update_TeachingPosition:
                            {
                                Qry_ReadTeachingPos();
                            }
                            break;

                        case RobotStep.Update_TurnStatus:
                            {
                                double _rDegree = Convert.ToDouble(RobotNodeAxisPos[(int)RobotAxis.R]);
                                Check_RAxisTurnStatus(_rDegree);
                                Qry_ReadPos();
                            }
                            break;

                        case RobotStep.Update_ArmPresence:
                            {
                                CheckAllArmPresence(TempDi_All);
                                Qry_Getstatus();
                            }
                            break;

                        #endregion

                        #region Jump

                        case RobotStep.Jump_Check1:
                        case RobotStep.Jump_Check2:
                        case RobotStep.Jump_AbnStart:
                        case RobotStep.Jump_AbnEnd:
                        case RobotStep.Jump_TopHomeStart:
                        case RobotStep.Jump_TopHomeEnd:
                        case RobotStep.Jump_NornalHomeStart:
                        case RobotStep.Jump_NornalHomeEnd:
                            {
                                Qry_Getstatus();
                            }
                            break;


                        #endregion

                        #region Wait

                        case RobotStep.Wait_BernoulliOff:
                        case RobotStep.Wait_VacOff:
                            {
                                Thread.Sleep(250);
                                Qry_Getstatus();
                            }
                            break;

                        case RobotStep.Wait_BernoulliOn:
                        case RobotStep.Wait_VacOn:
                            {
                                Thread.Sleep(250);
                                Qry_Getstatus();
                            }
                            break;

                        #endregion

                        case RobotStep.EQ_LD_start:
                            HCT_EFEM.common_docking0();
                            Qry_Getstatus();

                            break;
                        case RobotStep.EQ_LD_End:
                            HCT_EFEM.common_docking1();
                            Qry_Getstatus();

                            break;
                        case RobotStep.EQ_ULD_start:
                            HCT_EFEM.common_undocking();
                            Qry_Getstatus();

                            break;
                        case RobotStep.EQ_ULD_End:
                            HCT_EFEM.common_undockingEnd();
                            Qry_Getstatus();

                            break;

                        default:
                            //CommandQueue.EnQueue(MarcoCommand[NowStepCnt]);//送出指令
                            break;
                    }

                    while (!NextStepFlag)
                    {

                        string MarcoFeedback = StepRecQueue.DeQueue(TimeoutCount);

                        if (MarcoFeedback == null || MarcoFeedback == NormalStatic.End)
                        {
                            break;
                        }

                        //Chcek Obj Cassete Present
                        if (NowMainJab == SocketCommand.WaferGet || NowMainJab == SocketCommand.WaferPut)
                        {
                            //if (CheckDestinationPresent(NowUseObj)) //Check Object Cassette Present Error,Send Stop
                            //{
                            //    UI.Log(NormalStatic.Robot, DeviceName, SystemList.CommandParameter,
                            //           string.Format("Destination casette error , Trigger Stop at Not Moving)"));
                            //    Qry_Stop();
                            //    SpinWait.SpinUntil(() => false, 1000);

                            //    JobFail();
                            //    NowErrorList = ErrorList.RB_ActionNotCmd_0113;
                            //    NowErrorMsg = "Destination presnet Error";
                            //    ActionComplete(string.Format("{0},{1},{2},{3}", NowMainJab, NowUseArm, NowUseObj, NowUseSlotID), false, DeviceName);
                            //    continue;
                            //}
                        }

                        if (NowStopAction)
                        {
                            if (Stop)
                            {
                                NowStopAction = false;
                                NextStepFlag = true;
                                if (IniStopRestart == false)
                                    JobFail();
                                ActionComplete(string.Format("{0},{1},{2},{3}", NowMainJab, NowUseArm, NowUseObj, NowUseSlotID), false, DeviceName);
                                break;
                            }
                            else
                            {
                                CommandQueue.EnQueue("GD");
                                continue;
                            }
                        }

                        string[] Cmd = MarcoFeedback.Split(new string[] { " " }, StringSplitOptions.None);
                        switch (Cmd[0])
                        {
                            case "GP":
                            case "GD":
                                {
                                    Qry_Getstatus();
                                }
                                break;

                            case "LS":
                                {
                                    if (Check_DI && (MarcoCommand[NowStepCnt] == RobotStep.Check_WithLoop1s || MarcoCommand[NowStepCnt] == RobotStep.Check_WithoutLoop1s))
                                    {
                                        GetDiBytePdu(0);
                                    }
                                    else
                                    {
                                        if (!Moving)
                                        {
                                            NowStepCnt++;
                                            NextStepFlag = true;
                                            break;
                                        }

                                        Qry_ReadPos();
                                    }
                                }
                                break;

                            case "LR":
                                {
                                    Qry_Getstatus();
                                }
                                break;

                            case "LID0":
                                {
                                    if (Check_DI && (MarcoCommand[NowStepCnt] == RobotStep.Check_WithLoop1s || MarcoCommand[NowStepCnt] == RobotStep.Check_WithoutLoop1s))
                                    {
                                        GetDiBytePdu(1);
                                    }
                                    else
                                    {
                                        NowStepCnt++;
                                        NextStepFlag = true;
                                    }

                                }
                                break;

                            case "LID1":
                                {
                                    if (Check_DI && (MarcoCommand[NowStepCnt] == RobotStep.Check_WithLoop1s || MarcoCommand[NowStepCnt] == RobotStep.Check_WithoutLoop1s))
                                    {
                                        WaferStatus waferstatus = CheckSingleArmPresence(TempDi_All);

                                        if (((DateTime.Now - Chech_NowTime).TotalSeconds >= 2)
                                           || (waferstatus == WaferStatus.With && CheckFlagIsWith == true)
                                           || (waferstatus == WaferStatus.WithOut && CheckFlagIsWith == false))
                                        {
                                            Check_DI = false;
                                            NowStepCnt++;
                                            NextStepFlag = true;
                                        }
                                        else
                                        {
                                            GetDiBytePdu(0);
                                        }
                                    }
                                    else
                                    {
                                        NowStepCnt++;
                                        NextStepFlag = true;
                                    }
                                }
                                break;

                            default:
                                {
                                    NowStepCnt++;
                                    NextStepFlag = true;
                                }
                                break;
                        }
                    }
                }
            }
        }

        private void CommandBG_DoWork()
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
                    ReceiveNowStepCnt = NormalStatic.WaitReply;
                    DataSend(CMD_string);
                }

                REC_string = ReceiverQueue.DeQueue(TimeoutCount);//目前暫定十秒要回覆 無法多工

                // Walson 20210102追加防範機制
                if (NowStepCnt < MarcoCommand.Length)
                    UI.Log(NormalStatic.Robot, DeviceName, SystemList.CommandParameter, string.Format("Step{0}:{1} -> {2}", NowStepCnt, CMD_string, REC_string));

                if (REC_string == null)
                {
                    if (NowErrorList == ErrorList.MaxCnt)
                        NowErrorList = ErrorList.Timeout_1010;
                    JobFail();
                    ActionComplete(string.Format("{0},{1},{2},{3}", NowMainJab, NowUseArm, NowUseObj, NowUseSlotID), false, DeviceName);
                    StepRecQueue.EnQueue(NormalStatic.End);
                }
                else if (REC_string == NormalStatic.End)
                    break;

                else if (REC_string[0] == 'E')
                {
                    UI.Log(NormalStatic.Robot, DeviceName, SystemList.DeviceSend, string.Format("{0}:{1} -> {2}", MarcoCommand[NowStepCnt], CMD_string, REC_string));

                    Status_update(REC_string.Substring(1, 4));
                    string ErrorCode = REC_string.Substring(5, 2);
                    Ui_Status = ErrorCode;
                    NowErrorList = (ErrorList)HCT_EFEM.EFEM_HasTable.Robot_Error[ErrorCode];
                    JobFail();
                    ActionComplete(string.Format("{0},{1},{2},{3}", NowMainJab, NowUseArm, NowUseObj, NowUseSlotID), false, DeviceName);
                    StepRecQueue.EnQueue(NormalStatic.End);
                }
                else
                {
                    ReceiveHandler();
                }


            }
        }

        private void AutoStepFail()
        {
            if (NowErrorList == ErrorList.MaxCnt)
                NowErrorList = ErrorList.RB_StepFai_0115;

            if (NowStepCnt < MarcoCommand.Length)
                NowErrorMsg = MarcoCommand[NowStepCnt].ToString();

            JobFail();
            ActionComplete(string.Format("{0},{1},{2},{3}", NowMainJab, NowUseArm, NowUseObj, NowUseSlotID), false, DeviceName);
        }

        public void Close()
        {
            JobFail();
            NowStepCnt = 0;
            AutoStepQueue.EnQueue(new RobotStep[] { RobotStep.End });
            CommandQueue.EnQueue(NormalStatic.End);
            ReceiverQueue.EnQueue(NormalStatic.End);
            COM_Disconnect();
            AppSetting.SaveSetting(string.Format("{0}{1}", RobotName, Robot_Static.PosParamter), string.Format("{0},{1},{2},{3},{4},{5}", NowGPTCommandType, NowGPRobotAddress, NowUseArm, NowUseObj, NowUseSlotGap, NowUseSlotID));
            UI.CloseBG(DeviceName);
        }

        #endregion

        #region Stattus/Fail

        private void Status_update(string _RobotStatus)
        {
            Ui_Status = _RobotStatus;
            bool[] View = new bool[32];//有問題時可以回來看這裡
            byte[] StatusByte = Encoding.ASCII.GetBytes(_RobotStatus);
            Array.Reverse(StatusByte);
            BitArray _Status = new BitArray(StatusByte);
            for (int i = 0; i < 32; i++)
            {
                View[i] = _Status[i];
            }
            Ui_Remote = _Status[0];
            Stop = _Status[9];
            Moving = !_Status[17];//26];
        }

        private void Status_Check(string cmd, string _RobotStatus)
        {
            bool S1_CheckPass = false, S2_CheckPass = false, S3_CheckPass = false, S4_CheckPass = false;

            #region S1_Check

            string S1 = _RobotStatus.Substring(3, 1);
            switch (S1)
            {
                case "0":
                    {
                        NowErrorList = ErrorList.RB_OnlineMode_0701;
                    }
                    break;

                case "1":
                    {
                        S1_CheckPass = true;
                    }
                    break;

                case "2":
                    {
                        NowErrorList = ErrorList.RB_ManualMode_0702;
                    }
                    break;

                case "4":
                    {
                        NowErrorList = ErrorList.RB_AutoMode_0703;
                    }
                    break;

                default:
                    {
                        NowErrorList = ErrorList.RB_OverS1_0101;
                    }
                    break;
            }
            #endregion

            #region S2_Check

            if (S1_CheckPass)
            {
                string S2 = _RobotStatus.Substring(2, 1);
                switch (S2)
                {
                    case "0":
                        {
                            S2_CheckPass = true;
                        }
                        break;

                    case "1":
                        {
                            NowErrorList = ErrorList.RB_NotEQStopNotOn_0102;
                        }
                        break;

                    case "2":
                        {
                            if (cmd == "LS" || cmd == "GD")
                            {
                                S2_CheckPass = true;
                            }
                            else
                            {
                                if (NowErrorList != ErrorList.RB_AddressMovStop_0318)
                                    NowErrorList = ErrorList.RB_StopOn_0103;
                            }
                        }
                        break;

                    case "4":
                        {
                            NowErrorList = ErrorList.RB_InES_0104;
                        }
                        break;

                    case "6":
                        {
                            NowErrorList = ErrorList.RB_ESStopOn_0105;
                        }
                        break;

                    default:
                        {
                            NowErrorList = ErrorList.RB_OverS2_0106;
                        }
                        break;
                }

            }
            #endregion

            #region S3_Check

            if (S2_CheckPass)
            {
                string S3 = _RobotStatus.Substring(1, 1);
                switch (S3)
                {
                    case "0":
                        {
                            NowErrorList = ErrorList.RB_NeitherACAL_0107;
                        }
                        break;

                    case "1":
                        {
                            NowErrorList = ErrorList.RB_ZOut_0108;
                        }
                        break;

                    case "2":
                        {
                            NowErrorList = ErrorList.RB_ACALNotComplete_0109;
                        }
                        break;

                    case "5":
                    case "7":
                    case "4":
                        {
                            if (cmd == "LS" || cmd == "GP" || cmd == "GD" || cmd == "GE")
                            {
                                return;
                            }
                            else
                            {
                                NowErrorList = ErrorList.RB_PosNotComplete_0110;
                            }
                        }
                        break;

                    case "6":
                        {
                            S3_CheckPass = true;
                        }
                        break;

                    //case "7":
                    //    {
                    //        NowErrorList = ErrorList.RB_ZhigherPos_0111;
                    //    }
                    //    break;

                    default:
                        {
                            NowErrorList = ErrorList.RB_OverS3_0112;
                        }
                        break;
                }

            }
            #endregion

            #region S4_Check

            if (S3_CheckPass)
            {
                string S4 = _RobotStatus.Substring(0, 1);
                switch (S4)
                {
                    case "0":
                        {
                            S4_CheckPass = true;
                        }
                        break;

                    case "4":
                        {
                            if (!Moving)
                                NowErrorList = ErrorList.RB_ActionNotCmd_0113;
                            else
                                S4_CheckPass = true;
                        }
                        break;

                    default:
                        {
                            NowErrorList = ErrorList.RB_OverS4_0114;
                        }
                        break;

                }
            }
            #endregion

            if (S4_CheckPass)
            {

            }
            else
            {

                JobFail();
                ActionComplete(string.Format("{0},{1},{2},{3}", NowMainJab, NowUseArm, NowUseObj, NowUseSlotID), false, DeviceName);
                StepRecQueue.EnQueue(NormalStatic.End);
            }
        }

        private void JobFail()
        {
            ReceiveNowStepCnt = NormalStatic.Idle;
            CommandQueue.Clear();
            ReceiverQueue.Clear();
            NowStepCnt = 100;
            NextStepFlag = true;
            AutoStepQueue.Clear();
            StepRecQueue.Clear();
        }

        private bool CheckRestartCommand()
        {
            if ((IniStopRestart
               && (NowResumeJab == SocketCommand.WaferGet ||
                   NowResumeJab == SocketCommand.WaferPut ||
                   NowResumeJab == SocketCommand.TopWaferGet ||
                   NowResumeJab == SocketCommand.TopWaferPut)))
                return true;
            else
                return false;
        }

        private bool CheckStopCommand()
        {
            if (Moving ||
                  (NowResumeJab == SocketCommand.WaferGet ||
                   NowResumeJab == SocketCommand.WaferPut ||
                   NowResumeJab == SocketCommand.TopWaferGet ||
                   NowResumeJab == SocketCommand.TopWaferPut))
                return true;
            else
                return false;
        }

        #endregion

        #region Position

        private void UpdateRobotPosition()
        {
            #region X_Y
            for (int ArmCnt = 0; ArmCnt < IniArmCnt; ArmCnt++)
            {
                switch (ArmCnt)
                {

                    case 0:
                        {

                            if (Math.Abs((float.Parse(RobotHomeAxisPos[(int)RobotAxis.X])) - (float.Parse(RobotNodeAxisPos[(int)RobotAxis.X]))) < Robot_Static.TOROLANCE_XY)
                            {
                                Ui_ArmStatusX = ArmStatus.Arm_Home;
                            }
                            else
                            {
                                Ui_ArmStatusX = ArmStatus.Arm_Extend;
                            }
                        }
                        break;

                    case 1:
                        {
                            if (Math.Abs((float.Parse(RobotHomeAxisPos[(int)RobotAxis.Y])) - (float.Parse(RobotNodeAxisPos[(int)RobotAxis.Y]))) < Robot_Static.TOROLANCE_XY)
                            {
                                Ui_ArmStatusY = ArmStatus.Arm_Home;
                            }
                            else
                            {
                                Ui_ArmStatusY = ArmStatus.Arm_Extend;
                            }
                        }
                        break;
                }

            }
            #endregion

            #region R

            Check_RAxisTurnStatus(float.Parse(RobotNodeAxisPos[(int)RobotAxis.R]));

            #endregion

            #region Position
            //frmMain.CalTimeStart();
            float NowAxisC = float.Parse(RobotNodeAxisPos[(int)RobotAxis.C]);
            float NowAxisW = float.Parse(RobotNodeAxisPos[(int)RobotAxis.W]);
            float NowAxisZ = float.Parse(RobotNodeAxisPos[(int)RobotAxis.Z]);
            float ObjAxisC, ObjAxisW, ObjAxisZ;

            for (int i = 0; i < PosAddressMap.Length; i++)
            {
                string[] REC = Axis_PosAddressDictionay[PosAddressMap[i]].Split(new string[] { " " }, StringSplitOptions.None);

                ObjAxisC = float.Parse(REC[(int)RobotAxis.C]);
                ObjAxisW = float.Parse(REC[(int)RobotAxis.W]);
                ObjAxisZ = float.Parse(REC[(int)RobotAxis.Z]);

                if ((Math.Abs(ObjAxisC - NowAxisC) < Robot_Static.TOROLANCE_C) &&
                    (Math.Abs(ObjAxisW - NowAxisW) < Robot_Static.TOROLANCE_W) &&
                    (Math.Abs(ObjAxisZ - NowAxisZ) < Robot_Static.TOROLANCE_Z)
                    )
                {

                    //Walson 20210106
                    #region 55-0430/55-0359特例, 由於P3_Map和Stage2的C軸和W軸數值太接近, 會讓位置判斷出錯

                    if (NowMainJab >= SocketCommand.WaferGet
                    && NowMainJab <= SocketCommand.PutStandby
                    && RobotPosition.Home + i == RobotPosition.P3_Map)
                    {
                        Ui_RobotPos = RobotPosition.Stage2;
                        return;
                    }
                    #endregion

                    Ui_RobotPos = RobotPosition.Home + i;
                    return;
                }

            }
            Ui_RobotPos = RobotPosition.Unknown;

            #endregion
        }

        private bool CheckRobotPositionRule()
        {
            bool[] CheckAxis = new bool[(int)RobotAxis.MaxCnt];
            double CheckTol = Robot_Static.TOROLANCE_GP;
            switch (MarcoCommand[NowStepCnt])
            {
                #region Home

                case RobotStep.GP_Home_0:
                case RobotStep.GP_Normal_0:
                case RobotStep.GP_Mapping_0:
                    {
                        if (RetryFlag == true)
                        {
                            CheckAxis[(int)RobotAxis.W] = true;
                            CheckAxis[(int)RobotAxis.R] = true;
                            CheckAxis[(int)RobotAxis.C] = true;
                            CheckAxis[(int)RobotAxis.Z] = true;
                            if (NowUseArm == 0)
                                CheckAxis[(int)RobotAxis.Y] = true;
                            else
                                CheckAxis[(int)RobotAxis.X] = true;
                        }
                        else
                        {
                            CheckAxis[(int)RobotAxis.Y] = true;
                            CheckAxis[(int)RobotAxis.X] = true;
                        }

                    }
                    break;

                case RobotStep.GP_Home_1:
                    {
                        CheckAxis[(int)RobotAxis.W] = true;
                        CheckAxis[(int)RobotAxis.R] = true;
                        CheckAxis[(int)RobotAxis.C] = true;
                        CheckAxis[(int)RobotAxis.Z] = true;
                    }
                    break;

                case RobotStep.GP_AlignerOCR_Down:
                case RobotStep.GP_AlignerOCR_Up:
                    {
                        CheckAxis[(int)RobotAxis.W] = true;
                        CheckAxis[(int)RobotAxis.R] = true;
                        CheckAxis[(int)RobotAxis.C] = true;
                        CheckAxis[(int)RobotAxis.Z] = true;
                        if (NowUseArm == 0)
                            CheckAxis[(int)RobotAxis.Y] = true;
                        else
                            CheckAxis[(int)RobotAxis.X] = true;
                    }
                    break;

                case RobotStep.GP_Home_SnakeMotion:
                    {
                        CheckAxis[(int)RobotAxis.R] = true;
                        CheckAxis[(int)RobotAxis.C] = true;

                        if (NowUseArm == 0)
                            CheckAxis[(int)RobotAxis.Y] = true;
                        else
                            CheckAxis[(int)RobotAxis.X] = true;

                        CheckAxis[(int)RobotAxis.Z] = true;

                    }
                    break;

                #endregion

                #region Now_Not_Use

                //Now not use
                case RobotStep.GP_Normal_5:
                case RobotStep.GP_Normal_6:
                case RobotStep.GP_Normal_7:
                case RobotStep.GP_Normal_8:
                case RobotStep.GP_Normal_9:
                case RobotStep.GP_Top_5:
                case RobotStep.GP_Top_6:
                case RobotStep.GP_Top_7:
                case RobotStep.GP_Top_8:
                case RobotStep.GP_Top_9:
                    {
                        return true;
                    }
                    break;

                #endregion

                #region Mapping

                case RobotStep.GP_Mapping_1:
                case RobotStep.GP_Mapping_2:
                case RobotStep.GP_Mapping_3:
                    {
                        CheckAxis[(int)RobotAxis.W] = true;
                        CheckAxis[(int)RobotAxis.R] = true;
                        CheckAxis[(int)RobotAxis.C] = true;

                        if (MarcoCommand[NowStepCnt] == RobotStep.GP_Mapping_2)
                        {
                            CheckAxis[(int)RobotAxis.X] = true;
                            CheckAxis[(int)RobotAxis.Y] = true;
                        }
                        else if (MarcoCommand[NowStepCnt] == RobotStep.GP_Mapping_3)
                        {
                            CheckAxis[(int)RobotAxis.Z] = true;

                            if (DeviceName == "Robot1")
                                CheckAxis[(int)RobotAxis.X] = true;
                            else
                                CheckAxis[(int)RobotAxis.Y] = true;
                        }

                    }
                    break;

                #endregion

                #region GP_Normal

                case RobotStep.GP_Normal_1:
                case RobotStep.GP_Normal_2:
                case RobotStep.GP_Normal_3:
                case RobotStep.GP_Normal_4:
                    {
                        CheckAxis[(int)RobotAxis.W] = true;
                        CheckAxis[(int)RobotAxis.R] = true;
                        CheckAxis[(int)RobotAxis.C] = true;
                        switch (IniUseArmType[NowUseArm])
                        {
                            case ArmType.Pad:
                                {
                                    //Z up/down and retry Z
                                    if ((MarcoCommand[NowStepCnt] == RobotStep.GP_Normal_2) || ((RetryFlag == true)))
                                    {
                                        CheckAxis[(int)RobotAxis.X] = true;
                                        CheckAxis[(int)RobotAxis.Y] = true;
                                    }
                                    else // 1 and 3 check other arm and z
                                    {
                                        if (NowUseArm == 0)
                                            CheckAxis[(int)RobotAxis.Y] = true;
                                        else
                                            CheckAxis[(int)RobotAxis.X] = true;

                                        CheckAxis[(int)RobotAxis.Z] = true;
                                    }
                                }
                                break;

                            case ArmType.EdgeGrip:
                                {
                                    //Z up/down and retry Z
                                    if ((MarcoCommand[NowStepCnt] == RobotStep.GP_Normal_2 || MarcoCommand[NowStepCnt] == RobotStep.GP_Normal_3) || (RetryFlag == true))
                                    {
                                        CheckTol = Robot_Static.TOROLANCE_GP_4;
                                    }
                                    else // 1 and 4 check other arm and z
                                    {
                                        if (NowUseArm == 0)
                                            CheckAxis[(int)RobotAxis.Y] = true;
                                        else
                                            CheckAxis[(int)RobotAxis.X] = true;

                                        CheckAxis[(int)RobotAxis.Z] = true;
                                    }
                                }
                                break;
                        }
                    }
                    break;
                #endregion

                #region GP_Top

                case RobotStep.GP_Top_1:
                case RobotStep.GP_Top_2:
                case RobotStep.GP_Top_3:
                case RobotStep.GP_Top_4:
                    {
                        CheckAxis[(int)RobotAxis.W] = true;
                        CheckAxis[(int)RobotAxis.R] = true;
                        CheckAxis[(int)RobotAxis.C] = true;
                        if (NowUseArm == 0)
                            CheckAxis[(int)RobotAxis.Y] = true;
                        else
                            CheckAxis[(int)RobotAxis.X] = true;
                        //Z up/down and retry Z
                        if ((MarcoCommand[NowStepCnt] == RobotStep.GP_Top_2 || MarcoCommand[NowStepCnt] == RobotStep.GP_Top_3) || (RetryFlag == true))
                        {
                            CheckTol = Robot_Static.TOROLANCE_GP_4;
                        }
                        else //1/ 4 check other arm and z
                        {
                            CheckAxis[(int)RobotAxis.Z] = true;
                        }
                    }
                    break;

                #endregion

                #region Turn

                case RobotStep.GP_Turn_0:
                case RobotStep.GP_TurnBack_0:
                    {
                        CheckAxis[(int)RobotAxis.X] = true;
                        CheckAxis[(int)RobotAxis.Y] = true;
                        CheckAxis[(int)RobotAxis.R] = true;
                    }
                    break;

                case RobotStep.GP_Turn_1:
                case RobotStep.GP_Turn_2:
                case RobotStep.GP_Turn_3:
                case RobotStep.GP_TurnBack_1:
                case RobotStep.GP_TurnBack_2:
                case RobotStep.GP_TurnBack_3:
                    {
                        CheckAxis[(int)RobotAxis.W] = true;
                        CheckAxis[(int)RobotAxis.Z] = true;
                        CheckAxis[(int)RobotAxis.C] = true;

                        if (MarcoCommand[NowStepCnt] == RobotStep.GP_Turn_2
                         || MarcoCommand[NowStepCnt] == RobotStep.GP_TurnBack_2)
                        {
                            CheckAxis[(int)RobotAxis.X] = true;
                            CheckAxis[(int)RobotAxis.Y] = true;
                        }
                        else
                        {
                            CheckAxis[(int)RobotAxis.R] = true;
                        }
                    }
                    break;

                    #endregion
            }

            for (int i = 0; i < (int)RobotAxis.MaxCnt; i++)
            {
                if (CheckAxis[i] != true)
                {
                    if (i == (int)RobotAxis.Z && MarcoCommand[NowStepCnt] == RobotStep.GP_Normal_2 &&
                        (Math.Abs((float.Parse(RobotNodeAxisPos[(int)RobotAxis.Z])) - (float.Parse(RobotMoveAxisPos[(int)RobotAxis.Z]))) > NowUseSlotGap))
                    {
                        NowErrorMsg = "Z move over Slot Gap";
                        return false;
                    }
                    else
                        continue;
                }
                if (Math.Abs((float.Parse(RobotNodeAxisPos[i])) - (float.Parse(RobotMoveAxisPos[i]))) > CheckTol)
                {
                    NowErrorMsg = string.Format("(Axis {0})", (RobotAxis)i);
                    return false;
                }
            }
            return true;
        }

        private bool CheckRobotHomeBackRule()
        {
            bool[] CheckAxis = new bool[(int)RobotAxis.MaxCnt];
            double CheckTol = Robot_Static.TOROLANCE_GP;
            CheckAxis[(int)RobotAxis.W] = true;
            CheckAxis[(int)RobotAxis.R] = true;
            CheckAxis[(int)RobotAxis.C] = true;

            switch (IniUseArmType[NowUseArm])
            {

                case ArmType.EdgeGrip:
                    {
                        #region EdgeGrip
                        CheckTol = Robot_Static.TOROLANCE_GP_4;

                        switch (NowGPRobotAddress % 10)
                        {
                            case 2:
                            case 3:
                                {
                                    if (NowUseArm == 0)
                                        CheckAxis[(int)RobotAxis.Y] = true;
                                    else
                                        CheckAxis[(int)RobotAxis.X] = true;
                                }
                                break;
                        }

                        #endregion
                    }
                    break;

                default: //Pad
                    {
                        if ((NowTempHomeBackAddress % 10) != 2)
                        {
                            if (NowUseArm == 0)
                                CheckAxis[(int)RobotAxis.Y] = true;
                            else
                                CheckAxis[(int)RobotAxis.X] = true;
                        }
                    }
                    break;

            }


            for (int i = 0; i < (int)RobotAxis.MaxCnt; i++)
            {
                if (CheckAxis[i] != true)
                {
                    switch (i)
                    {
                        case (int)RobotAxis.Z:
                            {
                                if (Math.Abs((float.Parse(RobotNodeAxisPos[i])) - (float.Parse(RobotBackGPAxisPos[i])) - (Convert.ToDouble(NowUseSlotID) - 1) * NowUseSlotGap) > NowUseSlotGap)
                                    return false;
                                else
                                    continue;
                            }
                            break;

                        default:
                            {
                                if ((IniUseArmType[NowUseArm] == ArmType.EdgeGrip) && Math.Abs((float.Parse(RobotNodeAxisPos[i])) - (float.Parse(RobotBackGPAxisPos[i]))) > CheckTol)
                                    return false;
                                else
                                    continue;
                            }
                            break;
                    }
                }

                if (Math.Abs((float.Parse(RobotNodeAxisPos[i])) - (float.Parse(RobotBackGPAxisPos[i]))) > CheckTol)
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region Interlock

        public void SetInterlock(bool _Flag, ref SocketCommand _RobotCmd)
        {
            switch (_RobotCmd)
            {
                case SocketCommand.Home:
                case SocketCommand.Initial:
                case SocketCommand.InitialHome:
                case SocketCommand.Stop:
                case SocketCommand.ReStart:
                case SocketCommand.GetStatus:
                case SocketCommand.ResetError:
                case SocketCommand.ReadPosition:
                case SocketCommand.GetRobotMappingResult:
                case SocketCommand.GetRobotMappingResult2:
                case SocketCommand.GetRobotMappingErrorResult:
                case SocketCommand.GetRobotMappingErrorResult2:
                case SocketCommand.CheckArmOnSafetyPos:
                case SocketCommand.CheckWaferPresence:
                case SocketCommand.SetRobotSpeed:

                case SocketCommand.WaferGet:
                case SocketCommand.WaferPut:
                case SocketCommand.TopWaferGet:
                case SocketCommand.TopWaferPut:
                case SocketCommand.GetStandby:
                case SocketCommand.PutStandby:
                case SocketCommand.TopGetStandby:
                case SocketCommand.TopPutStandby:
                case SocketCommand.RobotMapping:

                case SocketCommand.BernoulliOn:
                case SocketCommand.BernoulliOff:
                //     case SocketCommand.EdgeGripOn:
                //     case SocketCommand.EdgeGripOff:
                case SocketCommand.VacuumOff:
                case SocketCommand.VacuumOn:
                case SocketCommand.ArmSafetyPosition:
                    //      case SocketCommand.Move_OCRReadPosition:
                    {
                        Ui_Busy = _Flag;
                    }
                    break;
            }

        }

        #endregion

        #region Reset

        private void ResetStepStart(ref SocketCommand _RobotCmd)
        {
            NowStepCnt = 0;
            NowStepGPCnt = 0;
            NowMainJab = _RobotCmd;
            NowResumeJab = NowMainJab;
            RetryFlag = false;
            RetryCnt = IniRetryCount;
            Robot_GPTArray.Clear();
            NowStopAction = false;
            NowErrorList = ErrorList.MaxCnt;
            NowErrorMsg = "";
            Check_DI = false;
            Check_PLC = false;
            Check_PLC_ReceiveIsOK = "";
        }

        #endregion

        #region Signal_Command

        private void Qry_Restart()
        {
            CommandQueue.EnQueue("GE");
        }

        private void Qry_ResetError()
        {
            CommandQueue.EnQueue("CL");
        }

        private void Qry_Stop()
        {
            CommandQueue.EnQueue("GD");
        }

        private void Qry_SetSpeed()
        {
            CommandQueue.EnQueue(string.Format("{0}{1}{2}{3}{4}", "SP", NormalStatic.Space, Speed, NormalStatic.Space, Speed));
        }

        private void Qry_SetLowerSpeed_5()
        {
            CommandQueue.EnQueue(string.Format("{0}{1}{2}{3}{4}", "SP", NormalStatic.Space, "5", NormalStatic.Space, "5"));
        }

        private void Qry_Getstatus()
        {
            CommandQueue.EnQueue("LS");
        }

        private void Qry_ReadLBD()
        {
            CommandQueue.EnQueue("LBD");
        }

        private void Qry_ReadLB_X()
        {
            CommandQueue.EnQueue("LB 7");
        }

        private void Qry_ReadLB_Y()
        {
            CommandQueue.EnQueue("LB 8");
        }

        private void Qry_ReadPos()
        {
            CommandQueue.EnQueue("LR");
        }

        private void Qry_ReadTeachingPos()
        {
            CommandQueue.EnQueue("LD" + PosAddressMap[AddressMapCnt]);
        }

        private void Qry_ReadHomeBackGPPos()
        {
            CommandQueue.EnQueue("LD" + NowTempHomeBackAddress);
        }

        private void Qry_MappingData()
        {
            CommandQueue.EnQueue("LE110");
        }

        private void Qry_MappingErrorData()
        {
            CommandQueue.EnQueue("LE111");
        }

        #endregion

        #region Process_Command

        public void ProcessSingleCommand(SocketCommand command)
        {

            UI.Log(NormalStatic.Robot, DeviceName, SystemList.CommandStart, string.Format("{0}:({1})({2})({3})", command, NowUseArm, NowUseObj, NowUseSlotID));
            UI.Log(NormalStatic.System, DeviceName, SystemList.CommandStart, string.Format("{0}:({1})({2})({3})", command, NowUseArm, NowUseObj, NowUseSlotID));

            if ((command == SocketCommand.Stop && CheckStopCommand()) ||
               (command == SocketCommand.ReStart && CheckRestartCommand()))
            {
                ;
            }
            else
            {
                JobFail();
                ResetStepStart(ref command);
                SetInterlock(true, ref command);

            }

            switch (command)
            {
                #region Normal

                case SocketCommand.Initial:
                    {
                        CleanPosAddress();
                        SetInitialStep();
                    }
                    break;

                case SocketCommand.InitialHome:
                case SocketCommand.Home:
                    {
                        SetHomeStep();
                    }
                    break;

                case SocketCommand.ArmSafetyPosition:
                    {
                        SetArmSafetyStep();
                    }
                    break;

                case SocketCommand.ResetError:
                    {
                        SetResetErrorStep();
                    }
                    break;

                case SocketCommand.Stop:
                    {
                        if (CheckStopCommand())
                        {
                            NowErrorList = ErrorList.RB_StopOn_0103;
                            NowStopAction = true;
                        }
                        else
                            SetStopStep();
                    }
                    break;

                case SocketCommand.ReStart:
                    {
                        if (CheckRestartCommand())
                            Qry_Restart();
                        else
                            SetRestartStep();
                    }
                    break;

                #endregion

                #region Set

                case SocketCommand.SetRobotSpeed:
                    {
                        SetSpeed();
                    }
                    break;

                case SocketCommand.BernoulliOn:
                case SocketCommand.BernoulliOff:
                //  case SocketCommand.EdgeGripOn:
                //  case SocketCommand.EdgeGripOff:
                case SocketCommand.VacuumOn:
                case SocketCommand.VacuumOff:
                    {
                        SetArmStatusStep();
                    }
                    break;

                #endregion

                #region Get
                //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                case SocketCommand.GetStatus:
                    {
                        SetCheckPresenceStep();
                    }
                    break;

                case SocketCommand.CheckArmOnSafetyPos:
                case SocketCommand.ReadPosition:
                    {
                        GetRobotPosition();
                    }
                    break;

                case SocketCommand.CheckWaferPresence:
                    {
                        SetCheckPresenceStep();
                    }
                    break;

                case SocketCommand.GetRobotMappingResult:
                case SocketCommand.GetRobotMappingResult2:
                    {
                        GetMappingData();
                    }
                    break;

                case SocketCommand.GetRobotMappingErrorResult:
                case SocketCommand.GetRobotMappingErrorResult2:
                    {
                        GetMappingErrorData();
                    }
                    break;

                #endregion

                #region Mov

                case SocketCommand.RobotMapping:
                    {
                        NowGPTCommandType = RobotGPT.Mapping;
                        SetMappingStep();
                    }
                    break;

                case SocketCommand.WaferGet:
                    {
                        NowGPTCommandType = RobotGPT.Get;
                        GPT_StepRule();
                        SetGetStep();
                    }
                    break;

                case SocketCommand.WaferPut:
                    {
                        NowGPTCommandType = RobotGPT.Put;
                        GPT_StepRule();
                        SetPutStep();
                    }
                    break;

                case SocketCommand.TopWaferGet:
                    {
                        NowGPTCommandType = RobotGPT.TopGet;
                        GPT_StepRule();
                        SetTopGetStep();
                    }
                    break;

                case SocketCommand.TopWaferPut:
                    {
                        NowGPTCommandType = RobotGPT.TopPut;
                        GPT_StepRule();
                        SetTopPutStep();
                    }
                    break;

                case SocketCommand.GetStandby:
                case SocketCommand.PutStandby:
                case SocketCommand.TopGetStandby:
                case SocketCommand.TopPutStandby:
                    {
                        GPT_StepRule();
                        SetStandbyStep();
                    }
                    break;

                    #endregion
            }
        }

        #endregion

        #region Step

        private void SetStandbyStep()
        {
            switch (NowMainJab)
            {
                //Check
                case SocketCommand.PutStandby:
                case SocketCommand.GetStandby:
                    {
                        Robot_GPTArray.AddRange(Set_Speed);
                        Robot_GPTArray.AddRange(Check_AllInterLock);
                        Robot_GPTArray.AddRange(GP_0);
                    }
                    break;

                //Check
                case SocketCommand.TopGetStandby:
                    {
                        Robot_GPTArray.AddRange(Set_Speed);
                        Robot_GPTArray.AddRange(Check_ExtendInterLock);

                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_Without);

                        Robot_GPTArray.AddRange(Check_TopRTurn_Jump1);   // Jump1
                        //Turn
                        Robot_GPTArray.AddRange(Turn_0);
                        Robot_GPTArray.AddRange(Turn_1);
                        Robot_GPTArray.AddRange(Turn_2);
                        Robot_GPTArray.AddRange(Turn_3);

                        Robot_GPTArray.AddRange(Jump_Check1);            // Jump1

                        Robot_GPTArray.AddRange(GP_Top0);
                    }
                    break;

                //Check
                case SocketCommand.TopPutStandby:
                    {
                        Robot_GPTArray.AddRange(Set_Speed);
                        Robot_GPTArray.AddRange(Check_ExtendInterLock);

                        Robot_GPTArray.AddRange(Check_TopRTurn_Jump1);   // Jump1
                        //Turn
                        Robot_GPTArray.AddRange(Turn_0);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);

                        Robot_GPTArray.AddRange(Turn_1);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);

                        Robot_GPTArray.AddRange(Turn_2);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);

                        Robot_GPTArray.AddRange(Turn_3);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);

                        Robot_GPTArray.AddRange(Jump_Check1);           // Jump1

                        Robot_GPTArray.AddRange(GP_Top0);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);
                    }
                    break;
            }
            Robot_GPTArray.AddRange(End_Step);
            AutoStepQueue.EnQueue(Robot_GPTArray.ToArray<RobotStep>());
        }

        private void SetGetStep()
        {
            switch (IniUseArmType[NowUseArm])
            {
                case ArmType.Vacuum:
                    {
                        #region Vac_OK

                        //Action_Check
                        Robot_GPTArray.AddRange(Set_Speed);
                        Robot_GPTArray.AddRange(Check_AllInterLock);
                        //Check No wafer
                        Robot_GPTArray.AddRange(Check_Without);
                        //Standby
                        Robot_GPTArray.AddRange(GP_0);
                        //ABN_GP
                        Robot_GPTArray.AddRange(Jump_AbnEnd);                    //JumpEnd

                        Robot_GPTArray.AddRange(GP_1);
                        Robot_GPTArray.AddRange(GP_2);
                        //Set DO
                        Robot_GPTArray.AddRange(Set_VacOn);
                        Robot_GPTArray.AddRange(Get_DI);
                        //Check_With
                        Robot_GPTArray.AddRange(Check_WithABN_Start);            //JumpStart
                        //Check_With_OK
                        Robot_GPTArray.AddRange(GP_3);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);
                        Robot_GPTArray.AddRange(End_Step);
                        //Check_With_Fail
                        Robot_GPTArray.AddRange(Jump_AbnStart);                   //JumpStart

                        Robot_GPTArray.AddRange(Set_VacOff);
                        //Check_Retry
                        Robot_GPTArray.AddRange(Check_WithABN_End);               //JumpEnd
                        //RetryCnt0
                        Robot_GPTArray.AddRange(GP_1);
                        Robot_GPTArray.AddRange(GP_0);

                        #endregion
                    }
                    break;

                case ArmType.Pad:
                    {
                        #region Pad

                        //Action_Check
                        Robot_GPTArray.AddRange(Set_Speed);
                        Robot_GPTArray.AddRange(Check_AllInterLock);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_Without);
                        //Standby
                        Robot_GPTArray.AddRange(GP_0);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_Without);

                        //ABN_GP
                        Robot_GPTArray.AddRange(Jump_AbnEnd);

                        Robot_GPTArray.AddRange(GP_1);
                        //Robot_GPTArray.AddRange(Get_DI);
                        //Robot_GPTArray.AddRange(Check_Without);

                        Robot_GPTArray.AddRange(GP_2);
                        Robot_GPTArray.Add(RobotStep.Set_Sleep_500ms);
                        Robot_GPTArray.AddRange(Get_DI);
                        //Check_With
                        Robot_GPTArray.AddRange(Check_WithABN_Start);
                        //Check_With_OK
                        Robot_GPTArray.AddRange(GP_3);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);
                        Robot_GPTArray.AddRange(End_Step);
                        //Check_With_Fail
                        Robot_GPTArray.AddRange(Jump_AbnStart);
                        //Check_Retry
                        Robot_GPTArray.AddRange(Check_WithABN_End);
                        //RetryCnt0
                        Robot_GPTArray.AddRange(GP_1);
                        Robot_GPTArray.AddRange(GP_0);
                        #endregion
                    }
                    break;

                case ArmType.EdgeGrip:
                    {
                        #region Edge

                        //Action_Check
                        Robot_GPTArray.AddRange(Set_Speed);
                        Robot_GPTArray.AddRange(Check_AllInterLock);
                        //Check No wafer
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_Without);
                        //Standby
                        Robot_GPTArray.AddRange(GP_0);
                        Robot_GPTArray.AddRange(GP_1);
                        //ABN_GP
                        Robot_GPTArray.AddRange(Jump_AbnEnd);

                        Robot_GPTArray.AddRange(GP_2);
                        Robot_GPTArray.AddRange(GP_3);
                        //Set DO
                        Robot_GPTArray.AddRange(Set_EdgeOn);
                        Robot_GPTArray.AddRange(Get_DI);
                        //Check_With
                        Robot_GPTArray.AddRange(Check_WithABN_Start);
                        //Check_With_OK
                        Robot_GPTArray.AddRange(GP_4);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);
                        Robot_GPTArray.AddRange(End_Step);
                        //Check_With_Fail
                        Robot_GPTArray.AddRange(Jump_AbnStart);

                        Robot_GPTArray.AddRange(Set_EdgeOff);
                        //Check_Retry
                        Robot_GPTArray.AddRange(Check_WithABN_End);
                        //RetryCnt0
                        Robot_GPTArray.AddRange(GP_2);
                        Robot_GPTArray.AddRange(GP_1);
                        Robot_GPTArray.AddRange(GP_0);
                        #endregion
                    }
                    break;

                case ArmType.Bernoulli:
                    {
                        #region Bernoulli

                        //Action_Check
                        Robot_GPTArray.AddRange(Set_Speed);
                        Robot_GPTArray.AddRange(Check_AllInterLock);
                        //Check No wafer
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_Without);
                        //Standby
                        Robot_GPTArray.AddRange(GP_0);
                        //ABN_GP
                        Robot_GPTArray.AddRange(Jump_AbnEnd);

                        if (NowUseObj == "EQ1")
                        {
                            Robot_GPTArray.AddRange(EQ_ULDstart);
                        }

                        Robot_GPTArray.AddRange(GP_1);
                        //Set DO
                        Robot_GPTArray.AddRange(Set_BernoulliOn);

                        Robot_GPTArray.AddRange(GP_2);
                        Robot_GPTArray.AddRange(Get_DI);
                        //Check_With
                        Robot_GPTArray.AddRange(Check_WithABN_Start);
                        //Check_With_OK
                        Robot_GPTArray.AddRange(GP_3);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);

                        if (NowUseObj == "EQ1")
                        {
                            Robot_GPTArray.AddRange(EQ_ULDend);
                        }

                        Robot_GPTArray.AddRange(End_Step);
                        //Check_With_Fail
                        Robot_GPTArray.AddRange(Jump_AbnStart);

                        Robot_GPTArray.AddRange(Set_BernoulliOff);
                        //Check_Retry
                        Robot_GPTArray.AddRange(Check_WithABN_End);
                        //RetryCnt0
                        Robot_GPTArray.AddRange(GP_1);
                        Robot_GPTArray.AddRange(GP_0);

                        #endregion
                    }
                    break;

                case ArmType.EdgeAndBernoulli:
                    {
                        #region EdgeAndBernoulli

                        //Action_Check
                        Robot_GPTArray.AddRange(Set_Speed);
                        Robot_GPTArray.AddRange(Check_AllInterLock);
                        //Check No wafer
                        Robot_GPTArray.AddRange(Set_BernoulliOn);
                        Robot_GPTArray.AddRange(Set_EdgeOn);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_Without);
                        Robot_GPTArray.AddRange(Set_EdgeOff);
                        Robot_GPTArray.AddRange(Set_BernoulliOff);
                        //Standby
                        Robot_GPTArray.AddRange(GP_0);
                        //ABN_GP
                        Robot_GPTArray.AddRange(Jump_AbnEnd);

                        Robot_GPTArray.AddRange(GP_1);
                        //Set DO
                        Robot_GPTArray.AddRange(Set_BernoulliOn);

                        Robot_GPTArray.AddRange(GP_2);
                        Robot_GPTArray.AddRange(Set_EdgeOn);
                        Robot_GPTArray.AddRange(Get_DI);
                        //Check_With
                        Robot_GPTArray.AddRange(Check_WithABN_Start);
                        //Check_With_OK
                        Robot_GPTArray.AddRange(GP_3);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);
                        Robot_GPTArray.AddRange(End_Step);
                        //Check_With_Fail
                        Robot_GPTArray.AddRange(Jump_AbnStart);

                        Robot_GPTArray.AddRange(Set_EdgeOff);
                        Robot_GPTArray.AddRange(Set_BernoulliOff);
                        //Check_Retry
                        Robot_GPTArray.AddRange(Check_WithABN_End);
                        //RetryCnt0
                        Robot_GPTArray.AddRange(GP_1);
                        Robot_GPTArray.AddRange(GP_0);

                        #endregion
                    }
                    break;
            }

            Robot_GPTArray.AddRange(End_Step);
            AutoStepQueue.EnQueue(Robot_GPTArray.ToArray<RobotStep>());
        }

        private void SetPutStep()
        {
            switch (IniUseArmType[NowUseArm])
            {

                case ArmType.Vacuum:
                    {
                        #region Vac

                        //Action_Check
                        Robot_GPTArray.AddRange(Set_Speed);
                        Robot_GPTArray.AddRange(Check_AllInterLock);
                        //Check has wafer
                        Robot_GPTArray.AddRange(Check_With);
                        //Standby
                        Robot_GPTArray.AddRange(GP_0);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);

                        Robot_GPTArray.AddRange(GP_1);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);
                        //Vac Off
                        Robot_GPTArray.AddRange(Set_VacOff);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_Without);

                        Robot_GPTArray.AddRange(GP_2);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_Without);

                        Robot_GPTArray.AddRange(GP_3);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_Without);

                        #endregion
                    }
                    break;

                case ArmType.Pad:
                    {
                        #region Pad
                        //Action_Check
                        Robot_GPTArray.AddRange(Set_Speed);
                        Robot_GPTArray.AddRange(Check_AllInterLock);
                        //Check has wafer
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);
                        //Standby
                        Robot_GPTArray.AddRange(GP_0);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);
                        //GP_1
                        Robot_GPTArray.AddRange(GP_1);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);
                        //GP_2
                        Robot_GPTArray.AddRange(GP_2);
                        //Robot_GPTArray.AddRange(Get_DI);
                        //Robot_GPTArray.AddRange(Check_Without);
                        //GP_3
                        Robot_GPTArray.AddRange(GP_3);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_Without);
                        #endregion
                    }
                    break;

                case ArmType.EdgeGrip:
                    {
                        #region Edge
                        //Action_Check
                        Robot_GPTArray.AddRange(Set_Speed);
                        Robot_GPTArray.AddRange(Check_AllInterLock);
                        //Check has wafer
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);
                        //Standby
                        Robot_GPTArray.AddRange(GP_0);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);
                        //GP_1
                        Robot_GPTArray.AddRange(GP_1);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);
                        //Off
                        Robot_GPTArray.AddRange(Set_EdgeOff);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_Without);
                        //GP_2
                        Robot_GPTArray.AddRange(GP_2);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_Without);
                        //GP_3
                        Robot_GPTArray.AddRange(GP_3);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_Without);
                        //GP_4
                        Robot_GPTArray.AddRange(GP_4);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_Without);

                        #endregion
                    }
                    break;

                case ArmType.Bernoulli:
                    {
                        #region Bernoulli

                        Robot_GPTArray.AddRange(Set_Speed);
                        Robot_GPTArray.AddRange(Check_AllInterLock);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);

                        Robot_GPTArray.AddRange(GP_0);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);

                        //Start
                        if (NowUseObj == "EQ1")
                        {
                            Robot_GPTArray.AddRange(EQ_LDstart);
                        }

                        Robot_GPTArray.AddRange(GP_1);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);

                        Robot_GPTArray.AddRange(Set_BernoulliOff);

                        Robot_GPTArray.AddRange(GP_2);
                        Robot_GPTArray.AddRange(Get_DI);
                        //  Robot_GPTArray.AddRange(Check_Without);

                        Robot_GPTArray.AddRange(GP_3);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_Without);


                        //end
                        if (NowUseObj == "EQ1")
                        {
                            Robot_GPTArray.AddRange(EQ_LDend);
                        }
                        #endregion
                    }
                    break;

                case ArmType.EdgeAndBernoulli:
                    {
                        #region EdgeAndBernoulli

                        Robot_GPTArray.AddRange(Set_Speed);
                        Robot_GPTArray.AddRange(Check_AllInterLock);

                        Robot_GPTArray.AddRange(Set_BernoulliOn);
                        Robot_GPTArray.AddRange(Set_EdgeOn);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);

                        Robot_GPTArray.AddRange(GP_0);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);

                        Robot_GPTArray.AddRange(GP_1);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);

                        Robot_GPTArray.AddRange(Set_EdgeOff);
                        Robot_GPTArray.AddRange(Set_BernoulliOff);

                        Robot_GPTArray.AddRange(GP_2);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_Without);

                        Robot_GPTArray.AddRange(GP_3);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_Without);

                        #endregion
                    }
                    break;
            }
            Robot_GPTArray.AddRange(End_Step);
            AutoStepQueue.EnQueue(Robot_GPTArray.ToArray<RobotStep>());
        }

        private void SetTopGetStep()
        {
            switch (IniUseArmType[NowUseArm])
            {
                case ArmType.EdgeGrip:
                    {
                        #region EdgeGrip

                        Robot_GPTArray.AddRange(Set_Speed);
                        Robot_GPTArray.AddRange(Check_ExtendInterLock);

                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_Without);

                        Robot_GPTArray.AddRange(Check_TopRTurn_Jump1);
                        //Turn
                        Robot_GPTArray.AddRange(Turn_0);
                        Robot_GPTArray.AddRange(Turn_1);
                        Robot_GPTArray.AddRange(Turn_2);
                        Robot_GPTArray.AddRange(Turn_3);

                        Robot_GPTArray.AddRange(Jump_Check1);
                        Robot_GPTArray.AddRange(GP_Top0);
                        //ABN_GP
                        Robot_GPTArray.AddRange(Jump_AbnEnd);

                        Robot_GPTArray.AddRange(GP_Top1);
                        Robot_GPTArray.AddRange(GP_Top2);

                        Robot_GPTArray.AddRange(Set_EdgeOn);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_WithABN_Start);

                        Robot_GPTArray.AddRange(GP_Top3);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);

                        Robot_GPTArray.AddRange(GP_Top4);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);
                        //TurnBack
                        Robot_GPTArray.AddRange(TurnBack_0);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);

                        Robot_GPTArray.AddRange(TurnBack_1);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);

                        Robot_GPTArray.AddRange(TurnBack_2);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);

                        Robot_GPTArray.AddRange(TurnBack_3);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);

                        Robot_GPTArray.AddRange(End_Step);

                        //Check_With_Fail
                        Robot_GPTArray.AddRange(Jump_AbnStart);

                        Robot_GPTArray.AddRange(Set_EdgeOff);
                        //Check_Retry
                        Robot_GPTArray.AddRange(Check_WithABN_End);
                        //RetryCnt0
                        Robot_GPTArray.AddRange(GP_Top3);
                        Robot_GPTArray.AddRange(GP_Top4);
                        Robot_GPTArray.AddRange(TurnBack_0);
                        Robot_GPTArray.AddRange(TurnBack_1);
                        Robot_GPTArray.AddRange(TurnBack_2);
                        Robot_GPTArray.AddRange(TurnBack_3);

                        #endregion
                    }
                    break;

                case ArmType.Bernoulli:
                    {
                        #region Bernoulli

                        Robot_GPTArray.AddRange(Set_Speed);
                        Robot_GPTArray.AddRange(Check_ExtendInterLock);

                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_Without);

                        Robot_GPTArray.AddRange(Check_TopRTurn_Jump1);
                        //Turn
                        Robot_GPTArray.AddRange(Turn_0);
                        Robot_GPTArray.AddRange(Turn_1);
                        Robot_GPTArray.AddRange(Turn_2);
                        Robot_GPTArray.AddRange(Turn_3);

                        Robot_GPTArray.AddRange(Jump_Check1);

                        Robot_GPTArray.AddRange(GP_Top0);
                        //ABN_GP
                        Robot_GPTArray.AddRange(Jump_AbnEnd);

                        Robot_GPTArray.AddRange(GP_Top1);
                        Robot_GPTArray.AddRange(GP_Top2);

                        Robot_GPTArray.AddRange(Set_BernoulliOn);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_WithABN_Start);

                        Robot_GPTArray.AddRange(GP_Top3);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);

                        Robot_GPTArray.AddRange(GP_Top4);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);
                        //TurnBack
                        Robot_GPTArray.AddRange(TurnBack_0);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);

                        Robot_GPTArray.AddRange(TurnBack_1);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);

                        Robot_GPTArray.AddRange(TurnBack_2);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);

                        Robot_GPTArray.AddRange(TurnBack_3);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);

                        Robot_GPTArray.AddRange(End_Step);

                        //Check_With_Fail
                        Robot_GPTArray.AddRange(Jump_AbnStart);

                        Robot_GPTArray.AddRange(Set_BernoulliOff);
                        //Check_Retry
                        Robot_GPTArray.AddRange(Check_WithABN_End);
                        //RetryCnt0
                        Robot_GPTArray.AddRange(GP_Top3);
                        Robot_GPTArray.AddRange(GP_Top4);

                        Robot_GPTArray.AddRange(TurnBack_0);
                        Robot_GPTArray.AddRange(TurnBack_1);
                        Robot_GPTArray.AddRange(TurnBack_2);
                        Robot_GPTArray.AddRange(TurnBack_3);

                        #endregion
                    }
                    break;
            }

            Robot_GPTArray.AddRange(End_Step);
            AutoStepQueue.EnQueue(Robot_GPTArray.ToArray<RobotStep>());
        }

        private void SetTopPutStep()
        {
            switch (IniUseArmType[NowUseArm])
            {
                case ArmType.EdgeGrip:
                    {
                        #region EdgeGrip

                        Robot_GPTArray.AddRange(Set_Speed);
                        Robot_GPTArray.AddRange(Check_ExtendInterLock);

                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);

                        Robot_GPTArray.AddRange(Check_TopRTurn_Jump1);
                        //Turn
                        Robot_GPTArray.AddRange(Turn_0);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);

                        Robot_GPTArray.AddRange(Turn_1);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);

                        Robot_GPTArray.AddRange(Turn_2);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);

                        Robot_GPTArray.AddRange(Turn_3);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);

                        Robot_GPTArray.AddRange(Jump_Check1);
                        //Top_0
                        Robot_GPTArray.AddRange(GP_Top0);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);
                        //Top_1
                        Robot_GPTArray.AddRange(GP_Top1);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);
                        //Top_2
                        Robot_GPTArray.AddRange(GP_Top2);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);
                        //Off
                        Robot_GPTArray.AddRange(Set_EdgeOff);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_Without);
                        //Top_3
                        Robot_GPTArray.AddRange(GP_Top3);
                        //Top_4
                        Robot_GPTArray.AddRange(GP_Top4);

                        //TurnBack
                        Robot_GPTArray.AddRange(TurnBack_0);
                        Robot_GPTArray.AddRange(TurnBack_1);
                        Robot_GPTArray.AddRange(TurnBack_2);
                        Robot_GPTArray.AddRange(TurnBack_3);

                        #endregion
                    }
                    break;

                case ArmType.Bernoulli:
                    {
                        #region Bernoulli

                        Robot_GPTArray.AddRange(Set_Speed);
                        Robot_GPTArray.AddRange(Check_ExtendInterLock);

                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);

                        Robot_GPTArray.AddRange(Check_TopRTurn_Jump1);

                        Robot_GPTArray.AddRange(Turn_0);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);

                        Robot_GPTArray.AddRange(Turn_1);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);

                        Robot_GPTArray.AddRange(Turn_2);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);

                        Robot_GPTArray.AddRange(Turn_3);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);

                        Robot_GPTArray.AddRange(Jump_Check1);

                        Robot_GPTArray.AddRange(GP_Top0);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);

                        Robot_GPTArray.AddRange(GP_Top1);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);

                        Robot_GPTArray.AddRange(GP_Top2);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_With);

                        Robot_GPTArray.AddRange(Set_BernoulliOff);
                        Robot_GPTArray.AddRange(Get_DI);
                        Robot_GPTArray.AddRange(Check_Without);

                        Robot_GPTArray.AddRange(GP_Top3);

                        Robot_GPTArray.AddRange(GP_Top4);

                        Robot_GPTArray.AddRange(TurnBack_0);

                        Robot_GPTArray.AddRange(TurnBack_1);

                        Robot_GPTArray.AddRange(TurnBack_2);

                        Robot_GPTArray.AddRange(TurnBack_3);

                        #endregion
                    }
                    break;
            }

            Robot_GPTArray.AddRange(End_Step);
            AutoStepQueue.EnQueue(Robot_GPTArray.ToArray<RobotStep>());
        }

        private void SetMappingStep()
        {
            Robot_GPTArray.AddRange(Set_Speed);
            Robot_GPTArray.AddRange(Mapping_0);
            Robot_GPTArray.AddRange(Mapping_1);
            Robot_GPTArray.AddRange(Mapping_2);
            Robot_GPTArray.AddRange(Mapping_3);
            Robot_GPTArray.Add(RobotStep.Get_MappingData);
            Robot_GPTArray.Add(RobotStep.Get_MappingErrorData);

            Robot_GPTArray.AddRange(End_Step);
            AutoStepQueue.EnQueue(Robot_GPTArray.ToArray<RobotStep>());
        }

        private void SetHomeStep()
        {
            if (NowMainJab == SocketCommand.InitialHome)
            {
                Robot_GPTArray.Add(RobotStep.Set_RsetError);
            }
            Robot_GPTArray.Add(RobotStep.Set_Speed);
            Robot_GPTArray.Add(RobotStep.Update_TurnStatus);
            Robot_GPTArray.Add(RobotStep.Check_HomeR_NormalOrTurn_Jump1);

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            Robot_GPTArray.Add(RobotStep.Jump_NornalHomeStart);
            Robot_GPTArray.Add(RobotStep.Get_DI_0);
            Robot_GPTArray.Add(RobotStep.Get_DI_1);
            Robot_GPTArray.Add(RobotStep.Update_ArmPresence);
            Robot_GPTArray.Add(RobotStep.Check_Home_GPNeedJumpEnd);             //Address = 0 , arm extend
            Robot_GPTArray.Add(RobotStep.Set_BackSpeed);                        //5-5
            Robot_GPTArray.Add(RobotStep.Update_RobotPosition);                 //Node
            Robot_GPTArray.Add(RobotStep.Update_HomeBackGPPosition);            //GP Position
            Robot_GPTArray.Add(RobotStep.Check_Home_PosRuleNeedJumpEnd);        //Node and GP position check
            Robot_GPTArray.Add(RobotStep.GP_NormalHomeZ_Back);                  //Get -1 address . put + 1 address
            Robot_GPTArray.Add(RobotStep.Update_RobotPosition);
            Robot_GPTArray.Add(RobotStep.Check_Home_GPJumpStart);
            Robot_GPTArray.Add(RobotStep.Set_Speed);
            Robot_GPTArray.Add(RobotStep.Jump_NornalHomeEnd);
            //Normal_R0_Home
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            Robot_GPTArray.Add(RobotStep.Get_LBD);
            Robot_GPTArray.Add(RobotStep.GP_Home_SnakeMotion);
            Robot_GPTArray.Add(RobotStep.GP_Home_1);
            Robot_GPTArray.Add(RobotStep.Update_RobotPosition);
            Robot_GPTArray.Add(RobotStep.GP_Home_0);
            Robot_GPTArray.Add(RobotStep.Update_RobotPosition);
            Robot_GPTArray.Add(RobotStep.MaxCnt);
            //Turn_R180_Home
            Robot_GPTArray.Add(RobotStep.Jump_Check1);
            Robot_GPTArray.Add(RobotStep.Check_HoemR_FinalOrUndone_Jump2);
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            Robot_GPTArray.Add(RobotStep.Jump_TopHomeStart);
            Robot_GPTArray.Add(RobotStep.Get_DI_0);
            Robot_GPTArray.Add(RobotStep.Get_DI_1);
            Robot_GPTArray.Add(RobotStep.Update_ArmPresence);
            Robot_GPTArray.Add(RobotStep.Check_Home_GPNeedJumpEnd);       //Address = 0 , arm extend
            Robot_GPTArray.Add(RobotStep.Set_BackSpeed);                  //5-5
            Robot_GPTArray.Add(RobotStep.Update_RobotPosition);           //Node
            Robot_GPTArray.Add(RobotStep.Update_HomeBackGPPosition);           //GP Position
            Robot_GPTArray.Add(RobotStep.Check_Home_PosRuleNeedJumpEnd);  //Node and GP position check
            Robot_GPTArray.Add(RobotStep.GP_TopHomeZ_Back);
            Robot_GPTArray.Add(RobotStep.Update_RobotPosition);
            Robot_GPTArray.Add(RobotStep.Set_Speed);
            Robot_GPTArray.Add(RobotStep.Jump_TopHomeEnd);
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            Robot_GPTArray.Add(RobotStep.GP_Home_1);
            Robot_GPTArray.Add(RobotStep.Update_RobotPosition);
            Robot_GPTArray.Add(RobotStep.GP_TurnBack_0);
            Robot_GPTArray.Add(RobotStep.Update_RobotPosition);
            Robot_GPTArray.Add(RobotStep.GP_TurnBack_1);
            Robot_GPTArray.Add(RobotStep.Update_RobotPosition);
            Robot_GPTArray.Add(RobotStep.GP_TurnBack_2);
            Robot_GPTArray.Add(RobotStep.Update_RobotPosition);
            Robot_GPTArray.Add(RobotStep.GP_TurnBack_3);
            Robot_GPTArray.Add(RobotStep.Update_RobotPosition);
            Robot_GPTArray.Add(RobotStep.GP_Home_1);
            Robot_GPTArray.Add(RobotStep.Update_RobotPosition);
            Robot_GPTArray.Add(RobotStep.GP_Home_0);
            Robot_GPTArray.Add(RobotStep.Update_RobotPosition);
            Robot_GPTArray.Add(RobotStep.MaxCnt);
            //Turn_7~174_Home
            Robot_GPTArray.Add(RobotStep.Jump_Check2);
            Robot_GPTArray.Add(RobotStep.GP_TurnBack_2);
            Robot_GPTArray.Add(RobotStep.Update_RobotPosition);
            Robot_GPTArray.Add(RobotStep.GP_TurnBack_3);
            Robot_GPTArray.Add(RobotStep.Update_RobotPosition);
            Robot_GPTArray.Add(RobotStep.GP_Home_1);
            Robot_GPTArray.Add(RobotStep.Update_RobotPosition);
            Robot_GPTArray.Add(RobotStep.GP_Home_0);
            Robot_GPTArray.Add(RobotStep.Update_RobotPosition);
            Robot_GPTArray.Add(RobotStep.MaxCnt);
            AutoStepQueue.EnQueue(Robot_GPTArray.ToArray<RobotStep>());
        }

        private void SetArmSafetyStep()
        {

            Robot_GPTArray.Add(RobotStep.Set_Speed);
            Robot_GPTArray.Add(RobotStep.Update_TurnStatus);
            Robot_GPTArray.Add(RobotStep.Check_HomeR_NormalOrTurn_Jump1);

            //Normal_R0_Home
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            Robot_GPTArray.Add(RobotStep.Get_LBD);
            Robot_GPTArray.Add(RobotStep.GP_Home_SnakeMotion);
            Robot_GPTArray.Add(RobotStep.GP_Home_1);
            Robot_GPTArray.Add(RobotStep.Update_RobotPosition);
            Robot_GPTArray.Add(RobotStep.MaxCnt);
            //Turn_R180_Home
            Robot_GPTArray.Add(RobotStep.Jump_Check1);
            Robot_GPTArray.Add(RobotStep.Check_HoemR_FinalOrUndone_Jump2);
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            Robot_GPTArray.Add(RobotStep.GP_Home_1);
            Robot_GPTArray.Add(RobotStep.Update_RobotPosition);
            Robot_GPTArray.Add(RobotStep.GP_TurnBack_0);
            Robot_GPTArray.Add(RobotStep.Update_RobotPosition);
            Robot_GPTArray.Add(RobotStep.GP_TurnBack_1);
            Robot_GPTArray.Add(RobotStep.Update_RobotPosition);
            Robot_GPTArray.Add(RobotStep.GP_TurnBack_2);
            Robot_GPTArray.Add(RobotStep.Update_RobotPosition);
            Robot_GPTArray.Add(RobotStep.GP_TurnBack_3);
            Robot_GPTArray.Add(RobotStep.Update_RobotPosition);
            Robot_GPTArray.Add(RobotStep.GP_Home_1);
            Robot_GPTArray.Add(RobotStep.Update_RobotPosition);
            Robot_GPTArray.Add(RobotStep.MaxCnt);
            //Turn_7~174_Home
            Robot_GPTArray.Add(RobotStep.Jump_Check2);
            Robot_GPTArray.Add(RobotStep.GP_TurnBack_2);
            Robot_GPTArray.Add(RobotStep.Update_RobotPosition);
            Robot_GPTArray.Add(RobotStep.GP_TurnBack_3);
            Robot_GPTArray.Add(RobotStep.Update_RobotPosition);
            Robot_GPTArray.Add(RobotStep.GP_Home_1);
            Robot_GPTArray.Add(RobotStep.Update_RobotPosition);
            Robot_GPTArray.Add(RobotStep.MaxCnt);
            AutoStepQueue.EnQueue(Robot_GPTArray.ToArray<RobotStep>());
        }

        private void SetInitialStep()
        {
            Robot_GPTArray.Add(RobotStep.Set_RsetError);
            Robot_GPTArray.Add(RobotStep.Get_Status);
            Robot_GPTArray.Add(RobotStep.Set_Speed);
            for (int i = 0; i < PosAddressMap.Length; i++)
            {
                Robot_GPTArray.Add(RobotStep.Update_TeachingPosition);
            }

            NeedSetIO_CheckPresenceStep();

            Robot_GPTArray.Add(RobotStep.Get_DI_0);
            Robot_GPTArray.Add(RobotStep.Get_DI_1);
            Robot_GPTArray.Add(RobotStep.Update_ArmPresence);
            Robot_GPTArray.Add(RobotStep.Update_RobotPosition);
            Robot_GPTArray.Add(RobotStep.MaxCnt);
            AutoStepQueue.EnQueue(Robot_GPTArray.ToArray<RobotStep>());
        }

        private void SetResetErrorStep()
        {
            Robot_GPTArray.Add(RobotStep.Set_RsetError);
            Robot_GPTArray.Add(RobotStep.Get_Status);
            Robot_GPTArray.Add(RobotStep.Update_RobotPosition);
            Robot_GPTArray.Add(RobotStep.MaxCnt);
            AutoStepQueue.EnQueue(Robot_GPTArray.ToArray<RobotStep>());
        }

        private void SetStopStep()
        {
            Robot_GPTArray.Add(RobotStep.Set_Stop);
            Robot_GPTArray.Add(RobotStep.Get_Status);
            Robot_GPTArray.Add(RobotStep.Update_RobotPosition);
            Robot_GPTArray.Add(RobotStep.MaxCnt);
            AutoStepQueue.EnQueue(Robot_GPTArray.ToArray<RobotStep>());
        }

        private void SetRestartStep()
        {
            Robot_GPTArray.Add(RobotStep.Set_Restart);
            Robot_GPTArray.Add(RobotStep.Get_Status);
            Robot_GPTArray.Add(RobotStep.Update_RobotPosition);
            Robot_GPTArray.Add(RobotStep.MaxCnt);
            AutoStepQueue.EnQueue(Robot_GPTArray.ToArray<RobotStep>());
        }

        private void SetSpeed()
        {
            Robot_GPTArray.Add(RobotStep.Set_Speed);
            Robot_GPTArray.Add(RobotStep.Get_Status);
            Robot_GPTArray.Add(RobotStep.MaxCnt);
            AutoStepQueue.EnQueue(Robot_GPTArray.ToArray<RobotStep>());
        }

        private void SetArmStatusStep()
        {
            switch (NowMainJab)
            {
                case SocketCommand.BernoulliOn:
                    {
                        Robot_GPTArray.AddRange(Set_BernoulliOn);
                        Robot_GPTArray.Add(RobotStep.Wait_VacOn);
                    }
                    break;

                case SocketCommand.BernoulliOff:
                    {
                        Robot_GPTArray.AddRange(Set_BernoulliOff);
                        Robot_GPTArray.Add(RobotStep.Wait_VacOff);
                    }
                    break;

                //case SocketCommand.EdgeGripOn:
                //    {
                //        Robot_GPTArray.AddRange(Set_EdgeOn);
                //        Robot_GPTArray.Add(RobotStep.Wait_VacOn);
                //    }
                //    break;

                //case SocketCommand.EdgeGripOff:
                //    {
                //        Robot_GPTArray.AddRange(Set_EdgeOff);
                //        Robot_GPTArray.Add(RobotStep.Wait_VacOff);
                //    }
                //    break;

                case SocketCommand.VacuumOn:
                    {
                        Robot_GPTArray.AddRange(Set_VacOn);
                        Robot_GPTArray.Add(RobotStep.Wait_VacOn);
                    }
                    break;

                case SocketCommand.VacuumOff:
                    {
                        Robot_GPTArray.AddRange(Set_VacOff);
                        Robot_GPTArray.Add(RobotStep.Wait_VacOff);
                    }
                    break;
            }

            Robot_GPTArray.AddRange(Get_DI);
            Robot_GPTArray.Add(RobotStep.Update_ArmPresence);
            Robot_GPTArray.Add(RobotStep.MaxCnt);
            AutoStepQueue.EnQueue(Robot_GPTArray.ToArray<RobotStep>());
        }

        private void SetCheckPresenceStep()
        {
            Robot_GPTArray.Add(RobotStep.Get_Status);
            Robot_GPTArray.Add(RobotStep.Update_RobotPosition);

            NeedSetIO_CheckPresenceStep();

            Robot_GPTArray.Add(RobotStep.Get_DI_0);
            Robot_GPTArray.Add(RobotStep.Get_DI_1);
            Robot_GPTArray.Add(RobotStep.Update_ArmPresence);
            Robot_GPTArray.Add(RobotStep.MaxCnt);
            AutoStepQueue.EnQueue(Robot_GPTArray.ToArray<RobotStep>());
        }

        private void GetMappingData()
        {
            Robot_GPTArray.Add(RobotStep.Get_MappingData);
            Robot_GPTArray.Add(RobotStep.MaxCnt);
            AutoStepQueue.EnQueue(Robot_GPTArray.ToArray<RobotStep>());
        }

        private void GetMappingErrorData()
        {
            Robot_GPTArray.Add(RobotStep.Get_MappingErrorData);
            Robot_GPTArray.Add(RobotStep.MaxCnt);
            AutoStepQueue.EnQueue(Robot_GPTArray.ToArray<RobotStep>());
        }

        private void GetRobotPosition()
        {
            Robot_GPTArray.Add(RobotStep.Update_RobotPosition);
            Robot_GPTArray.Add(RobotStep.MaxCnt);
            AutoStepQueue.EnQueue(Robot_GPTArray.ToArray<RobotStep>());
        }

        private void NeedSetIO_CheckPresenceStep()
        {
            switch (IniUseArmType[0])
            {
                case ArmType.EdgeGrip:
                    {
                        if (NowUseArm == 0)
                        {
                            if (IniArmCnt > 1)
                            {
                                Robot_GPTArray.AddRange(Check_UpperEdgeGrip);
                            }
                            Robot_GPTArray.AddRange(Check_LowerEdgeGrip);
                        }
                        else
                        {
                            Robot_GPTArray.AddRange(Check_LowerEdgeGrip);
                            Robot_GPTArray.AddRange(Check_UpperEdgeGrip);
                        }
                    }
                    break;

                case ArmType.Bernoulli:
                    {
                        if (NowUseArm == 0)
                        {
                            if (IniArmCnt > 1)
                            {
                                Robot_GPTArray.AddRange(Check_UpperBernoulli);
                            }
                            Robot_GPTArray.AddRange(Check_LowerBernoulli);
                        }
                        else
                        {
                            Robot_GPTArray.AddRange(Check_LowerBernoulli);
                            Robot_GPTArray.AddRange(Check_UpperBernoulli);
                        }
                    }
                    break;
            }

        }

        private void SetAlignerOCRReadStep()
        {
            Robot_GPTArray.AddRange(Set_Speed);
            Robot_GPTArray.AddRange(Check_AllInterLock);
            Robot_GPTArray.AddRange(GP_0);

            if (NowUseArm == 0)
                Robot_GPTArray.Add(RobotStep.GP_AlignerOCR_Down);
            else
                Robot_GPTArray.Add(RobotStep.GP_AlignerOCR_Up);

            Robot_GPTArray.Add(RobotStep.Update_RobotPosition);

            Robot_GPTArray.Add(RobotStep.MaxCnt);
            AutoStepQueue.EnQueue(Robot_GPTArray.ToArray<RobotStep>());
        }

        #endregion

        #region IO

        public void SetNowSpeed(int speed)
        {
            Speed = speed;
            AppSetting.SaveSetting(string.Format("{0}{1}", RobotName, Robot_Static.Speed), Speed.ToString());
        }

        public void SetNowUseArm(int _ArmID)
        {
            NowUseArm = _ArmID;
        }

        public void SetNowUseObj(string _Obj)
        {
            NowUseObj = _Obj;
        }

        private int[] ConverIntToBitArray(string _intData)
        {
            int[] bits = new int[_intData.Length];

            for (int idx = 0; idx < _intData.Length; idx++)
            {
                bits[idx] = Convert.ToInt16(_intData.Substring(_intData.Length - 1 - idx, 1));
            }
            return bits;
        }

        private int GetIntFromBitArray(int[] _Array)
        {
            int value = 0;

            for (int i = 0; i < 8; i++)
            {
                if (_Array[i] == 1)
                    value += Convert.ToInt16(Math.Pow(2, i));
            }
            return value;
        }

        private int SetDoArray(int _on, int _Off, bool _Ctrl, string _orgStatus)
        {
            int[] _array = ConverIntToBitArray(Convert.ToString(Convert.ToInt16(_orgStatus), 2).PadLeft(8, '0'));

            if (_Ctrl)
            {
                _array[_on] = 1;
                _array[_Off] = 0;
            }
            else
            {
                _array[_on] = 0;
                _array[_Off] = 1;
            }
            int _nn = GetIntFromBitArray(_array);
            return _nn;
        }

        #region Do

        private void GetDoProcess(ArmType _ArmType)
        {
            if (IniArmOutType[NowUseArm, 0] == _ArmType)
                CommandQueue.EnQueue(string.Format("{0}{1}{2}", "LOD", NormalStatic.Space, (IniArmOutOn[NowUseArm, 0] / Robot_Static.BYTESIZE).ToString()));
            else
                CommandQueue.EnQueue(string.Format("{0}{1}{2}", "LOD", NormalStatic.Space, (IniArmOutOn[NowUseArm, 1] / Robot_Static.BYTESIZE).ToString()));
        }
        //20220811 Arnold
        private void Set_DoBernoulliOn()
        {
            CommandQueue.EnQueue("SOD 0 4");
        }
        private void Set_DoBernoulliOff()
        {
            CommandQueue.EnQueue("SOD 0 8");
        }
        private void SetDoProcess(string _replyMsg, ArmType SetArmType, bool _IOControl)
        {
            int _mm;
            int _On;
            int _Off;

            if (IniArmOutType[NowUseArm, 0] == SetArmType)
            {
                _mm = IniArmOutOn[NowUseArm, 0] / Robot_Static.BYTESIZE;
                _On = IniArmOutOn[NowUseArm, 0] % Robot_Static.BYTESIZE;
                _Off = IniArmOutOff[NowUseArm, 0] % Robot_Static.BYTESIZE;
            }
            else
            {
                _mm = IniArmOutOn[NowUseArm, 1] / Robot_Static.BYTESIZE;
                _On = IniArmOutOn[NowUseArm, 1] % Robot_Static.BYTESIZE;
                _Off = IniArmOutOff[NowUseArm, 1] % Robot_Static.BYTESIZE;
            }
            int _nn = SetDoArray(_On, _Off, _IOControl, _replyMsg);
            CommandQueue.EnQueue(string.Format("{0}{1}{2}{3}{4}", "SOD", NormalStatic.Space, _mm, NormalStatic.Space, _nn));
        }
        private bool Check_DoBernoulliOn(string _replyMsg)
        {
            int[] _array = ConverIntToBitArray((Convert.ToString(Convert.ToInt16(_replyMsg), 2).PadLeft(8, '0')));
            if (_array[0] == 0 && _array[1] == 0 && _array[2] == 1 && _array[3] == 0)
            {
                return true;
            }
            return false;
        }
        private bool Check_DoBernoulliOff(string _replyMsg)
        {
            int[] _array = ConverIntToBitArray((Convert.ToString(Convert.ToInt16(_replyMsg), 2).PadLeft(8, '0')));
            if (_array[0] == 0 && _array[1] == 0 && _array[2] == 0 && _array[3] == 1)
            {
                return true;
            }
            return false;
        }
        private bool CheckDoProcess(string _replyMsg, ArmType CheckArmType, bool _IOControl)
        {
            int[] _array = ConverIntToBitArray((Convert.ToString(Convert.ToInt16(_replyMsg), 2).PadLeft(8, '0')));
            bool Result;
            int NowArmCnt = 0;

            if (IniArmOutType[NowUseArm, 0] == CheckArmType)
                NowArmCnt = 0;
            else
                NowArmCnt = 1;

            switch (_IOControl)
            {
                case true:
                    {
                        if (_array[IniArmOutOn[NowUseArm, NowArmCnt] % Robot_Static.BYTESIZE] == 1 && _array[IniArmOutOff[NowUseArm, NowArmCnt] % Robot_Static.BYTESIZE] == 0)
                            Result = true;
                        else
                            Result = false;
                    }
                    break;

                case false:
                    {
                        if (_array[IniArmOutOn[NowUseArm, NowArmCnt] % Robot_Static.BYTESIZE] == 0 && _array[IniArmOutOff[NowUseArm, NowArmCnt] % Robot_Static.BYTESIZE] == 1)
                            Result = true;
                        else
                            Result = false;
                    }
                    break;
                default:
                    Result = false;
                    break;
            }
            return Result;
        }

        #endregion

        #region DI

        private void GetDiBytePdu(int _mm)
        {
            CommandQueue.EnQueue(string.Format("{0}{1}", "LID", _mm));
        }

        private WaferStatus Check_Presence(int _arm, string _relyMsg)
        {

            int[] _ba = ConverIntToBitArray(_relyMsg);

            int tmp_cnt = 0;

            bool _result;

            for (int InCnt = 0; InCnt < IniArmInCnt[_arm]; InCnt++)
            {
                _result = (_ba[IniArmInCntBit[_arm, InCnt]] == IniArmInFlag[_arm, InCnt]) ? true : false;

                if (_result == true)
                    tmp_cnt++;

                // Walson 20201124
                #region ErrorMsg 顯示哪個sensor有問題

                switch (MarcoCommand[NowStepCnt])
                {
                    case RobotStep.Check_With:
                    case RobotStep.Check_WithLoop1s:
                    case RobotStep.Check_With_ABN:
                        {
                            if (_result == false)
                                NowErrorMsg = string.Format(" presence error:{0}", IniArmInCntBit[_arm, InCnt]);
                        }
                        break;
                    case RobotStep.Check_Without:
                    case RobotStep.Check_WithoutLoop1s:
                    case RobotStep.Check_Without_ABN:
                        {
                            if (_result == true)
                                NowErrorMsg = string.Format(" presence error:{0}", IniArmInCntBit[_arm, InCnt]);
                        }
                        break;
                }

                #endregion
            }

            if (tmp_cnt == 0)
                return WaferStatus.WithOut;
            else if (tmp_cnt == IniArmInCnt[_arm])
                return WaferStatus.With;
            else
                return WaferStatus.WithOut;

        }


        #region Check_SingleArmPresence

        private WaferStatus CheckSingleArmPresence(string _replyMsg)
        {

            WaferStatus _rtn = WaferStatus.Unknown;

            _rtn = Check_Presence(NowUseArm, _replyMsg);

            UI.Log(NormalStatic.Robot, DeviceName, SystemList.DiReceive, string.Format("{0}-> {1}", NowUseArm, _rtn));

            if (NowUseArm == 1)
                Ui_UpperWaferPresent = _rtn;
            else
                Ui_LowerWaferPresent = _rtn;

            return _rtn;
        }

        #endregion

        #region Check_All_ArmPresence
        private WaferStatus CheckAllArmPresence(string _replyMsg)
        {
            WaferStatus _rtn = WaferStatus.Unknown;
            for (int i = 0; i < IniArmCnt; i++)
            {
                _rtn = Check_Presence(i, _replyMsg);
                UI.Log(NormalStatic.Robot, DeviceName, SystemList.DiReceive, string.Format("{0}-> {1}", i, _rtn));

                if (i == 1)
                    Ui_UpperWaferPresent = _rtn;
                else
                    Ui_LowerWaferPresent = _rtn;
            }
            return _rtn;
        }

        #endregion


        #endregion

        #endregion

        #region Turn

        private void Check_RAxisTurnStatus(double _rDegree)
        {
            if (Math.Abs(_rDegree) <= 4)
            {
                Ui_ArmStatusR = ArmStatus.Arm_Home;
            }
            else if (Math.Abs(_rDegree) > 4 && Math.Abs(_rDegree) <= 176)
            {
                Ui_ArmStatusR = ArmStatus.Arm_Now_Turn;
            }
            else
            {
                Ui_ArmStatusR = ArmStatus.Arm_Turn;
            }
        }

        #endregion

        #region GPT

        #region GPT_Start

        public void GPT_Start(int PLCStart, string Obj, int SlotID, double Offset, int StartGPAddress)
        {

            NowPLCInvasionStart = PLCStart;
            NowUseObj = Obj;
            NowUseSlotGap = Offset;
            NowUseSlotID = SlotID;
            NowGPBaseAddress = StartGPAddress;

            UI.Log(NormalStatic.Robot, DeviceName, SystemList.CommandParameter, string.Format("{0}:({1})({2})", NowUseObj, NowUseSlotGap, NowGPBaseAddress));

        }

        #endregion

        #region GPT_StepPdu

        private void GPT_HomeStepPdu(RobotStep GTP_Step)
        {
            NowGPRobotAddress = GPT_StepCnt(GTP_Step);

            CommandQueue.EnQueue(string.Format("{0}{1}{2}", "GP", NormalStatic.Space, NowGPRobotAddress));
        }

        private void GPT_TurnStepPdu(RobotStep GTP_Step)
        {
            NowGPRobotAddress = GPT_StepCnt(GTP_Step);

            switch (NowUseObj)
            {
                case "Aligner1":
                    break;

                case "Stage1":
                    {
                        NowGPRobotAddress += 20;
                    }
                    break;

                case "Stage2":
                    {
                        NowGPRobotAddress += 40;
                    }
                    break;
            }

            CommandQueue.EnQueue(string.Format("{0}{1}{2}", "GP", NormalStatic.Space, NowGPRobotAddress));
        }

        private void GPT_StepPdu(RobotStep GTP_Step)
        {
            double _zPos;

            NowGPRobotAddress = GPT_StepCnt(GTP_Step);

            _zPos = Math.Round((Convert.ToDouble(NowUseSlotID) - 1) * NowUseSlotGap, 3, MidpointRounding.AwayFromZero);

            CommandQueue.EnQueue(string.Format("{0}{1}{2}{3}{4}{5}{6}", "GP", NormalStatic.Space, NowGPRobotAddress, NormalStatic.Space, "( 0 0 ", _zPos, " 0)"));
        }

        private void GPT_MappingStepPdu(RobotStep GTP_Step)
        {

            NowGPRobotAddress = GPT_StepCnt(GTP_Step);

            CommandQueue.EnQueue(string.Format("{0}{1}{2}{3}", "GP", NormalStatic.Space, NowGPRobotAddress, NormalStatic.Space, "( 0 0 0 0)"));

        }

        #endregion

        #region GPT_StepCnt

        private int GPT_StepCnt(RobotStep GTP_Step)
        {
            int _rtnCnt = 10;
            switch (GTP_Step)
            {
                #region Home

                case RobotStep.GP_Home_0:
                    {
                        _rtnCnt = 0;
                    }
                    break;

                case RobotStep.GP_Home_1:
                    {
                        _rtnCnt = 1;
                    }
                    break;

                case RobotStep.GP_AlignerOCR_Down:
                    {
                        _rtnCnt = 2;
                    }
                    break;

                case RobotStep.GP_AlignerOCR_Up:
                    {
                        _rtnCnt = 3;
                    }
                    break;

                case RobotStep.GP_Home_SnakeMotion_LP1:
                case RobotStep.GP_Home_SnakeMotion_LP2:
                case RobotStep.GP_Home_SnakeMotion_EQ1:
                case RobotStep.GP_Home_SnakeMotion_EQ2:
                    {
                        _rtnCnt = 10;
                    }
                    break;

                case RobotStep.GP_NormalHomeZ_Back:
                case RobotStep.GP_TopHomeZ_Back:
                    {
                        _rtnCnt = NowGPRobotAddress;
                    }
                    break;

                #endregion

                #region Turn

                case RobotStep.GP_Turn_0:
                    {
                        _rtnCnt = Other_StartAddressDictionay[Robot_Static.TurnPos];
                    }
                    break;

                case RobotStep.GP_Turn_1:
                    {
                        _rtnCnt = Other_StartAddressDictionay[Robot_Static.TurnPos] + 1;
                        if (NowUseArm == 1)
                            Ui_ArmStatusY = ArmStatus.Arm_Extend;
                        else
                            Ui_ArmStatusX = ArmStatus.Arm_Extend;
                    }
                    break;

                case RobotStep.GP_Turn_2:
                    {
                        _rtnCnt = Other_StartAddressDictionay[Robot_Static.TurnPos] + 2;
                        Ui_ArmStatusR = ArmStatus.Arm_Now_Turn;
                    }
                    break;

                case RobotStep.GP_Turn_3:
                    {
                        _rtnCnt = Other_StartAddressDictionay[Robot_Static.TurnPos] + 3;
                        Ui_ArmStatusR = ArmStatus.Arm_Turn;
                    }
                    break;

                case RobotStep.GP_TurnBack_0:
                    {
                        _rtnCnt = Other_StartAddressDictionay[Robot_Static.TurnBackPos];
                    }
                    break;

                case RobotStep.GP_TurnBack_1:
                    {
                        _rtnCnt = Other_StartAddressDictionay[Robot_Static.TurnBackPos] + 1;
                    }
                    break;

                case RobotStep.GP_TurnBack_2:
                    {
                        _rtnCnt = Other_StartAddressDictionay[Robot_Static.TurnBackPos] + 2;
                        Ui_ArmStatusR = ArmStatus.Arm_Now_Turn;
                    }
                    break;

                case RobotStep.GP_TurnBack_3:
                    {
                        _rtnCnt = Other_StartAddressDictionay[Robot_Static.TurnBackPos] + 3;
                    }
                    break;

                #endregion

                #region Normal

                case RobotStep.GP_Normal_0:
                case RobotStep.GP_Normal_1:
                case RobotStep.GP_Normal_2:
                case RobotStep.GP_Normal_3:
                case RobotStep.GP_Normal_4:
                case RobotStep.GP_Normal_5:
                case RobotStep.GP_Normal_6:
                case RobotStep.GP_Normal_7:
                case RobotStep.GP_Normal_8:
                case RobotStep.GP_Normal_9:
                    {
                        if (GTP_Step == RobotStep.GP_Normal_1)
                        {
                            if (NowUseArm == 1)
                                Ui_ArmStatusY = ArmStatus.Arm_Extend;
                            else
                                Ui_ArmStatusX = ArmStatus.Arm_Extend;
                        }

                        _rtnCnt = (NowGPBaseAddress + ((int)GTP_Step - (int)RobotStep.GP_Normal_0));
                    }
                    break;

                #endregion

                #region Top

                case RobotStep.GP_Top_0:
                case RobotStep.GP_Top_1:
                case RobotStep.GP_Top_2:
                case RobotStep.GP_Top_3:
                case RobotStep.GP_Top_4:
                case RobotStep.GP_Top_5:
                case RobotStep.GP_Top_6:
                case RobotStep.GP_Top_7:
                case RobotStep.GP_Top_8:
                case RobotStep.GP_Top_9:
                    {
                        if (GTP_Step == RobotStep.GP_Top_1)
                        {
                            if (NowUseArm == 1)
                                Ui_ArmStatusY = ArmStatus.Arm_Extend;
                            else
                                Ui_ArmStatusX = ArmStatus.Arm_Extend;
                        }

                        _rtnCnt = (NowGPBaseAddress + ((int)GTP_Step - (int)RobotStep.GP_Top_0));
                    }
                    break;

                #endregion

                #region Mapping

                case RobotStep.GP_Mapping_0:
                case RobotStep.GP_Mapping_1:
                case RobotStep.GP_Mapping_2:
                case RobotStep.GP_Mapping_3:
                    {
                        if (GTP_Step == RobotStep.GP_Mapping_1)
                        {
                            if (NowUseArm == 1)
                                Ui_ArmStatusY = ArmStatus.Arm_Extend;
                            else
                                Ui_ArmStatusX = ArmStatus.Arm_Extend;
                        }

                        _rtnCnt = (NowGPBaseAddress + ((int)GTP_Step - (int)RobotStep.GP_Mapping_0));

                    }
                    break;

                    #endregion

            }

            return _rtnCnt;
        }

        #endregion

        #region GPT_StepRule

        private void GPT_StepRule()
        {

            if (NowMainJab == SocketCommand.GetStandby || NowMainJab == SocketCommand.WaferGet || NowMainJab == SocketCommand.TopGetStandby || NowMainJab == SocketCommand.TopWaferGet)
            {
                NowGPBaseAddress += Robot_Static.GET200;
            }

            if (NowUseArm == 1)
            {
                NowGPBaseAddress += Robot_Static.ARM500;
            }

            if (NowMainJab == SocketCommand.TopPutStandby || NowMainJab == SocketCommand.TopWaferPut || NowMainJab == SocketCommand.TopGetStandby || NowMainJab == SocketCommand.TopWaferGet)
            {
                NowGPBaseAddress += Robot_Static.TOP2000;
            }
        }

        #endregion

        #endregion

        //20210607 Wayne Check Destination
        //private bool CheckDestinationPresent(string ref_Obj)
        //{
        //    bool Check = false;
        //    string Status = "";

        //    switch (ref_Obj)
        //    {
        //        case "LP1":
        //            // Status = Convert.ToString(PLC.W[(int)PLC_W.CstStatusStart], 2).PadLeft(16, '0');
        //            if (Status[(int)PortDi.Presence] != '1')
        //            {
        //                UI.Log(NormalStatic.Robot, LogDir.CP.ToString(), SystemList.CommandParameter,
        //                           string.Format("CP1 present is without,Trigger Robot Stop"));
        //                Check = true;
        //            }
        //            break;

        //    }

        //    return Check;
        //}
        public void ClearMainJob()
        {
            NowMainJab = SocketCommand.MaxCnt;
        }
    }
}
