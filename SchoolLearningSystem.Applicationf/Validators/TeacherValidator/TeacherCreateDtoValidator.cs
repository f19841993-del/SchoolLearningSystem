using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.Teacher;

namespace SchoolLearningSystem.Applicationf.Validators.TeacherValidator
{
    public class TeacherCreateDtoValidator : AbstractValidator<TeacherCreateDto>
    {
        public TeacherCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("اسم المعلم مطلوب.")
                .MaximumLength(100).WithMessage("الاسم يجب ألا يتجاوز 100 حرف.");

            // Subject لا يُفحص هنا عمداً — ثابتة على "Math" ولا تُرسل من الـ Client
            // (dtos_review_report.md #4، رقم 1)
        }
    }
}
