using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherWorkerService.Models
{
    public class OpenWeather
    {
        public int Id { get; set; }
        public double Temp { get; set; }
        public double FeelsLike { get; set; }
        public double TempMin { get; set; }
        public double TempMax { get; set; }
        public int Pressure { get; set; }
        public int Humidity { get; set; }
    }
}
