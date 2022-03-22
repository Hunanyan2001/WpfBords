using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfBords
{
    public class Ball
    {
        public Ellipse Ellipse { get; set; }
        public Directions Direction { get; set; } 
        public int Speed { get; set; }
        public SolidColorBrush Color { get; set; }
    }
}
