using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows.Forms;

namespace HirataMainControl
{
    #region EFEM

    #region Status

    public static class NormalStatic
    {
        /// <summary>
        /// 版本名稱: 專案代號(三碼)-西元年(二碼)-月份-日期
        /// </summary>
        public const string Version = "430-21-1101-01";
        public const string DeviceType = "Carrier Sorter";

        //Device
        public const string Robot = "Robot";
        public const string LP = "LP";
        public const string MagazinePort = "MP";
        public const string CstPort = "CP";
        public const string PLC = "PLC";

        public const string Adam = "Adam";
        public const string Cognex = "Cognex";
        public const string Aligner = "Aligner";
        public const string IO = "IO";
        public const string Buffer = "Buffer";
        public const string OCRReader = "OCRReader";
        public const string RFID = "RFID";
        public const string E84 = "E84";
        public const string Stage = "Stage";
        public const string D900 = "D900";
        public const string EQ = "EQ";

        public const string Load = "Load";
        public const string UnLoad = "UnLoad";
        public const string Load_UnLoad = "Load_UnLoad";
        public const string SECS = "SECS";
        public const string API = "API";

        public const string Wafer = "Wafer";
        public const string Carrier = "Carrier";
        public const string CarrierWafer = "CarrierWafer";

        //EFEM
        public const string EFEM = "EFEM";
        public const string InitialSystem = "InitialSystem";
        public const string InitialDevice = "InitialDevice";
        public const string Config = "Config";
        public const string Log = "Log";
        public const string System = "System";
        public const string Alarm = "Alarm";
        public const string Error = "Error";
        public const string Warning = "Warning";
        public const string Device = "Device";
        public const string Mode = "Mode";
        public const string Core = "Core";
        public const string SQL = "SQL";
        public const string Operation = "Operation";
        public const string AutoButton = "AutoButton";
        public const string Ready = "Ready";
        public const string Auto = "Auto";
        public const string Cycle = "Cycle";
        public const string Socket = "Socket";
        public const string Recipe = "Recipe";
        public const string Stop = "STOP";

        //Robot
        public const string POS = "_POS";
        public const string MAP = "_MAP";

        //Common
        public const string Comma = ",";
        public const string Space = " ";
        public const string UnderLine = "_";
        public const string LeftSquare = "[";
        public const string RightSquare = "]";
        public const string End = "End";
        public const string Close = "Close";
        public const string False = "False";
        public const string True = "True";
        public const string On = "On";
        public const string Off = "Off";
        public const string DO = "DO";
        public const string DI = "DI";
        public const string Flash = "Flash";
        public const string Timeout = "Timeout";
        public const string LogSplot = "@";
        public const string TimeFormat = "yyyy-MM-dd HH:mm:ss:ff";
        public const string FileFormat = "yyyy_MM_dd";
        //RS232
        public const byte StartByte_STX = 0x02;
        public const byte StartByte_SOH = 0x01;
        public const byte StartByte_E84 = 0xaa;
        public const byte StartByte_SC = 0xe0;
        public const byte StartByte_EOC = 0xea;
        public const byte EndByte_CR = 0x0D;
        public const byte EndByte_E84 = 0xbb;
        public const byte EndByte_ETX = 0x03;
        public const byte EndByte_LF = 0x0A;
        public const int Idle = 0;
        public const int WaitReply = 1;
        public const int Receiving = 2;
        public const int WaitCheckSum = 3;

        //Path
        public static string ExcelPath = Application.StartupPath + @"\ListItem\";
        public static string ManualPath = Application.StartupPath + @"\Manual\";
        public static string ConfigPath = Application.StartupPath + @"\HirataMainControl.EXE.config";
        public static string IniPath = Application.StartupPath + @"\ListItem\WaferInfo.ini";
        public static string DataPath = Application.StartupPath + @"\ListItem\DataInfo.ini";
        public static string DbPath = Application.StartupPath + @"\Hirata.db";
        public const string SECSLogPath = @"D:\HirataMain_Log\SECS_Log\";
        public const string OCRFailPath = @"D:\API_WID120_ReadFail\";
        public const string GEMVariablePath = "D:\\Hirata 程式\\HCT\\SECS\\EQ Simulator\\EQ Simulator\\bin\\Debug\\GEMVariable.ini";
        public const string VIDVariablePath = "D:\\Hirata 程式\\HCT\\SECS\\EQ Simulator\\EQ Simulator\\bin\\Debug\\VIDVariable.ini";
        public const string Presence = "Presence";
        public const string ID = "ID";
        // SECS
        public const string CEID = "CEID";
        public const string RPTID = "RPTID";
        public const string VID = "VID";
        public const string ReadyToLoad = "2";
        public const string ReadyToUnload = "3";
        public const string TransferBlocked = "1";
        public const string TransferResultOK = "P";
        public const string TransferResultFail = "F";

        #region Message
        public const string S1F1_Messgae = "S1F1_Are You There Request";
        public const string S1F2_Messgae = "S1F2_Online Data";
        public const string S1F3_Messgae = "S1F3_Selected Equipment Status Request";
        public const string S1F4_Messgae = "S1F4_Selected Equipment Status Data";
        public const string S1F12_Messgae = "S1F12_Status Variable Namelist Reply";
        public const string S1F13_Messgae = "S1F13_Establish Communication Request";
        public const string S1F14_Messgae = "S1F14_Establish Communication Request Ack";
        public const string S1F17_Messgae = "S1F17_Request ON-LINE";
        public const string S1F18_Messgae = "S1F18_ON-LINE Ack";
        public const string S2F31_Messgae = "S2F31_Date and Time Set Request";
        public const string S2F32_Messgae = "S2F32_Date and Time Set Ack";
        public const string S2F33_Messgae = "S2F33_Define Report";
        public const string S2F34_Messgae = "S2F34_Define Report Ack";
        public const string S2F35_Messgae = "S2F35_Link Event Report";
        public const string S2F36_Messgae = "S2F36_Link Event Report Ack";
        public const string S2F37_Messgae = "S2F37_Enable/Disable Event Report";
        public const string S2F38_Messgae = "S2F38_Enable/Disable Event Report Ack";
        public const string S2F41_Messgae = "S2F41_Host Command Send";
        public const string S2F42_Messgae = "S2F42_Host Command Ack";
        public const string S3F17_Messgae = "S3F17_Carrier Action Request";
        public const string S3F18_Messgae = "S3F18_Carrier Action Ack";
        public const string S5F1_Messgae = "S5F1_Alarm Report Send";
        public const string S5F2_Messgae = "S5F2_Alarm Report Ack";
        public const string S5F3_Messgae = "S5F3_Enable/Disable Alarm Send";
        public const string S5F4_Messgae = "S5F4_Enable/Disable Alarm Ack";
        public const string S6F11_Messgae = "S6F11_Event Report Send";
        public const string S6F12_Messgae = "S6F12_Event Report Ack";
        public const string S14F9_Messgae = "S14F9_Create Object Request";
        public const string S14F10_Messgae = "S14F10_Create Object Ack";
        public const string S16F11_Messgae = "S16F11_PRJobCreateEnh";
        public const string S16F12_Messgae = "S16F12_PRJobCreateEnh Ack";
        public const string S16F15_Messgae = "S16F15_PRJob Multi Create";
        public const string S16F16_Messgae = "S16F16_PRJob Multi Create Ack";
        #endregion
    }

