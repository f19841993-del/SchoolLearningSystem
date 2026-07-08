using System.ComponentModel.DataAnnotations;

namespace SchoolLearningSystem.Applicationf.DTOs.Teacher
{
    public class TeacherUpdateDto
    {
        [StringLength(100)]
        public string? Name { get; set; }

        [StringLength(500)]
        public string? Bio { get; set; }

        public string? ProfileImage { get; set; }

        // Subject مستثنى عمداً - نفس منطق TeacherCreateDto، القيمة ثابتة على "Math"
        // ولا تُعدَّل عبر API عام.
    }
}