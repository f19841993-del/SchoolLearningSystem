using System.ComponentModel.DataAnnotations;
using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.Applicationf.DTOs.Exercise
{
    public class ExerciseUpdateDto
    {
        [StringLength(500)]
        public string? Question { get; set; }

        public string? Answer { get; set; }

        public DifficultyLevel? Difficulty { get; set; }

        // نجعله Nullable لأنه في حال لم يرسل المستخدم رقماً، فهذا يعني أنه لا يريد تغيير الـ Lesson
        public int? LessonId { get; set; }
    }
}