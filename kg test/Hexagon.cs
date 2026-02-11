public class Hexagon
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Radius { get; set; }

    public Color LineColor { get; set; }
    public Color FillColor { get; set; }

    public Hexagon(double x, double y, double r, Color line, Color fill)
    {
        X = x;
        Y = y;
        Radius = r;
        LineColor = line;
        FillColor = fill;
    }
}