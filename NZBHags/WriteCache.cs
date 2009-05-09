using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace NZBHags
{
    public sealed class WriteCache
    {
        // Simle struct to keep stream + the last time it was used
        
        
        static readonly WriteCache instance = new WriteCache();
        static int MAXCACHE = 100; // MB
        static int STREAMTIMEOUT = 20; // seconds

        private ulong cacheSize; // Current cache size
        private List<Segment> segments; // segment cache
        private List<Segment> newSegments; // segments that needs to be processed

        private List<WriteStream> streams; // stream cache
        private Thread thread;
        private bool keepRunning = true;

        struct WriteStream
        {
           public FileStream stream;
           public FileJob filejob;
           public int lastuse;
        }

        WriteCache() {
            // init collections
            segments = new List<Segment>();
            streams = new List<WriteStream>();
            newSegments = new List<Segment>();

            // Start update-thread
            thread = new Thread(new ThreadStart(Update));
            thread.Start();
        }

        //private AsyncCallback SegCompleteCallback(Segment seg)
        //{
        //    //seg.data = null;
        //    AsyncCallback call = new AsyncCallback(seg.CleanSegment);
        //    return call;
        //}

        // Writes everything out and return
        public void Shutdown()
        {
            keepRunning = false;
            lock (segments)
            {
                foreach (Segment seg in segments)
                {
                    if (!seg.tempsaved)
                    {
                        saveSegment(seg);
                    }
                }
            }
            lock (streams)
            {
                foreach (WriteStream ws in streams)
                {
                    ws.stream.Close();
                }
            }
        }

        

        private void saveSegment(Segment seg)
        {
            bool saved = false;
            // Check if stream is in cache
            List<WriteStream> toremove = new List<WriteStream>();
            List<WriteStream>.Enumerator enumer = streams.GetEnumerator();
            if(enumer.MoveNext()) {
                do
                {
                    WriteStream ws = (WriteStream)enumer.Current;
                    if (ws.filejob.Equals(seg.parent))
                    {
                        if (seg.tempsaved)
                        {
                            FileStream stream = new FileStream(Properties.Settings.Default.tempFolder + "\\" + seg.tempname, FileMode.Open, FileAccess.Read);

                            byte[] buffer = new byte[seg.bytes];
                            stream.Read(buffer, 0, seg.bytes); // May end up blocking if seg.bytes is larger than actual file
                            ws.stream.Write(buffer, 0, buffer.Length);
                            stream.Close();
                            seg.parent.saveprogress = seg.id;
                        }

                        else
                        {
                            ws.stream.Write(seg.data, 0, seg.bytes);
                            seg.data = null;
                            //ws.stream.BeginWrite(seg.data, 0, seg.bytes, SegCompleteCallback(seg), null);
                            seg.parent.saveprogress = seg.id;
                        }
                        saved = true;
                        ws.lastuse = DateTime.Now.Millisecond;

                        // Is filejob complete?
                        if (seg.id == seg.parent.yparts)
                        {
                            // Close stream
                            ws.stream.Flush();
                            ws.stream.Close();
                            // Remove from collection
                            toremove.Add(ws);
                            seg.parent.complete = true;
                            cacheSize -= (ulong)seg.bytes;
                        }
                        // May conflict with async write
                       // seg.data = null;
                        break;
                    }
                }
                while (enumer.MoveNext());
            }
            foreach (WriteStream ws in toremove)
            {
                streams.Remove(ws);
            }

            // Else open a new stream
            if (!saved)
            {
                // Open new stream..
                FileStream stream = new FileStream(seg.parent.outputfilename, FileMode.Append, FileAccess.Write);
                stream.Write(seg.data, 0, seg.data.Length);
                seg.data = null;
                // Was this the last part?
                if (seg.id == seg.parent.yparts || seg.parent.yparts == 0)
                {
                    // Close stream..
                    stream.Flush();
                    stream.Close();
                }
                else
                {
                    // Cache stream - add new WriteStream
                    seg.parent.saveprogress = seg.id;
                    WriteStream ws = new WriteStream();
                    ws.lastuse = DateTime.Now.Millisecond;
                    ws.stream = stream;
                    ws.filejob = seg.parent;

                    streams.Add(ws);
                }
                // may conflict
                //seg.data = null;
            }

        }


        // Updates cache
        private void Update()
        {
            while (keepRunning)
            {
                List<Segment> toremove = new List<Segment>();
                lock (newSegments)
                {
                    foreach (Segment seg in newSegments)
                    {
                        processSegment(seg);
                        toremove.Add(seg);
                    }
                    foreach (Segment seg in toremove)
                    {
                        newSegments.Remove(seg);
                    }
                }
                
                // Check if anything in cache can be written
                toremove.Clear();
                foreach (Segment seg in segments)
                {
                    if (seg.id - 1 == seg.parent.saveprogress)
                    {
                        saveSegment(seg);
                        seg.complete = true;
                        seg.parent.saveprogress++;
                        toremove.Add(seg);
                    }
                }
                foreach (Segment seg in toremove)
                {
                    segments.Remove(seg);
                }
                

                // Empty out streams thats passed the timeout limit..
                foreach(WriteStream ws in streams) {
                    if (ws.lastuse + STREAMTIMEOUT * 1000 < DateTime.Now.Millisecond)
                    {
                        ws.stream.Close();
                    }
                }
                Thread.Sleep(1000); // Sleep 1sec before updating again
            }
        }


        

        public void addSegment(ref Segment segment)
        {
            lock (newSegments)
            {
                newSegments.Add(segment);
            }
        }

        // Processes new segment
        private void processSegment(Segment segment)
        {
            // is the cache waiting for this part?
            
            if (segment.id == 1)
            {
                Logging.Log("(Cache) Recieved first segment");
                // First part in a filejob..
                segment.parent.outputfilename = getFilename(ref segment);
                saveSegment(segment);

            }
            else if (segment.id == segment.parent.saveprogress+1)
            {
                // Append to file
                Logging.Log("(Cache) Recieved awaited segment");
                saveSegment(segment);
            }
            else if (cacheSize + (ulong)segment.bytes > (ulong)MAXCACHE * 1024 * 1024)
            {
                Logging.Log("Cache too big, saving to disk");
                segment.tempname = "temp"+new Random().Next(9999999);

                FileStream stream = new FileStream(Properties.Settings.Default.tempFolder + "\\" + segment.tempname, FileMode.Create, FileAccess.Write);
                stream.Write(segment.data, 0, segment.bytes);
                segment.data = null;
                stream.Flush();
                stream.Close();
                segments.Add(segment);
                segment.tempsaved = true;
            }
            else
            {
                // memorycache
                Logging.Log("(Cache) Saving segment to cache");
                segments.Add(segment);
                cacheSize += (ulong)segment.bytes;
            }
        }

        private string getFilename(ref Segment segment)
        {
            // Create output dictionary if necessary
            Directory.CreateDirectory(Properties.Settings.Default.outputPath + "\\" + segment.parent.parent.name + "\\");

            if (segment.yname != null && segment.yname != "")
            {
                return Properties.Settings.Default.outputPath + "\\" + segment.parent.parent.name + "\\" + segment.yname;
            }
            else
            {
                return Properties.Settings.Default.outputPath + "\\" + segment.parent.parent.name + "\\" + segment.parent.filename;
            }
        }

        // Singleton implementation
        public static WriteCache Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
