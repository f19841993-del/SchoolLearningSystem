using System.ComponentModel.DataAnnotations;

namespace SchoolLearningSystem.Applicationf.DTOs.Lesson
{
    public class LessonUpdateDto
    {
        [StringLength(200, MinimumLength = 3)]
        public string? Title { get; set; }

        public string? Content { get; set; }

        public string? VideoUrl { get; set; }

        public int? CourseId { get; set; }

        // Order و IsPublished مستثنيان عمداً - نفس منطق LessonCreateDto.
        // إذا احتجت إعادة ترتيب الدروس يدوياً (Drag & Drop بالفرونت)،
        // الأفضل معمارياً DTO مستقل: ReorderLessonsDto (List<{LessonId, NewOrder}>)
        // بدل خلطه هنا، حفاظاً على Single Responsibility.
    }
}