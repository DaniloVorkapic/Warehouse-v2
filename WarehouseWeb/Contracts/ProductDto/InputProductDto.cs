using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseWeb.Contracts.ProductDto
{
    public class InputProductDto:PaginatorDto
    {
        public string Search { get; set; }
        public string sortBy { get; set; }
        public string sortValue { get; set; }
        public long[] classificationValuesIdList { get; set; }




    }
}
