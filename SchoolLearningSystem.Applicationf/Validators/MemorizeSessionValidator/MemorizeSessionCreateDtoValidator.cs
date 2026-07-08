using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;

namespace SchoolLearningSystem.Applicationf.Validators.MemorizeSessionValidator
{
    public class MemorizeSessionCreateDtoValidator : AbstractValidator<MemorizeSessionCreateDto>
    {
        public MemorizeSessionCreateDtoValidator()
        {
            RuleFor(x => x.StudentId)
                .GreaterThan(0).WithMessage("رقم الطالب غير صالح.");

            RuleFor(x => x.ExerciseId)
                .GreaterThan(0).WithMessage("رقم التمرين غير صالح.")
                .When(x => x.ExerciseId.HasValue);

            // LessonId/Lesson لم يعودا موجودين بهذا الـ Entity إطلاقاً (قرار موثق مسبقاً) —
            // تأكد ألا يظهرا بهذا الـ DTO أبداً.
        }
    }
}
