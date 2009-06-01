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
        public EmptyQueue()
        {
            InitializeComponent();
        }

        public void ResizeInLayoutEvent()
        {
            this.Size = new Size(Parent.Size.Width - 6, 63);
        }

        public void UpdateUI()
        {
        }
    }
}
