using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Model;

namespace WarehouseWeb.Contracts
{
    public class ProductContract
    {
        public long Id { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
        public long ClassificationValuesId { get; set; }
        public long SupplierId { get; set; }
        // public DateTime CreateDate { get; set; }
        //  public DateTime ModifyDate { get; set; }

    }
}
