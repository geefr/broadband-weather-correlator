using System;
using Xunit;

using WeatherFetcher.Services;

namespace WeatherFetcher.Tests
{
    public class EnvAgencyRainfallTests
    {
        [Fact]
        public void TestListStations()
        {
            var api = new EnvAgencyRainfall();
            var stations = api.GetStations();
            Assert.NotNull(stations);
        }
    }
}
