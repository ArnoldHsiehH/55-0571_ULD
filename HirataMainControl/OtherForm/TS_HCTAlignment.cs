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
    public partial class TS_HCTAlignment : Form
    {

        #region Initial

        public TS_HCTAlignment()
        {
            InitializeComponent();
        }

        public void Initial()
        {
            #region Cmd

            for (int i = 0; i < (int)AlignmentCommand.MaxCnt; i++)
            {
                cboCommand.Items.Add(AlignmentCommand.Alignment_ResetError + i);
            }

            #endregion

            #region Cnt

            for (int i = 1; i < HCT_EFEM.AlignerCount + 1; i++)
                cboCnt.Items.Add(i);

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

            switch ((AlignmentCommand)Enum.Parse(typeof(AlignmentCommand), cboCommand.Text))
            {

                case AlignmentCommand.Alignment_ResetError:
                case AlignmentCommand.Alignment_Home:
                case AlignmentCommand.SmallCassetteAlignment:
                case AlignmentCommand.SmallCassetteClamp:
                case AlignmentCommand.SmallCassetteUnClamp:
                case AlignmentCommand.BigCassetteAlignment:
                case AlignmentCommand.BigCassetteClamp:
                case AlignmentCommand.BigCassetteUnClamp:
                    {
                        btnSend.Enabled = true;
                    }
                    break;
            }
        } 

        #endregion

        #region Close

        private void TS_TexegAligner_FormClosing(object sender, FormClosingEventArgs e)
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
