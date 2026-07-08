using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.Question;

namespace SchoolLearningSystem.Applicationf.Validators.QuestionValidator
{
    public class QuestionUpdateDtoValidator : AbstractValidator<QuestionUpdateDto>
    {
        public QuestionUpdateDtoValidator()
        {
            RuleFor(x => x.Text)
                .MaximumLength(1000).WithMessage("النص طويل جداً.")
                .When(x => !string.IsNullOrEmpty(x.Text));

            RuleFor(x => x.DifficultyLevel)
                .IsInEnum().WithMessage("مستوى الصعوبة غير صالح.")
                .When(x => x.DifficultyLevel.HasValue);
        }
    }
}
