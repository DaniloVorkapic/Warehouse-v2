using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Model;

namespace WarehouseWeb.Contracts
{
    public class StorageItemContract
    {
        public long Id { get; set; }
        public long StorageId { get; set; }
        public long ProductId { get; set; }
       // public long SupplierId { get; set; }
        public Quantity Quantity { get; set; }
      //  public DateTime CreateDate { get; set; }
      //  public DateTime ModifyDate { get; set; }
       // public List<StorageInputOutput> storageInputOutputList { get; set; }
    }

    }

