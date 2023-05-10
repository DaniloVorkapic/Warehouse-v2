using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Contracts;
using WarehouseWeb.Model;

namespace WarehouseWeb.Services
{
    public interface IOrderService
    {
        Task<Result> GetAllOrders();
        Task<Result> GetOrderById(long id);
        Task<Result> AddOrder(OrderContract oc);
        Task<Result> UpdateOrder(long id, OrderContract oc);
        Task<Result> DeleteOrder(long id);

    }
}
