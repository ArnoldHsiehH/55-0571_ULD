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
    public partial class TS_HCTD900 : Form
    {

        #region Initial

        public TS_HCTD900()
        {
            InitializeComponent();
        }

        public void Initial()
        {
            #region Cmd

            for (int i = (int)SocketCommand.OCR_Start + 1; i < (int)SocketCommand.OCR_End; i++)
            {
                cboCommand.Items.Add((SocketCommand)i);
            }

            #endregion

            #region Cnt

            for (int i = 1; i < HCT_EFEM.D900Count + 1; i++)
                cboCnt.Items.Add(i);

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
                case SocketCommand.Connect:
                case SocketCommand.Read:
                    {
                        btnSend.Enabled = true;
                    }
                    break;
            }
        }

        #endregion

        #region Close

        private void TS_D900_FormClosing(object sender, FormClosingEventArgs e)
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