    #endregion

    public enum CommandResult
    {
        OK,
        Fail,
        Unknown,
    }
    public enum readkind
    {
        RFID,
        Hand
    }

    public enum EFEMStatus
    {
        Unknown,
        Power_Off,
        SysCheck_Now,
        SysCheck_Fail,
        SysCheck_Finish,
        Init_Now,
        Init_Fail,
        Init_Finish,
        Ready_Now,
        Ready_Fail,
        Ready_Finish,
        Run_Now,
        Run_Fail,
        Run_Finish,
        Continue_Now,
        Continue_Fail,
        Continue_Finish,
        Alarming,
        MaxCnt,
    }

    public enum EFEMMode
    {
        Unknown,
        Local,
        Remote,
    }

    public enum AuthorityTable
    {
        Admin,
        Engineer,
        Operator,
        Null,
    }

    #region SQL

    public enum SQLTable
    {
        //CJ_Pool,
        PJ_Pool,
        PJ_History,
        //PJ_History_Temp,
        Authority,
        Account,
        PLC_Trace
    }

    public enum WaferInforTableItem
    {
        CJID,
        SocPortID,
        SocPort,
        SocSlot,
        SocSlotID_Up,
        SocSlotID_Down,
        DesPortID,
        DesPort,
        DesSlot,
        DesSlotID,
        IsOMS,
        StagePos,
        SwapPortID,
        SwapPort,
        SwapSlot,
        SwapSlotID_Up,
        SwapSlotID_Down,
        WaferStatus,
        CarrierStatus,
        StartTime,
        EndTime,
        PPID,
        IsAligner,
        IsOCR_Up,
        IsOCR_Down,
        IsFlip,
        OCRDegree,
        AlignerDegree,
        //Joanne 20211019 追加 Pass Wafer 功能
        PassWafer,
    }

    public enum SQLTableItem
    {
        Name,
        PassWord,
        Authority,
        UIName,
        UIIndex,
        Engineer,
        Admin,
        Operator,
        Init,
    }

    public enum PJ_Type
    {
        Load,
        Unload,
        LoadUnload,
        Sortering,
        MaxCnt,
    }

    public enum Core_Loop
    {
        Load,
        Unload,
        LoadUnload,
        Sortering,
        OMS_Load,
        OMS_Unload,
        OMS_LoadUnload,
        MaxCnt,
    }

    public enum SQLWaferInforStep
    {
        GetLP_Send,
        GetLP_Wait,
        PutAL_Send,
        PutAL_Wait,
        Alinger_Send,
        Aligner_Wait,
        ReadWaferID_Send,
        ReadWafer_Wait,
        GetAL_Send,
        GetAL_Wait,
        PutEQ_Send,
        PutEQ_Wait,
        PutLP_Send,
        PutLP_Wait,
        AL_Home,
        AL_GetStatus,
        GetEQ_Send,
        GetEQ_Wait,
        Finish
    }


    #endregion

    #endregion

    #region Wafer_Status_Enum

    public enum WaferStatus
    {
        WithOut = 0,
        With,
        Cross,
        Thickness,
        Thiness,
        Position,
        Unknown,
        MaxCnt,
    }

    #endregion

    #region Robot

    #region Status

    public static class Robot_Static
    {
        public const string RobotAddress = "001 ";
        public const string Uppwer = "Upper";
        public const string Lower = "Lower";
        public const int BYTESIZE = 8;
        public const int ARM500 = 500;
        public const int GET200 = 200;
        public const int TYPE1000 = 1000;
        public const int TOP2000 = 2000;
        public const int SMALL3000 = 3000;
        public const float TOROLANCE_XY = 5.0F;
        public const float TOROLANCE_W = 1.0F;
        public const float TOROLANCE_Z = 270.0F;
        public const float TOROLANCE_C = 20.0F;//125.0F;
        public const float TOROLANCE_GP = 1.0F;
        public const float TOROLANCE_GP_4 = 4F;

        public const double CassetteOffset = 6.35D;
        public const double MagazionOffset = 20D;
        public const double AlignerOffset = 15D;
        public const double StageOffset = 30D; //11D
        public const double BufferOffset = 40D;

        public static string TurnPos = "TurnPos";
        public static string TurnBackPos = "TurnBackPos";
        public static string PosAddress = "PosAddress";
        public static string PosParamter = "PosParamter";
        public static string RetryCount = "RetryCount";
        public static string StopRestart = "StopRestart";

        public static string StartAddress = "StartAddress";
        public static string RobotMapping = "Mapping";
        public static string RobotTop = "Top";
        public static string ArmNumber = "Arm_Number";
        public static string ArmTypeLower = "Arm_Type_Lower";
        public static string ArmTypeLowerOutType = "Arm_Type_Lower_Out";
        public static string ArmTypeLowerOutOn = "Arm_Type_Lower_Out_On";
        public static string ArmTypeLowerOutOff = "Arm_Type_Lower_Out_Off";
        public static string ArmTypeLowerInCnt = "Arm_Type_Lower_In_Cnt";
        public static string ArmTypeLowerIn = "Arm_Type_Lower_In";
        public static string ArmTypeLowerInWaferFlag = "Arm_Type_Lower_In_WF";

        public static string ArmTypeUpper = "Arm_Type_Upper";
        public static string ArmTypeUpperOutType = "Arm_Type_Upper_Out";
        public static string ArmTypeUpperOutOn = "Arm_Type_Upper_Out_On";
        public static string ArmTypeUpperOutOff = "Arm_Type_Upper_Out_Off";
        public static string ArmTypeUpperInCnt = "Arm_Type_Upper_In_Cnt";
        public static string ArmTypeUpperIn = "Arm_Type_Upper_In";
        public static string ArmTypeUpperInWaferFlag = "Arm_Type_Upper_In_WF";

        public static string Speed = "Speed";
    }

    #endregion

    #region Robot_Axis_Enum

    public enum RobotAxis
    {
        X = 0,
        Y,
        Z,
        W,
        R,
        C,
        MaxCnt = 7,
    }

    #endregion

    #region Robot_GPT

    public enum RobotGPT
    {
        Get,
        Put,
        TopGet,
        TopPut,
        Mapping,
        MaxCnt,
    }

    #endregion

    #region Robot_ArmType_Enum
    public enum ArmType
    {
        NoUse = 0,
        Bernoulli,
        EdgeGrip,
        Vacuum,
        Pad,
        EdgeAndBernoulli,
        MaxCnt,
    }
    #endregion

    #region Robot_ArmStatus_Enum

    public enum ArmStatus
    {
        Arm_Home = 0,
        Arm_Extend,
        Arm_Now_Turn,
        Arm_Turn,
        MaxCnt,
    }

    #endregion

    #region Robot_Now_Position_Enum

    public enum RobotPosition
    {
        Home,
        Turn_AL,
        Turn_S1,
        Turn_S2,
        Aligner1,
        P1,
        P1_Map,
        P2,
        P2_Map,
        P3,
        P3_Map,
        P4,
        P4_Map,
        P5,
        P5_Map,
        P6,
        P6_Map,
        P7,
        P7_Map,
        P8,
        P8_Map,
        P9,
        P9_Map,
        P10,
        P10_Map,
        Stage1,
        Stage2,
        Buffer1,
        Unknown,
        MaxCnt,
    }
    #endregion

