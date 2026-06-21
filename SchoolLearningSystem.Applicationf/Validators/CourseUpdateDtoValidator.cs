using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;

namespace SchoolLearningSystem.Applicationf.Validators
{
    public class CourseUpdateDtoValidator : AbstractValidator<CourseUpdateDto>
    {
        public CourseUpdateDtoValidator()
        {
            // نضع الشروط هنا، ونستخدم When لكي لا يفحص إلا إذا كانت القيمة غير فارغة
            RuleFor(x => x.Title)
                .MaximumLength(200).WithMessage("العنوان يجب أن لا يتجاوز 200 حرف.")
                .When(x => !string.IsNullOrEmpty(x.Title)); // افحص الطول فقط إذا أرسل المستخدم عنواناً جديداً

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("الوصف يجب أن لا يتجاوز 1000 حرف.")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
}