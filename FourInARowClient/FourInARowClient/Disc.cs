using System.Windows.Shapes;

namespace FourInARowClient

{
    internal class Disc
    {
        public Ellipse Circle { get; set; }
        public double X { get; set; }
        public double Y { get; set; }       
        public int YMove { get; set; }

        public int Column { get; set; }
    }
}