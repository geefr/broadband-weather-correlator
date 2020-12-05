using System;
using Xunit;

using WeatherFetcher.Services;

namespace WeatherFetcher.Tests
{
    public class EnvAgencyRainfallTests
    {
        [Fact]
        public void TestReadStationValues()
        {
            var api = new EnvAgencyRainfall();
            var stations = api.GetStations(51.0899, -0.4547, 20);
            Assert.NotNull(stations);

            foreach( var s in stations.items )
            {
                if( s.measures == null || s.measures.Count == 0 ) continue;
                var rainfallMeasure = s.measures.Find(x => x.parameter.Equals("rainfall") );
                if( rainfallMeasure == null ) continue;

                var measurement = api.GetLatestMeasurement(s.stationReference);
                if( measurement.items != null && measurement.items.Count > 0 )
                {
                    foreach( var i in measurement.items )
                    {
                        if( !i.parameter.Equals("rainfall") ) continue;
                        Console.WriteLine($"{s.stationReference} : {s.lat},{s.lon} : {i.latestReading.value}{i.unitName}");
                    }
                }
            }
        }
    }
}
