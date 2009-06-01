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
        public uint speed; // bytes/s
        public ulong progress; // kb
        public int id { get; set; }
        public Queue queue { get; set; }
        public CollectionStatus status { get; set; }

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
