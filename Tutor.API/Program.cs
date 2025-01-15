using MajaDum.Application.Configuration;
using Microsoft.EntityFrameworkCore;
using Tutor.API.Middlewares;
using Tutor.Application.Common.Interfaces;
using Tutor.Infrastructure.Configuration;
using Tutor.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// App context switches
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

// Configure services
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationService();
builder.Services.AddInfrastructureService(builder.Configuration);
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
var app = builder.Build();
// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseMiddleware<UnitOfWorkMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.Run();
