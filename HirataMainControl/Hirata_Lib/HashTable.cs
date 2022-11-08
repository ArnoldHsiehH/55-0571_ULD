using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace HirataMainControl
{
    public class HashTable
    {

        public Hashtable Aligner_Codes = new Hashtable(); //ErrorCode
        public Hashtable Aligner_InterLock = new Hashtable(); //InterLock
        public Hashtable Aligner_Error = new Hashtable(); //ABS ErrorCode
        public Hashtable Aligner_Getstatus = new Hashtable(); //Getstatus
        public Hashtable Aligner_GetType = new Hashtable(); //GetType

        public Hashtable Robot_Error = new Hashtable();

        public Hashtable LP_Interlock = new Hashtable();
        public Hashtable LP_Error = new Hashtable();
        public Hashtable LP_Event = new Hashtable();


        public Hashtable RFID_Error = new Hashtable();
        //public Hashtable E84_Event = new Hashtable();
        //public Hashtable E84_ErrorEvent = new Hashtable();
        //public Hashtable E84_Timeout = new Hashtable();


        public HashTable()
        {
            if(HCT_EFEM.RobotCount > 0)
                RobotHash();
            if(HCT_EFEM.AlignerCount > 0)
                AlignerHash();
        }

        private void RobotHash()
        {

            #region Error

            Robot_Error.Clear();
            Robot_Error.Add("09", ErrorList.RB_Alarm_0509);
            Robot_Error.Add("10", ErrorList.RB_Alarm_0510);
            Robot_Error.Add("20", ErrorList.RB_Alarm_0520);
            Robot_Error.Add("30", ErrorList.RB_Alarm_0530);
            Robot_Error.Add("31", ErrorList.RB_Alarm_0531);
            Robot_Error.Add("32", ErrorList.RB_Alarm_0532);
            Robot_Error.Add("40", ErrorList.RB_Alarm_0540);
            Robot_Error.Add("51", ErrorList.RB_Alarm_0551);
            Robot_Error.Add("61", ErrorList.RB_Alarm_0561);
            Robot_Error.Add("62", ErrorList.RB_Alarm_0562);
            Robot_Error.Add("63", ErrorList.RB_Alarm_0563);
            Robot_Error.Add("64", ErrorList.RB_Alarm_0564);
            Robot_Error.Add("67", ErrorList.RB_Alarm_0567);
            Robot_Error.Add("70", ErrorList.RB_Alarm_0570);
            Robot_Error.Add("80", ErrorList.RB_Alarm_0580);
            Robot_Error.Add("82", ErrorList.RB_Alarm_0582);
            Robot_Error.Add("84", ErrorList.RB_Alarm_0584);
            Robot_Error.Add("90", ErrorList.RB_Alarm_0590);
            Robot_Error.Add("95", ErrorList.RB_Alarm_0595);
            Robot_Error.Add("A0", ErrorList.RB_Alarm_05A0);
            Robot_Error.Add("B0", ErrorList.RB_Alarm_05B0);
            Robot_Error.Add("C0", ErrorList.RB_Alarm_05C0);
            Robot_Error.Add("D0", ErrorList.RB_Alarm_05D0);
            Robot_Error.Add("D1", ErrorList.RB_Alarm_05D1);
            Robot_Error.Add("D3", ErrorList.RB_Alarm_05D3);
            Robot_Error.Add("D5", ErrorList.RB_Alarm_05D5);
            Robot_Error.Add("D7", ErrorList.RB_Alarm_05D7);
            Robot_Error.Add("D8", ErrorList.RB_Alarm_05D8);
            Robot_Error.Add("D9", ErrorList.RB_Alarm_05D9);
            Robot_Error.Add("DA", ErrorList.RB_Alarm_05DA);
            Robot_Error.Add("DB", ErrorList.RB_Alarm_05DB);
            Robot_Error.Add("DC", ErrorList.RB_Alarm_05DC);
            Robot_Error.Add("E0", ErrorList.RB_Alarm_05E0); 

            #endregion
        }

        //private void LoadPortHash()
        //{
        //    #region Error
        //    LP_Error.Clear();
        //    LP_Error.Add("05", ErrorList.LP_Alarm_0505);
        //    LP_Error.Add("10", ErrorList.LP_Alarm_0510);
        //    LP_Error.Add("11", ErrorList.LP_Alarm_0511);
        //    LP_Error.Add("12", ErrorList.LP_Alarm_0512);
        //    LP_Error.Add("13", ErrorList.LP_Alarm_0513);
        //    LP_Error.Add("14", ErrorList.LP_Alarm_0514);
        //    LP_Error.Add("15", ErrorList.LP_Alarm_0515);
        //    LP_Error.Add("16", ErrorList.LP_Alarm_0516);
        //    LP_Error.Add("17", ErrorList.LP_Alarm_0517);
        //    LP_Error.Add("18", ErrorList.LP_Alarm_0518);
        //    LP_Error.Add("19", ErrorList.LP_Alarm_0519);
        //    LP_Error.Add("1A", ErrorList.LP_Alarm_051A);
        //    LP_Error.Add("1B", ErrorList.LP_Alarm_051B);
        //    LP_Error.Add("1F", ErrorList.LP_Alarm_051F);
        //    LP_Error.Add("20", ErrorList.LP_Alarm_0520);
        //    LP_Error.Add("21", ErrorList.LP_Alarm_0521);
        //    LP_Error.Add("22", ErrorList.LP_Alarm_0522);
        //    LP_Error.Add("23", ErrorList.LP_Alarm_0523);
        //    LP_Error.Add("28", ErrorList.LP_Alarm_0528);
        //    LP_Error.Add("29", ErrorList.LP_Alarm_0529);
        //    LP_Error.Add("2A", ErrorList.LP_Alarm_052A);
        //    LP_Error.Add("2B", ErrorList.LP_Alarm_052B);
        //    LP_Error.Add("40", ErrorList.LP_Alarm_0540);
        //    LP_Error.Add("41", ErrorList.LP_Alarm_0541);
        //    LP_Error.Add("70", ErrorList.LP_Alarm_0570);
        //    LP_Error.Add("71", ErrorList.LP_Alarm_0571);
        //    LP_Error.Add("72", ErrorList.LP_Alarm_0572);
        //    LP_Error.Add("73", ErrorList.LP_Alarm_0573);
        //    LP_Error.Add("74", ErrorList.LP_Alarm_0574);
        //    LP_Error.Add("77", ErrorList.LP_Alarm_0577);
        //    LP_Error.Add("A0", ErrorList.LP_Alarm_05A0);
        //    LP_Error.Add("A1", ErrorList.LP_Alarm_05A1);
        //    LP_Error.Add("A2", ErrorList.LP_Alarm_05A2);
        //    LP_Error.Add("A3", ErrorList.LP_Alarm_05A3);
        //    LP_Error.Add("A5", ErrorList.LP_Alarm_05A5);
        //    LP_Error.Add("B0", ErrorList.LP_Alarm_05B0);
        //    LP_Error.Add("C0", ErrorList.LP_Alarm_05C0);
        //    LP_Error.Add("E0", ErrorList.LP_Alarm_05E0);
        //    LP_Error.Add("E3", ErrorList.LP_Alarm_05E3);
        //    LP_Error.Add("FE", ErrorList.LP_Alarm_05FE); 

        //    #endregion

        //    #region Interlock

        //    LP_Interlock.Clear();
        //    LP_Interlock.Add("01", ErrorList.LP_Interlock_0401);
        //    LP_Interlock.Add("10", ErrorList.LP_Interlock_0410);
        //    LP_Interlock.Add("12", ErrorList.LP_Interlock_0412);
        //    LP_Interlock.Add("13", ErrorList.LP_Interlock_0413);
        //    LP_Interlock.Add("14", ErrorList.LP_Interlock_0414);
        //    LP_Interlock.Add("15", ErrorList.LP_Interlock_0415);
        //    LP_Interlock.Add("16", ErrorList.LP_Interlock_0416);
        //    LP_Interlock.Add("17", ErrorList.LP_Interlock_0417);
        //    LP_Interlock.Add("18", ErrorList.LP_Interlock_0418);
        //    LP_Interlock.Add("19", ErrorList.LP_Interlock_0419);
        //    LP_Interlock.Add("1A", ErrorList.LP_Interlock_041A);
        //    LP_Interlock.Add("1C", ErrorList.LP_Interlock_041C);
        //    LP_Interlock.Add("1D", ErrorList.LP_Interlock_041D);
        //    LP_Interlock.Add("1E", ErrorList.LP_Interlock_041E); 

        //    #endregion

        //    LP_Event.Clear();
        //    LP_Event.Add("PDON", "FoupPlace");
        //    LP_Event.Add("MNSW", "OperatorAccessButtonClick");
        //    LP_Event.Add("MESW", "OperatorAccessButton2Click");
        //    LP_Event.Add("PWON", "LPPowerOn");  
        //    LP_Event.Add("PDOF","FoupRemove");
        //    LP_Event.Add("AIRD", "AirDropped");
        //}

        private void AlignerHash()
        {
            Aligner_Codes.Clear();
            Aligner_Getstatus.Clear();
            Aligner_GetType.Clear();

            //ErrorCode
            Aligner_Codes.Add("01", ErrorList.AL_DeviceError_0101); 
            Aligner_Codes.Add("05", ErrorList.AL_Alarm_0505);
            //Aligner_Codes.Add("04", ErrorList.0404);
            Aligner_Codes.Add("06", ErrorList.LP_InProcess_0606);
            Aligner_Codes.Add("07", ErrorList.RB_ManualMode_0702);

            
            #region Interlock

            Aligner_InterLock.Clear();
            Aligner_InterLock.Add("10", ErrorList.AL_Interlock_0410);
            Aligner_InterLock.Add("13", ErrorList.AL_Interlock_0413);
            Aligner_InterLock.Add("15", ErrorList.AL_Interlock_0415);
            Aligner_InterLock.Add("16", ErrorList.AL_Interlock_0416);
            Aligner_InterLock.Add("18", ErrorList.AL_Interlock_0418);
            Aligner_InterLock.Add("21", ErrorList.AL_Interlock_0421); 

            #endregion

            //GetStutas
            Aligner_Getstatus.Add("A0", "Normal");
            Aligner_Getstatus.Add("A1", "Error");
            Aligner_Getstatus.Add("B0", "Auto");
            Aligner_Getstatus.Add("B1", "Manual"); 
            Aligner_Getstatus.Add("E00", "Normal From");
            Aligner_Getstatus.Add("E01", "Error code");
            Aligner_Getstatus.Add("F011", "Without wafer");
            Aligner_Getstatus.Add("F111", "With wafer");
            Aligner_Getstatus.Add("G0", "Not origin");
            Aligner_Getstatus.Add("G1", "Origin status"); 
            Aligner_Getstatus.Add("H0", "OFF");
            Aligner_Getstatus.Add("H1", "ON");
            Aligner_Getstatus.Add("J000", "000");
            Aligner_Getstatus.Add("J100", "100");
            Aligner_Getstatus.Add("J200", "200");
            Aligner_Getstatus.Add("J300", "300"); 
            //GetType
            Aligner_GetType.Add("8", "04");
            Aligner_GetType.Add("12", "05");
            Aligner_GetType.Add("4", "8");
            Aligner_GetType.Add("5", "12");

            #region Error

            Aligner_Error.Clear();
            Aligner_Error.Add("10", ErrorList.AL_Alarm_0510);
            Aligner_Error.Add("11", ErrorList.AL_Alarm_0511);
            Aligner_Error.Add("12", ErrorList.AL_Alarm_0512);
            Aligner_Error.Add("13", ErrorList.AL_Alarm_0513);
            Aligner_Error.Add("14", ErrorList.AL_Alarm_0514);
            Aligner_Error.Add("40", ErrorList.AL_Alarm_0540);
            Aligner_Error.Add("50", ErrorList.AL_Alarm_0550);
            Aligner_Error.Add("99", ErrorList.AL_Alarm_0599);
            Aligner_Error.Add("A0", ErrorList.AL_Alarm_05A0);
            Aligner_Error.Add("D0", ErrorList.AL_Alarm_05D0);
            Aligner_Error.Add("D1", ErrorList.AL_Alarm_05D1);
            Aligner_Error.Add("D3", ErrorList.AL_Alarm_05D3);
            Aligner_Error.Add("D5", ErrorList.AL_Alarm_05D5);
            Aligner_Error.Add("D7", ErrorList.AL_Alarm_05D7);
            Aligner_Error.Add("D8", ErrorList.AL_Alarm_05D8);
            Aligner_Error.Add("D9", ErrorList.AL_Alarm_05D9);
            Aligner_Error.Add("DA", ErrorList.AL_Alarm_05DA);
            Aligner_Error.Add("DB", ErrorList.AL_Alarm_05DB);
            Aligner_Error.Add("DC", ErrorList.AL_Alarm_05DC); 

            #endregion
        }

        //private void E84Hash()
        //{
        //    //42
        //    #region ErrorEvent

        //    E84_ErrorEvent.Clear();
        //    E84_ErrorEvent.Add("A0", ErrorList.E8_AlarmEvent_05A0);
        //    E84_ErrorEvent.Add("A1", ErrorList.E8_AlarmEvent_05A1);
        //    E84_ErrorEvent.Add("A2", ErrorList.E8_AlarmEvent_05A2);
        //    E84_ErrorEvent.Add("A3", ErrorList.E8_AlarmEvent_05A3);
        //    E84_ErrorEvent.Add("A4", ErrorList.E8_AlarmEvent_05A4);
        //    E84_ErrorEvent.Add("A5", ErrorList.E8_AlarmEvent_05A5);
        //    E84_ErrorEvent.Add("A6", ErrorList.E8_AlarmEvent_05A6);
        //    E84_ErrorEvent.Add("A7", ErrorList.E8_AlarmEvent_05A7);
        //    E84_ErrorEvent.Add("A8", ErrorList.E8_AlarmEvent_05A8);
        //    E84_ErrorEvent.Add("A9", ErrorList.E8_AlarmEvent_05A9);
        //    E84_ErrorEvent.Add("AF", ErrorList.E8_AlarmEvent_05AF);
        //    E84_ErrorEvent.Add("BO", ErrorList.E8_AlarmEvent_05B0);
        //    E84_ErrorEvent.Add("C0", ErrorList.E8_AlarmEvent_05C0);
        //    E84_ErrorEvent.Add("C1", ErrorList.E8_AlarmEvent_05C1);
        //    E84_ErrorEvent.Add("C2", ErrorList.E8_AlarmEvent_05C2);
        //    E84_ErrorEvent.Add("C3", ErrorList.E8_AlarmEvent_05C3);
        //    E84_ErrorEvent.Add("C4", ErrorList.E8_AlarmEvent_05C4);
        //    E84_ErrorEvent.Add("C5", ErrorList.E8_AlarmEvent_05C5);
        //    E84_ErrorEvent.Add("D0", ErrorList.E8_AlarmEvent_05DO);
        //    E84_ErrorEvent.Add("D1", ErrorList.E8_AlarmEvent_05D1);
        //    E84_ErrorEvent.Add("D2", ErrorList.E8_AlarmEvent_05D2);
        //    E84_ErrorEvent.Add("D3", ErrorList.E8_AlarmEvent_05D3);
        //    E84_ErrorEvent.Add("D4", ErrorList.E8_AlarmEvent_05D4);
        //    E84_ErrorEvent.Add("D5", ErrorList.E8_AlarmEvent_05D5);
        //    E84_ErrorEvent.Add("D6", ErrorList.E8_AlarmEvent_05D6);
        //    E84_ErrorEvent.Add("DC", ErrorList.E8_AlarmEvent_05DC);
        //    E84_ErrorEvent.Add("DD", ErrorList.E8_AlarmEvent_05DD);
        //    E84_ErrorEvent.Add("DE", ErrorList.E8_AlarmEvent_05DE);
        //    E84_ErrorEvent.Add("DF", ErrorList.E8_AlarmEvent_05DF);
        //    E84_ErrorEvent.Add("E0", ErrorList.E8_AlarmEvent_05E0);
        //    E84_ErrorEvent.Add("E1", ErrorList.E8_AlarmEvent_05E1);
        //    E84_ErrorEvent.Add("E2", ErrorList.E8_AlarmEvent_05E2);
        //    E84_ErrorEvent.Add("E3", ErrorList.E8_AlarmEvent_05E3);
        //    E84_ErrorEvent.Add("E4", ErrorList.E8_AlarmEvent_05E4);
        //    E84_ErrorEvent.Add("E5", ErrorList.E8_AlarmEvent_05E5);
        //    E84_ErrorEvent.Add("E6", ErrorList.E8_AlarmEvent_05E6);
        //    E84_ErrorEvent.Add("E7", ErrorList.E8_AlarmEvent_05E7);
        //    E84_ErrorEvent.Add("F4", ErrorList.E8_AlarmEvent_05F4);
        //    E84_ErrorEvent.Add("F7", ErrorList.E8_AlarmEvent_05F7);
        //    E84_ErrorEvent.Add("F8", ErrorList.E8_AlarmEvent_05F8);
        //    E84_ErrorEvent.Add("FB", ErrorList.E8_AlarmEvent_05FB);
        //    E84_ErrorEvent.Add("FC", ErrorList.E8_AlarmEvent_05FC); 

        //    #endregion

        //    //29
        //    #region Event

        //    E84_Event.Clear();
        //    E84_Event.Add("00", ErrorList.E8_Event_0000);
        //    E84_Event.Add("09", ErrorList.E8_Event_0009);
        //    E84_Event.Add("10", ErrorList.E8_Event_0010);
        //    E84_Event.Add("11", ErrorList.E8_Event_0011);
        //    E84_Event.Add("12", ErrorList.E8_Event_0012);
        //    E84_Event.Add("13", ErrorList.E8_Event_0013);
        //    E84_Event.Add("14", ErrorList.E8_Event_0014);
        //    E84_Event.Add("15", ErrorList.E8_Event_0015);
        //    E84_Event.Add("16", ErrorList.E8_Event_0016);
        //    E84_Event.Add("17", ErrorList.E8_Event_0017);
        //    E84_Event.Add("18", ErrorList.E8_Event_0018);
        //    E84_Event.Add("19", ErrorList.E8_Event_0019);
        //    E84_Event.Add("1A", ErrorList.E8_Event_001A);
        //    E84_Event.Add("1B", ErrorList.E8_Event_001B);
        //    E84_Event.Add("1C", ErrorList.E8_Event_001C);
        //    E84_Event.Add("1D", ErrorList.E8_Event_001D);
        //    E84_Event.Add("1E", ErrorList.E8_Event_001E);
        //    E84_Event.Add("1F", ErrorList.E8_Event_001F);
        //    E84_Event.Add("20", ErrorList.E8_Event_0020);
        //    E84_Event.Add("21", ErrorList.E8_Event_0021);
        //    E84_Event.Add("22", ErrorList.E8_Event_0022);
        //    E84_Event.Add("30", ErrorList.E8_Event_0030);
        //    E84_Event.Add("31", ErrorList.E8_Event_0031);
        //    E84_Event.Add("32", ErrorList.E8_Event_0032);
        //    E84_Event.Add("38", ErrorList.E8_Event_0038);
        //    E84_Event.Add("39", ErrorList.E8_Event_0039);
        //    E84_Event.Add("3A", ErrorList.E8_Event_003A);
        //    E84_Event.Add("72", ErrorList.E8_Event_0072);
        //    E84_Event.Add("73", ErrorList.E8_Event_0073);
 
        //    #endregion

        //    //10
        //    #region Timeout

        //    E84_Timeout.Clear();
        //    E84_Timeout.Add("80", ErrorList.E8_Timeout_1080);
        //    E84_Timeout.Add("81", ErrorList.E8_Timeout_1081);
        //    E84_Timeout.Add("82", ErrorList.E8_Timeout_1082);
        //    E84_Timeout.Add("83", ErrorList.E8_Timeout_1083);
        //    E84_Timeout.Add("84", ErrorList.E8_Timeout_1084);
        //    E84_Timeout.Add("86", ErrorList.E8_Timeout_1086);
        //    E84_Timeout.Add("88", ErrorList.E8_Timeout_1088);
        //    E84_Timeout.Add("8B", ErrorList.E8_Timeout_108B);
        //    E84_Timeout.Add("8C", ErrorList.E8_Timeout_108C); 
        //    #endregion
        //}

        //private void RFIDHash()
        //{
        //    #region Error

        //    RFID_Error.Clear();
        //    RFID_Error.Add("14", ErrorList.RF_FormatError_0114);
        //    RFID_Error.Add("70", ErrorList.RF_Communicatioins_0170);
        //    RFID_Error.Add("71", ErrorList.RF_VerificationError_0171);
        //    //RFID_Error.Add("72", ErrorList.);
        //    RFID_Error.Add("7B", ErrorList.RF_OutsideError_017B);
        //    RFID_Error.Add("7E", ErrorList.RF_IDError1_017E);
        //    RFID_Error.Add("7F", ErrorList.RF_IDError2_017F);

        //    #endregion
        //}
    }
}
