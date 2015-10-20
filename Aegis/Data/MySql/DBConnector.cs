﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;



namespace Aegis.Data.MySql
{
    internal sealed class DBConnector
    {
        public MySqlConnection Connection { get; private set; }
        public String ConnectionString { get { return Connection.ConnectionString; } }
        public IntervalCounter QPS { get; private set; }





        internal DBConnector()
        {
        }


        public void Connect(String hostIp, Int32 hostPortNo, String charSet, String dbName, String user, String pwd, Int32 commandTimeoutSec = 60)
        {
            if (Connection != null)
                return;


            QPS = new IntervalCounter(1000);
            Connection = new MySqlConnection(String.Format("Server={0};Port={1};CharSet={2};Database={3};Uid={4};Pwd={5};"
                                            , hostIp, hostPortNo, charSet, dbName, user, pwd));
            Connection.Open();


            /*using (DBCommand cmd = DBCommand.NewCommand(MySql))
            {
                cmd.QueryNoReader("set transaction isolation level read uncommitted;");
                cmd.QueryNoReader("set session wait_timeout=604800;set session interactive_timeout=604800;");
            }*/
        }


        public void Close()
        {
            if (Connection != null)
            {
                Connection.Close();
                Connection = null;
            }
        }


        public void Ping()
        {
            if (Connection != null)
                Connection.Ping();
        }
    }
}
