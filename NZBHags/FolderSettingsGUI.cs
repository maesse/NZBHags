using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace NZBHags
{
    public partial class FolderSettingsGUI : UserControl, ISettings
    {
        public FolderSettingsGUI()
        {
            InitializeComponent();
            UpdateUI();
        }

        public void UpdateUI()
        {
            textBox1.Text = Properties.Settings.Default.nzbFolder;
            textBox2.Text = Properties.Settings.Default.tempFolder;
            textBox3.Text = Properties.Settings.Default.outputPath;
        }

        public void Save()
        {

            if (ValidateUI())
            {
                if (Directory.Exists(textBox1.Text))
                {
                    // save nzbfolder
                    Properties.Settings.Default.nzbFolder = textBox1.Text;
                }
                Properties.Settings.Default.tempFolder = textBox2.Text;
                Properties.Settings.Default.outputPath = textBox3.Text;
            }
        }

        public bool ValidateUI()
        {
            if(Directory.Exists(textBox2.Text) && Directory.Exists(textBox3.Text)) {
                return true;
            }
            return false;
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            Shell32.ShellClass shl = new Shell32.ShellClass();
            Shell32.Folder2 fld = (Shell32.Folder2)shl.BrowseForFolder(0,
            "NZB Folder (eg. your download folder)", 0, System.Reflection.Missing.Value);
            if (fld != null)
            {
                textBox1.Text = fld.Self.Path;
            }
            //fld.Self.Path
        }

        // temp folder
        private void button2_Click(object sender, EventArgs e)
        {
            Shell32.ShellClass shl = new Shell32.ShellClass();
            Shell32.Folder2 fld = (Shell32.Folder2)shl.BrowseForFolder(0,
            "Folder for temp files", 0, System.Reflection.Missing.Value);
            if (fld != null)
            {
                textBox2.Text = fld.Self.Path;
            }
            //fld.Self.Path
        }


        private void button3_Click(object sender, EventArgs e)
        {
            Shell32.ShellClass shl = new Shell32.ShellClass();
            Shell32.Folder2 fld = (Shell32.Folder2)shl.BrowseForFolder(0,
            "Output folder for extracted files", 0, System.Reflection.Missing.Value);
            if (fld != null)
            {
                textBox3.Text = fld.Self.Path;
            }
            //fld.Self.Path
        }
    }
}
