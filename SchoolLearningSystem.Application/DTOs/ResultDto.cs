using System;

namespace SchoolLearningSystem.Application.DTOs
{
    public class ResultDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int LessonId { get; set; }
        public double Score { get; set; }
        public DateTime Date { get; set; }
    }
}
