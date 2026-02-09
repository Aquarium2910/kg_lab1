namespace kg_test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics; // Отримуємо інструмент художника
            g.Clear(Color.White);    // Очищаємо фон
            g.DrawRectangle(Pens.Red, 10, 10, 100, 50); // Малюємо тест (у пікселях)

            PointF point1 = new PointF(40, 100);
            PointF point2 = new PointF(100, 100);

            g.DrawLine(Pens.Black, point1, point2);
        }

        private void drawBtn_Click(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }
    }
}
