using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingEFRelations.Models;
using TestingEFRelations.Repositories;

namespace TestingEFRelations.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountApiController(IAccountRepository acountRepo)
        {
            _accountRepository = acountRepo;
        }

        [HttpPost("SignIn")]
        public async Task<ActionResult> SignIn([FromBody] SignIn signIn)
        {
            if (ModelState.IsValid)
            {
                //var result = await _accountRepository.SignIn(signIn);
                string token = _accountRepository.AuthToken(/*result,*/ signIn);

                CookieOptions cs = new CookieOptions();
                cs.Expires = DateTime.Now.AddHours(1);
                 Response.Cookies.Append("Token_AccessCookie", token, cs);
                
                
               

                //if (result.Succeeded)
                //{
                return Ok(new { access_token = token });
                //}

            }

            return BadRequest();

        }

        [HttpGet("LogOut")]
        public async Task<ActionResult> SignOut()
        {
            if (ModelState.IsValid)
            {
                await _accountRepository.SignOut();

                return Ok(true);
            }
            return BadRequest();

        }


    }
}
