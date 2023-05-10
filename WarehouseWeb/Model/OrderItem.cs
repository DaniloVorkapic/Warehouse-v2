using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseWeb.Model
{
    public class OrderItem:CoreObject   
    {
        public long OrderId { get; set; }
        public Order Order { get; set; }
        public long SerialNumber { get; set; }
        public double PricePerUnit { get; set; }
        public long ProductId { get; set; }
        public Product Product { get; set; }
        public Quantity Quantity { get; set; }
    }
}
