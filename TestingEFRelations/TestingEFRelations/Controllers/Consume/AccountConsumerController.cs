using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TestingEFRelations.Models;
using TestingEFRelations.Repositories;

namespace TestingEFRelations.Controllers.Consume
{
    public class AccountConsumerController : Controller
    {
        string errorMessage;
        private IHttpClientFactory _client;

        //private readonly IAccountRepository _accountRepository;

        public AccountConsumerController(/*IAccountRepository accountRepo*/ IHttpClientFactory clientFactory)
        {
            //_accountRepository = accountRepo;
            _client = clientFactory;
        }

        [HttpGet("SignInApi")]
        public ActionResult SignInApi()
        {
            return View("SignIn");
        }


        [Route("SignInApi")]
        [HttpPost]
        public async Task<ActionResult> SignInApi(SignIn signIn)
        {
            if (ModelState.IsValid)
            {
                //IdentityResult result = await _accountRepository.CreateUserAsync(signup);

                //if (!result.Succeeded)
                //{
                //    foreach (var errorMessage in result.Errors)
                //    {
                //        ModelState.AddModelError("", errorMessage.Description);
                //    }
                //}
                //ModelState.Clear();
               var client =  _client.CreateClient("Ecommerce");
                //var json = JsonConvert.SerializeObject(signIn);
                var response = await client.PostAsJsonAsync("AccountApi/SignIn",signIn /*new StringContent(json, Encoding.UTF8, "application/json")*/);
                
                
                if (response.IsSuccessStatusCode)
                {
                    errorMessage = "";
                }
                else
                {
                    errorMessage = $"There was an error login in : ({response.StatusCode})";
                }

                return RedirectToAction("Index", "ProductConsume");
            }
            return View(nameof(SignInApi));
        }


    }
}
