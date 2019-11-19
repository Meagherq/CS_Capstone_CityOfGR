using ParkingLocator.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParkingLocator.Core.Interfaces
{
    public interface IParkingService
    {
        Task<List<ActiveRootObject>> GetSocrataActiveSession();
        Task<List<Space>> GetSocrataMasterList();
        Task UpdateMap();
        Task<List<List<Space>>> GetFinalSpaces();
    }
}
