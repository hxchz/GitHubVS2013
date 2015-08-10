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
        public FrmTask()
        {
            InitializeComponent();
            dgv_1.AutoGenerateColumns = false;
            dataGridView1.AutoGenerateColumns = false;
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

    }
}
