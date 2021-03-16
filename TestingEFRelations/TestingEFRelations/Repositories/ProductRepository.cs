using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingEFRelations.Data;
using TestingEFRelations.Models;
using TestingEFRelations.Repositories.Interface;

namespace TestingEFRelations.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void AddProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> FindProduct(int? id)
        {
            var product = await _context.Product
               .Include(p => p.ProductImage)
               .Include(p => p.ProductSize)
               .FirstOrDefaultAsync(m => m.ProductID == id);

            return product;
        }

        public async Task<IEnumerable<Product>> GetProductItems()
        {
            var applicationDbContext = _context.Product
            .Include(p => p.ProductImage)
            .Include(p => p.ProductSize);
            var getAllProductItems = await applicationDbContext.ToListAsync();

            return getAllProductItems;
        }

        public async Task<bool> SaveProduct()
        {
            throw new NotImplementedException();
        }
    }
}
