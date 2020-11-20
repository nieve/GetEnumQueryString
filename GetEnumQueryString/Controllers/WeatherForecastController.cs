using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GetEnumQueryString.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly List<string> Summaries = new List<string>
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get([FromQuery]Model @params)
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = $"{Summaries[rng.Next(Summaries.Count)]}-{@params.Color}"
            })
            .ToArray();
        }

        [HttpPost]
        public IActionResult Post([FromBody]Model body)
        {
            if (!Summaries.Contains(body.Color.ToString())) Summaries.Add(body.Color.ToString());
            return Ok();
        }

        [HttpPost("{id}")]
        public IActionResult PostColor([FromRoute]int id, [FromQuery]Model body)
        {
            if (!Summaries.Contains(body.Color.ToString())) Summaries.Add(body.Color.ToString());
            return Ok();
        }
    }

    public class Model
    {
        [Required(ErrorMessage = "COLOR_MISSING"), EnumDataType(typeof(Colors), ErrorMessage = "INVALID_COLOR")]
        public Colors Color { get; set; }
    }

    public enum Colors
    {
        Red, Blue, Yellow
    }

    public class HttpErrorResult
    {
        public string Title { get; set; }
        public HttpStatusCode Status { get; set; }
        public Dictionary<string, List<string>> Errors { get; set; }
    }
}
