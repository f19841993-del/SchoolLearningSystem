using System.ComponentModel.DataAnnotations;

namespace SchoolLearningSystem.Applicationf.DTOs.StudentAnswer
{
    public class StudentAnswerDetailCreateDto
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public int QuestionId { get; set; }

        [Required]
        public string SelectedAnswer { get; set; } = string.Empty;

        [Required]
        public bool IsCorrect { get; set; }

        [Required]
        [Range(1, 3600, ErrorMessage = "Time must be between 1 second and 1 hour")]
        public int TimeTakenInSeconds { get; set; }
    }
}