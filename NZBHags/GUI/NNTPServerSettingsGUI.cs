 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NZBHags
{
    public partial class NNTPServerSettingsGUI : UserControl
    {

        private NewsServer server;
        public NNTPServerSettingsGUI(ref NewsServer server)
        {
            this.server = server;
            InitializeComponent();
        }

        public bool ValidateUI()
        {
            int port, connections, timeout;
            if (int.TryParse(textBox5.Text, out port) && int.TryParse(textBox6.Text, out connections) && int.TryParse(textBox7.Text, out timeout))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Save()
        {
            int port, connections, timeout;
            if (int.TryParse(textBox5.Text, out port) && int.TryParse(textBox6.Text, out connections) && int.TryParse(textBox7.Text, out timeout))
            {
                // Save settings...
                server.name = textBox1.Text;
                server.addr = textBox2.Text;
                server.username = textBox3.Text;
                server.password = textBox4.Text;
                server.port = port;
                server.connections = connections;
                server.timeout = timeout;
                server.Save();
            }
            else
            {
                MessageBox.Show("Check your Server values", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateUI(object sender, EventArgs e)
        {
            textBox1.Text = server.name;
            textBox2.Text = server.addr;
            textBox3.Text = server.username;
            textBox4.Text = server.password;
            textBox5.Text = "" + server.port;
            textBox6.Text = "" + server.connections;
            textBox7.Text = "" + server.timeout;
            //if (server.isConnected == true)
            //{
            //    button1.Enabled = false;
            //}
            //else
            //{
            //    button1.Enabled = true;
            //}
        }

        private void UpdateUI()
        {

        }
    }
}
