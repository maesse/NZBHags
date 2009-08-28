using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace NZBHags
{
    public class FileJob
    {
        public FileCollection parent { get; set; }

        // Meta data
        public string poster { get; set; }
        public ArrayList groups { get; set; }
        public string subject { get; set; }
        public int date { get; set; }

        // segments
        public List<Segment> segments { get; set; }
        private int _SegmentProgress = 0;
        public int SegmentProgress { get { return _SegmentProgress; } set { _SegmentProgress = value; } } // 0 -> files.Count
        public int SegmentCount { get { return (segments != null) ? segments.Count : 0; } }
        //public Queue queue { get; set; }
        public int yparts { get; set; }

        // File info
        public string filename { get; set; }
        public ulong size { get; set; } // expected bytes in total
        public string outputfilename { get; set; }
        
        // Progress
        public int saveprogress { get; set; } // filepart(s) written
        public bool complete { get; set; } // completed downloading+extracting
        public long byteprogress { get; set; } // bytes written, not used ATM
        

        public FileJob(ref FileCollection parent)
        {
            saveprogress = 0;
            this.parent = parent;
            Random random = new Random();
            filename = ""+random.Next(99999);
            segments = new List<Segment>();
            groups = new ArrayList();
            complete = false;
        }
        public FileJob()
        {
            //queue = new Queue();
        }
    }
}
