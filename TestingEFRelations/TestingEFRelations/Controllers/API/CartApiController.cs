using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ICartRepository _cart;

        public CartApiController(ICartRepository cart)
        {
            _cart = cart;
        }
        // GET: api/<CartApiController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cart>>> GetAllCarts()
        {
            return Ok(await _cart.GetCartItems());
        }

        // GET api/<CartApiController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetCart(int id)
        {
            return Ok(await _cart.FindCart(id));
        }

        // POST: api/CartApiController
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Cart>> PostProduct(Cart cart)
        {
            _cart.AddCart(cart);
            await _cart.SaveCart();

            return CreatedAtAction("GetProduct", new { id = cart.ProductID }, cart);
        }

        // PUT api/<CartApiController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCart(int id, Cart cart)
        {
            if (id != cart.ProductID)
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

        //// DELETE api/<CartApiController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //     Ok(_cart.DeleteCart(id));
        //}

        // DELETE: api/ProductApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Cart>> DeleteCart(int id)
        {
            await _cart.DeleteCart(id);
           return Ok(await _cart.SaveCart());      
        }
    }
}
