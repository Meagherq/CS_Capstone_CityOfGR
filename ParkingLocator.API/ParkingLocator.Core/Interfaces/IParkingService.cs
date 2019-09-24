using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParkingLocator.Core.Interfaces
{
    public interface IParkingService
    {
        Task<string> GetZoneList();
        Task<string> GetZone();
    }
}
