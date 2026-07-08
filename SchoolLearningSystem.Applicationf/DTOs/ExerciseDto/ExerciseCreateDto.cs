using System.ComponentModel.DataAnnotations;
using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.Applicationf.DTOs.ExerciseDto
{
    public class ExerciseCreateDto
    {
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public int LessonId { get; set; }
        public DifficultyLevel Difficulty { get; set; }
    }
}