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

            DrawAxes(g);

            DrawXSteps(g, pen, font, brush, y0, formatX);

            DrawYSteps(g, pen, font, brush, x0, formatY);

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

        private void drawBtn_Click(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
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



        private void DrawHexagonDetails(Graphics g, Hexagon hex)
        {
            var points = GetPoints(hex);

            Pen mainPen = new Pen(hex.Color, 2);              // Товста ручка для контуру
            Pen thinPen = new Pen(Color.Gray, 1);             // Тонка для допоміжних ліній
            thinPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash; // Пунктир
            Brush vertexBrush = new SolidBrush(hex.Color);    // Для точок вершин
            Brush centerBrush = Brushes.Red;                  // Для центру


            int centerX = XtoI(hex.X);
            int centerY = YtoJ(hex.Y);


            g.FillEllipse(centerBrush, centerX - 3, centerY - 3, 6, 6);


            using (SolidBrush fillBrush = new SolidBrush(Color.FromArgb(50, hex.Color)))
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

        private bool NeedsBoundsUpdate()
        {
            // Якщо список порожній (натиснули Clear) — треба оновити (скинути) межі
            if (hexagons.Count == 0) return true;

            // 1. Знаходимо межі даних (з врахуванням нуля)
            double dataMinX = 0;
            double dataMaxX = 0;
            double dataMinY = 0;
            double dataMaxY = 0;

            foreach (var h in hexagons)
            {
                if (h.X - h.Radius < dataMinX) dataMinX = h.X - h.Radius;
                if (h.X + h.Radius > dataMaxX) dataMaxX = h.X + h.Radius;
                if (h.Y - h.Radius < dataMinY) dataMinY = h.Y - h.Radius;
                if (h.Y + h.Radius > dataMaxY) dataMaxY = h.Y + h.Radius;
            }

            // 2. Перевірка: Чи вийшли ми за межі поточного екрана?
            // (Поточні межі екрана: minX, maxX, minY, maxY)

            // Додамо маленький запас (epsilon), щоб не реагувати на мікро-похибки
            double buffer = 0.1;

            bool outLeft = dataMinX < minX + buffer;
            bool outRight = dataMaxX > maxX - buffer;
            bool outBottom = dataMinY < minY + buffer;
            bool outTop = dataMaxY > maxY - buffer;

            // Якщо хоча б з одного боку тісно — повертаємо true
            return outLeft || outRight || outBottom || outTop;
        }

        private void UpdateBounds()
        {
            if (hexagons.Count == 0)
            {
                minX = -10; maxX = 10;
                minY = -10; maxY = 10;
                return;
            }

            // 1. Рахуємо нові межі на основі фігур
            double dataMinX = 0; double dataMaxX = 0;
            double dataMinY = 0; double dataMaxY = 0;

            foreach (var h in hexagons)
            {
                if (h.X - h.Radius < dataMinX) dataMinX = h.X - h.Radius;
                if (h.X + h.Radius > dataMaxX) dataMaxX = h.X + h.Radius;
                if (h.Y - h.Radius < dataMinY) dataMinY = h.Y - h.Radius;
                if (h.Y + h.Radius > dataMaxY) dataMaxY = h.Y + h.Radius;
            }

            double width = dataMaxX - dataMinX;
            double height = dataMaxY - dataMinY;

            double centerX = (dataMinX + dataMaxX) / 2.0;
            double centerY = (dataMinY + dataMaxY) / 2.0;

            // 2. Робимо квадратну область
            double maxDimension = Math.Max(width, height);

            // ВАЖЛИВО: Ставимо мінімальний розмір екрана (наприклад, 20 одиниць).
            // Це гарантує, що навіть якщо це перша фігура радіусом 1,
            // ми не зробимо Zoom-In, а покажемо нормальний масштаб (-10..10).
            if (maxDimension < 20) maxDimension = 20;

            // 3. Додаємо 10% відступу
            double halfSide = (maxDimension * 1.1) / 2.0;

            minX = centerX - halfSide;
            maxX = centerX + halfSide;
            minY = centerY - halfSide;
            maxY = centerY + halfSide;
        }

        // Метод для розрахунку красивого кроку сітки
        private double CalculateStep(double range)
        {
            if (range == 0) return 1;

            // Ми хочемо бачити приблизно 10-15 поділок на екрані
            double targetStep = range / 10.0;

            // Знаходимо степінь десятки (наприклад, для 45 це 10, для 0.45 це 0.1)
            double magnitude = Math.Pow(10, Math.Floor(Math.Log10(targetStep)));

            // Нормалізуємо крок до діапазону [1...10)
            double normalizedStep = targetStep / magnitude;

            // Підбираємо найближчий "красивий" крок
            double step;
            if (normalizedStep < 2) step = 1;       // Крок буде типу 1, 10, 100
            else if (normalizedStep < 5) step = 2;  // Крок буде типу 2, 20, 200
            else step = 5;                          // Крок буде типу 5, 50, 500

            return step * magnitude;
        }

        private void DrawAxes(Graphics g)
        {
            Pen axisPen = new Pen(Color.Black, 2);
            Pen gridPen = new Pen(Color.LightGray, 1);
            Font font = new Font("Arial", 10);
            Brush textBrush = Brushes.Black;

            int x0 = XtoI(0);
            int y0 = YtoJ(0);

            // --- МАЛЮЄМО ОСІ (ЛІНІЇ) ---
            // Якщо нуль видно на екрані, малюємо осі там. Якщо ні - малюємо по краях або взагалі не малюємо.
            // Для простоти малюємо лінії через весь екран, які проходять через (0,0)
            g.DrawLine(axisPen, XtoI(minX), y0, XtoI(maxX), y0); // Вісь X
            g.DrawLine(axisPen, x0, YtoJ(minY), x0, YtoJ(maxY)); // Вісь Y

            // --- ОБЧИСЛЮЄМО КРОК ---
            // Беремо ширину екрана в математичних одиницях
            double step = CalculateStep(maxX - minX);

            // Налаштування тексту (як ми робили раніше)
            StringFormat formatX = new StringFormat();
            formatX.Alignment = StringAlignment.Center;
            formatX.LineAlignment = StringAlignment.Near;

            StringFormat formatY = new StringFormat();
            formatY.Alignment = StringAlignment.Far;
            formatY.LineAlignment = StringAlignment.Center;

            // --- ЦИКЛ ПО X ---
            // Починаємо не з minX, а з першого числа, кратного step
            double startX = Math.Ceiling(minX / step) * step;

            for (double val = startX; val <= maxX; val += step)
            {
                // Не малюємо нуль двічі (або малюємо, але акуратно) - тут пропустимо
                if (Math.Abs(val) < step / 10.0) continue;

                int screenX = XtoI(val);

                // Засічка
                g.DrawLine(axisPen, screenX, y0 - 5, screenX, y0 + 5);

                // Сітка (опціонально, тонка сіра лінія)
                // g.DrawLine(gridPen, screenX, YtoJ(minY), screenX, YtoJ(maxY));

                // Цифра (округляємо, щоб не було 1.0000001)
                g.DrawString(Math.Round(val, 2).ToString(), font, textBrush, screenX, y0 + 8, formatX);
            }

            // --- ЦИКЛ ПО Y ---
            double startY = Math.Ceiling(minY / step) * step;

            for (double val = startY; val <= maxY; val += step)
            {
                if (Math.Abs(val) < step / 10.0) continue;

                int screenY = YtoJ(val);

                // Засічка
                g.DrawLine(axisPen, x0 - 5, screenY, x0 + 5, screenY);

                // Сітка (опціонально)
                // g.DrawLine(gridPen, XtoI(minX), screenY, XtoI(maxX), screenY);

                // Цифра
                g.DrawString(Math.Round(val, 2).ToString(), font, textBrush, x0 - 8, screenY, formatY);
            }

            // Нуль і підписи осей
            g.DrawString("0", font, textBrush, x0 - 15, y0 + 5);
            g.DrawString("X", new Font("Arial", 12, FontStyle.Bold), textBrush, XtoI(maxX) - 20, y0 - 25);
            g.DrawString("Y", new Font("Arial", 12, FontStyle.Bold), textBrush, x0 + 5, YtoJ(maxY));
        }
    }
}
