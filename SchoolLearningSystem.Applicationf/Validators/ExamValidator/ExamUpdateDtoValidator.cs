using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.Exam;

namespace SchoolLearningSystem.Applicationf.Validators.ExamValidator
{
    public class ExamUpdateDtoValidator : AbstractValidator<ExamUpdateDto>
    {
        public ExamUpdateDtoValidator()
        {
            RuleFor(x => x.Title)
                .MaximumLength(150).WithMessage("العنوان يجب ألا يتجاوز 150 حرف.")
                .When(x => !string.IsNullOrEmpty(x.Title));

            RuleFor(x => x.LessonId)
                .GreaterThan(0).WithMessage("رقم الدرس غير صالح.")
                .When(x => x.LessonId.HasValue);

            // تنبيه: CourseId مستثنى عمداً من هذا الـ DTO — نقل امتحان بين كورسات تصرف خطير
            // يستحق Use Case منفصل (dtos_review_report.md #4.1). تأكد أنه غير موجود بالـ DTO أصلاً.
        }
    }
}
