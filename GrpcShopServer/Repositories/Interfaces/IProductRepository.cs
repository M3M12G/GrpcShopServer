using GrpcShopServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcShopServer.Repositories.Interfaces
{
    public interface IProductRepository:IGenericRepository<Product>
    {
        //Checks if the name of the Product and its Category free
        Task<bool> isProductExist(string name,int catId);
        Task<Product> GetProductDetailsById(int id);
        Task<IReadOnlyList<Product>> GetAllProdsByCategoryId(int catId);
    }
}
