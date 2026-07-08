using System.ComponentModel.DataAnnotations;

namespace SchoolLearningSystem.Applicationf.DTOs.CourseDto
{
    public class CourseCreateDto
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string? Image { get; set; }

        public int TeacherId { get; set; }

        public int CurriculumId { get; set; }

        // 💡 Order مستثنى عمداً - نفس منطق Lesson.Order، يُحسب تلقائياً
        // بالـ Service (آخر ترتيب بالمنهج + 1) بدل أن يُرسل من الـ Client.
    }
}