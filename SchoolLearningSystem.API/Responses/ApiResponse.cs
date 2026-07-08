using SchoolLearningSystem.Applicationf.Exceptions;

namespace SchoolLearningSystem.API.Responses
{
    /// <summary>
    /// الاستجابة العادية: تُستخدم في الحالات التي لا تتطلب إرجاع بيانات
    /// (مثل رسائل الخطأ أو نجاح عمليات الحذف والتحديث).
    /// </summary>
    public class ApiResponse
    {
        public int Status { get; set; }
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// قائمة أخطاء التحقق التفصيلية (Field + Message لكل حقل).
        /// تبقى null في حالات النجاح، ولا تُستخدم أبداً كبديل عن Data.
        /// </summary>
        public List<ValidationErrorItem>? Errors { get; set; }

        public ApiResponse(int status, string message, List<ValidationErrorItem>? errors = null)
        {
            Status = status;
            Message = message;
            Errors = errors;
        }

        // ===================== Factory Methods (غير جينيرك) =====================

        public static ApiResponse SuccessResponse(string message = "تمت العملية بنجاح")
            => new(200, message);

        public static ApiResponse FailResponse(string message, int status = 400, List<ValidationErrorItem>? errors = null)
            => new(status, message, errors);
    }

    /// <summary>
    /// الاستجابة العامة (Generic): ترث من الاستجابة العادية وتُستخدم عندما تريد
    /// إرجاع بيانات محددة مع الرسالة والحالة.
    /// ملاحظة معمارية: Data و Errors لا يجتمعان أبداً بنفس الاستجابة —
    /// إما بيانات نجاح (Data) أو تفاصيل فشل (Errors)، وليس كلاهما معاً.
    /// </summary>
    public class ApiResponse<T> : ApiResponse
    {
        public T? Data { get; set; }

        public ApiResponse(int status, string message, T? data = default, List<ValidationErrorItem>? errors = null)
            : base(status, message, errors) // تمرير الحالة والرسالة والأخطاء إلى الكلاس الأب
        {
            Data = data;
        }

        // ===================== Factory Methods (جينيرك) =====================

        public static ApiResponse<T> Success(T data, string message = "تمت العملية بنجاح")
            => new(200, message, data);

        public static ApiResponse<T> Fail(string message, int status = 400, List<ValidationErrorItem>? errors = null)
            => new(status, message, default, errors);
    }
}