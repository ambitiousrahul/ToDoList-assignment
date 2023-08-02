using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace UrbanFTProject.ToDoList.Web.Middlewares
{
    public class LoginUrlAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                var loginUrl = context.HttpContext.Request.Scheme + "://" + context.HttpContext.Request.Host + "/account/login";
                context.Result = new JsonResult(new { LoginUrl = loginUrl }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }

}
