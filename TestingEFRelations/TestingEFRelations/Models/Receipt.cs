using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestingEFRelations.Models
{
    public class Receipt
    {
        [Display(Name ="Receipt ID")]
        public int ID { get; set; }
        //public int? CartID { get; set; }

      
        //public Cart Cart { get; set; }
        public int ProductID { get; set; }

        public Product Product { get; set; }


        public int ReceiptProductQuantity { get; set; }


        public double ReceiptTotal { get; set; }

    }
}
