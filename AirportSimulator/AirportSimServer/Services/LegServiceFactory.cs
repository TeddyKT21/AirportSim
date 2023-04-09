using AirportSimCore.Interfaces;
using Microsoft.Extensions.Configuration;
using ServerDal.Repositories;
using System.Reflection.Metadata.Ecma335;

namespace AirportSimServer.Services
{
    public class LegServiceFactory : ILegServiceFactory
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public LegServiceFactory(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;
       
        public IFlightLogCreator GetFlightLogCreator() => _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IFlightLogCreator>();

        public ILogger<T> GetLogger<T>() => _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ILogger<T>>();

        public ITerminalLegHub GetSignalR() => _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ITerminalLegHub>();
    }
}
