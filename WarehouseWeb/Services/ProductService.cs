using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Contracts;
using WarehouseWeb.Contracts.ClassificationValues;
using WarehouseWeb.Contracts.ProductDto;
using WarehouseWeb.Data;
using WarehouseWeb.Extensions;
using WarehouseWeb.Model;
using WarehouseWeb.Repositories;

namespace WarehouseWeb.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<ClassificationValues> _productCategoryRepository;

        public ProductService(IUnitOfWork unitOfWork, IGenericRepository<Product> productRepository,IGenericRepository<ClassificationValues> productCategoryRepository)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
        }

       


        public async Task<Result> GetAllProducts(InputProductDto input)
        {

            int totalCount = 0;
           
            var predicate = PredicateBuilder.True<Product>();
            var categoriesPredicate = PredicateBuilder.True<Product>();

            if (!string.IsNullOrWhiteSpace(input.Search))
            {
                predicate = predicate.And(x => x.Name.ToLower().Contains(input.Search.ToLower()));

            }

            var statusCode = StatusCodes.Status200OK;
            var allProducts = _productRepository.GetQueryable<Product>()
                .Where(predicate);

            if (input.classificationValuesIdList.Count() > 0 )
            {
                categoriesPredicate = categoriesPredicate.And(x => input.classificationValuesIdList.Any(y => y == x.ClassificationValuesId));

                allProducts = allProducts.Where(categoriesPredicate);
            

            }

           
            
             allProducts = allProducts
             .CountOut(out totalCount)
             .OrderByString(input.sortBy, input.sortValue);


            var productsResponse = allProducts
                .Skip((input.pageNumber - 1) * input.pageSize)
                .Take(input.pageSize)
                .Select(x => new GetAllProductsResponse(x.Id, x.Price, x.Name))
                .ToList();

           


            var result = Result.Create(productsResponse, statusCode,null,totalCount);
                return result;
            
        }

        public async Task<Result> GetAllProductCategories(InputProductCategoriesDto input)
        {
            var predicate = PredicateBuilder.True<ClassificationValues>();

            if(!string.IsNullOrWhiteSpace(input.Search))
            {
                predicate = predicate.And(x => x.Name.ToLower().Contains(input.Search.ToLower()));

            }

            var statusCode = StatusCodes.Status200OK;
            var allPorductCategories = _productCategoryRepository.GetQueryable<ClassificationValues>()
                                        .Where(x=> x.ClassificationSpecification.Name == "Product")
                                        .Where(predicate)
                                       // .Where(x => input.classificationValuesIdList.Any(y => y == x.Id))
                                        .OrderBy(x => x.Name)
                                        .Select(x => new GetAllProductCategoriesResponse(x.Name,x.Id,x.ClassifictionSpecificationId))
                                        .ToList();

            var result = Result.Create(allPorductCategories, statusCode, null, 0);
            return result;
            

        }

        public async Task<Result> AddProduct(ProductContract pc)
        {
           
                var statusCode = StatusCodes.Status500InternalServerError;
                var errorMessage = "Greska";
                var result = Result.Create(null,statusCode,errorMessage,0);

                if (pc == null)
                {
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    result.ErrorMessage = "Netacni ulazni parametri";
                    return result;
                }

                var product = new Product
                {
                    Price = pc.Price,
                    Name = pc.Name,
                    ClassificationValuesId = pc.ClassificationValuesId,
                    SupplierId = pc.SupplierId,
                };

                var isAdded = await _productRepository.AddEntity(product);
                if(isAdded == false)
            {
                result.Value = isAdded;
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Couldn't add product";
                return result;
            }

                _unitOfWork.commit();
                result.StatusCode = StatusCodes.Status200OK;
                result.ErrorMessage = null;
                result.Value = isAdded;  
                return result;
           
        }
       
        public async Task<Result> DeleteProduct(long id)
        {
            try
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var errorMessage = "Greska";
                var result = Result.Create(null, statusCode,errorMessage,0);
                var product = await _productRepository.GetById(id);

                if (product == null)
                {
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    result.ErrorMessage = "Ne postoji proizvod";
                    return result;
                }
                //ako postoji u storageItem ne birisi ga i vrati neki komentar
                 var isDeleted = await _productRepository.Delete(product);
                if(isDeleted == false)
                {
                    result.Value = isDeleted;
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    result.ErrorMessage = "Couldn't delete product";
                    return result;
                }
                _unitOfWork.commit();
                result.ErrorMessage = null;
                result.Value = isDeleted;
                result.StatusCode = StatusCodes.Status200OK;
                return result;
            }


            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<Result> GetProductById(long id)
        {
          
                var statusCode = StatusCodes.Status500InternalServerError;
                var errorMessage = "Greska";
                var result = Result.Create(null, statusCode,errorMessage,0);

                var product = _productRepository.GetQueryable<Product>()
                    .Where(x => x.Id == id)
                    .Select(x => new GetProductByIdResponse(x.Id, x.Price, x.Name))
                    .FirstOrDefault();
                    
                if (product == null)
                {
                    statusCode = StatusCodes.Status400BadRequest;
                    result.ErrorMessage = "Proivod nije pronadjen";
                    return result;
                }
                 
                 result.StatusCode = StatusCodes.Status200OK;
                 result.Value = product;
                 result.ErrorMessage = null;
                
                return result;
          
        }

        public async Task<Result> UpdateProduct(ProductContract pc)
        {
            
                var statusCode = StatusCodes.Status500InternalServerError;
            var errorMessage = "Greska";
            var result = Result.Create(null, statusCode, errorMessage,0);
            var product = await _productRepository.GetById(pc.Id);

            if (product == null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Proizvod nije pronadjen";
                return result;
            }
            product.Id = pc.Id;
            product.Price = pc.Price;
            product.ClassificationValuesId = pc.ClassificationValuesId;
            product.Name = pc.Name;
            product.SupplierId = pc.SupplierId;


            var isUpdated = await _productRepository.UpdateEntity(product);
            if(isUpdated == false)
            {
                result.Value = isUpdated;
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Couldn't update a product";
                return result;
            }

            _unitOfWork.commit();
            result.ErrorMessage = null;
            result.Value = isUpdated;
            result.StatusCode = StatusCodes.Status200OK;
            return result;


        }
    }
}
