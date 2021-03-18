using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestingEFRelations.Data;
using TestingEFRelations.Models;
using TestingEFRelations.Repositories.Interface;

namespace TestingEFRelations.Repositories
{
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly ApplicationDbContext _context;

        public ReceiptRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async void AddReceipt(Receipt receipt)
        {
            var cartContext = _context.Cart;

            foreach (var item in cartContext)
            {
                receipt.ProductID = item.ProductID;
                receipt.ReceiptProductQuantity = item.CartProductQuantity;
                receipt.ReceiptTotal = item.CartTotal;
                //remove items from the cart as they are submitted to the receipt
                var cart = await cartContext.FindAsync(item.ID);
                cartContext.Remove(cart);
                _context.Add(receipt);
                //create new object each time you add a new record
                //that way you avoid overriding the same object.
                receipt = new Receipt();
            }
        }

        public async Task<bool> DeleteReceipt(int id)
        {
            var receipt = await _context.Receipt.FirstOrDefaultAsync(m => m.ID == id);
            _context.Receipt.Remove(receipt);
            return true;
        }

        public async Task<IEnumerable<Receipt>> GetReceiptItems()
        {
            var getAllReceiptItems = _context.Receipt.Include(r => r.Product)
               .Include(r => r.Product.ProductSize)
               .Include(r => r.Product.ProductImage);

            return await getAllReceiptItems.ToListAsync();
        }

        public double ReceiptSumTotal(IEnumerable<Receipt> ReceiptItems)
        {
            double receiptTotal = 0;

            foreach (var item in ReceiptItems)
            {
                receiptTotal += item.ReceiptTotal;
            }

            return receiptTotal;
        }

        public async Task<bool> SaveReceipt()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
