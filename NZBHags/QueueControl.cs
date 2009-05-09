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
    public partial class QueueControl : UserControl
    {
        public FileCollection collection { get; set; }
        private MainGUI gui;
        public QueueControl(FileCollection collection, MainGUI gui)
        {
            this.gui = gui;
            this.collection = collection;
            InitializeComponent();
            labelName.Text = collection.name;
            
        }

        public void ResizeInLayoutEvent()
        {
            this.Size = new Size(Parent.Size.Width-6, 63);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            gui.remQueue(this);
            //Parent.Controls.Remove(this);
            //Dispose();
        }

        private void buttonQueueup_Click(object sender, EventArgs e)
        {
            gui.decrQueue(this);
        }

        private void buttonQueuedown_Click(object sender, EventArgs e)
        {
            gui.incrQueue(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (collection.status == CollectionStatus.PAUSE)
            {
                collection.status = CollectionStatus.QUEUED;
                button2.Image = Properties.Resources.pause;
            }
            else
            {
                collection.status = CollectionStatus.PAUSE;
                button2.Image = Properties.Resources.start;
            }
        }
    }
}
