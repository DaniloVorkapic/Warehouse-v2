using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseWeb.Contracts.ProductDto
{
    public class GetAllProductsResponse
    {
        public GetAllProductsResponse(long id,double price,string name)
        {
            this.Id = id;
            this.Price = price;
            this.Name = name;
            //this.totalCount = totalCount;

        }

        public long Id { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
      //  public int totalCount { get; set; }

    }
}
