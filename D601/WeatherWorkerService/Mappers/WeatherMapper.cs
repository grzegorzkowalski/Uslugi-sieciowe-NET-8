using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherWorkerService.Models;

namespace WeatherWorkerService.Mappers
{
    internal class WeatherMapper
    {
        public static OpenWeather MapWeather(WeatherData weatherData)
        {
            return new OpenWeather
            {
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
