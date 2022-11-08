using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Data;

namespace HirataMainControl
{
    public class SQLite
    {

        private static string ConnString;
        private static SQLiteConnection Conn;
       // private static SQLiteCommand Cmd;
       // private static SQLiteDataReader Dr;

        public static bool ConnOpen() 
        {
            try
            {
                ConnString = string.Format("{0}" + Application.StartupPath + @"\{1}","Data Source=" ,"Hirata.db");
                Conn = new SQLiteConnection(ConnString);
                Conn.Open();
                
                return true;
                
            }
            catch (Exception ex) 
            {
                MessageBox.Show(string.Format("SQLite Open Fail,Error Message{0}",ex.Message));
                return false;
            }
        }

        public static void CreatPassword() 
        {
            Conn = new SQLiteConnection(ConnString);
            Conn.SetPassword("Hirata28513754");
            Conn.Close();
        }

        #region Set

        public static void SetData(SQLTableItem Item, SQLTable table, string condition, string NewValue)
        {
            string CmdStr = "";
            try
            {
                CmdStr = string.Format("Update {0} Set {1} = {2} Where {3}", table, Item, NewValue, condition);
                SQLiteCommand Cmd = new SQLiteCommand(CmdStr, Conn);
                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                UI.Alarm(NormalStatic.SQL, ErrorList.AP_TryCatchError, string.Format("{0}-{1}", CmdStr, ex));
            }
        }

        public static void SetWaferData(WaferInforTableItem Item, string condition, string NewValue)
        {
            string CmdStr = "";
            try
            {
                CmdStr = string.Format("Update {0} Set {1} = {2} Where {3}", SQLTable.PJ_Pool, Item, NewValue, condition);
                SQLiteCommand Cmd = new SQLiteCommand(CmdStr, Conn);
                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                UI.Alarm(NormalStatic.SQL, ErrorList.AP_TryCatchError, string.Format("{0}-{1}", CmdStr, ex));
            }
        }


        public static void SetSwapTest(WaferInforTableItem Item1,WaferInforTableItem Item2, string condition)
        {
            string CmdStr = "";
            try
            {
                CmdStr = string.Format("Update {0} Set {1} = {2} ,{2} = {1} Where {3}", SQLTable.PJ_Pool, Item1, Item2, condition);
                SQLiteCommand Cmd = new SQLiteCommand(CmdStr, Conn);
                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                UI.Alarm(NormalStatic.SQL, ErrorList.AP_TryCatchError, string.Format("{0}-{1}", CmdStr, ex));
            }
        } 

        public static void Multi_SetData(string All) 
        {
            string CmdStr = "";
            try
            {
                CmdStr = "BEGIN TRANSACTION;";
                CmdStr += All;
                CmdStr += "COMMIT;";
                SQLiteCommand Cmd = new SQLiteCommand(CmdStr, Conn);
                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                UI.Alarm(NormalStatic.SQL, ErrorList.AP_TryCatchError, string.Format("{0}-{1}", CmdStr, ex));
            }
        }

        #endregion

        #region Read

        //public static int ReadDataTableValue(string table, string condition)
        //{
        //    string CmdStr = string.Format("Select * From {0} Where {1}", table, condition);
        //    SQLiteCommand Cmd = new SQLiteCommand(CmdStr, Conn);
        //    SQLiteDataAdapter DAR = new SQLiteDataAdapter(Cmd);
        //    DataTable tempDt = new DataTable();
        //    DAR.Fill(tempDt);
        //    return tempDt.Rows.Count;
        //}

        public static string ReadDataTableValue(WaferInforTableItem Item, SQLTable table, string condition)
        {
            string CmdStr = "";
            try
            {
                CmdStr = string.Format("Select {0} From {1} Where {2}", Item, table, condition);
                SQLiteCommand Cmd = new SQLiteCommand(CmdStr, Conn);
                object ExeScalar = Cmd.ExecuteScalar();

                if (ExeScalar != null)
                    return ExeScalar.ToString();
                else
                    return null;
            }
            catch (Exception ex)
            {
                UI.Alarm(NormalStatic.SQL, ErrorList.AP_TryCatchError, string.Format("{0}-{1}", CmdStr, ex));
                return null;
            }
        }

