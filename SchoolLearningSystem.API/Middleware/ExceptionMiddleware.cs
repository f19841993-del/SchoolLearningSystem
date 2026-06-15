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

            // القيم الافتراضية
            var statusCode = HttpStatusCode.InternalServerError;
            var message = "حدث خطأ غير متوقع في السيرفر.";

            // تصنيف الأخطاء
            if (exception is NotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;
                message = exception.Message;
            }
            // هنا نضيف معالجة الـ Validation (التي سأشرحها لك بالأسفل)
            else if (exception is ValidationException)
            {
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message;
            }
            else
            {
                _logger.LogError(exception, "خطأ غير معالج: {Message}", exception.Message);
            }

            context.Response.StatusCode = (int)statusCode;

            // ملاحظة: اجعل ApiResponse هو الشكل الموحد الذي يرجع في كل الحالات
            var response = new ApiResponse<string>((int)statusCode, message);

            return context.Response.WriteAsync(response.ToString());
        }
    }
}
