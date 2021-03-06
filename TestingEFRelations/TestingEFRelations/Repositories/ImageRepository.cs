using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Text;
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


            //if (product.ImageFile.Count > 0)
            //{
            //    foreach (var item in product.ImageFile)
            //    {
            //        byte[] bytes = Convert.FromBase64String(item);
            //        imageFolder = "Images/";
            //        imageFolder += Guid.NewGuid().ToString() + "_" + item;
            //        //combine server path with the image name
            //        string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, imageFolder);

            //        if (bytes.Length > 0)
            //        {
            //            using (var stream = new FileStream(serverFolder, FileMode.Create))
            //            {
            //                stream.Write(bytes, 0, bytes.Length);
            //                stream.Flush();
            //            }
            //        }

            //        image = new Image
            //        {
            //            ProductID = product.ProductID,
            //            ImageName = imageFolder
            //        };
            //        _context.Add(image);
            //    }
            //}



            //await _context.SaveChangesAsync();

            //return true;




            //if (product.ImageFileName != null)
            //{
            //    imageFolder = "Images/";
            //    imageFolder += Guid.NewGuid().ToString() + "_" + product.ImageFileExtension;
            //    string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, imageFolder);
            //    byte[] stringToByte = Convert.FromBase64String(product.ImageFileData);
            //    await File.WriteAllBytesAsync(serverFolder, stringToByte);

            //    image = new Image
            //    {
            //        ProductID = product.ProductID,
            //        ImageName = imageFolder
            //    };
            //    _context.Add(image);

            //    await _context.SaveChangesAsync();

            //    return true;
            //}

            //Images sent from API
            if (product.ImageFileName != null)
            {
               for (int i = 0; i < product.ImageFileName.Count; i++)
               {
                   //imageFolder = "Images/";
                   imageFolder = "Images/"+ Guid.NewGuid().ToString() + "_" + product.ImageFileExtension[i];
                   string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, imageFolder);
                   byte[] stringToByte = Convert.FromBase64String(product.ImageFileData[i]);
                   await File.WriteAllBytesAsync(serverFolder, stringToByte);
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

            //Images sent from Views
            //if the input has value create a new image file
            if (product.ImageFile.Count > 0)
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