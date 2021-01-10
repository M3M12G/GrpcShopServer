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
    public class ProductService:ProductCRUD.ProductCRUDBase
    {
        private readonly ILogger<ProductService> _logger;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public ProductService(IMapper mapper, IProductRepository productRepository, ILogger<ProductService> logger, ICategoryRepository categoryRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _logger = logger;
            _categoryRepository = categoryRepository;
        }

        public override async Task<ProductInfo> AddProduct(ProductCreate request, ServerCallContext context)
        {
           if(await _categoryRepository.GetRecById(request.CategoryId) != null)
            {
                if(await _productRepository.isProductExist(request.Name, request.CategoryId))
                {
                    _logger.LogError($"The product with name={request.Name} and Category id={request.CategoryId} is already exists");
                    return null;
                }

                var toSaveProduct = _mapper.Map<Product>(request);

                try
                {
                    await _productRepository.CreateRec(toSaveProduct);

                    var insertedProd = await _productRepository.GetProductDetailsById(toSaveProduct.Id);
                    var prodInfo = _mapper.Map<ProductInfo>(insertedProd);
                    return prodInfo;
                }
                catch(Exception e)
                {
                    _logger.LogError($"Some problems occured: {e.Message}");
                }
            }

            _logger.LogError($"Category id={request.CategoryId} for inserted product does not exist");
            _logger.LogError("AddProduc Process Failed");
            return null;
        }

        public override async Task<ProductInfo> GetProductById(ProductLookup request, ServerCallContext context)
        {
            var searchRes = await _productRepository.GetProductDetailsById(request.Id);

            if (searchRes != null)
            {
                var prodFound = _mapper.Map<ProductInfo>(searchRes);
                return prodFound;
            }
            _logger.LogError($"Product with id={request.Id} not found");
            return null;
        }

        public override async Task GetProductsByCategoryId(CategoryLookup request, IServerStreamWriter<ProductInfo> responseStream, ServerCallContext context)
        {
            var prodsList = await _productRepository.GetAllProdsByCategoryId(request.Id);
                foreach(var singleProd in prodsList)
                {
                    var prod = _mapper.Map<ProductInfo>(singleProd);
                    await responseStream.WriteAsync(prod);
                }
        }
    }
}
