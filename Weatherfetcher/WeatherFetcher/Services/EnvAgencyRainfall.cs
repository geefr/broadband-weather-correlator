using System.Collections.Generic;
using RestSharp;
using RestSharp.Serializers.SystemTextJson;
using System.Text.Json.Serialization;
using System;

namespace WeatherFetcher.Services
{
  /// <summary>
  /// Client for Environment agency's rainfall api
  /// https://environment.data.gov.uk/flood-monitoring/doc/rainfall
  /// </summary>
  public class EnvAgencyRainfall
  {
    private const string APIRoot = "http://environment.data.gov.uk/flood-monitoring";
    private RestClient Client = null;

    public EnvAgencyRainfall()
    {
      Client = new RestClient(APIRoot);
      // Using System.Text.Json here to support property renaming (The rest api uses 'long' as an identifier)
      Client.UseSystemTextJson();
    }

    /// <summary>
    /// List rainfall stations within distance(km) of lat,lon(wgs84) 
    /// </summary>
    public GetStationsData GetStations(double lat, double lon, int distance)
    {
      var req = new RestRequest($"id/stations?parameter=rainfall&lat={lat}&long={lon}&dist={distance}", DataFormat.Json);
      var res = Client.Get<GetStationsData>(req);
      if (!res.IsSuccessful) return null;
      return res.Data;
    }

    /// <summary>
    /// Read the latest measurement for a station
    /// </summary>
    /// <param name="stationReference">Station reference string, from GetStationData.Item.stationReference</param>
    public GetLatestMeasurementData GetLatestMeasurement(string stationReference)
    {
      var req = new RestRequest($"id/stations/{stationReference}/measures?parameter=rainfall", DataFormat.Json);
      var res = Client.Get<GetLatestMeasurementData>(req);
      if (!res.IsSuccessful) 
      {
        //Console.WriteLine($"Request failed: {res.ErrorMessage}");
        //Console.WriteLine($"{res.Content}");
        return null;
      }
      return res.Data;
    }


    public class StationMetaData
    {
      public string publisher { get; set; }
      public string licence { get; set; }
      public string documentation { get; set; }
      public string version { get; set; }
      public string comment { get; set; }
      public List<string> hasFormat { get; set; }
    }

    public class GetStationsData
    {
      public StationMetaData meta { get; set; }

      public class Item
      {
        public double easting { get; set; }
        public double northing { get; set; }
        public string gridReference { get; set; }
        public string label { get; set; }
        public double lat { get; set; }

        [JsonPropertyName("long")]
        public double lon { get; set; }

        public class Measure
        {
          public string parameter { get; set; }
          public string parameterName { get; set; }
          public double period { get; set; }
          public string qualifier { get; set; }
          public string unitName { get; set; } // mm, cm, m?
        }
        public List<Measure> measures {get; set;}
        public string notation { get; set; }
        public string stationReference { get; set; }
      }
      public List<Item> items { get; set; }
    }
  
    public class GetLatestMeasurementData
    {
      public StationMetaData meta {get;set;}

      public class Item
      {
        public string label {get;set;}
        public class Reading
        {
          public string date {get;set;}
          public string dateTime {get;set;}
          public string measure {get;set;}
          public double value {get;set;}
        }
        public Reading latestReading {get;set;}
        public string notation {get;set;}
        public string parameter {get;set;}
        public string parameterName {get;set;}
        public double period{get;set;}
        public string qualifier{get;set;}
        public string station{get;set;}
        public string stationReference{get;set;}
        public string unit{get;set;}
        public string unitName {get;set;}
        public string valueType {get;set;}
      }
      public List<Item> items {get;set;}
    }
  }
}