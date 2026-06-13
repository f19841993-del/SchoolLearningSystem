using System.ComponentModel.DataAnnotations;

namespace SchoolLearningSystem.Applicationf.DTOs.Lesson
{
    public class LessonCreateDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; } = string.Empty;

        [Required(ErrorMessage = "CourseId is required")]
        public int CourseId { get; set; }
    }
}