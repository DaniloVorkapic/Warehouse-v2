using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Contracts;

namespace WarehouseWeb.Services
{
    public interface ICustomerService
    {
        Task<Result> GetAllCustomers();
        Task<Result> GetCustomerById(long id);
        Task<Result> AddCustomer(CustomerContract cc);
        Task<Result> UpdateCustomer(long id, CustomerContract cc);
        Task<Result> DeleteCustomer(long id);
    }
}
