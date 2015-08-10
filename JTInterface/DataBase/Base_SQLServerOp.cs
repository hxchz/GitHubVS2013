using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.Sql;
using System.Data.OleDb;

namespace DataBase
{
    public class Base_SQLServerOp
    {
        /// <summary>
        /// �г�������������SQLServer������
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllSQLServers()
        {
            List<string> ret = new List<string>();
            SqlDataSourceEnumerator instance = SqlDataSourceEnumerator.Instance;
            DataTable table = instance.GetDataSources();
            foreach (DataRow dr in table.Rows)
            {
                if (dr["InstanceName"].ToString() != string.Empty)
                {
                    ret.Add(dr["ServerName"].ToString() + "\\" + dr["InstanceName"].ToString());
                }
                else
                {
                    ret.Add(dr["ServerName"].ToString());
                }
            }
            return ret;
        }
        /// <summary>
        /// ȡ�����ݷ��������������ݿ���
        /// </summary>
        /// <param name="login">��¼��Ϣ</param>
        /// <returns>���ݿ�������</returns>
        public List<string> GetAllDataBases(string connectString)
        {
            List<string> ret = new List<string>();
            Base_DbOperate DbOperate = new Base_DbOperate(connectString);
            if (!DbOperate.Open())
                return null;
            DataTable dt = DbOperate.GetDataTable("select name from Master.dbo.sysdatabases");
            foreach (DataRow dr in dt.Rows)
            {
                ret.Add(dr["name"].ToString());
            }
            return ret;
        }

        /// <summary>
        /// ȡ�����ݿ������еı���
        /// </summary>
        /// <param name="login">��¼</param>
        /// <returns>��������</returns>
        public List<string> getAllTables(string connectString)
        {
            List<string> ret = new List<string>();
            Base_DbOperate DbOperate = new Base_DbOperate(connectString);
            if (!DbOperate.Open())
                return null;
            DataTable dt = DbOperate.GetDataTable("select name from sysobjects where xtype = 'u' or xtype='v'");
            foreach (DataRow dr in dt.Rows)
            {
                ret.Add(dr["name"].ToString());
            }
            return ret;
        }

        /// <summary>
        /// ת�������Ӵ�,SQL��֤
        /// </summary>
        /// <param name="strServerName"></param>
        /// <param name="strDbName"></param>
        /// <param name="strUser"></param>
        /// <param name="strPwd"></param>
        /// <returns></returns>       
        public static string GetSQLServerOleConnectString(string strServerName, string strDbName, string strUser, string strPwd)
        {
            OleDbConnectionStringBuilder builder = new OleDbConnectionStringBuilder();
            builder.Clear();
            builder.Add("Data Source", strServerName);
            builder.Add("Initial Catalog", strDbName);
            //builder.Add("Integrated Security", login.winMode);
            builder.Add("User ID", strUser);
            builder.Add("Password", strPwd);
            builder.Add("Provider", "SQLOLEDB");
            //builder.Add("ConnectionTimeout", 120);//����������������ӳ�ʱʱ��,��λ-��
            return builder.ConnectionString;
        }
        /// <summary>
        /// Windows��֤
        /// </summary>
        /// <param name="strServerName"></param>
        /// <param name="strDbName"></param>
        /// <returns></returns>
        public static string GetSQLServerOleConnectString(string strServerName, string strDbName)
        {
            //Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=master;
            OleDbConnectionStringBuilder builder = new OleDbConnectionStringBuilder();
            builder.Clear();
            builder.Add("Data Source", strServerName);
            builder.Add("Initial Catalog", strDbName);
            builder.Add("Integrated Security", "SSPI");
            //builder.Add("Persist Security Info=False", "False");
            return builder.ConnectionString;
        }

