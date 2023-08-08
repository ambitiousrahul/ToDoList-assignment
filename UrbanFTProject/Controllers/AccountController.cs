

using System.Net.Http;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UrbanFTProject.ToDoList.Web.Middlewares;
using UrbanFTProject.ToDoList.Web.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace UrbanFTProject.ToDoList.Web.Controllers
{
    [Route("account")]
    [ApiController]
    
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<IdentityUser> userManager,IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
      

        [HttpPost]
        [Route("login")]
        [ServiceFilter(typeof(ValidateActionParametersAttribute))]
        public async Task<IActionResult> Login([FromBody]APILoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var returnUrl = HttpContext?.Request.Query.FirstOrDefault(r => r.Key == "returnUrl");

            if (user is null)
            { 

                // Replace the response with a new response containing the object you want to return               
                string registrationUrl = $"{HttpContext?.Request.Scheme}://{HttpContext?.Request.Host.Value}/identity/account/register";

                var unRegisteredUserResponse = new
                {
                    Message = "Unrecognized user. You will have to register to our application to use its api endpoints",
                    browserUrl = registrationUrl,
                    HttpContext?.Request.Method,
                    ContentType = "url"
                };
                return Unauthorized(unRegisteredUserResponse);
            }
            else
            {
                await _userManager.UpdateSecurityStampAsync(user);
#nullable disable
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

                string authenticatedJwtToken = HandleJwtAuthentication(user.Email);// return jwtToken;

                return new OkObjectResult(new JWTTokens
                {
                    Token = authenticatedJwtToken
                });

                //var token = _userManager.GenerateUserTokenAsync(user, "Default", "passwordless-auth");
                
                //var url = Url.ActionLink(action: "login", controller: "LoginRedirect", values: new
                //{
                //    Token = token.Result,
                //    model.Email,
                //    ReturnUrl = returnUrl?.Value
                //}, protocol: Request.Scheme);

                //var authenticatedUserResponse = new
                //{
                //    Message = "Login Successfull. Request below endpoint to get authorisation token",
                //    EndpointUrl = url,
                //    Method="GET",
                //    ContentType = "application/json"
                //};
                //return Ok(authenticatedUserResponse);
            }
        }


        private string HandleJwtAuthentication(string email)
        {
            /* sign in logic ... */
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email,email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var jwtToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(jwtToken);
        }
    }
}
