using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Model;
using WarehouseWeb.Model.Enums;

namespace WarehouseWeb.Contracts
{
    public class OrderContract
    {
       // public long Id { get; set; }
        public Status Status { get; set; }
        public long CustomerId { get; set; }
        public DateTime CreateDate { get; set; }
        public List<OrderItemContract> OrderItemList { get; set; }

    }
}
