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
    /// �̳������ݿ���ʻ��࣬��չһЩҵ������ݿ���� 2011-4-19 ����
    /// </summary>
    public  class DBOperate : DataBase.Base_DbOperate
    {
        #region private����
        /// <summary>
        /// ���׺�
        /// </summary>
        private String _AccId;
        #endregion

        #region ����ֵ

        public String DBServer
        {
            get
            {
                return m_OleCon.DataSource ;
            }
        }
        /// <summary>
        /// ������ ��ʽΪ �����ݿ�����@���ݿ���������ơ�
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
        /// ���ݶ�ռ����ʱ�����Ĵ�����Ϣ
        /// </summary>
        public string cVouchEorMsg = "";

        /// <summary>
        /// �����ݿ�����ȡ���׺�
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
        /// �鿴��ǰ���ݿ��Ƿ���U8�������ݿ�
        /// </summary>
        /// <returns>��������</returns>
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
        /// ������ĸ����û�Ȩ�ޣ���Ҫһ�ν����ŵ��ݵ�ɾ���Ͳ���SQL����
        /// </summary>
        /// <param name="SqlHead">ɾ��SQL</param>
        /// <param name="SqlBody_list">����SQL�б�</param>
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
                    //����ʼ


                    //ִ�б�ͷ��delete���
                    dap.UpdateCommand.CommandText = SqlHead;
                    if (transaction != null) dap.UpdateCommand.ExecuteNonQuery();
                    //��ͷ����ˣ�dap.InsertCommand�����µĲ���ǰ����Ҫ���ã�Parameters��Ҫ�ֶ�clear,CommandText���µ�����滻����
                    dap.UpdateCommand.Parameters.Clear();


                    //ִ����ϸ���insert
                    for (int k = 0; k < SqlBody_list.Count; k++)
                    {
                        string SqlBody = SqlBody_list[k];

                        dap.InsertCommand.CommandText = SqlBody;
                        if (transaction != null) dap.InsertCommand.ExecuteNonQuery();
                        dap.InsertCommand.Parameters.Clear();//Clear Parameters
                    }
                    
                    //��ɣ�Commit
                    if (transaction != null) transaction.Commit();
                    iRet = true;
                }
                catch (InvalidOperationException Ex)
                {
                    //oh,my God!������
                    if (transaction != null)
                    {
                        OnError("COleDbOperate.ExecSql<<<" + Ex.Message + ">>>,ִ��ʧ�����ݻع���", Ex);
                        cmd.Transaction.Rollback();
                    }
                    else
                    {
                        OnError("COleDbOperate.ExecSql(" + Ex.Message + "),��֧�ֲ�����������Ϊ�գ�", Ex);
                    }
                }
                catch (Exception Ex)
                {
                    if (transaction != null)
                    {
                        OnError("COleDbOperate.ExecSql(" + Ex.Message + "),ִ��ʧ�����ݻع���", Ex);
                        cmd.Transaction.Rollback();
                    }
                    else
                    {
                        OnError("COleDbOperate.ExecSql(" + Ex.Message + "),�����쳣��", Ex);
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
