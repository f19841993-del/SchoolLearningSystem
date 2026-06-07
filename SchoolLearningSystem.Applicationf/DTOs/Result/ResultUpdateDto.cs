using System;

namespace SchoolLearningSystem.Applicationf.DTOs.Result
{
    public class ResultUpdateDto
    {
        public double Score { get; set; }
        // نوع النتيجة (Homework, Quiz, Midterm, Final)
        public string ResultType { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
}
