using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.Exercise;

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
        }
    }
}
