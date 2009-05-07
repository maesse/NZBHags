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
        public ArrayList segments { get; set; }
        public Queue queue { get; set; }
        public int yparts { get; set; }

        // File info
        public string filename { get; set; }
        public ulong size { get; set; } // expected bytes in total
        public string outputfilename { get; set; }
        
        // Progress
        public int saveprogress { get; set; } // filepart(s) written
        public bool complete { get; set; } // completed downloading+extracting
        

        public FileJob(ref FileCollection parent)
        {
            saveprogress = 0;
            this.parent = parent;
            Random random = new Random();
            filename = ""+random.Next(99999);
            segments = new ArrayList();
            groups = new ArrayList();
            complete = false;
        }

        //public void Complete()
        //{
        //    bool missingParts = false;
        //    if (!complete)
        //    {
        //        FileStream output;
        //        if (segments.Count > 0)
        //        {   
        //            // Determine filename
        //            Segment seg = (Segment)segments[0];
                    
        //            Directory.CreateDirectory(Properties.Settings.Default.outputPath + "\\" + seg.parent.parent.name + "\\");
        //            if (seg.yname != null && seg.yname != "")
        //            {
        //                output = new FileStream(Properties.Settings.Default.outputPath + "\\" + seg.parent.parent.name + "\\" + seg.yname, FileMode.Create);
        //            }
        //            else
        //            {
        //                output = new FileStream(Properties.Settings.Default.outputPath + "\\" + seg.parent.parent.name + "\\" + filename, FileMode.Create);
        //            }
        //        }
        //        else
        //            return;
                
        //        //StreamWriter writer = new StreamWriter(output);
        //        bool found = false;
        //        if (yparts > 0)
        //        {
        //            for (int i = 1; i < segments.Count + 1; i++)
        //            {
        //                foreach (Segment seg in segments)
        //                {
        //                    if (seg.ypart == i)
        //                    {
        //                        Logging.Log("Writing part: " + i);
        //                        output.Write(seg.data, 0, seg.data.Length);
        //                        //writer.Write();
        //                        found = true;
        //                    }
        //                }
        //                if (found == false)
        //                {
        //                    // missing part fallback
        //                    missingParts = true;
        //                }
        //            }
        //        }
        //        else // No parts.. just write the damn thing
        //        {
        //            foreach (Segment seg in segments)
        //            {
        //                output.Write(seg.data, 0, seg.data.Length);
        //            }
        //        }
        //        output.Flush();
        //        output.Close();

        //        if (missingParts)
        //        {
        //            // Move file...
        //        }
        //        Logging.Log("Completed extracting file..");
        //        complete = true;
        //    } 
        //}
    }
}
