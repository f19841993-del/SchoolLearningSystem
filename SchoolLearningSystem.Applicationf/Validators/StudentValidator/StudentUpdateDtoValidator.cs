using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.Student;

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
        }
    }
}
// ملاحظة: لا يوجد StudentCreateDtoValidator منفصل هنا عمداً — التسجيل يمر عبر
// /api/auth/register/student (StudentCreateDto + Password)، وهذا الحقل (Password)
// غير موجود بأي Entity/DTO لحد الآن (فجوة موثقة بـ api_contract.md قسم 0، أولوية 🔴 عالية).
// لازم يُحسم شكل DTO التسجيل الجديد أولاً قبل كتابة Validator له.
