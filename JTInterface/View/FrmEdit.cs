using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JTInterface.Dao;

namespace JTInterface.View
{
    public partial class FrmEdit : Form
    {
        string no_bill;
        public FrmEdit(string sys_no_bill)
        {
            InitializeComponent();
            no_bill = sys_no_bill;
            textBox1.Text = no_bill;
            Init(no_bill);
        }
        private void Init(string no_bill)
        {
            UserDao dao = new UserDao();
            DataTable dt = new DataTable();
            string sql = string.Format("select * from ls_send_prepare where no_bill='{0}'",no_bill);
            dt = dao.GetScalar(sql);
            textBox2.Text = dt.Rows[0]["qty_send"].ToString();
            textBox3.Text = dt.Rows[0]["no_car"].ToString();
        }
    }
}
