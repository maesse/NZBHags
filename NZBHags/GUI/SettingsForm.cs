using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NZBHags.GUI;

namespace NZBHags
{
    public partial class SettingsForm : Form
    {
        private NewsServer server;
        NNTPServerSettingsGUI settings;
        FolderSettingsGUI folder;
        GeneralSettingsGUI general;

        public SettingsForm(ref NewsServer server)
        {
            settings = new NNTPServerSettingsGUI(ref server);
            folder = new FolderSettingsGUI();
            general = new GeneralSettingsGUI();
            this.server = server;
            InitializeComponent();

            setMain(general);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void setMain(Control control)
        {
            
            // todo: validate previous window before continuing
            if (mainPanel.Controls.Count == 1)
            {
                if (mainPanel.Controls[0] is ISettings)
                {
                    ISettings controlInterface = (ISettings)mainPanel.Controls[0];
                    if (!controlInterface.ValidateUI())
                    {
                        MessageBox.Show("Error in some of the values");
                        return;
                    }
                }
            }


            mainPanel.Controls.Clear();
            mainPanel.Controls.Add(control);
            control.Size = mainPanel.Size;
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            // Save all elements
            if (settings.ValidateUI())
                settings.Save();
            if (folder.ValidateUI())
                folder.Save();
            if (general.ValidateUI())
                general.Save();

            Properties.Settings.Default.Save();
            this.Close();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            mainPanel.Controls.Add(new NNTPServerSettingsGUI(ref server));
        }


        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            setMain(folder);
            toolStripButton1.Checked = false;
            toolStripButton2.Checked = true;
            toolStripButton3.Checked = false;
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            setMain(settings);
            toolStripButton1.Checked = true;
            toolStripButton2.Checked = false;
            toolStripButton3.Checked = false;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            setMain(general);
            toolStripButton1.Checked = false;
            toolStripButton2.Checked = false;
            toolStripButton3.Checked = true;
        }
    }
}
