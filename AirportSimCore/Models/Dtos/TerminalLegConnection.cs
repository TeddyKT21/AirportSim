using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSimCore.Models.Dtos
{
    public class TerminalLegConnection
    {

        public int TerminalLegStartId { get; set; }
        public int TerminalLegContinueId { get; set; }
        public TerminalLegDto? TerminalLegStart { get; set; }
        public TerminalLegDto? TerminalLegContinue { get; set; }

        public bool IsDepartingConnection { get; set; }
    }
}
