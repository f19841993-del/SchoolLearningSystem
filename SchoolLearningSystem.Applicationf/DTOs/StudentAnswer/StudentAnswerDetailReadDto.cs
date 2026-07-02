namespace SchoolLearningSystem.Applicationf.DTOs.StudentAnswer
{
    public class StudentAnswerDetailReadDto
    {
        public int Id { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string QuestionText { get; set; } = string.Empty;
        public string SelectedAnswer { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }

        // 🌟 إضافة حقل التقييم (Quality) للعرض
        public int Quality { get; set; }

        public int TimeTakenInSeconds { get; set; }
        public DateTime Timestamp { get; set; }
    }
}