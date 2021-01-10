using GrpcShopServer.Data;
using GrpcShopServer.Models;
using GrpcShopServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcShopServer.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity:class, IEntity
    {
        private readonly DataContext _dbContext;

        public GenericRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateRec(TEntity entity)
        {
                await _dbContext.Set<TEntity>().AddAsync(entity);
                await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRec(int id)
        {
            var entity = await _dbContext.Set<TEntity>().FindAsync(id);
            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<TEntity>> GetAllRecs()
        {
            return await _dbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetRecById(int id)
        {
            return await _dbContext.Set<TEntity>()
                     .AsNoTracking()
                     .FirstOrDefaultAsync(obj => obj.Id == id);
        }

        public async Task UpdateRec(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
