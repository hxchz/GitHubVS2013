using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using JTInterface.Dao;
using JTInterface.View;

namespace JTInterface
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            init();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                login();
                setDefaultUserName();
                FrmTask task = new FrmTask();
                FrmTasksplit task2 = new FrmTasksplit();
                task.Show();
                task2.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("登录失败！");
            }
            return;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void init()
        {
            textBox2.UseSystemPasswordChar = true;
            checkBox1.Checked = true;
            getDefaultUserName();
        }
        public void login()
        {
            string username = textBox1.Text.ToString().Trim();
            string password = textBox2.Text.ToString().Trim();
            int count = 0;
            UserDao userdao = new UserDao();
            count = userdao.Login(username, password);
            if (count > 0)
            {
                setDefaultUserName();
                if (checkBox1.Checked == true)
                {
                    setDefaultUserPassword();
                }
                else
                {
                    delDefaultUserPassword();
                }
                // MessageBox.Show("登录成功");
            }
            else
            {
                MessageBox.Show("用户名或密码错误！");
            }
        }
        public void getDefaultUserName()
        {
            XmlElement theUser = null, root = null;
            XmlDocument xmldoc = new XmlDocument();
            try
            {
                xmldoc.Load("UserName.xml");
                root = xmldoc.DocumentElement;
                theUser = (XmlElement)root.SelectSingleNode("/users/user");
                textBox1.Text = theUser.GetElementsByTagName("name").Item(0).InnerText;
                textBox2.Text = theUser.GetElementsByTagName("password").Item(0).InnerText;
            }
            catch (Exception ex)
            {
            }
        }
        public void setDefaultUserName()
        {
            XmlElement theUser = null, root = null;
            XmlDocument xmldoc = new XmlDocument();
            string username = textBox1.Text.ToString().Trim();
            try
            {
                xmldoc.Load("UserName.xml");
                root = xmldoc.DocumentElement;
                theUser = (XmlElement)root.SelectSingleNode("/users/user");
                theUser.GetElementsByTagName("name").Item(0).InnerText = username;
                xmldoc.Save("UserName.xml");
            }
            catch (Exception ex)
            {
            }
        }
        public void setDefaultUserPassword()
        {
            XmlElement theUser = null, root = null;
            XmlDocument xmldoc = new XmlDocument();
            string password = textBox2.Text.ToString().Trim();
            try
            {
                xmldoc.Load("UserName.xml");
                root = xmldoc.DocumentElement;
                theUser = (XmlElement)root.SelectSingleNode("/users/user");
                theUser.GetElementsByTagName("password").Item(0).InnerText = password;
                xmldoc.Save("UserName.xml");
            }
            catch (Exception ex)
            {
            }
        }
        public void delDefaultUserPassword()
        {
            XmlElement theUser = null, root = null;
            XmlDocument xmldoc = new XmlDocument();
            try
            {
                xmldoc.Load("UserName.xml");
                root = xmldoc.DocumentElement;
                theUser = (XmlElement)root.SelectSingleNode("/users/user");
                theUser.GetElementsByTagName("password").Item(0).InnerText = "";
                xmldoc.Save("UserName.xml");
            }
            catch (Exception ex)
            {
            }
        }
    }
}
