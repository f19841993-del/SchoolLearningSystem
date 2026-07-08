namespace SchoolLearningSystem.Applicationf.DTOs.StudentAnswer
{
    // 💡 لم يكن موجوداً سابقاً بمشروعك - أُضيف الآن لإكمال الثلاثية القياسية
    // (Create/Read/Update) بما إن الـ Entity الآن مؤكد بالكامل.
    public class StudentAnswerDetailReadDto
    {
        public int Id { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string QuestionText { get; set; } = string.Empty;
        public int MemorizeSessionId { get; set; }
        public string SelectedAnswer { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public int Quality { get; set; }
        public int TimeTakenInSeconds { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}