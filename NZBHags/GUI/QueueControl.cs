using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NZBHags.lib;
using NZBHags.GUI;

namespace NZBHags
{
    public partial class QueueControl : UserControl, IUpdatingControl
    {
        public FileCollection collection;
        private MainGUI gui;
        private ulong lastProgress = 0;
        private ulong speed = 0;
        public QueueControl(FileCollection collection, MainGUI gui)
        {
            this.gui = gui;
            this.collection = collection;
            InitializeComponent();
            UpdateUI();
            
        }

        private string bytesToString(long nBytes)
        {
            int sizei = 0;
            while (nBytes > 1024)
            {
                nBytes = nBytes / 1024;
                sizei++;
            }
            string outsize = null;
            switch (sizei)
            {
                case 0:
                    outsize = string.Format("{0:n} B", nBytes);
                    break;
                case 1:
                    outsize = string.Format("{0:n} KB", nBytes);
                    break;
                case 2:
                    outsize = string.Format("{0:n} MB", nBytes);
                    break;
                case 3:
                    outsize = string.Format("{0:n} GB", nBytes);
                    break;
            }
            return outsize;
        }

        public void UpdateUI() 
        {
            labelName.Text = collection.name;

            MBDone.Text = "Size:";
            labelMb.Text = bytesToString((long)collection.ProgressKB) + "/" +bytesToString((long)collection.TotalSize);

            if (collection.status != CollectionStatus.DOWNLOADING)
                labelTimeleft.Text = "Time Left: N/A";
            else
            {
                // Figure out speed
                ulong tempprogress = collection.ProgressKB - lastProgress;
                lastProgress = collection.ProgressKB;
                float time = (int)(1000f / (float)gui.timer1.Interval);
                tempprogress = (ulong)(tempprogress * time);
                speed += (ulong)(tempprogress / 1000f);
                speed /= 2;
                //labelSpeed.Text = string.Format("{0:n} KB/s", speed);
                gui.currentspeed = (int)speed;

                // Timeleft
                if (speed != 0)
                {
                    ulong left = collection.TotalSize - collection.ProgressKB; // bytes left
                    string timeleft = "";
                    int hour = 0, min = 0, secs = 0;
                    secs = (int)(left / (speed * 1000));
                    if (secs > 60)
                    {
                        min = secs / 60;
                        secs -= min * 60;
                        if (min > 60)
                        {
                            hour = min / 60;
                            min -= hour * 60;
                        }
                    }
                    timeleft = string.Format("Time Left: {0}:{1:00}:{2:00}", hour, min, secs);
                    labelTimeleft.Text = timeleft;
                }

                progressBar.Value = (int)((float)(collection.ProgressKB / (float)collection.TotalSize) * 100);
                labelProgress.Text = string.Format("{0:0}%", progressBar.Value);
            }
            
        }

        public void ResizeInLayoutEvent()
        {
            this.Size = new Size(Parent.Size.Width-6, 63);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            gui.remQueue(this);
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

        private void button1_Click_1(object sender, EventArgs e)
        {
            gui.remQueue(this);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            QueueDetailUI detail = new QueueDetailUI(ref collection);
            detail.Show();
        }
    }
}
