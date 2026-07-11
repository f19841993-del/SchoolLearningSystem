using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.Srs;

namespace SchoolLearningSystem.Applicationf.Validators.SrsValidator
{
    public class AnswerSubmissionDtoValidator : AbstractValidator<AnswerSubmissionDto>
    {
        public AnswerSubmissionDtoValidator()
        {
            RuleFor(x => x.StudentId)
                .GreaterThan(0).WithMessage("رقم الطالب غير صالح.");

            RuleFor(x => x.QuestionId)
                .GreaterThan(0).WithMessage("رقم السؤال غير صالح.");

            // إضافة: MemorizeSessionId FK إجباري (dtos_review_report.md #3.5) وكان مفقوداً من الفحص الأصلي
            RuleFor(x => x.MemorizeSessionId)
                .GreaterThan(0).WithMessage("رقم جلسة المراجعة غير صالح — كل إجابة يجب أن تنتمي لجلسة فعلية.");

            RuleFor(x => x.Quality)
                .InclusiveBetween(0, 5).WithMessage("تقييم جودة الإجابة يجب أن يكون بين 0 و 5 فقط.");

            // TimeTakenInSeconds فعلاً int? بالـ DTO الحقيقي (اختياري) — FluentValidation
            // يتجاوز الفحص تلقائياً لو null، لكن .When موضّح هنا للقراءة فقط
            RuleFor(x => x.TimeTakenInSeconds)
                .GreaterThanOrEqualTo(0).WithMessage("الوقت المستغرق لا يمكن أن يكون بالسالب.")
                .LessThan(3600).WithMessage("الوقت المستغرق غير منطقي (أكثر من ساعة!).")
                .When(x => x.TimeTakenInSeconds.HasValue);
            RuleFor(x => x.SelectedAnswer)
             .NotEmpty().WithMessage("يجب اختيار إجابة قبل الإرسال.");
        }
    }
}
