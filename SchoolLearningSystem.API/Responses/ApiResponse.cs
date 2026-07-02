namespace SchoolLearningSystem.API.Responses
{
    /// <summary>
    /// الاستجابة العادية: تُستخدم في الحالات التي لا تتطلب إرجاع بيانات (مثل رسائل الخطأ أو نجاح عمليات الحذف والتحديث).
    /// </summary>
    public class ApiResponse
    {
        public int Status { get; set; }
        public string Message { get; set; } = string.Empty;

        public ApiResponse(int status, string message)
        {
            Status = status;
            Message = message;
        }
    }

    /// <summary>
    /// الاستجابة العامة (Generic): ترث من الاستجابة العادية وتُستخدم عندما تريد إرجاع بيانات محددة مع الرسالة والحالة.
    /// </summary>
    public class ApiResponse<T> : ApiResponse
    {
        public T? Data { get; set; }

        public ApiResponse(int status, string message, T? data = default)
            : base(status, message) // تمرير الحالة والرسالة إلى الكلاس الأب
        {
            Data = data;
        }
    }
}