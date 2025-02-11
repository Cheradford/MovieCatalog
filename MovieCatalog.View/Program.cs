using MovieCatalog.Infrastructure;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(new Manager());
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");// Открыть Swagger на корне
});


/*app.UseHttpsRedirection();*/

app.UseAuthorization();
app.UseHttpMetrics();
app.MapControllers();
app.MapMetrics("/metrics");

app.Run();