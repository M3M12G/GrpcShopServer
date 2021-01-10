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
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly DataContext _dbContext;
        public ProductRepository(DataContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<IReadOnlyList<Product>> GetAllProdsByCategoryId(int catId)
        {
            return await _dbContext.Products.Include(p => p.Category).AsNoTracking().Where(cat => cat.CategoryId == catId).ToListAsync();
        }

        public async Task<Product> GetProductDetailsById(int id)
        {
            return await _dbContext.Products.Include(p => p.Category).AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> isProductExist(string name, int catId)
        {
            return await _dbContext.Products.AnyAsync(prod => prod.Name == name && prod.CategoryId == catId);
        }
    }
}
