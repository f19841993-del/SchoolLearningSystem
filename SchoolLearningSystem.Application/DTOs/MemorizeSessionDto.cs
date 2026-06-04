using System;

namespace SchoolLearningSystem.Application.DTOs
{
    public class MemorizeSessionDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int LessonId { get; set; }
        public int ExerciseId { get; set; }
        public int Attempts { get; set; }
        public double SuccessRate { get; set; }
        public DateTime Date { get; set; }
    }
}
