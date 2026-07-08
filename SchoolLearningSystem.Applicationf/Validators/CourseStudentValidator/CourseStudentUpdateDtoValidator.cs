using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.CourseStudent;

namespace SchoolLearningSystem.Applicationf.Validators.CourseStudentValidator
{
    // ProgressPercentage و LastAccessedAt مستثناتان عمداً من هذا الفحص —
    // محسوبتان تلقائياً من نشاط الطالب الفعلي ويُفترض ألا تُرسلا من الـ Client أصلاً
    // (dtos_review_report.md #4.1، نفس مبدأ الحماية المطبّق على StudentQuestionProgress)
    public class CourseStudentUpdateDtoValidator : AbstractValidator<CourseStudentUpdateDto>
    {
        public CourseStudentUpdateDtoValidator()
        {
            RuleFor(x => x.IsActive)
                .NotNull().WithMessage("يجب تحديد حالة التفعيل (تسجيل نشط / ملغى).");
        }
    }
}
