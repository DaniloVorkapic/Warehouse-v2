using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Contracts;

namespace WarehouseWeb.Model
{
    public class Product:CoreObject
    {
        public string Description { get; set; }
        public double Price { get; set; }
        public long ClassificationValuesId { get; set; }
        public ClassificationValues? ClassificationValues { get; set; }
    }
}
