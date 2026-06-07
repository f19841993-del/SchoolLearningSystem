using System;

namespace SchoolLearningSystem.Applicationf.DTOs.Result
{
    public class ResultReadDto
    {
        public int Id { get; set; }

        // الطالب المرتبط
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;

        // الدرس المرتبط
        public int LessonId { get; set; }
        public string LessonTitle { get; set; } = string.Empty;

        // الامتحان المرتبط (اختياري)
        public int? ExamId { get; set; }
        public string ExamTitle { get; set; } = string.Empty;

        // الدرجة
        public double Score { get; set; }

        // نوع النتيجة (Homework, Quiz, Midterm, Final)
        public string ResultType { get; set; } = string.Empty;

        // تاريخ التقييم
        public DateTime Date { get; set; }
    }
}
