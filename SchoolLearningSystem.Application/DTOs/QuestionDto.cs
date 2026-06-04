namespace SchoolLearningSystem.Application.DTOs
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public string DifficultyLevel { get; set; } = string.Empty;
        public int ExamId { get; set; }
    }
}
