using System.ComponentModel.DataAnnotations;
using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.Applicationf.DTOs.Question
{
    public class QuestionCreateDto
    {
        public string Text { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public DifficultyLevel DifficultyLevel { get; set; }
        public int LessonId { get; set; }

        public int? ExamId { get; set; } // اختياري، يمكن أن يكون السؤال في بنك الأسئلة دون امتحان
    }
}