using System.ComponentModel.DataAnnotations;
using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.Applicationf.DTOs.Question
{
    public class QuestionCreateDto
    {
        [Required(ErrorMessage = "Question text is required")]
        public string Text { get; set; } = string.Empty;

        [Required(ErrorMessage = "Answer is required")]
        public string Answer { get; set; } = string.Empty;

        [Required]
        public DifficultyLevel DifficultyLevel { get; set; }

        [Required]
        public int LessonId { get; set; }

        public int? ExamId { get; set; } // اختياري، يمكن أن يكون السؤال في بنك الأسئلة دون امتحان
    }
}