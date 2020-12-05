# Internet Speedtest Vs Weather Dashboard

Inspired by/based on [Internet Speedtest Dashboard](https://gitlab.com/phikai/docker-internet-speedtest-dashboard)

This is a slightly different take on the speedtest dashboard concept, specifically designed to log data about when my internet goes down.
I live in a rural area and like many in the UK the broadband infrastructure is lacking. In my case it's fiber to the cabinet, but then ~2 miles of copper phoneline, going through trees and across fields.

The addition here is pulling in weather data from uk.gov, initially rainfall data to detect when there's a storm.

# Usage
Basically the same as any docker-compose based project
```
git clone <repo>
# create a .env file containing
# - The weather api polling rate
# - latitude/longitude of the monitoring point
# - monitoring radius in km - All (ignoring errors) rainfall stations in this area will be reported)
WEATHER_POLLING_INTERVAL=10
WEATHER_LATITUDE=51.0899
WEATHER_LONGITUDE=-0.4547
WEATHER_RADIUSKM=30

docker-compose up -d
```

### Credits
* [Pedro Azevedo](https://github.com/pedrocesar-ti)
* [InfluxDB](https://www.influxdata.com/) 
* [Chronograf](https://www.influxdata.com/time-series-platform/chronograf/)
* [SpeedTest](https://github.com/sivel/speedtest-cli/)

