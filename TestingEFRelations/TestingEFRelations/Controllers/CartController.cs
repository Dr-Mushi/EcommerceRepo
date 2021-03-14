using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestingEFRelations.Data;
using TestingEFRelations.Models;
using TestingEFRelations.Repositories;

namespace TestingEFRelations.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWishlistRepository _wishlistRepository;

        public CartController(ApplicationDbContext context, IWishlistRepository wishlistRepository)
        {
            _context = context;
            _wishlistRepository = wishlistRepository;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Cart.Include(c => c.Product)
                .Include(r => r.Product.ProductSize)
                .Include(r => r.Product.ProductImage);
                

            var getAllCartItems = await applicationDbContext.ToListAsync();

            double cartTotal = 0;

            foreach (var item in getAllCartItems)
            {
                cartTotal += item.CartTotal;
            }

            ViewData["cartTotal"] = cartTotal.ToString("0.00");

            return View(getAllCartItems);
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID");
            return View();
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
                var productItem = await _context.Product.FirstOrDefaultAsync(m => m.ProductID == cart.ProductID);
                cart.CartTotal = cart.CartProductQuantity * productItem.ProductPrice;

                //if wishlist has the same item, that was added to the cart, remove the item from the wishlist
                if (await _wishlistRepository.HasSameItem(cart.ProductID))
                {
                    await _wishlistRepository.DeleteSameItem(cart.ProductID);

                    if (await HasSameItem(cart.ProductID))
                    {
                        await IncreaseProductQuantity(cart, cart.CartProductQuantity);
                        return RedirectToRoute(new
                        {
                            controller = "Wishlist",
                            action = "index"
                        });
                    }

                    _context.Add(cart);
                    await _context.SaveChangesAsync();
                    return RedirectToRoute(new
                    {
                        controller = "Wishlist",
                        action = "index"
                    });
                }

                if (await HasSameItem(cart.ProductID))
                {
                    await IncreaseProductQuantity(cart, cart.CartProductQuantity);
                    return RedirectToAction(nameof(Index));

                }


                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID", cart.ProductID);
            return View(nameof(Index));
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID", cart.ProductID);
            return View(cart);
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
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID", cart.ProductID);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cart = await _context.Cart.FirstOrDefaultAsync(m => m.ProductID == id);
            _context.Cart.Remove(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
            return _context.Cart.Any(e => e.ID == id);
        }


        public async Task<bool> HasSameItem(int? id)
        {
            if (id == null)
            {
                return false;
            }

            var cart = await _context.Cart
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (cart == null)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> IncreaseProductQuantity(Cart cartID , int quantity)
        {
            
            
            var cart = await _context.Cart
                .FirstOrDefaultAsync(m => m.ProductID == cartID.ProductID);

            var increasedQuantity =  cart.CartProductQuantity += quantity;
            if (increasedQuantity <= cart.Product.ProductQuantity)
            {
                var productItem = await _context.Product.FirstOrDefaultAsync(m => m.ProductID == cart.ProductID);
                cart.CartTotal = cart.CartProductQuantity * productItem.ProductPrice;
                await Edit(cart.ID, cart);
                return true;
            }
            

            return false;


         
        }
    }
}
