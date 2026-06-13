using System.ComponentModel.DataAnnotations;

namespace SchoolLearningSystem.Applicationf.DTOs.Teacher
{
    public class TeacherCreateDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Subject { get; set; } = "Math"; // افتراضي لأن النظام تعليم رياضيات

        [StringLength(500)]
        public string? Bio { get; set; }
    }
}