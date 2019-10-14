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
        [HttpGet("zonelist")]
        public async Task<ActionResult> GetZoneListPassport()
        {
            var results = await _parkingService.GetZoneListPassport();
          
            if (results == null)
            {
                return BadRequest(results);
            }
            
            return Ok(results);
        }

        [ProducesResponseType(200)]
        [HttpGet("zoneinfo")]
        public async Task<ActionResult> GetZoneInfoPassport()
        {
            var results = await _parkingService.GetZoneListPassport();
            return Ok(results);
        }

        [ProducesResponseType(200)]
        [HttpGet("veoci/{birdType}")]
        public async Task<ActionResult> GetVeoci()
        {
            var results = await _parkingService.GetZoneListPassport();
            return Ok(results);
        }

        [ProducesResponseType(200)]
        [HttpGet("flowbird")]
        public async Task<ActionResult> GetFlowbird()
        {
            var results = await _parkingService.GetFlowbird();
            if (results == null)
            {
                return BadRequest(results);
            }
            return Ok(results);
        }

        [ProducesResponseType(200)]
        [HttpGet("socrata")]
        public async Task<ActionResult> GetSocrata()
        {
            var results = await _parkingService.GetSocrata();
            return Ok(results);
        }
    }
}
