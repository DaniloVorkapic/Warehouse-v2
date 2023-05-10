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
    public class CustomerService:ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Customer> _customerRepository;
        public CustomerService(IUnitOfWork unitOfWork, IGenericRepository<Customer> customerRepository)
        {
            _unitOfWork = unitOfWork;
            _customerRepository = customerRepository;
        }
        public async Task<Result> GetAllCustomers()
        {
            try
            {
                var statusCode = StatusCodes.Status200OK;
                var allCustomers = await _customerRepository.GetAll();
                var result = Result.Create(allCustomers, statusCode);
                return result;
            }
            catch (Exception)
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode);
                return result;
            }
        }
        public async Task<Result> AddCustomer(CustomerContract cc)
        {
            try
            {
                var statusCode = StatusCodes.Status200OK;
                var result = Result.Create(null, 0);

                if (cc == null)
                {
                    statusCode = StatusCodes.Status400BadRequest;
                    result = Result.Create(null, statusCode);
                    return result;
                }
                var customer = new Customer
                {
                    // product.Id = pc.Id;
                    //Price = pc.Price,
                    //Description = pc.Description,
                    //ClassificationValuesId = pc.ClassificationValuesId,

                    //CreateDate = pc.CreateDate,
                    //ModifyDate = pc.ModifyDate
                };
                var addProduct = await _customerRepository.AddEntity(customer);
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

        public async Task<Result> DeleteCustomer(long id)
        {
            try
            {
                var statusCode = StatusCodes.Status200OK;
                var result = Result.Create(null, 0);
                var customer = _customerRepository.GetById(id);

                if(customer == null)
                {
                    statusCode = StatusCodes.Status400BadRequest;
                    result = Result.Create(null, statusCode);
                    return result;
                }
               // var deleteCustomer = await _customerRepository.Delete(customer);
                _unitOfWork.commit();
               // result = Result.Create(deleteCustomer, statusCode);
                return result;
            }
            catch (Exception ex)
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode);
                return result;
            }
        }

        public async Task<Result> GetCustomerById(long id)
        {
            try
            {
                var statusCode = StatusCodes.Status200OK;
                var result = Result.Create(null, 0);
                var customer = await _customerRepository.GetById(id);

                if(customer == null)
                {
                    statusCode = StatusCodes.Status400BadRequest;
                    result = Result.Create(null, statusCode);
                    return result;
                }
                result = Result.Create(customer, statusCode);
                return result;   
            }
            catch (Exception ex)
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode);
                return result;
            }
        }
        public async Task<Result> UpdateCustomer(long id, CustomerContract cc)
        {
            try
            {
                var statusCode = StatusCodes.Status200OK;
                var result = Result.Create(null, 0);
                var customer = await _customerRepository.GetById(id);

                if (customer == null)
                {
                    statusCode = StatusCodes.Status400BadRequest;
                    result = Result.Create(null, statusCode);
                    return result;
                }

                customer.Id = cc.Id;
                customer.Email = cc.Email;
                customer.PhoneNumber = cc.PhoneNumber;
                customer.CompanyId = cc.CompanyId;

                var updateProduct = await _customerRepository.UpdateEntity(id, customer);
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