    #region Robot_Step_Enum

    public enum RobotStep
    {
        #region Normal_GP

        GP_Normal_0 = 0,
        GP_Normal_1,
        GP_Normal_2,
        GP_Normal_3,
        GP_Normal_4,
        GP_Normal_5,
        GP_Normal_6,
        GP_Normal_7,
        GP_Normal_8,
        GP_Normal_9,
        GP_NormalHomeZ_Back,

        #endregion

        #region Top_GP

        GP_Top_0,
        GP_Top_1,
        GP_Top_2,
        GP_Top_3,
        GP_Top_4,
        GP_Top_5,
        GP_Top_6,
        GP_Top_7,
        GP_Top_8,
        GP_Top_9,
        GP_TopHomeZ_Back,

        #endregion

        #region Turn

        GP_Turn_0,
        GP_Turn_1,
        GP_Turn_2,
        GP_Turn_3,

        GP_TurnBack_0,
        GP_TurnBack_1,
        GP_TurnBack_2,
        GP_TurnBack_3,

        #endregion

        #region Mapping

        GP_Mapping_0,
        GP_Mapping_1,
        GP_Mapping_2,
        GP_Mapping_3,

        #endregion

        #region Home

        GP_Home_0,
        GP_Home_1,
        GP_Home_SnakeMotion,
        GP_Home_SnakeMotion_LP1,
        GP_Home_SnakeMotion_LP2,
        GP_Home_SnakeMotion_EQ1,
        GP_Home_SnakeMotion_EQ2,
        GP_AlignerOCR_Up,
        GP_AlignerOCR_Down,

        #endregion

        #region Get

        Get_Status,
        Get_LBD,
        Get_DI_0,
        Get_DI_1,
        Get_Vac,
        Get_Edge,
        Get_Bernoulli,
        Get_MappingData,
        Get_MappingErrorData,

        #endregion

        #region Set

        Set_Stop,
        Set_Restart,
        Set_RsetError,
        Set_Speed,
        Set_BackSpeed,
        Set_VacOn,
        Set_VacOff,
        Set_EdgeOn,
        Set_EdgeOff,
        Set_BernoulliOn,
        Set_BernoulliOff,

        SetLowerArm,
        SetUpperArm,
        Set_Sleep_500ms,

        #endregion

        #region Check

        Check_VacOn,
        Check_VacOff,
        Check_EdgeOn,
        Check_EdgeOff,
        Check_BernoulliOn,
        Check_BernoulliOff,

        Check_WithLoop1s,
        Check_With,
        Check_WithoutLoop1s,
        Check_Without,

        Check_With_ABN,
        Check_With_ABN_RetryEnd,
        Check_Without_ABN,
        Check_Without_ABN_RetryEnd,

        Check_TopR_IsTurn_Jump1,

        Check_HomeR_NormalOrTurn_Jump1,
        Check_HoemR_FinalOrUndone_Jump2,

        CheckError_IsTurn,
        CheckError_IsExtend,

        Check_ArmWith_Jump12,
        Check_Home_GPNeedJumpEnd,
        Check_Home_PosRuleNeedJumpEnd,
        Check_Home_GPJumpStart,

        #endregion

        #region Update

        Update_RobotPosition,
        Update_HomeBackGPPosition,
        Update_TeachingPosition,
        Update_TurnStatus,
        Update_ArmPresence,

        #endregion

        #region Jump

        Jump_Check1,
        Jump_Check2,
        Jump_AbnStart,
        Jump_AbnEnd,
        Jump_TopHomeStart,
        Jump_TopHomeEnd,

        Jump_NornalHomeChek1,
        Jump_NornalHomeStart,
        Jump_NornalHomeEnd,

        #endregion

        #region wait

        Wait_VacOff,
        Wait_VacOn,
        Wait_BernoulliOff,
        Wait_BernoulliOn,

        #endregion

        EQ_LD_start,
        EQ_LD_End,

        EQ_ULD_start,
        EQ_ULD_End,

        End,
        MaxCnt,
    }

    #endregion

    #endregion

    #region LoadPort

    public enum LPStep
    {
        GetStatus,
        GetTypeData,
        SetMappParameter, // 20190612
        DoMapping,
        GetWaferStatus,
        GetWaferThick01,
        GetWaferThick02,
        GetWaferThick03,
        GetWaferThick04,
        GetWaferThick05,
        GetWaferThick06,
        GetWaferPosition01,
        GetWaferPosition02,
        GetWaferPosition03,
        GetWaferPosition04,
        GetWaferPosition05,
        GetWaferPosition06,
        ResetError,
        Home,
        Load,
        Unload,
        Clmap,
        Unclmap,
        ClearMappData,
        GetType1,
        GetType2,
        GetType3,
        GetType4,
        GetType5,
        SetType1,
        SetType2,
        SetType3,
        SetType4,
        SetType5,
        ResetSlotData,
        LoadLED_On,
        LoadLED_Off,
        LoadLED_Blink,
        UnloadLED_On,
        UnloadLED_Off,
        UnloadLED_Blink,
        Status1LED_On,
        Status1LED_Off,
        Status1LED_Blink,
        Status2LED_On,
        Status2LED_Off,
        Status2LED_Blink,
        SwitchLED_On,
        SwitchLED_Off,
        SwitchLED_Blink,
        GetLEDStatus,
        GetZAxisPos,
        EventEnd,
        End,
        MaxCnt,
    }

    public enum LPType
    {
        Type1 = 0,
        Type2,
        Type3,
        Type4,
        Type5,
        Type6,
        Type7,
        Type8,
        Type9,
        Type10,
        MaxCnt,
    }

    public enum LPFoupMount
    {
        Absent = 0,
        Present,
        Unknown,
    }

    public enum LPPosition
    {
        InProcess = 0,
        Unload,
        Load,
    }

    public enum LPClamp
    {
        Clamp,
        Unclamp,
        Unknown
    }

    public enum LPDoor
    {
        Open,
        Close,
        Unknown
    }

    #endregion

    #region RFID

    #region Step

    public enum RFIDStep
    {
        Initial,
        ReadData,
        SetPageMap,
        End,
        MaxCnt,
    }

    #endregion 

    #endregion

    #region CP/MP

    public enum IOLPDevice
    {
        CP1,
        CP2,
        CP3,
        CP4,
        CP5,
        CP6,
        CP7,
        CP8,
        CP9,
        CP10,
        MP1,
        MP2,
        MP3,
        MP4,
        MP5,
        MP6,
        MP7,
        MP8,
        MaxCnt,
    }





    public enum IOLPStep
    {
        ResetError,
        OutDoorOpen,
        OutDoorClose,
        SetHomeCmd,
        SetLoadCmd,
        SetUnLoadCmd,
        SetReloadingCmd, // 20201109 Walson 用SECS指令來觸發Magazine Load動作
        RequestOn,
        ClearCmd,
        WaitReplyOn,
        WaitReplyOff,
        WaitFinish,
        GetReply,
        RequestOff,
        CompleteRequestOn,
        CompleteReplyOn,
        CompleteRequestOff,
        CompleteReplyOff,
        StartAlarmReset,
        WaitAlarmReset,
        End,
        MaxCnt,
    }

    public enum IOLPDoor
    {
        Open,
        Close,
        Unknown
    }

    public enum IOLPLed
    {
        Open,
        Close,
        Run,
        Unknown,
    }

