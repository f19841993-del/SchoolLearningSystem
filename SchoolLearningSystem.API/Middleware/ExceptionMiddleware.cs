// Middleware/ExceptionMiddleware.cs
using SchoolLearningSystem.API.Models;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.Exceptions;
using System.Net;

namespace SchoolLearningSystem.API.Middleware
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
                await _next(context); // استمرار الطلب بشكل طبيعي
            }
            catch (Exception ex)
            {
                _logger.LogError($"حدث خطأ غير متوقع: {ex.Message}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = HttpStatusCode.InternalServerError;
            var message = "حدث خطأ غير متوقع في السيرفر.";
            object? errorsData = null; // 👈 1. أضفنا هذا المتغير ليحمل قائمة الأخطاء إن وجدت

            if (exception is NotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;
                message = exception.Message;
            }
            // 👈 2. نقوم بعمل Cast للاستثناء لكي نستطيع الوصول إلى customValidationEx.Errors
            else if (exception is CustomValidationException customValidationEx)
            {
                statusCode = HttpStatusCode.BadRequest;
                message = customValidationEx.Message;
                errorsData = customValidationEx.Errors; // 👈 3. نأخذ قائمة الأخطاء التفصيلية
            }
            else if (exception is BadRequestException)
            {
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message;
            }
            else
            {
                _logger.LogError(exception, "خطأ غير معالج: {Message}", exception.Message);
            }

            context.Response.StatusCode = (int)statusCode;

            // 👈 4. نستخدم ApiResponse<object> لكي يستطيع حمل الـ List<string> الخاص بالأخطاء
            var response = new ApiResponse<object>((int)statusCode, message, errorsData);

            var jsonResponse = System.Text.Json.JsonSerializer.Serialize(response, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
            });

            return context.Response.WriteAsync(jsonResponse);
        }
    }
}