        public static DataTable ReadDataTableLimit(SQLTable table, string condition, int limit)
        {
            string CmdStr = string.Format("Select * From {0} Where {1} Limit {2}", table, condition, limit);
            SQLiteCommand Cmd = new SQLiteCommand(CmdStr, Conn);
            SQLiteDataAdapter DAR = new SQLiteDataAdapter(Cmd);
            DataTable tempDt = new DataTable();
            DAR.Fill(tempDt);
            return tempDt;
        }

        public static DataTable ReadDataTable(SQLTable table, string condition)
        {
            string CmdStr = string.Format("Select * From {0} Where {1}", table, condition);
            SQLiteCommand Cmd = new SQLiteCommand(CmdStr, Conn);
            SQLiteDataAdapter DAR = new SQLiteDataAdapter(Cmd);
            DataTable tempDt = new DataTable();
            DAR.Fill(tempDt);
            return tempDt;
        }

        public static DataTable ReadDataTableMulticol(SQLTable table, string[] col, string condition)
        {
            string CmdStr = "Select ";

            for (int i = 0; i < col.Length; i++)
            {
                CmdStr += string.Format("{0}", col[i]);

                if (i != col.Length - 1)
                {
                    CmdStr += ",";
                }
            }


            CmdStr += string.Format(" From {0} Where {1}", table, condition);
            SQLiteCommand Cmd = new SQLiteCommand(CmdStr, Conn);
            SQLiteDataAdapter DAR = new SQLiteDataAdapter(Cmd);
            DataTable tempDt = new DataTable();
            DAR.Fill(tempDt);
            return tempDt;
        }


        public static int ReadDataTableCount(SQLTable table, string condition)
        {
            string CmdStr = string.Format("Select * From {0} Where {1}", table, condition);
            SQLiteCommand Cmd = new SQLiteCommand(CmdStr, Conn);
            SQLiteDataAdapter DAR = new SQLiteDataAdapter(Cmd);
            DataTable tempDt = new DataTable();
            DAR.Fill(tempDt);
            return tempDt.Rows.Count;
        } 

        #endregion

        #region Truncate

        //public static void TruncateTable(string table)
        //{
        //    string CmdStr = "Truncate Table " + table;
        //    SQLiteCommand Cmd = new SQLiteCommand(CmdStr, Conn);
        //    Cmd.ExecuteNonQuery();
        //} 

        #endregion

        #region Insert
        //Account / Recipe
        public static void Insert(SQLTable table ,string Values)
        {
            string InsertCom = string.Format("Insert Into {0} Values {1}",table, Values);
            SQLiteCommand Cmd = new SQLiteCommand(InsertCom, Conn);
            Cmd.ExecuteNonQuery();
        }

        //WaferInfo
        public static void Multi_InsertWaferInfo(string Values)
        {
            string InsertCom = "BEGIN TRANSACTION;";
            InsertCom += Values;
            InsertCom += "COMMIT;";
            SQLiteCommand Cmd = new SQLiteCommand(InsertCom, Conn);
            Cmd.ExecuteNonQuery();
        }

        public static void CopyWaferInfoToHistory(SQLTable table, string condition)
        {
            string InsertCom = string.Format("Insert Into {0} Select * from {1} where {2}",
                                             SQLTable.PJ_History,
                                             table,
                                             condition
                                             );
            SQLiteCommand Cmd = new SQLiteCommand(InsertCom, Conn);
            Cmd.ExecuteNonQuery();
        }

        public static void LimitWaferInfoToHistory(SQLTable table, int count)
        {
            string InsertCom = string.Format("Delete from {0} where StartTime in (Select StartTime from {0} order by StartTime desc limit (Select count (StartTime) from {0}) offset {1})",
                                             table,
                                             count
                                             );
            SQLiteCommand Cmd = new SQLiteCommand(InsertCom, Conn);
            Cmd.ExecuteNonQuery();
        }
        #endregion

        #region Delete

        public static void Delete(SQLTable table, string condition)
        {
            string DeleteCom = string.Format("Delete from {0} where {1}", table, condition);
            SQLiteCommand Cmd = new SQLiteCommand(DeleteCom, Conn);
            Cmd.ExecuteNonQuery();
        }

        public static void Multi_DeleteWaferInfo(string condition)
        {
            string DeleteCom = "BEGIN TRANSACTION;";
            DeleteCom += condition;
            DeleteCom += "COMMIT;";
            SQLiteCommand Cmd = new SQLiteCommand(DeleteCom, Conn);
            Cmd.ExecuteNonQuery();
        }

        #endregion

    }

}
