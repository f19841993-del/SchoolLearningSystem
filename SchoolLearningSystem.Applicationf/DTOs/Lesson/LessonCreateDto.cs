using System.ComponentModel.DataAnnotations;

namespace SchoolLearningSystem.Applicationf.DTOs.Lesson
{
    public class LessonCreateDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; } = string.Empty;

        // اختياري - مو كل درس فيه فيديو
        public string? VideoUrl { get; set; }

        [Required(ErrorMessage = "CourseId is required")]
        public int CourseId { get; set; }

        // 💡 ملاحظة معمارية:
        // - Order: لا يُرسل من الـ Client، يُحسب تلقائياً بالـ Service (آخر ترتيب بالكورس + 1)
        // - IsPublished: يبدأ دائماً false، يتغير فقط عبر PublishLessonAsync (Use Case مستقل)
    }
}