using System.ComponentModel.DataAnnotations;

namespace SchoolLearningSystem.Applicationf.DTOs.CourseDto
{
    public class CourseUpdateDto
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }

        public int? TeacherId { get; set; }

        public int? CurriculumId { get; set; }

        // Order مستثنى عمداً - نفس منطق CourseCreateDto. لو تحتاج إعادة ترتيب
        // الكورسات يدوياً، الأفضل DTO مستقل ReorderCoursesDto بدل خلطه هنا.
    }
}