using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.Question;

namespace SchoolLearningSystem.Applicationf.Validators.QuestionValidator
{
    public class QuestionUpdateDtoValidator : AbstractValidator<QuestionUpdateDto>
    {
        public QuestionUpdateDtoValidator()
        {
            RuleFor(x => x.Text)
                .MaximumLength(1000).WithMessage("النص طويل جداً.")
                .When(x => !string.IsNullOrEmpty(x.Text));

            // إضافة: Answer كان مفقوداً بالكامل من الفحص
            RuleFor(x => x.Answer)
                .NotEmpty().WithMessage("الإجابة لا يمكن أن تكون فارغة إذا أُرسلت.")
                .When(x => x.Answer != null);

            RuleFor(x => x.DifficultyLevel)
                .IsInEnum().WithMessage("مستوى الصعوبة غير صالح.")
                .When(x => x.DifficultyLevel.HasValue);

            // ملاحظة: لا يوجد LessonId/ExamId هنا لأنهما غير موجودين أصلاً بـ QuestionUpdateDto الفعلي — صحيح كما هو
        }
    }
}
