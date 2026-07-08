using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.Applicationf.DTOs.Student
{
    public class StudentReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;       // ✅ إضافة - كان ناقصاً
        public string? Bio { get; set; }
        public string? Address { get; set; }
        public string? Education { get; set; }                  // ✅ إضافة - كان ناقصاً
        public string ProfileImage { get; set; } = string.Empty;
        public GradeLevel GradeLevel { get; set; }
    }
}