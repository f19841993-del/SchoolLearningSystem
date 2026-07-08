using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.StudentAnswer;

namespace SchoolLearningSystem.Applicationf.Validators.StudentAnswerDetailValidator
{
    // لا يوجد Endpoint مباشر لإنشائه (api_contract.md قسم 12) — يتم حصراً داخل
    // SrsService.ProcessAnswerAsync. هذا الفحص احتياطي فقط.
    public class StudentAnswerDetailCreateDtoValidator : AbstractValidator<StudentAnswerDetailCreateDto>
    {
        public StudentAnswerDetailCreateDtoValidator()
        {
            RuleFor(x => x.StudentId).GreaterThan(0).WithMessage("رقم الطالب غير صالح.");
            RuleFor(x => x.QuestionId).GreaterThan(0).WithMessage("رقم السؤال غير صالح.");
            RuleFor(x => x.MemorizeSessionId).GreaterThan(0).WithMessage("رقم جلسة المراجعة غير صالح.");
            RuleFor(x => x.SelectedAnswer).NotEmpty().WithMessage("الإجابة المختارة مطلوبة.");
            RuleFor(x => x.Quality).InclusiveBetween(0, 5).WithMessage("جودة الإجابة يجب أن تكون بين 0 و5.");

            // إضافة: TimeTakenInSeconds كان ناقصاً من الفحص (موجود بالـ DTO الفعلي)
            RuleFor(x => x.TimeTakenInSeconds)
                .GreaterThanOrEqualTo(0).WithMessage("الوقت المستغرق لا يمكن أن يكون سالباً.")
                .LessThan(3600).WithMessage("الوقت المستغرق غير منطقي (أكثر من ساعة!).");

            // ⚠️ ملاحظة معمارية (لا تحتاج قاعدة فحص): IsCorrect يُرسل جاهزاً من الـ Client هنا
            // بدل أن يُحسب تلقائياً (IsCorrect = Quality >= 3) داخل الـ Service — يستحق مراجعة
            // معمارية منفصلة، مذكور فعلاً كتنبيه داخل تعليقات الـ DTO نفسه.
        }
    }
}
