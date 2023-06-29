using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Contracts;
using WarehouseWeb.Contracts.SupplierDto;
using WarehouseWeb.Model;

namespace WarehouseWeb.Services
{
    public interface ISupplierService
    {
        Task<Result> GetAllSuppliers(InputSupplierDto input);
        Task<Result> GetSupplierById(long id);
        Task<Result> AddSupplier(SupplierContract sc);
        Task<Result> UpdateSupplier(SupplierContract sc);
        Task<Result> DeleteSupplier(long id);
    }
}