    public enum CassetterPortType
    {
        Real,
        Dummy,
    }

    public enum MagazinePortType
    {
        Real,
        OMS_In,
        OMS_Out,
    }

    public enum PortDi
    {
        Protrude = 6,
        Gratina,
        InClose,
        OutClose,
        InOpen,
        OutOpen,
        Presence,
        Alarm,
        PLC_Busy,
        Ready,
    }

    #endregion

    #region Aligner

    public enum AlignerStep
    {
        ResetError,
        GetStatus,

        Home,
        Alignment,
        FindNotch,
        ToAngle,
        GetAlignerDegree,
        SetAlignerDegree,
        GetIDReaderDegree,
        SetIDReaderDegree,
        SetCycleOCR,
        SetCycleEQ,
        AlignerVacuumOn,
        AlignerVacuumOff,
        SetAlignerWaferType,
        SetAlignerWaferTypeFix_8,
        SetAlignerWaferTypeFix_12,
        GetAlignerWaferType,

        Set_PLC_Up,
        Set_PLC_Down,

        End,
        MaxCnt,
    }

    public enum AlignerStatus
    {
        Home,
        FindNotch,
        ToAngle,
        Alignment,
        Unknown,
    }

    public enum LiftPinEnum
    {
        Up,
        Down,
        Unknown,
    }

    public enum AlignerDi
    {
        Presence = 12,
        Protrude,
        PinDown,
        PinUp,
    }

    #endregion

    #region Stage

    public static class EQ_Static
    {
        public const string OK = "OK";
        public const string FAIL = "FAIL";
    }

    public enum StageStep
    {
        //Set
        SetHome,
        SetClamp,
        SetLoad,
        SetUnload,
        SetGetCWafer,
        SetPutCWafer,
        SetGetWWafer,
        SetPutWWafer,
        SetResetError,

        //InterFace
        RequestOn,
        WaitReplyOn,
        RequestOff,
        ClearCmd,
        WaitReplyOff,
        WaitFinish,
        CompleteRequestOn,
        CompleteReplyOn,
        CompleteRequestOff,
        CompleteReplyOff,
        CheckPresence, // 20201122 Walson 確認Command完成後在席狀態是否正常
        StartAlarmReset,
        WaitAlarmReset,
        MaxCnt,
        End,
    }

    public enum StageStatus
    {
        Home,
        Mix,
        Separater,
        Unknown,
    }

    public enum StageDi
    {
        WaferAvailable = 3,
        CarrierAvailable,
        bit10,
        bit9,
        CarrierrProtrude,
        WaferProtrude,
        RobotWaferPresence,
        CarrierPresence,
        CarrierWaferPresence,
        WaferPresence,
        Alarm,
        PLC_StageBusy,
        Ready,
    }
    #endregion

    #region Adam

    #region Status

    public static class Adam_Static
    {
        public static string IOType = "IO Type";
        public static string IOName = "IO Name";
        public static string Channel = "Channel";
        public static string IOValue = "IO Value";
        public const string SetDO = "SetDO";
        public const string SetFlashDO = "SetFlashDO";
        public const string SetAllDO = "SetAllDO";
        public const string SetAO = "SetAO";
        public const string SetAORange = "SetAORange";
        public const string SetAIRange = "SetAIRange";

        public const string SetDO_Flash = "SetDO_Flash";
        public const string SetDO_All = "SetDO_All";
        public const string SetDO_AllFlash = "SetDO_AllFlash";
    }

    #endregion

    #region Adam6050

    public enum Adam6050_DI
    {
        Pressure,
        Vacuum,
        Ionizer1,
        FFU1,
        EQWafer,
        Stage1,
        RobotMode,
        RobotEnable,
        Door,
        EMO,
        Power,
        Null_1,
        //~~~~~~~~~
        WaferLoad,
        WaferUnload,
        Z_HighLimit,
        X_HighLimit,
        Y_HighLimit,
        WaferDetect,
        MicroscopeEMO,
        WaferShift, //20220125_Elijah
        CylinderCloseDI,
        CylinderOpenDI,
        Null_2,
        Null_3,
        //~~~~~~~~~~
        MaxCnt,
    }

    public enum Adam6050_DO
    {
        ST_Red,
        ST_Yellow,
        ST_Green,
        ST_Blue,
        Buzzer_1,
        EQMoving,
        Null_1,
        //~~~~~~~~~~
        CylinderOpenDo,
        CylinderCloseDo,
        Null_2,
        Null_3,
        Null_4,
        //~~~~~~~~~
        MaxCnt,
    }

    public enum SignalTown
    {
        AllOn,
        AllOff,
        AllFlash,
        RedOn,
        RedOff,
        RedFlash,
        YellowOn,
        YellowOff,
        YellowFlash,
        GreenOn,
        GreenOff,
        GreenFlash,
        BlueOn,
        BlueOff,
        BlueFlash,
    }


    #endregion

    #region Adam6024

    public enum Adam6024_AI
    {
        FFUPressure = 0,
        StaticElectricity,
        null_0,
        null_1,
        null_2,
        null_3,
        //~~~~~~~~~
        MaxCnt,
    }

    public enum Adam6024_AO
    {
        FFUControl_1 = 0,
        null_0,
        //~~~~~~~~~
        MaxCnt,
    }

    #endregion

    #endregion

    #region Log

    //public enum MainLanguage
    //{
    //    Item = 0,
    //    Englist,
    //    TS,
    //    CT,
    //    MaxCnt,
    //}

    public enum LogExcelTable
    {
        Log = 0,
        Error,
    }

    public enum SystemList
    {
        CommandStart,
        CommandComplete,
        CommandParameter,
        ConnectStatus,
        ResetErrorStart,
        UserLogin,
        UserLogOut,
        ProgramOpen,
        ProgramClose,
        SocketSend,
        SocketReceive,
        DeviceSend,
        DeviceReceive,
        DiReceive,
        ReadConfig,
        //14
    }

    public enum ErrorList
    {
        #region 01

        ChecksumError_0101 = 0,
        AL_DeviceError_0101,
        BC_ReadIDError_0101,
        RB_OverS1_0101,
        RB_NotEQStopNotOn_0102,
        RB_StopOn_0103,
        RB_InES_0104,
        RB_ESStopOn_0105,
        RB_OverS2_0106,
        RB_NeitherACAL_0107,
        RB_ZOut_0108,
        RB_ACALNotComplete_0109,
        RB_PosNotComplete_0110,
        RB_OverS3_0112,
        RB_ActionNotCmd_0113,
        RB_OverS4_0114,
        RB_StepFai_0115,
        RB_NowNotStop_0116,
        RB_Abn_0117,
        RB_ArmOnFail_0118,
        RB_ArmOffFail_0119,
        RB_Without_0120,
        RB_With_0121,
        RB_ArmUnknown_0122,

        AP_ExceptionOccur_0199,
        //29
        #endregion

        #region 02

        AP_CommandNoSup_0288,
        AP_ParameterFail_0291,
        //6
        #endregion

        #region 03

