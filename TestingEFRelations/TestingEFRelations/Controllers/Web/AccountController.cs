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
        [Route("Signup")]
        [HttpGet]
        public ActionResult Signup()
        {
            return View();
        }

        [Route("Signup")]
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
            return View(nameof(SignIn));
        }
        [Route("SignIn")]
        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }
        [Route("SignIn")]
        [HttpPost]
        public async Task<ActionResult> SignIn(SignIn signIn)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.SignIn(signIn);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "product");
                }
                ModelState.AddModelError("", "Invalid Credintials");
            }
            
            return View();
        }

        public async Task<ActionResult> SignOut()
        {
            await _accountRepository.SignOut();
            return RedirectToAction("Index", "product");
        }


    }
}
