using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.Question;

namespace SchoolLearningSystem.Applicationf.Validators.QuestionValidator
{
    public class QuestionCreateDtoValidator : AbstractValidator<QuestionCreateDto>
    {
        public QuestionCreateDtoValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("نص السؤال مطلوب.")
                .MaximumLength(1000).WithMessage("النص طويل جداً.");

            // إضافة: Answer كان مفقوداً بالكامل من الفحص رغم وجوده بالـ DTO الفعلي
            RuleFor(x => x.Answer)
                .NotEmpty().WithMessage("الإجابة الصحيحة مطلوبة.");

            RuleFor(x => x.DifficultyLevel)
                .IsInEnum().WithMessage("مستوى الصعوبة غير صالح.");

            // تصحيح: LessonId هو int عادي (إجباري دايماً بالـ DTO الفعلي) وليس nullable —
            // فحص .HasValue عليه كان يسبب CS1061. السؤال دايماً مرتبط بدرس عند الإنشاء.
            RuleFor(x => x.LessonId)
                .GreaterThan(0).WithMessage("رقم الدرس غير صالح.");

            // ExamId فعلاً int? بالـ DTO — اختياري بتصميم (سؤال بنك أسئلة عام أو ضمن امتحان)
            RuleFor(x => x.ExamId)
                .GreaterThan(0).WithMessage("رقم الامتحان غير صالح.")
                .When(x => x.ExamId.HasValue);
        }
    }
}
