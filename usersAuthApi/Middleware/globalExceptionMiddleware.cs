// Middleware/GlobalExceptionMiddleware.cs
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using usersAuthApi.Exceptions;  // Import both custom exceptions

namespace usersAuthApi.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
            // Handle GameNotFoundException
            catch (GameNotFoundCustomException ex)  
            {
                var errorId = new Random().Next(1000, 9999);
                // Log the GameNotFoundException
                _logger.LogError(ex, $"{errorId} : {ex.Message}");

                // Return a custom error response for GameNotFoundException
                httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                httpContext.Response.ContentType = "application/json";
                var error = new
                {
                    Id = errorId,
                    ErrorMessage = ex.Message,  // Message from GameNotFoundException
                };
                await httpContext.Response.WriteAsJsonAsync(error);
            }

            // Handle MyCustomException
            catch (PlayerNotFoundCustomException ex) 
            {
                var errorId = new Random().Next(1000, 9999);
                // Log the MyCustomException
                _logger.LogError(ex, $"{errorId} : {ex.Message}");

                // Return a custom error response for MyCustomException
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                httpContext.Response.ContentType = "application/json";
                var error = new
                {
                    Id = errorId,
                    ErrorMessage = ex.Message,  // Message from MyCustomException
                };
                await httpContext.Response.WriteAsJsonAsync(error);
            }
            catch (Exception ex)  // Handle all other exceptions
            {
                var errorId = new Random().Next(1000, 9999);
                // Log the general exception
                _logger.LogError(ex, $"{errorId} : {ex.Message}");

                // Return a generic error response for all other exceptions
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";
                var error = new
                {
                    Id = errorId,
                    ErrorMessage = "Something went wrong! We are looking into resolving these issues.",
                };
                await httpContext.Response.WriteAsJsonAsync(error);
            }
        }
    }
}
