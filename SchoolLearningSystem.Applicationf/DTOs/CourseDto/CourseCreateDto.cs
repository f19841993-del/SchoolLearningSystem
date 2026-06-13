using System.ComponentModel.DataAnnotations;

namespace SchoolLearningSystem.Applicationf.DTOs.CourseDto
{
    public class CourseCreateDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; } = string.Empty;

        public string? Image { get; set; } // جعلناه nullable لأنه قد لا يرفع صورة فوراً

        [Required(ErrorMessage = "TeacherId is required")]
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "CurriculumId is required")]
        public int CurriculumId { get; set; }
    }
}