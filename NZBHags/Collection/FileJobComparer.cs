using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NZBHags
{
    class FileJobComparer : IComparer<FileJob>
    {
        int IComparer<FileJob>.Compare(FileJob obj1, FileJob obj2)
        {
            FileJob seg1 = (FileJob)obj1;
            FileJob seg2 = (FileJob)obj2;

            if (seg1 == null && seg2 == null)
            {
                return 0;
            }
            else if (seg1 == null && seg2 != null)
            {
                return 1;
            }
            else if (seg1 != null && seg2 == null)
            {
                return -1;
            }
            else
            {
                return seg1.status.CompareTo(seg2.status);
            }
        }
    }
}
