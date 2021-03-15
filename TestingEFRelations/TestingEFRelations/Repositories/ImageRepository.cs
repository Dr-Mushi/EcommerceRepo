using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TestingEFRelations.Data;
using TestingEFRelations.Models;

namespace TestingEFRelations.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ImageRepository(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        //create an image from user input
        public async Task<bool> AddImage(Product product)
        {
            ////get all products
            //var getProductModel = _context.Product.Include(p => p.ProductImage);
            //var productItems = await getProductModel.ToListAsync();

            //var getImageModel = _context.Image;
            //var ImageItems = await getImageModel.ToListAsync();

            //image path
            string imageFolder;
            Image image;
 
            //if the input has value create a new image file
            if (product.ImageFile != null)
            {
                //Guid.NewGuid().ToString() + "_" +
                foreach (var item in product.ImageFile)
                {
                    imageFolder = "Images/";
                    imageFolder += Guid.NewGuid().ToString() + "_" + item.FileName;
                    string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, imageFolder);
                    await item.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                     image = new Image
                    {
                        ProductID = product.ProductID,
                        ImageName = imageFolder
                    };
                    _context.Add(image);
                }




                //imageFolder += Guid.NewGuid().ToString() + "_" + product.ImageFile.FileName;
                //string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, imageFolder);
                //await product.ImageFile.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
            }
            //create a placeholder if the value is null
            else
            {
                imageFolder = "/Images/";
                imageFolder += "PlaceholderImage.jpg";
                string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, imageFolder);
            }

            //add the new image path into the Image database
            //Image image = new Image
            //{
            //    ProductID = product.ProductID,
            //    ImageName = imageFolder
            //};

           
            await _context.SaveChangesAsync();

            return true;
        }




    }
}
