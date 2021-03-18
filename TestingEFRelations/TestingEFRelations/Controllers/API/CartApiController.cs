using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestingEFRelations.Models;
using TestingEFRelations.Repositories.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestingEFRelations.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartApiController : ControllerBase
    {
        private readonly IWishlistRepository _wishlist;
        private readonly ICartRepository _cart;
        public CartApiController(ICartRepository cart,
            IWishlistRepository wishlist)
        {
            _wishlist = wishlist;
            _cart = cart;
        }
        // GET: api/<CartApi>
        [HttpGet]
        public async Task<ActionResult> GetAllCarts()
        {
            return Ok(await _cart.GetCartItems());
        }

        // GET api/<CartApi>/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetCart(int id)
        {
            return Ok(await _cart.FindCart(id));
        }

        // POST: api/<CartApi>
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult> PostCart([Bind("ID,ProductID,CartProductQuantity")]  Cart cart)
        {

            if (ModelState.IsValid)
            {
                await _cart.SetCartTotal(cart);

                //BAD LOGIC
                //if wishlist has the same item that was added to the cart, remove the item from the wishlist
                if (await _wishlist.HasSameItem(cart.ProductID))
                {
                    
                     await _wishlist.DeleteSameItem(cart.ProductID);

                     await _wishlist.SaveWishlist();

                }

                //if cart has the same item that was created, increase the quantity of that item.
                if (await _cart.HasSameItem(cart.ProductID))
                {
                    await IncreaseProductQuantity(cart, cart.CartProductQuantity);
                    return Ok(GetCart(cart.ProductID).Result);
                }

                _cart.AddCart(cart);
                await _cart.SaveCart();
                return Ok(GetCart(cart.ProductID).Result);
            }
            return BadRequest();
        }

        // PUT api/<CartApi>/5
        //[HttpPut("{id}")]
        public async Task<IActionResult> PutCart(int id, [Bind("ID,ProductID,CartProductQuantity")] Cart cart)
        {
            if (id != cart.ID)
            {
                return BadRequest();
            }

            _cart.CartUpdate(cart);

            try
            {
                await _cart.SaveCart();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_cart.CartExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(cart);
        }

        // DELETE: api/CartApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCart(int id)
        {
            await _cart.DeleteCart(id);
           return Ok(await _cart.SaveCart());      
        }



        public async Task<bool> IncreaseProductQuantity(Cart cartID, int quantity)
        {

            var cart = _cart.IncreaseProductQuantity(cartID, quantity);

            await PutCart(cart.Result.ID, cart.Result);
            return true;
        }
    }
}
