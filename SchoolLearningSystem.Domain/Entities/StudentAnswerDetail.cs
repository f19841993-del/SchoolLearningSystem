namespace SchoolLearningSystem.Domain.Entities
{
    public class StudentAnswerDetail : BaseEntity
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public string SelectedAnswer { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }

        // الخصائص الذهبية للذكاء الاصطناعي
        public int TimeTakenInSeconds { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}