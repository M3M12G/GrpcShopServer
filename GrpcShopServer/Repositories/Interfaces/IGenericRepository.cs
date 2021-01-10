using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcShopServer.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity>
         where TEntity : class
    {
        Task<IReadOnlyList<TEntity>> GetAllRecs();
        Task<TEntity> GetRecById(int id);
        Task CreateRec(TEntity entity);
        Task UpdateRec(TEntity entity);
        Task DeleteRec(int id);
    }
}