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
    public partial class FileJobDetailUI : UserControl, IUpdateable
    {
        private FileJob job;
        private Segment currentSegment = null;
        public FileJobDetailUI(FileJob job)
        {
            this.job = job;
            InitializeComponent();
            Initialize();
        }

        private void Initialize() {
            // Progressbar
            progressBarParts.Maximum = job.SegmentCount;
            progressBarParts.Minimum = 0;
            progressBarParts.Value = job.SegmentProgress;

            // String above progressbar
            string progress = job.SegmentProgress + "/" + job.SegmentCount;
            labelProgress.Text = progress;
            
            // Fill segments in list
            if(job.segments != null)
            foreach (Segment seg in job.segments)
            {
                listBoxParts.Items.Add(seg);
            }

            // Heading
            string heading = job.filename;
            if (heading == null || heading.Equals(""))
                heading = "N/A";
            labelHeading.Text = heading;

            // Size label
            labelSize.Text = Misc.GetPrettyFileSize(job.size);
        }

        private void UpdateListBoxParts()
        {
            // TOdo: FIX
            foreach (object obj in listBoxParts.Items)
            {
                int a = 1;
                int b;
            }
        }

        public void UpdateUI()
        {
            // Progressbar
            progressBarParts.Value = job.SegmentProgress;

            // String above progressbar
            string progress = job.SegmentProgress + "/" + job.SegmentCount;
            labelProgress.Text = progress;

            UpdateListBoxParts();

            if (currentSegment != null)
                UpdateSegmentInfo(currentSegment);
        }

        // Updates gui elements
        private void UpdateSegmentInfo(Segment seg)
        {
            labelPart.Text = seg.id + "/" + job.SegmentCount;
            labelStatus.Text = Enum.GetName(typeof(Segment.Status), seg.status);
            string poster = seg.addr;
            if (seg.addr.Contains('@'))
            {
                int index = seg.addr.IndexOf('@')+1;
                poster = seg.addr.Substring(index, seg.addr.Length - index);
                if (poster.EndsWith(">"))
                    poster = poster.Substring(0, poster.Length - 1);
            }
            labelPoster.Text = poster;
            labelSegSize.Text = Misc.GetPrettyFileSize((ulong)seg.bytes);
            string crc;
            if (seg.crc != null)
            {
                if (seg.crcOk)
                    crc = seg.crc + " OK";
                else
                    crc = seg.crc + " FAILED";
            }
            else
                crc = "N/A";
            labelCRC.Text = crc;
            labelYenc.Text = (seg.isYenc) ? "Yes" : "No";
            labelYPart.Text = (seg.YPart == -1) ? "N/A" : seg.YPart + "";
            labelYName.Text = (seg.yname == null) ? "N/A" : seg.yname;

        }

        private void PartSelected(object sender, EventArgs e)
        {
            ListBox list = (ListBox)sender;
            // Set and Update segment gui
            if (list.SelectedItem != null && list.SelectedItem is Segment)
            {
                currentSegment = (Segment)list.SelectedItem;
                UpdateSegmentInfo(currentSegment);
            }
        }




    }
}
