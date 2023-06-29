using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Contracts;
using WarehouseWeb.Contracts.OrderDto;
using WarehouseWeb.Model;
using WarehouseWeb.Model.Enums;
using WarehouseWeb.Repositories;

namespace WarehouseWeb.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IGenericRepository<OrderItem> _orderItemRepository;
        private readonly IGenericRepository<StorageItem> _storageItemRepository;
        private readonly IGenericRepository<Product> _productRepository;

        public OrderService(IUnitOfWork unitOfWork, IGenericRepository<Order> orderRepository, IGenericRepository<OrderItem> orderItemRepository, IGenericRepository<StorageItem> storageItemRepository,IGenericRepository<Product> productRepository)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _storageItemRepository = storageItemRepository;
            _productRepository = productRepository;
        }

        public async Task<Result> CreateOrder(OrderItemContract orderItemDto)
        {
            var result = Result.Create(null, StatusCodes.Status500InternalServerError, "Greska",0);
            var order = _orderRepository.GetQueryable<Order>()
                .Any(x => x.CustomerId == 2 && x.Status == Status.Pending);
                
            

            if (order==true)
            {
                

                result.Value = await AddOrderItem(orderItemDto);
                result.ErrorMessage = null;
                result.StatusCode = StatusCodes.Status200OK;
                return result;
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.Value = await AddOrderWithOrderItem(orderItemDto);
            result.ErrorMessage = null;
            return result;


        }

        public async Task<Result> AddOrderWithOrderItem(OrderItemContract orderItemDto)
        {
            try
            {

           
                var statusCode = StatusCodes.Status500InternalServerError;

                var result = Result.Create(null, statusCode,"Greska",0);
                if (orderItemDto == null)
                {
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    result.ErrorMessage = "Losi ulazni parametri";
                    return result;
                }

                StorageItem storageItem = _storageItemRepository.GetQueryable<StorageItem>()
               .Include(x => x.StorageInputOutputList)
                .Where(x => x.ProductId == orderItemDto.ProductId).FirstOrDefault();

                if (storageItem == null)
                {
                    result.StatusCode = StatusCodes.Status404NotFound;
                    result.ErrorMessage = "Item nije pronadjen";
                    return result;
                }

                if (storageItem.Quantity.Amount < orderItemDto.Quantity.Amount)
                {
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    result.ErrorMessage = "Nema dovoljno itema na stanju";
                    return result;
                }
                StorageInputOutput storageInputOutput = new StorageInputOutput();
                storageInputOutput.StorageItemId = storageItem.Id;
                storageInputOutput.StorageInputOutputType = StorageInputOutputType.Output;
                storageInputOutput.Quantity = new Quantity
                {
                    Amount = orderItemDto.Quantity.Amount,
                    MeasurementUnitId = orderItemDto.Quantity.MeasurementUnitId
                };
                storageItem.StorageInputOutputList.Add(storageInputOutput);


                storageItem.GetStock();

                //double pricePerUnit = GetProductPrice(orderItemDto.ProductId);


                var order = new Order();


                    order.Status = Status.Pending;
                    order.CustomerId = 2;
                    order.OrderDate = null;
                    order.ExpireDate = null;
                    
                    

                    var orderItem = new OrderItem();
                    orderItem.SerialNumber = orderItemDto.SerialNumber;
                orderItem.PricePerUnit = orderItemDto.PricePerUnit;
                    orderItem.ProductId = orderItemDto.ProductId;
                    orderItem.Quantity = new Quantity()
                        {
                            Amount = orderItemDto.Quantity.Amount,
                            MeasurementUnitId = orderItemDto.Quantity.MeasurementUnitId
                
                        };
                
                
                List<OrderItem> lista = new List<OrderItem>();
                
                  lista.Add(orderItem);
                  order.OrderItemList = lista;
                  order.GetTotalPrice();




                var isAdded = await _orderRepository.AddEntity(order);
                if(isAdded == false)
            {
                result.Value = isAdded;
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Couldn't add order";
                return result;
            }
                _unitOfWork.commit();
                result.ErrorMessage = null;
                result.Value = isAdded;
                result.StatusCode = StatusCodes.Status200OK;
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }


        }
        public async Task<Result> AddOrderItem(OrderItemContract orderItemDto)
        {
            var statusCode = StatusCodes.Status500InternalServerError;
            var errorMessage = "Greska";
            var result = Result.Create(null, statusCode, errorMessage,0);

            if (orderItemDto == null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Losi Ulazni Podaci";
                return result;
            }

            Order order = _orderRepository.GetQueryable<Order>()
                    .Include(x => x.OrderItemList)
                    .ThenInclude(x => x.Product)
                    .Where(x => x.Status == Status.Pending && x.CustomerId == 2).FirstOrDefault();

           

            StorageItem storageItem = _storageItemRepository.GetQueryable<StorageItem>()
                .Include(x => x.StorageInputOutputList)
                 .Where(x => x.ProductId == orderItemDto.ProductId).FirstOrDefault();

            if(storageItem == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Item nije pronadjen";
                return result;    
            }

            if(storageItem.Quantity.Amount < orderItemDto.Quantity.Amount)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Nema dovoljno itema na stanju";
                return result;
            }
            StorageInputOutput storageInputOutput = new StorageInputOutput();
            storageInputOutput.StorageItemId = storageItem.Id;
            storageInputOutput.StorageInputOutputType = StorageInputOutputType.Output;
            storageInputOutput.Quantity = new Quantity
            {
                Amount = orderItemDto.Quantity.Amount,
                MeasurementUnitId = orderItemDto.Quantity.MeasurementUnitId
            };
            
            storageItem.StorageInputOutputList.Add(storageInputOutput);

            storageItem.GetStock();



            //foreach (var item in order.OrderItemList)
            //{
            //    item.PricePerUnit = item.Product.Price;

            //}

            order.OrderItemList.All(x => { x.PricePerUnit = x.Product.Price; return true; });

            
             var findOrderItem = order.OrderItemList
                .Any(x => x.OrderId == order.Id && x.ProductId == orderItemDto.ProductId);

            if (findOrderItem == true)
            {
                result.ErrorMessage = "Postoji dati orderItem za order, mozete promeniti kolicinu";
                result.StatusCode = StatusCodes.Status200OK;
                result.Value = null;
                return result;
            }

            
          
            

            if(order == null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Order ne postoji";
                return result;

            }

            
            var orderItem = new OrderItem();

            

            orderItem.OrderId = order.Id;
            orderItem.ProductId = orderItemDto.ProductId;
            orderItem.SerialNumber = orderItemDto.SerialNumber;
            orderItem.PricePerUnit = orderItemDto.PricePerUnit; 
            orderItem.Quantity = new Quantity()
            {

                Amount = orderItemDto.Quantity.Amount,
                MeasurementUnitId = orderItemDto.Quantity.MeasurementUnitId

            };
            order.OrderItemList.Add(orderItem);
            order.GetTotalPrice();
            


            var isAdded = await _orderItemRepository.AddEntity(orderItem);
            if (isAdded == false)
            {
                result.Value = isAdded;
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Couldn't add a storageItem";
            }

            _unitOfWork.commit();
            result.StatusCode = StatusCodes.Status200OK;
            result.Value = isAdded;
            result.ErrorMessage = null;
            return result;
        }


     


        //public double GetProductPrice(long productId)
        //{
        //    Product product = _productRepository.GetQueryable<Product>()
        //           .AsNoTracking()
        //           .Where(x => x.Id == productId)
        //           .FirstOrDefault();

        //    return product.Price;
        //}

        //public List<OrderItem> PrepareOrderItems(List<OrderItemContract> oic)
        //{
        //    List<OrderItem> lista = new List<OrderItem>();
        //    foreach (var oi in oic)
        //    {
        //        var orderItem = new OrderItem();
        //        orderItem.SerialNumber = oi.SerialNumber;
        //        orderItem.ProductId = oi.ProductId;
        //        orderItem.PricePerUnit = oi.PricePerUnit;
        //        orderItem.Quantity = new Quantity()
        //        {
        //            Amount = oi.Quantity.Amount,
        //            MeasurementUnitId = oi.Quantity.MeasurementUnitId

        //        };

        //        lista.Add(orderItem);
        //    }

        //    return lista;
        //}

        //public double GetProductStock(long storageItemId)
        //{
        //    var storageItem = _storageItemRepository.GetById(storageItemId);
        //    double storageItemQuantity = 0;
        //    storageItemQuantity = _storageItemRepository.GetQueryable<StorageItem>()
        //    .Sum(x => x.Quantity.Amount);

        //    return storageItemQuantity;
        //}

        public async Task<Result> DeleteOrder(long id)
        {
           
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode,"Greska",0);

                var order = await _orderRepository.GetById(id);
                if (order == null)
                {
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    result.ErrorMessage = "Order ne posotji";
                    return result;
                }
                var isDeleted = await _orderRepository.Delete(order);
            if (isDeleted == false)
            {
                result.Value = isDeleted;
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Couldn't delete order";
                return result;
            }

            _unitOfWork.commit();
                result.ErrorMessage = null;
                result.StatusCode = StatusCodes.Status200OK;
                result.Value = isDeleted;
                return result;
           
        }

        public async Task<Result> GetAllOrders()
        {
             var statusCode = StatusCodes.Status200OK;
                var allProducts = _orderRepository.GetQueryable<Order>()
                    .AsNoTracking()
                    .Select(x => new GetAllOrdersResponse(x.Id, x.TotalPrice, x.CustomerId)).ToList();
                var result = Result.Create(allProducts, statusCode,null,0);
                return result;

            

        }

        public async Task<Result> GetOrderById(long id)
        {
           
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode,"greska",0);
                var order = _orderRepository.GetQueryable<Order>()
                     .AsNoTracking()
                    .Where(x => x.Id == id)
                    .Select(x => new GetOrderByIdResponse(x.Id, x.TotalPrice, x.CustomerId))
                    .FirstOrDefault();

                if (order == null)
                {
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    result.ErrorMessage = "Ne postoji dati order";
                    return result;
                }
                result.StatusCode = StatusCodes.Status200OK;
                result.Value = order;
                result.ErrorMessage = null;
                return result;
           
        }

        public async Task<Result> UpdateOrder(OrderContract oc)
        {
            
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode,"Greska",0);
                var order = await _orderRepository.GetById(oc.Id);

                if (order == null)
                {
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    result.ErrorMessage = "Ne postoji dati order";
                    return result;
                }
       

                order.Status = oc.Status;
                order.CustomerId = oc.CustomerId;
               

                var isUpdeted = await _orderRepository.UpdateEntity(order);
            if (isUpdeted == false)
            {
                result.Value = isUpdeted;
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Couldn't update order";
                return result;
            }
            _unitOfWork.commit();
                result.Value = isUpdeted;
                result.StatusCode = StatusCodes.Status200OK;
                result.ErrorMessage = null;
                return result;
           
        }

        //public List<OrderItem> AllOrderItemsByOrderId(long orderId)
        //{
        //    List<OrderItem> orderItemList = new List<OrderItem>();

        //    orderItemList = _orderItemRepository.GetQueryable<OrderItem>()
        //         .Where(x => x.OrderId == orderId)
        //         .ToList();
        //    return orderItemList;
        //}
        public async Task<Result> GetAllOrderItemsByOrderId(long orderId)
        {
            var statusCode = StatusCodes.Status500InternalServerError;
            var errorMessage = "Greska";
            var result = Result.Create(null, statusCode, errorMessage,0);

            var order = GetOrderById(orderId);

            if (orderId < 1)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Los order id";
                return result;

            }
            if (order == null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Order  ne postoji";
                return result;
            }



            //  var statusCode = StatusCodes.Status200OK;
            var allOrderItems = _orderItemRepository.GetQueryable<OrderItem>()
                .AsNoTracking()
               .Where(x => x.OrderId == orderId)
                .Select(x => new GetAllOrderItemsByOrderIdResponse(x.Id, x.OrderId, x.ProductId, x.PricePerUnit)).ToList();

            result.Value = allOrderItems;
            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;
            return result;
        }

        public async Task<Result> GetOrderItemById(long id)
        {
            var statusCode = StatusCodes.Status500InternalServerError;
            var result = Result.Create(null, statusCode, "greska",0);
            var orderItem = _orderItemRepository.GetQueryable<OrderItem>()
                 .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new GetOrderItemByIdResponse(x.Id, x.OrderId, x.ProductId))
                .FirstOrDefault();

            if (orderItem == null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Ne postoji dati order item";
                return result;
            }
            result.StatusCode = StatusCodes.Status200OK;
            result.Value = orderItem;
            result.ErrorMessage = null;
            return result;


        }


        public async Task<Result> UpdateOrderItem(OrderItemContract orderItemDto)
        {
            var statusCode = StatusCodes.Status500InternalServerError;
            var errorMessage = "Greska";
            var result = Result.Create(null, statusCode, errorMessage,0);

            var order = _orderRepository.GetQueryable<Order>()
                .Include(x => x.OrderItemList)
                .ThenInclude(x=> x.Product)
                .Where(x => x.Status == Status.Pending && x.CustomerId == 2).FirstOrDefault();

            StorageItem storageItem = _storageItemRepository.GetQueryable<StorageItem>()
                .Include(x => x.StorageInputOutputList)
                
                .Where(x => x.ProductId == orderItemDto.ProductId).FirstOrDefault();

           

            OrderItem orderItem = order.OrderItemList.Find(x => x.Id == orderItemDto.Id);

            order.OrderItemList.All(x => { x.PricePerUnit = x.Product.Price; return true; });

            if (orderItem == null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Nije pronadjen order item";
                return result;
            }

          //  double pricePerUnit = GetProductPrice(orderItemDto.ProductId);
           // orderItem.ProductId = orderItemDto.ProductId;
            orderItem.Quantity = new Quantity()
            {
                Amount = orderItemDto.Quantity.Amount,
                MeasurementUnitId = orderItemDto.Quantity.MeasurementUnitId

            };



            var isUpdated = await _orderItemRepository.UpdateEntity(orderItem);
            
            

            if (isUpdated == false)
            {
                result.Value = isUpdated;
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Couldn'y update storage item";
                return result;
            }

            order.GetTotalPrice();


            if (storageItem == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Item nije pronadjen";
                return result;
            }

            if (storageItem.Quantity.Amount < orderItemDto.Quantity.Amount)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Nema dovoljno itema na stanju";
                return result;
            }



            StorageInputOutput storageInputOutput = storageItem.StorageInputOutputList.FindLast(x => x.StorageItemId == storageItem.Id && x.StorageInputOutputType == StorageInputOutputType.Output && x.CreatedBy == 2);
            storageItem.StorageInputOutputList.Remove(storageInputOutput);
            StorageInputOutput newStorageInputOutput = new StorageInputOutput();
            newStorageInputOutput.StorageItemId = storageItem.Id;
            newStorageInputOutput.StorageInputOutputType = StorageInputOutputType.Output;
            newStorageInputOutput.Quantity = new Quantity
            {
                Amount = orderItemDto.Quantity.Amount,
                MeasurementUnitId = orderItemDto.Quantity.MeasurementUnitId
            };

            storageItem.StorageInputOutputList.Add(newStorageInputOutput);

            storageItem.GetStock();

            _unitOfWork.commit();
            result.Value = isUpdated;
            result.ErrorMessage = null;
            result.StatusCode = StatusCodes.Status200OK;
            return result;

        }

        public  async Task<Result> DeleteOrderitem(long id)
        {
            var statusCode = StatusCodes.Status500InternalServerError;
            var result = Result.Create(null, statusCode, "Greska",0);

             Order order = _orderRepository.GetQueryable<Order>()
                    .Include(x => x.OrderItemList)
                    .ThenInclude(x => x.Product)
                    .Where(x => x.Status == Status.Pending && x.CustomerId == 2).FirstOrDefault();

            var orderItem = order.OrderItemList.Find(x => x.Id == id);

            StorageItem storageItem = _storageItemRepository.GetQueryable<StorageItem>()
                .Include(x => x.StorageInputOutputList)
                .Where(x => x.ProductId == orderItem.ProductId).FirstOrDefault();

           

            if (orderItem == null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Order item ne posotji";
                return result;
            }
            var isDeleted = order.OrderItemList.Remove(orderItem);
            if (isDeleted == false)
            {
                result.Value = isDeleted;
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Couldn't delete order";
                return result;
            }

            order.OrderItemList.All(x => { x.PricePerUnit = x.Product.Price; return true; });

            order.GetTotalPrice();
            var isOrderDeleted = false;
            
            if (order.OrderItemList.Count ==0)
            {
                isOrderDeleted = await  _orderRepository.Delete(order);

            }

            if (storageItem == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Item nije pronadjen";
                return result;
            }


            StorageInputOutput storageInputOutput = storageItem.StorageInputOutputList.FindLast(x => x.StorageItemId == storageItem.Id && x.StorageInputOutputType == StorageInputOutputType.Output && x.CreatedBy == 2);
            storageItem.StorageInputOutputList.Remove(storageInputOutput);

            storageItem.GetStock();


            _unitOfWork.commit();
            result.ErrorMessage = null;
            result.StatusCode = StatusCodes.Status200OK;
            result.Value = isDeleted;
            return result;


        }
    }
}
