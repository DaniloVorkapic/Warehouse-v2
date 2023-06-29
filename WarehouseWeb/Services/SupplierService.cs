using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Contracts;
using WarehouseWeb.Contracts.SupplierDto;
using WarehouseWeb.Model;
using WarehouseWeb.Repositories;

namespace WarehouseWeb.Services
{
    public class SupplierService:ISupplierService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Supplier> _supplierRepository;

        public SupplierService(IUnitOfWork unitOfWork, IGenericRepository<Supplier> supplierRepository)
        {
            _unitOfWork = unitOfWork;
            _supplierRepository = supplierRepository;
        }

        public async Task<Result> AddSupplier(SupplierContract sc)
        {
            
                var statusCode = StatusCodes.Status500InternalServerError;
                var errorMessage = "Greska";
                var result = Result.Create(null, statusCode,errorMessage,0);
                if (sc == null)
                {
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    result.ErrorMessage = "Ulazni Parametri losi";
                    return result;
                }
                var supplier = new Supplier
                {
                    Name = sc.Name,
                    City = sc.City,
                    Email = sc.Email,
                };

               var isAdded =  await _supplierRepository.AddEntity(supplier);
                if(isAdded == false)
            {
                result.Value = isAdded;
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Couldn't add supplier";
                return result;
            }
                _unitOfWork.commit();
                result.StatusCode = StatusCodes.Status200OK;
                result.ErrorMessage = null; 
                result.Value = isAdded;
                return result;
            
        }

        public async Task<Result> DeleteSupplier(long id)
        {
           
                var statusCode = StatusCodes.Status500InternalServerError;
                var errorMessage = "Greska";
                var result = Result.Create(null, statusCode,errorMessage,0);
                var supplier = await _supplierRepository.GetById(id);
                if (supplier == null)
                {
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    result.ErrorMessage = "Dobavljac nije pronadjen";
                    return result;
                }
               var isDeleted =  await _supplierRepository.Delete(supplier);
                if(isDeleted == false)
            {
                result.Value = isDeleted;
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Coudn't delete supplier";
                return result;
            }
                _unitOfWork.commit();
                result.StatusCode = StatusCodes.Status200OK;
                result.ErrorMessage = null;
                result.Value = isDeleted;
                return result;

            
        }

        public async Task<Result> GetAllSuppliers(InputSupplierDto input)
        {
            
                var statusCode = StatusCodes.Status200OK;
                var allSuppliers = _supplierRepository.GetQueryable<Supplier>()
                    .Select(x => new GetAllSuppliersResponse(x.Id, x.Name, x.City))
                    .Skip((input.pageNumber -1)* input.pageSize)
                    .Take(input.pageSize)
                    .ToList();
                var result = Result.Create(allSuppliers, statusCode,null,0);
                return result;
            
          

        }

        public async Task<Result> GetSupplierById(long id)
        {
            
                var statusCode = StatusCodes.Status500InternalServerError;
                var errorMessage = "Greska";
                var result = Result.Create(null, statusCode,errorMessage,0);
                var supplier =  _supplierRepository.GetQueryable<Supplier>()
                    .Where(x => x.Id == id)
                    .Select(x => new GetSupplierByIdResponse(x.Id, x.Name, x.City))
                    .FirstOrDefault();

                if (supplier == null)
                {
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    result.ErrorMessage = "Dobavljac nije pronadjen";
                    return result;
                }
                result.StatusCode = StatusCodes.Status200OK;
                result.ErrorMessage = null;
                result.Value = supplier;
                
                return result;
            
          
        }

        public async Task<Result> UpdateSupplier(SupplierContract sc)
        {
            
                var statusCode = StatusCodes.Status500InternalServerError;
                var errorMessage = "Greska";
                var result = Result.Create(null, statusCode,errorMessage,0);

                var supplier = await _supplierRepository.GetById(sc.Id);
                if (supplier == null)
                {
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    result.ErrorMessage = "Dobavljac nije pronadjen";
                    return result;
                }

                supplier.Id = sc.Id;
                supplier.Name= sc.Name;
                supplier.City = sc.City;
                supplier.Email = sc.Email;

                var isUpdated = await _supplierRepository.UpdateEntity( supplier);
                if(isUpdated == false)
            {
                result.Value = isUpdated;
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Couldn't uppdate supplier";
            }
                _unitOfWork.commit();
                result.StatusCode = StatusCodes.Status200OK;
                result.ErrorMessage = null;
                result.Value = isUpdated;
                return result;
           
        }

    }
}
