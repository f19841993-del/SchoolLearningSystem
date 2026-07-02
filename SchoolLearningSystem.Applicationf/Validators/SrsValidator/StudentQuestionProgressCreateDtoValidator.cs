using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.StudentQuestionProgress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Validators.SrsValidator
{
    
        public class StudentQuestionProgressCreateDtoValidator : AbstractValidator<StudentQuestionProgressCreateDto>
        {
            public StudentQuestionProgressCreateDtoValidator()
            {
                RuleFor(x => x.StudentId)
                    .GreaterThan(0).WithMessage("رقم الطالب غير صالح، يجب أن يكون أكبر من صفر.");

                RuleFor(x => x.QuestionId)
                    .GreaterThan(0).WithMessage("رقم السؤال غير صالح، يجب أن يكون أكبر من صفر.");
            }
        }
    
}
