using AirportSimCore.Models;
using AirportSimCore.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerDal
{
    internal class AirportDbContext : DbContext
    {

        public virtual DbSet<Flight> Flights { get; set; }
        public virtual DbSet<LegLogDto> LegLogs  { get; set; }
        public virtual DbSet<TerminalLegDto> Legs { get; set; }


        public AirportDbContext(DbContextOptions<AirportDbContext> options) : base(options) { }

        public AirportDbContext(string connectionString) : base(GetOptions(connectionString)) { }

        private static DbContextOptions<AirportDbContext> GetOptions(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AirportDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return optionsBuilder.Options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TerminalLegConnection>()
                .HasKey(c => new { c.TerminalLegStartId, c.TerminalLegContinueId });

            modelBuilder.Entity<TerminalLegConnection>()
            .HasOne(c => c.TerminalLegStart)
            .WithMany(l => l.NextLegConnections)
            .HasForeignKey(C => C.TerminalLegStartId);

            modelBuilder.Entity<TerminalLegConnection>()
            .HasOne(c => c.TerminalLegContinue)
            .WithMany()
            .HasForeignKey(C => C.TerminalLegContinueId);

            modelBuilder.Entity<TerminalLegConnection>()
            .HasIndex(c => new { c.TerminalLegStartId, c.TerminalLegContinueId, c.IsDepartingConnection })
            .IsUnique();
        }
    }
}
