using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Contracts;

namespace WarehouseWeb.Services
{
    public interface IStorageService
    {
        Task<Result> GetAllStorages();
        Task<Result> GetStorageById(long id);
        Task<Result> AddStorage(StorageContract storageContract);
        Task<Result> AddStorageItem(StorageItemContract storageItemContract);
        Task<Result> UpdateStorage(long storgeId, StorageContract storageContract);
        Task<Result> UpdateStorageItem(long StorageItemId, StorageItemContract storageItemContract);
        Task<Result> DeleteStorage(long id);
    }
}
