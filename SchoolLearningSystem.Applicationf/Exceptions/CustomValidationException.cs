using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace SchoolLearningSystem.Applicationf.Exceptions
{
    /// <summary>
    /// عنصر خطأ تحقق واحد، يربط اسم الحقل (Field) برسالة الخطأ (Message).
    /// هذا الشكل هو ما يُرسَل فعلياً للفرونت داخل ApiResponse.Errors،
    /// ليقدر الفرونت يعرض كل خطأ تحت الحقل الصحيح مباشرة بدل قائمة عامة.
    /// </summary>
    public class ValidationErrorItem
    {
        public string Field { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public ValidationErrorItem(string field, string message)
        {
            Field = field;
            Message = message;
        }
    }

    public class CustomValidationException : Exception
    {
        public List<ValidationErrorItem> Errors { get; }

        public CustomValidationException(IEnumerable<ValidationFailure> failures)
            : base("حدث خطأ في التحقق من البيانات.")
        {
            Errors = failures
                .Select(f => new ValidationErrorItem(
                    field: ToCamelCase(f.PropertyName),   // متوافق مع تسمية JSON بالفرونت (JS camelCase)
                    message: f.ErrorMessage))
                .ToList();
        }

        // تحويل بسيط لأول حرف لـ lowercase (StudentName -> studentName)
        // عشان يطابق أسماء الحقول اللي يتوقعها الفرونت بالفورم (JS/React state)
        private static string ToCamelCase(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)) return propertyName;
            return char.ToLowerInvariant(propertyName[0]) + propertyName.Substring(1);
        }
    }
}