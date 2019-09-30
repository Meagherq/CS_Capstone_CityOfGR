using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLocator.Core.Entities
{
    public class Zone
    {
        public int Count { get; set; }
        public int AvailableSpots { get; set; }
        public IList<Space> Spaces { get; set; }
    }
}
