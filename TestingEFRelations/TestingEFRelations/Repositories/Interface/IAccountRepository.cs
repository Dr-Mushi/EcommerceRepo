using TestingEFRelations.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace TestingEFRelations.Repositories
{
    public interface IAccountRepository
    {
        Task<IdentityResult> CreateUserAsync(SignUp signUp);
        Task<SignInResult> SignIn(SignIn signIn);

        Task SignOut();
    }
}