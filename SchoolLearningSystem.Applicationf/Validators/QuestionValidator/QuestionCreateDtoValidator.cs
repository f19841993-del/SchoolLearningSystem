using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.Question;

namespace SchoolLearningSystem.Applicationf.Validators.QuestionValidator
{
    public class QuestionCreateDtoValidator : AbstractValidator<QuestionCreateDto>
    {
        public QuestionCreateDtoValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("نص السؤال مطلوب.")
                .MaximumLength(1000).WithMessage("النص طويل جداً.");

            RuleFor(x => x.DifficultyLevel)
                .IsInEnum().WithMessage("مستوى الصعوبة غير صالح.");

            // السؤال إما عام (تدريب حر) أو مرتبط بامتحان — كلاهما اختياري بتصميم
            RuleFor(x => x.LessonId)
                .GreaterThan(0).WithMessage("رقم الدرس غير صالح.")
                .When(x => x.LessonId.HasValue);

            RuleFor(x => x.ExamId)
                .GreaterThan(0).WithMessage("رقم الامتحان غير صالح.")
                .When(x => x.ExamId.HasValue);
        }
    }
}
