namespace kg_test
{
    public partial class Form1 : Form
    {
        double minX = -10.0;
        double maxX = 10.0;

        double minY = -10.0;
        double maxY = 10.0;

        int margin = 50;

        List<Hexagon> hexagons = new List<Hexagon>();

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

            foreach (Hexagon hex in hexagons)
            {
                DrawHexagonDetails(g, hex);
            }
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            try
            {
                double x = double.Parse(textBox2.Text);
                double y = double.Parse(textBox3.Text);
                double r = double.Parse(textBox1.Text);

                if (r <= 0)
                {
                    MessageBox.Show("Радіус має бути більше нуля!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Hexagon newHex = new Hexagon(x, y, r, Color.Blue);

                hexagons.Add(newHex);

                pictureBox1.Invalidate();

                textBox2.Clear();
                textBox3.Clear();
                textBox1.Clear();
            }
            catch (FormatException)
            {
                MessageBox.Show("Будь ласка, введіть коректні числові значення!", "Помилка вводу", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void drawBtn_Click(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void DrawHexagonDetails(Graphics g, Hexagon hex)
        {
            // --- ЕТАП 1: Розрахунок математики ---
            Point[] points = new Point[6]; // Масив для 6 вершин на екрані

            for (int i = 0; i < 6; i++)
            {
                // Кут у радіанах (60 градусів * i)
                double angleRad = (Math.PI / 3) * i;

                // Формула полярних координат:
                // x = центр + радіус * cos(кута)
                // y = центр + радіус * sin(кута)
                double x_math = hex.X + hex.Radius * Math.Cos(angleRad);
                double y_math = hex.Y + hex.Radius * Math.Sin(angleRad);

                // Перетворюємо математичні координати в пікселі
                points[i] = new Point(XtoI(x_math), YtoJ(y_math));
            }

            // --- ЕТАП 2: Підготовка інструментів ---
            Pen mainPen = new Pen(hex.Color, 2);              // Товста ручка для контуру
            Pen thinPen = new Pen(Color.Gray, 1);             // Тонка для допоміжних ліній
            thinPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash; // Пунктир
            Brush vertexBrush = new SolidBrush(hex.Color);    // Для точок вершин
            Brush centerBrush = Brushes.Red;                  // Для центру

            // Центр у пікселях
            int centerX = XtoI(hex.X);
            int centerY = YtoJ(hex.Y);

            // --- ЕТАП 3: Малювання допоміжних елементів ---

            // А) Описане коло
            // Рахуємо радіус у пікселях як відстань від центру до першої вершини
            // (використовуємо теорему Піфагора для точності)
            int dx = points[0].X - centerX;
            int dy = points[0].Y - centerY;
            int r_px = (int)Math.Sqrt(dx * dx + dy * dy);

            // Малюємо коло (x, y - це лівий верхній кут описаного квадрата)
            g.DrawEllipse(thinPen, centerX - r_px, centerY - r_px, r_px * 2, r_px * 2);

            // Б) Діагоналі (з'єднуємо протилежні вершини: 0-3, 1-4, 2-5)
            g.DrawLine(thinPen, points[0], points[3]);
            g.DrawLine(thinPen, points[1], points[4]);
            g.DrawLine(thinPen, points[2], points[5]);

            // В) Центр фігури
            g.FillEllipse(centerBrush, centerX - 3, centerY - 3, 6, 6);


            // --- ЕТАП 4: Малювання основної фігури ---

            // Г) Заливка (напівпрозора)
            // Створюємо колір з прозорістю 50 (із 255)
            using (SolidBrush fillBrush = new SolidBrush(Color.FromArgb(50, hex.Color)))
            {
                g.FillPolygon(fillBrush, points);
            }

            // Д) Контур шестикутника
            g.DrawPolygon(mainPen, points);

            // Е) Вершини (маленькі точки на кутах)
            foreach (Point p in points)
            {
                g.FillEllipse(vertexBrush, p.X - 3, p.Y - 3, 6, 6);
            }
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
