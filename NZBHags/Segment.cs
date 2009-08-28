﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NZBHags
{
    

    public class Segment
    {
        public enum Status
        {
            QUEUED, // Not being handled
            DOWNLOADING, // Sitting in NNTPConnection
            YDECODING, // Sitting in YDecoder
            TEMPCACHED, // Temporarity written to disk by WriteCache
            WRITECACHE, // Sitting in WriteCache
            COMPLETE // Completed
        }

        public FileJob parent { get; set; } // parent filejob
        public int id { get; set; } // file id, sorted by nzb number

        public string addr { get; set; } // post address
        //public string subject { get; set; } // post subject (fallback filename?)

        //public bool complete { get; set; } // completed downloading
        public Status status { get; set; }
        public bool tempsaved { get; set; }
        public string tempname { get; set; }

        public byte[] data { get; set; } // data read (big)
        public int bytes { get; set; } // bytes read
        public int progress { get; set; } // progress in bytes updated from nntpconnection
        
        public int ypart { get; set; } // yenc part number
        public string yname { get; set; } // Filename as described by yenc

        public string crc { get; set; } // Expected crc
        public bool crcOk { get; set; } // CRC OK/Failed
        public bool isYenc { get; set; }

        public Segment()
        {
            status = Status.QUEUED;
        }

        public bool ValidateCRC()
        {
            string calculatedCRC = String.Format("{0:X8}", CRC32Hasher.Calculate(data)).ToLower();
            crc = crc.ToLower();
            if (crc.Length != 8 || calculatedCRC.Equals(crc))
            {
                // No CRC or CRC OK.. moving on
                //Logging.Log("(ValidateCRC) SUCESS - crc=" + calculatedCRC);
                crcOk = true;
            }
            else
            {
                // CRC failed
                
                Logging.Instance.Log("(ValidateCRC) (id=" + id + ") FAILED: Expected crc: " + crc + " - Actual crc: " + calculatedCRC);
                crcOk = false;
            }


            return crcOk;

            
        }

        public void setParent(ref FileJob parent)
        {
            this.parent = parent;
        }

        public void CleanSegment(IAsyncResult result)
        {
            if (result.IsCompleted)
            {
                data = null;
            }
        }


    }
}
