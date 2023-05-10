using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Model;

namespace WarehouseWeb.Contracts
{
    public class OrderItemContract
    {
        public long Id { get; set; }
      //  public long OrderId { get; set; }
        public long SerialNumber { get; set; }
        public double PricePerUnit { get; set; }
        public long ProductId { get; set; }
        public Quantity  Quantity { get; set; } 
    }
}
