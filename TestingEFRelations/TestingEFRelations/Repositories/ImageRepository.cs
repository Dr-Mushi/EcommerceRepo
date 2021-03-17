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
            //image path
            string imageFolder;
            Image image;
 
            //if the input has value create a new image file
            if (product.ImageFile != null)
            {
                foreach (var item in product.ImageFile)
                {
                    imageFolder = "Images/";
                    imageFolder += Guid.NewGuid().ToString() + "_" + item.FileName;
                    //combine server path with the image name
                    string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, imageFolder);
                    //Create new image
                    await item.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                    //add new image to image table
                     image = new Image
                    {
                        ProductID = product.ProductID,
                        ImageName = imageFolder
                    };
                    _context.Add(image);
                }
            }
            //create a placeholder if the user didn't insert an image
            else
            {
                imageFolder = "Images/";
                imageFolder += "PlaceholderImage.jpg";
                string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, imageFolder);
                image = new Image
                {
                    ProductID = product.ProductID,
                    ImageName = imageFolder
                };
                _context.Add(image);
            }  
            await _context.SaveChangesAsync();

            return true;
        }
    }
}





//for single image

//imageFolder += Guid.NewGuid().ToString() + "_" + product.ImageFile.FileName;
//string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, imageFolder);
//await product.ImageFile.CopyToAsync(new FileStream(serverFolder, FileMode.Create));