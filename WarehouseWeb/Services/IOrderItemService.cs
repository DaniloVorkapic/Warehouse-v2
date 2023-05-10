using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Contracts;
using WarehouseWeb.Model;

namespace WarehouseWeb.Services
{
   public interface IOrderItemService
    {
        Task<Result> GetAllOrderItems();
        Task<Result> GetOrderItemById(long id);
        Task<Result> AddOrderItem(OrderItemContract oic);
        Task<Result> UpdateOrderItem(long id, OrderItemContract oi);
        Task<Result> DeleteOrderItem(long id);
    }
}
