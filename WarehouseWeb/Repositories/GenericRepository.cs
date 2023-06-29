using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Data;

namespace WarehouseWeb.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DataContext _context;
        public GenericRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> AddEntity(T entity)
        {
             await _context.AddAsync(entity);   
            return true;
        }
        public async Task<T> GetById(long id)
        {
            return await  _context.Set<T>().FindAsync(id);
        }
        public async Task<bool> Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
           return true;   
        }
        public async  Task<IEnumerable<T>> GetAll()
        {

            return await _context.Set<T>().ToListAsync();
    
        }
        public async Task<bool> UpdateEntity( T entity)
        {
              _context.Update(entity);
            return  true;
        }
        public async void DeletAll(List<T> entities)
        {
           _context.RemoveRange(entities);
        }
        public IQueryable<T> GetQueryable<T>() where T : class
        {
            DbSet<T> set = _context.Set<T>();
            if (set == null)
            {
                throw new Exception("cannot-get-db-query");
            }
            return set as IQueryable<T>;
        }
    }
}
