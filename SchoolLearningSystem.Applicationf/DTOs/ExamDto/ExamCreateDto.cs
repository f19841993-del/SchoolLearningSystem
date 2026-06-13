using System.ComponentModel.DataAnnotations;

namespace SchoolLearningSystem.Applicationf.DTOs.ExamDto
{
    public class ExamCreateDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "CourseId is required")]
        public int CourseId { get; set; }

        [Range(1, 300, ErrorMessage = "Duration must be between 1 and 300 minutes")]
        public int DurationInMinutes { get; set; }
        public int LessonId { get; set; } // أضف هذا الحقل

        public int PassingScore { get; set; } // الدرجة المطلوبة للنجاح

        public DateTime StartDate { get; set; }
    }
}