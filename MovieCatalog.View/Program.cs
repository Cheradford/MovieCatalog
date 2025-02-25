using System.Diagnostics.Metrics;
using MovieCatalog.Infrastructure;
using MovieCatalog.View.Handlers;
using Prometheus;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Grafana.Loki;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.GrafanaLoki("http://host.docker.internal:3100", new List<LokiLabel>(){
        new LokiLabel(){Key = "app",Value = "my-aspnet-app" }, // Теги для Loki
        new LokiLabel(){Key = "env",Value = "development" },
    })
    .MinimumLevel.Information()
    .CreateLogger();

builder.Host.UseSerilog();
//builder.Logging.AddSerilog().SetMinimumLevel(LogLevel.Information);
//builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(new Manager());
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

app.UseSerilogRequestLogging();
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