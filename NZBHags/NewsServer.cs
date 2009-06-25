using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace NZBHags
{
    //[Serializable()]
    public class NewsServer// : ISerializable
    {
        public bool isConnected { get; set; }
        public string name {get; set; }
        public string addr {get; set;}
        public string username { get; set; }
        public string password { get; set; }
        public int port { get; set; }
        public int connections { get; set; }
        public int timeout { get; set; }
        public NNTPConnection[] nntpConnections { get; set; }

        public NewsServer()
        {
            name = Properties.Settings.Default.servername;
            addr = Properties.Settings.Default.serveraddr;
            username = Properties.Settings.Default.serveruser;
            password = Properties.Settings.Default.serverpass;
            port = Properties.Settings.Default.serverport;
            connections = Properties.Settings.Default.serverconnections;
            timeout = Properties.Settings.Default.servertimeout;
        }


        // Save to properties
        public void Save()
        {
            Properties.Settings.Default.servername = name;
            Properties.Settings.Default.serveraddr = addr;
            Properties.Settings.Default.serveruser = username;
            Properties.Settings.Default.serverpass = password;
            Properties.Settings.Default.serverport = port;
            Properties.Settings.Default.serverconnections = connections;
            Properties.Settings.Default.servertimeout = timeout;
            Properties.Settings.Default.Save();
        }

        public void Connect()
        {
            Logging.Log("Connecting to {0}, spawning {1} connections", name, connections);
            nntpConnections = new NNTPConnection[connections];

            // test code
            for (int i = 0; i < connections; i++ )
            {
                nntpConnections[i] = new NNTPConnection(i, this, QueueHandler.Instance);
            }
            isConnected = true;
        }

        public void Disconnect()
        {
            Logging.Log("Disconnting from {0}", name);
            if (nntpConnections != null)
            {
                foreach (NNTPConnection conn in nntpConnections)
                {
                    conn.Disconnect();
                }
            }
            isConnected = false;
        }

        

        //public NewsServer(SerializationInfo info, StreamingContext ctxt)
        //{
        //    name = (string)info.GetValue("name", typeof(string));
        //    addr = (string)info.GetValue("addr", typeof(string));
        //    username = (string)info.GetValue("username", typeof(string));
        //    password = (string)info.GetValue("password", typeof(string));
        //    port = (int)info.GetValue("port", typeof(int));
        //    connections = (int)info.GetValue("connections", typeof(int));
        //    timeout = (int)info.GetValue("timeout", typeof(int));
        //}

        //public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        //{
        //    info.AddValue("name", name);
        //    info.AddValue("addr", addr);
        //    info.AddValue("username", username);
        //    info.AddValue("password", password);
        //    info.AddValue("port", port);
        //    info.AddValue("connections", connections);
        //    info.AddValue("timeout", timeout);
        //}
        //public void Save()
        //{
        //    Stream stream = File.Open(name +".ini", FileMode.Create);
        //    BinaryFormatter formatter = new BinaryFormatter();

        //    Logging.Log("Saving NewsServer information for " + name);
        //    formatter.Serialize(stream, this);
        //    stream.Close();
        //}

        //public static NewsServer Load(string filename)
        //{
        //    try
        //    {
                
        //        Stream stream = File.Open(filename, FileMode.Open);
        //        BinaryFormatter formatter = new BinaryFormatter();

        //        NewsServer server = (NewsServer)formatter.Deserialize(stream);
        //        stream.Close();
        //        return server;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

    }
}
