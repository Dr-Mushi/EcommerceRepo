using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingEFRelations.Repositories.Interface
{
    public interface IRepository <T>
    {
        Task<bool> HasSameItem(int? id);
        Task<bool> DeleteSameItem(int? id);

        double TotalAsync(double entityTotal, IEnumerable<T> entitySize);
    }
}
