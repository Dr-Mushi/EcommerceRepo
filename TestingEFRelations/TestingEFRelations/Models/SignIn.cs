using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestingEFRelations.Models
{
    public class SignIn
    {
        [Required(ErrorMessage = "Please enter your Email/User name")]
        [Display(Name = "Email/User name")]
        [DataType(DataType.Text)]
        public string EmailOrName { get; set; }

        [Required(ErrorMessage = "please enter a password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

    }
}
