using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Data;
using WarehouseWeb.Model;

namespace WarehouseWeb.Repositories
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly DataContext _context;
        public UnitOfWork(DataContext context)
        { 
            _context = context; 
        }
      
        public void Dispose()
        {
            _context.Dispose();
        }
        public int commit()
        {
           UpdateAuditableEntities();
           return  _context.SaveChanges();
        }
        public IDbContextTransaction myTransaction()
        {
           return  _context.Database.BeginTransaction();
        }
        public void UpdateAuditableEntities()
        {

            //IEnumerable<EntityEntry<IAuditableEntity>> entries =_context.ChangeTracker.Entries<IAuditableEntity>();
           var entityEntries = _context.ChangeTracker.Entries<CoreObject>();

            foreach (var entry in entityEntries)
            {
                if (entry.State == EntityState.Added)
                    entry.Property(x => x.CreateDate).CurrentValue = DateTime.UtcNow;
                if(entry.State == EntityState.Modified)
                    entry.Property(x => x.ModifyDate).CurrentValue = DateTime.Now;
            }

            
        }
    }
}
