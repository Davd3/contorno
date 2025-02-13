using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace contorno
{
    public class ContourData
    {
        public string NomeOperatore { get; set; }
        public string DataTime { get; set; }
        public double CentroidX { get; set; }
        public double CentroidY { get; set; }
        public double RotationAngle { get; set; }

        public ContourData() { }
    }
}