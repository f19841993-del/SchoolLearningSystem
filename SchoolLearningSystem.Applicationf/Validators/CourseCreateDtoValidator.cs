using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;

namespace SchoolLearningSystem.Applicationf.Validators
{
    

    public class CourseCreateDtoValidator : AbstractValidator<CourseCreateDto>
    {
        public CourseCreateDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("عنوان الكورس مطلوب.")
                .MaximumLength(100).WithMessage("العنوان يجب أن لا يتجاوز 100 حرف.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("وصف الكورس لا يمكن أن يكون فارغاً.");
        }
    }
}
