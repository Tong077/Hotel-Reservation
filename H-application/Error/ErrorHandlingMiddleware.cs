using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace H_application.Error
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                // Check if it's an API request or MVC request
                bool isApiRequest = context.Request.Path.StartsWithSegments("/api")
                                 || context.Request.Headers["Accept"].ToString().Contains("application/json");

                if (isApiRequest)
                {
                    // Return JSON for API requests
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var errorResponse = new
                    {
                        message = "An unexpected error occurred",
                        detail = GetFullExceptionMessage(ex),
                        stackTrace = ex.StackTrace
                    };

                    var result = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

                    await context.Response.WriteAsync(result);
                }
                else
                {
                    // Store error in TempData via session for MVC views
                    context.Session.SetString("ErrorMessage", GetFullExceptionMessage(ex));
                    context.Response.Redirect("/Home/Index");
                }
            }
        }

        private string GetFullExceptionMessage(Exception ex)
        {
            if (ex == null) return string.Empty;
            string message = ex.Message;
            if (ex.InnerException != null)
                message += " | " + GetFullExceptionMessage(ex.InnerException);
            return message;
        }
    }
}