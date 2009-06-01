using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NZBHags.lib;

namespace NZBHags.GUI
{
    public partial class DLThreadGUI : UserControl, IUpdatingControl
    {
        public NNTPConnection conn;
        public DLThreadGUI(NNTPConnection connection)
        {
            conn = connection;
            InitializeComponent();
        }

        public void ResizeInLayoutEvent()
        {
            this.Size = new Size(Parent.Size.Width - 6, 43);
        }

        public void UpdateUI()
        {
            if (conn.currentSegment == null)
            {
                labelFileJob.Text = "Idle";
                progressBar1.Value = 0;
            }
            else
            {
                if (conn.currentSegment.bytes >= conn.currentSegment.progress)
                    progressBar1.Maximum = conn.currentSegment.bytes;
                else
                    progressBar1.Maximum = conn.currentSegment.progress;

                progressBar1.Value = conn.currentSegment.progress;
                labelFileJob.Text = conn.currentSegment.yname + "(p:" + conn.currentSegment.ypart + ")";
            }
        }
    }
}
