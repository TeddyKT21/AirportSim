using AirportSimCore.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AirportSimCore.Interfaces
{
    public interface IAirportRepository
    {
        public void Add<T>(T obj) where T : class, IEntity;
        public void Delete<T>(int Id) where T : class, IEntity;
        public void Update<T>(T obj) where T : class, IEntity;
        public T FindById<T>(int Id) where T : class, IEntity;
        public Task<T> FindByIdAsync<T>(int Id) where T : class, IEntity;
        public IEnumerable<T> FetchAll<T>() where T : class, IEntity;
        public Task<IEnumerable<T>> FetchAllAsync<T>() where T : class, IEntity;
        public Task AddAsync<T>(T obj) where T : class, IEntity;
        public Task<T> FindFirstAsync<T>(Expression<Func<T, bool>> condition) where T : class, IEntity;
        public Task<IEnumerable<T>> FilterByAsync<T>(Expression<Func<T, bool>> condition) where T : class, IEntity;
        public IEnumerable<TerminalLegDto> FetchAllLegs();

    }
}
