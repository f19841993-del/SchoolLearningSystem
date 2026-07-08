using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;

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
        }
    }
}
