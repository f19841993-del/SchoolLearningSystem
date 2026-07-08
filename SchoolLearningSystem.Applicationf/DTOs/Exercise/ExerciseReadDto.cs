using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.Applicationf.DTOs.Exercise
{
    public class ExerciseReadDto
    {
        public int Id { get; set; }
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public DifficultyLevel Difficulty { get; set; }

        public int LessonId { get; set; }
        public string LessonTitle { get; set; } = string.Empty; // لجعل العرض أسرع في الفرونت
    }
}