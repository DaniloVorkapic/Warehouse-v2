using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseWeb.Contracts.OrderDto
{
    public class GetOrderByIdResponse
    {
        public GetOrderByIdResponse(long id,double totalPrice, long cutomerId)
        {
            this.Id = id;
            this.TotalPrice = totalPrice;
            this.CustomerId = cutomerId;
                
        }


        public long Id { get; set; }
        public double TotalPrice { get; set; }
        public long CustomerId { get; set; }
    }
}
