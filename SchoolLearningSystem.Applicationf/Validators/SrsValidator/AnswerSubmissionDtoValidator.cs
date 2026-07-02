using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.Srs;

namespace SchoolLearningSystem.Applicationf.Validators.SrsValidator
{
    public class AnswerSubmissionDtoValidator : AbstractValidator<AnswerSubmissionDto>
    {
        public AnswerSubmissionDtoValidator()
        {
            // التأكد من أن الأرقام التعريفية صحيحة
            RuleFor(x => x.StudentId)
                .GreaterThan(0).WithMessage("رقم الطالب غير صالح.");

            RuleFor(x => x.QuestionId)
                .GreaterThan(0).WithMessage("رقم السؤال غير صالح.");

            // 🛡️ الحماية الأهم: جودة الإجابة يجب أن تكون حصراً بين 0 و 5
            RuleFor(x => x.Quality)
                .InclusiveBetween(0, 5).WithMessage("تقييم جودة الإجابة يجب أن يكون بين 0 و 5 فقط.");

            // حماية للوقت المستغرق
            RuleFor(x => x.TimeTakenInSeconds)
                .GreaterThanOrEqualTo(0).WithMessage("الوقت المستغرق لا يمكن أن يكون بالسالب.")
                .LessThan(3600).WithMessage("الوقت المستغرق غير منطقي (أكثر من ساعة!).");
        }
    }
}