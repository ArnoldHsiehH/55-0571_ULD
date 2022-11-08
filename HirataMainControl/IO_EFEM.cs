using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HirataMainControl
{
    public partial class IO_EFEM : UserControl
    {

        #region Event/delegate

        public delegate void DiChangeEvent(EFEM_DI DI, bool Result);
        //public delegate void SetResultEvnet(EFEM_DO DO, string Value);
        public event DiChangeEvent EvnetDIChange;
        //public event SetResultEvnet EvnetDOChange;

        #endregion

        #region Variable

        private DataGridView IO_DGV;
        public bool InitialEventFlag = false;
        

        #endregion

        #region Initial

        public IO_EFEM()
        {
            InitializeComponent();
        }

        public void Initial()
        {
            gbxTable.Text = NormalStatic.EFEM;

            #region UI

            IO_DGV = new DataGridView();
            IO_DGV.Dock = System.Windows.Forms.DockStyle.Fill;
            IO_DGV.AllowUserToResizeColumns = false;
            IO_DGV.AllowUserToResizeRows = false;
            IO_DGV.RowHeadersVisible = false;
            IO_DGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;


            IO_DGV.Columns.Add(Adam_Static.IOType, Adam_Static.IOType);
            IO_DGV.Columns.Add(Adam_Static.IOName, Adam_Static.IOName);
            IO_DGV.Columns.Add(Adam_Static.Channel, Adam_Static.Channel);
            IO_DGV.Columns.Add(Adam_Static.IOValue, Adam_Static.IOValue);

            for (int i = 0; i < 4; i++)
            {
                IO_DGV.Columns[i].ReadOnly = true;
                IO_DGV.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            gbxTable.Controls.Add(IO_DGV);

            int IndexOffset = 0;

      


            #endregion
            HT.EFEM.FFUAlarm = new bool[(int)EFEM_FFU.MaxCnt];
            for (int i = 0; i < (int)EFEM_FFU.MaxCnt; i++) 
            {
                HT.EFEM.FFUAlarm[i] = false;
            }

            InitialEventFlag = true;
        }

        #endregion

        #region Update

        public void Update_DI(EFEM_DI Di, bool value)
        {
            if (IO_DGV.Rows[(int)Di].Cells[3].Value.ToString() != value.ToString() || InitialEventFlag)
            {
                IO_DGV.Rows[(int)Di].Cells[3].Value = value;
                EvnetDIChange(Di, value);//Wayne 20190915
            }
        }


        #endregion

        #region Read/Set

        //public void Set_FFU(int FFUno,bool ref_val) 
        //{
        //    if (FFUAlarm[FFUno] == ref_val) return;
        //    FFUAlarm[FFUno] = ref_val;
        //    if (ref_val) 
        //    {
        //        UI.Alarm(string.Format(EFEM_FFU.FFU.ToString() + "{0}", FFUno + 1), ErrorList.EF_FFU_1104);
        //    }
        //}

        #endregion

        #region Control

        static int StatusVal = 0;
        public static void EFEMStatusControl(StatusControl ref_Control)
        {
            switch (ref_Control)
            {
                case StatusControl.Alloff:
                    StatusVal = 0;
                    break;
                case StatusControl.Run://4 = Green
                    StatusVal = 4;
                    break;
                case StatusControl.Idle: //2 = yellow
                    StatusVal = 2;
                    break;
                case StatusControl.Alarm:
                    StatusVal = 129; //Mike
                    break;
                case StatusControl.BuzzerOff:
                    char [] tempchar=Convert.ToString(StatusVal, 2).PadLeft(16, '0').ToCharArray();
                    tempchar[8] = '0';
                    tempchar[9] = '1';
                    StatusVal = Convert.ToInt32(new string(tempchar),2);
                    break;
            }
          
        }

        #endregion

        ////Wayne Test 0903
        //private void button1_Click(object sender, EventArgs e)
        //{
        //    StatusControl temp;
        //    Enum.TryParse(comboBox1.Text, out temp);
        //    switch (temp)
        //    {
        //        case StatusControl.Alloff:
        //            PLC.Plc_CmdSend(All_Device.EFEM, PLC_Device.W, 80, 0);
        //            break;

        //        case StatusControl.Run:
        //            PLC.Plc_CmdSend(All_Device.EFEM, PLC_Device.W, 80, 4);
        //            break;

        //        case StatusControl.Idle:
        //            PLC.Plc_CmdSend(All_Device.EFEM, PLC_Device.W, 80, 2);
        //            break;

        //        case StatusControl.Alarm:
        //            PLC.Plc_CmdSend(All_Device.EFEM, PLC_Device.W, 80, 129);
        //            break;

        //        case StatusControl.BuzzerOff:
        //            PLC.Plc_CmdSend(All_Device.EFEM, PLC_Device.W, 80, 129);
        //            break;
        //    }
        //}


        //private void button1_Click(object sender, EventArgs e)
        //{
        //    int no = -1;
        //    switch (comboBox1.Text)
        //    {
        //        case "CP0":
        //            //for (int i = 0; i < 10; i++)
        //            //{
        //            //    if (PLC.B[(int)PLC_B.CSTInvasionStart + (i * 1)])
        //            //    {
        //            //        PLC.Plc_CmdSend(All_Device.EFEM, PLC_Device.B, (int)PLC_B.CSTInvasionStart + (i * 1), 0);
        //            //    }
        //            //    else
        //            //    {
        //            //        PLC.Plc_CmdSend(All_Device.EFEM, PLC_Device.B, (int)PLC_B.CSTInvasionStart + (i * 1), 1);
        //            //    }
        //            //}
        //            if (PLC.B[(int)PLC_B.CSTInvasionStart])
        //            {
        //                PLC.Plc_CmdSend_multi(All_Device.EFEM, PLC_Device.B, (int)PLC_B.CSTInvasionStart, 10, 0);
        //            }
        //            else
        //            {
        //                PLC.Plc_CmdSend_multi(All_Device.EFEM, PLC_Device.B, (int)PLC_B.CSTInvasionStart, 10, 1);
        //            }
        //            break;
        //        case "MP0":

        //            for (int i = 0; i < 8; i++)
        //            {
        //                if (PLC.B[(int)PLC_B.MagInvasionStart + (i * 1)])
        //                {
        //                    PLC.Plc_CmdSend(All_Device.EFEM, PLC_Device.B, (int)PLC_B.MagInvasionStart + (i * 1), 0);
        //                }
        //                else
        //                {
        //                    PLC.Plc_CmdSend(All_Device.EFEM, PLC_Device.B, (int)PLC_B.MagInvasionStart + (i * 1), 1);
        //                }
        //            }
        //            break;
        //        case "Stage1":
        //        case "Stage2":
        //            for (int i = 0; i < 2; i++)
        //            {
        //                if (PLC.B[(int)PLC_B.Stage1WaferInvasion + (i * 1)])
        //                {
        //                    PLC.Plc_CmdSend(All_Device.EFEM, PLC_Device.B, (int)PLC_B.Stage1WaferInvasion + (i * 1), 0);
        //                }
        //                else
        //                {
        //                    PLC.Plc_CmdSend(All_Device.EFEM, PLC_Device.B, (int)PLC_B.Stage1WaferInvasion + (i * 1), 1);
        //                }
        //            }
        //            break;
        //        case "Stage1_C":
        //        case "Stage2_C":
        //            for (int i = 0; i < 2; i++)
        //            {
        //                if (PLC.B[(int)PLC_B.Stage1CarrierInvasion + (i * 1)])
        //                {
        //                    PLC.Plc_CmdSend(All_Device.EFEM, PLC_Device.B, (int)PLC_B.Stage1CarrierInvasion + (i * 1), 0);
        //                }
        //                else
        //                {
        //                    PLC.Plc_CmdSend(All_Device.EFEM, PLC_Device.B, (int)PLC_B.Stage1CarrierInvasion + (i * 1), 1);
        //                }
        //            }
        //            break;

        //    }
        //}
    }
    
}
