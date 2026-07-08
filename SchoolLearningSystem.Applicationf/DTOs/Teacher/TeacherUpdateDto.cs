using System.ComponentModel.DataAnnotations;

namespace SchoolLearningSystem.Applicationf.DTOs.Teacher
{
    public class TeacherUpdateDto
    {
        public string? Name { get; set; }
        public string? Bio { get; set; }

        public string? ProfileImage { get; set; }

        // Subject مستثنى عمداً - نفس منطق TeacherCreateDto، القيمة ثابتة على "Math"
        // ولا تُعدَّل عبر API عام.
    }
}