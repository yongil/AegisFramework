﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis;
using Aegis.Network;
using Aegis.Data.MySql;



namespace EchoServer.Logic
{
    public class ServerMain
    {
        public static ServerMain Instance { get { return Singleton<ServerMain>.Instance; } }
        public static MySqlDatabase MySql = new MySqlDatabase();




        private ServerMain()
        {
        }


        public void StartServer(System.Windows.Forms.TextBox ctrl)
        {
            //  Logger 설정
            Logger.AddLogger(new LogTextBox(ctrl));


            try
            {
                Logger.Write(LogType.Info, 2, "EchoServer (Aegis {0})", Aegis.Configuration.Environment.AegisVersion);

                Starter.Initialize(1, "./Config.xml");
                Starter.StartNetwork();
            }
            catch (Exception e)
            {
                Logger.Write(LogType.Err, 2, e.ToString());
            }
        }


        public void StopServer()
        {
            Starter.Release();
            Logger.Release();
        }


        public Int32 GetActiveSessionCount()
        {
            lock (NetworkChannel.Channels)
            {
                NetworkChannel channel = NetworkChannel.Channels.Find(v => v.Name == "NetworkClient");
                if (channel == null)
                    return 0;

                return channel.SessionManager.ActiveSessionCount;
            }
        }
    }
}
