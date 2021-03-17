using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestingEFRelations.Models;
using TestingEFRelations.Repositories;
using TestingEFRelations.Repositories.Interface;

namespace TestingEFRelations.Controllers
{
    public class ProductController : Controller
    {
        private readonly IImageRepository _image;
        private readonly IProductRepository _product;

        public ProductController(IImageRepository image,
            IProductRepository product)
        {
            _image= image;
            _product = product;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            return View(await _product.GetProductItems());
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _product.FindProduct(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            ViewData["SizeName"] = _product.SelectListSize();
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,ProductName,ProductDescription,ImageFile,SizeID,ProductQuantity,ProductPrice")] Product product)
        {
            if (ModelState.IsValid)
            {
                //after adding the product, take the product object as it has the new product id and add it to the image
                _product.AddProduct(product);
                await _product.SaveProduct();

                await _image.AddImage(product);

                return RedirectToAction(nameof(Index));
            }

            ViewData["SizeName"] = _product.SelectListSize();
            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _product.FindProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["SizeName"] = _product.SelectListSize();
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,ProductName,ProductDescription,ImageID,SizeID,ProductQuantity,ProductPrice")] Product product)
        {
            if (id != product.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _product.ProductUpdate(product);
                    await _product.SaveProduct();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_product.ProductExists(product.ProductID))
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
            ViewData["SizeName"] = _product.SelectListSize();
            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _product.FindProduct(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _product.DeleteProduct(id);
            await _product.SaveProduct();
            return RedirectToAction(nameof(Index));
        }


    }
}


//var applicationDbContext = _context.Product
//    .Include(p => p.ProductImage)
//    .Include(p => p.ProductSize);
//var result = await applicationDbContext.ToListAsync();


//var product = await _context.Product
//    .Include(p => p.ProductImage)
//    .Include(p => p.ProductSize)
//    .FirstOrDefaultAsync(m => m.ProductID == id);


