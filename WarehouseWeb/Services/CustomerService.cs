using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Contracts;
using WarehouseWeb.Contracts.CustomerDto;
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
                var allCustomers = _customerRepository.GetQueryable<Customer>()
                    .Select(x => new GetAllCustomersResponse(x.Id, x.FirstName, x.LastName))
                    .ToList();
                    
                var result = Result.Create(allCustomers, statusCode,null,0);
                return result;
            }
            catch (Exception)
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode,"Greska",0);
                return result;
            }
        }
        public async Task<Result> AddCustomer(CustomerContract cc)
        {
            try
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode,"Greska",0);

                if (cc == null)
                {
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    result.ErrorMessage = "Losi ulazni parametri";
                    return result;
                }
                var customer = new Customer
                {
                    //product.Id = pc.Id;
                    //     Price = pc.Price,
                    //Description = pc.Description,
                    //ClassificationValuesId = pc.ClassificationValuesId,

                    //CreateDate = pc.CreateDate,
                    //ModifyDate = pc.ModifyDate
                };
                var addProduct = await _customerRepository.AddEntity(customer);
                result.ErrorMessage = null;
                result.Value = addProduct;
                result.StatusCode = StatusCodes.Status200OK;
                _unitOfWork.commit();
                return result;
            }
            catch (Exception)
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode,"Greska",0);
                return result;
            }
        }

        public async Task<Result> DeleteCustomer(long id)
        {
            try
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                
                var result = Result.Create(null, statusCode,null,0);
                var customer = _customerRepository.GetById(id);

                if(customer == null)
                {
                    result.StatusCode = StatusCodes.Status400BadRequest;
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
                var result = Result.Create(null, statusCode,"Greska",0);
                return result;
            }
        }

        public async Task<Result> GetCustomerById(long id)
        {
            try
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode,null,0);
                var customer = _customerRepository.GetQueryable<Customer>()
                    .Where(x => x.Id == id)
                    .Select(x => new GetCustomerByIdResponse(x.Id, x.FirstName, x.LastName))
                    .FirstOrDefault();

                if(customer == null)
                {
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    result.ErrorMessage = "Customer nije pronadjen";
                    return result;
                }
                result.StatusCode = StatusCodes.Status200OK;
                result.Value = customer;
                return result;   
            }
            catch (Exception ex)
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode,"Greska",0);
                return result;
            }
        }
        public async Task<Result> UpdateCustomer(CustomerContract cc)
        {
            try
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode,null,0);
                var customer = await _customerRepository.GetById(cc.Id);

                if (customer == null)
                {
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    result.ErrorMessage = "Customer nije pronadjen";
                    return result;
                }

                customer.Id = cc.Id;
                customer.Email = cc.Email;
                customer.PhoneNumber = cc.PhoneNumber;
                customer.CompanyId = cc.CompanyId;

                var updateProduct = await _customerRepository.UpdateEntity(customer);
                result.Value = updateProduct;
                result.StatusCode = StatusCodes.Status200OK;
                _unitOfWork.commit();
                return result;
            }
            catch (Exception)
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode,"Greska",0);
                return result;
            }
        }

        
    }
}



