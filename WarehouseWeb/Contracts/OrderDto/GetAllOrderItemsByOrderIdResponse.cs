using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseWeb.Contracts.OrderDto
{
    public class GetAllOrderItemsByOrderIdResponse
    {
        public GetAllOrderItemsByOrderIdResponse(long id, long orderId, long productId ,double price)
        {
            this.Id = id;
            this.OrderId = orderId;
            this.ProductId = productId;
            this.Price = price;
            

        }
        public long Id { get; set; }
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public double Price { get; set; }

    }
}
