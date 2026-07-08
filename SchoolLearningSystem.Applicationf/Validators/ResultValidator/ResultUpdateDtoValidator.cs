using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.Result;

namespace SchoolLearningSystem.Applicationf.Validators.ResultValidator
{
    public class ResultUpdateDtoValidator : AbstractValidator<ResultUpdateDto>
    {
        public ResultUpdateDtoValidator()
        {
            RuleFor(x => x.Score)
                .GreaterThanOrEqualTo(0).WithMessage("الدرجة لا يمكن أن تكون سالبة.")
                .When(x => x.Score.HasValue);

            RuleFor(x => x.ResultType)
                .NotEmpty().WithMessage("نوع النتيجة لا يمكن أن يكون فارغاً إذا أُرسل.")
                .When(x => x.ResultType != null);
        }
    }
}
