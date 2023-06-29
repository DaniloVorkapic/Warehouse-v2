using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseWeb.Model
{
    public class StorageInputOutput:CoreObject   

    {
       public long? StorageItemId { get; set; }
        public StorageItem StorageItem { get; set; }
        public StorageInputOutputType StorageInputOutputType { get; set; }
        public Quantity Quantity { get; set; }
    }
}
