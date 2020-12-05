using System;
using System.Threading;

namespace WeatherFetcher
{
  class Program
  {
    static void Main(string[] args)
    {
      // TODO: Some startup/validation stuff goes here

      // Polling interval (seconds)
      var pollingInterval = 10;

      // Start processing, and run until killed
      while(true)
      {
        update();
        Thread.Sleep(pollingInterval * 1000);
      }
    }

    static void update()
    {
      Console.WriteLine("Hello, this is an update from your friendly local weatherman");
    }
  }
}
