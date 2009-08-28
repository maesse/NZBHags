using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NZBHags.lib;
using NZBHags.GUI;
namespace NZBHags
{
    public partial class MainGUI : Form
    {
        public delegate void LogHandler(string log);
        SettingsForm settingsForm;
        public NewsServer server;
        QueueHandler handler;
        public int currentspeed { get; set; }
        public uint dlspeed { get; set; }
        public LogHandler logHandler;
        private GraphGUI graphGUI;
        private DLThreadGUI[] dlthreads;
        public MainGUI()
        {
            InitializeComponent();
            logHandler = new LogHandler(LogString);
            handler = QueueHandler.Instance;
            flowLayoutPanel1.Controls.Add(new EmptyQueue(this));
            server = new NewsServer();
            graphGUI = new GraphGUI();
            panelGraph.Controls.Add(graphGUI);
            Logging.Instance.maingui = this;
        }

        public void LoadNZB(string filename)
        {
            FileCollection nzb = NZBFileHandler.loadFile(filename);
            QueueHandler.Instance.AddCollection(nzb);
            flowLayoutPanel1.Controls.Add(new QueueControl(nzb, this));
            UpdateUI(this, null);
        }

        private void OpenNzbDialog(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog(this);
            if (result.Equals(DialogResult.OK))
            {
                LoadNZB(openFileDialog1.FileName);
            }
        }

        private void Exit(object sender, EventArgs e)
        {
            this.Close();
        }

        public void appendLog(string str)
        {
            logTextBox.Text += str;
        }

        // Open settings
        private void OpenSettingsForm(object sender, EventArgs e)
        {
            if (settingsForm == null || !settingsForm.Visible)
            {
                settingsForm = new SettingsForm(ref server);
                settingsForm.Show(this);

            }

        }

        public void UpdateSingleThreadInfo(NNTPConnection conn)
        {
            if (dlthreads != null)
            {
                dlthreads[conn.id].Invoke(dlthreads[conn.id].updateDelegate);
            }
        }

        // Connect/Disconnect
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            // Connect to server...
            if (server.isConnected)
            {
                for (int i = 0; i < server.connections; i++)
                {
                    panel1.Controls.Clear();
                    
                }
                dlthreads = null;
                server.Disconnect();
                toolStripButton1.Image = NZBHags.Properties.Resources.connect_established;
            }
            else
            {
                server.Connect(this);
                toolStripButton1.Image = NZBHags.Properties.Resources.connect_no;
                dlthreads = new DLThreadGUI[server.connections];
                for (int i = 0; i < server.connections; i++ )
                {
                    
                    //nNTPConnectionBindingSource1.Add(server.nntpConnections[i]);
                    //flowLayoutPanelThreads.Controls.Add(new DLThreadGUI(server.nntpConnections[i]));
                    //listBox1.Controls.Add(new DLThreadGUI(server.nntpConnections[i]));
                    DLThreadGUI ctrl = new DLThreadGUI(server.nntpConnections[i]);
                    ctrl.Location = new System.Drawing.Point(0, panel1.Controls.Count * 23);
                    dlthreads[i] = ctrl;
                    panel1.Controls.Add(ctrl);
                    
                }
            }
        }

        public void remQueue(QueueControl control)
        {
            QueueHandler.Instance.removeCollection(control.collection);
            flowLayoutPanel1.Controls.Remove(control);
            UpdateUI(this, null);
        }

        public void incrQueue(QueueControl control)
        {
            
            //if (control.collection.id + 1 < flowLayoutPanel1.Controls.Count)
            //{
                QueueHandler.Instance.setCollectionID(control.collection, control.collection.id++);
                int index = flowLayoutPanel1.Controls.GetChildIndex(control);
                flowLayoutPanel1.Controls.SetChildIndex(flowLayoutPanel1.Controls[index++], index);
                flowLayoutPanel1.Controls.SetChildIndex(control, index++);
                UpdateUI(this, null);
            //}
        }

        public void LogString(string log)
        {
            log = "[" + DateTime.Now.ToString("HH:mm:ss") + "] " + log + '\n';
            logTextBox.AppendText(log);
        }

        public void decrQueue(QueueControl control)
        {
            QueueHandler.Instance.setCollectionID(control.collection, control.collection.id--);
            int index = flowLayoutPanel1.Controls.GetChildIndex(control);
            flowLayoutPanel1.Controls.SetChildIndex(flowLayoutPanel1.Controls[index--], index);
            flowLayoutPanel1.Controls.SetChildIndex(control, index--);
            UpdateUI(this, null);
        }

        // Timer updates
        private void UpdateUI(object sender, EventArgs e)
        {
            SpeedMonitor.Instance.tick();

            // If queue has more than 1 item, check for EmptyQueue control and remove it
            if (flowLayoutPanel1.Controls.Count > 1)
            {
                //foreach (Control control in flowLayoutPanel1.Controls)
                //{
                    if (flowLayoutPanel1.Controls[0] is EmptyQueue)
                    {
                        flowLayoutPanel1.Controls.Remove(flowLayoutPanel1.Controls[0]);
                        //break;
                    }
                //}
            }
            else if (flowLayoutPanel1.Controls.Count == 0)
            {
                // Insert EmptyQueue control if queue is empty
                flowLayoutPanel1.Controls.Add(new EmptyQueue(this));
            }

            // Updates Queue and Thread view
            foreach (IUpdatingControl control in flowLayoutPanel1.Controls)
            {
                control.UpdateUI();
            }
            
            

            // Updates speed graph
            graphGUI.UpdateUI(SpeedMonitor.Instance.Speed);

            //// Append logs
            //lock (typeof(Logging))
            //{
            //    foreach (string str in Logging.logList)
            //    {
            //        logTextBox.AppendText(str);
            //    }
            //    Logging.logList.Clear();
            //}
        }

        public void OpenNzbFileDialog()
        {
            // open file
            DialogResult result = openFileDialog1.ShowDialog(this);
            if (result.Equals(DialogResult.OK))
            {
                LoadNZB(openFileDialog1.FileName);
            }
        }

        // Open nzb
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            OpenNzbFileDialog();
        }

        // open about dialog
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 about = new AboutBox1();
            about.Show();
        }

        // Called on formClosing, shuts down gracefully
        private void Shutdown(object sender, FormClosingEventArgs e)
        {
            UIShutdown shutdown = new UIShutdown(this);
            toolStripStatusLabel1.Text = "Shutting down...";
                shutdown.ShowDialog(this);
        }

        // Queue flowpanel layout
        private new void Layout(object sender, LayoutEventArgs e)
        {
            foreach (Control ctrl in ((Control)sender).Controls)
            {
                if(ctrl is IUpdatingControl)
                    ((IUpdatingControl)ctrl).ResizeInLayoutEvent();
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            string windir = Environment.GetEnvironmentVariable("WINDIR");
            System.Diagnostics.Process prc = new System.Diagnostics.Process();
            prc.StartInfo.FileName = windir + @"\explorer.exe";
            prc.StartInfo.Arguments = Properties.Settings.Default.outputPath;
            prc.Start();
        }

        private void UpdateConnUI(object sender, EventArgs e)
        {
            foreach (IUpdatingControl control in panel1.Controls)
            {
                control.UpdateUI();
            }
        }

        private void ResetStatus(object sender, EventArgs e)
        {

            toolStripStatusLabel1.Text = "";
        }

        private void StatusConnect(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Connect/Disconnect";
        }

        private void StatusOpenNZB(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Open .NZB...";
        }

        private void StatusOpenFolder(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Open Output folder...";
        }

    }
}
