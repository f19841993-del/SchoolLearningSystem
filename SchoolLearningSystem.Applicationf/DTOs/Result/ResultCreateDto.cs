using System.ComponentModel.DataAnnotations;

namespace SchoolLearningSystem.Applicationf.DTOs.Result
{
    public class ResultCreateDto
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public int LessonId { get; set; }

        public int? ExamId { get; set; }

        [Required]
        public string ResultType { get; set; } = "Homework";

        [Range(0.0, 100.0, ErrorMessage = "Score must be between 0 and 100")]
        public double Score { get; set; }
    }
}