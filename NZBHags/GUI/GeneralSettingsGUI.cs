using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NZBHags.GUI
{
    public partial class GeneralSettingsGUI : UserControl
    {
        public GeneralSettingsGUI()
        {
            InitializeComponent();
        }

        public bool ValidateUI()
        {
            uint maxcache;
            if (uint.TryParse(textBoxCacheSize.Text, out maxcache))
            {
                return true;
            }
            return false;
        }

        public void Save()
        {
            if (ValidateUI())
            {
                Properties.Settings.Default.cachesize = uint.Parse(textBoxCacheSize.Text);
                Properties.Settings.Default.par2enabled = checkBoxPar2Enabled.Checked;
                Properties.Settings.Default.par2cleanup = checkBoxPostCleanup.Checked;
                Properties.Settings.Default.par2multicore = checkBoxMultiCore.Checked;
                Properties.Settings.Default.rarenabled = checkBoxRarEnabled.Checked;
            }
            else
            {
                MessageBox.Show("Cachesize needs to be a number!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateUI(object sender, EventArgs e)
        {
            textBoxCacheSize.Text = ""+Properties.Settings.Default.cachesize;
            checkBoxPar2Enabled.Checked = Properties.Settings.Default.par2enabled;
            checkBoxPostCleanup.Checked = Properties.Settings.Default.par2cleanup;
            checkBoxMultiCore.Checked = Properties.Settings.Default.par2multicore;
            checkBoxRarEnabled.Checked = Properties.Settings.Default.rarenabled;
        }
    }
}
