using AirportSimCore.Interfaces;
using AirportSimCore.Models.Dtos;
using AirportSimServer.Services.Interfaces;

namespace AirportSimServer.Services
{
    public class AirportMonitorService : IAirportMonitorService
    {
        private readonly ITerminal _terminal;

        public AirportMonitorService(ILogger<AirportMonitorService> logger, ITerminal terminal)
        {
            _terminal = terminal;
        }
        public List<TerminalLegDto> GetTerminalStatus()
        {
            var legs = new List<TerminalLegDto>();
            foreach(var logicLeg in _terminal.Legs)
            {
                var dtoLeg = new TerminalLegDto()
                {
                    CurrentFlight = logicLeg.CurrentFlight,
                    LegNumber = logicLeg.LegNumber,
                    Duration = logicLeg.Duration,
                    IsOperating = logicLeg.IsOperating
                };
                legs.Add(dtoLeg);
            }
            return legs;
        }
    }
}
