using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace NZBHags
{
    [Serializable()]
    public class NewsServer : ISerializable
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
        private QueueHandler handler;

        public NewsServer(string name, string addr, string username, string password, int port, int connections, int timeout)
        {
            this.name = name;
            this.addr = addr;
            this.username = username;
            this.password = password;
            this.port = port;
            this.connections = connections;
            this.timeout = timeout;
        }

        public void Connect()
        {
            Logging.Log("Connecting to {0}, spawning {1} connections", name, connections);
            nntpConnections = new NNTPConnection[connections];

            // test code
            for (int i = 0; i < connections; i++ )
            {
                nntpConnections[i] = new NNTPConnection(i, this, handler);
            }
            isConnected = true;
        }

        public void Disconnect()
        {
            Logging.Log("Disconnting from {0}", name);
            for (int i = 0; i < nntpConnections.Length; i++)
            {
                nntpConnections[i].Disconnect();
            }
            isConnected = false;
        }

        public NewsServer(QueueHandler handler)
        {
            this.handler = handler;
            name = "";
            addr = "";
            username = "";
            password = "";
        }

        public NewsServer(SerializationInfo info, StreamingContext ctxt)
        {
            name = (string)info.GetValue("name", typeof(string));
            addr = (string)info.GetValue("addr", typeof(string));
            username = (string)info.GetValue("username", typeof(string));
            password = (string)info.GetValue("password", typeof(string));
            port = (int)info.GetValue("port", typeof(int));
            connections = (int)info.GetValue("connections", typeof(int));
            timeout = (int)info.GetValue("timeout", typeof(int));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("name", name);
            info.AddValue("addr", addr);
            info.AddValue("username", username);
            info.AddValue("password", password);
            info.AddValue("port", port);
            info.AddValue("connections", connections);
            info.AddValue("timeout", timeout);
        }
        public void Save()
        {
            Stream stream = File.Open(name +".ini", FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();

            Logging.Log("Saving NewsServer information for " + name);
            formatter.Serialize(stream, this);
            stream.Close();
        }

        public static NewsServer Load(string filename, QueueHandler handler)
        {
            try
            {
                
                Stream stream = File.Open(filename, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();

                NewsServer server = (NewsServer)formatter.Deserialize(stream);
                server.handler = handler;
                stream.Close();
                return server;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
