using System.ComponentModel.DataAnnotations;

namespace SchoolLearningSystem.Applicationf.DTOs.CourseDto
{
    public class CourseUpdateDto
    {
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string? Title { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string? Description { get; set; }

        public string? Image { get; set; }

        public int? TeacherId { get; set; }

        public int? CurriculumId { get; set; }
    }
}