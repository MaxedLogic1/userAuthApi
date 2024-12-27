// Middleware/GlobalExceptionMiddleware.cs
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;

namespace usersAuthApi.Middleware
{
    public class globalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<globalExceptionMiddleware> _logger;

        public globalExceptionMiddleware(RequestDelegate next, ILogger<globalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                var errorId = new Random().Next(1000, 9999);
                //log this exception 
                _logger.LogError(ex, $"{errorId} : {ex.Message}");


                //return custom error response 
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "applicaton.json";
                var error = new
                {
                    Id = errorId,
                    ErrorMessage = "Something went wrong! we are loking int resolbing these ",
                };
                await httpContext.Response.WriteAsJsonAsync(error);
            }
        }
    }
}
