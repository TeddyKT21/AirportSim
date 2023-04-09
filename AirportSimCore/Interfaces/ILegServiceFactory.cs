using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSimCore.Interfaces
{
    public interface ILegServiceFactory
    {
        ITerminalLegHub GetSignalR();

        IFlightLogCreator GetFlightLogCreator();

        ILogger<T> GetLogger<T>();
    }
}
