using AirportSimCore.Interfaces;
using AirportSimCore.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ServerDal.Repositories
{
    public class AirportRepository : IAirportRepository
    {
        private AirportDbContext _context;
        public AirportRepository(string connectionSting) 
        {
            _context = new AirportDbContext(connectionSting);
            _context.Database.EnsureCreated();
        }

        public void Add<T>(T obj) where T : class, IEntity
        {
            _context.Add(obj);
            _context.SaveChanges();
        }

        public void Delete<T>(int Id) where T : class, IEntity
        {
            _context.Remove(FindById<T>(Id));
            _context.SaveChanges();
        }

        public IEnumerable<T> FetchAll<T>() where T : class, IEntity => _context.Set<T>();

        public async Task<IEnumerable<T>> FetchAllAsync<T>() where T : class,   IEntity => await _context.Set<T>().ToListAsync(); 

        public T FindById<T>(int Id) where T : class, IEntity => _context!.Set<T>().FirstOrDefault(x => x.Id == Id);

        public async Task AddAsync<T>(T obj) where T : class, IEntity
        {
            await _context.AddAsync(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<T> FindByIdAsync<T>(int Id) where T : class, IEntity => await _context!.Set<T>().FirstOrDefaultAsync(x => x.Id == Id);

        public async Task<T> FindFirstAsync<T>(Expression<Func<T, bool>> condition) where T : class, IEntity
        {
            return await _context.Set<T>().Where(condition).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> FilterByAsync<T>(Expression<Func<T, bool>> condition) where T : class, IEntity
        {
            return await _context.Set<T>().Where(condition).ToListAsync();
        }

        public void Update<T>(T item) where T : class, IEntity
        {
            _context.Update(item);
            _context.SaveChanges();
        }
        public  IEnumerable<TerminalLegDto> FetchAllLegs() =>  _context.Legs.Include(l => l.NextLegConnections).ToList();

    }
}
