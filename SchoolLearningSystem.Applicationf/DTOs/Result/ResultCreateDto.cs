using System;

namespace SchoolLearningSystem.Applicationf.DTOs.Result
{
    public class ResultCreateDto
    {
        public int StudentId { get; set; }
        public int LessonId { get; set; }
        public int? ExamId { get; set; }
        public double Score { get; set; }
        // نوع النتيجة (Homework, Quiz, Midterm, Final)
        public string ResultType { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
}
