///////////////////////
/// 数据库访问类
///2011-4-18 方烈
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
        /// Oh,My God!!!这是神马情况呀神马情况，竟然出异常了，怎么办呀怎么办~~~
        /// </summary>
        /// <param name="extraMsg">都出异常了，你还有什么想说的，就在这里说吧</param>
        /// <param name="ex">我就是异常</param>
        public DbErrorEventArgs(string extraMsg, Exception ex)  
        {
            ExtraMsg = extraMsg;
            Ex = ex;
            throw Ex;
        }

    }
    /// <summary>
    /// 数据库访问层基类，封装了数据操作的各种方法，2011-4-18 方烈
    /// </summary>
    /*abstract*/ public class Base_DbOperate
    {
        protected  Mutex m_Mutex = null;     //锁

        protected bool m_IsOpen = false;    //连接状态

        protected string m_sDbConnectString = "";//连接字符

        protected OleDbConnection m_OleCon;    //数据库连接对象   
                
        /// <summary>
        /// 返回是否打开
        /// </summary>
        public bool IsOpen
        {
            get
            {
                return m_IsOpen;
            }            
        }

        /// <summary>
        /// 返回连接状态
        /// </summary>
        public ConnectionState State
        {
            get
            {
                
                return m_OleCon.State;
            }
        }

        /// <summary>
        /// 返回数据库名
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
        /// 设置连接字符串，设置的时候会自动连接数据库
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
                        OnError("设置连接:" + value, Ex);                         
                    }
                    m_Mutex.ReleaseMutex();
                }
            }
        }
        /// <summary>
        /// 构造函数，初始化一个默认的线程同步 Mutex 类的新实例，初始化一个OleDbConnection 对象
        /// </summary>
        public Base_DbOperate()
        {            
            m_Mutex = new Mutex();

            m_OleCon = new OleDbConnection();   
        }
        /// <summary>
        /// 使用传入的连接字符串初始化一个OleDbConnection 实例
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
        /// 析构函数，Mutex.Close
        /// </summary>
        ~Base_DbOperate()
        {
            try
            {
                this.Close();
            }
            catch 
            {
                //都析构函数了，再出异常咋办呀？什么都不做
            }
            m_Mutex.Close();             
        }

        public event OnErrorEventHandler OnErrorEvent;

        /// <summary>
        /// 如果需要写日志需要自己重载,或者触发OnErrorEvent事件
        /// </summary>
        /// <param name="ExtraMsg">附加信息</param>
        /// <param name="Ex">错误异常</param>
        virtual public void OnError(string ExtraMsg,Exception Ex)
        {
            //System.Windows.Forms.MessageBox.Show (Ex.Message + "---" + ExtraMsg);
            if (OnErrorEvent != null)
            {
                OnErrorEvent(new DbErrorEventArgs(ExtraMsg, Ex));
            }
        }
        /// <summary>
        /// 打开连接
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
        /// 关闭连接
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
        /// 获得结果集
        /// </summary>
        /// <param name="SqlText">查询语句</param>
        /// <returns>结果集，失败返回null</returns>
        public DataSet GetDataSet(string SqlText)
        {  
            Open();
            if (!IsOpen) return null;

            DataSet ds = new DataSet();

            lock (this)
            {
                System.Data.OleDb.OleDbDataAdapter dap = new System.Data.OleDb.OleDbDataAdapter(SqlText, m_OleCon);
                dap.SelectCommand.CommandTimeout = 300;//设置查询超时时间-秒
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
        /// 获得结果集
        /// </summary>
        /// <param name="SqlText">查询语句</param>
        /// <param name="prams">OleDbParameter参数数组</param>
        /// <returns>成功返回结果集，失败返回null</returns>
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
        ///  获得结果集
        /// </summary>
        /// <param name="SqlText">查询语句</param>
        /// <param name="prams">OleDbParameter参数的list</param>
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
        /// 取得数据表，取出dateset中的第一个datetable
        /// </summary>
        /// <param name="SqlText">查询语句</param>
        /// <returns>返回数据表</returns>
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
        /// 取得数据表，取出dateset中的第一个datetable
        /// </summary>
        /// <param name="SqlText">查询语句</param>
        /// <param name="prams">OleDbParameter参数数组</param>
        /// <returns>返回数据表</returns>
        public DataTable GetDataTable(string SqlText, OleDbParameter[] prams)
        {
            DataSet ds = GetDataSet(SqlText, prams);
            if (ds != null && ds.Tables.Count > 0)// && ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }
        /// <summary>
        /// 取得数据表，取出dateset中的第一个datetable
        /// </summary>
        /// <param name="SqlText">查询语句</param>
        /// <param name="prams">OleDbParameter参数list</param>
        /// <returns>查询出的datetable</returns>
        public DataTable GetDataTable(string SqlText, List<OleDbParameter> prams)
        {
            DataSet ds = GetDataSet(SqlText, prams);
            if (ds != null && ds.Tables.Count > 0)// && ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        /// <summary>
        /// 执行SQL语句,返回执行结果的正误
        /// </summary>
        /// <param name="SqlText">SQL语句</param>
        /// <returns>受影响行数</returns>
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
                        Msg = string.Format("COleDbOperate.ExecSql({0}),数据库错误：{1},SQLState={2},说明:{3};", SqlText, Ex.Errors[0].NativeError, Ex.Errors[0].SQLState, Ex.Errors[0].Message);
                        OnError(Msg, Ex);
                    }
                    else
                    {
                        OnError("COleDbOperate.ExecSql(" + SqlText + "),错误号：" + Ex.ErrorCode, Ex);
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
        /// 执行SQL语句
        /// </summary>
        /// <param name="SqlText">SQL语句</param>
        /// <returns>受影响行数</returns>
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
                       // Msg = string.Format("COleDbOperate.ExecSql({0}),数据库错误：{1},SQLState={2},说明:{3};", SqlText, Ex.Errors[0].NativeError, Ex.Errors[0].SQLState, Ex.Errors[0].Message);
                        //OnError(Msg, Ex);
                    //}else
                    //{
                       // OnError("COleDbOperate.ExecSql(" + SqlText + "),错误号：" + Ex.ErrorCode, Ex);
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
        /// 执行SQL语句
        /// </summary>
        /// <param name="SqlText">执行SQL语句</param>
        /// <param name="prams">OleDbParameter参数数组</param>
        /// <returns>受影响行数</returns>
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
        /// 执行SQL语句
        /// </summary>
        /// <param name="SqlText">SQL语句</param>
        /// <param name="prams">OleDbParameter参数的LIST</param>
        /// <returns>返回受影响的行数</returns>
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
        /// 带事务的执行SQL LIST
        /// </summary>
        /// <param name="SqlText">SQL语句的list</param>
        /// <returns>受影响的行数</returns>
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
                        OnError("COleDbOperate.ExecSql(" + SqlText + "),执行失败数据回滚！", Ex);
                        cmd.Transaction.Rollback();
                    }
                    else
                    {
                        OnError("COleDbOperate.ExecSql(" + SqlText + "),不支持并行事务，事务为空！", Ex);
                    }
                }
                catch (Exception Ex)
                {
                    if (transaction != null)
                    {
                        OnError("COleDbOperate.ExecSql(" + SqlText + "),执行失败数据回滚！", Ex);
                        cmd.Transaction.Rollback();
                    }
                    else
                    {
                        OnError("COleDbOperate.ExecSql(" + SqlText + "),其他异常！", Ex);
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
        /// 带事务的执行SQL
        /// </summary>
        /// <param name="SqlText">SQL语句</param>
        /// <param name="prams">参数数组</param>
        /// <returns>受影响的行数</returns>
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
                        OnError("COleDbOperate.ExecSql(" + SqlText + "),执行失败数据回滚！", Ex);
                        cmd.Transaction.Rollback();
                    }
                    else
                    {
                        iRet = -1;
                        OnError("COleDbOperate.ExecSql(" + SqlText + "),不支持并行事务，事务为空！", Ex);
                    }
                }
                catch (Exception Ex)
                {
                    if (transaction != null)
                    {
                        OnError("COleDbOperate.ExecSql(" + SqlText + "),执行失败数据回滚！", Ex);
                        cmd.Transaction.Rollback();
                    }
                    else
                    {
                        OnError("COleDbOperate.ExecSql(" + SqlText + "),其他异常！", Ex);
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
        /// 带事务的执行SQL
        /// </summary>
        /// <param name="SqlText">SQL语句</param>
        /// <returns>受影响的行数</returns>
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
                cmd.CommandType = CommandType.Text;　　　　 //执行类型：命令文本

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
                        //OnError("COleDbOperate.ExecSql(" + SqlText + "),执行失败数据回滚！", Ex);
                        cmd.Transaction.Rollback();
                        throw Ex;
                    }
                    else
                    {
                        iRet = -1;
                        OnError("COleDbOperate.ExecSql(" + SqlText + "),不支持并行事务，事务为空！", Ex);
                    }
                }
                catch (Exception Ex)
                {
                    if (transaction != null)
                    {
                        OnError("COleDbOperate.ExecSql(" + SqlText + "),执行失败数据回滚！", Ex);
                        cmd.Transaction.Rollback();
                        throw Ex;
                    }
                    else
                    {
                        OnError("COleDbOperate.ExecSql(" + SqlText + "),其他异常！", Ex);
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
        /// 返回查询结果的第一行第一列，忽略其他行或列
        /// </summary>
        /// <param name="SqlText">SQL查询语句</param>
        /// <returns>有可能返回DBNULL</returns>
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
        /// 返回查询结果的第一行第一列，忽略其他行或列
        /// </summary>
        /// <param name="SqlText">查询语句</param>
        /// <param name="prams">OleDbParameter参数数组</param>
        /// <returns>有可能返回DBNULL</returns>
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
        /// 返回查询结果的第一行第一列，忽略其他行或列
        /// </summary>
        /// <param name="SqlText">查询语句</param>
        /// <param name="prams">OleDbParameter参数List</param>
        /// <returns>有可能返回DBNULL</returns>
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
        /// 执行带IDENTITY的插入SQL,该函数针对Sql Server
        /// </summary>
        /// <param name="SqlText"></param>
        /// <returns>返回行标识IDENTITY</returns>
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
        /// 执行带IDENTITY的插入SQL,该函数针对Sql Server
        /// </summary>
        /// <param name="SqlText">SQL语句</param>
        /// <param name="prams">参数</param>
        /// <returns>返回行标识IDENTITY</returns>
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

                // 依次把参数传入命令文本
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
        /// 执行存储过程
        /// </summary>
        /// <param name="procName">过程名</param>
        /// <returns>返回执行是否正确的结果</returns>
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
        /// 执行存储过程
        /// </summary>
        /// <param name="procName">存储过程名</param>
        /// <param name="prams">参数数组</param>
        /// <returns>执行结果bool</returns>
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
        /// 存储过程
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

        #region 根据命令文本生成OleDbDataAdapter
        /// <summary>
        /// 创建一个OleDbDataAdapter对象以此来执行命令文本
        /// </summary>
        /// <param name="SqlText">命令文本</param>
        /// <param name="prams">参数对象</param>
        /// <returns>OleDbDataAdapter</returns>
        private OleDbDataAdapter CreateDataAdaper(string SqlText, OleDbParameter[] prams)
        {
            OleDbDataAdapter dap = new OleDbDataAdapter(SqlText, m_OleCon);
            dap.SelectCommand.CommandType = CommandType.Text;  //执行类型：命令文本
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
            dap.SelectCommand.CommandType = CommandType.Text;  //执行类型：命令文本
            if (prams != null)
            {
                foreach (OleDbParameter parameter in prams)
                    dap.SelectCommand.Parameters.Add(parameter);
            }

            return dap;
        }

        #endregion

        #region   根据命令文本生成OleDbCommand
        /// <summary>
        /// 创建一个OleDbCommand对象以此来执行命令文本
        /// </summary>
        /// <param name="SqlText">命令文本</param>
        /// <param name="prams"命令文本所需参数</param>
        /// <returns>返回OleDbCommand对象</returns>
        private OleDbCommand CreateCommand(string SqlText, OleDbParameter[] prams)
        {
            OleDbCommand cmd = new OleDbCommand(SqlText, m_OleCon);
            cmd.CommandType = CommandType.Text;　　　　 //执行类型：命令文本

            // 依次把参数传入命令文本
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
            cmd.CommandType = CommandType.Text;　　　　 //执行类型：命令文本

            // 依次把参数传入命令文本
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
            cmd.CommandType = CommandType.Text;　　　　 //执行类型：命令文本

            // 依次把参数传入命令文本
            if (prams != null)
            {
                foreach (OleDbParameter parameter in prams)
                    cmd.Parameters.Add(parameter);
            }
            return cmd;
        }
        #endregion

        #region   传入参数并且转换为OleDbParameter类型
        /// <summary>
        /// 转换参数
        /// </summary>
        /// <param name="ParamName">参数名称</param>
        /// <param name="DbType">参数类型</param></param>
        /// <param name="Size">参数大小</param>
        /// <param name="Value">参数值</param>
        /// <returns>新的 parameter 对象</returns>
        public OleDbParameter MakeInParam(string ParamName, OleDbType DbType, int Size, object Value)
        {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Input, Value);
        }

        /// <summary>
        /// 初始化参数值
        /// </summary>
        /// <param name="ParamName">参数名称</param>
        /// <param name="DbType">参数类型</param>
        /// <param name="Size">参数大小</param>
        /// <param name="Direction">参数方向</param>
        /// <param name="Value">参数值</param>
        /// <returns>新的 parameter 对象</returns>
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

        #region   初始化返回参数
        /// <summary>
        /// 初始化返回参数
        /// </summary>
        /// <returns>新的 parameter 对象，参数名为“ReturnValue”</returns>
        public OleDbParameter MakeReturnParam()
        {
            OleDbParameter param = new OleDbParameter("ReturnValue", OleDbType.BigInt, 8,
                ParameterDirection.ReturnValue, false, 0, 0,
                string.Empty, DataRowVersion.Default, null);
            return param;
        }

        /// <summary>
        /// 初始化返回参数
        /// </summary>
        /// <param name="ParamName">存储过程名称或命令文本</param>
        /// <param name="DbType">参数类型</param>
        /// <param name="Size">参数大小</param>
        /// <returns>新的 parameter 对象</returns>
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
        /// 执行数据库升级，当前目录必须存在DBUpdate.sql文件，执行完后删除DBUpdate.sql文件
        /// </summary>
        public void ExecUpdate()
        {
            //在连接字符串中通过参数AutoTranslate=no关闭字符自动转换功能，使得中文被插入时不进行转换，这个是微软提供的一个解决方法。
            //SQL中有中文时会有问题，SET ANSI_DEFAULTS ON;Update tWarehouse Set Name=N'鑫泰佳1' where ID=1;SET ANSI_DEFAULTS OFF
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
                //最后删除文件
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
                //最后删除文件
                System.IO.File.Delete("ReInit.sql");
            }
        }
    }
}
