using System.ComponentModel.DataAnnotations;

namespace SchoolLearningSystem.Applicationf.DTOs.ExerciseDto
{
    public class ExerciseUpdateDto
    {
        [StringLength(200)]
        public string? Title { get; set; }

        [StringLength(500)]
        public string? Question { get; set; }

        public string? Answer { get; set; }

        // نجعله Nullable لأنه في حال لم يرسل المستخدم رقماً، فهذا يعني أنه لا يريد تغيير الـ Lesson
        public int? LessonId { get; set; }
    }
}