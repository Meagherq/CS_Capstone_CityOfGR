using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLocator.Core.Entities
{
    public class RootObject
    {
        public TheGeom the_geom { get; set; }
        public string objectid { get; set; }
        public string meterid { get; set; }
        public string spaceid { get; set; }
        public string mobile_pay_zone { get; set; }
        public string space_type { get; set; }
        public string space_size { get; set; }
        public string painted_space { get; set; }
        public string parking_regulation_type { get; set; }
        public string ev_charging_station { get; set; }
        public string disabled_parking { get; set; }
        public string location { get; set; }
        public string side_of_street { get; set; }
        public string to_street { get; set; }
        public string from_street { get; set; }
        public string operational_days { get; set; }
        public string operational_hours { get; set; }
        public string enforcement_type { get; set; }
        public string payment_type { get; set; }
        public string rate { get; set; }
        public string rate_duration { get; set; }
        public string max_time_minutes { get; set; }
        public string max_time_hours { get; set; }
        public string food_truck_parking { get; set; }
        public string creator { get; set; }
        public DateTime create_date { get; set; }
        public string editor { get; set; }
        public DateTime last_editor { get; set; }
        public string shape_starea { get; set; }
        public string shape_stlength { get; set; }
    }
}
