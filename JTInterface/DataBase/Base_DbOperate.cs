///////////////////////
/// ���ݿ������
///2011-4-18 ����
//////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Threading;
using System.Data.SqlClient;


namespace DataBase
{
    public delegate void OnErrorEventHandler(DbErrorEventArgs e);

    public class DbErrorEventArgs : EventArgs
    {
        public string ExtraMsg;
        public Exception Ex;
        /// <summary>
        /// Oh,My God!!!�����������ѽ�����������Ȼ���쳣�ˣ���ô��ѽ��ô��~~~
        /// </summary>
        /// <param name="extraMsg">�����쳣�ˣ��㻹��ʲô��˵�ģ���������˵��</param>
        /// <param name="ex">�Ҿ����쳣</param>
        public DbErrorEventArgs(string extraMsg, Exception ex)  
        {
            ExtraMsg = extraMsg;
            Ex = ex;
            throw Ex;
        }

    }
    /// <summary>
    /// ���ݿ���ʲ���࣬��װ�����ݲ����ĸ��ַ�����2011-4-18 ����
    /// </summary>
    /*abstract*/ public class Base_DbOperate
    {
        protected  Mutex m_Mutex = null;     //��

        protected bool m_IsOpen = false;    //����״̬

        protected string m_sDbConnectString = "";//�����ַ�

        protected OleDbConnection m_OleCon;    //���ݿ����Ӷ���   
                
        /// <summary>
        /// �����Ƿ��
        /// </summary>
        public bool IsOpen
        {
            get
            {
                return m_IsOpen;
            }            
        }

        /// <summary>
        /// ��������״̬
        /// </summary>
        public ConnectionState State
        {
            get
            {
                
                return m_OleCon.State;
            }
        }

        /// <summary>
        /// �������ݿ���
        /// </summary>
        public string Database
        {
            get
            {                
                return m_OleCon.Database;
            }
        }

        public string DataSource
        {
            get
            {
                return m_OleCon.DataSource;
            }
        }

        /// <summary>
        /// ���������ַ��������õ�ʱ����Զ��������ݿ�
        /// </summary>
        public string ConnectString
        {
            get
            {
                return m_sDbConnectString;
            }
            set
            {
                m_sDbConnectString = value;
                lock (this)
                {
                    m_Mutex.WaitOne();
                    try
                    {
                        this.Close();
                        this.Open();
                    }
                    catch (Exception Ex)
                    {
                        OnError("��������:" + value, Ex);                         
                    }
                    m_Mutex.ReleaseMutex();
                }
            }
        }
        /// <summary>
        /// ���캯������ʼ��һ��Ĭ�ϵ��߳�ͬ�� Mutex �����ʵ������ʼ��һ��OleDbConnection ����
        /// </summary>
        public Base_DbOperate()
        {            
            m_Mutex = new Mutex();

            m_OleCon = new OleDbConnection();   
        }
        /// <summary>
        /// ʹ�ô���������ַ�����ʼ��һ��OleDbConnection ʵ��
        /// </summary>
        /// <param name="ConnectStr"></param>
        public Base_DbOperate(string ConnectStr)
        {
            m_sDbConnectString = ConnectStr;
            m_Mutex = new Mutex();
            try
            {
                m_OleCon = new OleDbConnection(ConnectStr);
            }
            catch (OleDbException Ex)
            {
                m_IsOpen = false;
                OnError("COleDbOperate.Open(" + m_sDbConnectString + ")", Ex);
            } 
        }

        /// <summary>
        /// ����������Mutex.Close
        /// </summary>
        ~Base_DbOperate()
        {
            try
            {
                this.Close();
            }
            catch 
            {
                //�����������ˣ��ٳ��쳣զ��ѽ��ʲô������
            }
            m_Mutex.Close();             
        }

        public event OnErrorEventHandler OnErrorEvent;

        /// <summary>
        /// �����Ҫд��־��Ҫ�Լ�����,���ߴ���OnErrorEvent�¼�
        /// </summary>
        /// <param name="ExtraMsg">������Ϣ</param>
        /// <param name="Ex">�����쳣</param>
        virtual public void OnError(string ExtraMsg,Exception Ex)
        {
            //System.Windows.Forms.MessageBox.Show (Ex.Message + "---" + ExtraMsg);
            if (OnErrorEvent != null)
            {
                OnErrorEvent(new DbErrorEventArgs(ExtraMsg, Ex));
            }
        }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        public bool Open()
        {
            if(m_OleCon==null)
            {
                m_OleCon = new OleDbConnection(m_sDbConnectString);
            }

            try
            {
                if (m_OleCon.State == ConnectionState.Closed)
                {
                    m_OleCon.ConnectionString = this.m_sDbConnectString;
                    m_OleCon.Open();
                }
                else if (m_OleCon.State == ConnectionState.Broken)
                {
                    try
                    {
                        m_OleCon.Close();
                    }
                    catch
                    {
                    }
                    m_OleCon.ConnectionString = this.m_sDbConnectString;
                    m_OleCon.Open();
                }
                m_IsOpen = true;
            }
            catch (OleDbException Ex)
            {
                m_IsOpen = false;
                OnError("COleDbOperate.Open(" + m_sDbConnectString + ")",Ex);                
            }
            catch (InvalidOperationException Ex)
            {
                m_IsOpen = false;
                OnError("COleDbOperate.Open(" + m_sDbConnectString + "),InvalidOperation", Ex); 
                
            }
            catch (Exception Ex)
            {
                OnError("COleDbOperate.Open(" + m_sDbConnectString + ")", Ex);                  
            }                
            return m_IsOpen ;
        }
        /// <summary>
        /// �ر�����
        /// </summary>
        public void Close()
        {
            //if (m_OleCon != null)
            //{
            //    if (m_OleCon.State != ConnectionState.Closed)
            //    {
            //        m_OleCon.Close();
            //    }
            //}
            m_IsOpen = false;
        }

        /// <summary>
        /// ��ý����
        /// </summary>
        /// <param name="SqlText">��ѯ���</param>
        /// <returns>�������ʧ�ܷ���null</returns>
        public DataSet GetDataSet(string SqlText)
        {  
            Open();
            if (!IsOpen) return null;

            DataSet ds = new DataSet();

            lock (this)
            {
                System.Data.OleDb.OleDbDataAdapter dap = new System.Data.OleDb.OleDbDataAdapter(SqlText, m_OleCon);
                dap.SelectCommand.CommandTimeout = 300;//���ò�ѯ��ʱʱ��-��
                m_Mutex.WaitOne();
                try
                {
                    dap.Fill(ds);
                }
                catch (Exception Ex)
                {
                    OnError("COleDbOperate.GetDataSet(" + SqlText + ")", Ex);  
                }
                finally
                {
                    dap.Dispose();
                }
                m_Mutex.ReleaseMutex();
            }
            if (ds.Tables.Count > 0)
                return ds;
            else
                return null;
        }
        /*
        public bool Insert(DataTable dt,string tName) {
            Open();
            if (!IsOpen) return false;

            System.Data.OleDb.OleDbDataAdapter dap = new System.Data.OleDb.OleDbDataAdapter(SqlText, m_OleCon);
           
            dap.Fill

                return true;
        }
        */

        /// <summary>
        /// ��ý����
        /// </summary>
        /// <param name="SqlText">��ѯ���</param>
        /// <param name="prams">OleDbParameter��������</param>
        /// <returns>�ɹ����ؽ������ʧ�ܷ���null</returns>
        public DataSet GetDataSet(string SqlText, OleDbParameter[] prams)
        {            
            Open();
            if (!IsOpen) return null;

            DataSet ds = new DataSet();
            lock (this)
            {
                OleDbDataAdapter dap = CreateDataAdaper(SqlText, prams);
                //System.Data.OleDb.OleDbDataAdapter dap = new System.Data.OleDb.OleDbDataAdapter(SqlText, m_OleCon);
                m_Mutex.WaitOne();
                try
                {
                    dap.Fill(ds);
                }
                catch (Exception Ex)
                {
                    OnError("COleDbOperate.GetDataSet(" + SqlText + ")", Ex);                      
                }
                finally
                {
                    dap.Dispose();
                }
                m_Mutex.ReleaseMutex();
            }

            if (ds.Tables.Count > 0)
                return ds;
            else
                return null;
        }
        /// <summary>
        ///  ��ý����
        /// </summary>
        /// <param name="SqlText">��ѯ���</param>
        /// <param name="prams">OleDbParameter������list</param>
        /// <returns></returns>
        public DataSet GetDataSet(string SqlText, List<OleDbParameter> prams)
        {
            Open();
            if (!IsOpen) return null;

            DataSet ds = new DataSet();
            lock (this)
            {
                OleDbDataAdapter dap = CreateDataAdaper(SqlText, prams);
                //System.Data.OleDb.OleDbDataAdapter dap = new System.Data.OleDb.OleDbDataAdapter(SqlText, m_OleCon);
                m_Mutex.WaitOne();
                try
                {
                    dap.Fill(ds);
                }
                catch (Exception Ex)
                {
                    OnError("COleDbOperate.GetDataSet(" + SqlText + ")", Ex);
                }
                finally
                {
                    dap.Dispose();
                }
                m_Mutex.ReleaseMutex();
            }

            if (ds.Tables.Count > 0)
                return ds;
            else
                return null;
        }


        /// <summary>
        /// ȡ�����ݱ�ȡ��dateset�еĵ�һ��datetable
        /// </summary>
        /// <param name="SqlText">��ѯ���</param>
        /// <returns>�������ݱ�</returns>
        public DataTable GetDataTable(string SqlText)
        {
            DataSet ds = GetDataSet(SqlText);
            if (ds != null && ds.Tables.Count > 0) //&& ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            else
                return null;
        }

        /// <summary>
        /// ȡ�����ݱ�ȡ��dateset�еĵ�һ��datetable
        /// </summary>
        /// <param name="SqlText">��ѯ���</param>
        /// <param name="prams">OleDbParameter��������</param>
        /// <returns>�������ݱ�</returns>
        public DataTable GetDataTable(string SqlText, OleDbParameter[] prams)
        {
            DataSet ds = GetDataSet(SqlText, prams);
            if (ds != null && ds.Tables.Count > 0)// && ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }
        /// <summary>
        /// ȡ�����ݱ�ȡ��dateset�еĵ�һ��datetable
        /// </summary>
        /// <param name="SqlText">��ѯ���</param>
        /// <param name="prams">OleDbParameter����list</param>
        /// <returns>��ѯ����datetable</returns>
        public DataTable GetDataTable(string SqlText, List<OleDbParameter> prams)
        {
            DataSet ds = GetDataSet(SqlText, prams);
            if (ds != null && ds.Tables.Count > 0)// && ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        /// <summary>
        /// ִ��SQL���,����ִ�н��������
        /// </summary>
        /// <param name="SqlText">SQL���</param>
        /// <returns>��Ӱ������</returns>
        public bool ExecSqlToF(string SqlText)
        {
            bool iRet = false ;
            Open();
            if (!IsOpen) return false ;
            lock (this)
            {
                m_Mutex.WaitOne();

                System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand(SqlText, m_OleCon);

                try
                {
                    cmd.ExecuteNonQuery();
                    iRet = true;
                }
                catch (InvalidOperationException Ex)
                {
                    OnError("COleDbOperate.ExecSql(" + SqlText + "),InvalidOperation", Ex);
                }
                catch (OleDbException Ex)
                {
                    if (Ex.Errors.Count > 0)
                    {
                        string Msg = "";
                        Msg = string.Format("COleDbOperate.ExecSql({0}),���ݿ����{1},SQLState={2},˵��:{3};", SqlText, Ex.Errors[0].NativeError, Ex.Errors[0].SQLState, Ex.Errors[0].Message);
                        OnError(Msg, Ex);
                    }
                    else
                    {
                        OnError("COleDbOperate.ExecSql(" + SqlText + "),����ţ�" + Ex.ErrorCode, Ex);
                    }
                }
                catch (Exception Ex)
                {
                    OnError("COleDbOperate.ExecSql(" + SqlText + ")", Ex);
                }
                finally
                {
                    cmd.Dispose();
                }
                m_Mutex.ReleaseMutex();
            }
            return iRet;
        }

        /// <summary>
        /// ִ��SQL���
        /// </summary>
        /// <param name="SqlText">SQL���</param>
        /// <returns>��Ӱ������</returns>
        public int  ExecSql(string SqlText)
        {
           // cmd.CommandTimeout = 300;
            int iRet = 0;
            Open();
            if (!IsOpen) return 0;
            lock (this)
            {
                m_Mutex.WaitOne();

                System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand(SqlText, m_OleCon);
                cmd.CommandTimeout = 300;
                try
                {
                    iRet = cmd.ExecuteNonQuery();
                }
                catch (InvalidOperationException Ex)
                {
                    OnError("COleDbOperate.ExecSql(" + SqlText + "),InvalidOperation", Ex);
                }
                catch (OleDbException Ex)
                {
                    throw new Exception();
                    //if(Ex.Errors.Count>0)
                    //{
                        //string Msg="";
                       // Msg = string.Format("COleDbOperate.ExecSql({0}),���ݿ����{1},SQLState={2},˵��:{3};", SqlText, Ex.Errors[0].NativeError, Ex.Errors[0].SQLState, Ex.Errors[0].Message);
                        //OnError(Msg, Ex);
                    //}else
                    //{
                       // OnError("COleDbOperate.ExecSql(" + SqlText + "),����ţ�" + Ex.ErrorCode, Ex);
                    //}                    
                }
                catch (Exception Ex)
                {
                    OnError("COleDbOperate.ExecSql(" + SqlText + ")", Ex);
                }

                finally
                {
                    cmd.Dispose();
                }
                m_Mutex.ReleaseMutex();
            }
            return iRet;
        }
        /// <summary>
        /// ִ��SQL���
        /// </summary>
        /// <param name="SqlText">ִ��SQL���</param>
        /// <param name="prams">OleDbParameter��������</param>
        /// <returns>��Ӱ������</returns>
        public int ExecSql(string SqlText, OleDbParameter[] prams)
        {
            int iRet = 0;
            Open();
            if (!IsOpen) return 0;
            lock (this)
            {
                m_Mutex.WaitOne();

                OleDbCommand cmd = CreateCommand(SqlText, prams);

                //System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand(SqlText, m_OleCon);

                try
                {
                    iRet = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
                catch (InvalidOperationException Ex)
                {
                    OnError("COleDbOperate.ExecSql(" + SqlText + ")", Ex);
                    
                }
                catch (Exception Ex)
                {
                    OnError("COleDbOperate.ExecSql(" + SqlText + ")", Ex);
                }
                finally
                {
                    cmd.Dispose();
                }
                m_Mutex.ReleaseMutex();
            }
            return iRet;
        }

        /// <summary>
        /// ִ��SQL���
        /// </summary>
        /// <param name="SqlText">SQL���</param>
        /// <param name="prams">OleDbParameter������LIST</param>
        /// <returns>������Ӱ�������</returns>
        public int ExecSql(string SqlText, List<OleDbParameter> prams)
        {
            int iRet = 0;
            Open();
            if (!IsOpen) return 0;
            lock (this)
            {
                m_Mutex.WaitOne();
                
                OleDbCommand cmd = CreateCommand(SqlText, prams);

                //System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand(SqlText, m_OleCon);

                try
                {
                    iRet = cmd.ExecuteNonQuery();
                }
                catch (InvalidOperationException Ex)
                {
                    OnError("COleDbOperate.ExecSql(" + SqlText + ")", Ex);

                }
                catch (Exception Ex)
                {
                    OnError("COleDbOperate.ExecSql(" + SqlText + ")", Ex);
                }
                finally
                {
                    cmd.Dispose();
                }
                m_Mutex.ReleaseMutex();
            }
            return iRet;
        }


        /// <summary>
        /// �������ִ��SQL LIST
        /// </summary>
        /// <param name="SqlText">SQL����list</param>
        /// <returns>��Ӱ�������</returns>
        public bool  ExecSqlTransaction(List<string> SqlText)
        {
            bool  iRet = false;
            Open();
            if (!IsOpen) return false;
            lock (this)
            {
                m_Mutex.WaitOne();

                OleDbTransaction transaction = null;

                OleDbCommand cmd = new OleDbCommand ();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = m_OleCon;
                
                //System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand(SqlText, m_OleCon);                
                try
                {

                    transaction = m_OleCon.BeginTransaction(IsolationLevel.ReadUncommitted);

                    
                    //cmd.Connection = m_OleCon;
                    cmd.Transaction = transaction;

                    foreach (string SQL in SqlText)
                    {
                        cmd.CommandText = SQL;
                        if (transaction != null)  cmd.ExecuteNonQuery();
                    }

                    if (transaction != null) cmd.Transaction.Commit();
                    iRet = true;
                }
                catch (InvalidOperationException Ex)
                {
                    if (transaction != null)
                    {
                        OnError("COleDbOperate.ExecSql(" + SqlText + "),ִ��ʧ�����ݻع���", Ex);
                        cmd.Transaction.Rollback();
                    }
                    else
                    {
                        OnError("COleDbOperate.ExecSql(" + SqlText + "),��֧�ֲ�����������Ϊ�գ�", Ex);
                    }
                }
                catch (Exception Ex)
                {
                    if (transaction != null)
                    {
                        OnError("COleDbOperate.ExecSql(" + SqlText + "),ִ��ʧ�����ݻع���", Ex);
                        cmd.Transaction.Rollback();
                    }
                    else
                    {
                        OnError("COleDbOperate.ExecSql(" + SqlText + "),�����쳣��", Ex);
                    }
                }
                finally
                {
                    cmd.Dispose();
                }
                m_Mutex.ReleaseMutex();
            }
            return iRet;
        }


        /// <summary>
        /// �������ִ��SQL
        /// </summary>
        /// <param name="SqlText">SQL���</param>
        /// <param name="prams">��������</param>
        /// <returns>��Ӱ�������</returns>
        public int ExecSqlTransaction(string SqlText, List<OleDbParameter> prams)
        {
            int iRet = 0;
            Open();
            if (!IsOpen) return 0;
            lock (this)
            {
                m_Mutex.WaitOne();

                OleDbTransaction transaction = null;

                OleDbCommand cmd = CreateCommand(SqlText, prams);                

                //System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand(SqlText, m_OleCon);                
                try
                {
                    transaction = m_OleCon.BeginTransaction(IsolationLevel.ReadUncommitted);
                    //cmd.Connection = m_OleCon;
                    cmd.Transaction = transaction;

                    //cmd.Transaction.Begin();
                    if (transaction != null)  iRet = cmd.ExecuteNonQuery();
                    if (transaction != null)  cmd.Transaction.Commit();
                }
                catch (InvalidOperationException Ex)
                {                    
                    if (transaction != null)
                    {
                        OnError("COleDbOperate.ExecSql(" + SqlText + "),ִ��ʧ�����ݻع���", Ex);
                        cmd.Transaction.Rollback();
                    }
                    else
                    {
                        iRet = -1;
                        OnError("COleDbOperate.ExecSql(" + SqlText + "),��֧�ֲ�����������Ϊ�գ�", Ex);
                    }
                }
                catch (Exception Ex)
                {
                    if (transaction != null)
                    {
                        OnError("COleDbOperate.ExecSql(" + SqlText + "),ִ��ʧ�����ݻع���", Ex);
                        cmd.Transaction.Rollback();
                    }
                    else
                    {
                        OnError("COleDbOperate.ExecSql(" + SqlText + "),�����쳣��", Ex);
                    }
                }
                finally
                {                    
                    cmd.Dispose();
                }
                m_Mutex.ReleaseMutex();
            }
            return iRet;
        }

        /// <summary>
        /// �������ִ��SQL
        /// </summary>
        /// <param name="SqlText">SQL���</param>
        /// <returns>��Ӱ�������</returns>
        public int ExecSqlTransaction(string SqlText)
        {
            int iRet = 0;
            Open();
            if (!IsOpen) return 0;
            lock (this)
            {
                m_Mutex.WaitOne();

                OleDbTransaction transaction = null;

                OleDbCommand cmd = new OleDbCommand(SqlText, m_OleCon);
                cmd.CommandType = CommandType.Text;�������� //ִ�����ͣ������ı�

                //System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand(SqlText, m_OleCon);                
                try
                {
                    transaction = m_OleCon.BeginTransaction(IsolationLevel.ReadUncommitted);
                    //cmd.Connection = m_OleCon;
                    cmd.Transaction = transaction;

                    //cmd.Transaction.Begin();
                    if (transaction != null) iRet = cmd.ExecuteNonQuery();
                    if (transaction != null) cmd.Transaction.Commit();
                }
                catch (InvalidOperationException Ex)
                {
                    if (transaction != null)
                    {
                        //OnError("COleDbOperate.ExecSql(" + SqlText + "),ִ��ʧ�����ݻع���", Ex);
                        cmd.Transaction.Rollback();
                        throw Ex;
                    }
                    else
                    {
                        iRet = -1;
                        OnError("COleDbOperate.ExecSql(" + SqlText + "),��֧�ֲ�����������Ϊ�գ�", Ex);
                    }
                }
                catch (Exception Ex)
                {
                    if (transaction != null)
                    {
                        OnError("COleDbOperate.ExecSql(" + SqlText + "),ִ��ʧ�����ݻع���", Ex);
                        cmd.Transaction.Rollback();
                        throw Ex;
                    }
                    else
                    {
                        OnError("COleDbOperate.ExecSql(" + SqlText + "),�����쳣��", Ex);
                    }
                }
                finally
                {
                    cmd.Dispose();
                }
                m_Mutex.ReleaseMutex();
            }
            return iRet;
        }

        /// <summary>
        /// ���ز�ѯ����ĵ�һ�е�һ�У����������л���
        /// </summary>
        /// <param name="SqlText">SQL��ѯ���</param>
        /// <returns>�п��ܷ���DBNULL</returns>
        public object ExecuteScalar(string SqlText)
        {
            object oRet = null;
            Open();
            if (!IsOpen) return null;
            lock (this)
            {
                m_Mutex.WaitOne();
                System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand(SqlText, m_OleCon);
                try
                {
                    oRet = cmd.ExecuteScalar();
                }
                catch (InvalidOperationException Ex)
                {
                    OnError("COleDbOperate.ExecuteScalar(" + SqlText + ")", Ex);                    
                }
                catch (Exception Ex)
                {
                    OnError("COleDbOperate.ExecuteScalar(" + SqlText + ")", Ex);
                }
                finally
                {
                    cmd.Dispose();
                }

                m_Mutex.ReleaseMutex();
            }
            
            return oRet;
        }
        /// <summary>
        /// ���ز�ѯ����ĵ�һ�е�һ�У����������л���
        /// </summary>
        /// <param name="SqlText">��ѯ���</param>
        /// <param name="prams">OleDbParameter��������</param>
        /// <returns>�п��ܷ���DBNULL</returns>
        public object ExecuteScalar(string SqlText, OleDbParameter[] prams)
        {
            object oRet = null;
            Open();
            if (!IsOpen) return null;
            lock (this)
            {
                m_Mutex.WaitOne();
                OleDbCommand cmd = CreateCommand(SqlText, prams);
                
                try
                {
                    oRet = cmd.ExecuteScalar();
                }
                catch (InvalidOperationException Ex)
                {
                    OnError("COleDbOperate.ExecuteScalar(" + SqlText + ")", Ex);
                }
                catch (Exception Ex)
                {
                    OnError("COleDbOperate.ExecuteScalar(" + SqlText + ")", Ex);
                }
                finally
                {
                    cmd.Dispose();
                }

                m_Mutex.ReleaseMutex();
            }

            return oRet;
        }

        /// <summary>
        /// ���ز�ѯ����ĵ�һ�е�һ�У����������л���
        /// </summary>
        /// <param name="SqlText">��ѯ���</param>
        /// <param name="prams">OleDbParameter����List</param>
        /// <returns>�п��ܷ���DBNULL</returns>
        public object ExecuteScalar(string SqlText, List<OleDbParameter> prams)
        {
            object oRet = null;
            Open();
            if (!IsOpen) return null;
            lock (this)
            {
                m_Mutex.WaitOne();
                OleDbCommand cmd = CreateCommand(SqlText, prams);

                try
                {
                    oRet = cmd.ExecuteScalar();
                }
                catch (InvalidOperationException Ex)
                {
                    OnError("COleDbOperate.ExecuteScalar(" + SqlText + ")", Ex);
                }
                catch (Exception Ex)
                {
                    OnError("COleDbOperate.ExecuteScalar(" + SqlText + ")", Ex);
                }
                finally
                {
                    cmd.Dispose();
                }

                m_Mutex.ReleaseMutex();
            }

            return oRet;
        }
        
        /// <summary>
        /// ִ�д�IDENTITY�Ĳ���SQL,�ú������Sql Server
        /// </summary>
        /// <param name="SqlText"></param>
        /// <returns>�����б�ʶIDENTITY</returns>
        public long ExecIdentityInsert(string SqlText)
        {
            long lRet = 0;
            object oRet = null;
            string InsertSql = string.Format("SET NOCOUNT ON;{0};SELECT @@IDENTITY",SqlText);

            Open();
            if (!IsOpen) return 0;

            lock (this)
            {
                m_Mutex.WaitOne();
                System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand(InsertSql, m_OleCon);
                try
                {
                    oRet = cmd.ExecuteScalar();
                }
                catch (InvalidOperationException Ex)
                {
                    OnError("COleDbOperate.ExecIdentityInsert(" + InsertSql + ")", Ex);                    
                }
                catch (Exception Ex)
                {
                    OnError("COleDbOperate.ExecIdentityInsert(" + InsertSql + ")", Ex); 
                }
                finally
                {
                    cmd.Dispose();
                }

                m_Mutex.ReleaseMutex();
            }

            try
            {
                lRet = Convert.ToInt64(oRet);
            }
            catch
            {
            }
            return lRet;
        }

        /// <summary>
        /// ִ�д�IDENTITY�Ĳ���SQL,�ú������Sql Server
        /// </summary>
        /// <param name="SqlText">SQL���</param>
        /// <param name="prams">����</param>
        /// <returns>�����б�ʶIDENTITY</returns>
        public long ExecIdentityInsert(string SqlText, List<OleDbParameter> prams)
        {
            long lRet = 0;
            object oRet = null;
            string InsertSql = string.Format("SET NOCOUNT ON;{0};SELECT @@IDENTITY", SqlText);

            Open();
            if (!IsOpen) return 0;

            lock (this)
            {
                m_Mutex.WaitOne();
                System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand(InsertSql, m_OleCon);

                // ���ΰѲ������������ı�
                if (prams != null)
                {
                    foreach (OleDbParameter parameter in prams)
                        cmd.Parameters.Add(parameter);
                }

                try
                {
                    oRet = cmd.ExecuteScalar();
                }
                catch (InvalidOperationException Ex)
                {
                    OnError("COleDbOperate.ExecIdentityInsert(" + InsertSql + ")", Ex);
                }
                catch (Exception Ex)
                {
                    OnError("COleDbOperate.ExecIdentityInsert(" + InsertSql + ")", Ex);
                }
                finally
                {
                    cmd.Dispose();
                }

                m_Mutex.ReleaseMutex();
            }

            try
            {
                lRet = Convert.ToInt64(oRet);
            }
            catch
            {
            }
            return lRet;
        }


        /// <summary>
        /// ִ�д洢����
        /// </summary>
        /// <param name="procName">������</param>
        /// <returns>����ִ���Ƿ���ȷ�Ľ��</returns>
        public bool ExecProc(string procName)
        {            
            Open();
            if (!IsOpen) return false;

            bool bRet = false;
            lock (this)
            {
                m_Mutex.WaitOne();
                OleDbCommand cmd = new OleDbCommand(procName, m_OleCon);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    cmd.ExecuteNonQuery();
                    bRet = true;
                }
                catch (InvalidOperationException Ex)
                {
                    OnError("COleDbOperate.ExecProc(" + procName + ")", Ex);                    
                }
                catch (Exception Ex)
                {
                    OnError("COleDbOperate.ExecProc(" + procName + ")", Ex);                     
                }
                finally
                {
                    cmd.Dispose();
                }
                m_Mutex.ReleaseMutex();
            }

            return bRet;
        }
        /// <summary>
        /// ִ�д洢����
        /// </summary>
        /// <param name="procName">�洢������</param>
        /// <param name="prams">��������</param>
        /// <returns>ִ�н��bool</returns>
        public bool ExecProc(string procName, OleDbParameter[] prams)
        {
            bool bRet = false;
            Open();
            if (!IsOpen) return false;
            lock (this)
            {   
                m_Mutex.WaitOne();
                OleDbCommand cmd = new OleDbCommand(procName, m_OleCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 300;
                cmd.Parameters.AddRange(prams);

                try
                {
                    cmd.ExecuteNonQuery();
                    bRet = true;
                }
                catch (Exception er) {
                    throw er;
                }
                //catch (InvalidOperationException Ex)
                //{
                //    OnError("COleDbOperate.ExecProc(" + procName + ")", Ex);                    
                //}
                //catch (Exception Ex)
                //{
                //    OnError("COleDbOperate.ExecProc(" + procName + ")", Ex); 
                //}
                finally
                {
                    cmd.Dispose();
                }
                m_Mutex.ReleaseMutex();
            }

            return bRet;
        }

        /// <summary>
        /// �洢����
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="prams"></param>
        /// <param name="tbName"></param>
        /// <returns></returns>
        public DataSet ExecProc(string procName, OleDbParameter[] prams, string tbName)
        {
            DataSet ds = new DataSet();
            Open();
            if (!IsOpen) return null;

            lock (this)
            {
                m_Mutex.WaitOne();
                OleDbDataAdapter dap = CreateDataAdaper(procName, prams);
                dap.SelectCommand.CommandType = CommandType.StoredProcedure;

                try
                {
                    dap.Fill(ds, tbName);
                }
                catch (InvalidOperationException Ex)
                {
                    OnError("COleDbOperate.ExecProc(" + procName + ")", Ex); 
                }
                catch (Exception Ex)
                {
                    OnError("COleDbOperate.ExecProc(" + procName + ")", Ex); 
                }
                finally
                {
                    dap.Dispose();
                }
                m_Mutex.ReleaseMutex();
            }

            return ds;
        }

        #region ���������ı�����OleDbDataAdapter
        /// <summary>
        /// ����һ��OleDbDataAdapter�����Դ���ִ�������ı�
        /// </summary>
        /// <param name="SqlText">�����ı�</param>
        /// <param name="prams">��������</param>
        /// <returns>OleDbDataAdapter</returns>
        private OleDbDataAdapter CreateDataAdaper(string SqlText, OleDbParameter[] prams)
        {
            OleDbDataAdapter dap = new OleDbDataAdapter(SqlText, m_OleCon);
            dap.SelectCommand.CommandType = CommandType.Text;  //ִ�����ͣ������ı�
            if (prams != null)
            {
                foreach (OleDbParameter parameter in prams)
                    dap.SelectCommand.Parameters.Add(parameter);
            }       

            return dap;
        }

        private OleDbDataAdapter CreateDataAdaper(string SqlText, List<OleDbParameter> prams)
        {
            OleDbDataAdapter dap = new OleDbDataAdapter(SqlText, m_OleCon);
            dap.SelectCommand.CommandType = CommandType.Text;  //ִ�����ͣ������ı�
            if (prams != null)
            {
                foreach (OleDbParameter parameter in prams)
                    dap.SelectCommand.Parameters.Add(parameter);
            }

            return dap;
        }

        #endregion

        #region   ���������ı�����OleDbCommand
        /// <summary>
        /// ����һ��OleDbCommand�����Դ���ִ�������ı�
        /// </summary>
        /// <param name="SqlText">�����ı�</param>
        /// <param name="prams"�����ı��������</param>
        /// <returns>����OleDbCommand����</returns>
        private OleDbCommand CreateCommand(string SqlText, OleDbParameter[] prams)
        {
            OleDbCommand cmd = new OleDbCommand(SqlText, m_OleCon);
            cmd.CommandType = CommandType.Text;�������� //ִ�����ͣ������ı�

            // ���ΰѲ������������ı�
            if (prams != null)
            {
                foreach (OleDbParameter parameter in prams)
                    cmd.Parameters.Add(parameter);
            }            
            return cmd;
        }

        private OleDbCommand CreateCommand(string SqlText, List<OleDbParameter> prams)
        {
            OleDbCommand cmd = new OleDbCommand(SqlText, m_OleCon);
            cmd.CommandType = CommandType.Text;�������� //ִ�����ͣ������ı�

            // ���ΰѲ������������ı�
            if (prams != null)
            {
                foreach (OleDbParameter parameter in prams)
                    cmd.Parameters.Add(parameter);
            }
            return cmd;
        }
        private OleDbCommand CreateCommand(string SqlText, Collection<OleDbParameter> prams)
        {
            OleDbCommand cmd = new OleDbCommand(SqlText, m_OleCon);
            cmd.CommandType = CommandType.Text;�������� //ִ�����ͣ������ı�

            // ���ΰѲ������������ı�
            if (prams != null)
            {
                foreach (OleDbParameter parameter in prams)
                    cmd.Parameters.Add(parameter);
            }
            return cmd;
        }
        #endregion

        #region   �����������ת��ΪOleDbParameter����
        /// <summary>
        /// ת������
        /// </summary>
        /// <param name="ParamName">��������</param>
        /// <param name="DbType">��������</param></param>
        /// <param name="Size">������С</param>
        /// <param name="Value">����ֵ</param>
        /// <returns>�µ� parameter ����</returns>
        public OleDbParameter MakeInParam(string ParamName, OleDbType DbType, int Size, object Value)
        {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Input, Value);
        }

        /// <summary>
        /// ��ʼ������ֵ
        /// </summary>
        /// <param name="ParamName">��������</param>
        /// <param name="DbType">��������</param>
        /// <param name="Size">������С</param>
        /// <param name="Direction">��������</param>
        /// <param name="Value">����ֵ</param>
        /// <returns>�µ� parameter ����</returns>
        public OleDbParameter MakeParam(string ParamName, OleDbType DbType, Int32 Size, ParameterDirection Direction, object Value)
        {
            OleDbParameter param;

            if (Size > 0)
                param = new OleDbParameter(ParamName, DbType, Size);
            else
                param = new OleDbParameter(ParamName, DbType);

            param.Direction = Direction;
            if (!(Direction == ParameterDirection.Output && Value == null))
                param.Value = Value;
            return param;
        }
        #endregion

        #region   ��ʼ�����ز���
        /// <summary>
        /// ��ʼ�����ز���
        /// </summary>
        /// <returns>�µ� parameter ���󣬲�����Ϊ��ReturnValue��</returns>
        public OleDbParameter MakeReturnParam()
        {
            OleDbParameter param = new OleDbParameter("ReturnValue", OleDbType.BigInt, 8,
                ParameterDirection.ReturnValue, false, 0, 0,
                string.Empty, DataRowVersion.Default, null);
            return param;
        }

        /// <summary>
        /// ��ʼ�����ز���
        /// </summary>
        /// <param name="ParamName">�洢�������ƻ������ı�</param>
        /// <param name="DbType">��������</param>
        /// <param name="Size">������С</param>
        /// <returns>�µ� parameter ����</returns>
        public OleDbParameter MakeReturnParam(string ParamName, OleDbType DbType, Int32 Size)
        {
            //
            OleDbParameter param = new OleDbParameter(ParamName, DbType, Size,
                ParameterDirection.ReturnValue, false, 0, 0,
                string.Empty, DataRowVersion.Default, null);
            return param;
        }
        #endregion

        /// <summary>
        /// ִ�����ݿ���������ǰĿ¼�������DBUpdate.sql�ļ���ִ�����ɾ��DBUpdate.sql�ļ�
        /// </summary>
        public void ExecUpdate()
        {
            //�������ַ�����ͨ������AutoTranslate=no�ر��ַ��Զ�ת�����ܣ�ʹ�����ı�����ʱ������ת���������΢���ṩ��һ�����������
            //SQL��������ʱ�������⣬SET ANSI_DEFAULTS ON;Update tWarehouse Set Name=N'��̩��1' where ID=1;SET ANSI_DEFAULTS OFF
            if (System.IO.File.Exists("DBUpdate.sql"))
            {
                StreamReader streamReader = new StreamReader("DBUpdate.sql");

                if (streamReader != null)
                {
                    string strSQLText = streamReader.ReadToEnd();

                    streamReader.Close();

                    if (!string.IsNullOrEmpty(strSQLText))
                       this.ExecSql(strSQLText);

                }
                //���ɾ���ļ�
                System.IO.File.Delete("DBUpdate.sql");
            }
        }

        public void ReInitDb()
        {
            if (System.IO.File.Exists("ReInit.sql"))
            {
                StreamReader streamReader = new StreamReader("ReInit.sql");

                if (streamReader != null)
                {
                    do
                    {
                        try
                        {
                            string strSQLText = streamReader.ReadLine();

                            if (!string.IsNullOrEmpty(strSQLText))
                                this.ExecSql(strSQLText);
                        }
                        catch
                        {
                            break;
                        }
                    } while (!streamReader.EndOfStream);


                    streamReader.Close();
                }
                //���ɾ���ļ�
                System.IO.File.Delete("ReInit.sql");
            }
        }
    }
}
