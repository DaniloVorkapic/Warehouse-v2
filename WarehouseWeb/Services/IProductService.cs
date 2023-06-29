using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Contracts;
using WarehouseWeb.Contracts.ClassificationValues;
using WarehouseWeb.Contracts.ProductDto;
using WarehouseWeb.Model;

namespace WarehouseWeb.Services
{
   public interface IProductService 
    {
            Task<Result> GetAllProducts(InputProductDto input);
            Task<Result> GetAllProductCategories(InputProductCategoriesDto input);
            Task<Result> GetProductById(long id);
            Task<Result> AddProduct(ProductContract pc );
            Task<Result> UpdateProduct( ProductContract pc);
            Task<Result> DeleteProduct(long id);

    }
}
