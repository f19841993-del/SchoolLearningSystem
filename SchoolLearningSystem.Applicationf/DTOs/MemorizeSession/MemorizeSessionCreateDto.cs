using System.ComponentModel.DataAnnotations;

namespace SchoolLearningSystem.Applicationf.DTOs.MemorizeSession
{
    public class MemorizeSessionCreateDto
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public int LessonId { get; set; }

        public int? ExerciseId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Attempts must be at least 1")]
        public int Attempts { get; set; }

        [Range(0.0, 100.0, ErrorMessage = "Success rate must be between 0 and 100")]
        public double SuccessRate { get; set; }
    }
}