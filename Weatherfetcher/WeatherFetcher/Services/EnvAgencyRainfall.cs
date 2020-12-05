using System.Collections.Generic;
using RestSharp;

namespace WeatherFetcher.Services
{
  /// <summary>
  /// Client for Environment agency's rainfall api
  /// https://environment.data.gov.uk/flood-monitoring/doc/rainfall
  /// </summary>
  public class EnvAgencyRainfall
  {
    private const string APIRoot = "http://environment.data.gov.uk/flood-monitoring";
    private const string APIRainfallStations = "stations?parameter=rainfall";

    private RestClient Client = null;

    public class Station
    {
      
    }

    public EnvAgencyRainfall()
    {
      Client = new RestClient(APIRoot);
    }

    public List<Station> GetStations()
    {
      var req = new RestRequest(APIRainfallStations, DataFormat.Json);
      var res = Client.Get<List<Station>>(req);
      return res.Data;
    }

  }
}