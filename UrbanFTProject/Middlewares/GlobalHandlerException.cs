
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using UrbanFTProject.Models;

namespace UrbanFTProject.ToDoList.Web.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class GlobalHandlerException
    {
        private readonly RequestDelegate _next;

        public GlobalHandlerException(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this._next(context);
            }
            catch (Exception exceptionobj)
            {
                await this.HandleExceptionAsync(context, exceptionobj);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            ErrorViewModel errorResponseObj = default!;
            if (ex is DbUpdateException && ex.InnerException is SqlException sqlEx)
            {

                context.Response.StatusCode = StatusCodes.Status409Conflict;

                errorResponseObj = new ErrorViewModel
                {
                    ErrorCode = StatusCodes.Status409Conflict,
                    Message = "Unique key constraint was violated with the passed request payload"
                };

            }
            else if(ex is NotImplementedException)
            {
                context.Response.StatusCode = StatusCodes.Status501NotImplemented;

                errorResponseObj = new ErrorViewModel
                {
                    ErrorCode = StatusCodes.Status501NotImplemented,
                    Message = "the api is not implemented"
                };
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                errorResponseObj = new ErrorViewModel
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    Message = "An error was thrown at server for the request, please retry"
                };
            }

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(errorResponseObj.ToString());

        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class GlobalHandlerExceptionExtensions
    {
        public static IApplicationBuilder UseGlobalHandlerException(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalHandlerException>();
        }
    }
}
