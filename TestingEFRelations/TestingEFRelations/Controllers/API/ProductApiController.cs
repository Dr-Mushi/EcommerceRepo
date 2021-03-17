﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestingEFRelations.Data;
using TestingEFRelations.Models;
using TestingEFRelations.Repositories.Interface;

namespace TestingEFRelations.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly IProductRepository _product;

        public ProductApiController(IProductRepository product)
        {
            _product = product;
        }

        // GET: api/ProductApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {
            var getAllItems = await _product.GetProductItems();
            return Ok(getAllItems);
        }

        //// GET: api/ProductApi/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Product>> GetProduct(int id)
        //{
        //    var product = await _context.Product.FindAsync(id);

        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return product;
        //}

        //// PUT: api/ProductApi/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for
        //// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutProduct(int id, Product product)
        //{
        //    if (id != product.ProductID)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(product).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ProductExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/ProductApi
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for
        //// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPost]
        //public async Task<ActionResult<Product>> PostProduct(Product product)
        //{
        //    _context.Product.Add(product);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetProduct", new { id = product.ProductID }, product);
        //}

        //// DELETE: api/ProductApi/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Product>> DeleteProduct(int id)
        //{
        //    var product = await _context.Product.FindAsync(id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Product.Remove(product);
        //    await _context.SaveChangesAsync();

        //    return product;
        //}

        //private bool ProductExists(int id)
        //{
        //    return _context.Product.Any(e => e.ProductID == id);
        //}
    }
}
