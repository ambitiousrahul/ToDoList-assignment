

using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UrbanFTProject.ToDoList.Web.Middlewares;
using UrbanFTProject.ToDoList.Web.Models;

namespace UrbanFTProject.ToDoList.Web.Controllers
{
    [Route("account")]
    [ApiController]
    [ServiceFilter(typeof(ValidateActionParametersAttribute))]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
      

        [HttpPost]
        [Route("login")]
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
                var token = _userManager.GenerateUserTokenAsync(user, "Default", "passwordless-auth");
                
                var url = Url.ActionLink(action: "login", controller: "LoginRedirect", values: new
                {
                    Token = token.Result,
                    model.Email,
                    ReturnUrl = returnUrl?.Value
                }, protocol: Request.Scheme);

                var authenticatedUserResponse = new
                {
                    Message = "Login Successfull. Request below endpoint to get authorisation token",
                    EndpointUrl = url,
                    Method="GET",
                    ContentType = "application/json"
                };
                return Ok(authenticatedUserResponse);
            }
        }
    }
}
