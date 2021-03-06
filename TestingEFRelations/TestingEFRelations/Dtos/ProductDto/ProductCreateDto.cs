using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TestingEFRelations.Models;

namespace TestingEFRelations.Dtos.ProductDto
{
    public class ProductCreateDto
    {
        //public int ProductID { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string ProductDescription { get; set; }

        //[Display(Name = "Image")]
        //public List<Image> ProductImage { get; set; }
        [Required]
        [Display(Name = "Size ID")]
        public int SizeID { get; set; }

        //[Display(Name = "Size")]
        //public Size ProductSize { get; set; }
        [Required]
        [Display(Name = "Quantity")]
        public int ProductQuantity { get; set; }
        [Required]
        [Display(Name = "Price")]
        public double ProductPrice { get; set; }

        //public List<Wishlist> Wishlist { get; set; }
        //public List<Cart> Cart { get; set; }

        //public List<Receipt> Receipt { get; set; }

        //[Display(Name = "Choose product images")]
        //[NotMapped]
        //public IFormFile ImageFile { get; set; }

        [Display(Name = "Choose product images")]
        [NotMapped]
        public List<IFormFile> ImageFile { get; set; }

        [NotMapped]
        public List<string> ImageFileName { get; set; }
        [NotMapped]
        public List<string> ImageFileExtension { get; set; }
        [NotMapped]
        public List<string> ImageFileData { get; set; }
    }
}
