using System;
using System.Collections.Generic;
using System.Web;
using System.Xml;

namespace JTInterface.Dao
{
    public class ConStr
    {
        public string getConnectString()
        {
            string ConnString;
            string ServerName;
            string DBName;
            string User;
            string Password;
            XmlElement theUser = null, root = null;
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load("DBconfig.xml");
            root = xmldoc.DocumentElement;
            theUser = (XmlElement)root.SelectSingleNode("/dbs/db[name='ERP']");
            ServerName = theUser.GetElementsByTagName("ServerName").Item(0).InnerText;
            DBName = theUser.GetElementsByTagName("DBName").Item(0).InnerText;
            User = theUser.GetElementsByTagName("User").Item(0).InnerText;
            Password = theUser.GetElementsByTagName("Password").Item(0).InnerText;
            ConnString = DataBase.Base_SQLServerOp.GetSQLServerOleConnectString(ServerName, DBName, User, Password);
            return ConnString;
        }
        public string getSANYConnectString()
        {
            string ConnString;
            string ServerName;
            string DBName;
            string User;
            string Password;
            XmlElement theUser = null, root = null;
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load("DBconfig.xml");
            root = xmldoc.DocumentElement;
            theUser = (XmlElement)root.SelectSingleNode("/dbs/db[name='测试']");
            ServerName = theUser.GetElementsByTagName("ServerName").Item(0).InnerText;
            DBName = theUser.GetElementsByTagName("DBName").Item(0).InnerText;
            User = theUser.GetElementsByTagName("User").Item(0).InnerText;
            Password = theUser.GetElementsByTagName("Password").Item(0).InnerText;
            ConnString = DataBase.Base_SQLServerOp.GetSQLServerOleConnectString(ServerName, DBName, User, Password);
            return ConnString;
        }
    }
}