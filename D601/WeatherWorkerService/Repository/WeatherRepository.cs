using System.Data.Common;
using WeatherWorkerService.Data;
using WeatherWorkerService.Models;

namespace WeatherWorkerService.Repository
{
    public class WeatherRepository
    {
        public async void AddAsync(OpenWeather? weather)
        {
            using (var context = new ApplicationDbContext())
            {
                await context.Weathers.AddAsync(weather);
                await context.SaveChangesAsync();
            }

        }
    }
}
