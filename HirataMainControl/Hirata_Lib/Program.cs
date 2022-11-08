using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HirataMainControl
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //處理UI執行緒異常
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            //處理非執行緒異常
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            glExitApp = true;//標誌應用程式可以退出

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }

         //<summary>
         //是否退出應用程式
         //</summary>
        static bool glExitApp = false;

        /// <summary>
        /// 處理未捕獲異常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {

            UI.Error(NormalStatic.API, ErrorList.AP_TryCatchError, string.Format("CurrentDomain_UnhandledException:{0}", DateTime.Now.ToString(NormalStatic.TimeFormat)));
            UI.Error(NormalStatic.API, ErrorList.AP_TryCatchError,string.Format("IsTerminating : {0}",e.IsTerminating.ToString()));
            UI.Error(NormalStatic.API, ErrorList.AP_TryCatchError,e.ExceptionObject.ToString());

            while (true)
            {//迴圈處理，否則應用程式將會退出
                if (glExitApp)
                {//標誌應用程式可以退出，否則程式退出後，程序仍然在執行
                    UI.Error(NormalStatic.API, ErrorList.AP_TryCatchError, "ExitApp");

                    return;
                }
                System.Threading.Thread.Sleep(2 * 1000);
            };
        }

        /// <summary>
        /// 處理UI主執行緒異常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            UI.Error(NormalStatic.API, ErrorList.AP_TryCatchError,string.Format("Application_ThreadException:{0}",e.Exception.Message));
            UI.Error(NormalStatic.API, ErrorList.AP_TryCatchError,e.Exception.StackTrace);
        }
    }
}
