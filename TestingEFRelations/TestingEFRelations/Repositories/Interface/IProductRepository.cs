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


    }
}
