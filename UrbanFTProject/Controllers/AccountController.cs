

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UrbanFTProject.ToDoList.Web.Middlewares;
using UrbanFTProject.ToDoList.Web.Models;

namespace UrbanFTProject.ToDoList.Web.Controllers
{
    [Route("accounts")]
    [ServiceFilter(typeof(ValidateActionParametersAttribute))]
    public class AccountsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AccountsController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login(string? returnURL)
        {
            return Ok(new 
            {
                Message = "Unrecognized user. You must sign in to use this weather service.",
                LoginUrl = Url.ActionLink(action: "", controller: "Account", values:
                new
                {
                    ReturnURL = returnURL
                },
                protocol: Request.Scheme),                
                Schema = "{ \n userName * string \n  email * string($email) \n }",
                ContentType="application/json"
            });

            
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]APILoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var returnUrl = HttpContext?.Request.Query.FirstOrDefault(r => r.Key == "returnUrl");

            if (user is null)
            {                
                var baseUri = new Uri(HttpContext?.Request.PathBase);
                string registrationUrl = new Uri(baseUri, "/identity/account/register").ToString();

                return Unauthorized($"Email is not registered. To create new user go to {registrationUrl}");
            }
            else
            {
                var token = _userManager.GenerateUserTokenAsync(user, "Default", "passwordless-auth");
                var url = Url.ActionLink(action: "", controller: "LoginRedirect", values: new
                {
                    Token = token.Result,
                    Email = model.Email,
                    ReturnUrl = returnUrl?.Value
                }, protocol: Request.Scheme);
                return Ok(url);
            }
        }
    }
}
