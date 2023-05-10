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
        public async Task<T> AddEntity(T entity)
        {
            var result =  await _context.AddAsync(entity);   
            return result.Entity;
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
        public async Task<T> UpdateEntity(long id, T entity)
        {
            var result = _context.Update(entity);
            return result.Entity;
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
