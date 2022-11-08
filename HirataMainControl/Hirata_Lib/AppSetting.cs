using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace HirataMainControl
{
    class AppSetting
    {
        static Configuration configFile = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);

        static List<string> AppSettingDefaultList = new List<string>();

        #region Save
        public static void SaveSetting(string key, string value)
        {
            try
            {
                configFile.AppSettings.Settings[key].Value = value;
                configFile.Save();
            }
            catch
            {
                AddSetting(key, value);
            }
        } 
        #endregion

        #region Add
        public static void AddSetting(string key, string value)
        {
            try
            {
                configFile.AppSettings.Settings.Add(key, value);
                configFile.Save();
            }
            catch
            {
            }
        } 
        #endregion

        #region Delete
        public static void DeleteSetting(string key)
        {
            try
            {
                configFile.AppSettings.Settings.Remove(key);
                configFile.Save();
            }
            catch
            {
            }
        } 
        #endregion

        #region Load
        public static string LoadSetting(string key, string DefaultValue)
        {
            try
            {
                if (configFile.AppSettings.Settings[key].Value == "")
                {
                    AppSettingDefaultList.Add(string.Format(" Use Config Defaule Key:{0} \r\n ", key));
                    UI.Log(NormalStatic.IO, NormalStatic.Config, SystemList.ReadConfig, string.Format("{0}:({1})({2})", 0,key , DefaultValue));
                    return DefaultValue;
                }
                else
                {
                    UI.Log(NormalStatic.IO, NormalStatic.Config, SystemList.ReadConfig, string.Format("{0}:({1})({2})", 1,key,configFile.AppSettings.Settings[key].Value ));
                    return configFile.AppSettings.Settings[key].Value;
                }
            }
            catch
            {
                AppSettingDefaultList.Add(string.Format(" Use Config Defaule Key:{0} \r\n ", key));
                UI.Log(NormalStatic.IO, NormalStatic.Config, SystemList.ReadConfig, string.Format("{0}:({1})({2})", 0, key, DefaultValue));
               return DefaultValue;
            }
        }

        public static void CheckDefaultList()
        {
            if (AppSettingDefaultList.Count > 0)
            {
                UI.InitialSystem(NormalStatic.Config, NormalStatic.False, ErrorList.AP_IniError_0383);
            }
            else
            {
                UI.InitialSystem(NormalStatic.Config, NormalStatic.True, ErrorList.MaxCnt);

            }
        } 
        #endregion
    }
}
