using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;

namespace ReactifyBlog.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (System.Exception ex)
            {
                // Log the exception here
                Console.WriteLine($"An error occurred: {ex.Message}"); // Using ex here
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await httpContext.Response.WriteAsync("An unexpected error occurred.");
            }
        }
    }
}
