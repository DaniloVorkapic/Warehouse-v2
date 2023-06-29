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
        Task<Result> GetAllOrderItemsByOrderId(long orderId);
        Task<Result> GetOrderItemById(long id);
        Task<Result> CreateOrder(OrderItemContract orderItemDto);
        Task<Result> AddOrderWithOrderItem(OrderItemContract oc);
        Task<Result> AddOrderItem(OrderItemContract orderItemDto);
        Task<Result> UpdateOrder(OrderContract oc);
        Task<Result> UpdateOrderItem(OrderItemContract orderItemDto);
        Task<Result> DeleteOrder(long id);
        Task<Result> DeleteOrderitem(long id);

    }
}
