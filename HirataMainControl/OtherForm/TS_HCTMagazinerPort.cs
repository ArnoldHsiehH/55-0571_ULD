using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HirataMainControl
{
    public partial class TS_HCTMagazinerPort : Form
    {
        #region Initial

        public TS_HCTMagazinerPort()
        {
            InitializeComponent();
        }

        public void Initial()
        {
            #region Cmd

            cboCommand.Items.Add(SocketCommand.Home);

            cboCommand.Items.Add(SocketCommand.ResetError);

            for (int i = (int)SocketCommand.LP_Start + 1; i < (int)SocketCommand.LP_End; i++)
            {
                cboCommand.Items.Add((SocketCommand)i);
            }

            #endregion

            #region Cnt

            for (int i = 1; i < HCT_EFEM.MagazineCount + 1; i++)
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
            cboCnt.Enabled = true;
            btnSend.Enabled = false;

            if (cboCommand.Text == "")
                return;

            switch ((SocketCommand)Enum.Parse(typeof(SocketCommand), cboCommand.Text))
            {
                case SocketCommand.Home:
                case SocketCommand.ResetError:
                case SocketCommand.Load:
                case SocketCommand.Unload:
                case SocketCommand.OutDoorOpen:
                case SocketCommand.OutDoorClose:
                case SocketCommand.Reloading:
                    {
                        btnSend.Enabled = true;
                    }
                    break;
            }
        } 

        #endregion

        #region Close

        private void TS_HCTMagazinerPort_FormClosing(object sender, FormClosingEventArgs e)
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
