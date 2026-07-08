using System.ComponentModel.DataAnnotations;
using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.Applicationf.DTOs.Curriculum
{
    public class CurriculumUpdateDto
    {
        public GradeLevel? GradeLevel { get; set; }

        [StringLength(150, MinimumLength = 3)]
        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}