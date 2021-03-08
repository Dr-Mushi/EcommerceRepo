using TestingEFRelations.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace TestingEFRelations.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
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
    }
}
