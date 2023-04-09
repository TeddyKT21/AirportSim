using AirportSimCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSimCore.Models.Dtos
{
    public class TerminalLegDto : IEntity
    {
        public int Id { get; set; }
        public int LegNumber { get; set; }
        public int Duration { get; set; }
        public bool IsArrivingStart { get; set; }
        public bool IsDepartingStart { get; set; }
        [NotMapped]
        public Flight? CurrentFlight { get; set; }
        [NotMapped]
        public bool IsOperating { get; set; }
        public virtual ICollection<TerminalLegConnection>? NextLegConnections { get; set; }

    }
}


