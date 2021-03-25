using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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
        private readonly IAccountRepository _accountRepository;

        public ProductApiController(IImageRepository image,
            IProductRepository product,
            IMapper mapper,
            IAccountRepository accountRepo)
        {
            _image = image;
            _product = product;
            _mapper = mapper;
            _accountRepository = accountRepo;
        }

        // GET: api/ProductApi
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles ="Admin")]
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


        // POST: api/ProductApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ProductReadDto>> PostProduct([FromBody]/*[Bind("ProductID,ProductName,ProductDescription,ImageFile,SizeID,ProductQuantity,ProductPrice")] */ProductCreateDto product)
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
                return CreatedAtRoute(nameof(GetProduct), new { Id = productRead.ProductID }, productRead);
            }
            return BadRequest();
        }



        //// PUT: api/ProductApi/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for
        //// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<ActionResult> PutProduct(int id, [FromBody] /*[Bind("ProductID,ProductName,ProductDescription,ImageFile,SizeID,ProductQuantity,ProductPrice")] */ProductUpdateDto product)
        {
            var getProduct =await _product.FindProduct(id);

            if (getProduct == null)
            {
                return BadRequest();
            }

            //No image will be updated only content
             _mapper.Map(product, getProduct);

            //no need for this, but when we don't need a mapper it's a good
            //practice to have the update method ready
            _product.ProductUpdate(getProduct);

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

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> PatchProduct(int id, [FromBody][Bind("ProductID,ProductName,ProductDescription,ImageFile,SizeID,ProductQuantity,ProductPrice")]JsonPatchDocument<ProductUpdateDto> patchDoc)
        {
            if (ModelState.IsValid)
            {
                var getProduct = await _product.FindProduct(id);

                if (getProduct == null)
                {
                    return BadRequest();
                }
                var productToPatch = _mapper.Map<ProductUpdateDto>(getProduct);

                patchDoc.ApplyTo(productToPatch,ModelState);

                if (!TryValidateModel(productToPatch))
                {
                    return ValidationProblem(ModelState);
                }

                //No image will be updated only content
                _mapper.Map(productToPatch, getProduct);

                //no need for this, but when we don't need a mapper it's a good
                //practice to have the update method ready
                _product.ProductUpdate(getProduct);

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


            }
            return NoContent();
        }
        [HttpGet("Login")]
        public async Task<ActionResult> Login([FromBody]SignIn signIn)
        {
            if (ModelState.IsValid)
            {
                //var result = await _accountRepository.SignIn(signIn);
                string token =  _accountRepository.AuthToken(/*result,*/ signIn);
                //if (result.Succeeded)
                //{
                    return Ok(new {access_token = token });
                //}
               
            }

            return BadRequest();
            
        }

        [HttpGet("LogOut")]
        public async Task<ActionResult> LogOut()
        {
            if (ModelState.IsValid)
            {
                await _accountRepository.SignOut();
               
                return Ok(true);
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
