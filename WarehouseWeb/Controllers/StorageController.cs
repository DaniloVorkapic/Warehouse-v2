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
    public class StorageController : BaseController
    {
        private readonly IStorageService _storageService;
        public StorageController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [HttpGet]

        public async Task<ActionResult<Result<IEnumerable<Storage>>>> GetAllStorages()
        {
            Result r = await _storageService.GetAllStorages();
            return GetReturnResultByStatusCode(r);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<Result<Storage>>> GetStorageById(long id)
        {
            Result r = await _storageService.GetStorageById(id);
            return GetReturnResultByStatusCode(r);
        }
       
        [HttpPost]

        public async Task<ActionResult<Result<bool>>> AddStorage(StorageContract storageContract)
        {
            Result r = await _storageService.AddStorage(storageContract);
            return GetReturnResultByStatusCode(r);
        }

        [HttpPost("AddStorageItem")]
        public async Task<ActionResult<Result<bool>>> AddStorageItem([FromBody] StorageItemContract sic)
        {
            Result r = await _storageService.AddStorageItem(sic);
            return GetReturnResultByStatusCode(r);
        }
        //[HttpPut("{storageId}")]

        //public async Task<ActionResult<Result<bool>>> UpdateStorage(long storageId, StorageContract storageContract)
        //{

        //    Result r = await _storageService.UpdateStorage(storageId, storageContract);
        //    return GetReturnResultByStatusCode(r);
        //}

        [HttpPut("{storageItemId}")]

        public async Task<ActionResult<Result<bool>>> UpdateStorageItem(long storageItemId, StorageItemContract storageItemContract)
        {
            Result r = await _storageService.UpdateStorageItem(storageItemId, storageItemContract);
            return GetReturnResultByStatusCode(r);
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult<Result<bool>>> DeleteStorage(long id)
        {
            // var orderItemToDelete =  _orderItemService.GetOrderItemById(id);
            Result r = await _storageService.DeleteStorage(id);
            return GetReturnResultByStatusCode(r);
        }

    }
}
