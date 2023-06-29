using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseWeb.Model
{
    public class StorageItem : CoreObject
    {
        public long? StorageId { get; set; }
        public Storage Storage { get; set; }
        public int Serialnumber { get; set; }
        public long ProductId { get; set; }
        public Product Product { get; set; }
        public Quantity Quantity { get; set; }
        public List<StorageInputOutput> StorageInputOutputList { get; set; }

        public void  GetStock ()
        {
            long numberOfInputs = StorageInputOutputList.Where(x => x.StorageInputOutputType == StorageInputOutputType.Input)
                                                        .Sum(x => x.Quantity.Amount);
            long numberOfOutput = StorageInputOutputList.Where(x => x.StorageInputOutputType == StorageInputOutputType.Output)
                                                        .Sum(x => x.Quantity.Amount);
            Quantity.Amount = numberOfInputs- numberOfOutput;
        }

    }
}
