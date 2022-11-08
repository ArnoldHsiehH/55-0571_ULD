using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace HirataMainControl
{
    public class INI
    {
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string section, string key,string def,StringBuilder retval,int size,string filePath);
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern long WritePrivateProfileString(string section,string key, string val, string filePath);
        
        static object ReadLock = new object();
        static object WriteLock = new object();

        public static string ReadValue(string Section,string Key,string def,int size,string Path) 
        {
            lock (ReadLock)
            {
               // frmMain.CalTimeStart();
                StringBuilder temp=new StringBuilder(size);
                GetPrivateProfileString(Section, Key,def,temp,size,Path);
              //  frmMain.CalTimeEnd();
                return temp.ToString();
            }
        }
        public static void WriteValue(string Section, string Key, string Value, string Path)
        {
            lock (WriteLock)
            {
                //frmMain.CalTimeStart();
                WritePrivateProfileString(Section, Key, Value, Path);
                //frmMain.CalTimeEnd();
            }
        }
    }
}
