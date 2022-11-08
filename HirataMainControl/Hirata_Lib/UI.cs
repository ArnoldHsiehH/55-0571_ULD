using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Threading;

namespace HirataMainControl
{
    public class UI
    {
        #region Event/Delegate

        public delegate void InitialSystemEvent(string device, string status, string message);
        public delegate void LogEvent(string fileType, string datatime, string device, string description);
        public delegate void AlarmEvent(string alarmCode, string datatime, string device, string description);
        public delegate void CloseEvent(string closeMessage);
        public delegate void OperationEvent(string datatime, string device, string description);
        public delegate void OperationButtonEvent(string Item, string Status);
        public event InitialSystemEvent EventInitialSystem;
        public event LogEvent EventLog;
        public event AlarmEvent EventAlarm;
        public event CloseEvent EventClosing;
        public event OperationEvent EventOperation;
        public event OperationButtonEvent EventOperationButton;

        #endregion

        #region BG/Queue

        private BackgroundWorker UI_BG = new BackgroundWorker();
        private static BlockQueue<string> UIQueue = new BlockQueue<string>();

        #endregion

        #region Constructor

        public UI()
        {
           
        }

        public void init() 
        {
            UI_BG.DoWork += new DoWorkEventHandler(UI_DoWork);
            UI_BG.RunWorkerCompleted += new RunWorkerCompletedEventHandler(UI_RunWorkerCompleted);
            UI_BG.RunWorkerAsync();
        }

        #endregion

        #region BG

        private void UI_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = "MainForm End";
            string Job = "";
            string[] Receive;
            UI.InitialSystem(NormalStatic.System, NormalStatic.True, ErrorList.MaxCnt);
 
            while (true)
            {
                try
                {

                    Job = UIQueue.DeQueue();
                    if (Job == NormalStatic.End)
                        break;

                    Receive = Job.Split(new string[] { NormalStatic.LogSplot }, StringSplitOptions.None);

                    string JobType = Receive[0];

                    switch (JobType)
                    {
                        case NormalStatic.InitialSystem:
                            {
                                EventInitialSystem(Receive[1], Receive[2], Receive[3]);
                            }
                            break;

                        case NormalStatic.Log:
                            {
                                EventLog(Receive[1], Receive[2], Receive[3], Receive[4]);
                            }
                            break;

                        case NormalStatic.Alarm:
                            {
                                EventAlarm(Receive[1], Receive[2], Receive[3], Receive[4]);
                            }
                            break;
                        case NormalStatic.Close:
                            {
                                EventClosing(Receive[1]);
                            }
                            break;

                        case NormalStatic.Operation:
                            {
                                EventOperation(Receive[1], Receive[2], Receive[3]);
                            }
                            break;

                        case NormalStatic.AutoButton:
                            {
                                EventOperationButton(Receive[1], Receive[2]);
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    UI.Alarm(NormalStatic.Log, ErrorList.AP_TryCatchError, string.Format("{0},{1}", "LogBG", ex.ToString()));
                }
            }
        }

        private void UI_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            EventClosing(e.Result.ToString());
        }

        public void Close()
        {
            UIQueue.EnQueue(NormalStatic.End);
        }

        #endregion

        #region Method

        #region Initial

        public static void InitialSystem(string device, string status, ErrorList description1)
        {
            string errcode = string.Format("{0}{1}{2}{3}{4}{5}{6}", NormalStatic.InitialSystem,
                                                                    NormalStatic.LogSplot,
                                                                    device,
                                                                    NormalStatic.LogSplot,
                                                                    status,
                                                                    NormalStatic.LogSplot,
                                                                    description1);
            UIQueue.EnQueue(errcode);
        }

        #endregion

        #region System

