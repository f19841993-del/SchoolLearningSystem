using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using System;

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

            // إضافة: Image كان ناقصاً من الفحص
            RuleFor(x => x.Image)
                .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrEmpty(x.Image))
                .WithMessage("رابط صورة الكورس غير صالح.");
        }
    }
}
