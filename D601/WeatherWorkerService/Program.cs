using Microsoft.EntityFrameworkCore;
using WeatherWorkerService;
using WeatherWorkerService.Data;
using WeatherWorkerService.Repository;

var builder = Host.CreateApplicationBuilder(args);

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<WeatherRepository>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
