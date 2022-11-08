using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace HirataMainControl
{

    #region LoadPort
    public class LoadPortInfo
    {
        public bool Busy = false;
        private string Device;

        public void LoadInit(int index)
        {
            Device = string.Format("{0}{1}", NormalStatic.LP, index);
            Busy = false;
        }
    }
    #endregion

    #region Wafer Info
    public struct Waferinfo
    {
        public string SocPort { get; set; }
        public int SocSlot { get; set; }
        public WaferStatus Status { get; set; }

        public void clear()
        {
            SocPort = string.Empty;
            SocSlot = 0;
            Status = WaferStatus.WithOut;
        }

        public Waferinfo ConvertedInfo(string ref_str)
        {
            Waferinfo temp = new Waferinfo();
            int tempint = -1;
            if (ref_str == "")
                temp.clear();
            else
            {
                string[] splite = ref_str.Split(',');
                temp.SocPort = splite[0];
                temp.SocSlot = Int32.TryParse(splite[1], out tempint) ? tempint : -1;
                temp.Status = (WaferStatus)Enum.Parse(typeof(WaferStatus), splite[2]);
            }
            return temp;
        }
    }
    #endregion

    #region Data Info
    //Joanne 20200930 Start Add
    public struct DataInfo
    {
        public string SocPort { get; set; }
        public int SocSlot { get; set; }
        public string DesPort { get; set; }
        public int DesSlot { get; set; }
        public WaferStatus Status { get; set; }
        public bool IsTransferComplete { get; set; }

        public void Clear()
        {
            SocPort = string.Empty;
            SocSlot = 0;
            DesPort = string.Empty;
            DesSlot = 0;
            Status = WaferStatus.WithOut;
            IsTransferComplete = false;
        }

        public DataInfo ConvertedDataInfo(string ref_str)
        {
            DataInfo temp = new DataInfo();
            int tempint = -1;

            if (ref_str == "")
            {
                temp.Clear();
            }
            else
            {
                string[] splite = ref_str.Split(',');
                temp.Status = (WaferStatus)Enum.Parse(typeof(WaferStatus), splite[0]);
                temp.SocPort = splite[1];
                temp.SocSlot = Int32.TryParse(splite[2], out tempint) ? tempint : -1;
                temp.DesPort = splite[3];
                temp.DesSlot = Int32.TryParse(splite[4], out tempint) ? tempint : -1;
            }

            return temp;
        }
    }
    //Joanne 20200930 End Add
    #endregion
}
