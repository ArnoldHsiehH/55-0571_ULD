using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace HirataMainControl
{
    public partial class LogLib : UserControl
    {
        #region Variable

        public string LogPath = "";
        //public string[] Page;

        private RichTextBox[] LogPage;
        private TabPage[] Tabs;
        private int[] LogCount;
        private int LogLineLimit = 200;  //Log limit
        private int LogFolderLimit = 30; //30_day
        private string LogFolder;
     
        private string Date = "";
        private string Time = "";

        #endregion

        #region Initial

        public LogLib()
        {
            InitializeComponent();
        } 

        public void Initial()
        {
           // Page = _Page;
            Tabs = new TabPage[(int)LogDir.MaxCnt];
            LogPage = new RichTextBox[(int)LogDir.MaxCnt];
            LogCount = new int[(int)LogDir.MaxCnt];
            LogFolderLimit = Convert.ToInt32(AppSetting.LoadSetting("LogFolderLimit", "30"));
            LogLineLimit = Convert.ToInt32(AppSetting.LoadSetting("LogLineLimit", "200"));
            LogPath = string.Format("{0}{1}{2}{3}",  AppSetting.LoadSetting("LogPath", "D:\\HirataMain_Log\\"), NormalStatic.DeviceType, NormalStatic.UnderLine, NormalStatic.Log); // 20201211 Walson
            for (int i = 0; i < (int)LogDir.MaxCnt; i++)
            {
                LogCount[i] = 0;
                Tabs[i] = new TabPage();
                LogPage[i] = new RichTextBox();

                Tabs[i].BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
                Tabs[i].Dock = System.Windows.Forms.DockStyle.Fill;
                Tabs[i].Location = new System.Drawing.Point(4, 29);
                Tabs[i].Padding = new System.Windows.Forms.Padding(3);
                Tabs[i].Text = (LogDir.System+i).ToString();
                //Tabs[i].Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                tbctLog.Controls.Add(Tabs[i]);

                LogPage[i].BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
                LogPage[i].Dock = System.Windows.Forms.DockStyle.Fill;
                LogPage[i].Location = new System.Drawing.Point(3, 3);
                LogPage[i].ReadOnly = true;
                LogPage[i].Text = "";
                Tabs[i].Controls.Add(LogPage[i]);
                CheckFolder(ref i);
            }

        } 

        #endregion

        #region Write

        public void Write(string _LogType, string _Time, string _SubTitle, string _Description)
        {
            ////frmMain.CalTimeStart();
            if (_LogType == NormalStatic.Robot)
                _LogType = _SubTitle;
            LogDir i = (LogDir)Enum.Parse(typeof(LogDir), _LogType);
            WriteLogToUi(LogPage[(int)i], _Time, _SubTitle, _Description, ref LogCount[(int)i]);
            WriteLogToFile(_LogType, string.Format("{0}{1}{2}{3}{4}", _Time, NormalStatic.LeftSquare, _SubTitle, NormalStatic.RightSquare, _Description));
            ////frmMain.CalTimeEnd();
        }

        #endregion

        #region WriteForm

        public void WriteLogToUi(RichTextBox _UI, string _DateTime, string _SubTitle, string _Description, ref int _Count)
        {
            if (_Count >= LogLineLimit)
            {
                _UI.Clear();
                _Count = 0;
            }
            if (_Count > 0)
            {
                _UI.AppendText("\r\n");
            }

            _UI.SelectionColor = Color.DarkGreen;
            _UI.AppendText(_DateTime);
            _UI.SelectionColor = Color.DarkBlue;
            _UI.AppendText(string.Format("{0}{1}{2}",NormalStatic.LeftSquare, _SubTitle, NormalStatic.RightSquare));
            _UI.SelectionColor = Color.Black;
            _UI.AppendText(string.Format("{0}{1}",":",_Description));
            _UI.ScrollToCaret();
           
            _Count++;
        }

        public void WriteLogToWarningUi(RichTextBox _UI ,string _DateTime, string _SubTitle, string _Description, ref int _Count)
        {
            if (_Count >= LogLineLimit)
            {
                _UI.Clear();
                _Count = 0;
            }
            if (_Count > 0)
            {
                _UI.AppendText("\r\n");
            }

            _UI.SelectionColor = Color.DarkGreen;
            _UI.AppendText(_DateTime);
            _UI.SelectionColor = Color.DarkBlue;
            _UI.AppendText(string.Format("{0}{1}{2}", NormalStatic.LeftSquare, _SubTitle, NormalStatic.RightSquare));
            _UI.SelectionColor = Color.Red;
            _UI.AppendText(string.Format("{0}{1}", ":", _Description));
            _UI.ScrollToCaret();

            _Count++;
        }

        #endregion

        #region WriteFile

        private void WriteLogToFile(string _LogFile, string _LogMessage)
        {
            int i = (int)(LogDir)Enum.Parse(typeof(LogDir), _LogFile);
            CheckFolder(ref i);

            using (  StreamWriter Stream = new StreamWriter(Path.Combine(LogFolder, string.Format("{0}{1}{2}{3}", _LogFile,NormalStatic.UnderLine,Time,".txt")), true, Encoding.Default))
            {
                Stream.WriteLine(_LogMessage);
                Stream.Flush();
                Stream.Dispose();
                Stream.Close();
            }        
        } 

        #endregion

        #region CheckFolder

        private void CheckFolder(ref int Index)
        {
            DateTime dt = DateTime.Now;
            Date = dt.ToString(NormalStatic.FileFormat);
            Time = string.Format("{0:HH00}", dt);

            LogFolder = string.Format("{0}{1}{2}{3}{4}", LogPath, "\\", LogDir.System+Index, "\\", Date); ;

            if (!Directory.Exists(LogFolder))
            {
                Directory.CreateDirectory(LogFolder);
            }
            //delete folder < 30
            string[] FolderList = Directory.GetDirectories(string.Format("{0}{1}{2}", LogPath, "\\", LogDir.System+Index));

            if (FolderList.Length > LogFolderLimit)
            {
                for (int i = 0; i < FolderList.Length - LogFolderLimit; i++)
                {
                    Directory.Delete(FolderList[i], true);
                }
            }
        } 

        #endregion
    }
}