        DeviceIsBusy_0301,
        RB_ObjectBusy_0302,
        RB_LPDoorNotOpen_0303,
        RB_UpperNotTurn_0304,
        RB_NotDoTopAction_0305,
        RB_DeviceWith_0306,
        RB_DeviceWithout_0307,
        RB_DeviceSlotCountError_0308,
        RB_DevicePosUnKnown_0309,
        RB_WaferDataError_0313,
        RB_CaseDefault_0314,
        RB_ArmExtend_0316,
        RB_LowerTurn_0317,
        RB_AddressMovStop_0318,
        RB_NowAddressNotSafty_0319,
        LP_NowNotLoad_0330,
        LP_NowNotUnLoad_0331,
        LP_NotFoup_0333,
        AL_NotHome_0341,
        AL_VauccmFail_0342,
        AP_SerialError_0381,
        AP_SocketError_0382,
        AP_IniError_0383,
        AP_ExcelError_0384,
        AP_InitialFail_0393,
        //30
        #endregion

        #region 04

        AL_Interlock_0410,
        AL_Interlock_0413,
        AL_Interlock_0415,
        AL_Interlock_0416,
        AL_Interlock_0418,
        AL_Interlock_0421,
        //21
        #endregion

        #region 04 (Loadport)

        LP_PortOpenFail_0400,
        LP_SocketError_0401,
        LP_ConnectTimeout_0402,
        LP_ChecksumError_0403,
        LP_ComPortDisabled_0404,

        LP_IsBusy_0410,
        LP_IsLoad_0411,
        LP_IsUnload_0412,
        LP_FoupAbsent_0413,
        LP_FoupAbnormal_0414,
        LP_InMaintainMode_0415,
        LP_RobotUnknown_0416,
        LP_RobotIntrude_0417,

        LP_02CmdError_0440,
        LP_06InProcess_0441,
        LP_07ModeError_0442,
        LP_08MappingError_0443,
        LP_05InAalarm_0444,
        LP_04Interlock_0445,
        LP_04Interlock_0446,
        LP_04Interlock_0447,
        LP_04Interlock_0448,
        LP_04Interlock_0449,
        LP_04Interlock_0450,
        LP_04Interlock_0451,
        LP_04Interlock_0452,
        LP_04Interlock_0453,
        LP_04Interlock_0454,
        LP_04Interlock_0455,
        LP_04Interlock_0456,
        LP_04Interlock_0457,
        LP_04Interlock_0458,
        LP_05Error_0459,
        LP_05Error_0460,
        LP_05Error_0461,
        LP_05Error_0462,
        LP_05Error_0463,
        LP_05Error_0464,
        LP_05Error_0465,
        LP_05Error_0466,
        LP_05Error_0467,
        LP_05Error_0468,
        LP_05Error_0469,
        LP_05Error_0470,
        LP_05Error_0471,
        LP_05Error_0472,
        LP_05Error_0473,
        LP_05Error_0474,
        LP_05Error_0475,
        LP_05Error_0476,
        LP_05Error_0477,
        LP_05Error_0478,
        LP_05Error_0479,
        LP_05Error_0480,
        LP_05Error_0481,
        LP_05Error_0482,
        LP_05Error_0483,
        LP_05Error_0484,
        LP_05Error_0485,
        LP_05Error_0486,
        LP_05Error_0487,
        LP_05Error_0488,
        LP_05Error_0489,
        LP_05Error_0490,
        LP_05Error_0491,
        LP_05Error_0492,
        LP_05Error_0493,
        LP_05Error_0494,
        LP_05Error_0495,
        LP_05Error_0496,
        LP_05Error_0497,
        LP_05Error_0498,

        #endregion

        #region 05

        AL_Alarm_0505,
        AL_Alarm_0510,
        AL_Alarm_0511,
        AL_Alarm_0512,
        AL_Alarm_0513,
        AL_Alarm_0514,
        AL_Alarm_0540,
        AL_Alarm_0550,
        AL_Alarm_0599,
        AL_Alarm_05A0,
        AL_Alarm_05D0,
        AL_Alarm_05D1,
        AL_Alarm_05D3,
        AL_Alarm_05D5,
        AL_Alarm_05D7,
        AL_Alarm_05D8,
        AL_Alarm_05D9,
        AL_Alarm_05DA,
        AL_Alarm_05DB,
        AL_Alarm_05DC,

        OC_Alarm_0505,

        RB_Alarm_0509,
        RB_Alarm_0510,
        RB_Alarm_0520,
        RB_Alarm_0530,
        RB_Alarm_0531,
        RB_Alarm_0532,
        RB_Alarm_0540,
        RB_Alarm_0551,
        RB_Alarm_0561,
        RB_Alarm_0562,
        RB_Alarm_0563,
        RB_Alarm_0564,
        RB_Alarm_0567,
        RB_Alarm_0570,
        RB_Alarm_0580,
        RB_Alarm_0582,
        RB_Alarm_0584,
        RB_Alarm_0590,
        RB_Alarm_0595,
        RB_Alarm_05A0,
        RB_Alarm_05B0,
        RB_Alarm_05C0,
        RB_Alarm_05D0,
        RB_Alarm_05D1,
        RB_Alarm_05D3,
        RB_Alarm_05D5,
        RB_Alarm_05D7,
        RB_Alarm_05D8,
        RB_Alarm_05D9,
        RB_Alarm_05DA,
        RB_Alarm_05DB,
        RB_Alarm_05DC,
        RB_Alarm_05E0,
        LP_CloseGratingOn_0534,
        LP_OpenGratingOn_0535,
        LP_OutCloseTimeout_0536,
        LP_OutOpenTimeout_0537,
        LP_OutDoorIsOpen_0538,
        LP_InDoorIsOpen_0539,
        LP_ProtrudeIsTrue_053B,
        AL_LiftPinNotUp_053C,
        ProcessError,
        DeviceNotReady,
        PLCCmdRecvError,
        AP_DeviceWaferSensorError_0540,
        RB_DeviceInvasion_0541,
        LP_BarCodeIsNull_0542,
        EF_Air1Error_0543,
        EF_Air2Error_0544,
        EF_EMS_0545,
        EF_DoorOpen_0546,
        EF_PowerOff_0547,
        EF_Manual_0548,

        AP_PresenceIniNotMatch_0549,
        SG_WaferAvailableOff_0550,
        SG_CarrierAvailableOff_0551,
        AP_DeviceNotWithoutNoAuto_0552,
        //111
        #endregion

        #region 05 (Aligner)

        AL_PortOpenFail_0400,
        AL_SocketError_0401,
        AL_ConnectTimeout_0402,
        AL_ChecksumError_0403,
        AL_ComPortDisbled_0404,

        AL_02CmdError_0540,
        AL_06InProcess_0541,
        AL_07ModeError_0542,
        AL_05InAlarm_0543,
        AL_04Interlock_0444,
        AL_04Interlock_0445,
        AL_04Interlock_0446,
        AL_04Interlock_0447,
        AL_04Interlock_0448,
        AL_04Interlock_0449,
        AL_05Error_0450,
        AL_05Error_0451,
        AL_05Error_0452,
        AL_05Error_0453,
        AL_05Error_0454,
        AL_05Error_0455,
        AL_05Error_0456,
        AL_05Error_0457,
        AL_05Error_0458,
        AL_05Error_0459,
        AL_05Error_0460,
        AL_05Error_0461,
        AL_05Error_0462,
        AL_05Error_0463,
        AL_05Error_0464,
        AL_05Error_0465,
        AL_05Error_0466,
        AL_05Error_0467,
        AL_05Error_0468,

        #endregion

        #region 06

        LP_InProcess_0606,
        #region 06 (IO Loadport/RFID)

