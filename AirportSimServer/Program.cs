
using AirportSimCore.Interfaces;
using AirportSimServer.Services;
using AirportSimServer.Services.Interfaces;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Web;
using ServerDal.Repositories;
using ServerLogic.Models;
using System;

namespace AirportSimServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
            var builder = WebApplication.CreateBuilder(args);
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();
            builder.Services.AddSignalR();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });
            builder.Services.AddScoped<ITerminalLegHub,TerminalLegHub>();
            builder.Services.AddScoped<IAirportMonitorService, AirportMonitorService>();
            builder.Services.AddScoped<IAirportRepository, AirportRepository>(serviceProvider => new AirportRepository(builder.Configuration.GetConnectionString("AirportDatabase")));
            builder.Services.AddScoped<IFlightLogCreator, FlightLogCreator>();
            builder.Services.AddSingleton<ILegServiceFactory, LegServiceFactory>();
            builder.Services.AddSingleton<ITerminal,Terminal>(serviceProvider =>
            {
                {
                    using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
                    var repository = scope.ServiceProvider.GetRequiredService<IAirportRepository>();
                    var serviceFactory = serviceProvider.GetRequiredService<ILegServiceFactory>();

                    return new Terminal(serviceFactory, repository);
                }
            });
            var app = builder.Build();
            app.UseCors("AllowAll");
            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}
            app.MapHub<TerminalLegHub>("/StamHub");
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}