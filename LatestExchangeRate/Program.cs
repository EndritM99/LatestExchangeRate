using Hangfire;
using Hangfire.SqlServer;
using LatestExchangeRate.Context;
using LatestExchangeRate.Interfaces;
using LatestExchangeRate.Models;
using LatestExchangeRate.Services;
using LatestExchangeRate.Validators;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Hangfire services.
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true
    }));

builder.Services.Configure<RabbitMqConfiguration>(a => builder.Configuration.GetSection(nameof(RabbitMqConfiguration)).Bind(a));

// Add the processing server as IHostedService
builder.Services.AddHangfireServer();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add dependencies here
builder.Services.AddScoped<IJobScheduler, JobScheduler>();
builder.Services.AddScoped<IExchangeRate, FixerService>();
builder.Services.AddScoped<IDocumentProcessing, DocumentProcessingService>();
//builder.Services.AddScoped<IRabbitMq, RabbitMqService>();
builder.Services.AddSingleton(new ConnectionFactory() { HostName = "localhost" });
builder.Services.AddScoped<JobValidator>();
builder.Services.AddSingleton<IRabbitMqService, RabbitMqService>();
builder.Services.AddSingleton<IConsumerService, ConsumerService>();
builder.Services.AddSingleton<IResponsePublisher, ResponsePublisher>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseHangfireDashboard();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHangfireDashboard();
});

app.MapControllers();

app.Run();