using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace HirataMainControl
{
    public partial class Form_Initial : Form
    {
        public delegate void InitialEvent();
        public event InitialEvent CloseEvent;

        public delegate void WaferCheckEvent();
        public event WaferCheckEvent WaferCheck; 

        private int Initial_Count = 1;
        private int Current_Initial = 0;

        public Form_Initial(int Ini_Count)
        {
            InitializeComponent();
            txtSuccess.Text = string.Empty;
            Initial_Count = Ini_Count;
        }

        public void SetMessage(string devicemame, string status, string msg)
        {
            if (this.Visible)
            {
                Current_Initial++;

                if (status == NormalStatic.True)
                {
                    UI.Log(NormalStatic.System, devicemame, SystemList.ConnectStatus, "Success");
                    txtSuccess.Text = string.Format("{0}{1}{2}{3}{4}", txtSuccess.Text, Current_Initial, "-----", devicemame, "\r\n");
                }
                else
                {
                    UI.Log(NormalStatic.System, devicemame, SystemList.ConnectStatus, msg);

                    txtFail.Text = string.Format("{0}{1}{2}{3}{4}{5}", txtFail.Text, Current_Initial, "-----", devicemame, msg, "\r\n");
                }

                int Progress = (Current_Initial * 100) / Initial_Count;

                if ((Initial_Count != 0) && (Progress < 100))
                    prgbInitial.Value = Progress;
                else
                    prgbInitial.Value = 100;

                txtSuccess.SelectionStart = txtSuccess.Text.Length;
                txtSuccess.ScrollToCaret();
                txtSuccess.Refresh();

                if (Current_Initial >= Initial_Count)
                {
                    btnComplete.Enabled = true;

                    if (txtFail.Text == "")
                    {
                        HT.EFEM.IsInitialSuccess = true;
                        if (Initial_Count == HT.EFEM.Again_Count)
                        {
                            this.Close();
                        }
                    }
                    else
                        HT.EFEM.IsInitialSuccess = false;

                    btnClose.Enabled = true;
                }

            }
        }

        private void InitialComplete_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            CloseEvent();
        }
    }
}
