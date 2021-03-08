using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestingEFRelations.Models
{
    public class Wishlist
    {
        public int ID { get; set; }
        [ForeignKey("Product")]
        public int ProductID { get; set; }
        public Product Product { get; set; }

        //[ForeignKey("ProductName")]
        ////[InverseProperty("Product_Name")]
        //public int Product_Name { get; set; }
        //public Product ProductName { get; set; }
        //[ForeignKey("ProductImage")]
        ////[InverseProperty("Product_Image")]
        //public int Product_Image { get; set; }
        //public Product ProductImage { get; set; }
        //[ForeignKey("ProductSize")]
        ////[InverseProperty("Product_Size")]
        //public int Product_Size { get; set; }
        //public Product ProductSize { get; set; }
        //[ForeignKey("ProductPrice")]
        ////[InverseProperty("Product_Price")]
        //public int Product_Price { get; set; }
        //public Product ProductPrice { get; set; }

        public int WishlistProductQuantity { get; set; }
    }
}
