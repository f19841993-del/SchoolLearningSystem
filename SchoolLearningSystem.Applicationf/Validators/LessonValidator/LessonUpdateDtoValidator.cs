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

            RuleFor(x => x.VideoUrl)
                .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrEmpty(x.VideoUrl))
                .WithMessage("رابط الفيديو غير صالح.");
        }
    }
}
