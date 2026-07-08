using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using System;

namespace SchoolLearningSystem.Applicationf.Validators.CourseValidator
{
    public class CourseUpdateDtoValidator : AbstractValidator<CourseUpdateDto>
    {
        public CourseUpdateDtoValidator()
        {
            RuleFor(x => x.Title)
                .MaximumLength(200).WithMessage("العنوان يجب أن لا يتجاوز 200 حرف.")
                .When(x => !string.IsNullOrEmpty(x.Title));

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("الوصف يجب أن لا يتجاوز 1000 حرف.")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.Image)
                .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrEmpty(x.Image))
                .WithMessage("رابط صورة الكورس غير صالح.");

            // إضافة: TeacherId/CurriculumId موجودان فعلاً (nullable) هنا ويسمحان بنقل
            // الكورس لمعلم/منهج آخر — كانا بدون فحص إطلاقاً
            RuleFor(x => x.TeacherId)
                .GreaterThan(0).WithMessage("رقم المعلم غير صالح.")
                .When(x => x.TeacherId.HasValue);

            RuleFor(x => x.CurriculumId)
                .GreaterThan(0).WithMessage("رقم المنهج غير صالح.")
                .When(x => x.CurriculumId.HasValue);
        }
    }
}
