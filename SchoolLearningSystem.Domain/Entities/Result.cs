using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Domain.Entities
{
    public class Result
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }


        public int? ExamId { get; set; }   // ✅ جديد
        public Exam? Exam { get; set; }

        // ✅ جديد: نوع النتيجة (Homework, Quiz, Midterm, Final)
        public string ResultType { get; set; } = "Homework";
        public double Score { get; set; }
        public DateTime Date { get; set; }
    }
}
