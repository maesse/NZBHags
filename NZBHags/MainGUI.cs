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

        public MainGUI()
        {
            InitializeComponent();
            handler = QueueHandler.Instance;
            
            //server = NewsServer.Load("server.ini");
            server = new NewsServer();
            if (server == null)
            {
                // Create new..
                server = new NewsServer();
            }
            
        }

        private void LoadNZB(string filename)
        {
            FileCollection nzb = NZBFileHandler.loadFile(filename);
            nzb.queue = NZBFileHandler.genQueue(nzb.files);
            handler.collections.Add(nzb);

            // Add to UI
            TreeNode[] files = new TreeNode[nzb.files.Count];
            int i = 0;
            foreach (FileJob file in nzb.files)
            {
                TreeNode[] parts = new TreeNode[file.segments.Count];
                int j = 0;

                foreach (Segment part in file.segments)
                {
                    parts[j] = new TreeNode(string.Format("part({0}), bytes={1}, addr={2}", part.id, part.bytes, part.addr));
                    j++;
                }
                files[i] = new TreeNode("Filename: "+file.subject+" size=" + ((file.size/1024)/1024)+"MB", parts);
                i++;
            }
            treeView1.Nodes.Add(new TreeNode(filename, files));

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

        // Timer updates
        private void UpdateUI(object sender, EventArgs e)
        {
            // Refresh connection view
            dataGridView1.Refresh();

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
            server.Disconnect();
            YDecoder.Instance.Shutdown();
            WriteCache.Instance.Shutdown();
        }
    }
}
