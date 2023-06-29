using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Contracts;

namespace WarehouseWeb.Model
{
    public class Product:CoreObject
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public long? SupplierId { get; set; }
        public Supplier Supplier { get; set; }
        public long ClassificationValuesId { get; set; }
        public ClassificationValues? ClassificationValues { get; set; }
    }
}
