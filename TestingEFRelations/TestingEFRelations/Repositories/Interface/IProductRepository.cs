using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingEFRelations.Models;

namespace TestingEFRelations.Repositories.Interface
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductItems();

        void AddProduct(Product product);

        Task<bool> SaveProduct();

        Task<Product> FindProduct(int? id);
        Task<bool> DeleteProduct(int? id);
        SelectList SelectListSize();

        bool ProductExists(int id);

        public void ProductUpdate(Product product);

        IEnumerable<Product> search(string SearchTerm = null);

    }
}
