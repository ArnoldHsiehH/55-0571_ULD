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
    public partial class Form_Auto : Form
    {

        private class SlotInfo
        {
            public int No { get; set; }
            public int Slot { get; set; }
        }

        private Panel pnlForm;

        #region Load

        private Panel pnlCP;
        private GroupBox gbxCP;
        private Label labCP;

        private bool LoadAllStatus;
        private List<SlotInfo> LoadAllList;

        #endregion

        #region Unload

        private Panel pnlMP;
        private GroupBox gbxMP;
        private ComboBox cboMP;
        private Label labMP;

        #endregion

        #region Total

        private DataGridView dgvAutoSelectView;
        private GroupBox gbxTotal;
        private ContextMenuStrip TotalDGV_Menu;

        #endregion

        #region Control
        private Label labReceiptValue;
        private Label labReceipt;
        private GroupBox gbxControl;
        private Button btnRun;
        private Button btnLoadToUnloadAll;
        // private bool UnLoadAllStatus;
        #endregion

        DataGridView[] SlotDGV;
        List<int[]> SlotData;

        private int SorteringLoadtoDeviceNameTemp;
        private int SorteringLoadtoDeviceSlotTemp;
        private int TotalMenuRow = -1;
        private ComboBox cboCP;
        private int TotalMenuCol = -1;

        //private string EfemMode;

        private void InitializeComponent()
        {
            this.pnlForm = new System.Windows.Forms.Panel();
            this.labReceiptValue = new System.Windows.Forms.Label();
            this.gbxMP = new System.Windows.Forms.GroupBox();
            this.pnlMP = new System.Windows.Forms.Panel();
            this.cboMP = new System.Windows.Forms.ComboBox();
            this.labMP = new System.Windows.Forms.Label();
            this.gbxCP = new System.Windows.Forms.GroupBox();
            this.pnlCP = new System.Windows.Forms.Panel();
            this.labCP = new System.Windows.Forms.Label();
            this.gbxTotal = new System.Windows.Forms.GroupBox();
            this.gbxControl = new System.Windows.Forms.GroupBox();
            this.btnLoadToUnloadAll = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.labReceipt = new System.Windows.Forms.Label();
            this.cboCP = new System.Windows.Forms.ComboBox();
            this.pnlForm.SuspendLayout();
            this.gbxMP.SuspendLayout();
            this.gbxCP.SuspendLayout();
            this.gbxControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlForm
            // 
            this.pnlForm.Controls.Add(this.labReceiptValue);
            this.pnlForm.Controls.Add(this.gbxMP);
            this.pnlForm.Controls.Add(this.gbxCP);
            this.pnlForm.Controls.Add(this.gbxTotal);
            this.pnlForm.Controls.Add(this.gbxControl);
            this.pnlForm.Controls.Add(this.labReceipt);
            this.pnlForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlForm.Location = new System.Drawing.Point(0, 0);
            this.pnlForm.Name = "pnlForm";
            this.pnlForm.Size = new System.Drawing.Size(972, 729);
            this.pnlForm.TabIndex = 0;
            // 
            // labReceiptValue
            // 
            this.labReceiptValue.AutoSize = true;
            this.labReceiptValue.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labReceiptValue.Location = new System.Drawing.Point(812, 9);
            this.labReceiptValue.Name = "labReceiptValue";
            this.labReceiptValue.Size = new System.Drawing.Size(18, 19);
            this.labReceiptValue.TabIndex = 5;
            this.labReceiptValue.Text = "N";
            // 
            // gbxMP
            // 
            this.gbxMP.Controls.Add(this.pnlMP);
            this.gbxMP.Controls.Add(this.cboMP);
            this.gbxMP.Controls.Add(this.labMP);
            this.gbxMP.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxMP.Location = new System.Drawing.Point(362, 311);
            this.gbxMP.Name = "gbxMP";
            this.gbxMP.Size = new System.Drawing.Size(361, 405);
            this.gbxMP.TabIndex = 4;
            this.gbxMP.TabStop = false;
            this.gbxMP.Text = "MagazinePort";
            // 
            // pnlMP
            // 
            this.pnlMP.Location = new System.Drawing.Point(9, 38);
            this.pnlMP.Name = "pnlMP";
            this.pnlMP.Size = new System.Drawing.Size(346, 356);
            this.pnlMP.TabIndex = 2;
            // 
            // cboMP
            // 
            this.cboMP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMP.FormattingEnabled = true;
            this.cboMP.Location = new System.Drawing.Point(41, 14);
            this.cboMP.Name = "cboMP";
            this.cboMP.Size = new System.Drawing.Size(121, 23);
            this.cboMP.TabIndex = 1;
            this.cboMP.SelectedIndexChanged += new System.EventHandler(this.cmbAutoUnLP_SelectedIndexChanged);
            // 
            // labMP
            // 
            this.labMP.AutoSize = true;
            this.labMP.Location = new System.Drawing.Point(7, 17);
            this.labMP.Name = "labMP";
            this.labMP.Size = new System.Drawing.Size(28, 15);
            this.labMP.TabIndex = 0;
            this.labMP.Text = "MP:";
            // 
            // gbxCP
            // 
            this.gbxCP.Controls.Add(this.cboCP);
            this.gbxCP.Controls.Add(this.pnlCP);
            this.gbxCP.Controls.Add(this.labCP);
            this.gbxCP.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxCP.Location = new System.Drawing.Point(4, 311);
            this.gbxCP.Name = "gbxCP";
            this.gbxCP.Size = new System.Drawing.Size(352, 405);
            this.gbxCP.TabIndex = 3;
            this.gbxCP.TabStop = false;
            this.gbxCP.Text = "CassetterPort";
            // 
            // pnlCP
            // 
            this.pnlCP.Location = new System.Drawing.Point(9, 38);
            this.pnlCP.Name = "pnlCP";
            this.pnlCP.Size = new System.Drawing.Size(337, 356);
            this.pnlCP.TabIndex = 4;
            // 
            // labCP
            // 
            this.labCP.AutoSize = true;
            this.labCP.Location = new System.Drawing.Point(6, 17);
            this.labCP.Name = "labCP";
            this.labCP.Size = new System.Drawing.Size(28, 15);
            this.labCP.TabIndex = 0;
            this.labCP.Text = "CP:";
            // 
            // gbxTotal
            // 
            this.gbxTotal.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxTotal.Location = new System.Drawing.Point(4, 4);
            this.gbxTotal.Name = "gbxTotal";
            this.gbxTotal.Size = new System.Drawing.Size(719, 301);
            this.gbxTotal.TabIndex = 2;
            this.gbxTotal.TabStop = false;
            this.gbxTotal.Text = "Auto Job";
            // 
            // gbxControl
            // 
            this.gbxControl.Controls.Add(this.btnLoadToUnloadAll);
            this.gbxControl.Controls.Add(this.btnRun);
            this.gbxControl.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxControl.Location = new System.Drawing.Point(729, 316);
            this.gbxControl.Name = "gbxControl";
            this.gbxControl.Size = new System.Drawing.Size(231, 401);
            this.gbxControl.TabIndex = 1;
            this.gbxControl.TabStop = false;
            this.gbxControl.Text = "Control";
            // 
            // btnLoadToUnloadAll
            // 
            this.btnLoadToUnloadAll.Location = new System.Drawing.Point(22, 21);
            this.btnLoadToUnloadAll.Name = "btnLoadToUnloadAll";
            this.btnLoadToUnloadAll.Size = new System.Drawing.Size(187, 39);
            this.btnLoadToUnloadAll.TabIndex = 5;
            this.btnLoadToUnloadAll.Text = "Load to UnLoad All";
            this.btnLoadToUnloadAll.UseVisualStyleBackColor = true;
            this.btnLoadToUnloadAll.Click += new System.EventHandler(this.btnLoadToUnloadAll_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(22, 66);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(187, 38);
            this.btnRun.TabIndex = 16;
            this.btnRun.Text = "Setting";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnAutoRun_Click);
            // 
            // labReceipt
            // 
            this.labReceipt.AutoSize = true;
            this.labReceipt.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labReceipt.Location = new System.Drawing.Point(725, 9);
            this.labReceipt.Name = "labReceipt";
            this.labReceipt.Size = new System.Drawing.Size(81, 19);
            this.labReceipt.TabIndex = 0;
            this.labReceipt.Text = "Receipt:";
            // 
            // cboCP
            // 
            this.cboCP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCP.FormattingEnabled = true;
            this.cboCP.Location = new System.Drawing.Point(40, 14);
            this.cboCP.Name = "cboCP";
            this.cboCP.Size = new System.Drawing.Size(121, 23);
            this.cboCP.TabIndex = 5;
            // 
            // Form_Auto
            // 
            this.ClientSize = new System.Drawing.Size(972, 729);
            this.Controls.Add(this.pnlForm);
            this.Name = "Form_Auto";
            this.pnlForm.ResumeLayout(false);
            this.pnlForm.PerformLayout();
            this.gbxMP.ResumeLayout(false);
            this.gbxMP.PerformLayout();
            this.gbxCP.ResumeLayout(false);
            this.gbxCP.PerformLayout();
            this.gbxControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        public Form_Auto()
        {
            InitializeComponent();
        }

        public void Initial(List<int[]> AllslotData)
        {
            #region New

            DataTable OldWaferDt = SQLite.ReadDataTable(SQLTable.PJ_Pool, "1=1");
            labReceiptValue.Text = frmMain.EFEM_Mode;

            ChangeView(frmMain.EFEM_Mode);

            LoadAllStatus = true;
            //UnLoadAllStatus = true;     

            pnlCP.Controls.Clear();
            pnlMP.Controls.Clear();

            cboCP.Items.Clear();
            cboMP.Items.Clear();


            SlotData = AllslotData;
            dgvAutoSelectView = new DataGridView();
            dgvAutoSelectView.Dock = DockStyle.Fill;

            dgvAutoSelectView.Columns.Add(WaferInforTableItem.SocPort.ToString(), WaferInforTableItem.SocPort.ToString());
            dgvAutoSelectView.Columns.Add(WaferInforTableItem.SocSlot.ToString(), WaferInforTableItem.SocSlot.ToString());
            dgvAutoSelectView.Columns.Add(WaferInforTableItem.DesPort.ToString(), WaferInforTableItem.DesPort.ToString());
            dgvAutoSelectView.Columns.Add(WaferInforTableItem.DesSlot.ToString(), WaferInforTableItem.DesSlot.ToString());
            dgvAutoSelectView.Columns.Add(WaferInforTableItem.WaferStatus.ToString(), WaferInforTableItem.WaferStatus.ToString());
            dgvAutoSelectView.Columns.Add(WaferInforTableItem.CarrierStatus.ToString(), WaferInforTableItem.CarrierStatus.ToString());

            dgvAutoSelectView.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvAutoSelectView.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvAutoSelectView.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvAutoSelectView.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;

            dgvAutoSelectView.AllowUserToAddRows = false;

            dgvAutoSelectView.RowHeadersVisible = false;
            dgvAutoSelectView.ReadOnly = true;
            dgvAutoSelectView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvAutoSelectView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            dgvAutoSelectView.MouseClick += new MouseEventHandler(TotalDGVRightClick);

            gbxTotal.Controls.Add(dgvAutoSelectView);

        //    dgvAutoSelectView.DataSource = OldWaferDt;

            TotalDGV_Menu = new ContextMenuStrip();
            TotalDGV_Menu.Items.Add("Delete");
            TotalDGV_Menu.ItemClicked += new ToolStripItemClickedEventHandler(TotalDGVMeunClick);


            SlotDGV = new DataGridView[(int)IOLPDevice.MaxCnt];

            for (int i = 0; i < (int)IOLPDevice.MaxCnt; i++)
            {

                SlotDGV[i] = new DataGridView();
                SlotDGV[i].Dock = DockStyle.Fill;
                SlotDGV[i] = new DataGridView();

                if (i <= (int)IOLPDevice.CP10)
                {
                    cboMP.Items.Add(IOLPDevice.CP1 + i);

                    //DataGridViewCheckBoxColumn DGV_CB_Col = new DataGridViewCheckBoxColumn();
                    //DGV_CB_Col.HeaderText = "All";
                    //SlotDGV[i].Columns.Insert(0, DGV_CB_Col);
                    SlotDGV[i].Columns.Add("SlotNo", "SlotNo");
                    SlotDGV[i].Columns.Add("SlotData", "SlotData");
                    SlotDGV[i].Columns.Add("Des", "Des");
                    SlotDGV[i].Columns.Add("Slot", "Slot");
                    SlotDGV[i].Name = i.ToString();
                    SlotDGV[i].CellClick += new DataGridViewCellEventHandler(DgvLoadCellClick);
                    LoadSlotDataUpdataDgv(i);
                   // pnlCP.Controls.Add(SlotDGV[i]);
                }
                else
                {
                    cboMP.Items.Add(IOLPDevice.MP1 + i);

                    SlotDGV[i].Columns.Add("SlotNo", "SlotNo");
                    SlotDGV[i].Columns.Add("SlotData", "SlotData");
                    SlotDGV[i].Columns.Add("Source", "Source");
                    SlotDGV[i].Columns.Add("Slot", "Slot");
                    SlotDGV[i].Name = i.ToString();
                    SlotDGV[i].CellClick += new DataGridViewCellEventHandler(DgvUnLoadCellClick);
                    UnLoadSlotDataUpdataDgv(i);
                }

                SlotDGV[i].Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                SlotDGV[i].Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;

                SlotDGV[i].AllowUserToAddRows = false;
                SlotDGV[i].Dock = DockStyle.Fill;
                SlotDGV[i].RowHeadersVisible = false;
                SlotDGV[i].Columns[1].ReadOnly = true;
                SlotDGV[i].Columns[2].ReadOnly = true;
                SlotDGV[i].AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                SlotDGV[i].AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
                // SlotDGV[i].AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                SlotDGV[i].SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                SlotDGV[i].EditMode = DataGridViewEditMode.EditProgrammatically;
            }

            cboMP.SelectedIndex = 0;
            cboCP.SelectedIndex = 0;

            #endregion

            #region Old


            //if ( OldWaferDt != null)
            //{
            //    for (int row = 0; row < OldWaferDt.Rows.Count; row++)
            //    {
            //        string SourceDevice = OldWaferDt.Rows[row][0].ToString();
            //        int SourceSlot = Convert.ToInt32(OldWaferDt.Rows[row][1].ToString());
            //        int SourceID = (int)(IOLPDevice)Enum.Parse(typeof(IOLPDevice), SourceDevice);

            //        if (frmMain.EFEM_Mode == NormalStatic.Sortering)
            //        {
            //            string DestinationDevice = OldWaferDt.Rows[row][2].ToString();
            //            int DestinationSlot = Convert.ToInt32(OldWaferDt.Rows[row][3].ToString());
            //            int DestinationID = (int)(IOLPDevice)Enum.Parse(typeof(IOLPDevice), DestinationDevice);

            //            SlotDGV[DestinationID].Rows[DestinationSlot - 1].Cells[2].Value = SourceDevice;
            //            SlotDGV[DestinationID].Rows[DestinationSlot - 1].Cells[3].Value = SourceSlot;
            //            SlotDGV[DestinationID].Rows[DestinationSlot - 1].Cells[1].Style.BackColor = Color.LightGreen;

            //            SlotDGV[SourceID].Rows[SourceSlot - 1].Cells[2].Value = DestinationDevice;
            //            SlotDGV[SourceID].Rows[SourceSlot - 1].Cells[3].Value = DestinationSlot;//Wayne
            //        }

            //        SlotDGV[SourceID].Rows[SourceSlot-1].Cells[2].Style.BackColor = Color.Gray;
            //    }
            //} 

            #endregion
        }

        private void Form_Auto_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        #region Load

        private void LoadSlotDataUpdataDgv(int deviceCnt)
        {
            if (SlotData[deviceCnt] != null && SlotData[deviceCnt].Length != 0)
            {
                SlotDGV[deviceCnt].Rows.Clear();
                int UpSlotCount = SlotData[deviceCnt].Length;
                SlotDGV[deviceCnt].Rows.Add(UpSlotCount);
                for (int i = 0; i < UpSlotCount; i++)
                {
                    Color tempcolor = Color.White;
                    switch (SlotData[deviceCnt][i])
                    {
                        case 0:
                            tempcolor = Color.White;
                            break;
                        case 1:
                            tempcolor = Color.LightGreen;
                            break;
                        case 2:
                            tempcolor = Color.Red;
                            break;
                    }
                    SlotDGV[deviceCnt].Rows[i].Cells[0].Value = false;
                    SlotDGV[deviceCnt].Rows[i].Cells[1].Value = (i + 1);
                    SlotDGV[deviceCnt].Rows[i].Cells[2].Style.BackColor = tempcolor;
                }
                SlotDGV[deviceCnt].CurrentCell = SlotDGV[deviceCnt].Rows[0].Cells[1];
            }
        }

        private void DgvLoadCellClick(object obj, DataGridViewCellEventArgs e)
        {

            DataGridView tempDGV = (DataGridView)obj;
            int device = Convert.ToInt32(tempDGV.Name);
            // tbAutoLPSelect.Text = null;
            SorteringLoadtoDeviceNameTemp = -1;
            SorteringLoadtoDeviceSlotTemp = -1;

            if (e.RowIndex != -1)
            {
                tempDGV.CurrentCell = tempDGV.Rows[e.RowIndex].Cells[1];
            }

            //if (frmMain.EFEM_Mode != NormalStatic.Sortering)
            //{
            //    if (e.ColumnIndex != 0 && e.RowIndex != -1)  //Data one slot click
            //    {
            //        if (SlotData[device][e.RowIndex] != 1 || tempDGV.Rows[e.RowIndex].Cells[2].Style.BackColor == Color.Gray)
            //            return;

            //        tempDGV.Rows[e.RowIndex].Cells[2].Style.BackColor = Color.Gray;

            //        int LastRow = dgvAutoSelectView.Rows.Count;
            //        dgvAutoSelectView.Rows.Insert(LastRow);
            //        dgvAutoSelectView.Rows[LastRow].Cells[0].Value = string.Format("{0}", (IOLPDevice)device);    //DeviceName
            //        dgvAutoSelectView.Rows[LastRow].Cells[1].Value = Convert.ToInt32(tempDGV.Rows[e.RowIndex].Cells[1].Value.ToString()); //Slot
            //        dgvAutoSelectView.CurrentCell = dgvAutoSelectView.Rows[LastRow].Cells[0];
            //        dgvAutoSelectView.Show();

            //    }
            //    else if (e.ColumnIndex == 0 && e.RowIndex == -1)      // all select
            //    {
            //        if (tempDGV.Rows.Count == 0)
            //            return;

            //        for (int idx = 0; idx < SlotData[device].Length; idx++)
            //        {

            //            if (SlotData[device][idx] != 1 || tempDGV.Rows[idx].Cells[2].Style.BackColor == Color.Gray)
            //            {
            //                continue;
            //            }

            //            tempDGV.Rows[idx].Cells[2].Style.BackColor = Color.Gray;

            //            int lastrow = dgvAutoSelectView.Rows.Count;
            //            dgvAutoSelectView.Rows.Insert(lastrow);
            //            dgvAutoSelectView.Rows[lastrow].Cells[0].Value = string.Format("{0}", (IOLPDevice)device);  //DeviceName
            //            dgvAutoSelectView.Rows[lastrow].Cells[1].Value = Convert.ToInt32(tempDGV.Rows[idx].Cells[1].Value.ToString());//Slot
            //            dgvAutoSelectView.CurrentCell = dgvAutoSelectView.Rows[lastrow].Cells[0];
            //            dgvAutoSelectView.Show();

            //        }
            //    }
            //}
            //else
            {
                if (e.ColumnIndex.ToString() != "0" && e.RowIndex.ToString() != "-1")  //Sorter click Load save temp data
                {
                    if (SlotData[device][e.RowIndex] != 1 || tempDGV.Rows[e.RowIndex].Cells[2].Style.BackColor == Color.Gray)
                        return;

                    SorteringLoadtoDeviceNameTemp = device;
                    SorteringLoadtoDeviceSlotTemp = Convert.ToInt32(tempDGV.Rows[e.RowIndex].Cells[1].Value.ToString());

                }
                else if (e.ColumnIndex == 0 && e.RowIndex != -1)
                {
                    if (SlotData[device][e.RowIndex] != 1)
                    {
                        tempDGV.Rows[Convert.ToInt32(e.RowIndex.ToString())].Cells[0].Value = false;
                        return;
                    }

                    tempDGV.EditMode = DataGridViewEditMode.EditOnEnter;
                    switch (tempDGV.Rows[Convert.ToInt32(e.RowIndex.ToString())].Cells[0].Value.ToString())
                    {
                        case "True":
                            tempDGV.Rows[Convert.ToInt32(e.RowIndex.ToString())].Cells[0].Value = false;
                            break;
                        case "False":
                            tempDGV.Rows[Convert.ToInt32(e.RowIndex.ToString())].Cells[0].Value = true; ;
                            break;
                    }
                    tempDGV.CurrentCell = tempDGV.Rows[Convert.ToInt32(e.RowIndex.ToString())].Cells[1];
                    tempDGV.EditMode = DataGridViewEditMode.EditProgrammatically;
                }
                else if (e.ColumnIndex == 0 && e.RowIndex == -1)// all select
                {
                    if (tempDGV.Rows.Count == 0)
                        return;

                    LoadAllList = new List<SlotInfo>();
                    for (int idx = 0; idx < SlotData[device].Length; idx++)
                    {
                        if (SlotData[device][idx] != 1 || tempDGV.Rows[idx].Cells[2].Style.BackColor == Color.Gray)
                        {
                            continue;
                        }

                        if (SlotData[device][idx] == 1)
                        {
                            tempDGV.Rows[idx].Cells[0].Value = LoadAllStatus;
                            LoadAllList.Add(new SlotInfo
                            {
                                No = device,
                                Slot = idx
                            });
                        }
                        else
                        {
                            tempDGV.Rows[idx].Cells[0].Value = false;
                        }
                    }
                    LoadAllStatus = LoadAllStatus ? false : true;
                }
            }
        }

        #endregion

        #region UnLoad

        private void cmbAutoUnLP_SelectedIndexChanged(object sender, EventArgs e)
        {
            int device = (int)(IOLPDevice)Enum.Parse(typeof(IOLPDevice), cboMP.Text);
            pnlMP.Controls.Clear();
            pnlMP.Controls.Add(SlotDGV[device]);
        }

        private void UnLoadSlotDataUpdataDgv(int deviceCnt)
        {
            if (SlotData[deviceCnt] != null && SlotData[deviceCnt].Length != 0)
            {
                SlotDGV[deviceCnt].Rows.Clear();
                int UpSlotCount = SlotData[deviceCnt].Length;
                SlotDGV[deviceCnt].Rows.Add(UpSlotCount);
                for (int i = 0; i < UpSlotCount; i++)
                {
                    Color tempcolor = Color.White;
                    switch (SlotData[deviceCnt][i])
                    {
                        case 0:
                            tempcolor = Color.White;
                            break;
                        case 1:
                            tempcolor = Color.LightGreen;
                            break;
                        case 2:
                            tempcolor = Color.Red;
                            break;
                    }
                    SlotDGV[deviceCnt].Rows[i].Cells[0].Value = (i + 1);
                    SlotDGV[deviceCnt].Rows[i].Cells[1].Style.BackColor = tempcolor;
                }
                SlotDGV[deviceCnt].CurrentCell = SlotDGV[deviceCnt].Rows[0].Cells[0];
            }
        }

        private void DgvUnLoadCellClick(object obj, DataGridViewCellEventArgs e)
        {
            DataGridView tempDGV = (DataGridView)obj;
            int device = Convert.ToInt32(tempDGV.Name);

            if (e.RowIndex != -1)
            {
                tempDGV.CurrentCell = tempDGV.Rows[e.RowIndex].Cells[0];
            }

            if (e.ColumnIndex.ToString() != "0" && e.RowIndex.ToString() != "-1")
            {
                if (SorteringLoadtoDeviceNameTemp == -1 || SorteringLoadtoDeviceSlotTemp == -1)
                {
                    MessageBox.Show("No source data selected");
                    return;
                }
                if (SlotData[device][e.RowIndex] != 0 || tempDGV.Rows[e.RowIndex].Cells[2].Style.BackColor == Color.Gray)
                {
                    MessageBox.Show("Slot has data or dataError");
                    return;
                }

                if (tempDGV.Rows[e.RowIndex].Cells[3].Value != null)
                {
                    if (!string.IsNullOrEmpty(tempDGV.Rows[e.RowIndex].Cells[3].Value.ToString()))
                    {
                        MessageBox.Show("Slot already has other source data");
                        return;
                    }
                }

                string LoadDevice = string.Format("{0}", (IOLPDevice)SorteringLoadtoDeviceNameTemp);
                string UnLoadDevice = string.Format("{0}", (IOLPDevice)device);
                int refSorteringLoadtoDeviceSlotTemp = SorteringLoadtoDeviceSlotTemp - 1;
                int DeviceSlot = (e.RowIndex + 1);

                tempDGV.Rows[e.RowIndex].Cells[2].Value = LoadDevice;
                tempDGV.Rows[e.RowIndex].Cells[3].Value = SorteringLoadtoDeviceSlotTemp;
                tempDGV.Rows[e.RowIndex].Cells[1].Style.BackColor = Color.LightGreen;

                int lastrow = dgvAutoSelectView.Rows.Count;

                dgvAutoSelectView.Rows.Insert(lastrow);
                dgvAutoSelectView.Rows[lastrow].Cells[0].Value = LoadDevice;
                dgvAutoSelectView.Rows[lastrow].Cells[1].Value = SorteringLoadtoDeviceSlotTemp;
                dgvAutoSelectView.Rows[lastrow].Cells[2].Value = UnLoadDevice;
                dgvAutoSelectView.Rows[lastrow].Cells[3].Value = DeviceSlot;
                dgvAutoSelectView.CurrentCell = dgvAutoSelectView.Rows[lastrow].Cells[0];
                dgvAutoSelectView.Show();

                SlotDGV[SorteringLoadtoDeviceNameTemp].Rows[refSorteringLoadtoDeviceSlotTemp].Cells[3].Value = UnLoadDevice;
                SlotDGV[SorteringLoadtoDeviceNameTemp].Rows[refSorteringLoadtoDeviceSlotTemp].Cells[4].Value = DeviceSlot;
                SlotDGV[SorteringLoadtoDeviceNameTemp].Rows[refSorteringLoadtoDeviceSlotTemp].Cells[2].Style.BackColor = Color.Gray;


                SorteringLoadtoDeviceNameTemp = -1;
                SorteringLoadtoDeviceSlotTemp = -1;

            }
        }

        #endregion

        #region Total

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
                //if(frmMain.EFEM_Mode == NormalStatic.Sortering)
                {
                    string DesDevice = dgvAutoSelectView.Rows[TotalMenuRow].Cells[2].Value.ToString();
                    int DesNo = (int)(IOLPDevice)Enum.Parse(typeof(IOLPDevice), DesDevice);
                    int DesSolt = Convert.ToInt32(dgvAutoSelectView.Rows[TotalMenuRow].Cells[3].Value.ToString()) - 1;
                    SlotDGV[DesNo].Rows[DesSolt].Cells[1].Style.BackColor = Color.White;
                    SlotDGV[DesNo].Rows[DesSolt].Cells[2].Value = "";
                    SlotDGV[DesNo].Rows[DesSolt].Cells[3].Value = "";
                    SlotData[DesNo][DesSolt] = 0;
                }

                string SocDevice = dgvAutoSelectView.Rows[TotalMenuRow].Cells[0].Value.ToString();
                int SorteringLoadtoDeviceNameTemp = (int)(IOLPDevice)Enum.Parse(typeof(IOLPDevice), SocDevice);
                int SocSolt = Convert.ToInt32(dgvAutoSelectView.Rows[TotalMenuRow].Cells[1].Value.ToString()) - 1;        

                SlotDGV[SorteringLoadtoDeviceNameTemp].Rows[SocSolt].Cells[0].Value = false;
                SlotDGV[SorteringLoadtoDeviceNameTemp].Rows[SocSolt].Cells[2].Style.BackColor = Color.LightGreen;
                SlotDGV[SorteringLoadtoDeviceNameTemp].Rows[SocSolt].Cells[3].Value = "";
                SlotDGV[SorteringLoadtoDeviceNameTemp].Rows[SocSolt].Cells[4].Value = "";
                SlotData[SorteringLoadtoDeviceNameTemp][SocSolt] = 1;


                dgvAutoSelectView.Rows.RemoveAt(TotalMenuRow);

                string condition = string.Format("{0}= '{1}' and {2}= {3}", WaferInforTableItem.SocPort, SocDevice, WaferInforTableItem.SocSlot, SocSolt);
                if (SQLite.ReadDataTable(SQLTable.PJ_Pool, condition) != null)
                {
                    SQLite.Delete(SQLTable.PJ_Pool, condition);
                }

            }
        }

        #endregion

        #region Control

        private void btnLoadToUnloadAll_Click(object sender, EventArgs e)
        {
            string UnLPSelect = cboMP.Text;
            int GetEnumNo = (int)(IOLPDevice)Enum.Parse(typeof(IOLPDevice), UnLPSelect);

            //===== Check =====
            if (LoadAllList == null || LoadAllList.Count == 0 || SlotData[GetEnumNo] == null)
            {
                return;
            }
            for (int idx = 0; idx < LoadAllList.Count; idx++)
            {
                if (SlotData[GetEnumNo][idx] == 1)
                {
                    MessageBox.Show(string.Format("{0} Slot {1} has account cannot be added", UnLPSelect, (LoadAllList[idx].Slot + 1)));
                    return;
                }
            }

            for (int idx = 0; idx < LoadAllList.Count; idx++)
            {
                int SocPNo = LoadAllList[idx].No;
                string SocName = string.Format("{0}", (IOLPDevice)SocPNo);
                int refSorteringLoadtoDeviceSlotTemp = LoadAllList[idx].Slot;
                int ShowSorteringLoadtoDeviceSlotTemp = refSorteringLoadtoDeviceSlotTemp + 1;
                string DesName = string.Format("{0}", UnLPSelect);

                int lastrow = dgvAutoSelectView.Rows.Count;
                dgvAutoSelectView.Rows.Insert(lastrow);
                dgvAutoSelectView.Rows[lastrow].Cells[0].Value = SocName;
                dgvAutoSelectView.Rows[lastrow].Cells[1].Value = ShowSorteringLoadtoDeviceSlotTemp;
                dgvAutoSelectView.Rows[lastrow].Cells[2].Value = DesName;
                dgvAutoSelectView.Rows[lastrow].Cells[3].Value = ShowSorteringLoadtoDeviceSlotTemp;
                dgvAutoSelectView.CurrentCell = dgvAutoSelectView.Rows[lastrow].Cells[0];

                SlotDGV[SocPNo].Rows[refSorteringLoadtoDeviceSlotTemp].Cells[3].Value = DesName;
                SlotDGV[SocPNo].Rows[refSorteringLoadtoDeviceSlotTemp].Cells[4].Value = ShowSorteringLoadtoDeviceSlotTemp;
                SlotDGV[SocPNo].Rows[refSorteringLoadtoDeviceSlotTemp].Cells[2].Style.BackColor = Color.Gray;
                SlotDGV[SocPNo].Rows[refSorteringLoadtoDeviceSlotTemp].Cells[0].Value = false;
                SlotDGV[GetEnumNo].Rows[refSorteringLoadtoDeviceSlotTemp].Cells[2].Value = SocName;
                SlotDGV[GetEnumNo].Rows[refSorteringLoadtoDeviceSlotTemp].Cells[3].Value = ShowSorteringLoadtoDeviceSlotTemp;
                SlotDGV[GetEnumNo].Rows[refSorteringLoadtoDeviceSlotTemp].Cells[1].Style.BackColor = Color.Yellow;
            }
            LoadAllList.Clear();
        }

        private void ChangeView(string Mode)
        {
            switch (Mode)
            {
                case NormalStatic.Sortering:
                    {
                        gbxMP.Visible = true;
                    }
                    break;
                case NormalStatic.Load:
                    {
                        btnLoadToUnloadAll.Visible = false;
                        gbxMP.Visible = false;
                    }
                    break;
            }
        }

        private void btnAutoRun_Click(object sender, EventArgs e)
        {
            if (dgvAutoSelectView.Rows.Count == 0)
            {
                return;
            }

            SQLite.Delete(SQLTable.PJ_Pool,"1=1");

            string InsertCom = "";
            for (int row = 0; row < dgvAutoSelectView.Rows.Count; row++)
            {
                string SocPort = dgvAutoSelectView.Rows[row].Cells[0].Value.ToString();
                string SorteringLoadtoDeviceSlotTemp = dgvAutoSelectView.Rows[row].Cells[1].Value.ToString();

               // if (frmMain.EFEM_Mode == NormalStatic.Sortering)
                {
                    string DesPort = dgvAutoSelectView.Rows[row].Cells[2].Value.ToString();
                    string DesSlot = dgvAutoSelectView.Rows[row].Cells[3].Value.ToString();
                    InsertCom += string.Format("Insert Into {0} ({1},{2},{3},{4},{5}) Values ('{6}','{7}',{8},'{9}') ;",
                                                SQLTable.PJ_Pool,
                                                frmMain.EFEM_Mode,
                                                WaferInforTableItem.SocPort,
                                                WaferInforTableItem.SocSlot,
                                                WaferInforTableItem.DesPort,
                                                WaferInforTableItem.DesSlot,
                                                WaferInforTableItem.PPID,
                                                SocPort,
                                                SorteringLoadtoDeviceSlotTemp,
                                                DesPort, 
                                                DesSlot);

                    UI.Operate(NormalStatic.Core, string.Format("Sortering Insert Wafer infor :{0}-{1}-{2}-{3}", SocPort, SorteringLoadtoDeviceSlotTemp, DesPort, DesSlot));
                }
                //else
                //{
                //    InsertCom += string.Format("Insert Into {0} ({1},{2},{3}) Values ('{4}','{5}');",
                //                                SQLTable.PJ_Pool,
                //                                frmMain.EFEM_Mode,
                //                                WaferInforTableItem.SocPort,
                //                                WaferInforTableItem.SocSlot,
                //                                SocPort, 
                //                                SorteringLoadtoDeviceSlotTemp);

                //    UI.Operate(NormalStatic.Core, string.Format("EQ Insert Wafer infor :{0}-{1}", SocPort, SorteringLoadtoDeviceSlotTemp));
                //}
            }

            SQLite.Multi_InsertWaferInfo(InsertCom);
            this.Close();
        }

        #endregion

    }
}
