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

            DrawAxes(g);

            foreach (Hexagon hex in hexagons)
            {
                DrawHexagonDetails(g, hex);
            }
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            try
            {
                double x = ParseDouble(textBox2.Text);
                double y = ParseDouble(textBox3.Text);
                double r = ParseDouble(textBox1.Text);

                Color lColor = btnLineColor.BackColor;
                Color fColor = btnFillColor.BackColor;

                if (r <= 0)
                {
                    MessageBox.Show("Радіус має бути більше нуля!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Hexagon newHex = new Hexagon(x, y, r, lColor, fColor);

                hexagons.Add(newHex);

                if (NeedsBoundsUpdate())
                {
                    UpdateBounds();
                }

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

        private void btnClear_Click(object sender, EventArgs e)
        {
            hexagons.Clear();
            UpdateBounds();
            pictureBox1.Invalidate();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void btnLineColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                btnLineColor.BackColor = colorDialog1.Color;
            }
        }

        private void btnFillColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                btnFillColor.BackColor = colorDialog1.Color;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveDialog.Title = "Зберегти фігури";
            saveDialog.FileName = "hexagons.txt";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(saveDialog.FileName))
                    {
                        foreach (var hex in hexagons)
                        {
                            string line = string.Format("{0};{1};{2};{3};{4}",
                                hex.X,
                                hex.Y,
                                hex.Radius,
                                hex.LineColor.ToArgb(),
                                hex.FillColor.ToArgb());

                            writer.WriteLine(line);
                        }
                    }

                    MessageBox.Show("Файл успішно збережено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при збереженні: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DrawHexagonDetails(Graphics g, Hexagon hex)
        {
            var points = GetPoints(hex);

            Pen mainPen = new Pen(hex.LineColor, 2);              // Товста ручка для контуру
            Pen thinPen = new Pen(Color.Gray, 1);             // Тонка для допоміжних ліній
            thinPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash; // Пунктир
            Brush vertexBrush = new SolidBrush(hex.LineColor);    // Для точок вершин
            Brush centerBrush = Brushes.Red;                  // Для центру


            int centerX = XtoI(hex.X);
            int centerY = YtoJ(hex.Y);

            // Центральна точка
            g.FillEllipse(centerBrush, centerX - 3, centerY - 3, 6, 6);


            using (SolidBrush fillBrush = new SolidBrush(Color.FromArgb(50, hex.FillColor)))
            {
                g.FillPolygon(fillBrush, points);
            }

            g.DrawPolygon(mainPen, points);


            foreach (Point p in points)
            {
                g.FillEllipse(vertexBrush, p.X - 3, p.Y - 3, 6, 6);
            }
        }

        private Point[] GetPoints(Hexagon hex)
        {
            Point[] points = new Point[6];

            for (int i = 0; i < 6; i++)
            {
                double angleRad = (Math.PI / 3) * i;

                double x_math = hex.X + hex.Radius * Math.Cos(angleRad);
                double y_math = hex.Y + hex.Radius * Math.Sin(angleRad);

                points[i] = new Point(XtoI(x_math), YtoJ(y_math));
            }

            return points;
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

        private void DrawYSteps(Graphics g, Pen pen, Font font, Brush brush, int x0, StringFormat formatY, double step)
        {
            double start = Math.Ceiling(minY / step) * step;

            for (double val = start; val <= maxY; val += step)
            {
                if (Math.Abs(val) < step / 10.0) continue;

                int screenY = YtoJ(val);

                g.DrawLine(pen, x0 - 5, screenY, x0 + 5, screenY);
                g.DrawString(val.ToString("0.###"), font, brush, x0 - 8, screenY, formatY);
            }
        }

        private void DrawXSteps(Graphics g, Pen pen, Font font, Brush brush, int y0, StringFormat formatX, double step)
        {
            double start = Math.Ceiling(minX / step) * step;

            for (double val = start; val <= maxX; val += step)
            {
                if (Math.Abs(val) < step / 10.0) continue;

                int screenX = XtoI(val);

                g.DrawLine(pen, screenX, y0 - 5, screenX, y0 + 5);

                g.DrawString(val.ToString("0.###"), font, brush, screenX, y0 + 8, formatX);
            }
        }

        private bool NeedsBoundsUpdate()
        {
            if (hexagons.Count == 0) return false;

            foreach (var h in hexagons)
            {
                if (h.X + h.Radius > maxX || h.X - h.Radius < minX ||
                    h.Y + h.Radius > maxY || h.Y - h.Radius < minY)
                {
                    return true;
                }
            }
            return false;
        }

        private void UpdateBounds()
        {
            double maxExtent = 10.0;

            if (hexagons.Count > 0)
            {
                foreach (var h in hexagons)
                {
                    double distExploreX = Math.Abs(h.X) + h.Radius;
                    double distExploreY = Math.Abs(h.Y) + h.Radius;

                    if (distExploreX > maxExtent) maxExtent = distExploreX;
                    if (distExploreY > maxExtent) maxExtent = distExploreY;
                }
            }

            maxExtent *= 1.1;

            minX = -maxExtent;
            maxX = maxExtent;
            minY = -maxExtent;
            maxY = maxExtent;
        }

        // Метод для розрахунку красивого кроку сітки
        private double CalculateStep(double range)
        {
            if (range == 0) return 1;

            double targetStep = range / 10.0;

            // Знаходимо степінь двійки/десятки (порядок числа)
            // Наприклад, для 45 це буде 10, для 0.45 це буде 0.1
            double magnitude = Math.Pow(10, Math.Floor(Math.Log10(targetStep)));

            // Нормалізуємо крок до діапазону [1...10)
            double normalizedStep = targetStep / magnitude;

            // Вибираємо красивий крок: 1, 2 або 5
            double step;
            if (normalizedStep < 2) step = 1;
            else if (normalizedStep < 5) step = 2;
            else step = 5;

            return step * magnitude;
        }

        private void DrawAxes(Graphics g)
        {
            Pen axisPen = new Pen(Color.Black, 2);
            Font font = new Font("Arial", 10);
            Brush textBrush = Brushes.Black;

            int x0 = XtoI(0);
            int y0 = YtoJ(0);

            g.DrawLine(axisPen, XtoI(minX), y0, XtoI(maxX), y0);
            g.DrawLine(axisPen, x0, YtoJ(minY), x0, YtoJ(maxY));

            StringFormat formatX = new StringFormat();
            formatX.Alignment = StringAlignment.Center;
            formatX.LineAlignment = StringAlignment.Near;

            StringFormat formatY = new StringFormat();
            formatY.Alignment = StringAlignment.Far;
            formatY.LineAlignment = StringAlignment.Center;

            var stepX = CalculateStep(maxX - minX);
            var stepY = CalculateStep(maxY - minY);

            DrawXSteps(g, axisPen, font, textBrush, y0, formatX, stepX);
            DrawYSteps(g, axisPen, font, textBrush, x0, formatY, stepY);


            g.DrawString("0", font, textBrush, x0 + 2, y0 + 8);
            g.DrawString("X", new Font("Arial", 12, FontStyle.Bold), textBrush, XtoI(maxX) - 15, y0 - 25);
            g.DrawString("Y", new Font("Arial", 12, FontStyle.Bold), textBrush, x0 + 5, YtoJ(maxY));
        }

        private double ParseDouble(string val)
        {
            return double.Parse(val.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
