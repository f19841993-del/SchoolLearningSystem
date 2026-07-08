using System.ComponentModel.DataAnnotations;
using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.Applicationf.DTOs.Exam
{
    public class ExamUpdateDto
    {
        [StringLength(200, MinimumLength = 3)]
        public string? Title { get; set; }

        public ExamType? ExamType { get; set; }

        public DifficultyLevel? Difficulty { get; set; }

        public int? LessonId { get; set; }

        // CourseId مستثنى عمداً - نقل امتحان بين كورسات مختلفة تصرف نادر
        // وخطير (يكسر ربط النتائج/الأسئلة)؛ إذا لزم فعلاً، يُنفَّذ كـ
        // Use Case صريح منفصل بدل Update عام.
    }
}