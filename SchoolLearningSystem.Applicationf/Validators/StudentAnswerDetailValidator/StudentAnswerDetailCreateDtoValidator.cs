using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.StudentAnswer;

namespace SchoolLearningSystem.Applicationf.Validators.StudentAnswerDetailValidator
{
    // لا يوجد Endpoint مباشر لإنشاء StudentAnswerDetail (api_contract.md قسم 12) —
    // الإنشاء يتم حصراً داخل SrsService.ProcessAnswerAsync بعد التحقق من AnswerSubmissionDto.
    // هذا الـ Validator احتياطي فقط (لو استُدعي الـ Service من مصدر آخر مستقبلاً، أو للاختبارات).
    public class StudentAnswerDetailCreateDtoValidator : AbstractValidator<StudentAnswerDetailCreateDto>
    {
        public StudentAnswerDetailCreateDtoValidator()
        {
            RuleFor(x => x.StudentId).GreaterThan(0).WithMessage("رقم الطالب غير صالح.");
            RuleFor(x => x.QuestionId).GreaterThan(0).WithMessage("رقم السؤال غير صالح.");
            RuleFor(x => x.MemorizeSessionId).GreaterThan(0).WithMessage("رقم جلسة المراجعة غير صالح.");
            RuleFor(x => x.Quality).InclusiveBetween(0, 5).WithMessage("جودة الإجابة يجب أن تكون بين 0 و5.");
            RuleFor(x => x.SelectedAnswer).NotEmpty().WithMessage("الإجابة المختارة مطلوبة.");
        }
    }
}
