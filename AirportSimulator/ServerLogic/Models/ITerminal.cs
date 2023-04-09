using AirportSimCore.Models;
using ServerLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSimCore.Interfaces
{
    public interface ITerminal
    {
        public Task AddFlight(Flight flight);

        public List<TerminalLeg> Legs { get; }

    }
}
