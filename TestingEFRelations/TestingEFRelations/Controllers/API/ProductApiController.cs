using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestingEFRelations.Data;
using TestingEFRelations.Models;
using TestingEFRelations.Repositories;
using TestingEFRelations.Repositories.Interface;

namespace TestingEFRelations.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly IImageRepository _image;
        private readonly IProductRepository _product;

        public ProductApiController(IImageRepository image, IProductRepository product)
        {
            _image = image;
            _product = product;
        }

        // GET: api/ProductApi
        [HttpGet]
        public async Task<ActionResult> GetAllProducts()
        {
            return Ok(await _product.GetProductItems());
        }
        //// GET: api/ProductApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetProduct(int id)
        {
            return Ok(await _product.FindProduct(id));
        }


        //// PUT: api/ProductApi/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for
        //// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductID)
            {
                return BadRequest();
            }

            _product.ProductUpdate(product);

            try
            {
                await _product.SaveProduct();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_product.ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(product);
        }

        // POST: api/ProductApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<IActionResult> PostProduct([FromForm][Bind("ProductID,ProductName,ProductDescription,ImageFile,SizeID,ProductQuantity,ProductPrice")] Product product)
        {
            if (ModelState.IsValid)
            {
                _product.AddProduct(product);
                await _product.SaveProduct();

                //BAD LOGIC <the product is saved first then the image will be inserted,
                //which will be bad if the project stopped for some reason 
                //before the image was added>.
                await _image.AddImage(product);

                return Ok(product);
            }
            return BadRequest();
        }

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
