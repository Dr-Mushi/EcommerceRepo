using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingEFRelations.Models
{
    public class Image
    {
        public int ID { get; set; }
        public string ImageName { get; set; }

        public List<Product> Products { get; set; }
        public List<SmlImage> SmlImages { get; set; }
    }
}
