using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NZBHags
{
    public sealed class SpeedMonitor
    {
        static readonly SpeedMonitor instance = new SpeedMonitor();
        private List<NNTPConnection> Connections;
        public uint Speed { get; set; } // To be precise: bytes since last tick
    
        // Constructor
        SpeedMonitor()
        {
            Connections = new List<NNTPConnection>();
        }

        // Pumps the SpeedMonitor, so it updates pulls values from connections and calculates the speed
        public void tick()
        {
            uint accumulatedValue = 0;

            lock (Connections)
            {
                foreach (NNTPConnection conn in Connections)
                {
                    accumulatedValue += conn.speed;
                    conn.speed = 0;
                }
            }

            Speed = accumulatedValue;
        }


        // Adds a conntions to the connection list, so it can be polled.
        public void RegisterConnection(NNTPConnection conn) {
            lock (Connections)
            {
                Connections.Add(conn);
            }
        }

        public void DeRegisterConnection(NNTPConnection conn)
        {
            lock (Connections)
            {
                Connections.Remove(conn);
            }
        }

        // Singleton implementation
        public static SpeedMonitor Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
