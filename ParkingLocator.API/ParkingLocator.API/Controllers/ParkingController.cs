using Microsoft.AspNetCore.Mvc;
using ParkingLocator.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLocator.API.Controllers
{
    public class ParkingController : BaseController
    {
        private readonly IParkingService _parkingService;
        public ParkingController(IParkingService parkingService)
        {
            _parkingService = parkingService;
        }

        [ProducesResponseType(200)]
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var results = await _parkingService.GetZoneList();
            return Ok(results);
        }
    }
}
