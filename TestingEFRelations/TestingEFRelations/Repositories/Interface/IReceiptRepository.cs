using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingEFRelations.Models;

namespace TestingEFRelations.Repositories.Interface
{
    public interface IReceiptRepository
    {
        Task<IEnumerable<Receipt>> GetReceiptItems();

        double ReceiptSumTotal(IEnumerable<Receipt> ReceiptItems);

        Task<bool> DeleteReceipt(int id);

        Task<bool> SaveReceipt();

        void AddReceipt(Receipt wishlist);
    }
}
