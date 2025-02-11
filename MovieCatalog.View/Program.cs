using System.Diagnostics.Metrics;
using MovieCatalog.Infrastructure;
using MovieCatalog.View.Handlers;
using Prometheus;
using Serilog;
using Serilog.Sinks.Grafana.Loki;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.GrafanaLoki("http://host.docker.internal:3100")
    .CreateLogger();

builder.Logging.AddSerilog().SetMinimumLevel(LogLevel.Information);


builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(new Manager());
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");// Открыть Swagger на корне
});


app.UseHttpMetrics();
app.UseMetricServer();

app.MapControllers();
app.MapMetrics("/metrics");

app.Run();