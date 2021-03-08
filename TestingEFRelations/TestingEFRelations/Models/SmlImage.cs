using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingEFRelations.Models
{
    public class SmlImage
    {
        public int ID { get; set; }
        public string SmlImageName { get; set; }
        public int ImageID { get; set; }

        public Image Image { get; set; }


        //an image has multiple smlImages, and smlImages have one Image
    }
}
