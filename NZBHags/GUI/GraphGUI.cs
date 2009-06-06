using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NZBHags.lib;

namespace NZBHags.GUI
{
    public partial class GraphGUI : UserControl, IUpdatingControl
    {
        // Value Queue
        private static int MAXVALUES = 60;
        public Queue<int> s = new Queue<int>(MAXVALUES);
        
        // Graphics
        private Graphics g;
        public bool antiAliasing { get; set; }
        
        // Text
        private Font font = new Font("Arial", 8);
        private Brush textBrush = Brushes.Gold;

        // Background
        private LinearGradientBrush backgroundBrush;
        private Color backgroundColor1 = Color.YellowGreen;
        private Color backgroundColor2 = Color.DarkOliveGreen;

        // Graph-line
        private Pen graphPen;
        private Brush graphBrush = Brushes.Gold;

        // Average-line
        private Pen averagePen = new Pen(Color.LightGreen);
        private int averageValue = 0;
        public bool drawAverage { get; set; }

        // Grid
        private Pen gridPen = new Pen(Color.DarkOliveGreen);
        public bool drawGrid { get; set; }
        public static int GRIDWIDTH = 15;
        public static int GRIDHEIGHT = 15;
        private int gridScrollOffset = 0;
        private float gridScrollPixels = 0f;

        // Constructor
        public GraphGUI()
        {
            InitializeComponent();
            gridPen.DashStyle = DashStyle.Dot;
            averagePen.DashStyle = DashStyle.Dash;
            drawGrid = true;
            antiAliasing = true;
            drawAverage = true;
            // Intitialize Graph background colors
            backgroundBrush = new LinearGradientBrush(new Rectangle(0, 0, Size.Width, Size.Height), backgroundColor1, backgroundColor2, LinearGradientMode.Vertical);

            // Init graphics object
            g = Graphics.FromHwnd(panel1.Handle);
            if(antiAliasing)
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Init pen for drawing
            graphPen = new Pen(graphBrush, 1.5f);

            gridScrollPixels = (int)((float)Size.Width / (float)MAXVALUES);
            // Testing values
            TestValues();
        }

        private void TestValues()
        {
            s.Enqueue(1000);
            s.Enqueue(2000);
            s.Enqueue(1500);
            s.Enqueue(1750);
            s.Enqueue(250);
            s.Enqueue(1750);
        }

        // Called when parent is resized
        public void ResizeInLayoutEvent()
        {
            this.Size = new Size(Parent.Size.Width, Parent.Size.Height);
            panel1.Size = new Size(Parent.Size.Width, Parent.Size.Height);
            backgroundBrush = new LinearGradientBrush(new Rectangle(0, 0, Size.Width, Size.Height), backgroundColor1, backgroundColor2, LinearGradientMode.Vertical);
            gridScrollPixels = ((float)Size.Width / (float)MAXVALUES);
            UpdateUI();
        }

        // Updates graph
        public void UpdateUI()
        {
            // Copy stack to array
            int[] values = s.ToArray();
            Point[] points = new Point[values.Length];
            int xAlign = MAXVALUES - values.Length; // keeps graph aligned to the right side

            // Figure out maxvalue & average in array
            int maxval = 1;
            ulong average = 0;
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] > maxval)
                    maxval = values[i];
                if (drawAverage)
                    average += (ulong)values[i];
            }
            if (drawAverage)
                averageValue = (int)(average / (ulong)values.Length);
            float valueDivider = ((float)Size.Height - 4f) / (float)maxval;
            
            // Populate points position
            for (int i = 0; i < values.Length; i++)
            {
                int x = (int)((i + xAlign) * ((float)Size.Width / (float)MAXVALUES));
                int y = (int)((float)values[i] * valueDivider * -1) + (Size.Height - 6);
                points[i] = new Point(x, y);
            }

            // Clear panel
            g.Clear(Color.White);

            // Draw background
            g.FillRectangle(backgroundBrush, new Rectangle(0, 0, Size.Width, Size.Height));

            // Draw grid
            if (drawGrid)
            {
                int relativeGridOffset = (int)(gridScrollOffset * gridScrollPixels);
                if (relativeGridOffset > GRIDWIDTH)
                    relativeGridOffset = relativeGridOffset % GRIDWIDTH;
                for (int i = Size.Width - relativeGridOffset; i > 0; i -= GRIDWIDTH)
                {
                    g.DrawLine(gridPen, new Point(i, 0), new Point(i, Size.Height));
                }
                for (int i = 0; i < Size.Height; i += GRIDHEIGHT)
                {
                    g.DrawLine(gridPen, new Point(0, i), new Point(Size.Width, i));
                }
            }

            // Draw Average
            if (drawAverage && values.Length > 0)
            {
                int y = (int)((float)averageValue * valueDivider * -1f) + (Size.Height - 6);
                g.DrawLine(averagePen, new Point(0, y), new Point(Size.Width, y));
            }

            // Draw Graph
            if (points.Length > 1)
                g.DrawCurve(graphPen, points, 0f);

            // Draw text
            string cur = string.Format("Cur: {0}/s", bytesToString((ulong)values[values.Length - 1])); 
            string max = string.Format("Max: {0}/s", bytesToString((ulong)maxval));
            SizeF sCur, sMax;
            
            sCur = g.MeasureString(cur, font);
            sMax = g.MeasureString(max, font);
            g.DrawString(cur, font, textBrush, new PointF(Size.Width-sCur.Width-2, Size.Height-sCur.Height-5));
            g.DrawString(max, font, textBrush, new PointF(Size.Width-sMax.Width-2, 0f));

        }

        private string bytesToString(float nBytes)
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
                //case 0:
                //    outsize = string.Format("{0:n} B", nBytes);
                //    break;
                case 0:
                    outsize = string.Format("{0:n} KB", nBytes);
                    break;
                case 1:
                    outsize = string.Format("{0:n} MB", nBytes);
                    break;
                case 2:
                    outsize = string.Format("{0:n} GB", nBytes);
                    break;
            }
            return outsize;
        }

        public void UpdateUI(int speed)
        {
            // If queue is full, remove one element
            if (s.Count == MAXVALUES)
            {
                s.Dequeue();
            }
            s.Enqueue(speed);

            gridScrollOffset++;

            UpdateUI();
        }
    }
}
