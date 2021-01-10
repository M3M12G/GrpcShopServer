using GrpcShopServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcShopServer.Repositories.Interfaces
{
    public interface ICategoryRepository:IGenericRepository<Category>
    {
        Task<bool> isExist(string name);
    }
}
