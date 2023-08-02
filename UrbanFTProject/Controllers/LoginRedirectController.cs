using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using UrbanFTProject.ToDoList.Web.Models;

namespace UrbanFTProject.ToDoList.Web.Controllers
{
    public class LoginRedirectController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _iconfiguration;

        public LoginRedirectController(UserManager<IdentityUser> userManager, IConfiguration iconfiguration)
        {
            _userManager = userManager;
            _iconfiguration = iconfiguration;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string token, string email, string returnUrl,string contentType="")
        {
            var user = await _userManager.FindByEmailAsync(email);
            var isValid = await _userManager.VerifyUserTokenAsync(user, "Default", "passwordless-auth", token);

            if (isValid)
            {
                await _userManager.UpdateSecurityStampAsync(user);

                await HttpContext.SignInAsync(
                    IdentityConstants.ApplicationScheme,
                    new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new List<Claim>
                            {
                        new Claim("sub", user.Id)
                            },
                            IdentityConstants.ApplicationScheme
                        )
                   )
                );

                string authenticatedJwtToken = HandleJwtAuthentication(email);// return jwtToken;

                return Convert.ToBoolean(returnUrl?.StartsWith("api")) ? new OkObjectResult(new JWTTokens
                {
                    Token = authenticatedJwtToken
                }) : new RedirectResult($"~{returnUrl}");

            }

            return Unauthorized();
        }

        private string HandleJwtAuthentication(string email)
        {
            /* sign in logic ... */
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_iconfiguration["JWT:SigningKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email,email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var jwtToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(jwtToken);
        }
    }
}
