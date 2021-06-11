using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edge_detection
{
    public partial class HoughForm : Form
    {
        private Bitmap inputBitmap;
        private Image original;
        private string type;

        public HoughForm(Image afterProc, Image original, string type)
        {
            InitializeComponent();
            inputBitmap = new Bitmap(afterProc);
            this.original = original;
            this.type = type;

            pictureBox2.Image = original;
        }
        
        private void StartHough()
        {
            switch (type)
            {
                case "circles":
                    StartHoughCircles();
                    break;

                case "lines":
                    StartHoughLines();
                    break;
            }
        }

        private void SwitchEnableElements(bool enable)
        {
            trackBar1.Enabled = enable;
            trackBar2.Enabled = enable;
        }

        private void StartHoughCircles()
        {
            SwitchEnableElements(false);

            int r = trackBar2.Value;
            int tr = trackBar1.Value;

            pictureBox1.Image = Hough.TransformCircle(inputBitmap, r);
            pictureBox2.Image = new Bitmap(original);

            //Bitmap img = new Bitmap(original);
            Graphics g = Graphics.FromImage(pictureBox2.Image);
            Pen pen = new Pen(Color.Black, 3);

            Point Size = new Point(inputBitmap.Width, inputBitmap.Height);
            while (true)
            {
                Point pt = Hough.SearchCircle(Size, tr);
                if (pt.X == -1) break;
                g.DrawEllipse(pen, pt.X - r, pt.Y - r, r + r, r + r);
            }
            pictureBox2.Refresh();

            SwitchEnableElements(true);
        }

        private void StartHoughLines()
        {
            SwitchEnableElements(false);

            trackBar2.Visible = false;
            label2.Visible = false;
            numericUpDown2.Visible = false;

            int tr = trackBar1.Value;

            pictureBox1.Image = Hough.TransformLine(inputBitmap);
            pictureBox2.Image = new Bitmap(original);

            Bitmap img = new Bitmap(original);
            Graphics g = Graphics.FromImage(pictureBox2.Image);
            Pen pen = new Pen(Color.Red, 3);

            int dp = (int)Math.Round(Math.Sqrt(Math.Pow(inputBitmap.Width, 2) + Math.Pow(inputBitmap.Height, 2)));
            Point Size = new Point(180, dp);

            while (true)
            {
                Point pt = Hough.SearchLine(Size, tr);
                if (pt.X == -1) break;
                if (pt.X > 0)
                {
                    int y1 = (int)((-Math.Cos(pt.X * (Math.PI / 180)) / Math.Sin(pt.X * (Math.PI / 180))) * 0 + (double)pt.Y / Math.Sin(pt.X * (Math.PI / 180)));
                    int y2 = (int)((-Math.Cos(pt.X * (Math.PI / 180)) / Math.Sin(pt.X * (Math.PI / 180))) * img.Width + (double)pt.Y / Math.Sin(pt.X * (Math.PI / 180)));
                    g.DrawLine(pen, 0, y1, img.Width, y2);
                }
            }
            pictureBox2.Refresh();
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            SwitchEnableElements(true);
        }

        private void trackBar2_MouseUp(object sender, MouseEventArgs e)
        {
            StartHough();
        }

        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            StartHough();
        }

        private void HoughForm_Load(object sender, EventArgs e)
        {
            StartHough();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            numericUpDown1.Value = trackBar1.Value;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            numericUpDown2.Value = trackBar2.Value;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            trackBar1.Value = (int) numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            trackBar2.Value = (int) numericUpDown2.Value;
        }
    }
}
