using System.Collections.Generic;
using System.Threading.Tasks;
using TestingEFRelations.Models;

namespace TestingEFRelations.Repositories.Interface
{
    public interface IWishlistRepository
    {

        Task<bool> HasSameItem(int? id);
        //Task<bool> DeleteSameItem(int? id);

        Task<bool> SetWishlistTotal(Wishlist wishlist);
        double WishlistSumTotal(IEnumerable<Wishlist> wishlistItems);
        Task<IEnumerable<Wishlist>> GetWishlistItems();

        void AddWishlist(Wishlist wishlist);
        Task<bool> DeleteWishlist(int id);

        Task<bool> SaveWishlist();

        void WishlistUpdate(Wishlist wishlist);
        Task<Wishlist> FindWishlist(int? id);

        Task<Wishlist> IncreaseProductQuantity(Wishlist wishlistID, int quantity);

        bool WishlistExists(int id);

    }
}