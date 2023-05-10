using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Contracts;
using WarehouseWeb.Model;

namespace WarehouseWeb.Services
{
   public interface IProductService 
    {
            Task<Result> GetAllProducts();
            Task<Result> GetProductById(long id);
            Task<Result> AddProduct(ProductContract pc );
            Task<Result> UpdateProduct(long id, ProductContract pc);
            Task<Result> DeleteProduct(long id);

    }
}
