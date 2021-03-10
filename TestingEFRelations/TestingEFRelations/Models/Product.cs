using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestingEFRelations.Models
{
    public class Product
    {
        public int ProductID { get; set; }

        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Display(Name = "Description")]
        public string ProductDescription { get; set; }
        [Display(Name = "Image ID")]
        public int ImageID { get; set; }
        [Display(Name = "Image")]
        public Image ProductImage { get; set; }
        [Display(Name = "Size ID")]
        public int SizeID { get; set; }
        [Display(Name = "Size")]
        public Size ProductSize { get; set; }
        [Display(Name = "Quantity")]
        public int ProductQuantity { get; set; }
        [Display(Name = "Price")]
        public double ProductPrice { get; set; }

        public List<Cart> Cart { get; set; }

        [Display(Name = "Small Image")]
        public List<SmlImage> ProductSmlImage { get; set; }




        //[InverseProperty("ProductName")]
        //public List<Wishlist> Wishlist_product_name { get; set; }

        //[InverseProperty("ProductImage")]
        //public List<Wishlist> Wishlist_Product_Image { get; set; }

        //[InverseProperty("ProductSize")]
        //public List<Wishlist> Wishlist_Product_Size { get; set; }

        //[InverseProperty("ProductPrice")]
        //public List<Wishlist> Wishlist_Product_Price { get; set; }


        //[InverseProperty("ProductName")]
        //public List<Cart> Product_Name { get; set; }

        //[InverseProperty("ProductImage")]
        //public List<Cart> Product_Image { get; set; }

        //[InverseProperty("ProductSize")]
        //public List<Cart> Product_Size { get; set; }

        //[InverseProperty("ProductPrice")]
        //public List<Cart> Product_Price { get; set; }
    }
}
