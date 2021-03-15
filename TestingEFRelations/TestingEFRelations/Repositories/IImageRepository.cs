using System.Threading.Tasks;
using TestingEFRelations.Models;

namespace TestingEFRelations.Repositories
{
    public interface IImageRepository
    {
        Task<bool> AddImage(Product product);
    }
}