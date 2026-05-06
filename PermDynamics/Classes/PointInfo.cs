using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace PermDynamics.Classes
{
    public class PointInfo
    {
        public double value { get; set; }
        public Line line { get; set; }
        public PointInfo(double value)
        {
            this.value = value;
        }
    }
}
