using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.Exceptions;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Shared.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionHandling
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandling> _logger;

        public ExceptionHandling(ILogger<ExceptionHandling> logger, RequestDelegate next)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (NotFoundException ex)
            {
                if (!httpContext.Response.HasStarted)
                {
                    httpContext.Response.Clear();
                    httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

                    AddCorsHeaders(httpContext);

                    await httpContext.Response.WriteAsync(ex.Message);
                }
            }
            catch (ValidationException ex)
            {
                if (!httpContext.Response.HasStarted)
                {
                    httpContext.Response.Clear();
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

                    AddCorsHeaders(httpContext);

                    if (ex.Errors.Count <= 0)
                    {
                        await httpContext.Response.WriteAsync(ex.Message);
                        return;
                    }

                    var errors = JsonConvert.SerializeObject(ex.Errors);


                    await httpContext.Response.WriteAsync(errors);
                }
            }
            catch (ForbiddenException ex)
            {
                if (!httpContext.Response.HasStarted)
                {
                    httpContext.Response.Clear();
                    httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

                    AddCorsHeaders(httpContext);

                    await httpContext.Response.WriteAsync(ex.Message);
                }
            }
            catch (NotImplementedException ex)
            {
                if (!httpContext.Response.HasStarted)
                {
                    httpContext.Response.Clear();
                    httpContext.Response.StatusCode = StatusCodes.Status501NotImplemented;

                    AddCorsHeaders(httpContext);

                    await httpContext.Response.WriteAsync(ex.Message);
                }
            }
            catch (Exception ex)
            {
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }
                _logger.LogError(ex, $"Unhandled error occured with message: {ex.Message}");
                if (!httpContext.Response.HasStarted)
                {
                    httpContext.Response.Clear();
                    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

                    AddCorsHeaders(httpContext);

                    await httpContext.Response.WriteAsync("Something went wrong.");
                }
            }
        }

        public void AddCorsHeaders(HttpContext httpContext)
        {
            httpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            httpContext.Response.Headers.Add("Access-Control-Allow-Methods", "OPTIONS, GET, POST, PUT, PATCH, DELETE");
            httpContext.Response.Headers.Add("Access-Control-Allow-Headers", "X-PINGOTHER, Content-Type, Authorization");
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandling>();
        }
    }
}
