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
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ParkingLocator.Core.Concretes
{
    public class ParkingService : IParkingService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IOptions<ParkingKeyOptions> _options;

        public static List<List<Space>> finalSpaces;
        public ParkingService(IHttpClientFactory clientFactory, IOptions<ParkingKeyOptions> options)
        {
            _clientFactory = clientFactory;
            _options = options;
        }

        public async Task<List<ActiveRootObject>> GetSocrataActiveSession()
        {
            string results = "";
            string token = _options.Value.SocrataClientId + ":" + _options.Value.SocrataClientSecret;
            var encodedToken = Encoding.UTF8.GetBytes(token);
            token = Convert.ToBase64String(encodedToken);

            var request = new HttpRequestMessage(HttpMethod.Get,
            $"{ _options.Value.SocrataEndpoint }?$select=space_number, data_source, session_end&$limit=5000");

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
            var parkingData = JsonConvert.DeserializeObject<List<ActiveRootObject>>(results);
            return parkingData;
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
                    MeterId = x.meter_id,
                    SpaceId = x.space_id,
                    BoundingBox = x.the_geom.coordinates.First().First(),
                    OperationalDays = x.operational_days,
                    OperationalHours = x.operational_hours
                });
            });
            return masterSpaces;
        }

        public async Task UpdateMap()
        {
            var cnt = 0;
            // Three Lists. Green, Red, Grey Spots.
            List<List<Space>> mapLayoutPackage = new List<List<Space>>();
            List<Space> redSpaces =  new List<Space>();
            List<Space> greySpaces = new List<Space>();
            List<Space> greenSpaces = new List<Space>();
            List<Space> masterList = new List<Space>();
            List<ActiveRootObject> activeList = new List<ActiveRootObject>();
            List<int> checkedSpacesList = new List<int>();
            while (true)
            {
                try
                {
                    if (redSpaces != null)
                    {
                        if (redSpaces.Any())
                        {
                            redSpaces.Clear();
                        }
                    }
                    if (greenSpaces != null)
                    {
                        if (greenSpaces.Any())
                        {
                            greenSpaces.Clear();
                        }
                    }
                    if (greySpaces != null)
                    {
                        if (greySpaces.Any())
                        {
                            greySpaces.Clear();
                        }
                    }
                    masterList = await GetSocrataMasterList();
                    activeList = await GetSocrataActiveSession();
                    activeList.ForEach(x =>
                    {
                        if (x.data_source == "MOTU")
                        {
                            Space space = masterList.FirstOrDefault(y => y.SpaceId == x.space_number);
                            if (space != null)
                            {
                                if (IsSpaceEnforceable(space.OperationalHours))
                                {
                                    if (IsSpaceExpired(x.session_end) == true)
                                    {
                                        redSpaces.Add(space);
                                        checkedSpacesList.Add(space.ObjectId);
                                    }
                                    else
                                    {
                                        greenSpaces.Add(space);
                                        checkedSpacesList.Add(space.ObjectId);
                                    }
                                }
                            }
                        }
                        else if (x.data_source == "Flowbird")
                        {
                            Space space = masterList.FirstOrDefault(y => y.MeterId == x.space_number);
                            if (space != null)
                            {
                                if (IsSpaceEnforceable(space.OperationalHours))
                                {
                                    if (IsSpaceExpired(x.session_end) == true)
                                    {
                                        redSpaces.Add(space);
                                        checkedSpacesList.Add(space.ObjectId);
                                    }
                                    else
                                    {
                                        greenSpaces.Add(space);
                                        checkedSpacesList.Add(space.ObjectId);
                                    }
                                }
                            }
                        }
                    });
                    var unAddedSpots = masterList.Where(x => !checkedSpacesList.Any(y => x.ObjectId == y));
                    foreach (var item in unAddedSpots)
                    {
                        if (item.OperationalHours != null)
                        {
                            if (IsSpaceEnforceable(item.OperationalHours))
                            {
                                greenSpaces.Add(item);
                            }
                            else
                            {
                                greySpaces.Add(item);
                            }
                        }
                        else
                        {
                            greenSpaces.Add(item);
                        }

                    }
                    if (finalSpaces != null)
                    {
                        if (finalSpaces.Any())
                        {
                            finalSpaces.Clear();
                        }
                    }
                    if (mapLayoutPackage != null)
                    {
                        if (mapLayoutPackage.Any())
                        {
                            mapLayoutPackage.Clear();
                        }
                    }
                    mapLayoutPackage.Add(greenSpaces);
                    mapLayoutPackage.Add(redSpaces);
                    mapLayoutPackage.Add(greySpaces);
                    finalSpaces = mapLayoutPackage;
                    Console.WriteLine($"{cnt}: ", ConsoleColor.Green);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Number of API calls: {cnt} Error: {ex.Message}", ConsoleColor.Red);
                }
                finally
                {
                    var ts = new TimeSpan(0, 0, 10, 0, 0);
                    Thread.Sleep(ts);
                }
                cnt++;
            }
        }

        public bool IsSpaceExpired(string time)
        {
            var currentTime = DateTime.Now;
            DateTime oDate = Convert.ToDateTime(time);
            var x = oDate.TimeOfDay;
            TimeSpan ts = oDate - currentTime;
            if (ts.Minutes < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsSpaceEnforceable(string time)
        {
            var currentTime = DateTime.Now.TimeOfDay;
            if (time != null && time != "<Null>")
            {
                if (time != "24 hour")
                {
                    if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday || DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
                    {
                        var endEnforcement = TimeSpan.FromHours(Convert.ToInt32(time.Substring(7, 1)) + 12);
                        var startEnforcement = TimeSpan.FromHours(Convert.ToInt32(time.Substring(0, 1)));
                        if (currentTime > startEnforcement && currentTime < endEnforcement)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }  
        }

        public async Task<List<List<Space>>> GetFinalSpaces()
        {
            return await Task.FromResult(finalSpaces);
        }

        public async Task<string> GetEvents()
        {
            List<object> list = new List<object>();
            string results = "";

            var request = new HttpRequestMessage(HttpMethod.Get, _options.Value.EventsAPIEndpoint);

            var client = _clientFactory.CreateClient();

            string requestContentBase64String = string.Empty;

            string requestUri = HttpUtility.UrlEncode(request.RequestUri.AbsoluteUri.ToLower());

            string requestHttpMethod = request.Method.Method;

            //Calculate UNIX time
            DateTime epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = DateTime.UtcNow - epochStart;
            string requestTimeStamp = Convert.ToUInt64(timeSpan.TotalSeconds).ToString();

            //create random nonce for each request
            string nonce = Guid.NewGuid().ToString("N");

            //Checking if the request contains body, usually will be null with HTTP GET and DELETE
            if (request.Content != null)
            {
                byte[] content = Encoding.UTF8.GetBytes(await request.Content.ReadAsStringAsync());
                requestContentBase64String = Convert.ToBase64String(content);
            }

            //Creating the raw signature string
            string signatureRawData = String.Format("{0}{1}{2}{3}{4}{5}", _options.Value.EventsAppId, requestHttpMethod, requestUri, requestTimeStamp, nonce, requestContentBase64String);

            var secretKeyByteArray = Encoding.UTF8.GetBytes(_options.Value.EventsAPIKey);
            byte[] signature = Encoding.UTF8.GetBytes(signatureRawData);
            using (HMACSHA256 hmac = new HMACSHA256(secretKeyByteArray))
            {
                byte[] signatureBytes = hmac.ComputeHash(signature);
                string requestSignatureBase64String = Convert.ToBase64String(signatureBytes);
                request.Headers.Authorization = new AuthenticationHeaderValue("hmac", string.Format("{0}:{1}:{2}:{3}", _options.Value.EventsAppId, requestSignatureBase64String, nonce, requestTimeStamp));
            }
            //return request.CreateResponse();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                results = await response.Content.ReadAsStringAsync();
            }
            var res = JsonConvert.DeserializeObject<JObject>(results);
            var res1 = res.First.First.Children().ToList();
            res1.ForEach(x =>
            {
                var xx = x.ToArray()[13].First.First.First.ToArray()[3].First;
                list.Add(xx);
            });
            //res1.ForEach(x => x.First);
            Console.WriteLine(list);
            //var rest2 = res1[13];
            return results;
        }
    }
}
