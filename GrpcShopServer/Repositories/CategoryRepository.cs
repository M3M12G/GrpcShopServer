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
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private DataContext _dbContext;

        public CategoryRepository(DataContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<bool> isExist(string name)
        {
            return await _dbContext.Categories.AnyAsync(cat => cat.Name == name);
        }
    }
}
