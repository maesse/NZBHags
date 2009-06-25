using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace NZBHags.GUI
{
    public partial class UIShutdown : Form
    {
        private MainGUI main;

        // Constructor
        public UIShutdown(MainGUI main)
        {
            this.main = main;
            InitializeComponent();
        }


        // allow manual override, should the async shutdown fail
        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        // When Form is shown, async shutdown is started
        private void StartShutdown(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        // Shuts down the application async
        private void AsyncShutdown(object sender, DoWorkEventArgs e)
        {
            // Disconnect from server
            if (main.server != null && main.server.nntpConnections != null)
            {
                foreach (NNTPConnection conn in main.server.nntpConnections)
                {
                    conn.Disconnect();
                }
                main.server.isConnected = false;
            }
            // Shutdown YDecoder
            YDecoder.Instance.Shutdown();

            // Shutdown WriteCache
            WriteCache.Instance.Shutdown();
        }

        // Dispose form when async shutdown completes
        private void AsyncComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }
    }
}
