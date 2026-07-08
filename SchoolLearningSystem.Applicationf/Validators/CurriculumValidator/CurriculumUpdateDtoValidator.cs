using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.Curriculum;

namespace SchoolLearningSystem.Applicationf.Validators.CurriculumValidator
{
    public class CurriculumUpdateDtoValidator : AbstractValidator<CurriculumUpdateDto>
    {
        public CurriculumUpdateDtoValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(150).WithMessage("الاسم يجب ألا يتجاوز 150 حرف.")
                .When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x.GradeLevel)
                .IsInEnum().WithMessage("الصف الدراسي غير صالح.")
                .When(x => x.GradeLevel.HasValue);

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("الوصف يجب ألا يتجاوز 500 حرف.")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
}
