using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.Curriculum;

namespace SchoolLearningSystem.Applicationf.Validators.CurriculumValidator
{
    public class CurriculumCreateDtoValidator : AbstractValidator<CurriculumCreateDto>
    {
        public CurriculumCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("اسم المنهج مطلوب.")
                .MaximumLength(150).WithMessage("الاسم يجب ألا يتجاوز 150 حرف.");

            RuleFor(x => x.GradeLevel)
                .IsInEnum().WithMessage("الصف الدراسي غير صالح.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("الوصف يجب ألا يتجاوز 500 حرف.");
        }
    }
}
