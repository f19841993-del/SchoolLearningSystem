using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.Auth;

namespace SchoolLearningSystem.Applicationf.Validators.AuthValidator
{
    public class RegisterStudentDtoValidator : AbstractValidator<RegisterStudentDto>
    {
        public RegisterStudentDtoValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("اسم المستخدم مطلوب.")
                .MaximumLength(50).WithMessage("اسم المستخدم يجب ألا يتجاوز 50 حرف.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("البريد الإلكتروني مطلوب.")
                .EmailAddress().WithMessage("صيغة البريد الإلكتروني غير صحيحة.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("كلمة المرور مطلوبة.")
                .MinimumLength(6).WithMessage("كلمة المرور يجب أن تكون 6 أحرف على الأقل.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("اسم الطالب مطلوب.")
                .MaximumLength(100).WithMessage("الاسم يجب ألا يتجاوز 100 حرف.");

            RuleFor(x => x.GradeLevel)
                .IsInEnum().WithMessage("المرحلة الدراسية غير صحيحة.");
        }
    }
}