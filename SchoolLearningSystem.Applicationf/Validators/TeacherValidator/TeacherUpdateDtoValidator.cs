using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.Teacher;

namespace SchoolLearningSystem.Applicationf.Validators.TeacherValidator
{
    public class TeacherUpdateDtoValidator : AbstractValidator<TeacherUpdateDto>
    {
        public TeacherUpdateDtoValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage("الاسم يجب ألا يتجاوز 100 حرف.")
                .When(x => !string.IsNullOrEmpty(x.Name));

            // تنبيه: تأكد أن Subject غير موجود إطلاقاً بهذا الـ DTO — الـ Mapping الخاص به
            // حُذف بالكامل في mapping_profiles_review_report.md (#3، رقم 7). لو ظهر هنا فالثغرة رجعت.
        }
    }
}
