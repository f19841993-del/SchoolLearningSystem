using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.Exam;

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

            RuleFor(x => x.Difficulty)
                .NotEmpty().WithMessage("مستوى الصعوبة مطلوب.");

            // LessonId اختياري بتصميم — امتحان عام أو خاص بدرس (dtos_review_report.md #4.1)
            RuleFor(x => x.LessonId)
                .GreaterThan(0).WithMessage("رقم الدرس غير صالح.")
                .When(x => x.LessonId.HasValue);
        }
    }
}
