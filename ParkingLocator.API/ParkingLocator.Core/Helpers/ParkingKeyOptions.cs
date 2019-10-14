using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLocator.Core.Helpers
{
    public class ParkingKeyOptions
    {
        public string PassportEndpoint { get; set; }
        public string PassportStagingKey { get; set; }
        public string PassportAdminKey { get; set; }
        public string FlowbirdEndpoint { get; set; }
        public string FlowbirdUsername { get; set; }
        public string FlowbirdPassword { get; set; }
        public string VeociEndpoint { get; set; }
        public string VeociPublicKey { get; set; }
        public string VeociSecretKey { get; set; }
        public string SocrataEndpoint { get; set; }
        public string SocrataAdminKey { get; set; }
        public string SocrataClientId { get; set; }
        public string SocrataClientSecret { get; set; }
    }
}
