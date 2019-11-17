using ParkingLocator.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParkingLocator.Core.Interfaces
{
    public interface IParkingService
    {
        Task<string> GetZoneListPassport();
        Task<string> GetZoneInfoPassport();
        Task<string> GetVeoci();
        Task<string> GetFlowbird();
        Task<string> GetSocrataActiveSession();
        Task<List<Space>> GetSocrataMasterList();
    }
}
