using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.Teacher;
using System;

namespace SchoolLearningSystem.Applicationf.Validators.TeacherValidator
{
    public class TeacherUpdateDtoValidator : AbstractValidator<TeacherUpdateDto>
    {
        public TeacherUpdateDtoValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage("الاسم يجب ألا يتجاوز 100 حرف.")
                .When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x.Bio)
                .MaximumLength(500).WithMessage("النبذة طويلة جداً.")
                .When(x => !string.IsNullOrEmpty(x.Bio));

            // إضافة: ProfileImage كان ناقصاً من الفحص
            RuleFor(x => x.ProfileImage)
                .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrEmpty(x.ProfileImage))
                .WithMessage("رابط الصورة الشخصية غير صالح.");
        }
    }
}
