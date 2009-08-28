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

            TreeNode main = new TreeNode();
            main.Text = collection.name;
            
            foreach (FileJob job in collection.files)
            {
                TreeNode jobnode = new TreeNode(job.filename);
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

        public void UpdateTree() {
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateTree();
        }

        private void Closing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
        }
    }
}
