using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.ExamDto;

namespace SchoolLearningSystem.Applicationf.Validators.ExamValidator
{
    public class ExamCreateDtoValidator : AbstractValidator<ExamCreateDto>
    {
        public ExamCreateDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("عنوان الامتحان مطلوب.")
                .MaximumLength(150).WithMessage("العنوان يجب ألا يتجاوز 150 حرف.");

            RuleFor(x => x.CourseId)
                .GreaterThan(0).WithMessage("رقم الكورس غير صالح.");

            RuleFor(x => x.ExamType)
                .IsInEnum().WithMessage("نوع الامتحان غير صالح.");

            RuleFor(x => x.Difficulty)
                .IsInEnum().WithMessage("مستوى الصعوبة غير صالح.");

            RuleFor(x => x.LessonId)
                .GreaterThan(0).WithMessage("رقم الدرس غير صالح.")
                .When(x => x.LessonId.HasValue);
        }
    }
}
