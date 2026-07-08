using System.ComponentModel.DataAnnotations;
using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.Applicationf.DTOs.CurriculumDto
{
    public class CurriculumUpdateDto
    {
        public GradeLevel? GradeLevel { get; set; }
        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}