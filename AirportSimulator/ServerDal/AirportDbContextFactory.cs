using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerDal
{
    internal class AirportDbContextFactory : IDesignTimeDbContextFactory<AirportDbContext>
    {
        public AirportDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AirportDbContext>();
            optionsBuilder.UseSqlServer("Server=(LocalDb)\\MSSQLLocalDB;Database=AirportDatabaseVTwo;Trusted_Connection=True;");
            return new AirportDbContext(optionsBuilder.Options);
        }
    }
}
