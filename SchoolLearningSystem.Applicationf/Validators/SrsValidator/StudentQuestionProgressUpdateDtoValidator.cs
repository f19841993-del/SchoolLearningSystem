using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.StudentQuestionProgress;
using System;

namespace SchoolLearningSystem.Applicationf.Validators.SrsValidator
{
    public class StudentQuestionProgressUpdateDtoValidator : AbstractValidator<StudentQuestionProgressUpdateDto>
    {
        public StudentQuestionProgressUpdateDtoValidator()
        {
            // StudentId/QuestionId حُذفا من هذا الفحص عمداً — أصبحا Route Parameters فقط (dtos_review_report.md #3.3)

            RuleFor(x => x.RepetitionLevel)
                .GreaterThanOrEqualTo(0).WithMessage("مستوى التكرار لا يمكن أن يكون قيمة سالبة.")
                .When(x => x.RepetitionLevel.HasValue);

            RuleFor(x => x.EaseFactor)
                .GreaterThanOrEqualTo(1.3).WithMessage("معامل السهولة (Ease Factor) لا يمكن أن يقل عن 1.3 حسب معايير خوارزمية SM-2.")
                .When(x => x.EaseFactor.HasValue);

            RuleFor(x => x.Interval)
                .GreaterThanOrEqualTo(0).WithMessage("الفاصل الزمني (Interval) لا يمكن أن يكون قيمة سالبة.")
                .When(x => x.Interval.HasValue);

            RuleFor(x => x.TotalAttempts)
                .GreaterThanOrEqualTo(0).WithMessage("إجمالي المحاولات لا يمكن أن يكون سالباً.")
                .When(x => x.TotalAttempts.HasValue);

            RuleFor(x => x.CorrectAttempts)
                .GreaterThanOrEqualTo(0).WithMessage("المحاولات الصحيحة لا يمكن أن تكون سالبة.")
                .When(x => x.CorrectAttempts.HasValue);

            RuleFor(x => x)
                .Must(x => !x.CorrectAttempts.HasValue || !x.TotalAttempts.HasValue
                           || x.CorrectAttempts <= x.TotalAttempts)
                .WithMessage("عدد المحاولات الصحيحة لا يمكن أن يكون أكبر من إجمالي المحاولات!")
                .OverridePropertyName(nameof(StudentQuestionProgressUpdateDto.CorrectAttempts));

            RuleFor(x => x.LastReviewedAt)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("تاريخ آخر مراجعة لا يمكن أن يكون في المستقبل.")
                .When(x => x.LastReviewedAt.HasValue);

            // NextReviewDate: بلا قيود صارمة عمداً — تعديل إداري استثنائي فقط (راجع #3.3)
        }
    }
}
