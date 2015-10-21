using JTInterface.Dao;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JTInterface.View
{
    public partial class FrmTaskPush : Form
    {
        public FrmTaskPush()
        {
            InitializeComponent();
            dataGridView2.AutoGenerateColumns = false;
            //dataGridView3.AutoGenerateColumns = false;
            //dataGridView4.AutoGenerateColumns = false;
            //dataGridView5.AutoGenerateColumns = false;
            initcar();
            yupai();
            preparetask();
        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            initcar();
            yupai();
            preparetask();
        }

        private void 开启自动刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //System.Timers.Timer t = new System.Timers.Timer(300);
            ////实例化Timer类，设置间隔时间为10000毫秒；   
            //t.Elapsed += new System.Timers.ElapsedEventHandler(fresh);
            ////到达时间的时候执行事件；   
            //t.AutoReset = true;
            ////设置是执行一次（false）还是一直执行(true)；   
            //t.Enabled = true;
            ////是否执行System.Timers.Timer.Elapsed事件；
            timer1.Enabled = true;
        }

        private void 关闭自动刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //UserDao dao = new UserDao();
            //string no_bill = this.dataGridView2.CurrentRow.Cells["no_bill"].Value.ToString().Trim();
            //dataGridView5.DataSource = dao.getRequire(no_bill);
        }
        private void initcar()
        {
            UserDao dao = new UserDao();
            dataGridView1.DataSource = dao.getCar();
        }
        private void yupai()
        {
            UserDao dao = new UserDao();
            dataGridView2.DataSource = dao.getYupai();
        }
        private void preparetask()
        {
            UserDao dao = new UserDao();
            string sql_1 = string.Format("select * from ls_send_prepare where is_done=0 and no_fahuolou='1#' and date_bill between '{0}' and '{1}'", DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00", DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
            string sql_2 = string.Format("select * from ls_send_prepare where is_done=0 and no_fahuolou='2#' and date_bill between '{0}' and '{1}'", DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00", DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
            //dataGridView3.DataSource= dao.GetScalar(sql_1);
            //dataGridView4.DataSource = dao.GetScalar(sql_2);
        }
        private void 上移ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = (DataTable)dataGridView2.DataSource;
                int index = dataGridView2.SelectedRows[0].Index;

                if (dataGridView2.CurrentRow.Index <= 0)
                {
                    return;
                }
                else
                {
                    DataRow tempRow = dt.NewRow();
                    for (int i = 0; i < 65; i++)
                    {
                        tempRow[i] = dt.Rows[index][i];
                    }
                    dt.Rows.InsertAt(tempRow, index - 1);
                    dt.Rows.RemoveAt(index + 1);
                    dataGridView2.ClearSelection();
                    dataGridView2.Rows[index - 1].Selected = true;
                    dataGridView2.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("请选择一行！");
            }
        }

        private void 下移ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
            DataTable dt = new DataTable();
            dt = (DataTable)dataGridView2.DataSource;
            int index = dataGridView2.SelectedRows[0].Index;
            if (index == dt.Rows.Count - 1)
            {
                return;
            }
            else if (index == -1)
            {
                return;
            }
            else
            {
                DataRow tempRow = dt.NewRow();
                for (int i = 0; i < 65; i++)
                {
                    tempRow[i] = dt.Rows[index][i];
                }
                dt.Rows.InsertAt(tempRow, index + 2);
                dt.Rows.RemoveAt(index);
                dataGridView2.ClearSelection();
                dataGridView2.Rows[index + 1].Selected = true;
                dataGridView2.DataSource = dt;
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show("请选择一行！");
            }
        }

        private void 上移ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = (DataTable)dataGridView1.DataSource;
                int index = dataGridView1.SelectedRows[0].Index;

                if (dataGridView1.CurrentRow.Index <= 0)
                {
                    return;
                }
                else
                {
                    DataRow tempRow = dt.NewRow();
                    for (int i = 0; i < 1; i++)
                    {
                        tempRow[i] = dt.Rows[index][i];
                    }
                    dt.Rows.InsertAt(tempRow, index - 1);
                    dt.Rows.RemoveAt(index + 1);
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[index - 1].Selected = true;
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("请选择一行！");
            }
        }

        private void 下移ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = (DataTable)dataGridView1.DataSource;
                int index = dataGridView1.SelectedRows[0].Index;
                if (index == dt.Rows.Count - 1)
                {
                    return;
                }
                else if (index == -1)
                {
                    return;
                }
                else
                {
                    DataRow tempRow = dt.NewRow();
                    for (int i = 0; i < 1; i++)
                    {
                        tempRow[i] = dt.Rows[index][i];
                    }
                    dt.Rows.InsertAt(tempRow, index + 2);
                    dt.Rows.RemoveAt(index);
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[index + 1].Selected = true;
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("请选择一行！");
            }
        }

        private void 移除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try 
            {
                DialogResult dr = MessageBox.Show("确认删除吗？", "提示", MessageBoxButtons.OKCancel);
                if (dr == DialogResult.OK)
                {
                    UserDao dao = new UserDao();
                    string no_bill = this.dataGridView2.CurrentRow.Cells["no_bill"].Value.ToString().Trim();
                    string sql = string.Format("update ls_send_yupai set isdel=1 where no_bill='{0}'",no_bill);
                    dao.GetScalar(sql);
                    dataGridView2.Rows.RemoveAt(dataGridView2.SelectedRows[0].Index);
                    //yupai();
                }
                else if (dr == DialogResult.Cancel)
                {
                    return;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("请选择一行！");
            }
        }

        private void 刷新ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            initcar();
        }

        private void 刷新ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            yupai();
        }

        private void 移除ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show("确认删除吗？", "提示", MessageBoxButtons.OKCancel);
                if (dr == DialogResult.OK)
                {
                    //UserDao dao = new UserDao();
                    //string no_bill = this.dataGridView2.CurrentRow.Cells["no_bill"].Value.ToString().Trim();
                    //string sql = string.Format("update ls_send_yupai set isdel=1 where no_bill='{0}'", no_bill);
                    //dao.GetScalar(sql);
                    dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
                    //yupai();
                }
                else if (dr == DialogResult.Cancel)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("请选择一行！");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 10000;
            autopaidan();
        }
        private void autopaidan()
        {
            try 
            {
                UserDao dao = new UserDao();
                preparetask();
                if(1==1)//dataGridView3.Rows.Count<5)
                {
                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    {
                        string no_bill = dataGridView2.Rows[i].Cells["no_bill"].Value.ToString();
                        string no_fahuolou = dataGridView2.Rows[i].Cells["no_fahuolou"].Value.ToString();
                        if (no_fahuolou.Equals("1#"))
                        {
                            string sql_1 = string.Format("select req_qty from ls_send_require where no_bill='{0}'",no_bill);
                            string sql_2 = string.Format("select sum(qty_fact) from ls_send_prepare where no_bill='{0}' and isdel<>1", no_bill);
                            decimal req_qty= dao.GetScalar_d(sql_1);
                            decimal qty_total = dao.GetScalar_d(sql_2);
                            if (qty_total < req_qty)
                            {
                                string sql_3 = string.Format("select req_timespan from ls_send_require where no_bill='{0}'", no_bill);
                                string sql_4 = string.Format("select isnull(MAX(date_bill),'') as time from ls_send_prepare where no_bill='{0}'", no_bill);
                                double req_timespan = Convert.ToDouble(dao.GetScalar_d(sql_3));
                                DataTable last_time_table = dao.GetScalar(sql_4);
                                string last_time = last_time_table.Rows[0]["time"].ToString();
                                DateTime lasttime = DateTime.Parse (last_time);
                                if (DateTime.Now > lasttime.AddMinutes(req_timespan))
                                {
                                    for (int h = 0; h < dataGridView1.Rows.Count; h++)
                                    {
                                        string no_car = dataGridView1.Rows[h].Cells["carid"].Value.ToString();
                                        string sql_5 = string.Format("select count(*) from ls_send_carnotinclude where no_bill_prepare='{0}' and car_no='{1}'",no_bill,no_car);
                                        decimal isinclude = dao.GetScalar_d(sql_5);
                                        if(isinclude==0)
                                        {
                                            decimal qty = 0;
                                            string sql_6 = string.Format("select qty_stand from ls_car where no_ls='{0}'", no_car);
                                            decimal qty_stand = dao.GetScalar_d(sql_6);
                                            string no_prepare = dao.no_bill("","ls_send_prepare");
                                            string sql_7;
                                            if (req_qty - qty_total > qty_stand)
                                            {
                                                qty = qty_stand;
                                                sql_7 = string.Format("insert into ls_send_prepare (no_bill_prepare,no_fahuolou,no_bill,qty_send,date_bill,no_car,is_done) values('{0}','{1}','{2}','{3}','{4}','{5}',0)", no_bill,no_fahuolou, no_prepare, qty, DateTime.Now.ToString(), no_car);
                                            }
                                            else
                                            {
                                                qty = req_qty - qty_total;
                                                sql_7 = string.Format("insert into ls_send_prepare (no_bill_prepare,no_fahuolou,no_bill,qty_send,date_bill,no_car,is_done) values('{0}','{1}','{2}','{3}','{4}','{5}',0)", no_bill,no_fahuolou, no_prepare, qty, DateTime.Now.ToString(), no_car);
                                            }
                                            dao.GetScalar(sql_7);
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        
                    }
                }
                if (1==1)//(dataGridView4.Rows.Count < 5)
                {
                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    {
                        string no_bill = dataGridView2.Rows[i].Cells["no_bill"].Value.ToString();
                        string no_fahuolou = dataGridView2.Rows[i].Cells["no_fahuolou"].Value.ToString();
                        if (no_fahuolou.Equals("2#"))
                        {
                            string sql_1 = string.Format("select req_qty from ls_send_require where no_bill='{0}'", no_bill);
                            string sql_2 = string.Format("select sum(qty_fact) from ls_send_prepare where no_bill='{0}'", no_bill);
                            decimal req_qty = dao.GetScalar_d(sql_1);
                            decimal qty_total = dao.GetScalar_d(sql_2);
                            if (qty_total < req_qty)
                            {
                                string sql_3 = string.Format("select req_timespan from ls_send_require where no_bill='{0}'", no_bill);
                                string sql_4 = string.Format("select isnull(MAX(date_bill),'') as time from ls_send_prepare where no_bill='{0}'", no_bill);
                                double req_timespan = Convert.ToDouble(dao.GetScalar_d(sql_3));
                                DataTable last_time_table = dao.GetScalar(sql_4);
                                string last_time = last_time_table.Rows[0]["time"].ToString();
                                DateTime lasttime = DateTime.Parse(last_time);
                                if (DateTime.Now > lasttime.AddMinutes(req_timespan))
                                {
                                    for (int h = 0; h < dataGridView1.Rows.Count; h++)
                                    {
                                        string no_car = dataGridView1.Rows[h].Cells["carid"].Value.ToString();
                                        string sql_5 = string.Format("select count(*) from ls_send_carnotinclude where no_bill_prepare='{0}' and car_no='{1}'", no_bill, no_car);
                                        decimal isinclude = dao.GetScalar_d(sql_5);
                                        if (isinclude == 0)
                                        {
                                            decimal qty = 0;
                                            string sql_6 = string.Format("select qty_stand from ls_car where no_ls='{0}'", no_car);
                                            decimal qty_stand = dao.GetScalar_d(sql_6);
                                            string no_prepare = dao.no_bill("", "ls_send_prepare");
                                            string sql_7;
                                            if (req_qty - qty_total > qty_stand)
                                            {
                                                qty = qty_stand;
                                                sql_7 = string.Format("insert into ls_send_prepare (no_bill_prepare,no_fahuolou,no_bill,qty_send,date_bill,no_car,is_done) values('{0}','{1}','{2}','{3}','{4}','{5}',0)", no_bill, no_fahuolou, no_prepare, qty, DateTime.Now.ToString(), no_car);
                                            }
                                            else
                                            {
                                                qty = req_qty - qty_total;
                                                sql_7 = string.Format("insert into ls_send_prepare (no_bill_prepare,no_fahuolou,no_bill,qty_send,date_bill,no_car,is_done) values('{0}','{1}','{2}','{3}','{4}','{5}',0)", no_bill, no_fahuolou, no_prepare, qty, DateTime.Now.ToString(), no_car);
                                            }
                                            dao.GetScalar(sql_7);
                                            preparetask();
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                        }

                    }
                }
            }
            catch(Exception ex)
            {
            }
        }
        private void checkpaidan(string fahuolou)
        { 
        }

        private void 手动排任务ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            autopaidan();
        }

        private void 暂停ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = (DataTable)dataGridView2.DataSource;
                int index = dataGridView2.SelectedRows[0].Index;

                if (dataGridView2.CurrentRow.Index <= 0)
                {
                    return;
                }
                else
                {
                    dataGridView2.Rows[index].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("请选择一行！");
            }
        }

        private void 恢复ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = (DataTable)dataGridView2.DataSource;
                int index = dataGridView2.SelectedRows[0].Index;

                if (dataGridView2.CurrentRow.Index <= 0)
                {
                    return;
                }
                else
                {
                    if (dataGridView2.Rows[index].DefaultCellStyle.BackColor == System.Drawing.Color.Yellow)
                    {
                        dataGridView2.Rows[index].DefaultCellStyle.BackColor = System.Drawing.Color.White;
                        DataRow tempRow = dt.NewRow();
                        for (int i = 0; i < 65; i++)
                        {
                            tempRow[i] = dt.Rows[index][i];
                        }
                        dt.Rows.InsertAt(tempRow, 0);
                        dt.Rows.RemoveAt(index + 1);
                        dataGridView2.ClearSelection();
                        dataGridView2.Rows[0].Selected = true;
                        dataGridView2.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("请选择暂停的任务！");
            }
        }
        private void edit(string sys_bo_bill)
        {
            FrmEdit frm = new FrmEdit(sys_bo_bill);
            frm.Show();
        }

        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                UserDao dao = new UserDao();
                //int index = dataGridView3.SelectedRows[0].Index;
                //string no_bill = this.dataGridView3.Rows[index].Cells["danhao"].Value.ToString().Trim();
                //edit(no_bill);
            }
            catch (Exception ex)
            {
                MessageBox.Show("请选择一行！");
            }
        }
        
        private void 修改ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                UserDao dao = new UserDao();
                //int index = dataGridView4.SelectedRows[0].Index;
                //string no_bill = this.dataGridView4.Rows[index].Cells["danhao"].Value.ToString().Trim();
                //edit(no_bill);
            }
            catch (Exception ex)
            {
                MessageBox.Show("请选择一行！");
            }
        }

        private void 修改ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                UserDao dao = new UserDao();
                //int index = dataGridView6.SelectedRows[0].Index;
                //string no_bill = this.dataGridView6.Rows[index].Cells["danhao"].Value.ToString().Trim();
                //edit(no_bill);
            }
            catch (Exception ex)
            {
                MessageBox.Show("请选择一行！");
            }
        }

        private void 修改ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            try
            {
                UserDao dao = new UserDao();
                //int index = dataGridView7.SelectedRows[0].Index;
                //string no_bill = this.dataGridView7.Rows[index].Cells["danhao"].Value.ToString().Trim();
                //edit(no_bill);
            }
            catch (Exception ex)
            {
                MessageBox.Show("请选择一行！");
            }
        }

        private void 预排任务ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmTasksplit frm = new FrmTasksplit();
            frm.Show();
        }
    }
}
