using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using DotNetEnv;
using Newtonsoft.Json;


namespace lancer_resources_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResourcesController : ControllerBase
    {
        private readonly ILogger<ResourcesController> _logger;
        private readonly string _googleApiKey;

        public ResourcesController(ILogger<ResourcesController> logger)
        {
            Env.Load();

            _logger = logger;
            _googleApiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");
            
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var googleSheetResponse = new GoogleSheetResponse();
            var endpoint = @$"https://sheets.googleapis.com/v4/spreadsheets/1KUnYHyVZy-TAU1FbaSpyY6LJPJV2Q_HUGtTEs0RDyUU/values/A:F?key=" + _googleApiKey;
            using(var client = new HttpClient())
            {
                var clientResponse = await client.GetAsync(endpoint);
                var content = await clientResponse.Content.ReadAsStringAsync();
                googleSheetResponse = JsonConvert.DeserializeObject<GoogleSheetResponse>(content);
            }

            //Remove Headers from speadsheet
            googleSheetResponse.Values.RemoveAt(0);
            var response = new List<Resource>();

            foreach (var row in googleSheetResponse.Values)
            {
                var resource = new Resource
                {
                    Title = row[0],
                    Author = row[1],
                    URL = row[2],
                    Description = row[3],
                    Image = row[4],
                    Tags = row[5].Split(",").Select(x => x.Trim()).ToList()
                };

                response.Add(resource);
            }

            return Ok(response);
        }

        [HttpGet("tags")]
        public async Task<IActionResult> Tags()
        {
            var googleSheetResponse = new GoogleSheetResponse();
            var endpoint = @$"https://sheets.googleapis.com/v4/spreadsheets/1KUnYHyVZy-TAU1FbaSpyY6LJPJV2Q_HUGtTEs0RDyUU/values/F:F?key=" + _googleApiKey;
            using (var client = new HttpClient())
            {
                var clientResponse = await client.GetAsync(endpoint);
                var content = await clientResponse.Content.ReadAsStringAsync();
                googleSheetResponse = JsonConvert.DeserializeObject<GoogleSheetResponse>(content);
            }

            //Remove Headers from speadsheet
            googleSheetResponse.Values.RemoveAt(0);
            var response = new HashSet<string>();

            foreach (var row in googleSheetResponse.Values)
            {
                var tags = row[0].Split(",").Select(x => x.Trim()).ToList();
                foreach (var tag in tags)
                {
                    response.Add(tag);
                }
            }
            return Ok(response);
        }
    }
}
