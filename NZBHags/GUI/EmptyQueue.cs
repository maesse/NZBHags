using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NZBHags.lib;

namespace NZBHags
{
    public partial class EmptyQueue : UserControl, IUpdatingControl
    {
        private MainGUI main;
        public EmptyQueue(MainGUI main)
        {
            this.main = main;
            InitializeComponent();
        }

        public void ResizeInLayoutEvent()
        {
            this.Size = new Size(Parent.Size.Width - 6, 63);
        }

        public void UpdateUI()
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // open file
            DialogResult result = main.openFileDialog1.ShowDialog(this);
            if (result.Equals(DialogResult.OK))
            {
                main.LoadNZB(main.openFileDialog1.FileName);
            }
        }
    }
}