        public static void Log(string filePage, string device, SystemList description1, string description2)
        {
            string errcode = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}", NormalStatic.Log,
                                                                         NormalStatic.LogSplot,
                                                                         filePage,
                                                                         NormalStatic.LogSplot,
                                                                         DateTime.Now.ToString(NormalStatic.TimeFormat),
                                                                         NormalStatic.LogSplot,
                                                                         device,
                                                                         NormalStatic.LogSplot,
                                                                         string.Format("{0}{1}",
                                                                         HCT_EFEM.ExcelLogMessage[(int)LogExcelTable.Log][0][(int)description1, 1], description2));
            UIQueue.EnQueue(errcode);

        }

        #endregion

        #region Error

        public static void Error(string device, ErrorList description1, string description2)
        {
            string errcode = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}", NormalStatic.Log,
                                                                         NormalStatic.LogSplot,
                                                                         NormalStatic.Error,
                                                                         NormalStatic.LogSplot,
                                                                         DateTime.Now.ToString(NormalStatic.TimeFormat),
                                                                         NormalStatic.LogSplot,
                                                                         device,
                                                                         NormalStatic.LogSplot,
                                                                         string.Format("{0}{1}", HCT_EFEM.ExcelLogMessage[(int)LogExcelTable.Error][0][(int)description1, 2], description2));

           UIQueue.EnQueue(errcode);
        }

        public static void Error(string device, ErrorList description1)
        {
            string errcode = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}", NormalStatic.Log,
                                                                         NormalStatic.LogSplot,
                                                                         NormalStatic.Error,
                                                                         NormalStatic.LogSplot,
                                                                         DateTime.Now.ToString(NormalStatic.TimeFormat),
                                                                         NormalStatic.LogSplot,
                                                                         device,
                                                                         NormalStatic.LogSplot,
                                                                         HCT_EFEM.ExcelLogMessage[(int)LogExcelTable.Error][0][(int)description1, 2]);
            UIQueue.EnQueue(errcode);
        }

        #endregion

        #region Alarm

        public static void Alarm(string device, ErrorList description1)
        {
            string errcode = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}", NormalStatic.Alarm,
                                                                         NormalStatic.LogSplot,
                                                                       string.Format("{0}{1}", HCT_EFEM.ExcelLogMessage[(int)LogExcelTable.Error][0][(int)description1, 0],
                                                                                         HCT_EFEM.ExcelLogMessage[(int)LogExcelTable.Error][0][(int)description1, 1]),
                                                                         NormalStatic.LogSplot,
                                                                         DateTime.Now.ToString(NormalStatic.TimeFormat),
                                                                         NormalStatic.LogSplot,
                                                                         device,
                                                                         NormalStatic.LogSplot,
                                                                         HCT_EFEM.ExcelLogMessage[(int)LogExcelTable.Error][0][(int)description1, 2]);
            UIQueue.EnQueue(errcode);

        }

        public static void Alarm(string device, ErrorList description1, string description2)
        {
            string errcode = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}", NormalStatic.Alarm,
                                                                 NormalStatic.LogSplot,
                                                                 string.Format("{0}{1}", HCT_EFEM.ExcelLogMessage[(int)LogExcelTable.Error][0][(int)description1, 0],
                                                                                         HCT_EFEM.ExcelLogMessage[(int)LogExcelTable.Error][0][(int)description1, 1]),
                                                                 NormalStatic.LogSplot,
                                                                 DateTime.Now.ToString(NormalStatic.TimeFormat),
                                                                 NormalStatic.LogSplot,
                                                                 device,
                                                                 NormalStatic.LogSplot,
                                                                 string.Format("{0}{1}", HCT_EFEM.ExcelLogMessage[(int)LogExcelTable.Error][0][(int)description1, 2], description2));
            UIQueue.EnQueue(errcode);

        }

        public static void Alarm(string device,string PLCBit,int i)
        {
            string errcode = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}", NormalStatic.Alarm,
                                                                 NormalStatic.LogSplot,
                                                                 PLCBit,
                                                                 NormalStatic.LogSplot,
                                                                 DateTime.Now.ToString(NormalStatic.TimeFormat),
                                                                 NormalStatic.LogSplot,
                                                                 device,
                                                                 NormalStatic.LogSplot,
                                                                 string.Format("{0}{1}", "", HCT_EFEM.ExcelPLC[(int)LogExcelTable.Log][0][i, 0]));
            UIQueue.EnQueue(errcode);

        }

        #endregion

        #region Close

        public static void CloseBG(string closeMessage)
        {
            UIQueue.EnQueue(string.Format("{0}{1}{2}", NormalStatic.Close, NormalStatic.LogSplot, closeMessage));
        }

        #endregion

        #region Button

        public static void Operate(string device, string description)
        {
            UIQueue.EnQueue(string.Format("{0}{1}{2}{3}{4}{5}{6}", NormalStatic.Operation,
                        NormalStatic.LogSplot,
                        DateTime.Now.ToString(NormalStatic.TimeFormat),
                        NormalStatic.LogSplot,
                        device,
                        NormalStatic.LogSplot,
                        description));
        }

        #endregion

        #region Operation

        public static void AutoButton(EFEMStatus item, string InitialStatus)
        {
            UIQueue.EnQueue(string.Format("{0}{1}{2}{3}{4}", NormalStatic.AutoButton, NormalStatic.LogSplot, item.ToString(), NormalStatic.LogSplot, InitialStatus));
        }

        #endregion

        #endregion
    }
}
