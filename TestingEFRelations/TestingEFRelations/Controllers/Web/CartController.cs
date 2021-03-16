using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestingEFRelations.Data;
using TestingEFRelations.Models;
using TestingEFRelations.Repositories.Interface;

namespace TestingEFRelations.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWishlistRepository _wishlistRepository;
        private readonly ICartRepository _cart;
        public CartController(ApplicationDbContext context,ICartRepository cart,
            IWishlistRepository wishlistRepository
            )
        {
            _context = context;
            _wishlistRepository = wishlistRepository;
            _cart = cart;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            var getAllcartItems = await _cart.GetCartItems();

            ViewData["cartTotal"] = _cart.CartSumTotal(getAllcartItems).ToString("0.00");

            return View(getAllcartItems);
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ProductID,CartProductQuantity")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                //get product object that has the same ID as the cart product
                //var productItem = await _context.Product.FirstOrDefaultAsync(m => m.ProductID == cart.ProductID);
                //cart.CartTotal = cart.CartProductQuantity * productItem.ProductPrice;

                await _cart.SetCartTotal(cart);

                //if wishlist has the same item that was added to the cart, remove the item from the wishlist
                if (await _wishlistRepository.HasSameItem(cart.ProductID))
                {
                    
                    await _wishlistRepository.DeleteSameItem(cart.ProductID);

                    //if cart has the same item that was created , increase the quantity of that item.
                    if (await _cart.HasSameItem(cart.ProductID))
                    {
                        await IncreaseProductQuantity(cart, cart.CartProductQuantity);
                        return RedirectToRoute(new
                        {
                            controller = "Wishlist",
                            action = "index"
                        });
                    }

                    _cart.AddCart(cart);
                    await _cart.SaveCart();
                    return RedirectToRoute(new
                    {
                        controller = "Wishlist",
                        action = "index"
                    });
                }

                //if cart has the same item that was created, increase the quantity of that item.
                if (await _cart.HasSameItem(cart.ProductID))
                {
                    await IncreaseProductQuantity(cart, cart.CartProductQuantity);
                    return RedirectToAction(nameof(Index));
                }


                _cart.AddCart(cart);
                await _cart.SaveCart();
                return RedirectToAction(nameof(Index));
            }
            return View(nameof(Index));
        }


        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ProductID,CartProductQuantity")] Cart cart)
        {
            if (id != cart.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.ID))
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
            return NotFound();
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _cart.DeleteCart(id);
            await _cart.SaveCart();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
            return _context.Cart.Any(e => e.ID == id);
        }

        public async Task<bool> IncreaseProductQuantity(Cart cartID , int quantity)
        {
            
            //get the product Object inside cart with the existed cart product ID
            var cart = await _context.Cart
                .FirstOrDefaultAsync(m => m.ProductID == cartID.ProductID);

            var increasedQuantity =  cart.CartProductQuantity += quantity;
            //check if the increased quantity does NOT exceed the qunatity of the product.
            if (increasedQuantity <= cart.Product.ProductQuantity)
            {
               //refresh the cart total after quantity increased
                cart.CartTotal = cart.CartProductQuantity * cart.Product.ProductPrice;
                await Edit(cart.ID, cart);
                return true;
            }
            return false;
        }
    }
}


//inside increased quantity method
// var productItem = await _context.Product.FirstOrDefaultAsync(m => m.ProductID == cart.ProductID);



//ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID", cart.ProductID);

// GET: Carts/Delete/5
//public async Task<IActionResult> Delete(int? id)
//{
//    if (id == null)
//    {
//        return NotFound();
//    }

//    var cart = await _context.Cart
//        .Include(c => c.Product)
//        .FirstOrDefaultAsync(m => m.ID == id);
//    if (cart == null)
//    {
//        return NotFound();
//    }

//    return View(cart);
//}


// GET: Carts/Edit/5
//public async Task<IActionResult> Edit(int? id)
//{
//    if (id == null)
//    {
//        return NotFound();
//    }

//    var cart = await _context.Cart.FindAsync(id);
//    if (cart == null)
//    {
//        return NotFound();
//    }
//    ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID", cart.ProductID);
//    return View(cart);
//}




// GET: Carts/Create
//public IActionResult Create()
//{
//    ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID");
//    return View();
//}



//// GET: Carts/Details/5
//public async Task<IActionResult> Details(int? id)
//{
//    if (id == null)
//    {
//        return NotFound();
//    }

//    var cart = await _context.Cart
//        .Include(c => c.Product)
//        .FirstOrDefaultAsync(m => m.ID == id);
//    if (cart == null)
//    {
//        return NotFound();
//    }

//    return View(cart);
//}



//var applicationDbContext = _context.Cart.Include(c => c.Product)
//    .Include(r => r.Product.ProductSize)
//    .Include(r => r.Product.ProductImage);


//var getAllCartItems = await applicationDbContext.ToListAsync();

//double cartTotal = 0;

////sum of all cart tables totals
//foreach (var item in getAllCartItems)
//{
//    cartTotal += item.CartTotal;
//}

//ViewData["cartTotal"] = cartTotal.ToString("0.00");

//return View(getAllCartItems);





//public async Task<bool> HasSameItem(int? id)
//{
//    if (id == null)
//    {
//        return false;
//    }

//    var cart = await _context.Cart
//        .FirstOrDefaultAsync(m => m.ProductID == id);
//    if (cart == null)
//    {
//        return false;
//    }

//    return true;
//}
