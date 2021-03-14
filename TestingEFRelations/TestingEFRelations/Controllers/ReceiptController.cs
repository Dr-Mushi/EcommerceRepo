using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestingEFRelations.Data;
using TestingEFRelations.Models;

namespace TestingEFRelations.Controllers
{
    public class ReceiptController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReceiptController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Receipt
        public async Task<IActionResult> Index()
        {




            var applicationDbContext = _context.Receipt.Include(r => r.Product)
                .Include(r => r.Product.ProductSize)
                .Include(r => r.Product.ProductImage);
            var getAllReceiptItems = await applicationDbContext.ToListAsync();


            double receiptTotal = 0;

            foreach (var item in getAllReceiptItems)
            {
                receiptTotal += item.ReceiptTotal;
            }

            ViewData["total"] = receiptTotal.ToString("0.00");

            return View(getAllReceiptItems);

        }

        // GET: Receipt/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receipt = await _context.Receipt
                .FirstOrDefaultAsync(m => m.ID == id);
            if (receipt == null)
            {
                return NotFound();
            }

            return View(receipt);
        }

        // GET: Receipt/Create
        public IActionResult Create()
        {
            ViewData["CartID"] = new SelectList(_context.Cart, "ID", "ID");
            return View();
        }

        // POST: Receipt/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CartID")] Receipt receipt)
        {
            if (ModelState.IsValid)
            {
                var productContext = _context.Product;
                

                var cartContext = _context.Cart;

                //var getCartProduct = await applicationDbContext.ToListAsync();


                foreach (var item in cartContext)
                {
                    receipt.ProductID = item.ProductID;
                    receipt.ReceiptProductQuantity = item.CartProductQuantity;
                    receipt.ReceiptTotal = item.CartTotal;
                    var cart = await cartContext.FindAsync(item.ID);
                    cartContext.Remove(cart);
                    _context.Add(receipt);
                  
                    //create new object each time you add a new record
                    //that way you avoid overriding the same object.
                    receipt = new Receipt();
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["CartID"] = new SelectList(_context.Cart, "ID", "ID", receipt.CartID);
            return View(receipt);
        }

        // GET: Receipt/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receipt = await _context.Receipt.FindAsync(id);
            if (receipt == null)
            {
                return NotFound();
            }
            //ViewData["CartID"] = new SelectList(_context.Cart, "ID", "ID", receipt.CartID);
            return View(receipt);
        }

        // POST: Receipt/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CartID")] Receipt receipt)
        {
            if (id != receipt.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(receipt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReceiptExists(receipt.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            //ViewData["CartID"] = new SelectList(_context.Cart, "ID", "ID", receipt.CartID);
            return View(receipt);
        }

        // GET: Receipt/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receipt = await _context.Receipt
                .FirstOrDefaultAsync(m => m.ID == id);
            if (receipt == null)
            {
                return NotFound();
            }

            return View(receipt);
        }

        // POST: Receipt/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var receipt = await _context.Receipt.FindAsync(id);
            _context.Receipt.Remove(receipt);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReceiptExists(int id)
        {
            return _context.Receipt.Any(e => e.ID == id);
        }
    }
}
