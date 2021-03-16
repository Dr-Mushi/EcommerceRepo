using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingEFRelations.Data;

namespace TestingEFRelations.Repositories
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly ApplicationDbContext _context;

        public WishlistRepository(ApplicationDbContext context)
        {
            _context = context;
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

        public async Task<bool> DeleteSameItem(int? id)
        {
            var wishlist = await _context.Wishlist.FirstOrDefaultAsync(m => m.ProductID == id);
            _context.Wishlist.Remove(wishlist);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