        LP_IoDeviceDisc_0600,
        LP_ActionTimeOut_0601,

        LP_IsBusy_0610,
        LP_CstAbsent_0611,
        LP_CstAbnormal_0612,
        LP_RobotUnknown_0613,
        LP_RobotIntrude_0614,
        LP_Protrude_0615,
        LP_Grating_0616,
        LP_CstCover_0617,
        LP_OutdoorOpened_0618,
        LP_IndoorOpened_0619,

        LP_GratingTriggered_0640,
        LP_GratingTriggered_0641,
        LP_ProtrudeDuringLoad_0642,
        LP_CstCoverDrop_0643,
        RF_PortOpenFail_0670,
        RF_SocketError_0671,
        RF_ConnectTimeout_0672,
        RF_ComPortDisabled_0673,

        RF_IsBusy_0680,

        RF_Error_0690,
        RF_Error_0691,
        RF_Error_0692,
        RF_Error_0693,
        RF_Error_0694,
        RF_Error_0695,

        #endregion
        //1
        #endregion

        #region 07

        RB_OnlineMode_0701,
        RB_ManualMode_0702,
        RB_AutoMode_0703,
        //4
        #endregion

        #region 08

        //LP_MappingError_0808,
        //1
        #endregion

        #region 09

        //LP_SerialError_0909,
        //1
        AP_TryCatchError,
        IO_6050ConnectFail_0900,
        IO_6024ConnectFail_0901,
        IO_6050Disconnect_0902,
        IO_6024Disconnect_0903,
        IO_AdamDisabled_0904,

        IO_EtherCatConnectFail_0905,
        IO_EtherCatConnectError_0906,
        IO_EtherCatDisabled_0907,

        IO_PLCConnectFail_0910,
        IO_PLCDisonnect_0911,
        IO_PLCDisabled_0912,

        IO_EfemESTrigger_0920,
        IO_EfemDoorOpened_0921,
        IO_EfemMaitainMode_0922,
        IO_EfemPowerDown_0923,

        IO_SetBuzzerFail_0960,
        IO_SetSignalTowerFail_0961,
        IO_SetInterlockFail_0962,
        IO_SetFFUFail_0963,
        IO_Microscope_0964,
        IO_Microscope_0965,//20220125_Elijah
        #endregion

        #region 10

        Timeout_1010,
        T0TimeOut_1011,
        T1TimeOut_1012,
        T2TimeOut_1013,
        T3TimeOut_1014,
        StageRetryFail_1015,

        #endregion
        //Wayne 20210913 for c -> m Mapping Fail
        Load_JobFinish_Mapping_Alarm_1030,
        SECS_Exception_Error_1031,  //Joanne 20210922
        #region 11  Warning

        OC_CheckFail_1101,
        EF_Ionizer_1102,
        EF_FFU_1104,
        EF_NoJob_1105,
        EF_JobStatusFail_1106,

        #endregion

        MaxCnt,
    }

    public enum LogDir
    {
        System = 0,
        Robot1,
        LP,
        Aligner,
        E84,
        Stage,
        OCRReader,
        IO,
        Error,
        Alarm,
        Operation,
        SECS,
        Version,
        Trace,
        MaxCnt,
    }

    #endregion

    #region IO

    public enum EFEM_DI
    {
        Pressure = 0,//0
        Vacuum,//1
        Ionizer,//2
        FFU1,//3
        FFU2,//4
        FFU3,//5
        RobotMode,//6
        RobotEnable,//7
        DoorOpen,//8
        EMO,//9
        Power,//10
        FlowMeter//11
    }

    public enum EFEM_FFU
    {
        FFU,
        MaxCnt = 12
    }

    public enum EFEM_AI
    {
        MaxCnt,
    }

    public enum EFEM_DO
    {
        Red_Lamp,
        Yellow_Lamp,
        Green_Lamp,
        Buzzer,
        MaxCnt,
    }

    public enum EFEM_AO
    {
        MaxCnt,
    }


    #endregion

    #region Socket

    public static class Socket_Static
    {
        public const string ReplyINF = "INF";
        public const string ReplyABS = "ABS";
        public const string ReplyMOV = "MOV";
        public const string ReplySET = "SET";
        public const string ReplyGET = "GET";

        public const string ReplyNormal_00 = "00";
        public const string ReplyDeviceError_01 = "01";
        public const string ReplyFormatError_02 = "02";
        public const string ReplyAlarmTable_03 = "03";
        public const string ReplyInterlock_04 = "04";
        public const string ReplyInAlarm_05 = "05";
        public const string ReplyInProcess_06 = "06";
        public const string ReplyModeError_07 = "07";
        public const string ReplyMappingError_08 = "08";
        public const string ReplySerialError_09 = "09";
        public const string ReplyTimeout_10 = "10";
    }

    #endregion

    #region Core

    public struct CmdStruct
    {
        public bool Core;
        public string obj;
        public int port;
        public SocketCommand command;
        public string Parameter;
    }

    public enum SocketCommand
    {
        #region API

        API_Start = 0,
        Version,
        Remote,
        Local,
        CurrentMode,
        Hide,
        Show,
        API_End,

        Home,
        ResetError,
        GetStatus,

        #endregion

        #region RFID

        RFID_Start,
        ReadFoupID,
        RFID_End,

        #endregion

        #region LP

        LP_Start,

        Load,
        Unload,
        SetType,

        LP_End,

        #endregion

        #region Robot

        Robot_Start,
        Stop,
        ReStart,
        SetRobotSpeed,
        ReadPosition,
        //EdgeGripOn,
        //EdgeGripOff,
        BernoulliOn,
        BernoulliOff,
        WaferGet,
        WaferPut,
        GetStandby,
        PutStandby,
        CheckWaferPresence,
        ArmSafetyPosition,
        WaferPut_EQ,

        Robot_End,

        #endregion

        #region Aligner

        Aligner_Start,

        AlignerVacuum_on,
        AlignerVacuum_off,
        Alignment,
        FindNotch,
        ToAngle,
        GetAlignerDegree,
        SetAlignerDegree,
        GetIDReaderDegree,
        SetIDReaderDegree,

        Aligner_End,

        #endregion

        #region Alignment

        Alignment_Start,
        AlignmentClamp,
        AlignmentUnclamp,
        Alignment_End,

        #endregion

        #region OCR_BarCode

        OCR_Start,
        Read,
        OCR_End,

        #endregion

        #region IO

        IO_Start,
        SignalTower,
        GetSignalTower,
        GetPressureDifference,
        Buzzer,
        SetFFUVoltage,
        GetBufferStatus,
        IO_End,

        #endregion

        #region E84

        E84_Start,
        LinkTest,
        Enable,
        Auto,
        Manual,
        Reset,
        SetESOn,
        SetESOff,
        SetTimeOut,
        GetFoupSensorStatus,
        GetSensorOutputStatus,
        GetSensorInputStatus,
        GetDIStatus,
        GetControllerErrorCode,
        E84_End,
        #endregion

        #region Stage

        Stage_Start,
        Clamp,
        Mix,
        Separation,
        Get_Carrier_Wafer,
        Put_Carrier_Wafer,
        Get_Wafer_Wafer,
        Put_Wafer_Wafer,
        Stage_End,

        #endregion

        MaxCnt,

        Event,
        Initial,
        InitialHome,
        CycleHomeCheckDegree,
        CycleAlignmentOCR,
        CycleAlignmentFinish,

