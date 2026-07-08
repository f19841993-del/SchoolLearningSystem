using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.ExamDto;

namespace SchoolLearningSystem.Applicationf.Validators.ExamValidator
{
    public class ExamUpdateDtoValidator : AbstractValidator<ExamUpdateDto>
    {
        public ExamUpdateDtoValidator()
        {
            RuleFor(x => x.Title)
                .MaximumLength(150).WithMessage("العنوان يجب ألا يتجاوز 150 حرف.")
                .When(x => !string.IsNullOrEmpty(x.Title));

            RuleFor(x => x.ExamType)
                .IsInEnum().WithMessage("نوع الامتحان غير صالح.")
                .When(x => x.ExamType.HasValue);

            RuleFor(x => x.Difficulty)
                .IsInEnum().WithMessage("مستوى الصعوبة غير صالح.")
                .When(x => x.Difficulty.HasValue);

            RuleFor(x => x.LessonId)
                .GreaterThan(0).WithMessage("رقم الدرس غير صالح.")
                .When(x => x.LessonId.HasValue);

            // CourseId مستثنى عمداً - غير موجود بالـ DTO الفعلي (نفس القرار الموثق سابقاً)
        }
    }
}
