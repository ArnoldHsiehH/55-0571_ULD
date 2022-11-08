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
    public partial class Form_AutoJob : Form
    {
        //*************************************
        //* This Form is For 55-0359 Auto Job
        //*************************************

        #region Variable

        private List<int[]> SlotData;
        private DataGridView[] SlotDGV;
        private int SourceDeviceTemp;
        private int SourceSlotTemp;
        private int DesDeviceTemp;
        private int DesSlotTemp;

        private ContextMenuStrip TotalDGV_Menu;
        private DataGridView dgvAutoSelectView;
        private int TotalMenuRow = -1;
        private int TotalMenuCol = -1;

        #endregion

        #region JobParameter

        private Dictionary<string, IOLPDevice> OMS_In = new Dictionary<string, IOLPDevice>();
        private Dictionary<string, IOLPDevice> OMS_Out = new Dictionary<string, IOLPDevice>();

        #endregion

        #region Initial

        public Form_AutoJob()
        {
            InitializeComponent();
        }

        public void Initial(List<int[]> AllslotData, IOLPDevice[] In, IOLPDevice[] Out, string[] _Id)
        {

            #region UI

            pnlSource.Controls.Clear();

            #endregion

            #region AutoSelectView

            SlotData = AllslotData;
            dgvAutoSelectView = new DataGridView();
            dgvAutoSelectView.Dock = DockStyle.Fill;

            dgvAutoSelectView.Columns.Add(WaferInforTableItem.SocPort.ToString(), WaferInforTableItem.SocPort.ToString());
            dgvAutoSelectView.Columns.Add(WaferInforTableItem.SocSlot.ToString(), WaferInforTableItem.SocSlot.ToString());
            dgvAutoSelectView.Columns.Add(WaferInforTableItem.DesPort.ToString(), WaferInforTableItem.DesPort.ToString());
            dgvAutoSelectView.Columns.Add(WaferInforTableItem.DesSlot.ToString(), WaferInforTableItem.DesSlot.ToString());

            DataGridViewCheckBoxColumn dgv_CBCol = new DataGridViewCheckBoxColumn();
            dgv_CBCol.HeaderText = "Tilt Use";
            dgvAutoSelectView.Columns.Add(dgv_CBCol);


            for (int i = 0; i < dgvAutoSelectView.Columns.Count; i++)
            {
                dgvAutoSelectView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvAutoSelectView.Columns[i].ReadOnly = true;
            }
            dgvAutoSelectView.AllowUserToAddRows = false;
            dgvAutoSelectView.RowHeadersVisible = false;
            //dgvAutoSelectView.ReadOnly = true;
            dgvAutoSelectView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvAutoSelectView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            dgvAutoSelectView.MouseClick += new MouseEventHandler(TotalDGVRightClick);
            gbxTotal.Controls.Add(dgvAutoSelectView);

            #endregion

            #region TotalDGV Menu

            TotalDGV_Menu = new ContextMenuStrip();
            TotalDGV_Menu.Items.Add("Delete");
            TotalDGV_Menu.ItemClicked += new ToolStripItemClickedEventHandler(TotalDGVMeunClick);

            #endregion

            #region SlotDGV Initial

            SlotDGV = new DataGridView[30];
            SlotDGV_Initail();

            #endregion

            #region ComboBox Initial

            cboSource.Items.Clear();
            #endregion

            cboSource.SelectedIndex = -1;
            SourceDeviceTemp = -1;
            SourceSlotTemp = -1;

        }

        #endregion

        #region SlotDGV

        private void SlotDGV_Initail()
        {
            if (HT.Recipe.AutoMode == PJ_Type.Sortering)
            {
                for (int index = 0; index <= (int)IOLPDevice.CP10; index++)
                {
                    #region Sortering Mode

                    #region Source Port SlotDGV

                    SlotDGV[index] = new DataGridView();
                    SlotDGV[index].Columns.Add("SlotNo", "SlotNo");
                    SlotDGV[index].Columns.Add("SlotData", "SlotData");
                    SlotDGV[index].Columns.Add("Des", "Des");
                    SlotDGV[index].Columns.Add("Slot", "Slot");
                    SlotDGV[index].Name = index.ToString();
                    SlotDGV[index].CellClick += new DataGridViewCellEventHandler(SourceCellClick);
                    SlotDataUpdataDgv(index, index);

                    SlotDGV[index].Dock = DockStyle.Fill;
                    SlotDGV[index].AllowUserToAddRows = false;
                    SlotDGV[index].Columns[0].ReadOnly = true;
                    SlotDGV[index].Columns[1].ReadOnly = true;
                    SlotDGV[index].Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                    SlotDGV[index].Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                    SlotDGV[index].Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                    SlotDGV[index].Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                    SlotDGV[index].AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    SlotDGV[index].AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    SlotDGV[index].SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                    SlotDGV[index].EditMode = DataGridViewEditMode.EditProgrammatically;

                    #endregion

                    #region Destinated Port SlotDGV

                    SlotDGV[index + 10] = new DataGridView();
                    SlotDGV[index + 10].Columns.Add("SlotNo", "SlotNo");
                    SlotDGV[index + 10].Columns.Add("SlotData", "SlotData");
                    SlotDGV[index + 10].Columns.Add("Soc", "Soc");
                    SlotDGV[index + 10].Columns.Add("Slot", "Slot");
                    SlotDGV[index + 10].Name = index.ToString();
                    SlotDGV[index + 10].CellClick += new DataGridViewCellEventHandler(DesCellClick);
                    SlotDataUpdataDgv(index + 10, index);

                    SlotDGV[index + 10].Dock = DockStyle.Fill;
                    SlotDGV[index + 10].AllowUserToAddRows = false;
                    SlotDGV[index + 10].Columns[0].ReadOnly = true;
                    SlotDGV[index + 10].Columns[1].ReadOnly = true;
                    SlotDGV[index + 10].Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                    SlotDGV[index + 10].Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                    SlotDGV[index + 10].Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                    SlotDGV[index + 10].Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                    SlotDGV[index + 10].AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    SlotDGV[index + 10].AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    SlotDGV[index + 10].SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                    SlotDGV[index + 10].EditMode = DataGridViewEditMode.EditProgrammatically;

                    #endregion

                    #endregion
                }
            }
            else
            {
                for (int index = 0; index < (int)IOLPDevice.MaxCnt; index++)
                {
                    #region Not Sortering Mode

                    #region Source/Des/OMS_Out/OMS_In flag

                    string flag = "";

                    if (index <= (int)IOLPDevice.CP10)
                        flag = (HT.Recipe.AutoMode == PJ_Type.Load) ? "Source" : "Destination";
                    else
                    {
                        if (OMS_In.ContainsValue((IOLPDevice)index))
                        {
                            if (HT.Recipe.AutoMode == PJ_Type.Load)
                                flag = "Source";
                            else
                                flag = "OMS In";
                        }
                        else if (OMS_Out.ContainsValue((IOLPDevice)index))
                            flag = "OMS Out";
                        else
                            flag = (HT.Recipe.AutoMode == PJ_Type.Load) ? "Destination" : "Source";
                    }

                    string ColumnsTag = (flag == "Source") ? "Des" : "Soc";

                    if (flag == "OMS Out")
                        ColumnsTag = "Soc";
                    if (flag == "OMS In")
                        ColumnsTag = "Des";

                    #endregion

                    SlotDGV[index] = new DataGridView();
                    SlotDGV[index].Columns.Add("SlotNo", "SlotNo");
                    SlotDGV[index].Columns.Add("SlotData", "SlotData");
                    SlotDGV[index].Columns.Add(ColumnsTag, ColumnsTag);
                    SlotDGV[index].Columns.Add("Slot", "Slot");
                    SlotDGV[index].Name = index.ToString();
                    SlotDataUpdataDgv(index, index);


                    if (flag == "Source")
                        SlotDGV[index].CellClick += new DataGridViewCellEventHandler(SourceCellClick);
                    else
                        SlotDGV[index].CellClick += new DataGridViewCellEventHandler(DesCellClick);

                    if (flag == "OMS Out" || flag == "OMS In" || (HT.Recipe.AutoMode == PJ_Type.Load && flag == "Destination"))
                        SlotDGV[index].CellClick += new DataGridViewCellEventHandler(OMS_DesCellClick);



                    SlotDGV[index].Dock = DockStyle.Fill;
                    SlotDGV[index].AllowUserToAddRows = false;
                    SlotDGV[index].Columns[0].ReadOnly = true;
                    SlotDGV[index].Columns[1].ReadOnly = true;
                    SlotDGV[index].Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                    SlotDGV[index].Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                    SlotDGV[index].Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                    SlotDGV[index].Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                    SlotDGV[index].AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    SlotDGV[index].AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    SlotDGV[index].SelectionMode = DataGridViewSelectionMode.ColumnHeaderSelect;
                    SlotDGV[index].EditMode = DataGridViewEditMode.EditProgrammatically;

                    #endregion
                }
            }
        }

        private void SlotDataUpdataDgv(int slotDGVCnt, int deviceCnt)
        {
            if (SlotData[deviceCnt] != null && SlotData[deviceCnt].Length != 0)
            {
                SlotDGV[slotDGVCnt].Rows.Clear();
                int SlotCount = SlotData[deviceCnt].Length;

                SlotDGV[slotDGVCnt].Rows.Add(SlotCount);
                for (int i = 0; i < SlotCount; i++)
                {
                    Color tempcolor = Color.White;
                    switch (SlotData[deviceCnt][i])
                    {
                        case (int)WaferStatus.WithOut:
                            tempcolor = Color.White;
                            break;
                        case (int)WaferStatus.With:
                            tempcolor = Color.LightGreen;
                            break;
                        case (int)WaferStatus.Cross:
                            tempcolor = Color.Red;
                            break;
                        case (int)WaferStatus.Unknown:
                            tempcolor = Color.Gray;
                            break;
                    }

                    SlotDGV[slotDGVCnt].Rows[i].Cells[0].Value = (i + 1);
                    SlotDGV[slotDGVCnt].Rows[i].Cells[1].Style.BackColor = tempcolor;
                }
            }
        }

        private void SourceCellClick(object obj, DataGridViewCellEventArgs e)
        {
            DataGridView tempDGV = (DataGridView)obj;
            int device = Convert.ToInt32(tempDGV.Name);

            #region Interlock

            if (e.RowIndex == -1)
                return;

            if (tempDGV.Rows[e.RowIndex].Cells[2].Style.BackColor == Color.Gray)
            {
                MessageBox.Show("The source slot has be assigned!",
                                  "Warning",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
                return;
            }
            if (tempDGV.Rows[e.RowIndex].Cells[1].Style.BackColor != Color.LightGreen)
            {
                MessageBox.Show("Unselectable slot!",
                                  "Warning",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
                return;
            }
            #endregion

            SourceDeviceTemp = device;
            SourceSlotTemp = Convert.ToInt32(tempDGV.Rows[e.RowIndex].Cells[0].Value.ToString());
        }

        private void DesCellClick(object obj, DataGridViewCellEventArgs e)
        {
            DataGridView tempDGV = (DataGridView)obj;
            int device = Convert.ToInt32(tempDGV.Name);

            SourceDeviceTemp = -1;
            SourceSlotTemp = -1;
            DesDeviceTemp = -1;
            DesSlotTemp = -1;

        }

        private void OMS_DesCellClick(object obj, DataGridViewCellEventArgs e)
        {

            DataGridView tempDGV = (DataGridView)obj;
            int device = Convert.ToInt32(tempDGV.Name);

            SourceDeviceTemp = -1;
            SourceSlotTemp = -1;
            DesDeviceTemp = -1;
            DesSlotTemp = -1;
        }

        private void SwapCellClick(object obj, DataGridViewCellEventArgs e)
        {

            DataGridView tempDGV = (DataGridView)obj;
            int device = Convert.ToInt32(tempDGV.Name);

            #region Interlock

            if (e.RowIndex == -1)
                return;
            if (DesDeviceTemp != -1 || DesSlotTemp != -1)
                return;

            if (tempDGV.Rows[e.RowIndex].Cells[2].Style.BackColor == Color.Gray)
                return;
            if (tempDGV.Rows[e.RowIndex].Cells[1].Style.BackColor != Color.LightGreen)
            {
                MessageBox.Show("Unselectable slot!",
                                  "Warning",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
                return;
            }

            #endregion

            string SourceDevice = string.Format("{0}", (IOLPDevice)SourceDeviceTemp);
            string DesDevice = string.Format("{0}", (IOLPDevice)DesDeviceTemp);
            string SwapDevice = string.Format("{0}", (IOLPDevice)device);
            int SwapSlotTemp = (e.RowIndex + 1);

            Slot_UI_Update((IOLPDevice)SourceDeviceTemp, SourceSlotTemp, (IOLPDevice)DesDeviceTemp, DesSlotTemp);

            SlotDGV[(int)device].Rows[SwapSlotTemp - 1].Cells[2].Style.BackColor = Color.Gray;
            SlotDGV[(int)device].Rows[SwapSlotTemp - 1].Cells[3].Style.BackColor = Color.Gray;
            SlotDGV[(int)device].Rows[SwapSlotTemp - 1].Cells[2].Value = SourceDevice;
            SlotDGV[(int)device].Rows[SwapSlotTemp - 1].Cells[3].Value = SourceSlotTemp;

            SourceDeviceTemp = -1;
            SourceSlotTemp = -1;
            DesDeviceTemp = -1;
            DesSlotTemp = -1;

            gbxSource.Enabled = true;
        }

        #endregion

        #region UI Update

        private void Slot_UI_Update(IOLPDevice SocPort, int SocSlot, IOLPDevice DecPort, int DecSlot)
        {
            SlotDGV[(int)SocPort].Rows[SocSlot - 1].Cells[2].Style.BackColor = Color.Gray;
            SlotDGV[(int)SocPort].Rows[SocSlot - 1].Cells[3].Style.BackColor = Color.Gray;
            SlotDGV[(int)SocPort].Rows[SocSlot - 1].Cells[2].Value = DecPort.ToString();
            SlotDGV[(int)SocPort].Rows[SocSlot - 1].Cells[3].Value = DecSlot;

            int DesIndex = (int)DecPort;
            if (HT.Recipe.AutoMode == PJ_Type.Sortering)
                DesIndex += (int)IOLPDevice.CP10 + 1;

            if (OMS_Out.ContainsValue((DecPort)) != true)
            {
                SlotDGV[DesIndex].Rows[DecSlot - 1].Cells[2].Style.BackColor = Color.Gray;
                SlotDGV[DesIndex].Rows[DecSlot - 1].Cells[3].Style.BackColor = Color.Gray;
            }
            SlotDGV[DesIndex].Rows[DecSlot - 1].Cells[2].Value = SocPort.ToString();
            SlotDGV[DesIndex].Rows[DecSlot - 1].Cells[3].Value = SocSlot;
        }

        private void Add_To_JobList(string SocPort, int SocSlot, string DesPort, int DesSlot, bool oms)
        {

            int lastrow = dgvAutoSelectView.Rows.Count;
            dgvAutoSelectView.Rows.Insert(lastrow);
            dgvAutoSelectView.Rows[lastrow].Cells[0].Value = SocPort;
            dgvAutoSelectView.Rows[lastrow].Cells[1].Value = SocSlot;
            dgvAutoSelectView.Rows[lastrow].Cells[2].Value = DesPort;
            dgvAutoSelectView.Rows[lastrow].Cells[3].Value = DesSlot;
            dgvAutoSelectView.Rows[lastrow].Cells[4].Value = oms;
            dgvAutoSelectView.Rows[lastrow].Cells[7].Value = true;
            dgvAutoSelectView.CurrentCell = dgvAutoSelectView.Rows[lastrow].Cells[0];
            dgvAutoSelectView.Show();
        }

        private void Add_To_JobList(string SocPort, int SocSlot, string DesPort, int DesSlot, bool oms, string SwapPort, int SwapSlot)
        {
            int lastrow = dgvAutoSelectView.Rows.Count;

            dgvAutoSelectView.Rows.Insert(lastrow);
            dgvAutoSelectView.Rows[lastrow].Cells[0].Value = SocPort;
            dgvAutoSelectView.Rows[lastrow].Cells[1].Value = SocSlot;
            dgvAutoSelectView.Rows[lastrow].Cells[2].Value = DesPort;
            dgvAutoSelectView.Rows[lastrow].Cells[3].Value = DesSlot;
            dgvAutoSelectView.Rows[lastrow].Cells[4].Value = oms;
            dgvAutoSelectView.Rows[lastrow].Cells[5].Value = SwapPort;
            dgvAutoSelectView.Rows[lastrow].Cells[6].Value = SwapSlot;
            dgvAutoSelectView.Rows[lastrow].Cells[7].Value = true;
            dgvAutoSelectView.CurrentCell = dgvAutoSelectView.Rows[lastrow].Cells[0];
            dgvAutoSelectView.Show();
        }

        #endregion

        #region TotalDGV Job Delete

        private void TotalDGVRightClick(object obj, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                DataGridView tempDGV = (DataGridView)obj;
                var hti = tempDGV.HitTest(e.X, e.Y);
                if (hti.RowIndex == -1 || hti.ColumnIndex == -1) return;
                tempDGV.CurrentCell = tempDGV.Rows[hti.RowIndex].Cells[0];
                TotalMenuRow = hti.RowIndex;
                TotalMenuCol = hti.ColumnIndex;
                TotalDGV_Menu.Show(tempDGV, new Point(e.X, e.Y));
            }
        }

        private void TotalDGVMeunClick(object obj, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.ToString() == "Delete")
            {

                #region Update SlotDGV UI

                string SocDevice = dgvAutoSelectView.Rows[TotalMenuRow].Cells[0].Value.ToString();
                int SocNo = (int)(IOLPDevice)Enum.Parse(typeof(IOLPDevice), SocDevice);
                int SocSlot = Convert.ToInt32(dgvAutoSelectView.Rows[TotalMenuRow].Cells[1].Value.ToString());
                SlotDGV[SocNo].Rows[SocSlot - 1].Cells[2].Style.BackColor = Color.White;
                SlotDGV[SocNo].Rows[SocSlot - 1].Cells[3].Style.BackColor = Color.White;
                SlotDGV[SocNo].Rows[SocSlot - 1].Cells[2].Value = "";
                SlotDGV[SocNo].Rows[SocSlot - 1].Cells[3].Value = "";

                string DesDevice = dgvAutoSelectView.Rows[TotalMenuRow].Cells[2].Value.ToString();
                int DesNo = (int)(IOLPDevice)Enum.Parse(typeof(IOLPDevice), DesDevice);
                int DesSlot = Convert.ToInt32(dgvAutoSelectView.Rows[TotalMenuRow].Cells[3].Value.ToString());
                if (HT.Recipe.AutoMode == PJ_Type.Sortering)
                    DesNo += (int)IOLPDevice.CP10 + 1;
                SlotDGV[DesNo].Rows[DesSlot - 1].Cells[2].Style.BackColor = Color.White;
                SlotDGV[DesNo].Rows[DesSlot - 1].Cells[3].Style.BackColor = Color.White;
                SlotDGV[DesNo].Rows[DesSlot - 1].Cells[2].Value = "";
                SlotDGV[DesNo].Rows[DesSlot - 1].Cells[3].Value = "";


                if (HT.Recipe.AutoMode == PJ_Type.LoadUnload)
                {
                    string SwapDevice = dgvAutoSelectView.Rows[TotalMenuRow].Cells[5].Value.ToString();
                    int SwapNo = (int)(IOLPDevice)Enum.Parse(typeof(IOLPDevice), SwapDevice);
                    int SwapSlot = Convert.ToInt32(dgvAutoSelectView.Rows[TotalMenuRow].Cells[6].Value.ToString());
                    if (HT.Recipe.AutoMode == PJ_Type.Sortering)
                        DesNo += (int)IOLPDevice.CP10 + 1;
                    SlotDGV[SwapNo].Rows[SwapSlot - 1].Cells[2].Style.BackColor = Color.White;
                    SlotDGV[SwapNo].Rows[SwapSlot - 1].Cells[3].Style.BackColor = Color.White;
                    SlotDGV[SwapNo].Rows[SwapSlot - 1].Cells[2].Value = "";
                    SlotDGV[SwapNo].Rows[SwapSlot - 1].Cells[3].Value = "";
                }


                #endregion

                dgvAutoSelectView.Rows.RemoveAt(TotalMenuRow);

            }
        }

        #endregion

        #region ComboBox

        private void cboSoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region device

            IOLPDevice device;

            if (OMS_In.TryGetValue(cboSource.Text, out device) != true)
                device = (IOLPDevice)Enum.Parse(typeof(IOLPDevice), cboSource.Text);

            #endregion

            pnlSource.Controls.Clear();
            pnlSource.Controls.Add(SlotDGV[(int)device]);

            SourceDeviceTemp = -1;
            SourceSlotTemp = -1;
        }

        private void cboDes_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboSwap_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        #endregion

        #region Control

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (dgvAutoSelectView.Rows.Count == 0)
                return;

          
            string InsertCom = "";
            int StagePos_temp = 0;

            for (int row = 0; row < dgvAutoSelectView.Rows.Count; row++)
            {
                bool flag = false;
                string SocPort;
                string SocSlot;
                string DesPort;
                string DesSlot;

            }

            SQLite.Multi_InsertWaferInfo(InsertCom);
            this.Close();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            SourceDeviceTemp = -1;
            SourceSlotTemp = -1;
            gbxSource.Enabled = true;
        }

        #endregion

    }
}
