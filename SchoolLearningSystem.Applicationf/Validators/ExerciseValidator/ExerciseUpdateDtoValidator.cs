using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.ExerciseDto;

namespace SchoolLearningSystem.Applicationf.Validators.ExerciseValidator
{
    public class ExerciseUpdateDtoValidator : AbstractValidator<ExerciseUpdateDto>
    {
        public ExerciseUpdateDtoValidator()
        {
            RuleFor(x => x.Question)
                .MaximumLength(1000).WithMessage("النص طويل جداً.")
                .When(x => !string.IsNullOrEmpty(x.Question));

            RuleFor(x => x.Answer)
                .NotEmpty().WithMessage("الإجابة لا يمكن أن تكون فارغة إذا أُرسلت.")
                .When(x => x.Answer != null);

            RuleFor(x => x.Difficulty)
                .IsInEnum().WithMessage("مستوى الصعوبة غير صالح.")
                .When(x => x.Difficulty.HasValue);

            // إضافة: LessonId موجود فعلاً بالـ DTO الحقيقي (nullable) وكان بدون فحص
            RuleFor(x => x.LessonId)
                .GreaterThan(0).WithMessage("رقم الدرس غير صالح.")
                .When(x => x.LessonId.HasValue);
        }
    }
}
