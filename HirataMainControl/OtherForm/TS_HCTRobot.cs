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
    public partial class TS_HCTRobot : Form
    {
        #region Initial

        public TS_HCTRobot()
        {
            InitializeComponent();
        }

        public void Initial()
        {
            #region Cmd

            cboCommand.Items.Add(SocketCommand.Home);
            cboCommand.Items.Add(SocketCommand.GetStatus);
            cboCommand.Items.Add(SocketCommand.ResetError);

            for (int i = (int)SocketCommand.Robot_Start + 1; i < (int)SocketCommand.Robot_End; i++)
            {
                cboCommand.Items.Add((SocketCommand)i);
            }

            #endregion

            #region Cnt

            for (int i = 1; i <= HCT_EFEM.RobotCount; i++)
                cboCnt.Items.Add(i);

            #endregion

            #region Speed

            for (int i = 1; i <= 20; i++)
            {
                cboSpeed.Items.Add((i * 5).ToString());
            }

            #endregion

            #region Slot

            for (int i = 1; i < 26; i++)
                cboSlot.Items.Add(i.ToString());

            #endregion

            cboCnt.SelectedIndex = 0;
        } 

        #endregion

        #region Command

        private void cboCommand_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboDest.SelectedIndex = -1;
            cboSlot.SelectedIndex = -1;
            cboArm.SelectedIndex = -1;
            cboSpeed.SelectedIndex = -1;

            cboCnt.Enabled = true;
            cboDest.Enabled = false;
            cboSlot.Enabled = false;
            cboArm.Enabled = false;
            cboSpeed.Enabled = false;
            btnSend.Enabled = false;

            if (cboCommand.Text == "")
                return;

            switch ((SocketCommand)Enum.Parse(typeof(SocketCommand), cboCommand.Text))
            {
                case SocketCommand.Home:
                case SocketCommand.GetStatus:
                case SocketCommand.ResetError:

                case SocketCommand.ReStart:
                case SocketCommand.Stop:
                case SocketCommand.CheckWaferPresence:
                case SocketCommand.ReadPosition:
                case SocketCommand.CheckArmOnSafetyPos:
                case SocketCommand.ArmSafetyPosition:
                //case SocketCommand.SetRobotWaferInch:
                //case SocketCommand.GetRobotWaferInch:
                    {
                        btnSend.Enabled = true;
                    }
                    break;

                case SocketCommand.VacuumOn:
                case SocketCommand.VacuumOff:
                case SocketCommand.BernoulliOff:
                case SocketCommand.BernoulliOn:
                //case SocketCommand.EdgeGripOff:
                //case SocketCommand.EdgeGripOn:
                    {
                        cboArm.Items.Clear();
                        cboArm.Items.Add(Robot_Static.Lower);
                  //      cboArm.Items.Add(Robot_Static.Uppwer);
                        cboArm.SelectedIndex = 0;
                        cboArm.Enabled = true;
                        btnSend.Enabled = true;
                        cboCnt.SelectedIndex = 0;
                        cboCnt.Enabled = false;
                    }
                    break;

                case SocketCommand.SetRobotSpeed:
                    {
                        cboSpeed.Enabled = true;
                        cboSpeed.SelectedIndex = 0;
                    }
                    break;

                case SocketCommand.GetRobotMappingResult:
                case SocketCommand.GetRobotMappingResult2:
                case SocketCommand.GetRobotMappingErrorResult:
                case SocketCommand.GetRobotMappingErrorResult2:
                case SocketCommand.RobotMapping:
                    {
                        cboDest.Enabled = true;
                        DestChangeProgarm();
                    }
                    break;

                case SocketCommand.GetStandby:
                case SocketCommand.WaferGet:
                case SocketCommand.PutStandby:
                case SocketCommand.WaferPut:
                    {
                        cboArm.Items.Clear();
                        cboArm.Items.Add(Robot_Static.Lower);
                       // cboArm.Items.Add(Robot_Static.Uppwer);

                        cboArm.SelectedIndex = 0;
                        cboArm.Enabled = true;
                        cboDest.Enabled = true;
                        DestChangeProgarm();
                    }
                    break;

                case SocketCommand.TopGetStandby:
                case SocketCommand.TopWaferGet:
                case SocketCommand.TopWaferPut:
                case SocketCommand.TopPutStandby:
                    {
                        cboArm.Items.Clear();
                        cboArm.Items.Add(Robot_Static.Lower);
                        cboArm.SelectedIndex = 0;
                        cboArm.Enabled = false;
                        cboCnt.SelectedIndex = 0;
                        cboCnt.Enabled = false;
                        cboDest.Enabled = true;
                        DestChangeProgarm();
                    }
                    break;

            //    case SocketCommand.Move_OCRReadPosition:
                    //{
                    //    cboArm.Items.Clear();
                    //    cboArm.Items.Add(Robot_Static.Lower);
                    //   // cboArm.Items.Add(Robot_Static.Uppwer);
                    //    cboArm.SelectedIndex = 0;
                    //    cboArm.Enabled = true;
                    //    cboCnt.SelectedIndex = 0;
                    //    cboCnt.Enabled = false;
                    //    cboDest.Enabled = true;
                    //    DestChangeProgarm();
                    //}
                    //break;
            }
        } 

        #endregion

        #region Dest

        private void DestChangeProgarm()
        {
            cboDest.Items.Clear();
            btnSend.Enabled = false;

            if ((SocketCommand)Enum.Parse(typeof(SocketCommand), cboCommand.Text) == SocketCommand.RobotMapping
             || (SocketCommand)Enum.Parse(typeof(SocketCommand), cboCommand.Text) == SocketCommand.GetRobotMappingErrorResult
             || (SocketCommand)Enum.Parse(typeof(SocketCommand), cboCommand.Text) == SocketCommand.GetRobotMappingErrorResult2
             || (SocketCommand)Enum.Parse(typeof(SocketCommand), cboCommand.Text) == SocketCommand.GetRobotMappingResult
             || (SocketCommand)Enum.Parse(typeof(SocketCommand), cboCommand.Text) == SocketCommand.GetRobotMappingResult2
                )
            {
                if (cboCnt.Text == "1")
                {
                    for (int i = 1; i <= HCT_EFEM.CassetteCount; i++)
                    {
                        cboDest.Items.Add(string.Format("{0}{1}", NormalStatic.CstPort, i));
                    }
                }
                else
                {
                    for (int i = 1; i <= HCT_EFEM.MagazineCount; i++)
                    {
                        cboDest.Items.Add(string.Format("{0}{1}", NormalStatic.MagazinePort, i));
                    }
                }
            }
            else
            {
                switch (cboCnt.Text)
                {
                    case "1":
                        {

                            if ((SocketCommand)Enum.Parse(typeof(SocketCommand), cboCommand.Text) == SocketCommand.GetStandby
                             || (SocketCommand)Enum.Parse(typeof(SocketCommand), cboCommand.Text) == SocketCommand.WaferGet
                             || (SocketCommand)Enum.Parse(typeof(SocketCommand), cboCommand.Text) == SocketCommand.PutStandby
                             || (SocketCommand)Enum.Parse(typeof(SocketCommand), cboCommand.Text) == SocketCommand.WaferPut
                                )
                            {
                                for (int i = 1; i <= HCT_EFEM.LPCount; i++)
                                {
                                    cboDest.Items.Add(string.Format("{0}{1}", NormalStatic.LP, i));
                                }
                            }
   

                            for (int i = 1; i <= HCT_EFEM.AlignerCount; i++)
                            {
                                cboDest.Items.Add(string.Format("{0}{1}", NormalStatic.Aligner, i));
                            }
                            cboDest.Items.Add(string.Format("{0}{1}", NormalStatic.EQ, 1));
                        }
                        break;

                    case "2":
                        {
                            for (int i = 1; i <= HCT_EFEM.MagazineCount; i++)
                            {
                                cboDest.Items.Add(string.Format("{0}{1}", NormalStatic.MagazinePort, i));
                            }

                            for (int i = 1; i <= HCT_EFEM.StageCount; i++)
                            {
                                cboDest.Items.Add(string.Format("{0}{1}", NormalStatic.Stage, i));
                            }
                        }
                        break;
                }

            }
        }

        private void cboDest_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDest.SelectedIndex < 0)
                return;

            btnSend.Enabled = false;

            if ((SocketCommand)Enum.Parse(typeof(SocketCommand), cboCommand.Text) == SocketCommand.RobotMapping
            || (SocketCommand)Enum.Parse(typeof(SocketCommand), cboCommand.Text) == SocketCommand.GetRobotMappingErrorResult
            || (SocketCommand)Enum.Parse(typeof(SocketCommand), cboCommand.Text) == SocketCommand.GetRobotMappingErrorResult2
            || (SocketCommand)Enum.Parse(typeof(SocketCommand), cboCommand.Text) == SocketCommand.GetRobotMappingResult
            || (SocketCommand)Enum.Parse(typeof(SocketCommand), cboCommand.Text) == SocketCommand.GetRobotMappingResult2
                )
            {
                btnSend.Enabled = true;
                return;
            }

            switch (cboDest.Text)
            {
                case "Aligner1":
                case "Stage1":
                case "Stage2":
                case "EQ1":
                    {
                        if (cboSlot.SelectedIndex == 0)
                        {
                            btnSend.Enabled = true;
                        }
                        else
                        {
                            cboSlot.SelectedIndex = 0;
                        }

                        cboSlot.Enabled = false;
                    }
                    break;

                case "Stage3":
                    {
                        cboSlot.Items.Clear();
                        cboSlot.SelectedIndex = -1;
                        cboSlot.Enabled = true;

                        for (int i = 1; i <= 5; i++)
                            cboSlot.Items.Add(i.ToString());

                    }
                    break;

                case "LP1":
                case "LP2":

                    cboSlot.Items.Clear();

                    int PortSlot = Convert.ToInt32(cboDest.Text.Substring(2, 1)) - 1;

                    for(int i = 0; i < HT.LP_CurrentSlot[PortSlot]; i++) 
                    {
                        cboSlot.Items.Add(i + 1);
                    }
                    cboSlot.Enabled = true;

                    break;
                
                default:
                    {
                        cboSlot.Items.Clear();
                        cboSlot.SelectedIndex = -1;
                        cboSlot.Enabled = true;
                        int slotcount = 0;

                        slotcount = cboCnt.Text == "1" ? 25:12;

                        for (int i = 1; i <= slotcount; i++)
                            cboSlot.Items.Add(i.ToString());
                    }

                    break;
            }
            //RobotArm.Enabled = true;
        } 

        #endregion

        #region Slot

        private void cboSlot_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSend.Enabled = (cboSlot.SelectedIndex >= 0);
        } 

        #endregion

        #region Speed

        private void cboMSpeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSend.Enabled = (cboSpeed.SelectedIndex >= 0);
        } 
        
        #endregion

        #region Close

        private void TS_EFEMRobot_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                cboCommand.SelectedIndex = -1;

                e.Cancel = true;
                Hide();
            }
        }

        #endregion

        #region Cnt

        private void cboCnt_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cboCommand.Text == "")
                return;

            switch ((SocketCommand)Enum.Parse(typeof(SocketCommand), cboCommand.Text))
            {
                case SocketCommand.Home:
                case SocketCommand.GetStatus:
                case SocketCommand.ResetError:

                case SocketCommand.ReStart:
                case SocketCommand.Stop:
                case SocketCommand.CheckWaferPresence:
                case SocketCommand.ReadPosition:
                case SocketCommand.CheckArmOnSafetyPos:
                case SocketCommand.ArmSafetyPosition:
                case SocketCommand.VacuumOn:
                case SocketCommand.VacuumOff:
                case SocketCommand.BernoulliOff:
                case SocketCommand.BernoulliOn:
            //    case SocketCommand.EdgeGripOff:
            //    case SocketCommand.EdgeGripOn:
                case SocketCommand.SetRobotSpeed:
            //    case SocketCommand.Move_OCRReadPosition:

                    {
                        btnSend.Enabled = true;
                    }
                    break;

             
                case SocketCommand.RobotMapping:
                    {
                        btnSend.Enabled = false;
                        cboDest.Items.Clear();
                        cboSlot.Enabled = false;

                        if (cboCnt.Text == "1")
                        {
                            for (int i = 1; i <= HCT_EFEM.CassetteCount; i++)
                            {
                                cboDest.Items.Add(string.Format("{0}{1}", NormalStatic.CstPort, i));
                            }
                        }
                        else
                        {
                            for (int i = 1; i <= HCT_EFEM.MagazineCount; i++)
                            {
                                cboDest.Items.Add(string.Format("{0}{1}", NormalStatic.MagazinePort, i));
                            }
                        }
                    }
                    break;

                case SocketCommand.GetStandby:
                case SocketCommand.WaferGet:
                case SocketCommand.PutStandby:
                case SocketCommand.WaferPut:
                case SocketCommand.TopGetStandby:
                case SocketCommand.TopWaferGet:
                case SocketCommand.TopWaferPut:
                case SocketCommand.TopPutStandby:
                    {
                        btnSend.Enabled = false;
                        cboDest.Items.Clear();
                        //cboSlot.Items.Clear(); // Joanne 20191115
                        cboSlot.Enabled = false;
                        switch (cboCnt.Text)
                        {
                            case "1":
                                {
                                    for (int i = 1; i <= HCT_EFEM.CassetteCount; i++)
                                    {
                                        cboDest.Items.Add(string.Format("{0}{1}", NormalStatic.CstPort, i));
                                    }

                                    for (int i = 1; i <= HCT_EFEM.StageCount; i++)
                                    {
                                        cboDest.Items.Add(string.Format("{0}{1}", NormalStatic.Stage, i));
                                    }

                                    for (int i = 1; i <= HCT_EFEM.AlignerCount; i++)
                                    {
                                        cboDest.Items.Add(string.Format("{0}{1}", NormalStatic.Aligner, i));
                                    }
                                }
                                break;

                            case "2":
                                {
                                    for (int i = 1; i <= HCT_EFEM.MagazineCount; i++)
                                    {
                                        cboDest.Items.Add(string.Format("{0}{1}", NormalStatic.MagazinePort, i));
                                    }

                                    for (int i = 1; i <= HCT_EFEM.StageCount; i++)
                                    {
                                        cboDest.Items.Add(string.Format("{0}{1}", NormalStatic.Stage, i));
                                    }
                                }
                                break;
                        }
                    }
                    break;
            }
          
        } 

        #endregion
    }
}
