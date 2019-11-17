using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ParkingLocator.Core.Entities;
using ParkingLocator.Core.Helpers;
using ParkingLocator.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ParkingLocator.Core.Concretes
{
    public class ParkingService : IParkingService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IOptions<ParkingKeyOptions> _options;
        public ParkingService(IHttpClientFactory clientFactory, IOptions<ParkingKeyOptions> options)
        {
            _clientFactory = clientFactory;
            _options = options;
        }

        public async Task<string> GetZoneListPassport()
        {
            string results = "";
            var request = new HttpRequestMessage(HttpMethod.Get,
            _options.Value.PassportEndpoint + $"getzonelist?apikey=${_options.Value.PassportStagingKey}");
            request.Headers.Add("Accept", "application/json");
            //request.Headers.Add("Authorizationg", "Bearer Testtoken");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                results = await response.Content.ReadAsStringAsync();

            }
            return results;
        }

        public async Task<string> GetZoneInfoPassport()
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

        public async Task<string> GetVeoci()
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

        public async Task<string> GetFlowbird()
        {
            string results = "";
            var request = new HttpRequestMessage(HttpMethod.Get,
            "https://google.com");
            request.Headers.Add("Accept", "application/json");
            //request.Headers.Add("Authorizationg", "Bearer Testtoken");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                results = await response.Content.ReadAsStringAsync();
            }
            return results;
        }

        public async Task<string> GetSocrataActiveSession()
        {
            string results = "";
            string token = _options.Value.SocrataClientId + ":" + _options.Value.SocrataClientSecret;
            var encodedToken = Encoding.UTF8.GetBytes(token);
            token = Convert.ToBase64String(encodedToken);

            var request = new HttpRequestMessage(HttpMethod.Get,
            $"{ _options.Value.SocrataEndpoint }");

            request.Headers.Add("X-App-Token", _options.Value.SocrataAdminKey);
            request.Headers.Add("Authorization", $"Basic {token}");
            request.Headers.Add("Host", "data.grandrapidsmi.gov");
            request.Headers.Add("Connection", "keep-alive");
            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                results = await response.Content.ReadAsStringAsync();
            }
            //var parkingData = JsonConvert.DeserializeObject<JArray>(results);
            return results;
        }

        public async Task<List<Space>> GetSocrataMasterList()
        {
            string results = "";
            List<Space> masterSpaces = new List<Space>();
            var request = new HttpRequestMessage(HttpMethod.Get,
            _options.Value.SocrataMasterEndpoint + "?$limit=5000");


            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                results = await response.Content.ReadAsStringAsync();
            }
            var parkingData = JsonConvert.DeserializeObject<List<RootObject>>(results);
            parkingData.ForEach(x => 
            {
                masterSpaces.Add(new Space()
                {
                    ObjectId = x.objectid,
                    MeterId = x.meterid,
                    SpaceId = x.spaceid,
                    BoundingBox = x.the_geom.coordinates.First().First()
                });
            });
            return masterSpaces;
        }
    }
}
