using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.Domain.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public DifficultyLevel DifficultyLevel { get; set; }

        public int ExamId { get; set; }
        public Exam Exam { get; set; }
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }
    }
}
