using System.ComponentModel.DataAnnotations;
using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.Applicationf.DTOs.CurriculumDto
{
    public class CurriculumCreateDto
    {
        public GradeLevel GradeLevel { get; set; }
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}