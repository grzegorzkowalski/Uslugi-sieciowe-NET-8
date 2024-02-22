using Postcard.WebAPI_SignalR.Repository;
using Postcard.WebAPI_SignalR.SignalR;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
    .ReadFrom.Configuration(hostingContext.Configuration));

builder.Services.AddScoped<gRPCRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
        builder.WithOrigins("https://localhost:5555") // Specify the allowed origin(s)
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials()); // Necessary for SignalR
});

builder.Services.AddGrpcClient<Postcard.WebAPI_SignalR.Postcard.PostcardClient>(o =>
{
    o.Address = new Uri("https://localhost:7111"); // Adres us³ugi gRPC
});

builder.Services.AddSignalR();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.MapHub<PostCardHub>("/postcardHub");

app.UseAuthorization();

app.MapControllers();

app.Run();
