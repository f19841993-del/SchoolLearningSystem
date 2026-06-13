using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.Applicationf.DTOs.Question
{
    public class QuestionUpdateDto
    {
        public string? Text { get; set; }
        public string? Answer { get; set; }
        public DifficultyLevel? DifficultyLevel { get; set; }
    }
}