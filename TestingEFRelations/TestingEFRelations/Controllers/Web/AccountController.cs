using TestingEFRelations.Models;
using TestingEFRelations.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TestingEFRelations.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        // GET: AccountController
        public ActionResult Signup()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Signup(SignUp signup)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await _accountRepository.CreateUserAsync(signup);

                if (!result.Succeeded)
                {
                    foreach (var errorMessage in result.Errors)
                    {
                        ModelState.AddModelError("", errorMessage.Description);
                    }
                }
                ModelState.Clear();
            }
            return View();
        }


    }
}
