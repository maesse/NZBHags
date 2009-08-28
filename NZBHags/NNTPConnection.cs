using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Security.Cryptography;


namespace NZBHags
{
    public class NNTPConnection : System.Net.Sockets.TcpClient
    {
        public bool keepAlive { get; set; }
        public bool idle { get; set; }
        public uint speed { get; set; }
        public Segment currentSegment { get; set; }
        public MainGUI mainGui;

        private NewsServer serverInfo;
        private QueueHandler handler;
        private byte[] endbytes = { (byte)'\r', (byte)'\n' };
        private byte[] endbytes2 = { (byte)'.', (byte)'\r', (byte)'\n'};
        public int id;
        System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
        Performance.Stopwatch sw;
        public NNTPConnection(int id, NewsServer serverInfo, QueueHandler handler)
        {
            this.id = id;
            this.serverInfo = serverInfo;
            this.handler = handler;
            keepAlive = true;
            sw = new Performance.Stopwatch();

            // Register connection in SpeedMonitor
            SpeedMonitor.Instance.RegisterConnection(this);

            ThreadStart job = new ThreadStart(Run);
            Thread thread = new Thread(job);
            thread.IsBackground = true;
            thread.Start();
        }

        private void Login()
        {
            string response;
            try
            {
                Connect(serverInfo.addr, serverInfo.port);
                response = Response().Substring(0, 3);

                if (!response.Equals("200") && !response.Equals("201"))
                {
                    // abort
                    Logging.Instance.Log("(NNTPConnection({0})): Didn't get expected resonse.. got: {1}", id, response);
                    Disconnect();
                    return;
                }

                Write("authinfo user " + serverInfo.username + '\n');
                response = Response();
                Assert(response, "381"); // Pass required

                Write("authinfo pass " + serverInfo.password + '\n');
                response = Response();
                Assert(response, "281"); // Ok

                idle = true;
                Logging.Instance.Log("(NNTPConnection({0})): Connected to server.", id);
            }
            catch (Exception ex)
            {
                Disconnect();
            }
        }
        

        public void Run()
        {
            Login();

            Segment segment;
            while (keepAlive)
            {
                // Check Queuehandler..
                if ((segment = handler.getNextQueueItem()) != null)
                {
                    currentSegment = segment;
                    segment.status = Segment.Status.DOWNLOADING;
                    idle = false;
                    try
                    {
                        segment.data = RecieveSegment(segment);
                        YDecoder.Instance.DecodeSegment(segment);
                    }
                    catch (Exception ex)
                    {

                    }
                    idle = true;
                    currentSegment = null;
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }

        

        public byte[] RecieveSegment(Segment segment)
        {
            try
            {
                // Request body...

                Write("BODY "+ segment.addr +'\n');
                Assert(Response(), "222");

                NetworkStream stream = GetStream();
                    
                stream.ReadTimeout = 10000;
                
                byte[] buffer = new byte[segment.bytes];
                int read = 0;
                bool done = false;
                int chunk;
            
                while (!done && (chunk = stream.Read(buffer, read, buffer.Length - read)) > 0 && keepAlive)
                {
                    read += chunk;
                    segment.parent.parent.ProgressKB += (ulong)chunk;
                    segment.progress += chunk;
                    lock (this)
                    {
                        speed += (uint)chunk;
                    }
                    if (read == buffer.Length)
                    {
                        byte[] newBuffer = new byte[buffer.Length * 2];
                        Array.Copy(buffer, newBuffer, buffer.Length);
                        buffer = newBuffer;
                    }
                    mainGui.UpdateSingleThreadInfo(this);
                    // Looks for .\r\n (End of message)
                    if (read > 2 && buffer[read - 3] == '.' && buffer[read - 2] == '\r' && buffer[read - 1] == '\n')
                    {
                        // Check that it is in the beginning of a new line
                        if (read < 4 || buffer[read - 5] == '\r' && buffer[read - 4] == '\n')
                        {
                            break;
                        }
                    }
                }
                if (!keepAlive)
                {
                    throw new Exception();
                }
                byte[] returnarray = new byte[read - 3];
                Array.Copy(buffer, returnarray, read - 3);
                segment.bytes = read;
                Logging.Instance.Log("(NNTPConnection(" + id + ")) SEGMENT Complete: id=" + segment.id + " bytes=" + read);
                return returnarray;
            }
            catch (IOException ex)
            {
                throw;
            }
            
        }

        // Test for a particular responsecode
        private void Assert(string response, string str)
        {
            if (response == "\n")
            {
                response = Response();
            }
            if (!response.Substring(0, 3).Equals(str))
            {
                // abort
                Logging.Instance.Log("(NNTPConnection(" + id + ")) Could not connect... got message: " + response);
                Disconnect();
                return;
            }
        }

        private void Write(string message)
        {
            try
            {
                sw.Reset();
                sw.Start();
                System.Text.ASCIIEncoding en = new System.Text.ASCIIEncoding();

                byte[] WriteBuffer = new byte[message.Length];
                WriteBuffer = en.GetBytes(message);

                NetworkStream stream = GetStream();
                stream.Write(WriteBuffer, 0, WriteBuffer.Length);
                sw.Stop();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string getValue(string line, string parm)
        {
            if (line.Contains(parm + "="))
            {
                if (parm == "name")
                {
                    return line.Substring(line.IndexOf("name=") + 5);
                }
                string ret = line.Substring(line.IndexOf(parm + "=") + 1 + parm.Length);
                int i = ret.IndexOf(" ");
                if (i == -1)
                    i = ret.Length;
                return ret.Substring(0, i).Trim();
            }
            return null;
        }

        private string Response()
        {
            sw.Reset();
            sw.Start();
            NetworkStream stream = GetStream();
            byte[] buffer = new byte[1024];
            int pos = 0;

            try
            {
                while ((stream.Read(buffer, pos, 1)) > 0 && keepAlive)
                {

                    if (buffer[pos] == '\n')
                        break;
                    pos++;
                }
            }
            catch (IOException ex) 
            {
                throw;
            }
            string returnstring = enc.GetString(buffer, 0, pos);
            sw.Stop();
            return returnstring;
        }

        public void Disconnect()
        {
            // DeRegister connection in SpeedMonitor
            SpeedMonitor.Instance.DeRegisterConnection(this);
            keepAlive = false;
            int MaxWait = 1000; // 1 sec
            int AccWait = 0; // Accumulated wait
            int WaitStep = 10; // 10ms between checks
            while (!idle && AccWait < MaxWait)
            {
                AccWait += WaitStep;
                Thread.Sleep(WaitStep);
            }
            Close();
        }
    }
}
