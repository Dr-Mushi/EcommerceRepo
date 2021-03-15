using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestingEFRelations.Models
{
    public class Image
    {
        public int ID { get; set; }
        public string ImageName { get; set; }

        public int ProductID { get; set; }
        public Product Products { get; set; }



    }
}


//public List<SmlImage> SmlImages { get; set; }