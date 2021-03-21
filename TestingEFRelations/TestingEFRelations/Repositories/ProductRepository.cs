using Microsoft.AspNetCore.Mvc.Rendering;
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
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            _context.Add(product);
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

        public async Task<bool> DeleteProduct(int? id) 
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            return true;
        }

        public async Task<bool> SaveProduct()
        {
           return await _context.SaveChangesAsync() > 0;
        }

        public SelectList SelectListSize()
        {
            return new SelectList(_context.Size, "ID", "SizeName");
        }

        public bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductID == id);
        }

        public void ProductUpdate(Product product)
        {

            ////for API
            //_context.Entry(product).State = EntityState.Modified;

            //_context.Update(product);
        }




    }
}
