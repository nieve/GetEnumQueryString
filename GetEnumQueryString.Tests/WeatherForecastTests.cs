using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alba;
using GetEnumQueryString.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;

namespace GetEnumQueryString.Tests
{
    public class WeatherForecastTests
    {
        protected static SystemUnderTest System;

        [SetUp]
        public void Setup()
        {
            var hostBuilder = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
            System = new SystemUnderTest(hostBuilder);
        }

        [Test]
        public async Task ShouldReturnsCustomErrorOnPost()
        {
            var scenarioResult = await System.Scenario(scenario =>
            {
                scenario.Post.Json(new Model{Color = (Colors) (-1)}).ToUrl($"/WeatherForecast");
                scenario.StatusCodeShouldBe(400);
            });

            var result = scenarioResult.ResponseBody.ReadAsJson<HttpErrorResult>();
            Assert.That(result.Errors.ContainsKey("Color"));
            Assert.AreEqual(1, result.Errors.Values.SelectMany(x => x).Count());
            Assert.AreEqual("INVALID_COLOR", result.Errors.Values.First().First());
        }

        [Test]
        public async Task ShouldReturnsCustomErrorOnPostColor()
        {
            var scenarioResult = await System.Scenario(scenario =>
            {
                scenario.Post.Url($"/WeatherForecast/23?Color=-1");
                scenario.StatusCodeShouldBe(400);
            });

            var result = scenarioResult.ResponseBody.ReadAsJson<HttpErrorResult>();
            Assert.That(result.Errors.ContainsKey("Color"));
            Assert.AreEqual(1, result.Errors.Values.SelectMany(x => x).Count());
            Assert.AreEqual("INVALID_COLOR", result.Errors.Values.First().First());
        }

        [Test]
        public async Task ShouldReturnsCustomErrorOnGet()
        {
            var scenarioResult = await System.Scenario(scenario =>
            {
                scenario.Get.Url($"/WeatherForecast?Color=-1");
                scenario.StatusCodeShouldBe(400);
            });

            var result = scenarioResult.ResponseBody.ReadAsJson<HttpErrorResult>();
            Assert.That(result.Errors.ContainsKey("Color"));
            Assert.AreEqual(1, result.Errors.Values.SelectMany(x=>x).Count());
            Assert.AreEqual("INVALID_COLOR", result.Errors.Values.First().First());
        }
    }
}