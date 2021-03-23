using TestingEFRelations.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace TestingEFRelations.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> CreateUserAsync(SignUp signUp)
        {
            var user = new ApplicationUser()
            {
                Email = signUp.Email,
                UserName = signUp.UserName,
                UserFirstName = signUp.FirstName,
                UserLastName = signUp.LastName

            };
            IdentityResult result = await _userManager.CreateAsync(user, signUp.Password);
            
            return result;
        }

        public async Task<SignInResult> SignIn(SignIn signIn)
        {
            string user = signIn.EmailOrName;
            if (await _userManager.FindByNameAsync(user) == null)
            {
                 var userEmail = await _userManager.FindByEmailAsync(user);
                var userLogIn = await _signInManager.PasswordSignInAsync(userEmail.UserName, signIn.Password, signIn.RememberMe, false);
                return userLogIn;
            }
            
            
            var result = await _signInManager.PasswordSignInAsync(user, signIn.Password, signIn.RememberMe , false);
            
            return result;
        }

        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
