using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseWeb.Contracts.OrderDto
{
    public class GetAllOrdersResponse
    {
        public GetAllOrdersResponse(long id, double totalPrice, long customerId)
        {

            this.Id = id;
            this.TotalPrice = totalPrice;
            this.CustomerId = customerId;

        }
        public long Id { get; set; }
        public double TotalPrice { get; set; }
        public long CustomerId { get; set; }

    }
}
