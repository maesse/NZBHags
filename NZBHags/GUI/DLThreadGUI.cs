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
    public delegate void UpdateDelegate();
    public partial class DLThreadGUI : UserControl, IUpdatingControl
    {
        
        
        public NNTPConnection conn;
        public UpdateDelegate updateDelegate;
        public DLThreadGUI(NNTPConnection connection)
        {
            conn = connection;
            updateDelegate = new UpdateDelegate(this.UpdateUI);
            InitializeComponent();

        }

        public void ResizeInLayoutEvent()
        {
            this.Size = new Size(Parent.Size.Width - 24, 22);
        }

        public void UpdateUI()
        {
            
            labelName.Text = "" + conn.id + ": ";
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
                labelFileJob.Text = "(" + conn.currentSegment.id + "/" + conn.currentSegment.parent.segments.Count + ")" +conn.currentSegment.parent.filename;
            }
        }
    }
}
