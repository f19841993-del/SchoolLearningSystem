namespace SchoolLearningSystem.Applicationf.DTOs.Question
{
    public class QuestionUpdateDto
    {
        public string Text { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public string DifficultyLevel { get; set; } = string.Empty;
        public int QuestionNumber { get; set; }
    }
}
