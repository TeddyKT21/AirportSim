using AirportSimCore.Models;
using AirportSimCore.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSimCore.Interfaces
{
    public interface ITerminalLegHub
    {
        public Task SendLeavingMessege(int legNumber, Flight flight);
        public Task SendEnteringMessege(int legNumber, Flight flight);

        public Task SendPendingStatus(int legNumber);
    }
}
