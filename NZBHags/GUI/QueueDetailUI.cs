using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NZBHags.GUI
{
    public partial class QueueDetailUI : Form
    {
        private ImageList il;
        private FileCollection collection;
        private TreeNode nodeCollection;
        public QueueDetailUI(ref FileCollection collection)
        {
            this.collection = collection;
            InitializeComponent();

            // Set form title
            if(collection.name != null && !collection.name.Equals(""))
                this.Text = "Details for: " + collection.name;

            InitializeImages();
            InitializeTree();
            timer1.Start();
        }

        private void InitializeImages()
        {
            il = new ImageList();
            
            il.Images.Add(new Icon("Resources/cross.ico"));
            il.Images.Add(new Icon("Resources/tick_circle.ico"));
            treeView1.ImageList = il;
        }

        public void InitializeTree()
        {
            if (collection == null)
            {
                
            }

            treeView1.AfterSelect += new TreeViewEventHandler(treeView1_AfterSelect);

            TreeNode main = new TreeNode();
            main.Text = collection.name;
            
            foreach (FileJob job in collection.files)
            {
                TreeNode jobnode = new TreeNode(job.filename);
                jobnode.Name = "J";
                jobnode.Tag = job;
                if (job.complete)
                {
                    jobnode.ImageIndex = 1;
                    jobnode.SelectedImageIndex = 1;
                }
                else
                {
                    jobnode.ImageIndex = 0;
                    jobnode.SelectedImageIndex = 0;
                }
                foreach(Segment seg in job.segments) 
                {
                    TreeNode segnode = new TreeNode("Segment "+seg.id + " - bytes: " + seg.bytes);
                    segnode.Name = "S";
                    if (seg.progress > 0)
                    {
                        segnode.ImageIndex = 1;
                        segnode.SelectedImageIndex = 1;
                    }
                    else
                    {
                        segnode.ImageIndex = 0;
                        segnode.SelectedImageIndex = 0;
                    }
                    jobnode.Nodes.Add(segnode);
                }
                main.Nodes.Add(jobnode);
            }
            main.Expand();
            nodeCollection = main;
            treeView1.Nodes.Add(main) ;
        }

        // Shows filejob if selected
        void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Name.Equals("J"))
            {
                if (splitContainer1.Panel2.Controls.Count > 0)
                    splitContainer1.Panel2.Controls.Clear();

                splitContainer1.Panel2.Controls.Add(new FileJobDetailUI((FileJob)e.Node.Tag));
                splitContainer1.Panel2.Controls[0].Dock = DockStyle.Fill;
            }
            int a;
        }

        private void UpdateTree()
        {

            for (int i = 0; i < collection.files.Count; i++)
            {
                FileJob job = collection.files[i];
                TreeNode jobnode = nodeCollection.Nodes[i];
                for (int j = 0; j < job.segments.Count; j++)
                {
                    Segment segment = job.segments[j];
                    TreeNode segnode = jobnode.Nodes[j];

                    if (segment.progress > 0)
                    {
                        segnode.ImageIndex = 1;
                        segnode.SelectedImageIndex = 1;
                    }
                }
                if (job.complete)
                {
                    jobnode.ImageIndex = 1;
                    jobnode.SelectedImageIndex = 1;
                }

            }
        }

        private void UpdateSubPanel()
        {
            if (splitContainer1.Panel2.Controls.Count == 1)
            {
                IUpdateable ui = (IUpdateable)splitContainer1.Panel2.Controls[0];
                ui.UpdateUI();
            }
        }

        public void UpdateUI() {
            UpdateTree();
            UpdateSubPanel();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void Closing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
        }
    }
}
