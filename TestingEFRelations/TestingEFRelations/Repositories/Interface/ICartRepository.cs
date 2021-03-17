using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingEFRelations.Models;

namespace TestingEFRelations.Repositories.Interface
{
    public interface ICartRepository
    {
        Task<bool> SetCartTotal(Cart cart);
        double CartSumTotal(IEnumerable<Cart> cartItems);
        Task<IEnumerable<Cart>> GetCartItems();

        void AddCart(Cart cart);
        Task<bool> DeleteCart(int id);

        Task<bool> SaveCart();
        Task<bool> HasSameItem(int? id);


        void CartUpdate(Cart cart);
        Task<Cart> FindCart(int? id);

        Task<Cart> IncreaseProductQuantity(Cart cartID, int quantity);

        bool CartExists(int id);
    }
}
