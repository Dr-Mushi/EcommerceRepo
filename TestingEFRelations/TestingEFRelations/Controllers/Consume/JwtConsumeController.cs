using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TestingEFRelations.Controllers.Consume
{
    public class JwtConsumeController : Controller
    {
        public string JwtDecode()
        {
            //consume jwt cookie and decode it
            string cookie  = Request.Cookies["Token_AccessCookie"];
            var handler = new JwtSecurityTokenHandler();
            var decodedValue = handler.ReadJwtToken(cookie);

            return null;
        }
    }
}
