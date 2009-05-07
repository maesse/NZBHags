using System;
using System.Collections;
using System.Linq;
using System.Text;


namespace NZBHags
{
    public class Logging
    {
        static public ArrayList logList = new ArrayList();


        public static void Log(string str) 
        {
            lock (typeof(Logging))
            {
                logList.Add(str + '\n');
                System.Console.WriteLine(str);
            }
        }

        public static void Log(string str, params object[] more)
        {
            Log(string.Format(str, more));
        }
    }
}
