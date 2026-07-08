using System.ComponentModel.DataAnnotations;

namespace SchoolLearningSystem.Applicationf.DTOs.Lesson
{
    public class LessonCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        // اختياري - مو كل درس فيه فيديو
        public string? VideoUrl { get; set; }
        public int CourseId { get; set; }

        // 💡 ملاحظة معمارية:
        // - Order: لا يُرسل من الـ Client، يُحسب تلقائياً بالـ Service (آخر ترتيب بالكورس + 1)
        // - IsPublished: يبدأ دائماً false، يتغير فقط عبر PublishLessonAsync (Use Case مستقل)
    }
}