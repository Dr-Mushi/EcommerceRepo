using Microsoft.AspNetCore.Identity;

namespace TestingEFRelations.Models
{
    public class ApplicationUser : IdentityUser
    {
        //add new columns to the userIdentity table

        //foreign key from address table
       // public int AddressID { get; set; }
        //users will take address
        //public Address UserAddress { get; set; }
        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

       // public string UserPhN { get; set; }
    }
}
