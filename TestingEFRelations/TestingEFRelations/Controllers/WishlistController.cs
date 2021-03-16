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
    public class WishlistController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WishlistController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Wishlists
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Wishlist.Include(w => w.Product)
                .Include(c => c.Product.ProductImage)
                .Include(c => c.Product.ProductSize);

            var getAllWishlistItems = await applicationDbContext.ToListAsync();

            double wishlistTotal = 0;

            //sum of all wishlist tables totals
            foreach (var item in getAllWishlistItems)
            {
                wishlistTotal += item.WishlistTotal;
            }

            ViewData["wishlistTotal"] = wishlistTotal.ToString("0.00");

            return View(getAllWishlistItems);
        }

        // POST: Wishlists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ProductID,WishlistProductQuantity")] Wishlist wishlist)
        {
            if (ModelState.IsValid)
            {
                //get product object that has the same ID as the wishlist product
                var productItem = await _context.Product.FirstOrDefaultAsync(m => m.ProductID == wishlist.ProductID);
                

                wishlist.WishlistTotal = wishlist.WishlistProductQuantity * productItem.ProductPrice;

                if (await HasSameItem(wishlist.ProductID))
                {
                    await IncreaseProductQuantity(wishlist, wishlist.WishlistProductQuantity);
                    return RedirectToAction(nameof(Index));

                }

                _context.Add(wishlist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID", wishlist.ProductID);
            return View(wishlist);
        }
        // POST: Wishlists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ProductID,WishlistProductQuantity")] Wishlist wishlist)
        {
            if (id != wishlist.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wishlist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WishlistExists(wishlist.ID))
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
            return View(wishlist);
        }

        // POST: Wishlists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wishlist = await _context.Wishlist.FirstOrDefaultAsync(m => m.ProductID == id);
            _context.Wishlist.Remove(wishlist);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WishlistExists(int id)
        {
            return _context.Wishlist.Any(e => e.ID == id);
        }

        public async Task<bool> HasSameItem(int? id)
        {
            if (id == null)
            {
                return false;
            }

            var cart = await _context.Wishlist
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (cart == null)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> IncreaseProductQuantity(Wishlist wishlistID, int quantity)
        {

            var wishlist = await _context.Wishlist.FirstOrDefaultAsync(m => m.ProductID == wishlistID.ProductID);

            var increasedQuantity = wishlist.WishlistProductQuantity += quantity;
            if (increasedQuantity <= wishlist.Product.ProductQuantity)
            {
                wishlist.WishlistTotal = wishlist.WishlistProductQuantity * wishlist.Product.ProductPrice;
                await Edit(wishlist.ID, wishlist);
                return true;
            }
            return false;
        }
    }
}






//ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID", wishlist.ProductID);


//var productItem = await _context.Product.FirstOrDefaultAsync(m => m.ProductID == wishlist.ProductID);

//// GET: Wishlists/Details/5
//public async Task<IActionResult> Details(int? id)
//{
//    if (id == null)
//    {
//        return NotFound();
//    }

//    var wishlist = await _context.Wishlist
//        .Include(w => w.Product)
//        .FirstOrDefaultAsync(m => m.ID == id);
//    if (wishlist == null)
//    {
//        return NotFound();
//    }

//    return View(wishlist);
//}



//// GET: Wishlists/Delete/5
//public async Task<IActionResult> Delete(int? id)
//{
//    if (id == null)
//    {
//        return NotFound();
//    }

//    var wishlist = await _context.Wishlist
//        .Include(w => w.Product)
//        .FirstOrDefaultAsync(m => m.ID == id);
//    if (wishlist == null)
//    {
//        return NotFound();
//    }

//    return View(wishlist);
//}


//// GET: Wishlists/Edit/5
//public async Task<IActionResult> Edit(int? id)
//{
//    if (id == null)
//    {
//        return NotFound();
//    }

//    var wishlist = await _context.Wishlist.FindAsync(id);
//    if (wishlist == null)
//    {
//        return NotFound();
//    }
//    ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID", wishlist.ProductID);
//    return View(wishlist);
//}

//// GET: Wishlists/Create
//public IActionResult Create()
//{
//    ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID");
//    return View();
//}