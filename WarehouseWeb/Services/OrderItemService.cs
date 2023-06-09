﻿//using Microsoft.AspNetCore.Http;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using WarehouseWeb.Contracts;
//using WarehouseWeb.Model;
//using WarehouseWeb.Repositories;

//namespace WarehouseWeb.Services
//{
//    public class OrderItemService:IOrderItemService
//    {
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly IGenericRepository<OrderItem> _orderItemRepository;
//        private readonly IGenericRepository<Product> _productRepository;
//        public OrderItemService(IUnitOfWork unitOfWork,IGenericRepository<OrderItem> orderItemRepository,IGenericRepository<Product> productRepository)
//        {
//           _unitOfWork = unitOfWork;
//           _orderItemRepository = orderItemRepository;
//           _productRepository = productRepository;
//        }

//        public  async Task<Result> AddOrderItem(OrderItemContract oic)
//        {
//            try
//            {
//                var statusCode = StatusCodes.Status500InternalServerError;
//                var errorMessage = "Greska";
//                var result = Result.Create(null, statusCode,errorMessage);
//                if (oic == null) {
//                    result.StatusCode = StatusCodes.Status400BadRequest;
//                    result.ErrorMessage = "Losi ulanzi parametri";
//                    return result;    
//                }
//                var orderItem = new OrderItem();
   
//                orderItem.SerialNumber = oic.SerialNumber;
//                orderItem.ProductId = oic.ProductId;
//                orderItem.PricePerUnit = oic.PricePerUnit;
//                orderItem.CreateDate = null;
                

//                var addProduct = await _orderItemRepository.AddEntity(orderItem);
//                result.Value = addProduct;
//                result.StatusCode = StatusCodes.Status200OK;
//                result.ErrorMessage = null;
//                _unitOfWork.commit();
//                return result;
//            }
//            catch (Exception)
//            {
//                var statusCode = StatusCodes.Status500InternalServerError;
//                var result = Result.Create(null, statusCode,"greska");
//                return result;
//            }
//        }

//        public async Task<Result> DeleteOrderItem(long id)
//        {
//            try
//            {
//                var statusCode = StatusCodes.Status500InternalServerError;
//                var errorMessage = "Greska";
//                var result = Result.Create(null, statusCode,errorMessage);
//                var orderItem =   await _orderItemRepository.GetById(id);
//                if(orderItem == null)
//                {
//                    result.StatusCode = StatusCodes.Status400BadRequest;
//                    result.ErrorMessage = "Item nije pronadjen";
//                    return result;
//                }

//                var deleteOrderIdem = await _orderItemRepository.Delete(orderItem);
//                result.Value = deleteOrderIdem;
//                result.StatusCode = StatusCodes.Status200OK;
//                result.ErrorMessage = null;
//                _unitOfWork.commit();
//                return result;
//            }
//            catch (Exception)
//            {
//                var statusCode = StatusCodes.Status500InternalServerError;
//                var result = Result.Create(null, statusCode,"Greska");
//                return result;
//            }
//        }
//        public async  Task<Result> GetAllOrderItems()
//        {
//            try
//            {
//                var statusCode = StatusCodes.Status200OK;
//                var allOrders =  await _orderItemRepository.GetAll();
//                var result = Result.Create(allOrders, statusCode,null);

//                return result;
//            }
//            catch (Exception)
//            {
//                var statusCode = StatusCodes.Status500InternalServerError;
//                var result = Result.Create(null, statusCode,"Greska");
//                return result;

//            }
//        }

//        public  async Task<Result> GetOrderItemById(long id)
//        {
//            try
//            {
//                var statusCode = StatusCodes.Status500InternalServerError;
//                var errorMessage = "Greska";
//                var result = Result.Create(null, statusCode.ToString);
//                var orderItem = await _orderItemRepository.GetById(id);
//                if (orderItem == null)
//                {
//                    statusCode = StatusCodes.Status400BadRequest;
//                    result = Result.Create(null, statusCode);
//                    return result;
//                }
//                result = Result.Create(orderItem, statusCode);
//                return result;
//            }
//            catch (Exception)
//            {
//                var statusCode = StatusCodes.Status500InternalServerError;
//                var result = Result.Create(null, statusCode);
//                return result;  
//            }
//        }

//        public async Task<Result> UpdateOrderItem( OrderItemContract oi)
//        {
//            try
//            {
//                var statusCode = StatusCodes.Status200OK;
//                var result = Result.Create(null, 0);
//                var orderItem = await _orderItemRepository.GetById(oi.Id);

//                if(orderItem == null)
//                {
//                    statusCode = StatusCodes.Status400BadRequest;
//                    result = Result.Create(null, statusCode);
//                    return result;
//                }

//                 orderItem.Id = oi.Id;
//                orderItem.SerialNumber = oi.SerialNumber;
//                orderItem.ProductId = oi.ProductId;

//               // orderItem.PricePerUnit = oi.PricePerUnit;
//               // orderItem.SupplierId = oi.SupplierId;
//               // orderItem.Quantity = oi.Quantity;
//               //// resault.CreateDate = orderItem.CreateDate;
//               // orderItem.ModifyDate = oi.ModifyDate;

//                var updateOrderItem =  await _orderItemRepository.UpdateEntity(orderItem);
//                _unitOfWork.commit();
//                result = Result.Create(updateOrderItem, statusCode);
//                return result;   
//            }
//            catch (Exception)
//            {
//                var statusCode = StatusCodes.Status500InternalServerError;
//                var result = Result.Create(null, statusCode);
//                return result;

//            }
//        }
//    }
//}
