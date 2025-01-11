using AspNetCoreRateLimit;
using HackerNews.API.ControllerActionsExecutors;
using HackerNews.BusinessLogic.DI;
using HackerNews.Infrastructure.AutoMapper;
using HackerNews.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddConfigurationSections(builder.Configuration);
builder.Services.AddBusinessLogicServices();
builder.Services.AddAutoMapper();
builder.Services.AddScoped<ControllerActionsExecutor>();
builder.Services.AddRateLimiting(builder.Configuration);

builder.Services.AddMemoryCache();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseIpRateLimiting();

app.MapControllers();

app.Run();
