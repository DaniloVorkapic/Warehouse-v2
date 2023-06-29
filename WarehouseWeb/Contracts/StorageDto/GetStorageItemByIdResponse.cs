using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseWeb.Contracts.StorageDto
{
    public class GetStorageItemByIdResponse
    {

        public GetStorageItemByIdResponse(long id,long? storageId, long productId)
        {
            this.Id = id;
            this.ProductId = productId;
            this.StorageId = storageId;
         
        }
        public long Id { get; set; }
        public long? StorageId { get; set; }
        public long ProductId { get; set; }
        
    }
}