        #region RFID_No_Use

        SetPageMap,

        #endregion

        #region IO_NO_Use

        GetEFEMInterlock,
        SetEFEMInterlock,
        GetEQInterlock,
        ShutterDoor,
        GetShutterDoorStatus,
        GetBufferProtrusionSensor,

        #endregion

        #region Robot_No_Use

        CheckArmOnSafetyPos,
        TopWaferGet,
        TopWaferPut,
        TopGetStandby,
        TopPutStandby,
        RobotMapping,
        SetRobotWaferInch,
        GetRobotWaferInch,
        GetRobotMappingResult,
        GetRobotMappingErrorResult,
        GetRobotMappingResult2,
        GetRobotMappingErrorResult2,
        VacuumOn,
        VacuumOff,

        #endregion

        #region Aligner_No_Use

        LiftPinUp,
        LiftPinDown,

        SetAlignerWaferType,
        GetAlignerWaferType,
        AlignerVacuum,

        #endregion

        #region LP_No_Use
        InDoorOpen,
        //Clamp,
        UnClamp,
        GetWaferSlot,
        GetWaferSlot2,
        GetWaferThickness,
        GetWaferPosition,
        
        SetMapp,
        GetMapp,
        GetProtrusionSensor,
        LEDLoad,
        LEDUnLoad,
        LEDStatus1,
        LEDStatus2,
        GetLEDStatus,
        SetOperatorAccessButton,
        GetZAxisPos,
        OutDoorOpen,
        OutDoorClose,
        Reloading,  // 20201109 Walson 用SECS指令來觸發Magazine Load動作
        Map,

        #endregion

        #region OCR_No_Use

        Connect,

        #endregion

        #region Stage_No_Use



        #endregion
    }
    #endregion

    #region SECS

    public enum IDExcelTable
    {
        CEID,
        RPTID,
        VID,
    }

    public enum CEID_Item
    {
        ToolModeChange_Offline,         //Index = 0
        ToolModeChange_Local,           //Index = 1
        ToolModeChange_Remote,          //Index = 2
        Alarm_Detect,                   //Index = 3
        Alarm_Clear,                    //Index = 4
        LoadComplete,                   //Index = 5
        UnloadComplete,                 //Index = 6
        ReadyToLoad,                    //Index = 7
        ReadyToUnload,                  //Index = 8
        CarrierIDRead_WaitingForHost,   //Index = 9
        SlotMapResult_WaitingForHost,   //Index = 10
        SlotMapVerifyOK,                //Index = 11
        DockComplete,                   //Index = 12
        CarrierReleaseComplete,         //Index = 13
        ControlJobStart_Auto,           //Index = 14
        ControlJobEnd_Complete,         //Index = 15
        PJPooled,                       //Index = 16
        PJSettingUp,                    //Index = 17
        ProcessJobStart_Auto,           //Index = 18
        ProcessJobEnd_Complete,         //Index = 19
        Trigger_ReadOCR,                //Index = 20
        Trigger_ReadTrayID,             //Index = 21
        Trigger_CombineTrayWafer,       //Index = 22
        OMS_Out,                        //Index = 23
        OMS_ReadID,                     //Index = 24    //Joanne 20201106 Add
        Continue_Finish,                //Index = 25 
    }

    #region VID Item
    public enum VID_Item
    {
        Clock,
        ControlMode,
        PreviousProcessState,
        CST1_AccessModeState,
        CST2_AccessModeState,
        CST3_AccessModeState,
        CST4_AccessModeState,
        CST5_AccessModeState,
        CST6_AccessModeState,
        CST7_AccessModeState,
        CST8_AccessModeState,
        CST9_AccessModeState,
        CST10_AccessModeState,
        MGZ1_AccessModeState,
        MGZ2_AccessModeState,
        MGZ3_AccessModeState,
        MGZ4_AccessModeState,
        MGZ5_AccessModeState,
        MGZ6_AccessModeState,
        MGZ7_AccessModeState,
        MGZ8_AccessModeState,
        PortTransferList,
        CST1_PortTransfer,
        CST2_PortTransfer,
        CST3_PortTransfer,
        CST4_PortTransfer,
        CST5_PortTransfer,
        CST6_PortTransfer,
        CST7_PortTransfer,
        CST8_PortTransfer,
        CST9_PortTransfer,
        CST10_PortTransfer,
        MGZ1_PortTransfer,
        MGZ2_PortTransfer,
        MGZ3_PortTransfer,
        MGZ4_PortTransfer,
        MGZ5_PortTransfer,
        MGZ6_PortTransfer,
        MGZ7_PortTransfer,
        MGZ8_PortTransfer,
        CarrierLocationMatrix,
        CST1_ID,
        CST2_ID,
        CST3_ID,
        CST4_ID,
        CST5_ID,
        CST6_ID,
        CST7_ID,
        CST8_ID,
        CST9_ID,
        CST10_ID,
        MGZ1_ID,
        MGZ2_ID,
        MGZ3_ID,
        MGZ4_ID,
        MGZ5_ID,
        MGZ6_ID,
        MGZ7_ID,
        MGZ8_ID,
        AlarmID,
        Current_AccessMode,
        CarrierAccessingStatus,
        Current_PortID,
        FoupID_Status,
        LocationID,
        Current_PortNo,
        PortListForAccessMode,
        Current_PortTransferState,
        Reason,
        SlotMapStatus,
        SlotMap,
        FoupType,
        Current_SlotNo,
        Current_MagazineID,
        ProcessJob_ID,
        ProcessJob_State,
        PrvPRJobState,
        PRMtlNameList,
        RecVariableList,
        ControlJob_ID,
        ControlJob_State,
        PrvCJState,
        SubstState,
        PrvSubstState,
        Current_WaferType,
        Upper_WaferID,
        CarrierID,
        Lower_WaferID,
        CombinePort,
        CombinePortID,
        All_PortType,
        CST1_PortType,
        CST2_PortType,
        CST3_PortType,
        CST4_PortType,
        CST5_PortType,
        CST6_PortType,
        CST7_PortType,
        CST8_PortType,
        CST9_PortType,
        CST10_PortType,
        MGZ1_PortType,
        MGZ2_PortType,
        MGZ3_PortType,
        MGZ4_PortType,
        MGZ5_PortType,
        MGZ6_PortType,
        MGZ7_PortType,
        MGZ8_PortType,
        WaferInfo,
        SubstInfoList,
    }
    #endregion

    public enum CJData_Item
    {
        SocPortID,
        SocPort,
        SocSlot,
        SocSlotID,
        DesPortID,
        DesPort,
        DesSlot,
        DesSlotID,
        IsOMS,
        StagePos,
        SwapPortID,
        SwapPort,
        SwapSlot,
        SwapSlotID,
    }

    #region Stream And Function
    public enum StreamNo
    {
        S1 = 1,
        S2,
        S3,
        S4,
        S5,
        S6,
        S7,
        S8,
        S9,
        S10,
        S11,
        S12,
        S13,
        S14,
        S15,
        S16,
    }

