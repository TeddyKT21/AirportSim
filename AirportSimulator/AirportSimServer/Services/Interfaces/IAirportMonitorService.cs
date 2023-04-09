using AirportSimCore.Models.Dtos;

namespace AirportSimServer.Services.Interfaces
{
    public interface IAirportMonitorService
    {
        List<TerminalLegDto> GetTerminalStatus();
    }
}
