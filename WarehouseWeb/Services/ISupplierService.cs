using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Contracts;
using WarehouseWeb.Model;

namespace WarehouseWeb.Services
{
    public interface ISupplierService
    {
        Task<Result> GetAllSuppliers();
        Task<Result> GetSupplierById(long id);
        Task<Result> AddSupplier(SupplierContract sc);
        Task<Result> UpdateSupplier(long id, SupplierContract sc);
        Task<Result> DeleteSupplier(long id);
    }
}
