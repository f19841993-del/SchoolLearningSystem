using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.Auth;

namespace SchoolLearningSystem.Applicationf.Validators.AuthValidator
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.UsernameOrEmail)
                .NotEmpty().WithMessage("اسم المستخدم أو البريد الإلكتروني مطلوب.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("كلمة المرور مطلوبة.");
        }
    }
}