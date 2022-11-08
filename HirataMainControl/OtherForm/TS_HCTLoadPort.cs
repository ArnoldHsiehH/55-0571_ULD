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
    public partial class TS_HCTLoadPort : Form
    {
        #region Initial

        public TS_HCTLoadPort()
        {
            InitializeComponent();
        }

        public void Initial()
        {
            #region Cmd

            cboCommand.Items.Add(SocketCommand.Home);
            cboCommand.Items.Add(SocketCommand.GetStatus);
            cboCommand.Items.Add(SocketCommand.ResetError);

            for (int i = (int)SocketCommand.LP_Start + 1; i < (int)SocketCommand.LP_End; i++)
            {
                cboCommand.Items.Add((SocketCommand)i);
            }

            #endregion

            #region Cnt

            for (int i = 1; i < HCT_EFEM.LPCount + 1; i++)
                cboCnt.Items.Add(i);

            #endregion

            #region LED

            cboLED.Items.Add(NormalStatic.On);
            cboLED.Items.Add(NormalStatic.Off);
            cboLED.Items.Add(NormalStatic.Flash);

            #endregion

            #region Type

            for (int i = 0; i < 5; i++)
                cboType.Items.Add(i+1);

            #endregion

            #region Mapping

            //Thinkness
            cboThinkness.Items.Add(100);
            cboThinkness.Items.Add(750);
            cboThinkness.Items.Add(900);
            cboThinkness.Items.Add(3000);

            //Pitch
            cboPitch.Items.Add(6000);
            cboPitch.Items.Add(6250);
            cboPitch.Items.Add(10000);
            cboPitch.Items.Add(20000);

            //Slot
            cboSlot.Items.Add(5);
            cboSlot.Items.Add(13);
            cboSlot.Items.Add(25);
            cboSlot.Items.Add(30);

            //Offset
            cboDistance.Items.Add(-30000);
            cboDistance.Items.Add(0);
            cboDistance.Items.Add(30000);

            //Thick Tolerance
            cboThickTolerance.Items.Add(50);
            cboThickTolerance.Items.Add(500);
            cboThickTolerance.Items.Add(1000);

            //Position Tolerance
            cboPosTolerance.Items.Add(500);
            cboPosTolerance.Items.Add(1000);
            cboPosTolerance.Items.Add(2000);
            
            //12 inch, 8 inch Wafer
            cboSensorType.Items.Add(0);
            cboSensorType.Items.Add(1);
            #endregion

            cboCnt.SelectedIndex = 0;
        } 

        #endregion

        #region Command

        private void cboCommand_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboLED.SelectedIndex = -1;
            cboType.SelectedIndex = -1;

            cboThinkness.SelectedIndex = -1;
            cboPitch.SelectedIndex = -1;
            cboSlot.SelectedIndex = -1;
            cboDistance.SelectedIndex = -1;
            cboThickTolerance.SelectedIndex = -1;
            cboPosTolerance.SelectedIndex = -1;
            cboSensorType.SelectedIndex = -1;

            cboCnt.Enabled = true;

            cboLED.Enabled = false;
            cboType.Enabled = false;

            cboThinkness.Enabled = false;
            cboPitch.Enabled = false;
            cboSlot.Enabled = false;
            cboDistance.Enabled = false;
            cboThickTolerance.Enabled = false;
            cboPosTolerance.Enabled = false;
            cboSensorType.Enabled = false;

            btnSend.Enabled = false;

            if (cboCommand.Text == "")
                return;

            switch ((SocketCommand)Enum.Parse(typeof(SocketCommand), cboCommand.Text))
            {
                case SocketCommand.ResetError:
                case SocketCommand.Home:
                case SocketCommand.GetStatus:
                case SocketCommand.Clamp:
                case SocketCommand.UnClamp:
                case SocketCommand.Load:
                case SocketCommand.Unload:
                case SocketCommand.Map:
                case SocketCommand.GetWaferSlot:
                case SocketCommand.GetWaferSlot2:
                case SocketCommand.GetWaferThickness:
                case SocketCommand.GetWaferPosition:
                case SocketCommand.GetProtrusionSensor:
                case SocketCommand.GetLEDStatus:
                case SocketCommand.GetZAxisPos:
                    {
                        btnSend.Enabled = true;
                    }
                    break;

                case SocketCommand.LEDLoad:
                case SocketCommand.LEDUnLoad:
                case SocketCommand.LEDStatus1:
                case SocketCommand.LEDStatus2:
                case SocketCommand.SetOperatorAccessButton:
                    {   
                        cboLED.Enabled = true;
                    }
                    break;

               case SocketCommand.SetType:
               case SocketCommand.GetMapp:
                    {   
                        cboType.Enabled = true;
                    }
                    break;

              case SocketCommand.SetMapp:
                    {
                        cboType.Enabled = true;
                        cboThinkness.Enabled = true;
                        cboPitch.Enabled = true;
                        cboSlot.Enabled = true;
                        cboDistance.Enabled = true;
                        cboThickTolerance.Enabled = true;
                        cboPosTolerance.Enabled = true;
                        cboSensorType.Enabled = true;
                    }
                    break;
            }
        } 

        #endregion

        #region LED

        private void cboLED_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSend.Enabled = (cboLED.SelectedIndex >= 0);
        }

        #endregion

        #region Type

        private void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((SocketCommand)Enum.Parse(typeof(SocketCommand), cboCommand.Text) == SocketCommand.SetMapp)
                CheckMappingSelect();
            else
                btnSend.Enabled = (cboType.SelectedIndex >= 0);
        }

        #endregion

        #region Mapp

        private void CheckMappingSelect()
        {
            btnSend.Enabled = (cboType.SelectedIndex >= 0 &&
                    cboThinkness.SelectedIndex >= 0 &&
                    cboPitch.SelectedIndex >= 0 &&
                    cboSlot.SelectedIndex >= 0 &&
                    cboDistance.SelectedIndex >= 0 &&
                    cboThickTolerance.SelectedIndex >= 0 &&
                    cboPosTolerance.SelectedIndex >= 0 &&
                    cboSensorType.SelectedIndex >= 0);
        }

        private void cboThinkness_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckMappingSelect();
        }

        private void cboPitch_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckMappingSelect();
        }

        private void cboSlot_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckMappingSelect();
        }

        private void cboDistance_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckMappingSelect();
        }

        private void cboThickTolerance_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckMappingSelect();
        }

        private void cboPosTolerance_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckMappingSelect();
        }

        private void cboSensorType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckMappingSelect();
        }

        #endregion

        #region Close

        private void TS_HCTLoadPort_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                cboType.SelectedIndex = -1;
                cboThinkness.SelectedIndex = -1;
                cboPitch.SelectedIndex = -1;
                cboSlot.SelectedIndex = -1;
                cboDistance.SelectedIndex = -1;
                cboThickTolerance.SelectedIndex = -1;
                cboPosTolerance.SelectedIndex = -1;
                cboSensorType.SelectedIndex = -1;

                cboCommand.SelectedIndex = -1;
                e.Cancel = true;
                Hide();
            }
        }

        #endregion
    }
}
