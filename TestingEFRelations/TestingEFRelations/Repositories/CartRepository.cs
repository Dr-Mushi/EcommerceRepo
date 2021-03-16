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
    public class CartRepository : ICartRepository
    {

         private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void AddCart(Cart cart)
        {
            _context.Cart.AddAsync(cart);

        }

        public double CartSumTotal(IEnumerable<Cart> cartItems)
        {
            double cartTotal = 0;

            //sum of all cart tables totals
            foreach (var item in cartItems)
            {
                cartTotal += item.CartTotal;
            }
            return cartTotal;
        }

        public async Task<bool> DeleteCart(int id)
        {
            var cart = await _context.Cart.FirstOrDefaultAsync(m => m.ProductID == id);
            _context.Cart.Remove(cart);
            return true;
        }

        public async Task<IEnumerable<Cart>> GetCartItems()
        {
            var applicationDbContext = _context.Cart.Include(c => c.Product)
               .Include(r => r.Product.ProductSize)
               .Include(r => r.Product.ProductImage);


            var getAllCartItems = await applicationDbContext.ToListAsync();

            return getAllCartItems;
        }

        public async Task<bool> HasSameItem(int? id)
        {
            if (id == null)
            {
                return false;
            }

            var cart = await _context.Cart
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (cart == null)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> SaveCart()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> SetCartTotal(Cart cart)
        {
            var productItem = await _context.Product.FirstOrDefaultAsync(m => m.ProductID == cart.ProductID);
            cart.CartTotal = cart.CartProductQuantity * productItem.ProductPrice;

            return true;
        }
    }
}
