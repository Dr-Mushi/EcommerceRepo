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
    public class WishlistRepository : IWishlistRepository
    {
        private readonly ApplicationDbContext _context;

        public WishlistRepository(ApplicationDbContext context)
        {
            _context = context;
        }

       
        public async Task<IEnumerable<Wishlist>> GetWishlistItems()
        {
            var applicationDbContext = _context.Wishlist.Include(w => w.Product)
                .Include(c => c.Product.ProductImage)
                .Include(c => c.Product.ProductSize);

            var getAllWishlistItems = await applicationDbContext.ToListAsync();

            return getAllWishlistItems;
        }

        public void AddWishlist(Wishlist wishlist)
        {
            _context.Wishlist.Add(wishlist);
        }

        public async Task<bool> DeleteWishlist(int id)
        {
            var wishlist = await _context.Wishlist.FirstOrDefaultAsync(m => m.ProductID == id);
            _context.Wishlist.Remove(wishlist);
            return true;
        }
        
        public void WishlistUpdate(Wishlist wishlist)
        {
            _context.Update(wishlist);
        }

        public async Task<bool> SaveWishlist()
        {
            return await _context.SaveChangesAsync() > 0;

        }



        public double WishlistSumTotal(IEnumerable<Wishlist> wishlistItems)
        {
            double wishlistTotal = 0;

            //sum of all wishlist tables totals
            foreach (var item in wishlistItems)
            {
                wishlistTotal += item.WishlistTotal;
            }

            return wishlistTotal;
        }

        public async Task<bool> HasSameItem(int? id)
        {
            if (id == null)
            {
                return false;
            }

            var wishlist = await _context.Wishlist
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (wishlist == null)
            {
                return false;
            }

            return true;
        }


         public async Task<bool> SetWishlistTotal(Wishlist wishlist)
         {
            var productItem = await _context.Product.FirstOrDefaultAsync(m => m.ProductID == wishlist.ProductID);
            wishlist.WishlistTotal = wishlist.WishlistProductQuantity * productItem.ProductPrice;

            return true;
         }

        public async Task<Wishlist> FindWishlist(int? id)
        {
            var wishlist = await _context.Wishlist.FirstOrDefaultAsync(m => m.ProductID == id);

            return wishlist;
        }

        public async Task<Wishlist> IncreaseProductQuantity(Wishlist wishlistID, int quantity)
        {
            //get the product Object inside wishlist with the existed wishlist product ID

            var wishlist = await FindWishlist(wishlistID.ProductID);
            
            var increasedQuantity = wishlist.WishlistProductQuantity += quantity;
            //check if the increased quantity does NOT exceed the qunatity of the product.
            if (increasedQuantity <= wishlist.Product.ProductQuantity)
            {
                wishlist.WishlistTotal = wishlist.WishlistProductQuantity * wishlist.Product.ProductPrice;
                
                return wishlist;
            }
            wishlist.WishlistProductQuantity -= quantity;
            return wishlist;
        }

        public bool WishlistExists(int id)
        {
            return _context.Wishlist.Any(e => e.ID == id);
        }

        //public async Task<bool> DeleteSameItem(int? id)
        //{
        //    var wishlist = await _context.Wishlist.FirstOrDefaultAsync(m => m.ProductID == id);
        //    _context.Wishlist.Remove(wishlist);
        //    await _context.SaveChangesAsync();

        //    return true;

        //}




    }
}
