using System.ComponentModel.DataAnnotations;

namespace SchoolLearningSystem.Applicationf.DTOs.Teacher
{
    public class TeacherUpdateDto
    {
        [StringLength(100)]
        public string? Name { get; set; }

        public string? Subject { get; set; }

        [StringLength(500)]
        public string? Bio { get; set; }

        public string? ProfileImage { get; set; }
    }
}