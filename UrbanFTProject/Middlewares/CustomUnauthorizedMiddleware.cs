
using UrbanFTProject.ToDoList.Web.Middlewares;

namespace UrbanFTProject.ToDoList.Web.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CustomUnauthorizedMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomUnauthorizedMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {

            await _next(httpContext);

            // Check if the response is an Unauthorized response
            if (httpContext.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                if (Convert.ToBoolean(httpContext?.Request?.Path.Value?.StartsWith("/api")))
                {
                    // Replace the response with a new response containing the object you want to return               
                    string loginUrl = $"{httpContext?.Request.Scheme}://{httpContext?.Request.Host.Value}/Account/Login";
#nullable disable
                    if (!httpContext.Response.HasStarted)
                    {
                        httpContext.Response.ContentType = "application/json";
                        await httpContext.Response.WriteAsJsonAsync(new
                        {
                            Message = "Unrecognized user. You must sign in to use this endpoint",
                            LoginUrl = loginUrl,
                            httpContext.Request.Method,
                            Schema = new
                            {
                                email = "${registeredEmail}"
                            },
                            ContentType = "application/json"
                        });
                    }
                }
                else
                {
                    if (!httpContext.Response.HasStarted)
                    {
                        httpContext.Response.Redirect($"{httpContext?.Request.Scheme}://{httpContext?.Request.Host.Value}/Identity/Account/Login?returnUrl={httpContext?.Request.Path.Value}");
                    }
                }
            }
        }

    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CustomUnauthorizedMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomUnauthorizedMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomUnauthorizedMiddleware>();
        }
    }
}
