using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.StudentQuestionProgress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Validators.SrsValidator
{
    public class StudentQuestionProgressUpdateDtoValidator : AbstractValidator<StudentQuestionProgressUpdateDto>
    {
        public StudentQuestionProgressUpdateDtoValidator()
        {
            // 1. فحص المفاتيح
            RuleFor(x => x.StudentId)
                .GreaterThan(0).WithMessage("رقم الطالب غير صالح.");
            RuleFor(x => x.QuestionId)
                .GreaterThan(0).WithMessage("رقم السؤال غير صالح.");

            // 2. فحص بيانات خوارزمية (SM-2)
            RuleFor(x => x.RepetitionLevel)
                .GreaterThanOrEqualTo(0).WithMessage("مستوى التكرار لا يمكن أن يكون قيمة سالبة.");

            RuleFor(x => x.EaseFactor)
                .GreaterThanOrEqualTo(1.3).WithMessage("معامل السهولة (Ease Factor) لا يمكن أن يقل عن 1.3 حسب معايير خوارزمية SM-2.");

            RuleFor(x => x.Interval)
                .GreaterThanOrEqualTo(0).WithMessage("الفاصل الزمني (Interval) لا يمكن أن يكون قيمة سالبة.");

            // 3. فحص بيانات الإحصائيات (منطق رياضي)
            RuleFor(x => x.TotalAttempts)
                .GreaterThanOrEqualTo(0).WithMessage("إجمالي المحاولات لا يمكن أن يكون سالباً.");

            RuleFor(x => x.CorrectAttempts)
                .GreaterThanOrEqualTo(0).WithMessage("المحاولات الصحيحة لا يمكن أن تكون سالبة.")
                .LessThanOrEqualTo(x => x.TotalAttempts).WithMessage("عدد المحاولات الصحيحة لا يمكن أن يكون أكبر من إجمالي المحاولات!");

            // 4. فحص التواريخ
            RuleFor(x => x.LastReviewedAt)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("تاريخ آخر مراجعة لا يمكن أن يكون في المستقبل.");

            // ملاحظة: NextReviewDate يمكن أن يكون في الماضي أو المستقبل، لذلك لا نضع عليه قيوداً صارمة هنا، فهو يعتمد على التعديل الإداري.
        }
    }
}
