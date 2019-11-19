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
        [HttpGet("spaces")]
        public async Task<ActionResult> GetFinalSpaces()
        {
            var result = await _parkingService.GetFinalSpaces();
            return Ok(result);
        }

        [ProducesResponseType(200)]
        [HttpGet("socrata")]
        public async Task<ActionResult> GetSocrataMasterList()
        {
            var results = await _parkingService.GetSocrataMasterList();
            return Ok(results);
        }

        [ProducesResponseType(200)]
        [HttpGet("activesocrata")]
        public async Task<ActionResult> GetSocrataActiveSession()
        {
            var results = await _parkingService.GetSocrataActiveSession();
            return Ok(results);
        }
    }
}
