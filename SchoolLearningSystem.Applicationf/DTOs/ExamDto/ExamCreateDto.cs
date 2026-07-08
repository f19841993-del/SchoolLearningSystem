using System.ComponentModel.DataAnnotations;
using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.Applicationf.DTOs.ExamDto
{
    public class ExamCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public ExamType ExamType { get; set; }
        public DifficultyLevel Difficulty { get; set; }
        public int CourseId { get; set; }

        // اختياري: امتحان شامل للكورس (null) أو كويز خاص بدرس محدد
        public int? LessonId { get; set; }
    }
}