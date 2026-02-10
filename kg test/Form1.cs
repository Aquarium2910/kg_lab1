namespace kg_test
{
    public partial class Form1 : Form
    {
        double minX = -10.0;
        double maxX = 10.0;

        double minY = -10.0;
        double maxY = 10.0;

        int margin = 50;

        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.White);

            Pen pen = new Pen(Color.Black, 2);
            Font font = new Font("Arial", 10);
            Brush brush = Brushes.Black;

            int x0 = XtoI(0);
            int y0 = YtoJ(0);

            StringFormat formatX = new StringFormat();
            formatX.Alignment = StringAlignment.Center;
            formatX.LineAlignment = StringAlignment.Near;

            StringFormat formatY = new StringFormat();
            formatY.Alignment = StringAlignment.Far;
            formatY.LineAlignment = StringAlignment.Center;

            g.DrawLine(pen, XtoI(minX), y0, XtoI(maxX), y0);
            g.DrawLine(pen, x0, YtoJ(minY), x0, YtoJ(maxY));

            DrawXSteps(g, pen, font, brush, y0, formatX);

            DrawYSteps(g, pen, font, brush, x0, formatY);

            g.DrawString("0", font, brush, x0 - 15, y0 + 5);

            g.DrawString("X", new Font("Arial", 12, FontStyle.Bold), brush, XtoI(maxX) + 10, y0 - 10);
            g.DrawString("Y", new Font("Arial", 12, FontStyle.Bold), brush, x0 + 5, YtoJ(maxY) - 20);
        }


        private void drawBtn_Click(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }


        private int XtoI(double x)
        {
            int W = pictureBox1.Width - 2 * margin;

            return margin + (int)((x - minX) * (W / (maxX - minX)));
        }

        private int YtoJ(double y)
        {
            int H = pictureBox1.Height - 2 * margin;

            return (pictureBox1.Height - margin) - (int)((y - minY) * (H / (maxY - minY)));
        }        
        
        private void DrawYSteps(Graphics g, Pen pen, Font font, Brush brush, int x0, StringFormat formatY)
        {
            for (int i = (int)Math.Ceiling(minY); i <= (int)Math.Floor(maxY); i++)
            {
                if (i == 0) continue;

                int screenY = YtoJ(i);

                g.DrawLine(pen, x0 - 5, screenY, x0 + 5, screenY);

                g.DrawString(i.ToString(), font, brush, x0 - 8, screenY, formatY);
            }
        }

        private void DrawXSteps(Graphics g, Pen pen, Font font, Brush brush, int y0, StringFormat formatX)
        {
            for (int i = (int)Math.Ceiling(minX); i <= (int)Math.Floor(maxX); i++)
            {
                if (i == 0) continue;

                int screenX = XtoI(i);

                g.DrawLine(pen, screenX, y0 - 5, screenX, y0 + 5);

                g.DrawString(i.ToString(), font, brush, screenX, y0 + 8, formatX);
            }
        }
    }
}
