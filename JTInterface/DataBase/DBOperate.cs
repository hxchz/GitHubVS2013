using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Threading;

namespace DataBase
{
    /// <summary>
    /// 继承自数据库访问基类，扩展一些业务的数据库操作 2011-4-19 方烈
    /// </summary>
    public  class DBOperate : DataBase.Base_DbOperate
    {
        #region private变量
        /// <summary>
        /// 帐套号
        /// </summary>
        private String _AccId;
        #endregion

        #region 属性值

        public String DBServer
        {
            get
            {
                return m_OleCon.DataSource ;
            }
        }
        /// <summary>
        /// 帐套名 格式为 “数据库名称@数据库服务器名称”
        /// </summary>
        public String AccName
        {
            get
            {
                return m_OleCon.Database  + "@" + m_OleCon.DataSource;
            }
        }

        public String AccId
        {
            get
            {
                return _AccId;
            }
            set
            {
                _AccId = value;
            }
        }
        
        /// <summary>
        /// 单据独占操作时发生的错误信息
        /// </summary>
        public string cVouchEorMsg = "";

        /// <summary>
        /// 从数据库名获取帐套号
        /// </summary>
        /// <param name="DBName"></param>
        /// <returns></returns>
        public String getAccId(String DBName)
        {
            String AccId;
            if (DBName.Length < 10)
                return "";
            AccId = DBName.Substring(7, 3);
            ///MessageBox.Show(AccId);
            return AccId;
        }

        #endregion
        /// <summary>
        /// 查看当前数据库是否是U8帐套数据库
        /// </summary>
        /// <returns>表名链表</returns>
        public int isU8Model()
        {
            if (!IsOpen )
                return -1;
            DataTable dt = this.GetDataTable("select name from sysobjects where ( xtype = 'u' or xtype='v') and name in ('GL_accVouch')");
            if (dt.Rows.Count > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        
        /// <summary>
        /// 带事务的更新用户权限，需要一次将整张单据的删除和插入SQL传入
        /// </summary>
        /// <param name="SqlHead">删除SQL</param>
        /// <param name="SqlBody_list">新增SQL列表</param>
        /// <returns></returns>
        public bool UpdateYhQxTransaction(string SqlHead, List<string> SqlBody_list)
        {
            bool iRet = false;
            Open();
            if (!IsOpen) return false;
            lock (this)
            {
                m_Mutex.WaitOne();

                OleDbTransaction transaction = null;

                OleDbDataAdapter dap = new OleDbDataAdapter();

                OleDbCommand cmd = new OleDbCommand();
                cmd.CommandType = CommandType.Text;

                OleDbCommand selectcmd = new OleDbCommand();
                selectcmd.CommandType = CommandType.Text;

                OleDbCommand updatecmd = new OleDbCommand();
                updatecmd.CommandType = CommandType.Text;

                cVouchEorMsg = "";

                try
                {
                    transaction = m_OleCon.BeginTransaction(IsolationLevel.RepeatableRead);
                    cmd.Transaction = transaction;

                    dap.InsertCommand = cmd;
                    dap.InsertCommand.Connection = m_OleCon;
                    dap.InsertCommand.Transaction = transaction;

                    dap.SelectCommand = selectcmd;
                    dap.SelectCommand.Connection = m_OleCon;
                    dap.SelectCommand.Transaction = transaction;

                    dap.UpdateCommand = updatecmd;
                    dap.UpdateCommand.Connection = m_OleCon;
                    dap.UpdateCommand.Transaction = transaction;
                    //事务开始


                    //执行表头的delete语句
                    dap.UpdateCommand.CommandText = SqlHead;
                    if (transaction != null) dap.UpdateCommand.ExecuteNonQuery();
                    //表头插好了，dap.InsertCommand进行新的操作前，需要重置，Parameters需要手动clear,CommandText用新的语句替换即可
                    dap.UpdateCommand.Parameters.Clear();


                    //执行明细表的insert
                    for (int k = 0; k < SqlBody_list.Count; k++)
                    {
                        string SqlBody = SqlBody_list[k];

                        dap.InsertCommand.CommandText = SqlBody;
                        if (transaction != null) dap.InsertCommand.ExecuteNonQuery();
                        dap.InsertCommand.Parameters.Clear();//Clear Parameters
                    }
                    
                    //完成，Commit
                    if (transaction != null) transaction.Commit();
                    iRet = true;
                }
                catch (InvalidOperationException Ex)
                {
                    //oh,my God!出错了
                    if (transaction != null)
                    {
                        OnError("COleDbOperate.ExecSql<<<" + Ex.Message + ">>>,执行失败数据回滚！", Ex);
                        cmd.Transaction.Rollback();
                    }
                    else
                    {
                        OnError("COleDbOperate.ExecSql(" + Ex.Message + "),不支持并行事务，事务为空！", Ex);
                    }
                }
                catch (Exception Ex)
                {
                    if (transaction != null)
                    {
                        OnError("COleDbOperate.ExecSql(" + Ex.Message + "),执行失败数据回滚！", Ex);
                        cmd.Transaction.Rollback();
                    }
                    else
                    {
                        OnError("COleDbOperate.ExecSql(" + Ex.Message + "),其他异常！", Ex);
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

    }
}
