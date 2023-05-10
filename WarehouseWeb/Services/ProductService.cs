using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Contracts;
using WarehouseWeb.Data;
using WarehouseWeb.Model;
using WarehouseWeb.Repositories;

namespace WarehouseWeb.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Product> _productRepository;
        
        public ProductService(IUnitOfWork unitOfWork, IGenericRepository<Product> productRepository)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
        }

        public async Task<Result> GetAllProducts()
        {

            try
            {
                var statusCode = StatusCodes.Status200OK;
                var allProducts = await _productRepository.GetAll();
                var result = Result.Create(allProducts,statusCode);
                return result;
            }
            catch (Exception)
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null,statusCode);
                return result;
            }
        }

        public async Task<Result> AddProduct(ProductContract pc)
        {
            try
            {
                var statusCode = StatusCodes.Status200OK;
                var result = Result.Create(null,0);

                if (pc == null)
                {
                    statusCode = StatusCodes.Status400BadRequest;
                    result = Result.Create(null, statusCode);
                    return result;
                }
             
                var product = new Product
                {
                    Price = pc.Price,
                    Description = pc.Description,
                    ClassificationValuesId = pc.ClassificationValuesId,
                    CreateDate = pc.CreateDate,
                };

                var addProduct = await _productRepository.AddEntity(product);
                _unitOfWork.commit();
                result = Result.Create(addProduct, statusCode);
                return result;
            }
                catch (Exception)
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode);
                return result;
            }
        }
       
        public async Task<Result> DeleteProduct(long id)
        {
            try
            {
                var statusCode = StatusCodes.Status200OK;
                var result = Result.Create(null, 0);
                var product = await _productRepository.GetById(id);

                if (product == null)
                {
                    statusCode = StatusCodes.Status400BadRequest;
                    result = Result.Create(null, statusCode);
                    return result;
                }

                 var deleteProduct = await _productRepository.Delete(product);
                _unitOfWork.commit();
                result = Result.Create(deleteProduct, statusCode);
                return result;
            }
            catch (Exception)
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode);
                return result;
            }
        }

        public async Task<Result> GetProductById(long id)
        {
            try
            {
                var statusCode = StatusCodes.Status200OK;
                var result = Result.Create(null, 0);

                var product = await _productRepository.GetById(id);
                if (product == null)
                {
                    statusCode = StatusCodes.Status400BadRequest;
                    result = Result.Create(null, statusCode);
                    return result;
                }
                
                result = Result.Create(product, statusCode);
                return result;
            }
            catch (Exception)
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode);
                return result;
            }
        }

        public async Task<Result> UpdateProduct(long id, ProductContract pc)
        {
            try
            {
                var statusCode = StatusCodes.Status200OK;
                var result = Result.Create(null, 0);
                var product = await _productRepository.GetById(id);

                if (product == null)
                {
                    statusCode = StatusCodes.Status400BadRequest;
                    result = Result.Create(null, statusCode);
                    return result;
                }
                product.Id = pc.Id;
                product.Price = pc.Price;


               var updateProduct = await _productRepository.UpdateEntity(id, product);
                _unitOfWork.commit();
                result = Result.Create(updateProduct, statusCode);
                return result;

            }
            catch (Exception)
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode);
                return result;
            }
        }
    }
}
