using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLocator.Core.Entities
{
    public class TheGeom
    {
        public string type { get; set; }
        public List<List<List<List<double>>>> coordinates { get; set; }
    }
}
