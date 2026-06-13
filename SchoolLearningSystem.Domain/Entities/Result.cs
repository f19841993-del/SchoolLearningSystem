namespace SchoolLearningSystem.Domain.Entities
{
    public class Result : BaseEntity // وراثة BaseEntity لتوحيد الخصائص
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }

        public int? ExamId { get; set; }
        public Exam? Exam { get; set; }

        public string ResultType { get; set; } = "Homework";
        public double Score { get; set; }

        // 💡 إضافة جوهرية للذكاء الاصطناعي: الوقت المستغرق
        public int DurationInSeconds { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}