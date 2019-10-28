﻿using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        public async Task<string> GetSocrata()
        {
            string results = "";
            string token = _options.Value.SocrataClientId + ":" + _options.Value.SocrataClientSecret;
            var encodedToken = Encoding.UTF8.GetBytes(token);
            token = Convert.ToBase64String(encodedToken);

            var request = new HttpRequestMessage(HttpMethod.Get,
            $"{ _options.Value.SocrataEndpoint }");
            //"$where=within_circle(the_geom, -85.67, 42.97, 1000)");
            //request.Headers.Add("Host", "data.grandrapidsmi.gov");
            request.Headers.Add("Accept", "application/json");
            //request.Headers.Add("Content-Length", "253");
            //request.Headers.Add("Content-Type", "application/json");
            request.Headers.Add("X-App-Token", _options.Value.SocrataAdminKey);
            request.Headers.Add("Authorization", $"Basic {token}");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                results = await response.Content.ReadAsStringAsync();
            }
            var parkingData = JsonConvert.DeserializeObject<JArray>(results);
            return results;
        }
    }
}