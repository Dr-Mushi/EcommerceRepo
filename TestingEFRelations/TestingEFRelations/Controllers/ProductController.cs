using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestingEFRelations.Data;
using TestingEFRelations.Models;

namespace TestingEFRelations.Controllers
{
   //[ApiController]
   // [Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            //
            var applicationDbContext = _context.Product.Include(p => p.ProductImage).Include(p => p.ProductSize);
            var result = await applicationDbContext.ToListAsync();
            return View(result);
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }



            var product = await _context.Product
                .Include(p => p.ProductImage)
                .Include(p => p.ProductSize)
                .FirstOrDefaultAsync(m => m.ProductID == id);
                

            //var temp = await _context.Image
            //    .Include(p => p.SmlImages)
            //    .Include(p => p.Products)
            //    .FirstOrDefaultAsync(i => i.ID == product.ImageID);



            //var result = await temp.FirstOrDefaultAsync(i => i.ID == product.ImageID);




            //    .where(m => m.ImageID == product.ImageID)
            //    .ToListAsync();

            //var temp = from m in _context.SmlImage
            //         select m;

            // var temps = temp.Where(m => m.ImageID == product.ImageID);

            // ViewData["smlImages"] = temps;


            //var getSmlImages = await _context.SmlImage
            //    .Find()
            //    .where(m => m.ImageID == product.ImageID)
            //    .ToListAsync();

            //ViewBag.smlImages = getSmlImages.SmlImageName;

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            
            ViewData["ImageName"] = new SelectList(_context.Image, "ID", "ImageName");
            ViewData["SizeName"] = new SelectList(_context.Set<Size>(), "ID", "SizeName");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,ProductName,ProductDescription,ImageID,SizeID,ProductQuantity,ProductPrice")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ImageName"] = new SelectList(_context.Image, "ID", "ImageName", product.ProductImage);
            ViewData["SizeName"] = new SelectList(_context.Set<Size>(), "ID", "SizeName", product.SizeID);
            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ImageName"] = new SelectList(_context.Image, "ID", "ImageName", product.ProductImage);
            ViewData["SizeName"] = new SelectList(_context.Set<Size>(), "ID", "SizeName", product.SizeID);
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
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductID))
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
            ViewData["ImageName"] = new SelectList(_context.Image, "ID", "ImageName", product.ProductImage);
            ViewData["SizeName"] = new SelectList(_context.Set<Size>(), "ID", "SizeName", product.SizeID);
            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.ProductImage)
                .Include(p => p.ProductSize)
                .FirstOrDefaultAsync(m => m.ProductID == id);
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
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductID == id);
        }

        //[Route("api/[controller]/[action]")]
        //public string Get() {

        //    return "hello";
        //}
    }
}
