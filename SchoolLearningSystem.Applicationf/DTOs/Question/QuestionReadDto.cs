using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.Applicationf.DTOs.Question
{
    public class QuestionReadDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public DifficultyLevel DifficultyLevel { get; set; }
        public int LessonId { get; set; }
        public string LessonTitle { get; set; } = string.Empty;
    }
}