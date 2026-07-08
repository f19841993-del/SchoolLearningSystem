using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using System;

namespace SchoolLearningSystem.Applicationf.Validators.LessonValidator
{
    public class LessonUpdateDtoValidator : AbstractValidator<LessonUpdateDto>
    {
        public LessonUpdateDtoValidator()
        {
            RuleFor(x => x.Title)
                .MaximumLength(150).WithMessage("العنوان يجب ألا يتجاوز 150 حرف.")
                .When(x => !string.IsNullOrEmpty(x.Title));

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("محتوى الدرس لا يمكن أن يكون فارغاً إذا أُرسل.")
                .When(x => x.Content != null);

            RuleFor(x => x.VideoUrl)
                .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrEmpty(x.VideoUrl))
                .WithMessage("رابط الفيديو غير صالح.");

            // إضافة: CourseId موجود فعلاً هنا (nullable) ويسمح بنقل الدرس بين كورسات —
            // بعكس Exam حيث استُبعد CourseId عمداً. تأكد هذا مقصود قبل الاعتماد عليه.
            RuleFor(x => x.CourseId)
                .GreaterThan(0).WithMessage("رقم الكورس غير صالح.")
                .When(x => x.CourseId.HasValue);
        }
    }
}
