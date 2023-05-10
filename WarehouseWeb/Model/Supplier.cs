using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseWeb.Model
{
    public class Supplier:CoreObject
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
    }
}
