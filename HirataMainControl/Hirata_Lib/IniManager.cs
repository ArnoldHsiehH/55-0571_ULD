using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace HirataMainControl
{
    public class IniManager
    {
        #region 讀寫ini文件 API定義
        [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileStringA")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileStringA")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileSection")]
        private static extern int GetPrivateProfileSection(string section, byte[] buffer, int size, string filePath);
        #endregion

        private static string defaultPath = "..\\Config.ini";

        #region ==== Device Count ====
        public int Adam_DCnt;
        public int Adam_ACnt;
        public int RobotCnt;
        public int LoadPortCnt;
        public int RFIDCnt;
        public int AlignerCnt;
        public int AlignmentCnt;
        public int BarcodeReaderCnt;
        public int OCRCnt;
        public int IoCardCnt;
        public int EQStageCnt;
        public int E84Cnt;
        public int ShutterDoorCnt;
        #endregion

        #region ==== Ini Path ====
        static string RFIDini = @"C:\System\Config\Hirata_RFID.ini";
        static string Robotini = @"C:\System\Config\Hirata_Robot.ini";
        static string E84ini = @"C:\System\Config\Hirata_E84.ini";
        public static string LoadPortini = @"C:\System\Config\Hirata_LoadPort.ini";
        static string Adamini = @"C:\System\Config\Hirata_ADAM.ini";
        static string Aligner_ini = @"C:\System\Config\Hirata_Aligner.ini";
        static string BarCodeini = @"C:\System\Config\Hirata_BarCode.ini";
        static string OCRReaderini = @"C:\System\Config\Hirata_OCRReader.ini";
        static string Alignmentini = @"C:\System\Config\Hirata_Alignment.ini";
        static string EFEMini = @"C:\System\Config\Hirata_EFEM.ini";
        static string EQini = @"C:\System\Config\Hirata_EQ.ini";
        static string IOCardini = @"C:\System\Config\Hirata_IOCard.ini";
        static string Otherini = @"C:\System\Config\Hirata_Other.ini";
        #endregion

        public static string Count = "Count";
        public static string ADAM_D = "ADAM_D";
        public static string ADAM_A = "ADAM_A";
        public static string Robot = "Robot";
        public static string ShutterDoor = "ShutterDoor";
        public static string LoadPort = "Load_Port";
        public static string RFID = "RFID";
        public static string Aligner = "Aligner";
        public static string Alignment = "Alignment";
        public static string BarcodeReader = "Barcode_Reader";
        public static string OCR = "OCRReader";
        public static string IOCard = "IO_Card";
        public static string EQStage = "EQ_Stage";
        public static string E84 = "E84";

        public IniManager()
        {
            ReadConfig();
        }

        #region ==== Read ini ====
        private void ReadConfig()
        {
            Adam_DCnt = Convert.ToInt16(IniManager.IniReadValue("Count", ADAM_D, EFEMini, "0"));
            Adam_ACnt = Convert.ToInt16(IniManager.IniReadValue("Count", ADAM_A, EFEMini, "0"));
            RobotCnt = Convert.ToInt16(IniManager.IniReadValue("Count", Robot, EFEMini, "0"));
            LoadPortCnt = Convert.ToInt16(IniManager.IniReadValue("Count", LoadPort, EFEMini, "0"));
            RFIDCnt = Convert.ToInt16(IniManager.IniReadValue("Count", RFID, EFEMini, "0"));
            E84Cnt = Convert.ToInt16(IniManager.IniReadValue("Count", E84, EFEMini, "0"));
            AlignerCnt = Convert.ToInt16(IniManager.IniReadValue("Count", Aligner, EFEMini, "0"));
            AlignmentCnt = Convert.ToInt16(IniManager.IniReadValue("Count", Alignment, EFEMini, "0"));
            BarcodeReaderCnt = Convert.ToInt16(IniManager.IniReadValue("Count", BarcodeReader, EFEMini, "0"));
            OCRCnt = Convert.ToInt16(IniManager.IniReadValue("Count", OCR, EFEMini, "0"));
            IoCardCnt = Convert.ToInt16(IniManager.IniReadValue("Count", IOCard, EFEMini, "0"));
            EQStageCnt = Convert.ToInt16(IniManager.IniReadValue("Count", EQStage, EFEMini, "0"));
            ShutterDoorCnt = Convert.ToInt16(IniManager.IniReadValue("Count", ShutterDoor, EFEMini, "0"));
        }
        #endregion

        #region Write_ini
        public static bool IniWriteValue(string Section, string Key, string Value)
        {
            try
            {
                WritePrivateProfileString(Section, Key, Value, defaultPath);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool IniWriteValue(string Section, string Key, string Value, string IniPath)
        {
            try
            {
                WritePrivateProfileString(Section, Key, Value, IniPath);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        public static List<string> inidefaultlist = new List<string>();

        #region Read_ini
        public static string IniReadValue(string Section, string Key, string IniPath, string Default)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, IniPath);
            if (temp.ToString().Trim().Length == 0)
            {
                //Wayne 20181017 Example
                inidefaultlist.Add(string.Format("[Alarm]Ini Use Defaule . IniPath:{0},Section:{1},Key:{2} . Value={3}", IniPath, Section, Key, Default));
                return Default;
            }
            else
                return temp.ToString();
        }
        #endregion

        #region Read_Key
        public static string IniReadKeyValue(string Section, string Key, string IniPath, string Default)
        {
            StringBuilder temp = new StringBuilder(511);
            int i = GetPrivateProfileString(Section, Key, "", temp, 511, IniPath);
            if (temp.ToString().Trim().Length == 0)
                return Default;
            else
                return temp.ToString();
        }
        #endregion
    }
}
