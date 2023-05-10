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

        public async Task<ActionResult<Result<IEnumerable<Order>>>> GetAllOrders()
        {
            Result r = await _orderService.GetAllOrders();
            return GetReturnResultByStatusCode(r);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<Result<Order>>> GetOrderById(long id)
        {
            Result r = await  _orderService.GetOrderById(id);
            return GetReturnResultByStatusCode(r);
        }

        [HttpPost]

        public async Task<ActionResult<Result<bool>>> AddOrder(OrderContract oc)
        {           
            Result r = await _orderService.AddOrder(oc);
            return GetReturnResultByStatusCode(r);
        }

        [HttpPut("{id}")]

        public async Task<ActionResult<Result<bool>>> UpdateOrder(long id, OrderContract oc)
        {         
            Result r = await _orderService.UpdateOrder(id, oc);
            return GetReturnResultByStatusCode(r);
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult<Result<bool>>> DeleteOrder(long id)
        {
            // var orderItemToDelete =  _orderItemService.GetOrderItemById(id);
            Result r = await _orderService.DeleteOrder(id);
            return GetReturnResultByStatusCode(r);
            
        }
    }
}
