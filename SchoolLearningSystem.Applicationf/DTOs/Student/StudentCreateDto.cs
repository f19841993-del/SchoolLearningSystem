using System.ComponentModel.DataAnnotations;
using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.Applicationf.DTOs.Student
{
    public class StudentCreateDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        [Required]
        public GradeLevel GradeLevel { get; set; }
    }
}