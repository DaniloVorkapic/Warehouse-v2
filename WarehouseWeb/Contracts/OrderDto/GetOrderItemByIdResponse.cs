using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseWeb.Contracts.OrderDto
{
    public class GetOrderItemByIdResponse
    {
        public GetOrderItemByIdResponse(long id, long orderId, long productId)
        {
            this.Id = id;
            this.OrderId = orderId;
            this.ProductId = productId;
            
        }

        public long Id { get; set; }
        public long OrderId { get; set; }
        public long ProductId { get; set; }
       
    }
}
