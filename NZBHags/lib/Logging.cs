using System;
using System.Collections;
using System.Linq;
using System.Text;


namespace NZBHags
{
    sealed class Logging
    {
        private static Logging _Instance = new Logging();
        static public ArrayList logList = new ArrayList();
        public MainGUI maingui;

        Logging() { }


        public void Log(string str)
        {
            lock (typeof(Logging))
            {
                //logList.Add(str + '\n');
                System.Console.WriteLine(str);
                if (maingui != null)
                {
                    try
                    {
                        maingui.logTextBox.Invoke(maingui.logHandler, str);
                    } catch(Exception ex) {
                        // Might throw somethign when exiting
                    }
                   // maingui.logHandler.Invoke(str);
                    // Fire delegate
                }
            }
        }

        public void Log(string str, params object[] more)
        {
            Log(string.Format(str, more));
        }

        public static Logging Instance
        {
            get { return _Instance; }
        }
    }
}