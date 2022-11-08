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
    public partial class TS_HCTRFID : Form
    {
        #region Initial

        public TS_HCTRFID()
        {
            InitializeComponent();
        }

        public void Initial()
        {
            #region Cmd

            for (int i = (int)SocketCommand.RFID_Start + 1; i < (int)SocketCommand.RFID_End; i++)
            {
                cboCommand.Items.Add((SocketCommand)i);
            }

            #endregion

            #region Cnt

            for (int i = 1; i < HCT_EFEM.RFIDCount + 1; i++)
              cboCnt.Items.Add(i);

            #endregion

            clstPage.Enabled = false;

            cboCnt.SelectedIndex = 0;
            
        }

        #endregion

        #region Command

        private void cboCommand_SelectedIndexChanged(object sender, EventArgs e)
        {
            clstPage.Enabled = false;
            btnSend.Enabled = false;
            cboCnt.Enabled = true;

            if (cboCommand.Text == "")
                return;

            switch ((SocketCommand)Enum.Parse(typeof(SocketCommand), cboCommand.Text))
            {
                case SocketCommand.ReadFoupID:
                    {
                        btnSend.Enabled = true;
                    }
                    break;

                case SocketCommand.SetPageMap:
                    {
                        foreach (int index in clstPage.CheckedIndices)
                        {
                            clstPage.SetItemChecked(index, false);
                        }

                        clstPage.Enabled = true;
                    }
                    break;
            }
        }

        #endregion

        private void clstPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            int cnt = 0;
            for (int i = 0; i < clstPage.Items.Count; i++) 
            {
                if (clstPage.GetItemChecked(i))
                    cnt++;
            }

            if (cnt > 0 && cnt < 17)
                btnSend.Enabled = true;
            else
                btnSend.Enabled = false;
        }

        #region Close

        private void TS_HCTE84_FormClosed(object sender, FormClosingEventArgs e)
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
