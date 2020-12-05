using System;
using System.Threading;
using WeatherFetcher.Services;

namespace WeatherFetcher
{
  class Program
  {
    static int Main(string[] args)
    {
      // TODO: Need to read parameters from environment vars, passed into container

      // Polling interval (seconds)

      var pollingIntervalEnv = Environment.GetEnvironmentVariable("POLLING_INTERVAL");
      var latEnv = Environment.GetEnvironmentVariable("LATITUDE");
      var lonEnv = Environment.GetEnvironmentVariable("LONGITUDE");
      var distEnv = Environment.GetEnvironmentVariable("DISTANCE");
      if( string.IsNullOrEmpty(pollingIntervalEnv ) ||
          string.IsNullOrEmpty(latEnv ) ||
          string.IsNullOrEmpty(lonEnv ) ||
          string.IsNullOrEmpty(distEnv ))
      {
        Console.WriteLine("Failed to read environment vars");
        return 1;
      }

      var pollingInterval = 60;
      int.TryParse(pollingIntervalEnv, out pollingInterval);
      var lat = 0.0;
      double.TryParse(latEnv, out lat);
      var lon = 0.0;
      double.TryParse(lonEnv, out lon);
      var dist = 30;
      int.TryParse(distEnv, out dist);

      var api = new EnvAgencyRainfall();
      // Read list of stations once on startup, assume we'll be restarted whenever params change, or to pull in new stations
      var stations = api.GetStations(lat, lon, dist);
      if( stations == null )
      {
        Console.WriteLine("Failed to acquire any stations, try increasing the search radius?");
        return 1;
      }

      // Start processing, and run until killed
      // TODO: Just polling the api like this isn't the best approach, as it can give us a list of measurements since X time
      // TODO: But it's a quick way to get things going. Best to poll once a minute or 3, looks like all the local stations
      // to me provide mm rainfall per 15-minute intervals so no need to spam too much
      while (true)
      {
        update(api, stations);
        Thread.Sleep(pollingInterval * 1000);
      }
    }

    static void update(EnvAgencyRainfall api, EnvAgencyRainfall.GetStationsData stations)
    {
      foreach (var s in stations.items)
      {
        if (s.measures == null || s.measures.Count == 0) continue;
        var rainfallMeasure = s.measures.Find(x => x.parameter.Equals("rainfall"));
        if (rainfallMeasure == null) continue;

        var measurement = api.GetLatestMeasurement(s.stationReference);
        if( measurement == null ) {
          // TODO: One instance of this is a rainfall sensor returning value: [0, 0.4], as array instead of single value
          // There doesn't appear to be anything in the api to signify this, will need to read the docs to tell if it's
          // an actual error. No big deal really just that sensor won't be read
          continue;
        }
        if (measurement.items != null && measurement.items.Count > 0)
        {
          foreach (var i in measurement.items)
          {
            if (!i.parameter.Equals("rainfall")) continue;

            var absReading = i.latestReading.value;
            var mmPerPeriod = absReading;
            if( !i.unit.Equals("http://qudt.org/1.1/vocab/unit#Millimeter"))
            {
              // Again TODO - Ignore for now as most sensors seem to be mm based.
              // Should just scale up and we'd be good
              continue;
            }
            var mmPerHour = mmPerPeriod * (3600.0 / i.period);

            Console.WriteLine($"{s.stationReference} : {s.lat},{s.lon} : {mmPerHour}mm/h");
          }
        }
      }

    }
  }
}
