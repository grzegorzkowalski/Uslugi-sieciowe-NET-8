using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherWorkerService.Models;

namespace WeatherWorkerService.Logic
{
    public class WeatherMapper
    {
        public WeatherModel GetWeatherData(WeatherData weatherData, string city)
        {
            return new WeatherModel()
            {
                City = city,
                WeatherDate = DateTime.Now,
                Temp = weatherData.Main.Temp,
                FeelsLike = weatherData.Main.FeelsLike,
                TempMin = weatherData.Main.TempMin,
                TempMax = weatherData.Main.TempMax,
                Pressure = weatherData.Main.Pressure,
                Humidity = weatherData.Main.Humidity
            };
        }
    }
}
