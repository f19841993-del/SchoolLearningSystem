using System.ComponentModel.DataAnnotations;
using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.Applicationf.DTOs.Curriculum
{
    public class CurriculumCreateDto
    {
        [Required(ErrorMessage = "GradeLevel is required")]
        public GradeLevel GradeLevel { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(150, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}