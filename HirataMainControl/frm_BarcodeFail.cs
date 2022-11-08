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
    public partial class frm_BarcodeFail : Form
    {
        public string BarCodeValue = "";
        public frm_BarcodeFail()
        {
            InitializeComponent();
        }

        public void Initial() 
        {
            BarCodeValue = "";
            if (InvokeRequired) 
            {
                Invoke(new MethodInvoker(() =>
                {
                    textBox1.Text = "";
                }));
            }
            else 
            {
                textBox1.Text = "";
            }
        }

        private void btn_BarcodeEnter_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text)) 
            {
                MessageBox.Show("Not enter barcode");
                return;
            }
            if(MessageBox.Show(string.Format("Please check barcode vaule is {0}",textBox1.Text),"Warning",MessageBoxButtons.YesNo,MessageBoxIcon.Warning) == DialogResult.Yes) 
            {
                BarCodeValue = textBox1.Text;
                this.Hide();
            }
        }
    }
}
