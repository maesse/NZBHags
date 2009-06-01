using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NZBHags
{
    public partial class MainGUI : Form
    {
        SettingsForm settingsForm;
        NewsServer server;
        QueueHandler handler;
        public uint dlspeed { get; set; }

        public MainGUI()
        {
            InitializeComponent();
            handler = QueueHandler.Instance;
            
            server = new NewsServer();
            
        }

        private void LoadNZB(string filename)
        {
            FileCollection nzb = NZBFileHandler.loadFile(filename);
            nzb.queue = NZBFileHandler.genQueue(nzb.files);
            QueueHandler.Instance.AddCollection(nzb);
            flowLayoutPanel1.Controls.Add(new QueueControl(nzb, this));
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

        // Connect/Disconnect
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            // Connect to server...
            if (server.isConnected)
            {
                for (int i = 0; i < server.connections; i++)
                {

                    nNTPConnectionBindingSource1.Remove(server.nntpConnections[i]);
                }
                server.Disconnect();
                toolStripButton1.Image = NZBHags.Properties.Resources.connect_established;
            }
            else
            {
                server.Connect();
                toolStripButton1.Image = NZBHags.Properties.Resources.connect_no;
                for (int i = 0; i < server.connections; i++ )
                {
                    
                    nNTPConnectionBindingSource1.Add(server.nntpConnections[i]);
                }
            }
        }

        public void remQueue(QueueControl control)
        {
            QueueHandler.Instance.removeCollection(control.collection);
            flowLayoutPanel1.Controls.Remove(control);
        }

        public void incrQueue(QueueControl control)
        {
            
            //if (control.collection.id + 1 < flowLayoutPanel1.Controls.Count)
            //{
                QueueHandler.Instance.setCollectionID(control.collection, control.collection.id++);
                int index = flowLayoutPanel1.Controls.GetChildIndex(control);
                flowLayoutPanel1.Controls.SetChildIndex(flowLayoutPanel1.Controls[index++], index);
                flowLayoutPanel1.Controls.SetChildIndex(control, index++);
                
            //}
        }

        public void decrQueue(QueueControl control)
        {
            QueueHandler.Instance.setCollectionID(control.collection, control.collection.id--);
            int index = flowLayoutPanel1.Controls.GetChildIndex(control);
            flowLayoutPanel1.Controls.SetChildIndex(flowLayoutPanel1.Controls[index--], index);
            flowLayoutPanel1.Controls.SetChildIndex(control, index--);
        }

        // Timer updates
        private void UpdateUI(object sender, EventArgs e)
        {
            // Refresh connection view
            dataGridView1.Refresh();
            if (QueueHandler.Instance.changed)
            {
                //flowLayoutPanel1.Controls.Clear();
                //FileCollection[] coll = QueueHandler.Instance.getCollections();
                //for (int i = 0; i < coll.Length; i++)
                //{
                //    flowLayoutPanel1.Controls.Add(new QueueControl(coll[i]));
                //}

                QueueHandler.Instance.changed = false;
            }
            //if (server.nntpConnections != null)
            //{
            //    uint tempspeed = 0;
            //    foreach (NNTPConnection con in server.nntpConnections)
            //    {
            //        tempspeed += con.speed;
            //    }
            //    tempspeed /= (uint)server.nntpConnections.Length;
            //    dlspeed = tempspeed;
            //}
            //if (dlspeed > 1024)
            //{
            //    dlspeed /= 1024;
            //    labelSpeed.Text = string.Format("{0:n} KB/s", dlspeed);
            //}
            //else
            //{
            //    if (dlspeed != 0)
            //    {
            //        labelSpeed.Text = string.Format("{0:n} B/s", dlspeed);
            //    }
            //    else
            //    {
            //        labelSpeed.Text = "";
            //    }
            //}
            foreach (QueueControl control in flowLayoutPanel1.Controls)
            {
                control.UpdateUI();
            }
            // Append logs
            lock (typeof(Logging))
            {
                foreach (string str in Logging.logList)
                {
                    logTextBox.AppendText(str);
                }
                Logging.logList.Clear();
            }
        }

        // Open nzb
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            // open file
            DialogResult result = openFileDialog1.ShowDialog(this);
            if (result.Equals(DialogResult.OK))
            {
                LoadNZB(openFileDialog1.FileName);
            }
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
            if(server != null)
                server.Disconnect();
            YDecoder.Instance.Shutdown();
            WriteCache.Instance.Shutdown();
        }

        // Queue flowpanel layout
        private void Layout(object sender, LayoutEventArgs e)
        {
            foreach (Control ctrl in ((Control)sender).Controls)
            {
                ((QueueControl)ctrl).ResizeInLayoutEvent();
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

    }
}
