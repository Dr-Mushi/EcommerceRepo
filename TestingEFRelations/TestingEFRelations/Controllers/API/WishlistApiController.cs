using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingEFRelations.Models;
using TestingEFRelations.Repositories.Interface;

namespace TestingEFRelations.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistApiController : ControllerBase
    {
        private readonly IWishlistRepository _wishlist;
        public WishlistApiController(IWishlistRepository wishlist)
        {
            _wishlist = wishlist;
        }
        // GET api/<WishlistApi>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Wishlist>>> GetAllWishlists()
        {
            return Ok(await _wishlist.GetWishlistItems());
        }
        // GET api/<WishlistApi>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Wishlist>>> GetWishlist(int id)
        {
            return Ok(await _wishlist.FindWishlist(id));
        }

        // POST: api/WishlistApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Wishlist>> PostWishlist([Bind("ID,ProductID,CartProductQuantity")] Wishlist wishlist)
        {

            if (ModelState.IsValid)
            {
                await _wishlist.SetWishlistTotal(wishlist);

                //if wishlist has the same item that was added to the cart, remove the item from the wishlist
                if (await _wishlist.HasSameItem(wishlist.ProductID))
                {
                    await IncreaseProductQuantity(wishlist, wishlist.WishlistProductQuantity);
                    return Ok(GetWishlist(wishlist.ProductID).Result);

                }
                _wishlist.AddWishlist(wishlist);
                await _wishlist.SaveWishlist();
                return Ok(GetWishlist(wishlist.ProductID).Result);
            }
            return BadRequest();
        }


        // PUT api/<WishlistApi>/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut("{id}")]
       
        public async Task<IActionResult> PutWishlist(int id, [Bind("ID,ProductID,WishlistProductQuantity")] Wishlist wishlist)
        {
            if (id != wishlist.ID)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _wishlist.SetWishlistTotal(wishlist);
                    _wishlist.WishlistUpdate(wishlist);
                    await _wishlist.SaveWishlist();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_wishlist.WishlistExists(wishlist.ID))
                    {
                        return BadRequest();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Ok(wishlist);
            }
            return BadRequest();
        }

        // DELETE api/<WishlistApi>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Cart>> DeleteCart(int id)
        {
            await _wishlist.DeleteWishlist(id);
            return Ok(await _wishlist.SaveWishlist());
        }


        public async Task<bool> IncreaseProductQuantity(Wishlist wishlistID, int quantity)
        {

            var wishlist = _wishlist.IncreaseProductQuantity(wishlistID, quantity);

            await PutWishlist(wishlist.Result.ID, wishlist.Result);
            return true;
        }

    }
}
