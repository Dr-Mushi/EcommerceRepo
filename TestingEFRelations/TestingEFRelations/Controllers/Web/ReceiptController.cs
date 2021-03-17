using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestingEFRelations.Models;
using TestingEFRelations.Repositories.Interface;

namespace TestingEFRelations.Controllers
{
    public class ReceiptController : Controller
    {
        private readonly IReceiptRepository _receipt;


        public ReceiptController(IReceiptRepository receipt)
        {
            _receipt = receipt;
        }

        // GET: Receipt
        public async Task<IActionResult> Index()
        {
            var getAllReceiptItems = await _receipt.GetReceiptItems();

            ViewData["total"] = _receipt.ReceiptSumTotal(getAllReceiptItems).ToString("0.00");

            return View(getAllReceiptItems);
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

                _receipt.AddReceipt(receipt);
                await _receipt.SaveReceipt();

                return RedirectToAction(nameof(Index));
            }
            return View(receipt);
        }

        // POST: Receipt/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("ID,CartID")] Receipt receipt)
        //{
        //    if (id != receipt.ID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(receipt);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ReceiptExists(receipt.ID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(receipt);
        //}

        // POST: Receipt/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            await _receipt.DeleteReceipt(id);
            await _receipt.SaveReceipt();
            return RedirectToAction(nameof(Index));
        }

        //private bool ReceiptExists(int id)
        //{
        //    return _context.Receipt.Any(e => e.ID == id);
        //}
    }
}


//// GET: Receipt/Details/5
//public async Task<IActionResult> Details(int? id)
//{
//    if (id == null)
//    {
//        return NotFound();
//    }

//    var receipt = await _context.Receipt
//        .FirstOrDefaultAsync(m => m.ID == id);
//    if (receipt == null)
//    {
//        return NotFound();
//    }

//    return View(receipt);
//}

//// GET: Receipt/Create
//public IActionResult Create()
//{
//    ViewData["CartID"] = new SelectList(_context.Cart, "ID", "ID");
//    return View();
//}


//// GET: Receipt/Delete/5
//public async Task<IActionResult> Delete(int? id)
//{
//    if (id == null)
//    {
//        return NotFound();
//    }

//    var receipt = await _context.Receipt
//        .FirstOrDefaultAsync(m => m.ID == id);
//    if (receipt == null)
//    {
//        return NotFound();
//    }

//    return View(receipt);
//}


//// GET: Receipt/Edit/5
//public async Task<IActionResult> Edit(int? id)
//{
//    if (id == null)
//    {
//        return NotFound();
//    }

//    var receipt = await _context.Receipt.FindAsync(id);
//    if (receipt == null)
//    {
//        return NotFound();
//    }
//    //ViewData["CartID"] = new SelectList(_context.Cart, "ID", "ID", receipt.CartID);
//    return View(receipt);
//}


           //ViewData["CartID"] = new SelectList(_context.Cart, "ID", "ID", receipt.CartID);
