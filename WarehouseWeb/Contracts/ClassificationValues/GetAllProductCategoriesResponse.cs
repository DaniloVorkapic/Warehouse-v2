using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseWeb.Contracts.ClassificationValues
{
    public class GetAllProductCategoriesResponse
    {

        public GetAllProductCategoriesResponse(string name,long id, long clssificationSpecificationId)
        {
            this.Name = name;
            this.Id = id;
            this.ClassificationSpecificationId = clssificationSpecificationId;
        }

        public string Name { get; set; }
        public long Id { get; set; }
        public long ClassificationSpecificationId { get; set; }
    }
}
