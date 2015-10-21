using JTInterface.Dao;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dal;

namespace JTInterface.View
{
    public partial class FrmTasksplit : Form
    {
        int i;
        public string budan = "";
        public sys_all_edit_bill this_edit;
        decimal yudingQty = 0M;//申购预定数量
        //decimal leijiQty = 0M;//累计发货量
        decimal leijiQty_2 = 0M;//该搅拌楼累计发货数字
        decimal sumQty = 0M;//已发货单数量 = qty_send-qty_baofei-qty_back，用于发货数量的控制判断
        decimal sumQty2 = 0M;
        bool is_ok = true;
        string no_send = "";
        private string bianhao_jiashiyuan = "";
        private int load = 0;
        public FrmTasksplit()
        {
            InitializeComponent();
            cmb_no_fahuolou.SelectedValue = "1#";
            txt_person_make.Text = "管理员";
            Init();
        }
        public void theout(object source, System.Timers.ElapsedEventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //System.Timers.Timer t = new System.Timers.Timer(1000);
            ////实例化Timer类，设置间隔时间为10000毫秒；   
            //t.Elapsed += new System.Timers.ElapsedEventHandler(theout);
            ////到达时间的时候执行事件；   
            //t.AutoReset = true;
            ////设置是执行一次（false）还是一直执行(true)；   
            //t.Enabled = true;
            ////是否执行System.Timers.Timer.Elapsed事件；
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void btn_print_old_Click(object sender, EventArgs e)
        {

        }
        private void Init()
        {
            UserDao dao = new UserDao();
            dgv_1.DataSource = dao.SelectSend(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00", DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
        }
        private void dgv_1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            send_mx_load();
            leijiQty_2_load();
            leijiQty_pd();
        }
        private void send_mx_load()
        {
            UserDao dao = new UserDao();
            //this_edit.ds.Tables["head"].Rows[0]["no_productorder"] = this.dgv_1.CurrentRow.Cells["no_productorder"].Value.ToString().Trim();
            //this_edit.ds.Tables["head"].Rows[0]["shigongbuwei"] = BLL.string_1(this.dgv_1.CurrentRow.Cells["shigongbuwei"].Value);
            //this_edit.ds.Tables["head"].Rows[0]["distance_car"] = BLL.decimal_1(dgv_1.CurrentRow.Cells["distance_car"].Value);
            txt_distance_car.Text = dgv_1.CurrentRow.Cells["distance_car"].Value.ToString();
            decimal yudingQty = decimal.Parse(this.dgv_1.CurrentRow.Cells["qty_yuding"].Value.ToString());//预定数量
            this.txt_no_product__2.Text = this.dgv_1.CurrentRow.Cells["no_productorder"].Value.ToString().Trim();
            this.txt_no_hetong.Text = this.dgv_1.CurrentRow.Cells["no_hetong"].Value.ToString().Trim();
            this.txt_no_productorder__1__4.Text = this.dgv_1.CurrentRow.Cells["no_bill_in"].Value.ToString().Trim();
            this.textBox6.Text = this.dgv_1.CurrentRow.Cells["shigongbuwei"].Value.ToString().Trim();
            this.textBox7.Text = this.dgv_1.CurrentRow.Cells["no_type_bengsong"].Value.ToString().Trim();

            this.txt_peibidan.Text = this.dgv_1.CurrentRow.Cells["no_peibidan"].Value.ToString().Trim();
            //this.txt_no_productorder__1__2__2.Text = this.dgv_1.CurrentRow.Cells["name_company"].Value.ToString().Trim();
            //this.txt_no_productorder__1__3.Text = this.dgv_1.CurrentRow.Cells["name_product"].Value.ToString().Trim();
            //this.txt_no_productorder__1.Text = this.dgv_1.CurrentRow.Cells["shigongbuwei"].Value.ToString().Trim();
            this.cmb_no_type_bengsong.SelectedValue = this.dgv_1.CurrentRow.Cells["name_type_bengsong"].Value.ToString().Trim();
            this.cmb_no_type_bengsong.Text = this.dgv_1.CurrentRow.Cells["name_type_bengsong"].Value.ToString().Trim();
            if (this.cmb_no_type_bengsong.SelectedValue == "01")
            {
                //2011-06-14注释，模式变动，打泵改成从发货单做
                this.cmb_no_benghao.Enabled = false;
                this.cmb_no_bengrenyuan.Enabled = false;
            }
            else
            {
                //泵送非泵送的都不用输入泵人员
                this.cmb_no_benghao.Enabled = true;
                this.cmb_no_bengrenyuan.Enabled = true;
                if (this.dgv_1.CurrentRow.Cells["no_benghao"].Value != null && this.dgv_1.CurrentRow.Cells["no_benghao"].Value.ToString().Trim() != "")
                {
                    this.cmb_no_benghao.SelectedValue = this.dgv_1.CurrentRow.Cells["no_benghao"].Value.ToString().Trim();
                    this.cmb_no_bengrenyuan.SelectedValue = this.dgv_1.CurrentRow.Cells["no_bengrenyuan"].Value.ToString().Trim();
                }
            }
            this.cmb_no_qiangdu_ranks.SelectedValue = dgv_1.CurrentRow.Cells["name_qiangdu_ranks"].Value.ToString().Trim();
            this.cmb_no_qiangdu_ranks.Text = dgv_1.CurrentRow.Cells["name_qiangdu_ranks"].Value.ToString().Trim();
            //获取是否灌桩
            if (this.dgv_1.CurrentRow.Cells["name_guanzhuang"].Value.ToString() == "")
            {

            }
            else
            {
                string sql = string.Format("select no_ls from ls_guanzhuang where name_guanzhuang='{0}'", this.dgv_1.CurrentRow.Cells["name_guanzhuang"].Value.ToString().Trim());
                this.cmb_no_guanzhuang.SelectedValue = dao.GetScalar_s(sql);
            }
            txt_no_bill.Text = dao.no_bill("","ls_send_yupai");
        }
        private void leijiQty_2_load()
        {
            UserDao dao = new UserDao();
            //if (this.cmb_no_fahuolou.SelectedValue == null)
            //{
            //    return;
            //}
            if (this.dgv_1.CurrentCell == null)
            {
                return;
            }
            //刷新累计车次和累计方量
            //已发货单数量 = qty_send-qty_baofei-qty_back,用于发货数量的控制判断
            //sumQty = db.GetScalar_d("select sum(qty_send)-sum(qty_back)-sum(qty_baofei)-sum(qty_zy) from ls_send where no_productorder ='" + this.dgv_1.CurrentRow.Cells["no_productorder"].Value.ToString().Trim() + "' and no_fahuolou='" + BLL.string_1(this.cmb_no_fahuolou.SelectedValue) + "' and date_bill between '" + DateTime.Now.ToShortDateString() + "' and '" + DateTime.Now.ToShortDateString() + " 23:59:59'");
            sumQty = dao.GetScalar_d("select sum(qty_send)-sum(qty_back)-sum(qty_baofei)-sum(qty_zy) from ls_send where no_productorder ='" + this.dgv_1.CurrentRow.Cells["no_productorder"].Value.ToString().Trim() + "'");
            this.txt_qty_leiji.Text = sumQty.ToString();
            //this_edit.ds.Tables["head"].Rows[0]["qty_leiji"] = sumQty.ToString();

            //sumQty2 = db.GetScalar_d("select sum(qty_send) from ls_send where no_productorder ='" + this.dgv_1.CurrentRow.Cells["no_productorder"].Value.ToString().Trim() + "' and no_fahuolou='" + BLL.string_1(this.cmb_no_fahuolou.SelectedValue) + "'");
            sumQty2 = dao.GetScalar_d("select sum(qty_send) from ls_send where no_productorder ='" + this.dgv_1.CurrentRow.Cells["no_productorder"].Value.ToString().Trim() + "'");
            this.txt_qty_send_total.Text = sumQty2.ToString();
            //this_edit.ds.Tables["head"].Rows[0]["qty_send_total"] = sumQty2.ToString();

            //累计发货车次
            //int checi = db.GetScalar("select count(*) from ls_send where no_productorder='" + this.dgv_1.CurrentRow.Cells["no_productorder"].Value.ToString().Trim() + "' and no_fahuolou='" + BLL.string_1(this.cmb_no_fahuolou.SelectedValue) + "' and date_bill between '" + DateTime.Now.ToShortDateString() + "' and '" + DateTime.Now.ToShortDateString() + " 23:59:59'") + 1;
            int checi = Convert.ToInt32(dao.GetScalar_d("select count(*) from ls_send where no_productorder='" + this.dgv_1.CurrentRow.Cells["no_productorder"].Value.ToString().Trim() + "'")) + 1;
            this.txt_checi.Text = checi.ToString();
            //this_edit.ds.Tables["head"].Rows[0]["checi"] = checi.ToString();
        }
        private void leijiQty_pd()
        {
            UserDao dao = new UserDao();
            string sql = String.Format("select no_send_control from ls_hetong where no_bill='{0}'", txt_no_hetong.Text.Trim());

            //判断选中的合同发货控制类型（01先款后货  02其他）
            if (dao.GetScalar_s(sql) == "01")
            {
                leijiQty_2 = dao.GetScalar_d("select sum(qty_send)-sum(qty_back)-sum(qty_baofei)-sum(qty_zy) from ls_send where no_productorder ='" + this.dgv_1.CurrentRow.Cells["no_productorder"].Value.ToString().Trim() + "'");
                if (leijiQty_2 >= yudingQty)
                {
                    MessageBox.Show("发货数量已经超出预定数量，不允许发货");
                    this.is_ok = false;
                }
            }
            if (this.is_ok)
            {
            }
            else
            {
                this.btn_save2.Enabled = false;
            }
            this.is_ok = true;
        }

        private void btn_save2_Click(object sender, EventArgs e)
        {
            UserDao dao = new UserDao();
            string no_hetong = txt_no_hetong.Text;
            string no_hetong_neibu = txt_no_productorder__1__4.Text;
            string peibidan = txt_peibidan.Text;
            string no_bill = txt_no_bill.Text;
            string date_make = dtp_date_make.Text;
            string no_fahuolou = cmb_no_fahuolou.Text;
            string no_car = cmb_no_car.Text;
            string no_driver = cmb_no_driver.Text;
            string no_qiangdu_ranks = cmb_no_qiangdu_ranks.Text;
            string no_type_bengsong = textBox7.Text;
            string no_benghao = cmb_no_benghao.Text;
            string no_bengrenyuan = cmb_no_bengrenyuan.Text;
            string no_tadiao = cmb_no_tadiao.Text;
            string huizhi = textBox8.Text;
            string no_guanzhuang = cmb_no_guanzhuang.Text;
            string no_product__2 = txt_no_product__2.Text;
            string remark = txt_remark.Text;
            string person_make = txt_person_make.Text;
            string shigongbuwei=textBox6.Text;
            int isjiesuan=0;
            int add_car=0;
            int tuobeng=0;
            double qty_back = 0.00;
            double qty_in = 0.00;
            double qty_baofei = 0.00;
            double qty_zy = 0.00;
            decimal distance_car = txt_distance_car.Text!=""?Convert.ToDecimal(txt_distance_car.Text):0;
            decimal qty_fact =txt_qty_fact.Text!=""? Convert.ToDecimal(txt_qty_fact.Text):0;
            decimal qty_from = txt_qty_from.Text!=""?Convert.ToDecimal(txt_qty_from.Text):0;
            decimal qty_send = txt_qty_send.Text!=""?Convert.ToDecimal(txt_qty_send.Text):0;
            decimal qty_leiji =txt_qty_leiji.Text!=""? Convert.ToDecimal(txt_qty_leiji.Text):0;
            decimal checi =txt_checi.Text!=""? Convert.ToDecimal(txt_checi.Text):0;
            decimal qty_send_total =txt_qty_send_total.Text!=""? Convert.ToDecimal(txt_qty_send_total.Text):0;
            decimal qty_shajiang =txt_qty_shajiang.Text!=""? Convert.ToDecimal(txt_qty_shajiang.Text):0;
            decimal req_timespan = textBox1.Text!=""?Convert.ToDecimal(textBox1.Text):0;
            decimal req_qty = textBox2.Text!=""?Convert.ToDecimal(textBox2.Text):0;
            decimal req_carnum =textBox3.Text!=""? Convert.ToDecimal(textBox3.Text):0;
            decimal req_time = textBox4.Text != "" ? Convert.ToDecimal(textBox4.Text) : 0;
            if(no_bill.Trim().Equals(""))
            {
                MessageBox.Show("请选择任务！");
                return;
            }
            foreach (DataGridViewRow dgvRow in this.dataGridView1.Rows)
            {
                string no_carnot = dgvRow.Cells["carid"].Value.ToString();
                string sql_car = String.Format("insert into ls_send_carnotinclude(no_bill_prepare,car_no) values('{0}','{1}')", no_bill, no_carnot);
                dao.GetScalar(sql_car);
            }
            string sql_require = string.Format("insert into ls_send_require(no_bill,req_time,req_qty,req_carnum,req_timespan) values('{0}','{1}','{2}','{3}','{4}')",no_bill,req_time,req_qty,req_carnum,req_timespan);
            string sql = String.Format("insert into ls_send_yupai (no_bill,date_bill,no_productorder,no_fahuolou,no_car,no_driver,is_clear,huizhi,qty_shajiang,qty_send,qty_fact,qty_from,remark,add_car,tuobeng,no_type_bengsong,no_benghao,no_bengrenyuan,qty_back,qty_in,qty_baofei,qty_zy,qty_leiji,checi,amount,amount_add_car,amount_tuobeng,amount_total,state_send,time_jiange,is_over,person_make,person_check,person_modify,date_make,date_check,date_modify,no_qiangdu_ranks,amount_in,no_do_advice,no_guanzhuang,check_bad_times,check_remark,check_state,amount_guanzhuang,amount_bengsong,state_dabeng,date_bill2,qty_send_total,is_fh,no_clear_hetong_2,is_sys,shigongbuwei,is_wj_clear,qty_shoudong,no_tadiao,amount_tadiao,qty_butie,person_fuhe,person_quxiaofuhe,date_fuhe,date_quxiaofuhe,distance_car,isnot_distance,person_state_send,isdel) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}','{31}','{32}','{33}','{34}','{35}','{36}','{37}','{38}','{39}','{40}','{41}','{42}','{43}','{44}','{45}','{46}','{47}','{48}','{49}','{50}','{51}','{52}','{53}','{54}','{55}','{56}','{57}','{58}','{59}','{60}','{61}','{62}','{63}','{64}',0)"
                , no_bill, date_make, no_product__2, no_fahuolou, no_car, no_driver, isjiesuan, huizhi, qty_shajiang, qty_send, qty_fact, qty_from, remark, add_car, tuobeng, no_type_bengsong,no_benghao, no_bengrenyuan, qty_back, qty_in, qty_baofei, qty_zy, qty_leiji, checi, "0", "0", "0", "0", "0", req_timespan, "0", person_make, person_make, person_make, date_make,"","",no_qiangdu_ranks,"0","","","0","","0","0","0", "0","", qty_send_total, "0","","0",shigongbuwei,"0","0.00","","0.00","0.00","","","","",distance_car,"0","");
            dao.GetScalar(sql_require);
            dao.GetScalar(sql);
            MessageBox.Show("预排成功！");
            txt_no_bill.Text = "";
            shuzi_null();
        }
        private void save_old()
        {
            UserDao dao = new UserDao();
            if (BLL.string_1(dgv_1.CurrentRow.Cells["no_productorder"].Value) != BLL.string_1(txt_no_product__2.Text))
            {
                MessageBox.Show("任务单与选中任务单不一致");
                return;
            }
            if (BLL.string_1(this.cmb_no_car.SelectedValue) != "")
            {
                //string no_car_select = BLL.string_1(this.cmb_no_car.SelectedValue);
                //string sql_no_car_come = "select no_car_come from ls_car where no_ls='" + no_car_select + "'";
                //string no_car_come = dao.GetScalar_s(sql_no_car_come);
                //if (no_car_come == "01")
                //{
                //    if (BLL.string_1(this.cmb_no_driver.SelectedValue) == "")
                //    {
                //        MessageBox.Show("驾驶员不能为空");
                //        return;
                //    }
                //}
            }
            if (budan == "")
            {
                this_edit.ds.Tables["head"].Rows[0]["date_make"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                this_edit.ds.Tables["head"].Rows[0]["date_bill"] = this_edit.ds.Tables["head"].Rows[0]["date_make"];

                string sql_peibidan = "select count(*) from ls_send where no_productorder='" + BLL.string_1(dgv_1.CurrentRow.Cells["no_productorder"].Value) + "' and date_bill>dateadd(day,-3,getdate())";
                int count_pro = Convert.ToInt32(dao.GetScalar(sql_peibidan));
                if (count_pro <= 1)
                {
                    MessageBox.Show("当前任务单第一车，注意带配比单!!!");
                    MessageBox.Show("当前任务单第一车，注意带配比单!!!");
                    MessageBox.Show("当前任务单第一车，注意带配比单!!!");
                }
            }
            else if (budan == "budan")
            {
                this_edit.ds.Tables["head"].Rows[0]["date_make"] = dtp_date_make.Value.ToString("yyyy-MM-dd HH:mm:ss");
                this_edit.ds.Tables["head"].Rows[0]["date_bill"] = this_edit.ds.Tables["head"].Rows[0]["date_make"];
            }
            this_edit.ds.Tables["head"].Rows[0]["distance_car"] = BLL.decimal_1(dgv_1.CurrentRow.Cells["distance_car"].Value);
            if (BLL.decimal_1(dgv_1.CurrentRow.Cells["distance_car"].Value) == 0M)
            {
                MessageBox.Show("没有运距,不能发货");
                return;
            }
            this.label17.Focus();

            if (txt_qty_send.Text == null || txt_qty_send.Text == "" || BLL.decimal_1(txt_qty_send.Text) == 0)
            {
                if (MessageBox.Show("没有有效发货数量，确定要发货吗？ “是”确定发货，“否”返回修改", "发货提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

                decimal qty_send = BLL.decimal_1(txt_qty_send.Text);
                if (BLL.decimal_1(txt_qty_fact.Text) == 0 && qty_send == 0)
                {
                    MessageBox.Show("方量为零，禁止保存！");
                    return;
                }
            }
            {
                decimal qty_fact = BLL.decimal_1(this.txt_qty_fact.Text);
                decimal qty_from = BLL.decimal_1(this.txt_qty_from.Text);
                decimal qty_send = qty_fact + qty_from;
                decimal qty_send_total = sumQty2 + qty_send;
                decimal qty_leiji = sumQty + qty_send;
                this.txt_qty_send.Text = BLL.string_1(qty_send);
                this.txt_qty_send.DataBindings[0].WriteValue();
                this.txt_qty_send_total.Text = BLL.string_1(qty_send_total);
                this.txt_qty_send_total.DataBindings[0].WriteValue();
                this.txt_qty_leiji.Text = BLL.string_1(qty_leiji);
                this.txt_qty_leiji.DataBindings[0].WriteValue();
            }

            string sql = "";
            sql = String.Format("select no_send_control from ls_hetong where no_bill='{0}'", txt_no_hetong.Text.Trim());

            string no_productorder = BLL.string_1(dgv_1.CurrentRow.Cells["no_productorder"].Value);
            if (this_edit.save(0, 1))
            {
                if (cmb_no_fahuolou.Text == "")
                {
                    return;
                }
                string no_fahuolou = this.cmb_no_fahuolou.SelectedValue.ToString();
                DateTime aa = DateTime.Now;
                string date_a = aa.ToString("yyyy-MM-dd");
                string date_b = aa.ToString("yyyy-MM-dd") + " 23:59:59";
                sql = string.Format("select max(date_bill) as date_max from ls_send where date_bill between '{0}' and '{1}' and no_bill!='{2}'", date_a, date_b, BLL.string_1(this_edit.ds.Tables["head"].Rows[0]["no_bill"]));
                string date_last = dao.GetScalar_s(sql);

                DateTime bb = aa;
                if (date_last != null && date_last != "")
                {
                    bb = Convert.ToDateTime(date_last);
                }
                DateTime cc = Convert.ToDateTime(this.dtp_date_make.Text);
                TimeSpan tms = cc - bb;
                int min = tms.Minutes + tms.Days * 1440 + tms.Hours * 60;
                sql = string.Format("select count(*) from ls_send where date_bill between '{0}' and '{1}' and no_fahuolou='{2}'", date_a, date_b, no_fahuolou);
                if (Convert.ToInt32(dao.GetScalar(sql)) == 0)
                {
                    //sql = string.Format("update ls_fahuolou set time_jiange='{0}' where no_ls='{1}'", min, no_fahuolou);
                    // dao.ExecuteCommand(sql);
                }
                else
                {
                    sql = string.Format("select time_jiange from ls_fahuolou where no_ls='{0}'", no_fahuolou);
                    if (min > Convert.ToInt32(dao.GetScalar(sql)))
                    {
                        //sql = string.Format("update ls_fahuolou set time_jiange='{0}' where no_ls='{1}'", min, no_fahuolou);
                        //dao.GetScalar(sql);
                    }
                }
                //sql = string.Format("update ls_fahuolou set time_lastsend='{0}',qty_fact='{1}' where no_ls='{2}'", Convert.ToDateTime(this_edit.ds.Tables["head"].Rows[0]["date_make"]), BLL.decimal_1(txt_qty_send_total.Text), no_fahuolou);
                //dao.GetScalar(sql);


                //修改车辆状态为出厂（01:入厂  02出厂）
                if (cmb_no_car.Text == "")
                {
                    return;
                }
                string no_car = this.cmb_no_car.SelectedValue.ToString();
                if (budan != "budan")
                {
                    //sql = string.Format("update ls_car set no_car_state='02',no_driver='{0}',time_lastsend='{1}',name_product='{2}' where no_ls='{3}'", BLL.string_1(this.cmb_no_driver.SelectedValue), Convert.ToDateTime(this_edit.ds.Tables["head"].Rows[0]["date_make"]), BLL.string_1(txt_no_productorder__1__3.Text), no_car);
                    //dao.GetScalar(sql);
                }
                //赋值
                string no_bill = this_edit.no_bill;

                //                if (dgv_1.CurrentRow != null)
                //                {
                //                    if (no_bill != "")
                //                    {

                //                        draw_print a = new draw_print();
                //                        DataTable dt_1;
                //                        string sql_1 = @"select a.no_bill,c.no_bill_in as no_hetong,a.distance_car,c.name_product,d.name_company,
                //                    case when a.qty_send=a.qty_shajiang then e.name_qiangdu_ranks+'砂浆' when b.qty_chanheliao1=0 then e.name_qiangdu_ranks when a.qty_shajiang=0 then  e.name_qiangdu_ranks+' '+convert(varchar,b.qty_chanheliao1)+'%'+j.name_chanheliao else e.name_qiangdu_ranks end as name_qiangdu_ranks,f.name_type_bengsong,isnull(g.name_tanluodu,'') as name_tanluodu,
                //                    isnull(h.name_fahuolou,'') as name_fahuolou,case when a.qty_shajiang>0 and a.qty_shajiang<a.qty_send then  convert(varchar,a.qty_send)+'('+convert(varchar,a.qty_send)+'含'+convert(varchar,a.qty_shajiang)+')' when (a.qty_shajiang=0 or a.qty_shajiang=a.qty_send) then convert(varchar,a.qty_send) end as qty_send,a.qty_leiji,
                //                    a.no_car,i.name_driver,c.addr_sign,
                //                    convert(varchar(10),a.date_bill,121) as date_bill,a.person_make as pet_name,a.shigongbuwei,
                //                    convert(varchar(12),a.date_bill,108) as time_bill,a.remark,a.no_productorder,b.other,j.name_chanheliao,isnull(b.linktel,'') as linktel
                //                    from ls_send a 
                //                    join ls_productorder b on a.no_productorder=b.no_bill 
                //                    join ls_hetong c on b.no_hetong=c.no_bill 
                //                    join ls_shengoudan k on c.no_bill=k.no_hetong
                //                    join ls_company d on c.no_company=d.no_ls
                //                    join ls_qiangdu_ranks e on a.no_qiangdu_ranks=e.no_ls 
                //                    join ls_type_bengsong f on a.no_type_bengsong=f.no_ls
                //                    left join ls_tanluodu g on b.no_tanluodu=g.no_ls
                //                    left join ls_fahuolou h on a.no_fahuolou=h.no_ls
                //                    left join ls_driver i on a.no_driver=i.no_ls
                //                    left join ls_chanheliao j on b.no_chanheliao1=j.no_ls
                //                    where a.no_bill='" + no_bill + @"'";
                //                        string sql_2_1 = string.Format(@"select no_productorder,case when qty_send < 0 then -1 when qty_send > 0 then 1 end as checi, isnull(qty_send,0) as 
                //            qty_send from ls_send where no_productorder='" + no_productorder + "'");
                //                        string sql_2 = string.Format(@"select no_productorder , sum(checi) as checi, sum(qty_send) as qty_total from ({0}) a group by no_productorder", sql_2_1);



                //                        string sql_3 = "select sql_1.*,sql_2.* from (" + sql_1 + ")sql_1 left join (" + sql_2 + ")sql_2 on sql_1.no_productorder=sql_2.no_productorder";
                //                        dt_1 = db.GetDataSet(sql_3);
                //                        string sql_4_1 = string.Format("select 'a' as a,count(*) as checi_total from ls_send where date_bill between '" + DateTime.Now.ToShortDateString() + " 00:00:00" + "' and '" + DateTime.Now.ToShortDateString() + " 23:59:59" + "' and no_fahuolou='{0}'", BasicInfo.DataBase.no_fahuolou);
                //                        string sql_4_2 = string.Format("select 'a' as a,count(*)+count(*) as checi_total from ls_send where date_bill between '" + DateTime.Now.ToShortDateString() + " 00:00:00" + "' and '" + DateTime.Now.ToShortDateString() + " 23:59:59" + "' and ls_send.qty_send < 0 and no_fahuolou='{0}'", BasicInfo.DataBase.no_fahuolou);
                //                        string sql_4 = string.Format("select sql_4_1.checi_total - isnull(sql_4_2.checi_total,0) as checi_total from ({0})sql_4_1 left join ({1})sql_4_2 on sql_4_1.a=sql_4_2.a", sql_4_1, sql_4_2);
                //                        DataTable dt_2 = db.GetDataSet(sql_4);
                //                        a.add_dt_head(dt_1, 0);
                //                        a.add_dt_head(dt_2, 0);
                //                        config_function.print_do(a, this.Name);
                //                    }
                //                }

                no_send = no_bill;
                this_edit.reload_no_bill(null);
            }
            //btn_add.PerformClick();
            //car_load();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!textBox5.Text.Trim().Equals(""))
            {
                int index = this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[index].Cells[0].Value = textBox5.Text;
                textBox5.Text = "";
            }
            else
            {
                MessageBox.Show("车号不能为空！");
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgvRow in this.dataGridView1.SelectedRows) 
            {
                this.dataGridView1.Rows.Remove(dgvRow); 
            } 
    }

       
        private bool car_max()
        {
            UserDao dao = new UserDao();
            decimal car_max = dao.GetScalar_d(string.Format("select car_max_fangliang from ls_system_init"));
            decimal qty_send = BLL.decimal_1(txt_qty_send.Text);

            if (qty_send > car_max)
            {
                MessageBox.Show("单车发货量大于单车最大方量");
                return false;
            }
            return true;
        }
        private void shuzi_null()
        {
            //将数量清空
            this.txt_qty_fact.Text = "0";
            decimal qty_fact = BLL.decimal_1(this.txt_qty_fact.Text);
            decimal qty_from = BLL.decimal_1(this.txt_qty_from.Text);
            decimal qty_send = qty_fact + qty_from;
            decimal qty_send_total = sumQty2 + qty_send;
            decimal qty_leiji = sumQty + qty_send;
            this.txt_qty_send.Text = BLL.string_1(qty_send);
            //this.txt_qty_send.DataBindings[0].WriteValue();
            this.txt_qty_send_total.Text = BLL.string_1(qty_send_total);
            //this.txt_qty_send_total.DataBindings[0].WriteValue();
            this.txt_qty_leiji.Text = BLL.string_1(qty_leiji);
            //this.txt_qty_leiji.DataBindings[0].WriteValue();
        }
    }
}
