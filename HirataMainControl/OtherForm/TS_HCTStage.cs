using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HirataMainControl
{
    public partial class TS_HCTStage : Form
    {
        #region Initial

        public TS_HCTStage()
        {
            InitializeComponent();
        }

        public void Initial()
        {
            #region Cmd

            cboCommand.Items.Add(SocketCommand.Home);
            cboCommand.Items.Add(SocketCommand.ResetError);

            for (int i = (int)SocketCommand.Stage_Start + 1; i < (int)SocketCommand.Stage_End; i++)
            {
                cboCommand.Items.Add((SocketCommand)i);
            }

            #endregion

            #region Cnt

            for (int i = 1; i < HCT_EFEM.StageCount + 1; i++)
            {
                cboCnt.Items.Add(i);
            }
            #endregion

            cboCnt.SelectedIndex = 0;
        }

        #endregion

        #region Command

        private void cboCommand_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSend.Enabled = false;
    		cboCnt.Enabled = true;
			
            if (cboCommand.Text == "")
                return;

            switch ((SocketCommand)Enum.Parse(typeof(SocketCommand), cboCommand.Text))
            {
                case SocketCommand.Home:
                case SocketCommand.ResetError:
                case SocketCommand.Clamp:
                case SocketCommand.Mix:
                case SocketCommand.Separation:
                case SocketCommand.Get_Carrier_Wafer:
                case SocketCommand.Put_Carrier_Wafer:
                case SocketCommand.Get_Wafer_Wafer:
                case SocketCommand.Put_Wafer_Wafer:
                    {
                        btnSend.Enabled = true;
                    }
                    break;
            }
        }

        #endregion

        #region Close

        private void TS_HCTStage_FormClosed(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                cboCommand.SelectedIndex = -1;

                e.Cancel = true;
                Hide();
            }
        }

        #endregion
    }
}
