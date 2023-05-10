using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Contracts;
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

        public async Task<ActionResult<Result<IEnumerable<Supplier>>>> GetAllSuppliers()
        {
            Result r = await _supplierService.GetAllSuppliers();
            return GetReturnResultByStatusCode(r);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<Result<Supplier>>> GetSupplierById(long id)
        { 
            Result r = await _supplierService.GetSupplierById(id);
            return GetReturnResultByStatusCode(r);
        }

        [HttpPost]

        public async Task<ActionResult<Result<bool>>> AddSuplier(SupplierContract sc)
        {          
            Result r = await _supplierService.AddSupplier(sc);
            return GetReturnResultByStatusCode(r);  
        }

        [HttpPut("{id}")]

        public async Task<ActionResult<Result<bool>>> UpdateSupplier(long id, SupplierContract sc)
        { 
            Result r = await _supplierService.UpdateSupplier(id, sc);
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
