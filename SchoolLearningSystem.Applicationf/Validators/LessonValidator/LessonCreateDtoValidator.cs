using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using System;

namespace SchoolLearningSystem.Applicationf.Validators.LessonValidator
{
    public class LessonCreateDtoValidator : AbstractValidator<LessonCreateDto>
    {
        public LessonCreateDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("عنوان الدرس مطلوب.")
                .MaximumLength(150).WithMessage("العنوان يجب ألا يتجاوز 150 حرف.");

            RuleFor(x => x.CourseId)
                .GreaterThan(0).WithMessage("رقم الكورس غير صالح.");

            RuleFor(x => x.VideoUrl)
                .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrEmpty(x.VideoUrl))
                .WithMessage("رابط الفيديو غير صالح.");

            // Order و IsPublished مستثنيان عمداً — يُديرهما الـ Service حصراً
            // (PublishLessonAsync ومنطق ترتيب تلقائي، dtos_review_report.md #3.4)
        }
    }
}
