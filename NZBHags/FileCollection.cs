using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;
using System.IO;

namespace NZBHags
{
    public enum CollectionStatus
    {
        PAUSE,
        DOWNLOADING,
        QUEUED
    }

    public class FileCollection
    {
        public string name { get; set; }
        public ArrayList files { get; set; }
        public ulong size; // kb
        public ulong progress; // kb
        public int id { get; set; }
        public Queue queue { get; set; }
        public CollectionStatus status { get; set; }

        public void Remove()
        {
            queue = null;
            foreach (FileJob file in files)
            {
                if (file.complete)
                {
                    if (File.Exists(file.outputfilename))
                        File.Delete(file.outputfilename);
                }
                else
                {
                    foreach (Segment seg in file.segments)
                    {
                        while (seg.status == Segment.Status.YDECODING || seg.status == Segment.Status.WRITECACHE)
                        {
                            Thread.Sleep(10);
                        }
                        if (seg.status == Segment.Status.TEMPCACHED)
                        {
                            // remove temp file
                            File.Delete(Properties.Settings.Default.tempFolder + "\\" + seg.tempname);
                        }
                        else if (seg.status == Segment.Status.COMPLETE)
                        {
                            // remove complete file
                            if (File.Exists(seg.parent.outputfilename))
                                File.Delete(seg.parent.outputfilename);
                        }
                    }
                }
                
            }
            files.Clear();
            QueueHandler.Instance.removeCollection(this);
        }

        public FileCollection(string name, ArrayList files)
        {
            this.name = name;
            this.files = files;
        }

        public FileCollection(string name)
        {
            this.name = name;
            this.files = files;
        }
    }
}
