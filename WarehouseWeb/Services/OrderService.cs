using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Contracts;
using WarehouseWeb.Model;
using WarehouseWeb.Repositories;

namespace WarehouseWeb.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IGenericRepository<OrderItem> _orderItemRepository;
        private readonly IGenericRepository<StorageItem> _storageItemRepository;

        public OrderService(IUnitOfWork unitOfWork, IGenericRepository<Order> orderRepository,IGenericRepository<OrderItem> orderItemRepository,IGenericRepository<StorageItem> storageItemRepository)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _storageItemRepository = storageItemRepository;
        }

        public async Task<Result> AddOrder(OrderContract oc)
        {
            try
            {   
                var statusCode = StatusCodes.Status200OK;
                var result = Result.Create(null, 0);
                if (oc == null)
                {
                    statusCode = StatusCodes.Status400BadRequest;
                    result = Result.Create(null, 0);
                    return result;
                } 

                List<OrderItem> orderItems = PrepareOrderItems(oc.OrderItemList);
                double sum = orderItems.Sum(oi => oi.PricePerUnit * oi.Quantity.Amount);
       
                var order = new Order
                {
                    //order.Id = oc.Id;
                    Status = oc.Status,
                    CustomerId = oc.CustomerId,
                    OrderDate = oc.CreateDate,
                    ExpireDate = oc.CreateDate,
                    OrderItemList = orderItems,
                    TotalPrice = sum,
                };
             
                var addOrder = await _orderRepository.AddEntity(order);
                _unitOfWork.commit();
                result = Result.Create(addOrder.Id, statusCode);
                return result;

            }
            catch (Exception ex)
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode);
                return result;
            }
        }

        public List<OrderItem> PrepareOrderItems(List<OrderItemContract> oic)
        {
            List<OrderItem> lista = new List<OrderItem>();
            foreach (var oi in oic)
            {  
                var orderItem = new OrderItem();
                orderItem.SerialNumber = oi.SerialNumber;
                orderItem.ProductId = oi.ProductId;
                orderItem.PricePerUnit = oi.PricePerUnit;
                orderItem.Quantity = oi.Quantity;
      
                lista.Add(orderItem);
        }

            return lista;
        }

        public double GetProductStock(long storageItemId)
        {
           var storageItem =  _storageItemRepository.GetById(storageItemId); 
            double storageItemQuantity = 0;
            storageItemQuantity = _storageItemRepository.GetQueryable<StorageItem>()
            .Sum(x => x.Quantity.Amount);

            return storageItemQuantity;
        }

        public async Task<Result> DeleteOrder(long id)
        {
            try
            {
                var statusCode = StatusCodes.Status200OK;
                var result = Result.Create(null, statusCode);

                var order = await _orderRepository.GetById(id);
                if (order == null)
                {
                    statusCode = StatusCodes.Status400BadRequest;
                    result = Result.Create(null, statusCode);
                    return result;
                }
                var deleteOrder = await _orderRepository.Delete(order);
                _unitOfWork.commit();
                result = Result.Create(deleteOrder, statusCode);
                return result;
            }
            catch (Exception)
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode);
                return result;
            }
        }

        public async Task<Result> GetAllOrders()
        {
            try
            {
                var statusCode = StatusCodes.Status200OK;
                var allProducts = await _orderRepository.GetAll();
                var result = Result.Create(allProducts, statusCode);
                return result;

            }
            catch (Exception)
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode);
                return result;
            }

        }

        public async Task<Result> GetOrderById(long id)
        {
            try
            {
                var statusCode = StatusCodes.Status200OK;
                var result = Result.Create(null, 0);
                var order = await _orderRepository.GetById(id);

                if (order == null)
                {
                    statusCode = StatusCodes.Status400BadRequest;
                    result = Result.Create(null, statusCode);
                    return result;
                }
                result = Result.Create(order, statusCode);
                return result;
            }
            catch (Exception)
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode);
                return result;
            }
        }

        public async Task<Result> UpdateOrder(long id, OrderContract oc)
        {
            try
            {
                var statusCode = StatusCodes.Status200OK;
                var result = Result.Create(null, 0);
                var order = await _orderRepository.GetById(id);

                if (order == null)
                {
                    statusCode = StatusCodes.Status400BadRequest;
                    result = Result.Create(null, statusCode);
                    return result;
                }

                var orderItemList = GetAllOrderItemsByOrderId(order.Id);

                _orderItemRepository.DeletAll(orderItemList);

                List<OrderItem> orderItems = PrepareOrderItems(oc.OrderItemList);

                double sum = orderItems.Sum(oi => oi.PricePerUnit * oi.Quantity.Amount);

                order.Status = oc.Status;
                order.CustomerId = oc.CustomerId;
                order.OrderDate = oc.CreateDate;
                order.ExpireDate = oc.CreateDate;
                order.OrderItemList = orderItems;
                order.TotalPrice = sum;
               
                var addOrder = await _orderRepository.UpdateEntity(id,order);
                _unitOfWork.commit();
                result = Result.Create(addOrder.Id, statusCode);
                return result;
            }
            catch (Exception ex)
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode);
                return result;
            }
        }
        public List<OrderItem> GetAllOrderItemsByOrderId(long orderId)
        {
            List<OrderItem> orderItemList = new List<OrderItem>();

            orderItemList = _orderItemRepository.GetQueryable<OrderItem>()
                 .Where(x => x.OrderId == orderId)
                 .ToList();
            return orderItemList;
        }
    }
}
