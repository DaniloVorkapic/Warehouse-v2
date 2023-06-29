using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Model;

namespace WarehouseWeb.Data
{
    public class DataContext:IdentityDbContext<User> 
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("data source=(localdb)\\MSSQLLocalDB;Initial Catalog=WarehouseDBVol3;Integrated Security=True;");
        }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerRole>()
                .HasOne(e => e.Role)
                .WithMany(cr => cr.CustomerRoleList)
                .HasForeignKey(ki => ki.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CustomerRole>()
                .HasOne(e => e.Customer)
                .WithMany(cr => cr.CustomerRoleList)
                .HasForeignKey(ki => ki.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RoleClaims>()
               .HasOne(e => e.Role)
               .WithMany(cr => cr.RoleClaimsList)
               .HasForeignKey(ki => ki.RoleId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RoleClaims>()
                .HasOne(e => e.Claims)
                .WithMany(cr => cr.RoleClaimsList)
                .HasForeignKey(ki => ki.ClaimsId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StorageItem>()
                .HasOne(s => s.Storage)
                .WithMany(g => g.StorageItemList)
                .HasForeignKey(s => s.StorageId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StorageItem>()
                .HasMany(s => s.StorageInputOutputList)
                .WithOne(g => g.StorageItem)
                .HasForeignKey(s => s.StorageItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StorageItem>()
               .HasOne(s => s.Product)
               .WithMany()
               .HasForeignKey(s => s.ProductId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Product>()
               .HasOne(s => s.Supplier)
               .WithMany()
               .HasForeignKey(s => s.SupplierId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
               .HasOne(s => s.ClassificationValues)
               .WithMany()
               .HasForeignKey(s => s.ClassificationValuesId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
               .HasOne(s => s.Order)
               .WithMany(g => g.OrderItemList)
               .HasForeignKey(s => s.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
               .HasOne(s => s.Product)
               .WithMany()
               .HasForeignKey(s => s.ProductId)
               .OnDelete(DeleteBehavior.Restrict);

            //     modelBuilder.Entity<OrderItem>()
            //.HasOne(s => s.Supplier)
            //.WithMany()
            //.HasForeignKey(s => s.SupplierId)
            //.OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
               .HasOne(s => s.Customer)
               .WithMany()
               .HasForeignKey(s => s.CustomerId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Customer>()
               .HasOne(s => s.Company)
               .WithMany()
               .HasForeignKey(s => s.CompanyId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClassificationValues>()
               .HasOne(s => s.ClassificationSpecification)
               .WithMany(g => g.ClassificationValuesList)
               .HasForeignKey(s => s.ClassifictionSpecificationId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StorageItem>()
               .OwnsOne(x => x.Quantity);

            modelBuilder.Entity<StorageInputOutput>()
               .OwnsOne(x => x.Quantity);

            modelBuilder.Entity<OrderItem>()
               .OwnsOne(x => x.Quantity);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Product> Product { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<Storage> Storage { get; set; }
        public DbSet<StorageItem> StorageItem { get; set; }
        public DbSet<StorageInputOutput> StorageInputOutput { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Claims> Claims { get; set; }
        public  DbSet<RoleClaims> RoleClaims { get; set; }
        public DbSet<CustomerRole> CustomerRole { get; set; }
        public DbSet<ClassificationSpecification> ClassificationSpecification { get; set; }
        public DbSet<ClassificationValues> ClassificationValues { get; set; }
    }
}
