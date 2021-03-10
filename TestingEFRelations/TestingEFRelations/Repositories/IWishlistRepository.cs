using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestingEFRelations.Repositories
{
    public interface IWishlistRepository
    {

        Task<bool> HasSameItem(int? id);
        Task<bool> DeleteSameItem(int? id);
    }
}