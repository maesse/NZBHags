using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GraphTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void updateCurve()
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Point[] points = new Point[8];

            points[0] = new Point(0, 0);
            points[1] = new Point(10, 10);
            points[2] = new Point(20, 20);
            points[3] = new Point(30, 50);
            points[4] = new Point(50, 20);
            points[5] = new Point(70, 100);
            points[6] = new Point(90, 70);
            points[7] = new Point(150, 85);
            Pen pen = new Pen(Brushes.Black, 2.4f);
            Graphics gfx = Graphics.FromHwnd(panel1.Handle);
            gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            gfx.DrawCurve(pen, points);
            //gfx.DrawBeziers(pen, points);
        }
    }
}
