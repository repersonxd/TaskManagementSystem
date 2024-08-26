using System.Net;
using System.Text.Json;
using GorevY.Models;


namespace GorevY.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex.Message}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errorDetails = new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error. Please try again later."
            };

            // Geliştirme ortamında daha ayrıntılı hata mesajı göster
            if (context.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment())
            {
                errorDetails.Detailed = exception.Message;
            }

            var errorJson = JsonSerializer.Serialize(errorDetails);
            return context.Response.WriteAsync(errorJson);
        }
    }
}
