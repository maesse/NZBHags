using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace NZBHags
{
    public sealed class YDecoder
    {
        static readonly YDecoder instance = new YDecoder();
        System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
        private Queue segments;
        public bool keepRunning { get; set; } 
        private int[] special = {0, 9, 10, 13, 27, 32, 46, 61};

        YDecoder()
        {
            segments = new Queue();
            keepRunning = true;

            ThreadStart job = new ThreadStart(Run);
            Thread thread = new Thread(job);
            thread.IsBackground = true;
            thread.Start();
        }

        public void DecodeSegment(Segment segment)
        {
            segment.status = Segment.Status.YDECODING;
            lock (segments)
            {
                segments.Enqueue(segment);
            }
        }

        private string ReadLine(Stream stream)
        {
            byte[] temp = new byte[1024];
            int pos = 0;
            byte[] buf = new byte[1];
            while(stream.Read(buf, 0, 1) > 0) {
                if(buf[0] == '\n')
                    break;

                temp[pos] = buf[0];
                pos++;
            }
            if (pos > 0 && temp[pos - 1] == '\r')
                pos--;
            return enc.GetString(temp, 0, pos);
        }

        private string EnsureGoodName(string file)
        {
            string[] forbidden = {"(", ")", "*", "%", "^" };
            for (int i = 0; i < forbidden.Length; i++)
            {
                file.Replace(forbidden[i], "-");
            }
            return file;
        }

        private void ProcessSegment(Segment segment)
        {
            int expectedlenght = 0; // TODO: Check accuracy of this value
            

            MemoryStream ms = new MemoryStream(segment.data);
            
            string header = ReadLine(ms);
            while(header.Equals(""))
                header = ReadLine(ms);

            int ysize = 0;  // TODO: Check accuracy of this value

            if (header.Contains("=ybegin ") && header.Contains("line=") && header.Contains("size=") && header.Contains("name="))
            {
                try
                {
                    segment.isYenc = true;
                    ysize = int.Parse(getValue(header, "size"));
                    segment.yname = getValue(header, "name");
                    segment.yname = EnsureGoodName(segment.yname);
                    segment.parent.filename = segment.yname;
                    if (header.Contains("part="))
                    {
                        segment.ypart = int.Parse(getValue(header, "part"));
                        if (segment.ypart > segment.parent.yparts)
                            segment.parent.yparts = segment.ypart;

                        string partheader = ReadLine(ms);
                        if (partheader.Contains("=ypart ") && partheader.Contains("begin=") && partheader.Contains("end="))
                        {
                            int partBegin = int.Parse(getValue(partheader, "begin"));
                            int partEnd = int.Parse(getValue(partheader, "end"));
                            expectedlenght = partEnd - partBegin;
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                }
            }

            if (!segment.isYenc)
            {
                segment.bytes = segment.data.Length;
                WriteCache.Instance.addSegment(ref segment);
                return;
            }
            else
            {

                // Start decoding data...
                byte[] buffer = new byte[1];
                byte[] output = new byte[segment.data.Length];
                bool cr = false, dot = false, finished = false, decode = true, nl = false, bleh2 = false; // flags
                int bufpos = 0;

                // Decode byte-for-byte
                while (ms.Read(buffer, 0, 1) > 0 && !finished)
                {
                    if (cr && buffer[0] == '\n')
                    {
                        // Discard \r\n by going back one byte and discarding current byte
                        bufpos--;
                        decode = false;
                    }
                    else if (nl && dot && buffer[0] == '.')
                    {
                        // Discard current (2nd) dot
                        decode = false;
                    }
                    if(buffer[0] != '.')
                        nl = false;

                    if (cr && buffer[0] == '\n')
                    {
                        nl = true;
                    }
                    // Reset flags
                    cr = false;
                    dot = false;
                    // Set flags
                    switch ((int)buffer[0])
                    {
                        case '=':
                            ms.Read(buffer, 0, 1);
                            if (buffer[0] == 'y')
                            {
                                string yend = ReadLine(ms);
                                if (segment.ypart != 0)
                                    segment.crc = getValue(yend, "pcrc32");
                                else
                                    segment.crc = getValue(yend, "crc32");
                                finished = true;
                                break;
                            } else {
                                for (int i = 0; i < special.Length; i++)
                                {
                                    if(buffer[0] == special[i]+64)
                                        buffer[0] = (byte)(char)special[i];
                                }
                            }
                            break;
                        case '\r':
                            cr = true;
                            break;
                        case '.':
                            dot = true;
                            break;

                    }
                    if (decode && !finished)
                    {
                        // Decode byte
                        buffer[0] -= 42 %256;
                        output[bufpos] = buffer[0];

                        bufpos++;
                    }
                    
                    decode = true;
                }
                byte[] data = new byte[bufpos];
                Array.Copy(output, data, bufpos);
                segment.data = data;
                segment.bytes = segment.data.Length;
                segment.ValidateCRC();
                WriteCache.Instance.addSegment(ref segment);
                
                
            }
        }

        // Thread loop
        private void Run()
        {
            bool sleep = false;
            Segment segment = null;

            while (keepRunning)
            {
                // Only hold lock for as little time as necessary
                lock (segments)
                {
                    if (segments.Count > 0)
                        segment = (Segment)segments.Dequeue();
                    else
                        sleep = true;
                }
                if (segment != null)
                {
                    ProcessSegment(segment);
                    segment = null;
                }
                if (sleep)
                {
                    Thread.Sleep(1000);
                    sleep = false;
                }
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

        // Finishes decoding and returns
        public void Shutdown()
        {
            keepRunning = false;
            lock (segments)
            {
                foreach (Segment seg in segments)
                {
                    ProcessSegment(seg);
                }
                segments.Clear();
                System.Console.WriteLine("(YDecoder) Shutting down...");
                return;
            }
            
        }

        // Singleton implementation
        public static YDecoder Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
