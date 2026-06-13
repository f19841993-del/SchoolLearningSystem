using System.ComponentModel.DataAnnotations;

namespace SchoolLearningSystem.Applicationf.DTOs.CurriculumDto
{
    public class CurriculumUpdateDto
    {
        [StringLength(150, ErrorMessage = "Title cannot exceed 150 characters")]
        public string? Title { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        public string? Level { get; set; }
    }
}