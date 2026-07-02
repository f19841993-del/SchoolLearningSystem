namespace SchoolLearningSystem.Applicationf.DTOs.StudentQuestionProgress
{
    public class StudentQuestionProgressReadDto
    {
        public int StudentId { get; set; }
        public int QuestionId { get; set; }
        public string QuestionText { get; set; } = string.Empty; // لجلب نص السؤال مباشرة

        // بيانات الـ SRS
        public DateTime NextReviewDate { get; set; }
        public int RepetitionLevel { get; set; }
        public double EaseFactor { get; set; }
        public int Interval { get; set; }

        // بيانات التحليل
        public int TotalAttempts { get; set; }
        public int CorrectAttempts { get; set; }
        public DateTime LastReviewedAt { get; set; }
    }
}