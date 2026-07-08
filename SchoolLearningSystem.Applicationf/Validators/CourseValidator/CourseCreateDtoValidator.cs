using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.Course;

namespace SchoolLearningSystem.Applicationf.Validators.CourseValidator
{
    public class CourseCreateDtoValidator : AbstractValidator<CourseCreateDto>
    {
        public CourseCreateDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("عنوان الكورس مطلوب.")
                .MaximumLength(100).WithMessage("العنوان يجب أن لا يتجاوز 100 حرف.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("وصف الكورس لا يمكن أن يكون فارغاً.");

            RuleFor(x => x.TeacherId)
                .GreaterThan(0).WithMessage("رقم المعلم غير صالح.");

            RuleFor(x => x.CurriculumId)
                .GreaterThan(0).WithMessage("رقم المنهج غير صالح.");

            // Order مستثنى عمداً — يُحسب بالـ Service (dtos_review_report.md #4.1)
        }
    }
}
