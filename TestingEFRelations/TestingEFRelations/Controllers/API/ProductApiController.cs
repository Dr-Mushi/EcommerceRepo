using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestingEFRelations.Data;
using TestingEFRelations.Dtos.ProductDto;
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
        private readonly IMapper _mapper;

        public ProductApiController(IImageRepository image,
            IProductRepository product,
            IMapper mapper)
        {
            _image = image;
            _product = product;
            _mapper = mapper;
        }

        // GET: api/ProductApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductReadDto>>> GetAllProducts()
        {
            var getProductItems = await _product.GetProductItems();
            return Ok(_mapper.Map<IEnumerable<ProductReadDto>>(getProductItems));
        }
        //// GET: api/ProductApi/5
        [HttpGet("{id}", Name = "GetProduct") ]
        public async Task<ActionResult<ProductReadDto>> GetProduct(int id)
        {
            var getProduct = await _product.FindProduct(id);
            if (getProduct != null)
            {
                return Ok(_mapper.Map<ProductReadDto>(getProduct));
                
            }
            return NotFound();
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
        public async Task<ActionResult<ProductReadDto>> PostProduct([FromForm][Bind("ProductID,ProductName,ProductDescription,ImageFile,SizeID,ProductQuantity,ProductPrice")] ProductCreateDto product)
        {
            if (ModelState.IsValid)
            {
                var productCreate = _mapper.Map<Product>(product);
                _product.AddProduct(productCreate);
                await _product.SaveProduct();

                //BAD LOGIC <the product is saved first then the image will be inserted,
                //which will be bad if the project stopped for some reason 
                //before the image was added>.
                await _image.AddImage(productCreate);


                var productRead = _mapper.Map<ProductReadDto>(productCreate);
                return CreatedAtRoute(nameof(GetProduct) , new {Id = productRead.ProductID } , productRead);
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
