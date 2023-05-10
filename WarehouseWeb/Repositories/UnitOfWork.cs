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
        public UnitOfWork(DataContext context,IGenericRepository<Product> productRepository,IGenericRepository<OrderItem> orderItemRepository,IGenericRepository<Supplier> supplierRepository,IGenericRepository<StorageItem> storageItemRepository,IGenericRepository<Order> orderRepository,IGenericRepository<Customer> customerRepository,IGenericRepository<Storage> storageRepository,IGenericRepository<StorageInputOutput> storageInputOutputRepository,IGenericRepository<CustomerRole> customerRoleRepository, IGenericRepository<Role> roleRepository,IGenericRepository<User> userRepository,IGenericRepository<Claims> claimsRepository, IGenericRepository<RoleClaims> roleClaimsRepository)
        { 
            _context = context;
            UserRepository = userRepository;
            ClaimsRepository = claimsRepository;
            RoleClaimsRepository = roleClaimsRepository;
            ProductRepository = productRepository;
            OrderItemRepository = orderItemRepository;
            SupplierRepository = supplierRepository;
            StorageItemRepository = storageItemRepository;
            OrderRepository = orderRepository;
            CustomerRepository = customerRepository;
            StorageRepository = storageRepository;
            StorageInputOutputRepository = storageInputOutputRepository;
            CustomerRoleRepository = customerRoleRepository;
            RoleRepository = roleRepository;
        }
       public IGenericRepository<Product> ProductRepository { get; }
        public IGenericRepository<OrderItem> OrderItemRepository { get; }
        public IGenericRepository<Supplier> SupplierRepository { get; }
        public IGenericRepository<StorageItem> StorageItemRepository { get; }
        public IGenericRepository<Storage> StorageRepository { get; }
        public IGenericRepository<StorageInputOutput> StorageInputOutputRepository { get; }
        public IGenericRepository<CustomerRole> CustomerRoleRepository { get; }
        public IGenericRepository<Role> RoleRepository { get; }
        public IGenericRepository<User> UserRepository { get; }
        public IGenericRepository<Claims> ClaimsRepository { get; }
        public IGenericRepository<RoleClaims> RoleClaimsRepository { get; }
        public IGenericRepository<Order> OrderRepository { get; }
        public IGenericRepository<Customer> CustomerRepository { get; }
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
