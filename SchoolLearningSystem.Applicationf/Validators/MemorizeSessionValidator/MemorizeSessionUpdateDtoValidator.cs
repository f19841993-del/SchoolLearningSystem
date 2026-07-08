using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;
using System;

namespace SchoolLearningSystem.Applicationf.Validators.MemorizeSessionValidator
{
    public class MemorizeSessionUpdateDtoValidator : AbstractValidator<MemorizeSessionUpdateDto>
    {
        public MemorizeSessionUpdateDtoValidator()
        {
            RuleFor(x => x.DurationInSeconds)
                .GreaterThanOrEqualTo(0).WithMessage("مدة الجلسة لا يمكن أن تكون سالبة.")
                .When(x => x.DurationInSeconds.HasValue);

            RuleFor(x => x.TotalAttempts)
                .GreaterThanOrEqualTo(0).WithMessage("إجمالي المحاولات لا يمكن أن يكون سالباً.")
                .When(x => x.TotalAttempts.HasValue);

            RuleFor(x => x.CompletedAt)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("تاريخ الإنهاء لا يمكن أن يكون بالمستقبل.")
                .When(x => x.CompletedAt.HasValue);

            // إذا IsCompleted=true من الأفضل منطقياً أن يكون CompletedAt موجوداً أيضاً
            RuleFor(x => x)
                .Must(x => x.IsCompleted != true || x.CompletedAt.HasValue)
                .WithMessage("عند إنهاء الجلسة يجب إرسال وقت الإنهاء (CompletedAt).")
                .When(x => x.IsCompleted.HasValue)
                .OverridePropertyName(nameof(MemorizeSessionUpdateDto.CompletedAt));
        }
    }
}
