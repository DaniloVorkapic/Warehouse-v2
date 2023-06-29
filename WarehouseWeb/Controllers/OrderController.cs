using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Contracts;
using WarehouseWeb.Data;
using WarehouseWeb.Model;
using WarehouseWeb.Services;

namespace WarehouseWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : BaseController 
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Route("api/controller/GetAllOrders")]

        public async Task<ActionResult<Result<IEnumerable<Order>>>> GetAllOrders()
        {
            Result r = await _orderService.GetAllOrders();
            return GetReturnResultByStatusCode(r);
        }
        [HttpGet]
        [Route("api/controller/GetAllOrderItemsByOrderId")]

        public async Task<ActionResult<Result<IEnumerable<Order>>>> GetAllOrderItemsByOrderId(long orderId)
        {
            Result r = await _orderService.GetAllOrderItemsByOrderId(orderId);
            return GetReturnResultByStatusCode(r);
        }
        [HttpGet]
        [Route("api/controller/GetOrderItemById")]
        public async Task<ActionResult<Result<IEnumerable<Order>>>> GetOrderItemById(long id)
        {
            Result r = await _orderService.GetOrderItemById(id);
            return GetReturnResultByStatusCode(r);
        }


        [HttpGet("{id}")]

        public async Task<ActionResult<Result<Order>>> GetOrderById(long id)
        {
            Result r = await  _orderService.GetOrderById(id);
            return GetReturnResultByStatusCode(r);
        }
        [HttpPost]
        [Route("api/controller/CreateOrder")]

        public async Task<ActionResult<Result<bool>>> CreateOrder(OrderItemContract oc)
        {
            Result r = await _orderService.CreateOrder(oc);
            return GetReturnResultByStatusCode(r);
        }

        //[HttpPost]
        //[Route("api/controller/AddOrder")]

        //public async Task<ActionResult<Result<bool>>> AddOrderWithOrderItem(OrderItemContract oc)
        //{           
        //    Result r = await _orderService.AddOrderWithOrderItem(oc);
        //    return GetReturnResultByStatusCode(r);
        //}

        //[HttpPost]
        //[Route("api/controller/AddOrderItem")]

        //public async Task<ActionResult<Result<bool>>> AddOrderItem(OrderItemContract oc)
        //{
        //    Result r = await _orderService.AddOrderItem(oc);
        //    return GetReturnResultByStatusCode(r);
        //}


        [HttpPut]
        [Route("api/controller/UpdateOrder")]

        public async Task<ActionResult<Result<bool>>> UpdateOrder(OrderContract oc)
        {         
            Result r = await _orderService.UpdateOrder( oc);
            return GetReturnResultByStatusCode(r);
        }

        [HttpPut]
        [Route("api/controller/UpdateOrderItem")]

        public async Task<ActionResult<Result<bool>>> UpdateOrderItem(OrderItemContract oc )
        {
            Result r = await _orderService.UpdateOrderItem(oc);
            return GetReturnResultByStatusCode(r);
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult<Result<bool>>> DeleteOrder(long id)
        {
            // var orderItemToDelete =  _orderItemService.GetOrderItemById(id);
            Result r = await _orderService.DeleteOrder(id);
            return GetReturnResultByStatusCode(r);
            
        }
        [HttpDelete]
        [Route("api/controller/DeleteOrderitem")]
        public async Task<ActionResult<Result<bool>>> DeleteOrderitem(long id)
        {
            // var orderItemToDelete =  _orderItemService.GetOrderItemById(id);
            Result r = await _orderService.DeleteOrderitem(id);
            return GetReturnResultByStatusCode(r);

        }
    }
}
