using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.Student;
using System;

namespace SchoolLearningSystem.Applicationf.Validators.StudentValidator
{
    public class StudentUpdateDtoValidator : AbstractValidator<StudentUpdateDto>
    {
        public StudentUpdateDtoValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage("الاسم يجب ألا يتجاوز 100 حرف.")
                .When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x.Phone)
                .Matches(@"^\+?[0-9]{7,15}$").WithMessage("رقم الهاتف غير صالح.")
                .When(x => !string.IsNullOrEmpty(x.Phone));

            RuleFor(x => x.Address)
                .MaximumLength(250).WithMessage("العنوان طويل جداً.")
                .When(x => !string.IsNullOrEmpty(x.Address));

            RuleFor(x => x.Bio)
                .MaximumLength(500).WithMessage("النبذة طويلة جداً.")
                .When(x => !string.IsNullOrEmpty(x.Bio));

            RuleFor(x => x.Education)
                .MaximumLength(150).WithMessage("الحقل طويل جداً.")
                .When(x => !string.IsNullOrEmpty(x.Education));

            // إضافة: ProfileImage كان ناقصاً بالكامل من الفحص
            RuleFor(x => x.ProfileImage)
                .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrEmpty(x.ProfileImage))
                .WithMessage("رابط الصورة الشخصية غير صالح.");
        }
    }
}
// ملاحظة: لا يوجد StudentCreateDtoValidator عمداً — التسجيل يمر عبر /api/auth/register/student
// وشكل الـ DTO المرتبط بالـ Password لسه غير محسوم (فجوة موثقة بـ api_contract.md).
