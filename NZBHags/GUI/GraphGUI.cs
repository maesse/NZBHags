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
        public Queue<uint> s = new Queue<uint>(MAXVALUES);
        
        // Graphics
        private Graphics g;
        public bool antiAliasing { get; set; }
        
        // Text
        private Font font = new Font("Arial", 9);
        private Brush textBrush = Brushes.Khaki;
        private Brush textShadowBrush = Brushes.Black;

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
           // this.SetStyle(ControlStyles.DoubleBuffer, true);
            // Init graphics object
            g = Graphics.FromHwnd(panel1.Handle);
            if(antiAliasing)
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Init pen for drawing
            graphPen = new Pen(graphBrush, 1.7f);

            gridScrollPixels = (int)((float)Size.Width / (float)MAXVALUES);
            // Testing values
            //TestValues();
            
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
            uint[] values = s.ToArray();
            Point[] points = new Point[values.Length];
            int xAlign = MAXVALUES - values.Length; // keeps graph aligned to the right side

            // Figure out maxvalue & average in array
            uint maxval = 0;
            ulong average = 0;
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] > maxval)
                    maxval = values[i];
                if (drawAverage && i>(values.Length/2))
                    average += (ulong)values[i];
            }
            if (drawAverage && values.Length > 0)
            {
                averageValue = (int)((average * 2) / (ulong)values.Length);
            }
            float valueDivider;
            if (maxval > 0)
            {
                valueDivider = ((float)Size.Height - 4f) / (float)maxval;
            }
            else
            {
                valueDivider = ((float)Size.Height - 4f) / 1f;
            }
            
            // Populate points position
            for (int i = 0; i < values.Length; i++)
            {
                int x = (int)((i + xAlign) * ((float)Size.Width / (float)MAXVALUES));
                int y = (int)((float)values[i] * valueDivider * -1) + (Size.Height - 6);
                points[i] = new Point(x, y);
            }

            // Clear panel
            //g.Clear(Color.White);

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
                int y = (int)((float)averageValue * valueDivider * -1f *2) + (Size.Height - 6);
                g.DrawLine(averagePen, new Point(0, y), new Point(Size.Width, y));
            }

            // Draw Graph
            if (points.Length > 1)
                g.DrawCurve(graphPen, points, 0f);

            // Draw text
            ulong currentvalue = 0;
            if (values.Length > 0)
                currentvalue = (ulong)values[values.Length - 1];
            string cur = string.Format("Cur: {0}/s", bytesToString(currentvalue)); 
            string max = string.Format("Max: {0}/s", bytesToString((ulong)maxval));
            
            SizeF sCur, sMax, sAvg;
            
            sCur = g.MeasureString(cur, font);
            sMax = g.MeasureString(max, font);
            
            // current-speed text
            g.DrawString(cur, font, textShadowBrush, new PointF(Size.Width - sCur.Width - 1, Size.Height - sCur.Height - 4));
            g.DrawString(cur, font, textBrush, new PointF(Size.Width-sCur.Width-2, Size.Height-sCur.Height-5));
            // Avg-speed text
            g.DrawString(max, font, textShadowBrush, new PointF(Size.Width - sMax.Width - 1, 1f));
            g.DrawString(max, font, textBrush, new PointF(Size.Width-sMax.Width-2, 0f));
            if (drawAverage && values.Length > 0)
            {
                string avg = string.Format("Avg: {0}/s", bytesToString((ulong)averageValue));

                sAvg = g.MeasureString(avg, font);
                // Align average text to line
                float avgy = ((float)averageValue * valueDivider * -1f *2) + (Size.Height - 6);
                if ((avgy + 6 + sAvg.Height) > Size.Height)
                {
                    avgy = Size.Height - sAvg.Height - 5;
                }
                g.DrawString(avg, font, textShadowBrush, new PointF(1, avgy+1));
                g.DrawString(avg, font, textBrush, new PointF(0, avgy));
            }
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

        public void UpdateUI(uint speed)
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
