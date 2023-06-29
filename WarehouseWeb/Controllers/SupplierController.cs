using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Contracts;
using WarehouseWeb.Contracts.SupplierDto;
using WarehouseWeb.Model;
using WarehouseWeb.Services;

namespace WarehouseWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : BaseController
    {
        private readonly ISupplierService _supplierService;
        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet]
        [Route("api/controller/GetAllSuppliers")]
        public async Task<ActionResult<Result<IEnumerable<Supplier>>>> GetAllSuppliers(int pageNumber, int pageSize)
        {
            InputSupplierDto input = new InputSupplierDto
            {
                pageNumber = pageNumber,
                pageSize = pageSize
            };

            Result r = await _supplierService.GetAllSuppliers(input);
            return GetReturnResultByStatusCode(r);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<Result<Supplier>>> GetSupplierById(long id)
        { 
            Result r = await _supplierService.GetSupplierById(id);
            return GetReturnResultByStatusCode(r);
        }

        [HttpPost]
        [Route("api/controller/AddSuplier")]

        public async Task<ActionResult<Result<bool>>> AddSuplier(SupplierContract sc)
        {          
            Result r = await _supplierService.AddSupplier(sc);
            return GetReturnResultByStatusCode(r);  
        }

        [HttpPut]
        [Route("api/controller/UpdateSupplier")]

        public async Task<ActionResult<Result<bool>>> UpdateSupplier( SupplierContract sc)
        { 
            Result r = await _supplierService.UpdateSupplier( sc);
            return GetReturnResultByStatusCode(r);
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult<Result<bool>>> DeleteSupplier(long id)
        {
            // var orderItemToDelete =  _orderItemService.GetOrderItemById(id);
            Result r = await _supplierService.DeleteSupplier(id);
            return GetReturnResultByStatusCode(r);  
        }


    }
}
