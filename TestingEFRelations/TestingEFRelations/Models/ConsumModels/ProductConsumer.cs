using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingEFRelations.Models.ConsumModels
{
    public class ProductConsumer
    {

        public class Rootobject
        {
            public Class1[] Property1 { get; set; }
        }

        public class Class1
        {
            public int productID { get; set; }
            public string productName { get; set; }
            public string productDescription { get; set; }
            public Productimage1[] productImage { get; set; }
            public Productsize productSize { get; set; }
            public int productQuantity { get; set; }
            public float productPrice { get; set; }
        }

        public class Productsize
        {
            public int id { get; set; }
            public string sizeName { get; set; }
            public Product[] products { get; set; }
        }

        public class Product
        {
            public int productID { get; set; }
            public string productName { get; set; }
            public string productDescription { get; set; }
            public Productimage[] productImage { get; set; }
            public int sizeID { get; set; }
            public int productQuantity { get; set; }
            public float productPrice { get; set; }
            public object wishlist { get; set; }
            public object cart { get; set; }
            public object receipt { get; set; }
            public object imageFile { get; set; }
        }

        public class Productimage
        {
            public int id { get; set; }
            public string imageName { get; set; }
            public int productID { get; set; }
        }

        public class Productimage1
        {
            public int id { get; set; }
            public string imageName { get; set; }
            public int productID { get; set; }
            public Products products { get; set; }
        }

        public class Products
        {
            public int productID { get; set; }
            public string productName { get; set; }
            public string productDescription { get; set; }
            public Productimage3[] productImage { get; set; }
            public int sizeID { get; set; }
            public Productsize1 productSize { get; set; }
            public int productQuantity { get; set; }
            public float productPrice { get; set; }
            public object wishlist { get; set; }
            public object cart { get; set; }
            public object receipt { get; set; }
            public object imageFile { get; set; }
        }

        public class Productsize1
        {
            public int id { get; set; }
            public string sizeName { get; set; }
            public Product1[] products { get; set; }
        }

        public class Product1
        {
            public int productID { get; set; }
            public string productName { get; set; }
            public string productDescription { get; set; }
            public Productimage2[] productImage { get; set; }
            public int sizeID { get; set; }
            public int productQuantity { get; set; }
            public float productPrice { get; set; }
            public object wishlist { get; set; }
            public object cart { get; set; }
            public object receipt { get; set; }
            public object imageFile { get; set; }
        }

        public class Productimage2
        {
            public int id { get; set; }
            public string imageName { get; set; }
            public int productID { get; set; }
        }

        public class Productimage3
        {
            public int id { get; set; }
            public string imageName { get; set; }
            public int productID { get; set; }
        }

    }
}
