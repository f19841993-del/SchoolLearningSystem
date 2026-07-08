using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;

namespace SchoolLearningSystem.Applicationf.Validators.MemorizeSessionValidator
{
    public class MemorizeSessionUpdateDtoValidator : AbstractValidator<MemorizeSessionUpdateDto>
    {
        public MemorizeSessionUpdateDtoValidator()
        {
            // تصحيح جذري: TotalAttempts/CompletedAt غير موجودين بالـ DTO الفعلي —
            // الحقول الحقيقية هي SuccessRate/DurationInSeconds/IsCompleted فقط.

            RuleFor(x => x.SuccessRate)
                .InclusiveBetween(0, 100).WithMessage("نسبة النجاح يجب أن تكون بين 0 و100.")
                .When(x => x.SuccessRate.HasValue);

            RuleFor(x => x.DurationInSeconds)
                .GreaterThanOrEqualTo(0).WithMessage("مدة الجلسة لا يمكن أن تكون سالبة.")
                .When(x => x.DurationInSeconds.HasValue);

            // IsCompleted نفسه bool? بسيط، لا يحتاج قاعدة فحص قيمة —
            // فقط تأكيد وجوده كخيار تبديل للحالة (تعليقه هنا للتوثيق فقط لا للفحص الفعلي)
        }
    }
}
