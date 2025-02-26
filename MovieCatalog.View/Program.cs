using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Net.Sockets;
using MovieCatalog.Infrastructure;
using MovieCatalog.View.Handlers;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Prometheus;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Grafana.Loki;
using ExportProcessorType = OpenTelemetry.ExportProcessorType;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService("MyAspNetApp"))
    .WithMetrics(metrics =>
    {
        metrics.AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation()
            .AddPrometheusExporter();
    })
    .WithTracing(tracing =>
    {
        tracing.AddSource("MyAspNetApp")
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddSqlClientInstrumentation()
            .AddOtlpExporter(options =>
            {
                options.Protocol = OtlpExportProtocol.Grpc;
                options.Endpoint = new Uri("http://otel-collector:4317");
            });
    });

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

//app.UseSerilogRequestLogging();
// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");// Открыть Swagger на корне
});

/*app.UseHttpMetrics();
app.UseMetricServer();*/

app.MapControllers();
//app.MapMetrics("/metrics");

app.MapPrometheusScrapingEndpoint();

app.Run();