    public enum FunctionNo
    {
        F1 = 1,
        F2,
        F3,
        F4,
        F5,
        F6,
        F7,
        F8,
        F9,
        F10,
        F11,
        F12,
        F13,
        F14,
        F15,
        F16,
        F17,
        F18,
        F19,
        F20,
        F21,
        F22,
        F23,
        F24,
        F25,
        F26,
        F27,
        F28,
        F29,
        F30,
        F31,
        F32,
        F33,
        F34,
        F35,
        F36,
        F37,
        F38,
        F39,
        F40,
        F41,
        F42,
    }
    #endregion

    #region Excel Col
    public enum CEID_Col
    {
        CEID_Name = 1,
        CEID,
        CEID_Enable,
        Link_RPTIDs,
    }

    public enum RPTID_Col
    {
        RPTID_Name = 1,
        RPTID,
        Link_VIDs,
    }

    public enum VID_Col
    {
        VID_Name = 1,
        VID,
        VID_DataType,
        VID_Units,
        VID_Value,
    }
    #endregion

    public enum LoadUnloadStatus
    {
        Unknown,
        Load,
        Unload,
    }

    #endregion

    #region Ini

    public enum UpdateItem
    {
        Presence,
        Barcode,
        Data,
    }

    public enum DeviceWaferStatus
    {
        Robot1_Upper,
        Robot1_Lower,
        Robot2_Upper,
        Robot2_Lower,
        Stage1_Wafer,
        Stage1_Carrier,
        Stage1_CarrierWafer,
        Stage2_Wafer,
        Stage2_Carrier,
        Stage2_CarrierWafer,
        Aligner,
        Maxcnt
    }

    #endregion

    #region PLC

    public enum UIChange
    {
        PLC_RTStatus,
        PLC_CmdStatus,
        OCRStatus,
        Null
    }

    public enum Sk_Connstr
    {
        Connect,
        DisConnect,
        Unknow
    }

    public enum Sk_Device
    {
        PLC_RT,
        PLC_Cmd,
        OCR,
        CCDAL_RT,
        CCDAL_Cmd,
        Fn
    }

    public enum PLC_Device
    {
        B,
        W,
        LR,
        DM,
        Fn
    }

    public enum PLC_RTDevice
    {
        B1,
        W1,
        W2,
        LR1,
        Fn
    }

    public enum All_Device
    {
        CP1 = 1,
        CP2,
        CP3,
        CP4,
        CP5,
        CP6,
        CP7,
        CP8,
        CP9,
        CP10,
        MP1,
        MP2,
        MP3,
        MP4,
        MP5,
        MP6,
        MP7,
        MP8,
        Stage1,
        Stage2,
        PLCInterface,
        PLCInitial,
        PLCAlarmReset,
        EFEM,
        Robot1,
        Robot2,
        Aligner1,
        Fn
    }

    public enum PLC_B
    {
        InterfaceRequest = 0x00,
        CSTCommandStart = 0x10,
        MagCommaneStart = 0x1A,
        Stage1Command = 0x24,
        Stage2Command,
        InitialCommand,
        AlarmResetCommand,
        PC_StageProtrude_Reply = 0x50,
        PC_Stage2Protrude_Reply = 0x51,
        D900OnOff = 0x70,
        PC_StageProtrudeSetting = 0x80,
        PC_Stage2ProtrudeSetting = 0x81,
        CSTInvasionStart = 0x90,
        MagInvasionStart = 0x9A,
        Stage1WaferInvasion = 0xA4,
        Stage1CarrierInvasion,
        Stage2WaferInvasion,
        Stage2CarrierInvasion,
        CSTProcess = 0xB0,  //Joanne 20201009 Add
        MagProcess = 0xBA,  //Joanne 20201009 Add
        Stage_Busy = 0x800,   //Wayne 20210624 Stage busy status change to bit
        PLC_StageProtrude_Request = 0x1050,
        PLC_Stage2Protrude_Request = 0x1051,
        InterfaceRqply = 0x1000,
        CSTReplyStart = 0x1010,
        MagReplyStart = 0x101A,
        Stage1Reply = 0x1024,
        Stage2Reply,
        InitialReply,
        AlarmResetReply,
        AlarmCodeStart = 0x1100,
        MaxCnt = 0x270F,
        Is_Remote = 0x3000,
    }

    public enum PLC_W
    {
        CstCommandNoStart = 0x00,
        CstCommandTypeStart = 0x01,
        MagCommandNoStart = 0x14,
        MagCommandTypeStart = 0x15,
        Stage1CommandNo = 0x30,
        Stage1CommandType = 0x31,
        Stage2CommandNo = 0x34,
        Stage2CommandType = 0x35,
        InitialCommandUnit = 0x40,
        AlarmReset = 0x48,
        AlignerLiftPin = 0x51,
        StageRetry = 0x52,
        StageRetryDelayTime = 0x56,
        CstCommandResultStart = 0x1000,
        CstMappSlot = 0x1001,
        MapCommandResultStart = 0x1014,
        MapMappSlot = 0x1015,
        Stage1CommandResult = 0x1030,
        Stage1MappSlot,
        Stage2CommandResult = 0x1034,
        Stage2MappSlot = 0x1035,
        InitialCommandResult = 0x1040,
        AlarmRestResult = 0x1048,
        Version = 0x1060, // 20201211 Walson
        FFUAlarm = 0x1070,
        PSW,
        MainStatus = 0x1080,
        AlignerStatus = 0x1081,
        CstStatusStart = 0x1082,
        MagStatusStrat = 0x108C,
        StatusControl = 0x109F,
        Stage1Status = 0x10A0,
        BufferStart = 0x10A2,
        CstRFIDStart = 0x10B0,
        MagRFIDStart = 0x1178,
        SatgeProtrudeAVGValue = 0x15B0,
        SatgeProtrudeSensorValue = 0x15C0,
        SatgeProtrudeCompareValue = 0x15C8,
        SatgeProtrudeGoldenValue = 0x15E0,
        SatgeProtrudeTValue = 0x15F0,
        SatgeProtrudeGValue2 = 0x1600,
        Satge2ProtrudeAVGValue = 0x16B0,
        Satge2ProtrudeSensorValue = 0x16C0,
        Satge2ProtrudeCompareValue = 0x16C8,
        Satge2ProtrudeGoldenValue = 0x16E0,
        Satge2ProtrudeGoldenValue1 = 0x1700,
        Satge2ProtrudeTValue = 0x16F0,
    }

    public enum StatusControl
    {
        Alloff,
        BuzzerOff,
        Idle,
        Run,
        Alarm,
        Max
    }

    public enum InitialStep
    {
        CheckAlarm,
        AlarmSetCommand,
        AlarmRequestOn,
        AlarmReplyOn,
        AlarmReplyOff,
        AlarmRequestOff,
        ClearInvasion,
        ClearRequest,
        ClearCommand,
        InitialRequestOn,
        WaitReplyOn,
        InitialRequestOff,
        WaitReplyOff,
        WaitBusyOff,
        Max
    }

    #endregion

    //Joanne 20211019 追加 Pass Wafer 功能
    public enum emPortTransferStatus
    {
        OutOfService,
        TransferBlocked,
        ReadyToLoad,
        ReadyToUnload,
    }

    //Joanne 20211019 追加 Pass Wafer 功能
    public enum emPortState
    {
        NoState,
        LoadCompleted,
        UnloadCompleted,
        CID_Waiting_Verify,
        CID_Verify_OK,
        CID_Verify_Fail,
        SlotMap_Waiting_Verify,
        SlotMap_Verify_OK,
        SlotMap_Verify_Fail,
    }
}
