using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLocator.Core.Entities
{
    public class Space
    {
        public int ObjectId { get; set; }
        public List<List<double>> BoundingBox { get; set; }
        public string SpaceId { get; set; }
        public string MeterId { get; set; }
        //public int MobilePayZone { get; set; }
        //public string SpaceType { get; set; }
        //public string SpaceSize { get; set; }
        //public string SideOfStreet { get; set; }
        //public string ToStreet { get; set; }
        //public string FromStreet { get; set; }
        public string OperationalDays { get; set; }
        public string OperationalHours { get; set; }
        //public string EnforcementType { get; set; }
        //public string PaymentType { get; set; }
        //public double Rate { get; set; }
        //public string RateDuration { get; set; }
        //public int MaxTimeMinutes { get; set; }
        //public string ParkingRestriction { get; set; }
    }
}
