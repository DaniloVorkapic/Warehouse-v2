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
        [Route("api/controller/GetAllStorages")]

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
        [HttpGet]
        [Route("api/controller/GetStorageItemById")]
        public async Task<ActionResult<Result<Storage>>> GetStorageItemById(long id)
        {
            Result r = await _storageService.GetStorageitemById(id);
            return GetReturnResultByStatusCode(r);
        }
        [HttpGet]
        [Route("api/controller/GetAllStorageItemsByStorageId")]
        public async Task<ActionResult<Result<Storage>>> GetAllStorageItemsByStorageId(long id)
        {
            Result r = await _storageService.GetAllStorageItemsByStorageId(id);
            return GetReturnResultByStatusCode(r);
        }


        [HttpPost]
        [Route("api/controller/AddStorage")]

        public async Task<ActionResult<Result<bool>>> AddStorage(StorageContract storageContract)
        {
            Result r = await _storageService.AddStorage(storageContract);
            return GetReturnResultByStatusCode(r);
        }

        [HttpPost]
        [Route("api/controller/AddStorageItem")]
        public async Task<ActionResult<Result<bool>>> AddStorageItem(StorageItemContract sic)
        {
            Result r = await _storageService.AddStorageItem(sic);
            return GetReturnResultByStatusCode(r);
        }
        [HttpPut]
        [Route("api/controller/UpdateStorage")]

        public async Task<ActionResult<Result<bool>>> UpdateStorage(StorageContract storageContract)
        {

            Result r = await _storageService.UpdateStorage(storageContract);
            return GetReturnResultByStatusCode(r);
        }

        [HttpPut]
        [Route("api/controller/UpdateStorageItem")]

        public async Task<ActionResult<Result<bool>>> UpdateStorageItem( StorageItemContract storageItemContract)
        {
            Result r = await _storageService.UpdateStorageItem( storageItemContract);
            return GetReturnResultByStatusCode(r);
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult<Result<bool>>> DeleteStorage(long id)
        {
            // var orderItemToDelete =  _orderItemService.GetOrderItemById(id);
            Result r = await _storageService.DeleteStorage(id);
            return GetReturnResultByStatusCode(r);
        }
        [HttpDelete]
        [Route("api/controller/DeleteStorageItem")]

        public async Task<ActionResult<Result<bool>>> DeleteStorageItem(long id)
        {
            // var orderItemToDelete =  _orderItemService.GetOrderItemById(id);
            Result r = await _storageService.DeleteStorageItem(id);
            return GetReturnResultByStatusCode(r);
        }

    }
}
