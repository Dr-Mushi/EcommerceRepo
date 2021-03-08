using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingEFRelations.Models
{
    public class Size
    {
        public int ID { get; set; }
        public string SizeName { get; set; }

        public List<Product> Products { get; set; }
    }
}
