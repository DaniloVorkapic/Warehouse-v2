using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseWeb.Repositories
{
   public interface IGenericRepository<T> where T: class
    {
       Task<IEnumerable<T>> GetAll();
       Task<T> GetById(long id);
       Task<bool> AddEntity(T entity);
       Task<bool> UpdateEntity(T entity);
       Task<bool> Delete(T entity);
       void DeletAll(List<T> entities);
       IQueryable<T> GetQueryable<T>() where T : class;

    }
}
