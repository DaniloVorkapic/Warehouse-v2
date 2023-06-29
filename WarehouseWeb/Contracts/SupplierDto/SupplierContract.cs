using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseWeb.Contracts
{
    public class SupplierContract
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
       // public DateTime CreateDate { get; set; }
      //  public DateTime ModifyDate { get; set; }
        public string Email { get; set; }
    }
}
