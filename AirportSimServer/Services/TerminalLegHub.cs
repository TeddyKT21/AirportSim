using AirportSimCore.Interfaces;
using AirportSimCore.Models;
using AirportSimCore.Models.Dtos;
using Microsoft.AspNetCore.SignalR;
using ServerLogic.Models;
using System.Runtime.CompilerServices;

namespace AirportSimServer.Services

{
    public class TerminalLegHub : Hub, ITerminalLegHub
    {
        private readonly IHubContext<TerminalLegHub> _hubContext;
        public TerminalLegHub(IHubContext<TerminalLegHub> hubContext) => _hubContext = hubContext;
        public async Task SendLeavingMessege(int legNumber, Flight flight) => await _hubContext.Clients.All.SendAsync("SectionUpdate", new { legNumber, flight, IsEntering = false });
        public async Task SendEnteringMessege(int legNumber, Flight flight) => await _hubContext.Clients.All.SendAsync("SectionUpdate", new { legNumber, flight, IsEntering = true });
        public async Task SendPendingStatus(int legNumber) => await _hubContext.Clients.All.SendAsync("SectionPendingUpdate", legNumber);
    }
}
