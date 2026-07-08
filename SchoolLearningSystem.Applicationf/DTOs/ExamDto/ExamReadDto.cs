using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.Applicationf.DTOs.ExamDto
{
    public class ExamReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public ExamType ExamType { get; set; }
        public DifficultyLevel Difficulty { get; set; }

        public int CourseId { get; set; }
        public string CourseTitle { get; set; } = string.Empty;

        public int? LessonId { get; set; }
        public string? LessonTitle { get; set; }

        public int QuestionsCount { get; set; }
    }
}