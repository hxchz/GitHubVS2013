using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using JTInterface.Dao;

namespace JTInterface.View
{
    public partial class FrmTask : Form
    {
        public DataTable dtmain = new DataTable();
        public FrmTask()
        {
            InitializeComponent();
            dgv_1.AutoGenerateColumns = false;
            dataGridView1.AutoGenerateColumns = false;
            //timer1.Enabled = true;
            //Init();
        }
        private void Init()
        {
            UserDao dao = new UserDao();
            dgv_1.DataSource = dao.SelectAll();
            dataGridView1.DataSource = dao.getCarInfo();
            dataGridView2.DataSource = dao.getSanYCarInfo();
            act_color();
        }
        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserDao dao=new UserDao();
            dgv_1.DataSource = dao.SelectAll();
            dataGridView1.DataSource=dao.getCarInfo();
            dataGridView2.DataSource = dao.getSanYCarInfo();
            act_color();
        }
        private void act_color()
        {
            for (int i = 0; i < dgv_1.Rows.Count; i++)
            {
                string name_type_bengsong = dgv_1.Rows[i].Cells["no_type_bengsong__1"].Value.ToString();
                if (name_type_bengsong == "泵送")
                {
                    dgv_1.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightPink;
                }
            }
        }

        private void dgv_1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowindex = dgv_1.SelectedCells[0].RowIndex;
            string txt = dgv_1.CurrentRow.Cells["no_bill"].Value.ToString();
            MessageBox.Show(txt);
        }

        private void FrmTask_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void 定时刷新ToolStripMenuItem_Click(object sender, EventArgs e)
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
        public void fresh()
        {
            UserDao dao = new UserDao();
            dgv_1.DataSource = dao.SelectAll();
            dataGridView1.DataSource = dao.getCarInfo();
            dataGridView2.DataSource = dao.getSanYCarInfo();
            act_color();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            //timer1.Enabled = true;
            fresh();
        }

        private void 暂停刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }

    }
}
