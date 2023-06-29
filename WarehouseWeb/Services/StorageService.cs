using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Contracts;
using WarehouseWeb.Contracts.StorageDto;
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
            
                var statusCode = StatusCodes.Status500InternalServerError;
                var errorMessage = "Greska";
                var result = Result.Create(null, statusCode,errorMessage,0);

                if (sc == null)
                {
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    result.ErrorMessage = "Netacni ulazni parametri";
                    return result;
                }                

                var storage = new Storage
                {
                    Serialnumber = sc.SerialNumber,
                    Name = sc.Name,
                    City = sc.City,
                   // StorageItemList = storageItemList
                };

                var isAdded = await _storageRepository.AddEntity(storage);
                    if(isAdded == false)
            {
                result.Value = isAdded;
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Coudn't add a stoage";
                return result;
            }

                result.StatusCode = StatusCodes.Status200OK;
                result.ErrorMessage = null;
                result.Value = isAdded;
                _unitOfWOrk.commit();
                return result;
           
            
        }

        public async Task<Result> AddStorageItem(StorageItemContract storageItemContract)
        {
           
                var statusCode = StatusCodes.Status500InternalServerError;
                var errorMessage = "Greska";
                var result = Result.Create(null,statusCode,errorMessage,0);

            if (storageItemContract == null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Losi ulazni parametri";
                return result;
            }
            if(storageItemContract.StorageId < 1)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Ne postoji dati storage";
                return result;
            }

            var findStorageItem = _storageItemRepository.GetQueryable<StorageItem>()
                   .Any(x => x.StorageId == storageItemContract.StorageId &&  x.ProductId == storageItemContract.ProductId);
                   

            
            if(findStorageItem == true)
            {
                result.ErrorMessage = "Postoji dati prozizvod za dati storage, mozete promeniti kolicinu";
                result.StatusCode = StatusCodes.Status200OK;
                result.Value = null;
                return result;
                //findStorageItem.Quantity.Amount += storageItemContract.Quantity.Amount;
                //_unitOfWOrk.commit();
                //result.StatusCode = StatusCodes.Status200OK;
                //result.Value = true;
                //result.ErrorMessage = null;
                //return result;
            }

                var storageInpoutOutputList = AddStorageInputOutputList(storageItemContract);
                
                var storageItem = new StorageItem();


                storageItem.StorageId = storageItemContract.StorageId;
                storageItem.ProductId = storageItemContract.ProductId;
                storageItem.Quantity = new Quantity() {

                    Amount = storageItemContract.Quantity.Amount,
                    MeasurementUnitId = storageItemContract.Quantity.MeasurementUnitId

                };
                storageItem.StorageInputOutputList = storageInpoutOutputList;
                
                
                var isAdded = await _storageItemRepository.AddEntity(storageItem);
                if(isAdded == false)
            {
                result.Value = isAdded;
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Couldn't add a storageItem";
            }
                
                _unitOfWOrk.commit();
                result.StatusCode = StatusCodes.Status200OK;
                result.Value = isAdded;
                result.ErrorMessage = null;
            return result;
           
        }

        public List<StorageInputOutput> AddStorageInputOutputList(StorageItemContract storageItemContract)
        {
            List<StorageInputOutput> lista = new List<StorageInputOutput>();
            
            var storageInputOutput = new StorageInputOutput()
            {

                StorageInputOutputType = StorageInputOutputType.Input,
                Quantity = storageItemContract.Quantity
            };
            lista.Add(storageInputOutput);

            return lista;

        }

        public StorageInputOutput AddStorageInput(StorageItemContract storageItemContract)
        {
            var storageInputOutput = new StorageInputOutput();


            storageInputOutput.StorageInputOutputType = StorageInputOutputType.Input;
            storageInputOutput.Quantity = new Quantity
            {
                Amount = storageItemContract.Quantity.Amount,
                MeasurementUnitId = storageItemContract.Quantity.MeasurementUnitId
            };
           
            return storageInputOutput;

        }
        


        public async Task<Result> DeleteStorage(long id)
        {
            
                var statusCode = StatusCodes.Status500InternalServerError;
                var errorMessage = "Greska";
                var result = Result.Create(null, statusCode,errorMessage,0);
                var storage = await _storageRepository.GetById(id);

                if(storage == null)
                {
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    result.ErrorMessage = "Ne postoji storage";
                    return result;
                }

                var isDeleted = await _storageRepository.Delete(storage);
            if(isDeleted == false)
            {
                result.Value = isDeleted;
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Nije Izbrisan storage";
                return result;
            }
                _unitOfWOrk.commit();
                result.StatusCode = StatusCodes.Status200OK;
                result.ErrorMessage = null;
                result.Value = isDeleted;
                
                return result;

            
        }


        public async Task<Result> GetAllStorages()
        {
           
                var statusCode = StatusCodes.Status200OK;
                var allStorages = _storageRepository.GetQueryable<Storage>()
                    .AsNoTracking()
                    .Select(x => new GetAllStoragesResponse(x.Id, x.Name, x.City)).ToList();
                var result = Result.Create(allStorages, statusCode,null,0);

                return result;

            
        }

        public async Task<Result> GetStorageById(long id)
        {
            
                var statusCode = StatusCodes.Status500InternalServerError;
                var errorMessage = "Greska";
                var result = Result.Create(null, statusCode,errorMessage,0);

                var storage =  _storageRepository.GetQueryable<Storage>()
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .Select(x => new GetStorageByIdResponse(x.Id, x.Name, x.City))
                    .FirstOrDefault();
                    

                if(storage == null)
                {
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    result.ErrorMessage = "Ne postoji storage";
                    return result;
                }
                result.StatusCode = StatusCodes.Status200OK;
                result.Value = storage;
                result.ErrorMessage = null;
                return result;
            
        }

        public async Task<Result> GetStorageitemById(long id)
        {
            var statusCode = StatusCodes.Status500InternalServerError;
            var errorMessage = "Greska";
            var result = Result.Create(null, statusCode, errorMessage,0);

            var storageItem = _storageItemRepository.GetQueryable<StorageItem>()
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new GetStorageItemByIdResponse(x.Id, x.StorageId, x.ProductId))
                .FirstOrDefault();


            if (storageItem == null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Ne postoji storage";
                return result;
            }
            result.StatusCode = StatusCodes.Status200OK;
            result.Value = storageItem; 
            result.ErrorMessage = null;
            return result;

        }
        public async Task<Result> GetAllStorageItemsByStorageId(long storageId)
        {
            var statusCode = StatusCodes.Status500InternalServerError;
            var errorMessage = "Greska";
            var result = Result.Create(null, statusCode, errorMessage,0);

            var storage = GetStorageById(storageId);

            if (storageId < 1)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Los storagId";
                return result;

            }
            if (storage == null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Storage ne postoji";
                return result;
            }
            var allStorageItems = _storageItemRepository.GetQueryable<StorageItem>()
               .AsNoTracking()
              .Where(x => x.StorageId == storageId)
               .Select(x => new GetStorageItemByIdResponse(x.Id, x.StorageId, x.ProductId)).ToList();

            result.Value = allStorageItems;
            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;
            return result;

        }
        public async Task<Result> UpdateStorage(StorageContract storageContract)
        {
           
                var statusCode = StatusCodes.Status500InternalServerError;
                var errorMessage = "Greska";
                var result = Result.Create(null, statusCode,errorMessage,0);
                var storage = await _storageRepository.GetById(storageContract.Id);

                if(storage == null)
                {
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    result.ErrorMessage = "Nije pronadjen storage";
                    return result;

                }

                storage.Serialnumber = storageContract.SerialNumber;
                storage.Name = storageContract.Name;
                storage.City = storageContract.City;


                var IsUpdated =  await _storageRepository.UpdateEntity(storage);
                
                if(IsUpdated == false)
            {
                result.Value = IsUpdated;
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Coudn't update storage";
                return result;

            }

                _unitOfWOrk.commit();

                result.Value = IsUpdated;
                result.ErrorMessage = null;
                result.StatusCode = StatusCodes.Status200OK;
                return result;
            
        }
      
        public async Task<Result> UpdateStorageItem(StorageItemContract storageItemContract)
        {
            
                var statusCode = StatusCodes.Status500InternalServerError;
                var errorMessage = "Greska";
                var result = Result.Create(null, statusCode,errorMessage,0);

                var storageItem = _storageItemRepository.GetQueryable<StorageItem>()
                    .Include(x=> x.StorageInputOutputList) 
                    .Where(x => x.Id == storageItemContract.Id)                 
                    .FirstOrDefault();

                if( storageItem == null)
                {
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    result.ErrorMessage = "Nije pronadjen item";
                        return result;
                }

               storageItem.StorageInputOutputList.RemoveAll(x => x.StorageInputOutputType == StorageInputOutputType.Input);


                var newStorageInput = AddStorageInput(storageItemContract);


                storageItem.StorageId = storageItemContract.StorageId;
                storageItem.ProductId = storageItemContract.ProductId;
                storageItem.Quantity = new Quantity()
                {
                    Amount = storageItemContract.Quantity.Amount,
                    MeasurementUnitId = storageItemContract.Quantity.MeasurementUnitId

                };
                storageItem.StorageInputOutputList.Add(newStorageInput);

                

                var isUpdated = await _storageItemRepository.UpdateEntity(storageItem);
                  if(isUpdated == false)
                     {
                result.Value = isUpdated;
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Couldn'y update storage item";
                return result;
                      }

                _unitOfWOrk.commit();
                result.Value = isUpdated;
                result.ErrorMessage = null;
                result.StatusCode = StatusCodes.Status200OK;
                return result;

            
        }
        public List<StorageInputOutput> GetAllStorageInputOutputsByStorageItemId(long storageItemId)
        {
            List<StorageInputOutput> storageInputOutputList = new List<StorageInputOutput>();
            storageInputOutputList = _storageInputOutputRepository.GetQueryable<StorageInputOutput>()
                .Where(x => x.StorageItemId == storageItemId)
                .ToList();

            return storageInputOutputList;
        }


            

          //  var statusCode = StatusCodes.Status200OK;
           

        

        public async Task<Result> DeleteStorageItem(long id)
        {
            var statusCode = StatusCodes.Status500InternalServerError;
            var errorMessage = "Greska";
            var result = Result.Create(null, statusCode, errorMessage,0);
            var storageItem = await _storageItemRepository.GetById(id);

            if (storageItem == null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Ne postoji storage item";
                return result;
            }

            var isDeleted = await _storageItemRepository.Delete(storageItem);
            if (isDeleted == false)
            {
                result.Value = isDeleted;
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Nije Izbrisan storage item";
                return result;
            }
            _unitOfWOrk.commit();
            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;
            result.Value = isDeleted;

            return result;
        }
    }

        
    }

