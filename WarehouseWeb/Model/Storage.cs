using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseWeb.Model
{
    public class Storage:CoreObject
    {
        //public long Id { get; set; }
        public string Serialnumber { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public List<StorageItem> StorageItemList { get; set; }
    }
}
