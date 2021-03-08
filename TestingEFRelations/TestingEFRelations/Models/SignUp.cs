using System.ComponentModel.DataAnnotations;

namespace TestingEFRelations.Models
{
    public class SignUp
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter your email address")]
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Please enter your user name")]
        [Display(Name = "User name")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter your first name")]
        [Display(Name = "first name")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Please enter your last name")]
        [Display(Name = "last name")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "please enter a password")]
        [Compare("ConfirmPassword", ErrorMessage = "Password does not match")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "please confirm the password")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
