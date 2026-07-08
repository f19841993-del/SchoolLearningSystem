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

            // القاعدة التجارية المفتوحة (dtos_review_report.md #4.2) ولم تُطبّق بعد بأي Service:
            // يجب توفر LessonId أو ExamId على الأقل. هذا أنسب مكان لفرضها فعلياً الآن.
            RuleFor(x => x)
                .Must(x => x.LessonId.HasValue || x.ExamId.HasValue)
                .WithMessage("يجب ربط النتيجة بدرس أو بامتحان على الأقل.")
                .OverridePropertyName(nameof(ResultCreateDto.LessonId));
        }
    }
}
