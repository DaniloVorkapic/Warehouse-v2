using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Contracts;
using WarehouseWeb.Model;
using WarehouseWeb.Services;

namespace WarehouseWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : BaseController
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }
      

        [HttpGet]

        public async Task<ActionResult<Result<IEnumerable<OrderItem>>>> GetAllOrderItems()
        {
            Result r = await _orderItemService.GetAllOrderItems();
            return GetReturnResultByStatusCode(r);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<Result<OrderItem>>> GetOrderItemById(long id)
        {
            Result r = await _orderItemService.GetOrderItemById(id);
            return GetReturnResultByStatusCode(r);
            
        }


        [HttpPost]

        public async Task<ActionResult<Result<bool>>> AddOrderItem(OrderItemContract oic)
        {
           
            Result r = await _orderItemService.AddOrderItem(oic);
            return GetReturnResultByStatusCode(r);
           

        }

        [HttpPut("{id}")]

        public async Task<ActionResult<Result<bool>>> UpdateOrderItem(OrderItemContract oi)
        {
            
            Result r = await _orderItemService.UpdateOrderItem(oi);
            return GetReturnResultByStatusCode(r);
        }


        [HttpDelete("{id}")]

        public async Task<ActionResult<Result<bool>>> DeleteOrderItem(long id)
        {
            // var orderItemToDelete =  _orderItemService.GetOrderItemById(id);
            Result r = await _orderItemService.DeleteOrderItem(id);
            return GetReturnResultByStatusCode(r);
            
        }

    }
}
