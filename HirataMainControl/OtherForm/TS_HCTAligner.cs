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
    public partial class TS_HCTAligner : Form
    {
        #region Initial

        public TS_HCTAligner()
        {
            InitializeComponent();
        }

        public void Initial()
        {
            #region Cmd

            cboCommand.Items.Add(SocketCommand.Home);
            cboCommand.Items.Add(SocketCommand.GetStatus);
            cboCommand.Items.Add(SocketCommand.ResetError);

            for (int i = (int)SocketCommand.Aligner_Start + 1; i < (int)SocketCommand.Aligner_End; i++)
            {
                cboCommand.Items.Add((SocketCommand)i);
            }

            #endregion

            #region Cnt

            for (int i = 1; i < HCT_EFEM.AlignerCount + 1; i++)
                cboCnt.Items.Add(i);

            #endregion

            #region Degree

            txtDegree.Text = "";

            #endregion

            #region Type

            cboType.Items.Add("8");
            cboType.Items.Add("12");

            #endregion

            #region Vacuum

            cboOnOff.Items.Add(NormalStatic.On);
            cboOnOff.Items.Add(NormalStatic.Off);

            #endregion

            cboCnt.SelectedIndex = 0;
        } 

        #endregion

        #region Command

        private void cboCommand_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboType.SelectedIndex = -1;
            cboOnOff.SelectedIndex = -1;
            txtDegree.Text = "";

            cboCnt.Enabled = true;
            cboType.Enabled = false;
            txtDegree.Enabled = false;
            cboOnOff.Enabled = false;
            btnSend.Enabled = false;

            if (cboCommand.Text == "")
                return;

            switch ((SocketCommand)Enum.Parse(typeof(SocketCommand), cboCommand.Text))
            {
                case SocketCommand.ResetError:
                case SocketCommand.Home:
                case SocketCommand.GetStatus:
                case SocketCommand.GetAlignerDegree:
                case SocketCommand.GetIDReaderDegree:
                case SocketCommand.GetAlignerWaferType:
                case SocketCommand.LiftPinDown:
                case SocketCommand.LiftPinUp:
                case SocketCommand.FindNotch:
                case SocketCommand.ToAngle:
                case SocketCommand.AlignerVacuum_on:
                case SocketCommand.AlignerVacuum_off:
                    {
                        btnSend.Enabled = true;
                    }
                    break;

                case SocketCommand.Alignment:
                case SocketCommand.SetAlignerDegree:
                case SocketCommand.SetIDReaderDegree:
                    {
                        txtDegree.Enabled = true;
                    }
                    break;

                case SocketCommand.SetAlignerWaferType:
                    {
                        cboType.Enabled = true;
                    }
                    break;

                case SocketCommand.AlignerVacuum:
                    {
                        cboOnOff.Enabled = true;
                    }
                    break;
            }
        } 

        #endregion

        #region Degree

        private void txtDegree_KeyDown(object sender, KeyEventArgs e)
        {
            double degree;

            if (e.KeyCode == Keys.Enter)
            {
                if (double.TryParse(txtDegree.Text, out degree))
                {
                    degree = Math.Round(degree, 1, MidpointRounding.AwayFromZero);
                    if (degree >= 0 && degree <= 360)
                    {
                        btnSend.Enabled = true;
                    }
                }
            }
        }

        private void txtDegree_TextChanged(object sender, EventArgs e)
        {
            btnSend.Enabled = false;
        }

        #endregion

        #region Type

        private void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSend.Enabled = (cboType.SelectedIndex >= 0);
        }

        #endregion

        #region Vacuum

        private void cboOnOff_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSend.Enabled = (cboOnOff.SelectedIndex >= 0);
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