        /// <summary>
        /// ����SQL Server���ݿ⣬SQL��֤
        /// </summary>
        /// <param name="strServerName">������</param>
        /// <param name="strDbName">���ݿ���</param>
        /// <param name="strUser">�û�</param>
        /// <param name="strPwd">����</param>
        /// <returns>true,false</returns>
        public static bool ShrinkSQLDatabase(string strServerName, string strDbName, string strUser, string strPwd)
        {
            bool bRet = false;
            System.Data.OleDb.OleDbConnection mDbConnect = new System.Data.OleDb.OleDbConnection();
            System.Data.OleDb.OleDbCommand mDbCommand = new System.Data.OleDb.OleDbCommand();
            mDbCommand.Connection = mDbConnect;

            mDbConnect.ConnectionString = "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=" + strUser + ";password=" + strPwd + ";Initial Catalog=master;Data Source=" + strServerName;

            mDbCommand.CommandText = String.Format("dbcc ShrinkDatabase('{0}',0)", strDbName);
            try
            {
                mDbConnect.Open();
                if (mDbCommand.ExecuteNonQuery() != 0)
                {
                    bRet = true;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
                //����ʲô
            }
            finally
            {
                mDbConnect.Close();
            }
            return bRet;
        }
        /// <summary>
        /// ����SQL Server���ݿ⣬Windows��֤
        /// </summary>
        /// <param name="strServerName"></param>
        /// <param name="strDbName"></param>
        /// <returns></returns> 
        public static bool ShrinkSQLDatabase(string strServerName, string strDbName)
        {
            bool bRet = false;
            System.Data.OleDb.OleDbConnection mDbConnect = new System.Data.OleDb.OleDbConnection();
            System.Data.OleDb.OleDbCommand mDbCommand = new System.Data.OleDb.OleDbCommand();
            mDbCommand.Connection = mDbConnect;

            mDbConnect.ConnectionString = "Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=master;Data Source=" + strServerName;

            mDbCommand.CommandText = String.Format("dbcc ShrinkDatabase('{0}',0)", strDbName);
            try
            {
                mDbConnect.Open();
                if (mDbCommand.ExecuteNonQuery() != 0)
                {
                    bRet = true;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
                //����ʲô
            }
            finally
            {
                mDbConnect.Close();
            }

            return bRet;
        }
        //BACKUP LOG LYFOAMANAGEDB WITH NO_LOG
        public static bool TruncationSQLDatabase(string strServerName, string strDbName, string strUser, string strPwd)
        {
            bool bRet = false;
            System.Data.OleDb.OleDbConnection mDbConnect = new System.Data.OleDb.OleDbConnection();
            System.Data.OleDb.OleDbCommand mDbCommand = new System.Data.OleDb.OleDbCommand();
            mDbCommand.Connection = mDbConnect;

            mDbConnect.ConnectionString = "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=" + strUser + ";password=" + strPwd + ";Initial Catalog=master;Data Source=" + strServerName;

            mDbCommand.CommandText = String.Format("BACKUP LOG {0} WITH NO_LOG", strDbName);
            try
            {
                mDbConnect.Open();
                if (mDbCommand.ExecuteNonQuery() != 0)
                {
                    bRet = true;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
                //����ʲô
            }
            finally
            {
                mDbConnect.Close();
            }
            return bRet;
        }

        public static bool TruncationSQLDatabase(string strServerName, string strDbName)
        {
            bool bRet = false;
            System.Data.OleDb.OleDbConnection mDbConnect = new System.Data.OleDb.OleDbConnection();
            System.Data.OleDb.OleDbCommand mDbCommand = new System.Data.OleDb.OleDbCommand();
            mDbCommand.Connection = mDbConnect;

            mDbConnect.ConnectionString = "Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=master;Data Source=" + strServerName;

            mDbCommand.CommandText = String.Format("BACKUP LOG {0} WITH NO_LOG", strDbName);
            try
            {
                mDbConnect.Open();
                if (mDbCommand.ExecuteNonQuery() != 0)
                {
                    bRet = true;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
                //����ʲô
            }
            finally
            {
                mDbConnect.Close();
            }
            return bRet;
        }



        /// <summary>
        /// ����SQL Server���ݿ⣬SQL��֤
        /// </summary>
        /// <param name="strServerName"></param>
        /// <param name="strDbName"></param>
        /// <param name="strUser"></param>
        /// <param name="strPwd"></param>
        /// <param name="BakPhyDevName"></param>
        /// <returns></returns>
 
        public static bool BackupSQLDatabase(string strServerName, string strDbName, string strUser, string strPwd, string BakPhyDevName)
        {
            bool bRet = false;
            System.Data.OleDb.OleDbConnection mDbConnect = new System.Data.OleDb.OleDbConnection();
            System.Data.OleDb.OleDbCommand mDbCommand = new System.Data.OleDb.OleDbCommand();
            mDbCommand.Connection = mDbConnect;

            mDbConnect.ConnectionString = "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=" + strUser + ";password=" + strPwd + ";Initial Catalog=master;Data Source=" + strServerName;

            mDbCommand.CommandText = String.Format("BACKUP DATABASE [{0}] TO  DISK = N'{1}' WITH  NOINIT ,  NOUNLOAD ,  NAME = N'{2}',  NOSKIP ,  STATS = 10,  NOFORMAT", strDbName, BakPhyDevName, strDbName + " Backup");
            try
            {
                mDbConnect.Open();
                if (mDbCommand.ExecuteNonQuery() != 0)
                {
                    bRet = true;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
                //����ʲô
            }
            finally
            {
                mDbConnect.Close();
            }
            return bRet;
        }

        /// <summary>
        /// ����SQL Server���ݿ⣬Windows��֤
        /// </summary>
        /// <param name="strServerName"></param>
        /// <param name="strDbName"></param>
        /// <param name="BakPhyDevName"></param>
        /// <returns></returns>
 
        public static bool BackupSQLDatabase(string strServerName, string strDbName, string BakPhyDevName)
        {
            bool bRet = false;
            System.Data.OleDb.OleDbConnection mDbConnect = new System.Data.OleDb.OleDbConnection();
            System.Data.OleDb.OleDbCommand mDbCommand = new System.Data.OleDb.OleDbCommand();
            mDbCommand.Connection = mDbConnect;

            mDbConnect.ConnectionString = "Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=master;Data Source=" + strServerName;

            mDbCommand.CommandText = String.Format("BACKUP DATABASE [{0}] TO  DISK = N'{1}' WITH  NOINIT ,  NOUNLOAD ,  NAME = N'{2}',  NOSKIP ,  STATS = 10,  NOFORMAT", strDbName, BakPhyDevName, strDbName + " Backup");
            try
            {
                mDbConnect.Open();
                if (mDbCommand.ExecuteNonQuery() != 0)
                {
                    bRet = true;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
                //����ʲô
            }
            finally
            {
                mDbConnect.Close();
            }

            return bRet;
        }

        /// <summary>
        /// ��ԭSQL Server���ݿ⣬SQL��֤
        /// </summary>
        /// <param name="strServerName"></param>
        /// <param name="strDbName"></param>
        /// <param name="strUser"></param>
        /// <param name="strPwd"></param>
        /// <param name="BakPhyDevName"></param>
        /// <returns></returns>
 
        public static bool RestoreSQLDatabase(string strServerName, string strDbName, string strUser, string strPwd, string BakPhyDevName)
        {
            bool bRet = false;
            System.Data.OleDb.OleDbConnection mDbConnect = new System.Data.OleDb.OleDbConnection();
            System.Data.OleDb.OleDbCommand mDbCommand = new System.Data.OleDb.OleDbCommand();
            mDbCommand.Connection = mDbConnect;

            mDbConnect.ConnectionString = "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=" + strUser + ";password=" + strPwd + ";Initial Catalog=master;Data Source=" + strServerName;

            mDbCommand.CommandText = String.Format("RESTORE DATABASE [{0}] FROM  DISK = N'{1}'", strDbName, BakPhyDevName);
            try
            {
                mDbConnect.Open();
                if (mDbCommand.ExecuteNonQuery() != 0)
                {
                    bRet = true;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
                //����ʲô
            }
            finally
            {
                mDbConnect.Close();
            }
            return bRet;
        }

        /// <summary>
        /// ��ԭSQL Server���ݿ⣬Windows��֤
        /// </summary>
        /// <param name="strServerName"></param>
        /// <param name="strDbName"></param>
        /// <param name="BakPhyDevName"></param>
        /// <returns></returns>
 
        public static bool RestoreSQLDatabase(string strServerName, string strDbName, string BakPhyDevName)
        {
            bool bRet = false;
            System.Data.OleDb.OleDbConnection mDbConnect = new System.Data.OleDb.OleDbConnection();
            System.Data.OleDb.OleDbCommand mDbCommand = new System.Data.OleDb.OleDbCommand();
            mDbCommand.Connection = mDbConnect;

            mDbConnect.ConnectionString = "Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=master;Data Source=" + strServerName;

            mDbCommand.CommandText = String.Format("RESTORE DATABASE [{0}] FROM  DISK = N'{1}'", strDbName, BakPhyDevName);
            try
            {
                mDbConnect.Open();
                if (mDbCommand.ExecuteNonQuery() != 0)
                {
                    bRet = true;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
                //����ʲô
            }
            finally
            {
                mDbConnect.Close();
            }

            return bRet;
        }

    }
}
