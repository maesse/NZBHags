using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace NZBHags
{
    class SegmentComparer : IComparer<Segment>
    {
        int IComparer<Segment>.Compare(Segment obj1, Segment obj2)
        {
            Segment seg1 = (Segment)obj1;
            Segment seg2 = (Segment)obj2;

            if (seg1 == null && seg2 == null)
            {
                return 0;
            }

            else  if (seg1 == null && seg2 != null)
            {
                return 1;
            }
            else if (seg1 != null && seg2 == null)
            {
                return -1;
            }

            else
            {
                return seg1.id.CompareTo(seg2.id);
            }
        }
    }
}
