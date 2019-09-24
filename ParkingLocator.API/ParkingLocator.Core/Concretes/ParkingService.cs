using Microsoft.Extensions.Options;
using ParkingLocator.Core.Helpers;
using ParkingLocator.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ParkingLocator.Core.Concretes
{
    public class ParkingService : IParkingService
    {
        private readonly IHttpClientFactory _clientFactory;
        public ParkingService(IHttpClientFactory clientFactory, IOptions<ParkingKeyOptions> option)
        {
            _clientFactory = clientFactory;
        }

        public async Task<string> GetZoneList()
        {
            string results = "";
            var request = new HttpRequestMessage(HttpMethod.Get,
            "https://test.com");
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorizationg", "Bearer Testtoken");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                results = await response.Content.ReadAsStringAsync();
            }
            return results;
        }

        public async Task<string> GetZone()
        {
            string results = "";
            var request = new HttpRequestMessage(HttpMethod.Get,
            "https://test.com");
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorizationg", "Bearer Testtoken");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                results = await response.Content.ReadAsStringAsync();
            }
            return results;
        }
    }
}
