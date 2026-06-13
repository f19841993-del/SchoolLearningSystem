using System.ComponentModel.DataAnnotations;

namespace SchoolLearningSystem.Applicationf.DTOs.ExerciseDto
{
    public class ExerciseCreateDto
    {
        [Required]
        [StringLength(500)]
        public string QuestionText { get; set; } = string.Empty;

        [Required]
        public string CorrectAnswer { get; set; } = string.Empty;

        [Required]
        public int LessonId { get; set; }

        [Range(1, 5, ErrorMessage = "Difficulty level must be between 1 and 5")]
        public int DifficultyLevel { get; set; } = 1; // 1 = سهل، 5 = صعب
    }
}