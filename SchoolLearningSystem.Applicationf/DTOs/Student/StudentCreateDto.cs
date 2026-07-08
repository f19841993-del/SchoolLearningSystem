using System.ComponentModel.DataAnnotations;
using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.Applicationf.DTOs.Student
{
    public class StudentCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;
        public GradeLevel GradeLevel { get; set; }

        // Bio/Address/Education/ProfileImage مستثناة عمداً من Create -
        // بيانات تكميلية تُضاف بعد التسجيل عبر Update، نفس منطق Teacher.Subject
    }
}