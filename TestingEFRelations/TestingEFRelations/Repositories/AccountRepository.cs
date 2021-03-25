using TestingEFRelations.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using Microsoft.IdentityModel.JsonWebTokens;

namespace TestingEFRelations.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppSettings _JWT;

        public AccountRepository(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<AppSettings> JWT)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _JWT = JWT.Value;
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
            //if the username is null and the email is not, find the username from user email
            if (await _userManager.FindByNameAsync(user) == null && await _userManager.FindByEmailAsync(user) != null)
            {
                 var userEmail = await _userManager.FindByEmailAsync(user);
                var userLogIn = await _signInManager.PasswordSignInAsync(userEmail.UserName, signIn.Password, signIn.RememberMe, false);
                //AuthToken(userLogIn, signIn);
                return userLogIn;
            }
            
            
            var result = await _signInManager.PasswordSignInAsync(user, signIn.Password, signIn.RememberMe , false);
           // AuthToken(result, signIn);
            return result;
        }

        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }

        public string AuthToken(/*SignInResult result,*/ SignIn signIn)
        {

            //if (result.Succeeded)
            //{
            //    //this is an object to handle tokens
            //    var tokenHandler = new JwtSecurityTokenHandler();
            //    //get the secret key in byte
            //    var key = Encoding.ASCII.GetBytes(_JWT.Secret);
            //    //description of the token
            //    var tokenDescriptor = new SecurityTokenDescriptor
            //    {
            //        Subject = new ClaimsIdentity(new Claim[]
            //        {
            //            new Claim(ClaimTypes.Name, signIn.EmailOrName)

            //        }),

            //        //when the token expires
            //        Expires = DateTime.UtcNow.AddSeconds(60),
            //        //secure algorithm
            //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
            //        SecurityAlgorithms.HmacSha256Signature)
            //    };
            //    //create the new token after writing all the description needed
            //    var token = tokenHandler.CreateToken(tokenDescriptor);
            //    //for now just generate the token here and see the output.
            //    var tokenWrite = tokenHandler.WriteToken(token);
            //}

            //if (result.Succeeded)
            //{
            //create a claim for the user
            var claims = new[]
            {
                    new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, signIn.EmailOrName),
                    new Claim("role","Admin"),
                    new Claim("SomeCookie","YayCookie")
                };

                //take the secret key 
                var secretKey = Encoding.UTF8.GetBytes(Constants.Secret);
                //give it a symmetric key
                var key = new SymmetricSecurityKey(secretKey);
                //sign the key
                var signingCredintials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                //put all the information for the token
                var token = new JwtSecurityToken
                     (
                    Constants.Issuer,
                    Constants.Audiance,
                    claims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddSeconds(20),
                    signingCredintials                    
                    );

                //generate the key
                var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);

                return tokenJson;
            //}
            return null;
        }
    }
}
