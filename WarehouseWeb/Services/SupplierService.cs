using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Contracts;
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
            try
            {
                var statusCode = StatusCodes.Status200OK;
                var result = Result.Create(null, 0);
                if (sc == null)
                {
                    statusCode = StatusCodes.Status400BadRequest;
                    result = Result.Create(null, statusCode);
                    return result;
                }
                var supplier = new Supplier
                {
                    Name = sc.Name,
                    City = sc.City,
                    Email = sc.Email,
                };

               var addSupplier =  await _supplierRepository.AddEntity(supplier);
                result = Result.Create(addSupplier, statusCode);
                _unitOfWork.commit();
                return result;
            }
            catch (Exception)
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode);
                return result;
            }
        }

        public async Task<Result> DeleteSupplier(long id)
        {
            try
            {
                var statusCode = StatusCodes.Status200OK;
                var result = Result.Create(null, 0);
                var supplier = await _supplierRepository.GetById(id);
                if (supplier == null)
                {
                    statusCode = StatusCodes.Status400BadRequest;
                    result = Result.Create(null, statusCode);
                    return result;
                }
               var deleteSupplier =  _supplierRepository.Delete(supplier);
                _unitOfWork.commit();
                result = Result.Create(deleteSupplier, statusCode);
                return result;

            }
            catch (Exception)
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode);
                return result;
            }
        }

        public async Task<Result> GetAllSuppliers()
        {
            try
            {
                var statusCode = StatusCodes.Status200OK;
                var allSuppliers = await _supplierRepository.GetAll();
                var result = Result.Create(allSuppliers, statusCode);
                return result;
            }
            catch (Exception)
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode);
                return result;
            }

        }

        public async Task<Result> GetSupplierById(long id)
        {
            try
            {
                var statusCode = StatusCodes.Status200OK;
                var result = Result.Create(null, 0);
                var supplier = await _supplierRepository.GetById(id);
                if (supplier == null)
                {
                    statusCode = StatusCodes.Status400BadRequest;
                    result = Result.Create(null, statusCode);
                    return result;
                }

                result = Result.Create(supplier, statusCode);
                return result;
            }
            catch (Exception)
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode);
                return result;
            }
        }

        public async Task<Result> UpdateSupplier(long id, SupplierContract sc)
        {
            try
            {
                var statusCode = StatusCodes.Status200OK;
                var result = Result.Create(null, 0);

                var supplier = await _supplierRepository.GetById(id);
                if (supplier == null)
                {
                    statusCode = StatusCodes.Status400BadRequest;
                    result = Result.Create(null, statusCode);
                    return result;
                }

                supplier.Id = sc.Id;
                supplier.Name= sc.Name;
                supplier.City = sc.City;
                supplier.Email = sc.Email;

                var updateSupplier = await _supplierRepository.UpdateEntity(id, supplier);
                _unitOfWork.commit();
                result = Result.Create(updateSupplier, statusCode);
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
