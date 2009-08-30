using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NZBHags
{
    class Misc
    {
        public static string GetPrettyFileSize(ulong nBytes) 
        {
            int sizei = 0;
            while (nBytes > 1024)
            {
                nBytes = nBytes / 1024;
                sizei++;
            }
            string outsize = null;
            switch (sizei)
            {
                case 0:
                    outsize = string.Format("{0:n} B", nBytes);
                    break;
                case 1:
                    outsize = string.Format("{0:n} KB", nBytes);
                    break;
                case 2:
                    outsize = string.Format("{0:n} MB", nBytes);
                    break;
                case 3:
                    outsize = string.Format("{0:n} GB", nBytes);
                    break;
            }
            return outsize;
        }
    }
}
