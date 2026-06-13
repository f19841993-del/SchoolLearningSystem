using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.Applicationf.DTOs.Student
{
    public class StudentReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ProfileImage { get; set; } = string.Empty;
        public GradeLevel GradeLevel { get; set; }
    }
}