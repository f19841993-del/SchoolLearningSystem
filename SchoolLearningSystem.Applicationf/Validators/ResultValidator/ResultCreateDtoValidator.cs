using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.Result;

namespace SchoolLearningSystem.Applicationf.Validators.ResultValidator
{
    public class ResultCreateDtoValidator : AbstractValidator<ResultCreateDto>
    {
        public ResultCreateDtoValidator()
        {
            RuleFor(x => x.StudentId)
                .GreaterThan(0).WithMessage("رقم الطالب غير صالح.");

            RuleFor(x => x.Score)
                .GreaterThanOrEqualTo(0).WithMessage("الدرجة لا يمكن أن تكون سالبة.");

            RuleFor(x => x.ResultType)
                .NotEmpty().WithMessage("نوع النتيجة مطلوب.");

            // إضافة: DurationInSeconds كان ناقصاً بالكامل من الفحص
            RuleFor(x => x.DurationInSeconds)
                .GreaterThanOrEqualTo(0).WithMessage("المدة المستغرقة لا يمكن أن تكون سالبة.");

            // ملاحظة تصحيح: هذه القاعدة موجودة فعلاً بمنطق ResultService.CreateAsync حسب
            // تعليق الـ DTO نفسه - إضافتها هنا طبقة حماية إضافية (Defense in Depth) لا تكرار ضار
            RuleFor(x => x)
                .Must(x => x.LessonId.HasValue || x.ExamId.HasValue)
                .WithMessage("يجب ربط النتيجة بدرس أو بامتحان على الأقل.")
                .OverridePropertyName(nameof(ResultCreateDto.LessonId));
        }
    }
}
