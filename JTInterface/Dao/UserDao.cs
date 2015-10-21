using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Text;
using System;
using JTInterface.Dao;
using System.Data.SqlClient;

namespace JTInterface.Dao
{
    class UserDao
    {
        public DataBase.DBOperate dboperate;
        public DataTable allRecord = new DataTable();
        public string getConnectString()
        {
            string ConnString;
            ConStr con = new ConStr();
            ConnString = con.getConnectString();
            return ConnString;
        }
        public DataTable SelectAll()
        {
            dboperate = new DataBase.DBOperate();
            DataTable dt = new DataTable();
            string ConnString = getConnectString();
            dboperate.ConnectString = ConnString;
            StringBuilder sql = new StringBuilder(@"select distinct x.* from ( select 0 as AutoID,a.no_bill as no_bill,a.date_bill as date_bill,a.no_bill_in as no_bill_in,a.supply_time as 
supply_time,b.no_hetong__1,b.no_hetong__2__1,b.no_hetong__2__2,b.no_hetong__3,b.no_hetong__4,e.no_fahuolou__1,a.shigongbuwei as 
shigongbuwei,a.qty_yuding as qty_yuding,a.tongqiangdu_date as 
tongqiangdu_date,g.no_qiangdu_ranks__1,i.no_type_bengsong__1,k.no_guliaomax__1,a.kangzhe as kangzhe,a.bengche as 
bengche,m.no_tanluodu__1,a.gongyingpinlv as gongyingpinlv,o.no_guanzhuang__1,a.linkman as linkman,a.linktel as 
linktel,q.no_chanheliao1__1,a.qty_chanheliao1 as qty_chanheliao1,s.no_chanheliao2__1,a.qty_chanheliao2 as 
qty_chanheliao2,u.no_chanheliao3__1,a.qty_chanheliao3 as qty_chanheliao3,a.other as other,a.done_time as done_time,a.person_make as 
person_make,a.person_check as person_check,a.person_modify as person_modify,a.date_make as date_make,a.date_check as date_check,a.date_modify 
as date_modify,a.no_shengoudan as no_shengoudan,w.no_kangshen__1,a.isno_peibi as isno_peibi,a.check_bad_times as check_bad_times,a.check_remark 
as check_remark,y.check_state__1,y.check_state__2,a.done_state as done_state,a.price_unit_chanheliao1 as 
price_unit_chanheliao1,a.price_unit_chanheliao2 as price_unit_chanheliao2,a.price_unit_chanheliao3 as price_unit_chanheliao3,a.is_lingxing as 
is_lingxing,a.checi as checi,a.yaoqiu as yaoqiu,a.qty_shajiang as qty_shajiang,a.no_gongkongji as no_gongkongji,a.is_guanzhuang as 
is_guanzhuang,a1.no_benghao__1,c1.no_bengrenyuan__1,a.is_shajiang as is_shajiang,a.qty_rank as qty_rank,a.qty_rank_shajiang as 
qty_rank_shajiang,e1.no_employee1__1,g1.no_employee2__1,i1.no_employee3__1,k1.no_employee4__1,m1.no_employee5__1,o1.no_employee6__1,q1.no_car1__1,s1.no_car2__1,u1.no_car3__1,w1.no_car4__1,y1.no_car5__1,a2.no_car6__1,a.qty1 
as qty1,a.qty2 as qty2,a.qty3 as qty3,a.qty4 as qty4,a.qty5 as qty5,a.qty6 as qty6 from (select * from ls_productorder  where  check_state='2' 
and isno_peibi='1' and done_state='0' and (supply_time between DATEADD(day,-2,getdate()) and DATEADD(day,0,getdate())) ) a  left outer join  ( 
select c.no_bill,c.no_bill as no_hetong__1,d.no_hetong__2__1 as no_hetong__2__1,d.no_hetong__2__2 as no_hetong__2__2,c.name_product as 
no_hetong__3,c.no_bill_in as no_hetong__4 from ls_hetong c     left outer join  ( select e.no_ls,e.no_ls as no_hetong__2__1,e.name_company as 
no_hetong__2__2 from ls_company e      ) d on d.no_ls=c.no_company      ) b on b.no_bill=a.no_hetong left outer join  ( select 
f.no_ls,f.name_fahuolou as no_fahuolou__1 from ls_fahuolou f    ) e on e.no_ls=a.no_fahuolou left outer join  ( select 
h.no_ls,h.name_qiangdu_ranks as no_qiangdu_ranks__1 from ls_qiangdu_ranks h    ) g on g.no_ls=a.no_qiangdu_ranks left outer join  ( select 
j.no_ls,j.name_type_bengsong as no_type_bengsong__1 from ls_type_bengsong j    ) i on i.no_ls=a.no_type_bengsong left outer join  ( select 
l.no_ls,l.name_guliaomax as no_guliaomax__1 from ls_guliaomax l    ) k on k.no_ls=a.no_guliaomax left outer join  ( select 
n.no_ls,n.name_tanluodu as no_tanluodu__1 from ls_tanluodu n    ) m on m.no_ls=a.no_tanluodu left outer join  ( select 
p.no_ls,p.name_guanzhuang as no_guanzhuang__1 from ls_guanzhuang p    ) o on o.no_ls=a.no_guanzhuang left outer join  ( select 
r.no_ls,r.name_chanheliao as no_chanheliao1__1 from ls_chanheliao r    ) q on q.no_ls=a.no_chanheliao1 left outer join  ( select 
t.no_ls,t.name_chanheliao as no_chanheliao2__1 from ls_chanheliao t    ) s on s.no_ls=a.no_chanheliao2 left outer join  ( select 
v.no_ls,v.name_chanheliao as no_chanheliao3__1 from ls_chanheliao v    ) u on u.no_ls=a.no_chanheliao3 left outer join  ( select 
x.no_ls,x.name_kangshen as no_kangshen__1 from ls_kangshen x    ) w on w.no_ls=a.no_kangshen left outer join  ( select 
z.check_state,z.check_state as check_state__1,z.name_check_state as check_state__2 from ls_sys_check_state z      ) y on 
y.check_state=a.check_state left outer join  ( select b1.no_ls,b1.no_ls as no_benghao__1 from ls_car b1    ) a1 on a1.no_ls=a.no_benghao left 
outer join  ( select d1.no_ls,d1.name_driver as no_bengrenyuan__1 from ls_driver d1    ) c1 on c1.no_ls=a.no_bengrenyuan left outer join  ( 
select f1.no_ls,f1.name_employee as no_employee1__1 from ls_employee f1    ) e1 on e1.no_ls=a.no_employee1 left outer join  ( select 
h1.no_ls,h1.name_employee as no_employee2__1 from ls_employee h1    ) g1 on g1.no_ls=a.no_employee2 left outer join  ( select 
j1.no_ls,j1.name_employee as no_employee3__1 from ls_employee j1    ) i1 on i1.no_ls=a.no_employee3 left outer join  ( select 
l1.no_ls,l1.name_employee as no_employee4__1 from ls_employee l1    ) k1 on k1.no_ls=a.no_employee4 left outer join  ( select 
n1.no_ls,n1.name_employee as no_employee5__1 from ls_employee n1    ) m1 on m1.no_ls=a.no_employee5 left outer join  ( select 
p1.no_ls,p1.name_employee as no_employee6__1 from ls_employee p1    ) o1 on o1.no_ls=a.no_employee6 left outer join  ( select r1.no_ls,r1.no_ls 
as no_car1__1 from ls_car r1    ) q1 on q1.no_ls=a.no_car1 left outer join  ( select t1.no_ls,t1.no_ls as no_car2__1 from ls_car t1    ) s1 on 
s1.no_ls=a.no_car2 left outer join  ( select v1.no_ls,v1.no_ls as no_car3__1 from ls_car v1    ) u1 on u1.no_ls=a.no_car3 left outer join  ( 
select x1.no_ls,x1.no_ls as no_car4__1 from ls_car x1    ) w1 on w1.no_ls=a.no_car4 left outer join  ( select z1.no_ls,z1.no_ls as no_car5__1 
from ls_car z1    ) y1 on y1.no_ls=a.no_car5 left outer join  ( select b2.no_ls,b2.no_ls as no_car6__1 from ls_car b2    ) a2 on 
a2.no_ls=a.no_car6 ) x  order by x.date_bill desc");
            dt = dboperate.GetDataTable(sql.ToString().Trim());
            return dt;
        }
        public DataTable SelectSend(string sdate,string edate)
        {
            dboperate = new DataBase.DBOperate();
            DataTable dt = new DataTable();
            string ConnString = getConnectString();
            dboperate.ConnectString = ConnString;
            string sql =string.Format(@"select top 200 sql_1.*,isnull(sql_total.qty_leiji,0) as qty_leiji from (select ls_productorder.no_bill as no_productorder,convert(bit,0) as 
is_wancheng,ls_hetong.no_bill as no_hetong,
                                ls_hetong.no_bill_in as no_bill_in,ls_peibidan.no_bill as no_peibidan,ls_productorder.no_fahuolou,
                                ls_company.name_company,ls_hetong.name_product,ls_productorder.shigongbuwei,ls_productorder.qty_yuding,
                                ls_productorder.supply_time,ls_productorder.gongyingpinlv ,ls_productorder.no_qiangdu_ranks,
                                ls_qiangdu_ranks.name_qiangdu_ranks,ls_productorder.no_type_bengsong,ls_type_bengsong.name_type_bengsong,
                                
ls_guanzhuang.name_guanzhuang,ls_productorder.linkman,ls_productorder.linktel,done_time,ls_productorder.no_benghao,
                                ls_productorder.no_bengrenyuan,chanheliao1.name_chanheliao as no_chanheliao1__1,chanheliao2.name_chanheliao as 
no_chanheliao2__1,ls_productorder.qty_chanheliao1,ls_productorder.qty_chanheliao2,ls_hetong.distance_car
                                from ls_productorder 
                                left join ls_hetong on ls_productorder.no_hetong=ls_hetong.no_bill 
                                left join ls_peibidan on ls_peibidan.no_product=ls_productorder.no_bill 
                                left join ls_company on ls_company.no_ls=ls_hetong.no_company 
                                left join ls_qiangdu_ranks on ls_qiangdu_ranks.no_ls=ls_productorder.no_qiangdu_ranks 
                                left join ls_type_bengsong on ls_type_bengsong.no_ls=ls_productorder.no_type_bengsong 
                                left join ls_guanzhuang on ls_guanzhuang.no_ls=ls_productorder.no_guanzhuang
                                left join ls_chanheliao chanheliao1 on ls_productorder.no_chanheliao1=chanheliao1.no_ls
                                left join ls_chanheliao chanheliao2 on ls_productorder.no_chanheliao2=chanheliao1.no_ls
                                where 1=1 and done_state = 0 and done_time is not null  and done_time is not null ) sql_1 
                                            left join (select ls_send.no_productorder,sum(isnull(qty_send,0)) as qty_leiji from ls_send where 
ls_send.date_bill 
                                    between '{0}' and '{1}' 
                                    and no_fahuolou='1#' group by ls_send.no_productorder) sql_total on 
sql_1.no_productorder=sql_total.no_productorder 
                                            order by done_time desc "
                ,sdate
                ,edate
                );
            dt = dboperate.GetDataTable(sql.ToString().Trim());
            return dt;
        }
        public int Login(string username,string password)
        {
            dboperate = new DataBase.DBOperate();
            int count = 0;
            string ConnString = getConnectString();
            dboperate.ConnectString = ConnString;
            string sql = string.Format(@"
                            select count(*) from ls_sys_account
                            where username='{0}'
                            and password='{1}'
                    "
                   , username
                   , password
                   );
            count=Convert.ToInt32(dboperate.ExecuteScalar(sql.ToString().Trim()));
            return count;
        }
        public DataTable getCarInfo()
        {
            DataTable dt = new DataTable();
            dboperate = new DataBase.DBOperate();
            string ConnString = getConnectString();
            dboperate.ConnectString = ConnString;
            string sql = string.Format(@"
                            select a.id,a.carid,'厂内' as locationstatus from gpscartmp a
                            inner join (select max(id) as maxid,carid from gpscartmp
                            group by carid) b on a.id=b.maxid
                            where a.locationstatus=4
                            order by a.carid
                    "
                   );
            dt = dboperate.GetDataTable(sql.ToString().Trim());
            return dt;
        }
        public DataTable getCar()
        {
            DataTable dt = new DataTable();
            dboperate = new DataBase.DBOperate();
            string ConnString = getConnectString();
            dboperate.ConnectString = ConnString;
            string sql = string.Format(@"
                            select ownercode as carid,intime from GpsBusQueue
                            where Isfactory=1 and istask=0
                    "
                   );
            dt = dboperate.GetDataTable(sql.ToString().Trim());
            return dt;
        }
        public DataTable getRequire(string no_bill)
        {
            DataTable dt = new DataTable();
            dboperate = new DataBase.DBOperate();
            string ConnString = getConnectString();
            dboperate.ConnectString = ConnString;
            string sql = string.Format(@"
                            select * from ls_send_require
                            where no_bill='{0}'
                    ",no_bill
                   );
            dt = dboperate.GetDataTable(sql.ToString().Trim());
            return dt;
        }
        public DataTable getYupai()
        {
            DataTable dt = new DataTable();
            dboperate = new DataBase.DBOperate();
            string ConnString = getConnectString();
            dboperate.ConnectString = ConnString;
            string sdate = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00";
            string edate = DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59";
            string sql = string.Format(@"
                            select (case no_type_bengsong when '01' then '非泵' when '02' then '泵送' end) as name_bengsong,* from ls_send_yupai
                            where date_bill between '{0}' and '{1}' 
                            and isdel=0
                    ", sdate,edate
                   );
            dt = dboperate.GetDataTable(sql.ToString().Trim());
            return dt;
        }
        public DataTable GetScalar(string sql)
        {
            DataTable dt = new DataTable();
            dboperate = new DataBase.DBOperate();
            string ConnString = getConnectString();
            dboperate.ConnectString = ConnString;
            dt = dboperate.GetDataTable(sql.ToString().Trim());
            return dt;
        }
        public string GetScalar_s(string sql)
        {
            string dt;
            dboperate = new DataBase.DBOperate();
            string ConnString = getConnectString();
            dboperate.ConnectString = ConnString;
            dt = dboperate.GetDataTable(sql.ToString().Trim()).ToString();
            return dt;
        }
        public decimal GetScalar_d(string sql)
        {
            decimal dt;
            try
            {
                dboperate = new DataBase.DBOperate();
                string ConnString = getConnectString();
                dboperate.ConnectString = ConnString;
                dt = Convert.ToDecimal(dboperate.ExecuteScalar(sql.ToString().Trim()));
                return dt;
            }
            catch (Exception ex)
            {
                dt = 0;
                return dt;
            }
            
        }
        public DataTable getSanYCarInfo()
        {
            DataTable dt = new DataTable();
            dboperate = new DataBase.DBOperate();
            ConStr con = new ConStr();
            string ConnString = con.getSANYConnectString();
            dboperate.ConnectString = ConnString;
            string sql = string.Format(@"
                            select b.companyname,b.deliveryname,b.truckid,b.quantity,b.itemno from  curout a
                            inner join outmaster b on a.deliveryid=b.deliveryid
                    "
                   );
            dt = dboperate.GetDataTable(sql.ToString().Trim());
            return dt;
        }
        public string no_bill(string s, string tablename)
        {
            DataTable reader = new DataTable();
            dboperate = new DataBase.DBOperate();
            string ConnString = getConnectString();
            dboperate.ConnectString = ConnString;
            string date_now = DateTime.Now.ToString("yyyyMMdd");
            string no_bill = "";
            string sql = "select no_bill from " + tablename + " where substring(no_bill,1,8)='" + date_now + "' order by no_bill desc";
            reader = dboperate.GetDataTable(sql);
            try
            {
                if (reader != null)
                {
                    no_bill = reader.Rows[0]["no_bill"].ToString().Trim();
                    if (no_bill.Length > 4)
                    {
                        string bill_1 = no_bill.Substring(0, no_bill.Length - 4);
                        if (bill_1 == DateTime.Now.ToString("yyyyMMdd"))
                        {
                            string bill_2 = no_bill.Substring(no_bill.Length - 3, 3);
                            int no = Convert.ToInt32(bill_2) + 1;
                            if (no < 10)
                            {
                                bill_2 = "00" + no.ToString();
                            }
                            else if (no < 100)
                            {
                                bill_2 = "0" + no.ToString();
                            }
                            no_bill = bill_1 + "-" + bill_2;
                        }
                        else
                        {
                            no_bill = DateTime.Now.ToString("yyyyMMdd") + "-001";
                        }
                    }
                    else
                    {
                        no_bill = DateTime.Now.ToString("yyyyMMdd") + "-001";
                    }
                }
                else
                {
                    no_bill = DateTime.Now.ToString("yyyyMMdd") + "-001";
                }
            }
            catch(Exception ex)
            {
                no_bill = DateTime.Now.ToString("yyyyMMdd") + "-001";
            }
            no_bill = s + no_bill;
            return no_bill;
        }
    }
}
