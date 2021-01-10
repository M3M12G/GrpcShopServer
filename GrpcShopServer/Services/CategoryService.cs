using AutoMapper;
using Grpc.Core;
using GrpcShopServer.Models;
using GrpcShopServer.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcShopServer.Services
{
    public class CategoryService:CategoryCRUD.CategoryCRUDBase
    {
        private readonly ILogger<CategoryService> _logger;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, ILogger<CategoryService> logger, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public override async Task<CategoryInfo> AddCategory(CategoryCreate request, ServerCallContext context)
        {
            if(await _categoryRepository.isExist(request.Name))
            {
                _logger.LogError($"the category with name={request.Name} are already exists in DB");
                return null;
            }

            var toSaveCat = _mapper.Map<Category>(request);

            try
            {
                await _categoryRepository.CreateRec(toSaveCat);

                var catInfo = _mapper.Map<CategoryInfo>(toSaveCat);

                _logger.LogInformation($"The new record to Categories table added: {catInfo.Id + catInfo.Name}");
                
                return catInfo;
            }
            catch(Exception e)
            {
                _logger.LogError($"Some errors occured: {e.Message}");
            }
            _logger.LogError("AddCategory Process Failed");
            return null;
        }

        public override async Task GetAllCategories(AllLookup request, IServerStreamWriter<CategoryInfo> responseStream, ServerCallContext context)
        {
            var allCats = await _categoryRepository.GetAllRecs();

            foreach(var singleCat in allCats)
            {
                var category = _mapper.Map<CategoryInfo>(singleCat);

                await responseStream.WriteAsync(category);
            }
        }

        public override async Task<CategoryInfo> GetCategoryById(CategoryLookup request, ServerCallContext context)
        {
            var searchRes = await _categoryRepository.GetRecById(request.Id);

            if(searchRes != null)
            {
                var catFound = _mapper.Map<CategoryInfo>(searchRes);
                return catFound;
            }

            _logger.LogError($"Category record with id={request.Id} not found");
            return null;
        }

        public override async Task<AllLookup> DeleteCategory(CategoryLookup request, ServerCallContext context)
        {
            var searchRes = await _categoryRepository.GetRecById(request.Id);
            if(searchRes != null)
            {
                try
                {
                    await _categoryRepository.DeleteRec(request.Id);
                    return new AllLookup();
                }
                catch (Exception e)
                {
                    _logger.LogError($"Some errors occured: {e.Message}");
                }
            }

            _logger.LogError($"DeleteCategory Process Failed.The category with id={request.Id} does not exist");
            return null;
        }

        public override async Task<CategoryInfo> UpdateCategory(CategoryInfo request, ServerCallContext context)
        {
            if(await _categoryRepository.isExist(request.Name))
            {
                _logger.LogError($"The category name={request.Name} cannot be used again");
                return null;
            }

            try
            {
                var updCat = _mapper.Map<Category>(request);
                await _categoryRepository.UpdateRec(updCat);

                var updatedCat = await _categoryRepository.GetRecById(request.Id);
                var grpcCat = _mapper.Map<CategoryInfo>(updatedCat);

                return grpcCat;
            }
            catch(Exception e)
            {
                _logger.LogError($"Some problems occured: {e.Message}");
            }

            _logger.LogError("UpdateCategory Process Failed");
            return null;
        }
    }
}
