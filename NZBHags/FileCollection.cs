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
        QUEUED,
        DOPAR2
    }

    public class FileCollection
    {
        public string name { get; set; }
        public List<FileJob> files { get; set; }
        public ulong TotalSize; // kb
        public uint speed; // bytes/s
        public ulong ProgressKB; // kb
        public int FileCount { get { return (files != null) ? files.Count : 0; } }
        private int _FileProgress = 0;
        public int FileProgress { get { return _FileProgress;  } set { _FileProgress = value; } } // 0 -> files.Count
        public int id { get; set; }
        //public Queue queue { get; set; }
        public CollectionStatus status { get; set; }

        public FileCollection(string name, List<FileJob> files)
        {
            this.name = name;
            this.files = files;
        }

        public FileCollection(string name)
        {
            this.name = name;
            this.files = new List<FileJob>();
        }

        

        public void GenerateQueue()
        {
            
        }

        public void CalculateTotalSize()
        {
            TotalSize = 0;
            foreach (FileJob job in files)
            {
                TotalSize += job.size;
            }
        }
    }
}
