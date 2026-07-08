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

            // إضافة: Bio كان ناقصاً من الفحص
            RuleFor(x => x.Bio)
                .MaximumLength(500).WithMessage("النبذة طويلة جداً.");

            // Subject لا يُفحص عمداً - ثابتة على "Math" ولا تُرسل من الـ Client
        }
    }
}
