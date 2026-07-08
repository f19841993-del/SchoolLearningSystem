using System.ComponentModel.DataAnnotations;
using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.Applicationf.DTOs.Exam
{
    public class ExamCreateDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public ExamType ExamType { get; set; }

        [Required]
        public DifficultyLevel Difficulty { get; set; }

        [Required(ErrorMessage = "CourseId is required")]
        public int CourseId { get; set; }

        // اختياري: امتحان شامل للكورس (null) أو كويز خاص بدرس محدد
        public int? LessonId { get; set; }
    }
}