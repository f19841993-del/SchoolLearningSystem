// Middleware/ExceptionMiddleware.cs
using SchoolLearningSystem.API.Responses; // ⚠️ تأكد أن ApiResponse<T> معرّفة هنا فعلاً
                                          // وليست بـ SchoolLearningSystem.API.Models —
                                          // لو كانت هناك، احذف هذا الـ using وأضف ذاك بدلاً عنه.

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
                // ملاحظة: التسجيل الفعلي (بمستوياته المختلفة) صار بالكامل
                // داخل HandleExceptionAsync، لتفادي تسجيل نفس الاستثناء مرتين
                // وبمستوى خطأ (LogError) غير مناسب لحالات الأعمال المتوقعة.
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var statusCode = HttpStatusCode.InternalServerError;
            var message = "حدث خطأ غير متوقع في السيرفر.";
            object? errorsData = null; // يحمل قائمة الأخطاء التفصيلية إن وجدت

            if (exception is NotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;
                message = exception.Message;

                // Warning لا Error: هذا تدفق عمل طبيعي (مورد غير موجود)، مو عطل بالسيرفر
                _logger.LogWarning("NotFound: {Message}", exception.Message);
            }
            else if (exception is CustomValidationException customValidationEx)
            {
                statusCode = HttpStatusCode.BadRequest;
                message = customValidationEx.Message;
                errorsData = customValidationEx.Errors; // قائمة الأخطاء التفصيلية

                _logger.LogWarning("فشل تحقق: {Errors}", string.Join("; ", customValidationEx.Errors));
            }
            else if (exception is BadRequestException)
            {
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message;

                _logger.LogWarning("BadRequest: {Message}", exception.Message);
            }
            else
            {
                // الاستثناء الحقيقي غير المتوقع فقط هو اللي يستاهل LogError
                // مع تمرير الكائن كاملاً (exception وليس exception.Message فقط)
                // للحصول على الـ Stack Trace الكامل بسجلات المطور.
                _logger.LogError(exception, "خطأ غير معالج: {Message}", exception.Message);
            }

            context.Response.StatusCode = (int)statusCode;

            // استخدام ApiResponse<object> لكي يستطيع حمل الـ List<string> الخاص بالأخطاء
            var response = new ApiResponse<object>((int)statusCode, message, errorsData);
            var jsonResponse = System.Text.Json.JsonSerializer.Serialize(response, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
            });

            return context.Response.WriteAsync(jsonResponse);
        }
    }
}