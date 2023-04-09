using AirportSimCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSimCore.Models
{
    public class Flight : IEntity
    {
        public int Id { get; set; }

        public int FlightNumber { get; set; }

        public int NumberOfPassengers { get; set; }

        [MaxLength(50)]
        public string? PlaneModel { get; set; }
        [MaxLength(50)]
        public string? AirLine {get; set; }

        public bool IsDeparting { get; set; }

        public DateTime MadeContactAt { get; set;}

    }
}
