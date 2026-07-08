using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.CourseStudent;

namespace SchoolLearningSystem.Applicationf.Validators.CourseStudentValidator
{
    public class CourseStudentCreateDtoValidator : AbstractValidator<CourseStudentCreateDto>
    {
        public CourseStudentCreateDtoValidator()
        {
            RuleFor(x => x.CourseId).GreaterThan(0).WithMessage("رقم الكورس غير صالح.");
            RuleFor(x => x.StudentId).GreaterThan(0).WithMessage("رقم الطالب غير صالح.");
        }
    }
}
