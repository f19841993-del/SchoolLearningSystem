using System.ComponentModel.DataAnnotations;

namespace SchoolLearningSystem.Applicationf.DTOs.Lesson
{
    public class LessonUpdateDto
    {
        [StringLength(200, MinimumLength = 3)]
        public string? Title { get; set; }

        public string? Content { get; set; }

        public int? CourseId { get; set; }
    }
}