using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.Exercise;

namespace SchoolLearningSystem.Applicationf.Validators.ExerciseValidator
{
    public class ExerciseCreateDtoValidator : AbstractValidator<ExerciseCreateDto>
    {
        public ExerciseCreateDtoValidator()
        {
            RuleFor(x => x.Question)
                .NotEmpty().WithMessage("نص التمرين مطلوب.")
                .MaximumLength(1000).WithMessage("النص طويل جداً.");

            RuleFor(x => x.Answer)
                .NotEmpty().WithMessage("الإجابة الصحيحة مطلوبة.");

            RuleFor(x => x.Difficulty)
                .IsInEnum().WithMessage("مستوى الصعوبة غير صالح.");

            RuleFor(x => x.LessonId)
                .GreaterThan(0).WithMessage("رقم الدرس غير صالح.");

            // تأكد ألا يوجد حقل Title هنا — كان بقايا نسخ-لصق من DTO قديم وحُذف فعلياً (dtos_review_report.md #3.2)
        }
    }
}
