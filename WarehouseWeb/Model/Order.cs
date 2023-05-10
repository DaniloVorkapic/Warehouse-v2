using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Model.Enums;

namespace WarehouseWeb.Model
{
    public class Order:CoreObject
    {
        public double TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public Status Status { get; set; }
        public long CustomerId { get; set; }
        public Customer Customer { get; set; }
        public List<OrderItem> OrderItemList { get; set; }
    }
}
