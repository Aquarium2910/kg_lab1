
namespace kg_test
{
    public class Hexagon
    {
        public double X { get; set; }      
        public double Y { get; set; }      
        public double Radius { get; set; } 
        public Color Color { get; set; }   

        public Hexagon(double x, double y, double r, Color c)
        {
            X = x; Y = y; Radius = r; Color = c;
        }
    }
}
