using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestingEFRelations.Models;
using TestingEFRelations.Repositories.Interface;

namespace TestingEFRelations.Controllers
{
    public class WishlistController : Controller
    {
        private readonly IWishlistRepository _wishlist;

        public WishlistController(IWishlistRepository wishlist)
        {
            _wishlist = wishlist;
        }

        // GET: Wishlists
        public async Task<IActionResult> Index()
        {
            var getAllWishlistItems = await _wishlist.GetWishlistItems();

            ViewData["wishlistTotal"] = _wishlist.WishlistSumTotal(getAllWishlistItems).ToString("0.00");

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

                await _wishlist.SetWishlistTotal(wishlist);

                //if wishlist has the same item that was created , increase the quantity of that item.
                if (await _wishlist.HasSameItem(wishlist.ProductID))
                {
                    await IncreaseProductQuantity(wishlist, wishlist.WishlistProductQuantity);
                    return RedirectToAction(nameof(Index));
                }

                _wishlist.AddWishlist(wishlist);
                await _wishlist.SaveWishlist();

                return RedirectToAction(nameof(Index));
            }
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
                    _wishlist.WishlistUpdate(wishlist);
                    await _wishlist.SaveWishlist();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_wishlist.WishlistExists(wishlist.ID))
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
            await _wishlist.DeleteWishlist(id);
            await _wishlist.SaveWishlist();
            return RedirectToAction(nameof(Index));
        }

        public async Task<bool> IncreaseProductQuantity(Wishlist wishlistID, int quantity)
        {

            var wishlist = _wishlist.IncreaseProductQuantity(wishlistID, quantity);

            await Edit(wishlist.Result.ID, wishlist.Result);
            return true;
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


//ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID", wishlist.ProductID);
















//var applicationDbContext = _context.Wishlist.Include(w => w.Product)
//    .Include(c => c.Product.ProductImage)
//    .Include(c => c.Product.ProductSize);

//var getAllWishlistItems = await applicationDbContext.ToListAsync();

//sum of all wishlist tables totals
//foreach (var item in getAllWishlistItems)
//{
//    wishlistTotal += item.WishlistTotal;
//}




//public async Task<bool> HasSameItem(int? id)
//{
//    if (id == null)
//    {
//        return false;
//    }

//    var cart = await _context.Wishlist
//        .FirstOrDefaultAsync(m => m.ProductID == id);
//    if (cart == null)
//    {
//        return false;
//    }

//    return true;
//}



////get product object that has the same ID as the wishlist product
//var productItem = await _context.Product.FirstOrDefaultAsync(m => m.ProductID == wishlist.ProductID);


//wishlist.WishlistTotal = wishlist.WishlistProductQuantity * productItem.ProductPrice;


//_context.Add(wishlist);
//await _context.SaveChangesAsync();


//var wishlist = await _context.Wishlist.FirstOrDefaultAsync(m => m.ProductID == id);
//_context.Wishlist.Remove(wishlist);
//await _context.SaveChangesAsync();



//    //get the product Object inside wishlist with the existed wishlist product ID
//    //var wishlist = await _context.Wishlist.FirstOrDefaultAsync(m => m.ProductID == wishlistID.ProductID);

//    var wishlist = await _wishlist.FindWishlist(wishlistID.ProductID);

//    var increasedQuantity = wishlist.WishlistProductQuantity += quantity;
//    //check if the increased quantity does NOT exceed the qunatity of the product.
//    if (increasedQuantity <= wishlist.Product.ProductQuantity)
//    {
//        wishlist.WishlistTotal = wishlist.WishlistProductQuantity * wishlist.Product.ProductPrice;
//        await Edit(wishlist.ID, wishlist);
//        return true;
//    }
//    return false;
//}
