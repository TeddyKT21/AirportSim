using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirportSimCore.Interfaces;
namespace AirportSimCore.Models.Dtos
{
    public class LegLogDto : IEntity
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Text { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual Flight Flight { get; set; }

        public virtual TerminalLegDto Leg { get; set; }
    }
}
