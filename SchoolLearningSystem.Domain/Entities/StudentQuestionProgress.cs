namespace SchoolLearningSystem.Domain.Entities
{
    // حذفنا BaseEntity هنا
    public class StudentQuestionProgress
    {
        // مفاتيح الربط (سيتم تعريفها كمفتاح مركب في الـ AppDbContext)
        public int StudentId { get; set; }
        public int QuestionId { get; set; }

        // العلاقات
        public Student Student { get; set; } = null!;
        public Question Question { get; set; } = null!;

        // حقول SRS (تم وضع قيم ابتدائية منطقية للخوارزمية)
        public DateTime NextReviewDate { get; set; } = DateTime.UtcNow;
        public int RepetitionLevel { get; set; } = 0; // أول مرة يتعلم السؤال
        public double EaseFactor { get; set; } = 2.5; // القيمة المعيارية في خوارزمية SM-2

        // حقول التحليل
        public int TotalAttempts { get; set; } = 0;
        public int CorrectAttempts { get; set; } = 0;
        public DateTime LastReviewedAt { get; set; } = DateTime.UtcNow;
    }
}