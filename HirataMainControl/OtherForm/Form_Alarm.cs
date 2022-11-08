using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace HirataMainControl
{
    public partial class Form_Alarm : Form
    {
        public Form_Alarm()
        {
            InitializeComponent();
            lvAlarmList.Items.Clear();
        }

        public void CallAlarm(string Time,string device ,string Code,string description , bool warning)
        {
         
            lvAlarmList.Items.Add(new ListViewItem(new[] { Time,device, Code, description }));

            if (warning)
                lvAlarmList.Items[lvAlarmList.Items.Count-1].ForeColor = Color.Blue;
            else
                lvAlarmList.Items[lvAlarmList.Items.Count-1].ForeColor = Color.Red;

            lvAlarmList.EnsureVisible(lvAlarmList.Items.Count - 1);
            lvAlarmList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent - 1);
            if (lvAlarmList.Items.Count > 0)
                this.Show();
        }

        public void ClearAlarmClick()
        {
            if (lvAlarmList.CheckedItems.Count == 0)
                return;


            while (lvAlarmList.CheckedItems.Count > 0)
            {
                lvAlarmList.CheckedItems[0].Remove();
            }
            if (lvAlarmList.Items.Count == 0)
            {
                chkSelectAll.Checked = false;
                this.Hide();
            }
     
        }

        private void SelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSelectAll.Checked)
                for (int i = 0; i < lvAlarmList.Items.Count; i++)
                    lvAlarmList.Items[i].Checked = true;
            else
                for (int i = 0; i < lvAlarmList.Items.Count; i++)
                    lvAlarmList.Items[i].Checked = false;
        }

        public void ClearAlarmEnable(bool Enable)
        {
            btnClearAlarm.Enabled = Enable;
            //btnBuzzerOff.Enabled = Enable;
        }

    }
}
