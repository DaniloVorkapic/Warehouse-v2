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
    public class StorageService : IStorageService
    {
        private readonly IUnitOfWork _unitOfWOrk;
        private readonly IGenericRepository<Storage> _storageRepository;
        private readonly IGenericRepository<StorageItem> _storageItemRepository;
        private readonly IGenericRepository<StorageInputOutput> _storageInputOutputRepository;

        public StorageService(IUnitOfWork unitOfWOrk,IGenericRepository<Storage> storageRepository,IGenericRepository<StorageItem> storageItemRepository,IGenericRepository<StorageInputOutput> storageInputOutputRepository)
        {
            _unitOfWOrk = unitOfWOrk;
            _storageRepository = storageRepository;
            _storageItemRepository = storageItemRepository;
            _storageInputOutputRepository = storageInputOutputRepository;
        }

        public async Task<Result> AddStorage(StorageContract sc)
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

                var storage = new Storage
                {
                    Serialnumber = sc.SerialNumber,
                    Name = sc.Name,
                    City = sc.City,
                   // StorageItemList = storageItemList
                };

                var addedStorage = await _storageRepository.AddEntity(storage);
                _unitOfWOrk.commit();
                result = Result.Create(addedStorage.Id, statusCode);
                return result;
            }
            catch (Exception)
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode);
                return result;
            }
        }

        public async Task<Result> AddStorageItem(StorageItemContract storageItemContract)
        {
            try
            {
                var statusCode = StatusCodes.Status200OK;
                var result = Result.Create(null, 0);

                if (storageItemContract == null)
                {
                    statusCode = StatusCodes.Status400BadRequest;
                    result = Result.Create(null, statusCode);
                    return result;
                }

                var storageInpoutOutput = PrepareStorageInputOutput(storageItemContract);
                List<StorageInputOutput> lista = new List<StorageInputOutput>();
                lista.Add(storageInpoutOutput);
                var storageItem = new StorageItem
                {
                    StorageId = storageItemContract.StorageId,
                    Serialnumber = storageItemContract.SerialNumber,
                    ProductId = storageItemContract.ProductId,
                    SupplierId = storageItemContract.SupplierId,
                    Quantity = storageItemContract.Quantity,
                    StorageInputOutputList = lista 
                };
                
                var addedStorageItem = await _storageItemRepository.AddEntity(storageItem);
                _unitOfWOrk.commit();
                result = Result.Create(addedStorageItem, statusCode);
                return result;
            }
            catch (Exception)
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode);
                return result;
            }
        }

        public StorageInputOutput PrepareStorageInputOutput(StorageItemContract storageItemContract)
        {
                var storageInputOutput = new StorageInputOutput
                {
                    StorageInputOutputType = StorageInputOutputType.Input,
                    Quantity = storageItemContract.Quantity
               };
             
              return storageInputOutput;

        }
        


        public async Task<Result> DeleteStorage(long id)
        {
            try
            {
                var statusCode = StatusCodes.Status200OK;
                var result = Result.Create(null, 0);
                var storage = await _storageRepository.GetById(id);

                if(storage == null)
                {
                    statusCode = StatusCodes.Status400BadRequest;
                    result = Result.Create(null, statusCode);
                    return result;
                }

                var deletedStorage = await _storageRepository.Delete(storage);
                _unitOfWOrk.commit();
                result = Result.Create(deletedStorage, statusCode);
                return result;

            }
            catch (Exception)
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode);
                return result;
            }
        }


        public async Task<Result> GetAllStorages()
        {
            try
            {
                var statusCode = StatusCodes.Status200OK;
                var allStorages = await _storageRepository.GetAll();
                var result = Result.Create(allStorages, statusCode);

                return result;

            }
            catch (Exception)
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode);
                return result;
            } 
        }

        public async Task<Result> GetStorageById(long id)
        {
            try
            {
                var statusCode = StatusCodes.Status200OK;
                var result = Result.Create(null, 0);

                var storage = await _storageRepository.GetById(id);

                if(storage == null)
                {
                    statusCode = StatusCodes.Status400BadRequest;
                    result = Result.Create(null, statusCode);
                    return result;
                }

                result = Result.Create(storage, statusCode);
                return result;
            }
            catch (Exception)
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode);
                return result;
            }
        }
        public async Task<Result> UpdateStorage(long storageId,StorageContract storageContract)
        {
            try
            {
                var statusCode = StatusCodes.Status200OK;
                var result = Result.Create(null, 0);
                var storage = await _storageRepository.GetById(storageId);

                if(storage == null)
                {
                    statusCode = StatusCodes.Status400BadRequest;
                    result = Result.Create(null, statusCode);
                    return result;

                }

                var createDate = storage.CreateDate;
                await _storageRepository.Delete(storage);

                var newStorage = new Storage
                {
                    Serialnumber = storageContract.SerialNumber,
                    Name = storageContract.Name,
                    City = storageContract.City,
                };

                var updatedStorage = _storageRepository.UpdateEntity(storageId, newStorage);
                _unitOfWOrk.commit();
                result = Result.Create(updatedStorage, statusCode);
                return result;
            }
            catch (Exception)
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode);
                return result;
            }
        }
      
        public async Task<Result> UpdateStorageItem(long storageItemId, StorageItemContract storageItemContract)
        {
            try
            {
                var statusCode = StatusCodes.Status200OK;
                var result = Result.Create(null, 0);

                var storageItem = await _storageItemRepository.GetById(storageItemId);

                if(storageItem == null)
                {
                    statusCode = StatusCodes.Status400BadRequest;
                    result = Result.Create(null, statusCode);
                        return result;
                }
                var storageInputOutputList = GetAllStorageInputOutputsByStorageItemId(storageItemId);
                _storageInputOutputRepository.DeletAll(storageInputOutputList);
                //      var storageItems = GetAllStorageItemsByStorageId(storage.Id);
                var createDate = storageItem.CreateDate;
                var storageId = storageItem.StorageId;
                await  _storageItemRepository.Delete(storageItem);

                //  List<StorageItem> storageItemList = PrepareStorageItem(sc.StorageItemList);
                var storageInpoutOutput = PrepareStorageInputOutput(storageItemContract);
                List<StorageInputOutput> lista = new List<StorageInputOutput>();
                lista.Add(storageInpoutOutput);
                var newStorageItem = new StorageItem
                {
                    Id = storageItemId,
                    StorageId = storageItem.StorageId,
                    Serialnumber = storageItemContract.SerialNumber,
                    ProductId = storageItemContract.ProductId,
                    SupplierId = storageItemContract.SupplierId,
                    Quantity = storageItemContract.Quantity,
                    CreateDate = createDate,
                    StorageInputOutputList = lista

                };

                var updatedStorageItem = await _storageItemRepository.UpdateEntity(storageItemId,newStorageItem);
                _unitOfWOrk.commit();
                result = Result.Create(updatedStorageItem.Id, statusCode);
                return result;

            }
            catch (Exception)
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode);
                return result; 
            }
        }
        public List<StorageInputOutput> GetAllStorageInputOutputsByStorageItemId(long storageItemId)
        {
            List<StorageInputOutput> storageInputOutputList = new List<StorageInputOutput>();
            storageInputOutputList = _storageInputOutputRepository.GetQueryable<StorageInputOutput>()
                .Where(x => x.StorageItemId == storageItemId)
                .ToList();

            return storageInputOutputList;
        }
    }
}
