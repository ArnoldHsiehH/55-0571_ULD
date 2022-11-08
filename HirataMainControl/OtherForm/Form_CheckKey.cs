using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Management;
using System.Security.Cryptography;
using System.IO;


namespace HirataMainControl
{

    public partial class Form_CheckKey : Form
    {
        private const string HirataAPIKey = "HirataMainKey";
        private string IniKey;

        public delegate void CheckKeyEvent();
        public event CheckKeyEvent CloseEvent;  

        public bool CheckFlag = false;

        public Form_CheckKey()
        {
            InitializeComponent();
        }

        public void Form_CheckKey_Load(object sender, EventArgs e)
        {

            txtSerial.Text = GetCPUSerialNo();
            IniKey = AppSetting.LoadSetting(HirataAPIKey, " ");
            rtfPassKey.Text = IniKey;
            if (IniKey != " ")
            {
                if (CheckPassKey(IniKey))
                {
                    CheckFlag = true;
                }
                else
                {
                    this.Show();
                }
            }
        }

        private void btnCkeck_Click(object sender, EventArgs e)
        {
            string PassKey = rtfPassKey.Text.Trim();
            if (CheckPassKey(PassKey))
            {
                MessageBox.Show("Check key 'Success'",
                                "Success",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                AppSetting.SaveSetting(HirataAPIKey, PassKey);
                CloseForm();
            }
            else
            {
                MessageBox.Show("Check key 'Error'",
                                     "Error",
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Warning);
            }          
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            CheckFlag = false;
            CloseEvent();
        }

        private bool CheckPassKey(string _sPassKey)
        {
            if (_sPassKey.Length % 2 == 0 && _sPassKey.Length > 0)
            {
                Byte[] Passkey = new Byte[_sPassKey.Length / 2];
                int keyNo = 0;
                for (int i = 0; i < _sPassKey.Length - 1; i += 2)
                {
                    try
                    {
                        byte _tobyte = Convert.ToByte(_sPassKey.Substring(i, 2), 16);
                        Passkey[keyNo] = _tobyte;
                        keyNo++;
                    }
                    catch 
                    {
                        return false;
                    }
                }

                string Recervery = "";
                CspParameters param = new CspParameters();
                param.KeyContainerName = "Hirata";
                param.Flags = CspProviderFlags.UseMachineKeyStore;
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(param);
                try
                {
                    Byte[] arDecrypted = RSA.Decrypt(Passkey, false);
                    string _sDecrypt = "";
                    for (int i = 0; i < arDecrypted.Length; i++)
                    {
                        _sDecrypt = _sDecrypt + ((Convert.ToString(arDecrypted[i], 16)).PadLeft(2, '0'));
                    }
                    Recervery = _sDecrypt.ToUpper().Trim();
                    if (txtSerial.Text == Recervery && txtSerial.Text.Length > 0)
                    {
                        return true;//Success
                    }
                    else
                    {
                        return false;
                    }
                }
                catch 
                {
                    return false;
                }
            }
            return false;
        }

        private string GetCPUSerialNo()
        {
            string _replay = "";

            ManagementObjectSearcher wmiSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");

            foreach (ManagementObject obj in wmiSearcher.Get())
            {
                // 取得CPU 序號

                _replay = obj["ProcessorId"].ToString();
            }

            return _replay;
        }

        private void CloseForm()
        {
            CheckFlag = true;
            CloseEvent();
        }

    }
}
